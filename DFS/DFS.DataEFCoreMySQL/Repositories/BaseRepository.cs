using DFS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFS.DataEFCoreMySQL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        public readonly DfsContext _context;

        public BaseRepository(DfsContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T newDeveloper, CancellationToken ct = default(CancellationToken))
        {
            _context.Set<T>().Add(newDeveloper);
            await _context.SaveChangesAsync(ct);
            return newDeveloper;
        }

       

        public Task<bool> DeleteAsync(string devID, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<List<T>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            return await _context.Set<T>().ToListAsync(ct);
        }

        public Task<T> GetByIdAsync(CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T entity, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
