using DFS.DataDapper.Repositories;
using DFS.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFS.UnitTest.Repository
{
    public  class DeveloperRepositoryTest
    {
        private readonly DeveloperRepository _repo;

        //public DeveloperRepositoryTest()
        //{
        //    string connectionString = "192.168.230.131,3306;Database=DFS;User=root;Password=root;Trusted_Connection=False;";
        //    _repo = new DeveloperRepository(new Domain.DbInfo.DbInfo(connectionString));
        //}

        [Fact]
        public async Task DeveloperAddAsync()
        {
            var developer = new Developer();
            developer.Email = "dafsad@lfjsdlfj.com";
            developer = await _repo.AddAsync(developer,default(CancellationToken));

            Assert.Equal("A201811131318318923a19accc072146", developer.DevID);
            //Assert.Single(developer);
        }
    }
}
