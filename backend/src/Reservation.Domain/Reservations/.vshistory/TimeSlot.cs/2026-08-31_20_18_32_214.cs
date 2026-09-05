using Reservation.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Reservation.Domain.Reservations {
    public sealed class TimeSlot :ValueObject{
        private static readonly TimeOnly OpenAt = new TimeOnly(11, 0);
        private static readonly TimeOnly CloseAt = new TimeOnly(21, 0);
        public DateTimeOffset StartAt { get; }
        public TimeSpan Duration { get; }
        public DateTimeOffset EndAt => StartAt + Duration;

        private TimeSlot(DateTimeOffset startAt, TimeSpan duration) {
            StartAt = startAt;
            Duration = duration;
        }
        public static TimeSlot Of(DateTimeOffset startAt, TimeSpan duration) {
            if (duration <= TimeSpan.Zero || duration > TimeSpan.FromHours(3)) {
                throw new DomainException("用餐時長必須大於 0 且不超過 3 小時。");
            }
            var start = TimeOnly.FromDateTime(startAt.LocalDateTime);
            var end =   TimeOnly.FromDateTime((startAt + duration).LocalDateTime);
            if (start<OpenAt||end>CloseAt) {
                throw new DomainException($"訂位時段必須介於營業時間: {OpenAt:HH:\\:mm}-{CloseAt:HH:\\:mm}");
            }
            return new TimeSlot(startAt, duration);
        }
        public bool Overlaps(TimeSlot other) =>
            (StartAt < other.EndAt) && (other.StartAt < EndAt);

        protected override IEnumerable<object?> GetEqualityComponents() {
            yield return StartAt;
            yield return Duration;
        }
    }
}
