using FluentValidation;

namespace Oikono.Application.Authentication.Commands.Refresh;

internal class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.TokenToRefresh)
            .NotEmpty();
    }
}