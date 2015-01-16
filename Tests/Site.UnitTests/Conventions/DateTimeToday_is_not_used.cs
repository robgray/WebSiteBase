using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Mvc.Tests.Conventions
{
    public class DateTimeToday_is_not_used : ConventionTest
    {
        protected override ConventionData SetUp()
        {
            return new ConventionData
            {
                Types = FizzioFitTypes,
                Must = NotHaveAnyUsagesOfDateTimeDotNow,
                FailDescription =
                    "Classes must not make use of System.DateTime.Today.\n\tThe following methods use System.DateTime.Today. (Please use DateTimeHelper.UtcNow instead)\n",
                FailItemDescription = ItemDescription
            };
        }

        private string ItemDescription(Type type)
        {
            return string.Join("\n\t", GetDateTimeNowUsages(type));
        }

        private bool NotHaveAnyUsagesOfDateTimeDotNow(Type type)
        {
            return !GetDateTimeNowUsages(type).Any();
        }

        private IEnumerable<string> GetDateTimeNowUsages(Type type)
        {
            var assembly = GetAssemblyDefinition(type);

            var q = from typedef in assembly.MainModule.Types.Where(t => t.Name == type.Name)
                    from method in typedef.Methods.Where(m => m.HasBody)
                    from ins in method.Body.Instructions.Where(i => i != null && i.OpCode.Name == "call")
                    let methodRef = ins.Operand as MethodReference
                    where
                        methodRef != null && methodRef.FullName == "System.DateTime System.DateTime::get_Today()"
                    select method.FullName;

            return q.ToArray();
        }

        private AssemblyDefinition _assembly;
        private AssemblyDefinition GetAssemblyDefinition(Type type)
        {
            var targetAssemblyName = type.Assembly.FullName;

            if (_assembly == null || _assembly.Name.ToString() != targetAssemblyName)
                _assembly = AssemblyDefinition.ReadAssembly(type.Assembly.GetModules().First().Name);

            return _assembly;
        }

        private static bool FizzioFitTypes(Type type)
        {
            return type.Namespace != null && type.Namespace.StartsWith("FizzioFit.") && !type.Name.Contains("DateTimeHelper");
        }
    }
}
