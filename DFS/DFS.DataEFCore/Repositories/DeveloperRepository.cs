using DFS.Domain.Entities;
using DFS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFS.DataEFCore.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly DfsContext _context;

        public DeveloperRepository(DfsContext context)
        {
            _context = context;
        }

        public async Task<Developer> AddAsync(Developer newDeveloper, CancellationToken ct = default(CancellationToken))
        {
            _context.dfs_developer.Add(newDeveloper);
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

        public async Task<List<Developer>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            return await _context.dfs_developer.ToListAsync(ct);
        }

        public Task<Developer> GetByIdAsync(CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Developer developer, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
