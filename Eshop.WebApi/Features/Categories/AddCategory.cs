using Eshop.Domain;
using Eshop.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Eshop.WebApi.Features.Categories
{
    public class AddCategory
    {
        public record Command(AddCategoryRequestDto Request) : IRequest<AddCategoryResponseDto>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Request.Name).NotEmpty();
            }
        }

        public class Hanlder : IRequestHandler<Command, AddCategoryResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Hanlder(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<AddCategoryResponseDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var request = command.Request;
                var category = new Category(0, request.Name);
                var result = await dbContext.Categories.AddAsync(category, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);

                return AddCategoryResponseDto.Map(result);
            }
        }
    }

    public class AddCategoryRequestDto
    {
        public required string Name { get; set; }
    }

    public class AddCategoryResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        internal static AddCategoryResponseDto Map(EntityEntry<Category> result)
        {
            return new AddCategoryResponseDto
            {
                Id = result.Entity.Id,
                Name = result.Entity.Name,
            };
        }
    }
}