using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Infrastructure.Repositories.NHibernate.Mappings.Conventions
{
    public class ManyToManyIdNameConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "Id");
            instance.Relationship.Column(instance.Relationship.StringIdentifierForModel + "Id");
        }
    }
}
