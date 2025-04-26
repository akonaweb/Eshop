using Eshop.Domain;
using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Orders
{
    public class GetOrder
    {
        public record Query(int Id) : IRequest<GetOrderResponseDto>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, GetOrderResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<GetOrderResponseDto> Handle(Query query, CancellationToken cancellationToken)
            {
                var order = await dbContext.OrdersViews
                    .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
                if (order is null)
                {
                    throw new NotFoundException($"Order not found - ID: {query.Id}");
                }

                return GetOrderResponseDto.Map(order);
            }
        }
    }

    public class GetOrderResponseDto
    {
        public int Id { get; set; }
        public required string Customer { get; set; }
        public required string Address { get; set; }
        public required IEnumerable<GetOrderItemResponseDto> Items { get; set; }
        public decimal TotalPrice { get; set; }

        public class GetOrderItemResponseDto
        {
            public int ProductId { get; set; }
            public required string ProductTitle { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        internal static GetOrderResponseDto Map(Order result)
        {
            return new GetOrderResponseDto
            {
                Id = result.Id,
                Customer = result.Customer,
                Address = result.Address,
                Items = result.Items.Select(x =>
                {
                    return new GetOrderItemResponseDto
                    {
                        ProductId = x.Product.Id,
                        ProductTitle = x.Product.Title,
                        Quantity = x.Quantity,
                        Price = x.Price
                    };
                }).ToList(),
                TotalPrice = result.TotalPrice
            };
        }
    }
}