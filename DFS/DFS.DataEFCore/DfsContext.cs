using Microsoft.EntityFrameworkCore;
using DFS.Domain.Entities;
using System.Threading;
using DFS.DataEFCore.Configurations;

namespace DFS.DataEFCore
{
    public class DfsContext : DbContext
    {
        public virtual DbSet<Developer> dfs_developer { get; set; }

        //public virtual DbSet<ApiCallLog> ApiCallLog { get; set; }


        public static long InstanceCount;

        public DfsContext(DbContextOptions options) : base(options) =>
            Interlocked.Increment(ref InstanceCount);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new DeveloperConfiguration(modelBuilder.Entity<Developer>());
        }
    }
}
