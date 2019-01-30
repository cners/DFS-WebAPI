using DFS.Domain.Entities;
using DFS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFS.MockData.Repositories
{
    public class DeveloperRepository:IDeveloperRepository
    {
        public void Dispose()
        {

        }

        public Task<List<Developer>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Developer> GetByIdAsync(CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<Developer> AddAsync(Developer newDeveloper, CancellationToken ct = default(CancellationToken))
        {
            newDeveloper.DevID = "A201811131318318923a19accc072146";
            return newDeveloper;

        }

        public Task<bool> UpdateAsync(Developer developer, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string devID, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

    }
}
