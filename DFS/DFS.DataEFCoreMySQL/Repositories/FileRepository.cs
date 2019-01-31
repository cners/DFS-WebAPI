using DFS.Domain.Entities;
using DFS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFS.DataEFCoreMySQL.Repositories
{
    public class FileRepository : BaseRepository<DfsFile>, IFileRepository
    {
        public FileRepository(DfsContext context) : base(context) { }
    }
}
