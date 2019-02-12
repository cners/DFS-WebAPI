using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFS.API.Filters
{
    public class AddHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new NonBodyParameter()
            {
                Name = "Authorization",
                In = "header",
                Type = "string",
                Required = false,
                Description="返回的Token信息"
            });
            //operation.Parameters.Add(new NonBodyParameter()
            //{
            //    Name = "TimeStamp",
            //    In = "header",
            //    Type = "string",
            //    Required = true,
            //    Description = "时间戳",
            //});
        }
    }
}
