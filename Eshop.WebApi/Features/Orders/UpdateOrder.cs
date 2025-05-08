using Eshop.Domain;
using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using MediatR;
using FluentValidation;

namespace Eshop.WebApi.Features.Orders
{
    public class UpdateOrder
    {
        public record Command(int Id, string Customer, string Address) : IRequest<UpdateOrdersResponseDto>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
                RuleFor(x => x.Customer).NotEmpty().Length(1, 50);
                RuleFor(x => x.Address).NotEmpty().Length(1, 500);
            }
        }

        public class Handler : IRequestHandler<Command, UpdateOrdersResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<UpdateOrdersResponseDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var order = await dbContext.Orders.FindAsync(new object[] { command.Id }, cancellationToken);
                if (order == null)
                {
                    throw new NotFoundException($"Order with ID {command.Id} not found.");
                }

                order.Update(command.Customer, command.Address);

                await dbContext.SaveChangesAsync(cancellationToken);

                return UpdateOrdersResponseDto.Map(order);
            }
        }
    }

    public class UpdateOrdersResponseDto
    {
        public int Id { get; set; }
        public required string Customer { get; set; }
        public required string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        internal static UpdateOrdersResponseDto Map(Order result)
        {
            return new UpdateOrdersResponseDto
            {
                Id = result.Id,
                Customer = result.Customer,
                Address = result.Address,
                CreatedAt = result.CreatedAt
            };
        }
    }

    public class UpdateOrdersRequestDto
    {
        public required string Customer { get; set; }
        public required string Address { get; set; }
    }
}