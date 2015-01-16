using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzioFit.Model;
using FluentNHibernate.Mapping;

namespace FizzioFit.Repositories.Mappings
{
    public class ExerciseDefinitionAlternativeMap : ClassMap<ExerciseDefinitionAlternative>
    {
        public ExerciseDefinitionAlternativeMap()
        {
            Table("ExerciseDefinition");

            Id(x => x.Id);            
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.ImageUrl);

            //HasManyToMany(x => x.Equipment)
            //    .Access.CamelCaseField(Prefix.Underscore)
            //    .Cascade.All()
            //    .Inverse()
            //    .ParentKeyColumn("ExerciseDefinitionId")
            //    .ChildKeyColumn("EquipmentId")
            //    .Table("ExerciseDefinitionEquipment");
        }
    }
}
