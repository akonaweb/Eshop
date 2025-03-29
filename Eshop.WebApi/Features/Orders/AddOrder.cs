using Eshop.Domain;
using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Orders
{
    public class AddOrder
    {
        public record Command(AddOrderRequestDto Request) : IRequest<AddOrderResponseDto>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
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

        public class Hanlder : IRequestHandler<Command, AddOrderResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Hanlder(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<AddOrderResponseDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var request = command.Request;
                var order = new Order(0, request.Customer, request.Address);

                foreach (var item in request.Items)
                {
                    var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId, cancellationToken);
                    if (product is null)
                    {
                        throw new NotFoundException($"Product not found during creation of the order - ID: {item.ProductId}");
                    }

                    var orderItem = new OrderItem(item.Quantity, product.Price, product);
                    order.AddItem(orderItem);
                }

                var result = await dbContext.Orders.AddAsync(order, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                return AddOrderResponseDto.Map(result.Entity);
            }
        }
    }
    public class AddOrderRequestDto
    {
        public required string Customer { get; set; }
        public required string Address { get; set; }
        public required IEnumerable<AddOrderItemRequestDto> Items { get; set; }

        public class AddOrderItemRequestDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }

    public class AddOrderResponseDto
    {
        public int Id { get; set; }
        public required string Customer { get; set; }
        public required string Address { get; set; }
        public required IEnumerable<AddOrderItemResponseDto> Items { get; set; }

        public class AddOrderItemResponseDto
        {
            public int ProductId { get; set; }
            public required string ProductTitle { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        internal static AddOrderResponseDto Map(Order result)
        {
            return new AddOrderResponseDto
            {
                Id = result.Id,
                Customer = result.Customer,
                Address = result.Address,
                Items = result.Items.Select(x =>
                {
                    return new AddOrderItemResponseDto
                    {
                        ProductId = x.Product.Id,
                        ProductTitle = x.Product.Title,
                        Quantity = x.Quantity,
                        Price = x.Price
                    };
                }).ToList()
            };
        }
    }
}