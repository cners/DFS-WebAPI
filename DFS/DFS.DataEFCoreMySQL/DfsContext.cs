using Microsoft.EntityFrameworkCore;
using DFS.Domain.Entities;
using System.Threading;
using DFS.DataEFCoreMySQL.Configurations;

namespace DFS.DataEFCoreMySQL
{
    public class DfsContext : DbContext
    {
        public virtual DbSet<Developer> Developer { get; set; }

        public virtual DbSet<DfsFile> DfsFile { get; set; }
        //public virtual DbSet<ApiCallLog> ApiCallLog { get; set; }


        public static long InstanceCount;

        public DfsContext(DbContextOptions options) : base(options) =>
            Interlocked.Increment(ref InstanceCount);

        //public DfsContext(DbContextOptions<DfsContext> options) : base(options)
        //{
        //}



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new DeveloperConfiguration(modelBuilder.Entity<Developer>());
        }


    }
}
