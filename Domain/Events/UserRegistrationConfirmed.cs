using Domain.Entities;

namespace Domain.Events
{
    public class UserRegistrationConfirmed : IDomainEvent
    {
        public User User { get; set; }
    }
}
