using Eshop.Domain;
using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Products
{
    public class UpdateProduct
    {
        public record Command(int Id, UpdateProductRequestDto Request) : IRequest<UpdateProductResponseDto>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
                RuleFor(x => x.Request.Title).NotEmpty();
                RuleFor(x => x.Request.Description).NotEmpty();
                RuleFor(x => x.Request.Price).GreaterThanOrEqualTo(0);
                RuleFor(x => x.Request.CategoryId).Must(x => x is null || x > 0);
            }
        }

        public class Handler : IRequestHandler<Command, UpdateProductResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<UpdateProductResponseDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var product = await dbContext.Products.FindAsync(command.Id, cancellationToken);
                if (product is null)
                {
                    throw new NotFoundException($"Product not found - Id: {command.Id}");
                }

                var request = command.Request;
                Category? category = null;
                if (request.CategoryId is not null)
                {
                    category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
                    if (category is null)
                    {
                        throw new NotFoundException($"Category not found - Id: {request.CategoryId}");
                    }
                }

                product.Update(request.Title, request.Description, request.Price, category);

                await dbContext.SaveChangesAsync(cancellationToken);

                return UpdateProductResponseDto.Map(product);
            }
        }
    }

    public class UpdateProductRequestDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
    }

    public class UpdateProductResponseDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public UpdateProductCategoryDto? Category { get; set; }

        public class UpdateProductCategoryDto
        {
            public int Id { get; set; }
            public required string Name { get; set; }
        }

        internal static UpdateProductResponseDto Map(Product result)
        {
            return new UpdateProductResponseDto
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Price = result.Price,
                Category = result.Category != null ? new UpdateProductCategoryDto
                {
                    Id = result.Category.Id,
                    Name = result.Category.Name
                } : null
            };
        }
    }
}
