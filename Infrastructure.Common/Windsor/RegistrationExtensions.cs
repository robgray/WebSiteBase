using System.Web;
using Castle.MicroKernel.Registration;

namespace Infrastructure.Common.Windsor
{
    public static class RegistrationExtensions
    {
	    public static BasedOnDescriptor LifestylePerWebRequestOrScoped(this BasedOnDescriptor descriptor)
	    {
		    return HttpContext.Current != null
			           ? descriptor.LifestylePerWebRequest()
			           : descriptor.LifestyleScoped();
	    }

	    public static ComponentRegistration<TService> LifestylePerWebRequestOrScoped<TService>(this ComponentRegistration<TService> registration) where TService : class
	    {
		    return HttpContext.Current != null
			           ? registration.LifestylePerWebRequest()
			           : registration.LifestyleScoped();
	    }
    }
}
