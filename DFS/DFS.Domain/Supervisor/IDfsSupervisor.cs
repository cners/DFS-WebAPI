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
        // 01 开发者
        Task<List<DeveloperViewModel>> GetAllDeveloperAsync(CancellationToken ct = default(CancellationToken));

        Task<DeveloperViewModel> AddDeveloperAsync(DeveloperViewModel newDevloperViewModel,
            CancellationToken ct = default(CancellationToken));


        // 02 分布式文件
        Task<List<DfsFileViewModel>> GetAllDfsFileAsync(CancellationToken ct = default(CancellationToken));

        Task<DfsFileViewModel> AddDfsFileAsync(DfsFileViewModel newDfsFileViewModel,
            CancellationToken ct = default(CancellationToken));
        Task<string> Upload(DfsFileUploadViewModel uploadViewModel,
           CancellationToken ct = default);

    }
}
