namespace Reservation.Domain.Common;
public class DomainException : Exception
{
    public DomainException(string message):base(message)
    {
        
    }
    public DomainException(string message,Exception baseExp) : base(message, baseExp)
    {
        
    }
}