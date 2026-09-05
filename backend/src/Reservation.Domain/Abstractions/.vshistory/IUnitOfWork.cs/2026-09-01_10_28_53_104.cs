using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Domain.Abstractions {
    public interface IUnitOfWork {
        Task<int> SaveChangesAsyn(CancellationToken ct = default);
    }
}
