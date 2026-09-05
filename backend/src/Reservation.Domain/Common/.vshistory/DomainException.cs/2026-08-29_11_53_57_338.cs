namespace Reservation.Domain.Common;
public class DomainExcption : Exception
{
    public DomainExcption(string message):base(message)
    {
        
    }
    public DomainExcption(string message,Exception baseExp) : base(message, baseExp)
    {
        
    }
}