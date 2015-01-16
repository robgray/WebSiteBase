using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using FluentNHibernate.Mapping;

namespace Infrastructure.Repositories.NHibernate.Mappings
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(x => x.Id).Column("RoleId").Not.Nullable();
            Map(x => x.Name).Column("RoleName").Not.Nullable();
            Map(x => x.IsAdministrator);

            HasManyToMany(x => x.Users)
                .Cascade.AllDeleteOrphan()
                .Table("UserRole");
        }
    }
}
