namespace DailyLife.Domain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        protected AggregateRoot(Id id) : base(id)
        {
        }

        private List<IDomainEvent> _events = new();

        public IReadOnlyCollection<IDomainEvent> Events => _events.ToList();
        public void ClearDomainEvents() => _events.Clear();
        public void RaiseDomainEvent(IDomainEvent domainEvent)
            => _events.Add(domainEvent);
    }
}
