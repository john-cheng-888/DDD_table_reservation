using Reservation.Domain.Common;

namespace Reservation.Domain.Reservations;
public sealed class PartySize : ValueObject
{
    public const int Min = 1;
    public const int Max = 12;
    public int Value { get; }
    private PartySize(int value) => Value = value;
    public static PartySize Of(int value)
    {
        if (value < Min || value > Max)
        {
            throw new DomainException($"訂位人數須介於{Min}至{Max}人,收到{value}.");
        }
        return new PartySize(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}