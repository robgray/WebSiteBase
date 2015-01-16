using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Infrastructure.Repositories.NHibernate.Mappings.Conventions
{
    public class OneToManyIdNameConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {            
            //instance.Key.PropertyRef(instance.EntityType.Name + "Id");
            instance.Key.Column(instance.EntityType.Name + "Id");
        }
    }
}
