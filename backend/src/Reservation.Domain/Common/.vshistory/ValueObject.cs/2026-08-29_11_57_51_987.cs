namespace Reservation.Domain.Common;
public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();
    public override bool Equals(object? obj)
    {
        if(obj is null || obj.GetType()!=GetType()) return false;
        return GetEqualityComponents().SequenceEqual(
            ((ValueObject)obj).GetEqualityComponents()
        );

        //return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        //return base.GetHashCode();
        GetEqualityComponents().Aggregate(0,(h,v)=>HashCode.Combine(h,v));
    }
    public static bool operator == (ValueObject? a,ValueObject? b)=>Equals(a,b);
    public static bool operator !=(ValueObject? a,ValueObject? b)=>!Equals(a,b);
}
    

