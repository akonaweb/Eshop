using Eshop.Domain;
using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Orders
{
    public class UpdateOrder
    {
        public record Command(int Id, UpdateOrderRequestDto Request) : IRequest<UpdateOrderResponseDto>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
                RuleFor(x => x.Request.Customer).NotEmpty();
                RuleFor(x => x.Request.Address).NotEmpty();
                RuleForEach(x => x.Request.Items).ChildRules(x =>
                {
                    x.RuleFor(i => i.ProductId).GreaterThan(0);
                    x.RuleFor(i => i.Quantity).GreaterThan(0);
                });
                RuleFor(x => x.Request.Items)
                    .NotEmpty()
                    .Must(items => items.Select(i => i.ProductId).Distinct().Count() == items.Count())
                    .WithMessage("Items must have unique ProductId.");
            }
        }

        public class Handler : IRequestHandler<Command, UpdateOrderResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<UpdateOrderResponseDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var order = await dbContext.Orders.Include(x => x.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(x => x.Id == command.Id);
                if (order is null)
                {
                    throw new NotFoundException($"Order not found - Id: {command.Id}");
                }

                var request = command.Request;
                var orderItems = await GetOrderItems(request, cancellationToken);
                order.Update(request.Customer, request.Address, orderItems);

                var result = dbContext.Orders.Update(order);
                await dbContext.SaveChangesAsync(cancellationToken);

                return UpdateOrderResponseDto.Map(result.Entity);
            }

            private async Task<List<OrderItem>> GetOrderItems(UpdateOrderRequestDto request, CancellationToken cancellationToken)
            {
                var orderItems = new List<OrderItem>();
                foreach (var item in request.Items)
                {
                    var product = await GetProduct(item, cancellationToken);
                    var orderItem = new OrderItem(item.Quantity, product.Price, product);
                    orderItems.Add(orderItem);
                }

                return orderItems;
            }

            private async Task<Product> GetProduct(UpdateOrderRequestDto.UpdateOrderItemRequestDto item, CancellationToken cancellationToken)
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId, cancellationToken);
                if (product is null)
                {
                    throw new NotFoundException($"Product not found during creation of the order - Product Id: {item.ProductId}");
                }

                return product;
            }
        }
    }

    public class UpdateOrderRequestDto
    {
        public required string Customer { get; set; }
        public required string Address { get; set; }
        public required IEnumerable<UpdateOrderItemRequestDto> Items { get; set; }

        public class UpdateOrderItemRequestDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }

    public class UpdateOrderResponseDto
    {
        public int Id { get; set; }
        public required string Customer { get; set; }
        public required string Address { get; set; }
        public required IEnumerable<UpdateOrderItemResponseDto> Items { get; set; }
        public decimal TotalPrice { get; set; }

        public class UpdateOrderItemResponseDto
        {
            public int ProductId { get; set; }
            public required string ProductTitle { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal TotalPrice { get; set; }
        }

        internal static UpdateOrderResponseDto Map(Order result)
        {
            return new UpdateOrderResponseDto
            {
                Id = result.Id,
                Customer = result.Customer,
                Address = result.Address,
                Items = result.Items.Select(x =>
                {
                    return new UpdateOrderItemResponseDto
                    {
                        ProductId = x.Product.Id,
                        ProductTitle = x.Product.Title,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        TotalPrice = x.TotalPrice
                    };
                }).ToList(),
                TotalPrice = result.TotalPrice
            };
        }
    }
}