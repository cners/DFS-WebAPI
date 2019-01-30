using DFS.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFS.Domain.Supervisor
{
    public interface IDfsSupervisor
    {
        Task<List<DeveloperViewModel>> GetAllDeveloperAsync(CancellationToken ct = default(CancellationToken));

        Task<DeveloperViewModel> AddDeveloperAsync(DeveloperViewModel newDevloperViewModel,
            CancellationToken ct = default(CancellationToken));

    }
}
