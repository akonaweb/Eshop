using Eshop.Domain;
using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Categories
{
    public class GetCategory
    {
        public record Query(int Id) : IRequest<GetCategoryResponseDto>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, GetCategoryResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<GetCategoryResponseDto> Handle(Query query, CancellationToken cancellationToken)
            {
                var category = await dbContext.CategoriesViews
                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
                if (category is null)
                {
                    throw new NotFoundException($"Category not found - ID: {query.Id}");
                }

                return GetCategoryResponseDto.Map(category);
            }
        }
    }

    public class GetCategoryResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        internal static GetCategoryResponseDto Map(Category result)
        {
            return new GetCategoryResponseDto
            {
                Id = result.Id,
                Name = result.Name
            };
        }
    }
}