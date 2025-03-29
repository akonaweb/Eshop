using Eshop.Domain;
using Eshop.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Orders
{
    public class GetOrders
    {
        public record Query() : IRequest<IEnumerable<GetOrdersResponseDto>>;

        public class Handler : IRequestHandler<Query, IEnumerable<GetOrdersResponseDto>>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<IEnumerable<GetOrdersResponseDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                var orders = await dbContext.OrdersViews.ToListAsync(cancellationToken);
                return orders.Select(GetOrdersResponseDto.Map).ToList();
            }
        }
    }

    public class GetOrdersResponseDto
    {
        public int Id { get; set; }
        public required string Customer { get; set; }
        public required string Address { get; set; }
        public DateTime CreatedAt { get; set; }

        internal static GetOrdersResponseDto Map(Order result)
        {
            return new GetOrdersResponseDto
            {
                Id = result.Id,
                Customer = result.Customer,
                Address = result.Address,
                CreatedAt = result.CreatedAt
            };
        }
    }
}