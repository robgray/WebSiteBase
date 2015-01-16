using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Infrastructure.Repositories.NHibernate.Mappings.Conventions
{
    public class ReferenceIdNameConvention : IReferenceConvention
    {        
        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(instance.Property.Name + "Id");
        }
    }
}
