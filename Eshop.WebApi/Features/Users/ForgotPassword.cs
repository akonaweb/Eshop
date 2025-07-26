using Eshop.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Eshop.WebApi.Features.Users
{
    public class ForgotPassword
    {
        public record Command(ForgotPasswordRequest Request) : IRequest<IResult>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Request.Email).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, IResult>
        {
            private readonly UserManager<ApplicationUser> userManager;

            public Handler(UserManager<ApplicationUser> userManager)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            }

            public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var model = request.Request;
                var message = "Reset password link sent.";
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Results.Ok(message);

                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                //var resetUrl = $"{configuration["FrontendUrl"]}/reset-password?token={WebUtility.UrlEncode(token)}&email={user.Email}";
                //await emailSender.SendEmailAsync(user.Email!, "Reset Password", $"Click <a href='{resetUrl}'>here</a> to reset your password.");

                return Results.Ok(message);
            }
        }
    }
}