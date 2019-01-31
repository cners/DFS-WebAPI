using DFS.DataDapper.Repositories;
using DFS.Domain.Entities;
using JetBrains.dotMemoryUnit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFS.UnitTest.Repository
{
    public class DeveloperRepositoryTest
    {
        private readonly DeveloperRepository _repo;

        public DeveloperRepositoryTest()
        {
            string connectionString = "Server=.;Database=DFS;User ID=sa;Password=sa;";
            connectionString = "Server=192.168.230.131;Database=DFS;User ID=root;Password=root;";//MySQL
            _repo = new DeveloperRepository(new Domain.DbInfo.DbInfo(connectionString));
        }

        //[AssertTraffic(AllocatedSizeInBytes = 1000, Types = new[] { typeof(Developer) })]
        [Fact]
        public async Task DeveloperAddAsync()
        {
            var developer = new Developer();
            developer.DevID = Guid.NewGuid().ToString("N");
            developer.Email = "dafsad@lfjsdlfj.com";
            developer = await _repo.AddAsync(developer, default(CancellationToken));

            //Assert.Equal("A201811131318318923a19accc072146", developer.DevID);
            //Assert.Single(developer);
        }
        [Fact]
        public async Task DeveloperAddMulAsync()
        {
            for (int j = 0; j < 2; j++)
            {
                var developer = new Developer();
                developer.DevID = Guid.NewGuid().ToString("N");
                developer.Email = $"{j}-d@dfa.com";
                var developerd = _repo.AddAsync(developer, default(CancellationToken));
            }


            //Assert.Equal("A201811131318318923a19accc072146", developer.DevID);
            //Assert.Single(developer);
        }


        [DotMemoryUnit(FailIfRunWithoutSupport = false)]
        [Fact]
        public async Task DeveloperGetAllAsync()
        {
            var developers = await _repo.GetAllAsync();

            Assert.Single(developers);

        }


    }
}
