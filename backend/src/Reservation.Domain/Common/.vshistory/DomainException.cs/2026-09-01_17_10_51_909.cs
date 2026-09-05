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
public sealed class InvalidateInputExcption : DomainException {
    public InvalidateInputExcption(string msg) : base(msg) { 

    }
}
public sealed class BusinessRuleViolationException : DomainException {
    public BusinessRuleViolationException(string msg) : base(msg) { 

    }
}