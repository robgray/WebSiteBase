using System;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Infrastructure.Repositories.NHibernate.Mappings.Conventions
{
    public class PropertyAccessConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            var entityType = instance.EntityType;
            var camelCaseUnderscoreName =
                ConvertToCamelCaseUnderscore(instance.Name);

            var hasBackingField = HasField(entityType, camelCaseUnderscoreName);

            // Default is to use property setter, so only modify mapping
            // if there is a backing field

            if (hasBackingField)
                instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
        }

        private static string ConvertToCamelCaseUnderscore(string propertyName)
        {
            return "_" +
                propertyName[0].ToString().ToLower() +
                propertyName.Substring(1);
        }

        private bool HasField(Type type, string fieldName)
        {
            FieldInfo backingField = type.GetField(
                fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            return backingField != null;
        }
    }
}
