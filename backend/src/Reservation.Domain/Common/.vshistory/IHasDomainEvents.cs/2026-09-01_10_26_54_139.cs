using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Common {
    public interface IHasDomainEvents {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
