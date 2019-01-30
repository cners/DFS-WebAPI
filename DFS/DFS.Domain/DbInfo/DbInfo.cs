using System;
using System.Collections.Generic;
using System.Text;

namespace DFS.Domain.DbInfo
{
    public class DbInfo : IDbInfo
    {
        public DbInfo(string connectionStrings)
        {
            ConnectionStrings = connectionStrings;
        }

        public string ConnectionStrings { get; }
    }
}
