using FluentValidation;

namespace Infrastructure.Common.Validators
{
    public static class Extensions
    {
        // An extension of a custom validator for FlientValidation.
        // Allows the following: RuleFor(x => x.ProviderNumber).ProviderNumber();        
        public static IRuleBuilderOptions<T, string> ProviderNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new ProviderNumberValidator());
        }
    }
}
