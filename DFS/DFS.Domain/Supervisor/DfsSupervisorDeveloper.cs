using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFS.Domain.Converters;
using DFS.Domain.Entities;
using DFS.Domain.ViewModels;

namespace DFS.Domain.Supervisor
{
    public partial class DfsSupervisor : IDfsSupervisor
    {
        public async Task<DeveloperViewModel> AddDeveloperAsync(DeveloperViewModel newDevloperViewModel,
            CancellationToken ct = default(CancellationToken))
        {
            var developer = new Developer
            {
                DevID = Guid.NewGuid().ToString("N"),
                Email = "liuzhuang@6iuu.com"
            };

            developer = await _developerRepository.AddAsync(developer, ct);
            newDevloperViewModel.DevID = developer.DevID;
            return newDevloperViewModel;

        }

        public async Task<List<DeveloperViewModel>> GetAllDeveloperAsync(CancellationToken ct = default(CancellationToken))
        {
            var develepers = DeveloperConverter.ConvertList(await _developerRepository.GetAllAsync(ct));
            return develepers;
        }
    }
}
