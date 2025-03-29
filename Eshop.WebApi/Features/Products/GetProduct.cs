using Eshop.Domain;
using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Products
{
    public class GetProduct
    {
        public record Query(int Id) : IRequest<GetProductResponseDto>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, GetProductResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<GetProductResponseDto> Handle(Query query, CancellationToken cancellationToken)
            {
                var product = await dbContext.ProductsViews
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
                if (product is null)
                {
                    throw new NotFoundException($"Product not found - Id: {query.Id}");
                }

                return GetProductResponseDto.Map(product);
            }
        }
    }

    public class GetProductResponseDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public GetProductCategoryDto? Category { get; set; }

        public class GetProductCategoryDto
        {
            public int Id { get; set; }
            public required string Name { get; set; }
        }

        internal static GetProductResponseDto Map(Product result)
        {
            return new GetProductResponseDto
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Price = result.Price,
                Category = result.Category != null ? new GetProductCategoryDto
                {
                    Id = result.Category.Id,
                    Name = result.Category.Name
                } : null
            };
        }
    }
}
