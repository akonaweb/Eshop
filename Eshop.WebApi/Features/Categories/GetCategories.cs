using Eshop.Domain;
using Eshop.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Categories
{
    public class GetCategories
    {
        public record Query() : IRequest<IEnumerable<GetCategoriesResponseDto>>;

        public class Handler : IRequestHandler<Query, IEnumerable<GetCategoriesResponseDto>>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<IEnumerable<GetCategoriesResponseDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                var categories = await dbContext.CategoriesViews
                    .ToListAsync(cancellationToken);

                return categories.Select(GetCategoriesResponseDto.Map).ToList();
            }
        }
    }

    public class GetCategoriesResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        internal static GetCategoriesResponseDto Map(Category result)
        {
            return new GetCategoriesResponseDto
            {
                Id = result.Id,
                Name = result.Name
            };
        }
    }
}