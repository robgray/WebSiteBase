using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Castle.Core.Internal;
using FizzioFit.Mvc.Api.Dtos;
using FizzioFit.Mvc.Api;

namespace FizzioFit.Mvc.Tests.Conventions
{
    public class Dtos_Have_only_automatic_properties : ConventionTest
    {
        protected override ConventionData SetUp()
        {
            return new ConventionData
            {
                Types = t => IsServiceContract(t) == false && IsSecurityContract(t) == false && IsEnum(t) == false
                    && IsApiController(t) == false && IsController(t) == false,
                Must = OnlyHaveAutomaticProperties,
                FailDescription = "Dto types should only have automatic properties, since all they are here for is to transfer data.\r\nThe following types have some unexpected members. Please refactor them.",
                FailItemDescription = t => BuildTypeDescription(t)

            }.FromAssembly(typeof(ExerciseDto).Assembly)
                       .WithApprovedExceptions("Some exceptions are allowed, for example ToString() so that DTO is easier to work with in the debugger or can be directly bound to on a readonly screen.");
        }

        private string BuildTypeDescription(Type type)
        {
            var builder = new StringBuilder();
            builder.AppendLine(type.ToString());
            foreach (var member in GetInvalidMembers(type))
            {
                builder.AppendLine("\t\t" + member);
            }
            return builder.ToString();
        }

        private bool OnlyHaveAutomaticProperties(Type obj)
        {
            var invalidMembers = GetInvalidMembers(obj);
            return invalidMembers.Length == 0;
        }

        private MemberInfo[] GetInvalidMembers(Type obj)
        {
            var members = obj.GetMembers(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            var properties = members.OfType<PropertyInfo>().ToArray();
            var accessors = properties.SelectMany(p => p.GetAccessors()).ToArray();
            var invalidMembers = members.Where(m => IsDefaultConstructor(m) == false &&
                                                    IsAutomaticProperty(m) == false &&
                                                    IsPropertyAccessor(accessors, m) == false &&
                                                    IsAutomaticPropertyBackingField(m) == false)
                .ToArray();
            return invalidMembers;
        }

        private bool IsAutomaticPropertyBackingField(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType != MemberTypes.Field)
            {
                return false;
            }
            var field = (FieldInfo)memberInfo;
            return field.Name.EndsWith(">k__BackingField");
        }

        private bool IsPropertyAccessor(MethodInfo[] accessors, MemberInfo memberInfo)
        {
            return accessors.Contains(memberInfo);
        }

        private bool IsAutomaticProperty(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType != MemberTypes.Property)
            {
                return false;
            }
            var property = (PropertyInfo)memberInfo;
            var accessors = property.GetAccessors();
            return accessors.Length == 2 && accessors.All(a => a.IsPublic);
        }

        private bool IsDefaultConstructor(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType != MemberTypes.Constructor)
            {
                return false;
            }
            var ctor = (ConstructorInfo)memberInfo;
            return ctor.GetParameters().Length == 0;
        }

        private bool IsSecurityContract(Type type)
        {
            return type.Name == "ModuleActions";
        }

        private bool IsEnum(Type type)
        {
            return type.BaseType == typeof(Enum);
        }

        private bool IsApiController(Type type)
        {
            return type.BaseType == typeof (ApiControllerBase);
        }

        private bool IsController(Type type)
        {
            return type.BaseType == typeof(ControllerBase);
        }

        private bool IsServiceContract(Type type)
        {
            //return type.HasAttribute<ServiceContractAttribute>();
            return false;
        }
    }
}
