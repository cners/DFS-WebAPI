using DFS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFS.Domain.Repositories
{
   public interface IDeveloperRepository:IDisposable
    {
        Task<List<Developer>> GetAllAsync(CancellationToken ct = default(CancellationToken));

        Task<Developer> GetByIdAsync(CancellationToken ct = default(CancellationToken));

        Task<Developer> AddAsync(Developer newDeveloper, CancellationToken ct = default(CancellationToken));

        Task<bool> UpdateAsync(Developer developer, CancellationToken ct = default(CancellationToken));

        Task<bool> DeleteAsync(string devID, CancellationToken ct = default(CancellationToken));
    }
}
