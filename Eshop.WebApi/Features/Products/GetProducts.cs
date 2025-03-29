using Eshop.Domain;
using Eshop.Persistence;
using Eshop.WebApi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Products
{
    public class GetProducts
    {
        public record Query() : IRequest<IEnumerable<GetProductsResponseDto>>;

        public class Handler : IRequestHandler<Query, IEnumerable<GetProductsResponseDto>>
        {
            private readonly EshopDbContext dbContext;
            private readonly IUserContext userContext;

            public Handler(EshopDbContext dbContext, IUserContext userContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                this.userContext = userContext;
            }

            public async Task<IEnumerable<GetProductsResponseDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                var userId = userContext.GetUserId();
                var products = await dbContext.ProductsViews
                    .Include(x => x.Category)
                    .ToListAsync(cancellationToken);

                return products.Select(GetProductsResponseDto.Map).ToList();
            }
        }
    }

    public class GetProductsResponseDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public GetProductsCategoryDto? Category { get; set; }

        public class GetProductsCategoryDto
        {
            public int Id { get; set; }
            public required string Name { get; set; }
        }

        internal static GetProductsResponseDto Map(Product result)
        {
            return new GetProductsResponseDto
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Price = result.Price,
                Category = result.Category != null ? new GetProductsCategoryDto
                {
                    Id = result.Category.Id,
                    Name = result.Category.Name
                } : null
            };
        }
    }
}
