using Domain.Entities;

namespace UnitTests.Model.Conventions
{
    public class Entity_classes_are_in_the_entities_namespace : ConventionTest
    {
        protected override ConventionData SetUp()
        {
            return new ConventionData
                       {
                           Types = t => t.IsConcrete<IHasId>(),
                           Must = t => t.FullName.Contains("Domain.Entities.")
                       };
        }
    }
}
