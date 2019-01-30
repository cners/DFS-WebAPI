using DFS.Domain.Repositories;

namespace DFS.Domain.Supervisor
{
    public partial class DfsSupervisor:IDfsSupervisor
    {
        private readonly IDeveloperRepository _developerRepository;

        public DfsSupervisor() { }

        public DfsSupervisor(IDeveloperRepository developerRepository)
        {
            _developerRepository = developerRepository;
        }
    }
}
