using DFS.Domain.Repositories;

namespace DFS.Domain.Supervisor
{
    public partial class DfsSupervisor:IDfsSupervisor
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly IFileRepository _fileRepository;

        public DfsSupervisor() { }

        public DfsSupervisor(IDeveloperRepository developerRepository,
           IFileRepository fileRepository)
        {
            _developerRepository = developerRepository;
            _fileRepository = fileRepository;
        }
    }
}
