using Domain.Entities;
using FluentNHibernate.Mapping;

namespace Infrastructure.Repositories.NHibernate.Mappings
{
	public class UserMap : ClassMap<User>
	{
		public UserMap()
		{
            Not.LazyLoad();
			Id(x => x.Id).Column("UserId");
		    Map(x => x.Username).Not.Nullable();
		    Map(x => x.EmailAddress).Column("EmailAddress").Not.Nullable();
			Map(x => x.SecurityQuestion);
			Map(x => x.SecurityAnswer);
			Map(x => x.DateJoined).Nullable();			
			Map(x => x.ProfileImageUrl);			
		    Map(x => x.IPAddress, "IPAddres");		    
		    Map(x => x.ConfirmationToken);
		    Map(x => x.IsConfirmed);
		    Map(x => x.LastPasswordFailureDate);
		    Map(x => x.PasswordFailuresSinceLastSuccess).Not.Nullable();
		    Map(x => x.Password).Not.Nullable();
		    Map(x => x.PasswordChangedDate);
		    Map(x => x.PasswordSalt).Not.Nullable();
		    Map(x => x.PasswordVerificationToken);
		    Map(x => x.PasswordVerificationTokenExpirationDate);

		    HasManyToMany(x => x.Roles)
		        .Cascade.AllDeleteOrphan()
		        .Table("UserRole");
		}
	}
}
