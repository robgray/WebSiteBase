using FluentValidation;

namespace WebBase.Mvc.ViewModels.Validators
{
    public class LostPasswordViewModelValidator : AbstractValidator<LostPasswordViewModel>
    {
        public LostPasswordViewModelValidator()
        {
            RuleFor(e => e.EmailAddress).EmailAddress();
        }
    }
}