using System;
using System.Linq;
using Castle.MicroKernel;
using Castle.Windsor;

namespace Mvc.Tests.Installers
{
	public abstract class InstallerTests
	{
		protected IHandler[] GetAllHandlers(IWindsorContainer container)
		{
			return GetHandlersFor(typeof(object), container);
		}

		protected IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
		{
			return container.Kernel.GetAssignableHandlers(type);
		}

		protected Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
		{
			var handlers = GetHandlersFor(type, container);

			return handlers
				.Select(h => h.ComponentModel.Implementation)
				.OrderBy(t => t.Name)
				.ToArray();
		}

		protected Type[] GetPublicClassesFromApplicationAssembly<T>(Predicate<Type> where)
		{
			return typeof(T).Assembly.GetExportedTypes()
				.Where(t => t.IsClass)
				.Where(t => t.IsAbstract == false)
				.Where(where.Invoke)
				.OrderBy(t => t.Name)
				.ToArray();
		}
	}
}
