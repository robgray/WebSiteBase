using FluentValidation;

namespace WebBase.Mvc.ViewModels.Validators
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(reg => reg.NewPassword)
                //.NotEmpty().WithMessage("Password is required")
                .Length(6, 100).WithMessage("Password must be at least 6 characters").NotNull();

            RuleFor(reg => reg.ConfirmPassword)
                //.NotEmpty()
                .Equal(reg => reg.NewPassword).WithMessage("The password and confirmation password do not match");    
        }
    }
}