using System.Linq;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

namespace Infrastructure.Common.Windsor
{
    public class LifestyleScopeInterceptor : IInterceptor
    {
        private IKernel Kernel { get; set; }

        public LifestyleScopeInterceptor(IKernel kernel)
        {
            Kernel = kernel;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.CustomAttributes.Any(t => t.AttributeType == typeof(LifestyleScopeAttribute)))
            {                
                using (Kernel.BeginScope())
                {
                    invocation.Proceed();
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
