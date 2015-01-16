using FluentValidation.Validators;

namespace Infrastructure.Common.Validators
{
    // An example custom validator for FlientValidation.
    // eg RuleFor(x => x.ProviderNumber).ProviderNumber();
    // Once the extension method is hooked up.  See Extensions.
    public class ProviderNumberValidator : RegularExpressionValidator
    {
        public ProviderNumberValidator() : base("^[0-9]{6}[0-Z][A-Z]$") { }        
    }        
}
