using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFS.Domain.Repositories
{
    public partial interface IBaseRepository<T> : IDisposable
    {
        Task<List<T>> GetAllAsync(CancellationToken ct = default(CancellationToken));

        Task<T> GetByIdAsync(CancellationToken ct = default(CancellationToken));

        Task<T> AddAsync(T newEntity, CancellationToken ct = default(CancellationToken));

        Task<bool> UpdateAsync(T entity, CancellationToken ct = default(CancellationToken));

        Task<bool> DeleteAsync(string id, CancellationToken ct = default(CancellationToken));
    }
}
