using Dapper;
using DFS.Domain.DbInfo;
using DFS.Domain.Entities;
using DFS.Domain.Repositories;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFS.DataDapper.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private DbInfo _dbInfo;
        public DeveloperRepository(DbInfo dbInfo)
        {
            _dbInfo = dbInfo;
        }

        public IDbConnection Connection => new SqlConnection(_dbInfo.ConnectionStrings);

        public async Task<Developer> AddAsync(Developer newDeveloper, CancellationToken ct = default)
        {
            using (IDbConnection cn = Connection)
            {
                var parameters = new
                {
                    newDeveloper.DevID,
                    newDeveloper.Email
                };

                cn.Open();
                newDeveloper.DevID = cn.Query<string>(
                    "INSERT INTO dfs_developer(devid,email) VALUES (%DevID,%Email)", parameters).FirstOrDefault();
            }
            return newDeveloper;
        }

        public Task<bool> DeleteAsync(string devID, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Developer>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                return cn.Query<Developer>("select * from dfs_developer").ToList();
            }
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
