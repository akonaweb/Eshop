using Eshop.Persistence;
using Eshop.WebApi.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Eshop.WebApi.Features.Users
{
    public class ChangePassword
    {
        public record Command(ChangePasswordRequest Request) : IRequest<IResult>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Request.NewPassword).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, IResult>
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly IUserContext userContext;

            public Handler(UserManager<ApplicationUser> userManager, IUserContext userContext)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            }

            public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var model = request.Request;
                var userId = userContext.GetUserId();
                var user = await userManager.FindByIdAsync(userId.ToString());

                if (user == null)
                    return Results.NotFound("User not found.");

                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                    return Results.BadRequest(result.Errors);

                return Results.Ok("Password changed successfully");
            }
        }
    }
}
