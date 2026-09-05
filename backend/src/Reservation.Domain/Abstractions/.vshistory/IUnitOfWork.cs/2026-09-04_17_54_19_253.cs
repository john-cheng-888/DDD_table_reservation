using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Abstractions {
    public interface IUnitOfWork {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
