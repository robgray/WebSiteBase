using System;
using System.Reflection;
using Castle.Facilities.TypedFactory;

namespace WindowsService.Tasks
{
    public interface TaskDefinitionFactory
    {
        ITaskDefinition[] GetAll();
    }

    public class TaskSelector : DefaultTypedFactoryComponentSelector
    {
        protected override Type GetComponentType(MethodInfo method, object[] arguments)
        {
            return typeof (ITaskDefinition).MakeArrayType();
        }
    }
}
