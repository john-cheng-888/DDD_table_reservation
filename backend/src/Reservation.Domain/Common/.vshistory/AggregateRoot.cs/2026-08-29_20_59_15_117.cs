namespace Reservation.Domain.Common;
public interface IDomainEvent
{
    DateTimeOffset OccuredAt {get;}
}
public abstract class AggregateRoot<TId>
{
    private readonly List<IDomainEvent> _domainEvents=[];
    public TId Id {get;protected set;} =default!;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void Raise(IDomainEvent e)=>_domainEvents.Add(e);
    public void ClearDomainEvents()=>_domainEvents.Clear();
}