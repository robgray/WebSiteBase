using System;
using Castle.Windsor;
using FluentValidation;

namespace WebBase.Mvc.Plumbing
{
    public class WindsorFluentValidatorFactory : ValidatorFactoryBase
    {
        private readonly IWindsorContainer _container;

        public WindsorFluentValidatorFactory(IWindsorContainer 
            container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {            
            return (_container.Kernel.HasComponent(validatorType)
                        ? _container.Resolve(validatorType)
                        : null) as IValidator;            
        }
    }
}