using FluentValidation;

namespace WebBase.Mvc.ViewModels.Validators
{
    public class LoginViewViewModelValidator : AbstractValidator<LoginViewViewModel>
    {
        public LoginViewViewModelValidator()
        {
            RuleFor(l => l.UserName)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(l => l.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}