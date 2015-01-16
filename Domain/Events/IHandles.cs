namespace Domain.Events
{
    public interface IHandles<T> where T : class, IDomainEvent
    {
        void Handle(T domainEvent);
    }
}
