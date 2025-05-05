using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using FluentValidation;
using MediatR;

namespace Eshop.WebApi.Features.Orders
{
    public class DeleteOrder
    {
        public record Command(int Id) : IRequest<Unit>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var order = await dbContext.Orders.FindAsync(command.Id, cancellationToken);
                if (order == null)
                {
                    throw new NotFoundException($"Order not found - ID: {command.Id}");
                }

                dbContext.Orders.Remove(order);
                await dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
