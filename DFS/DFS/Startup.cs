using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFS.API.Configurations;
using DFS.API.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace DFS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMemoryCache();

            services.AddResponseCaching();

            services.AddMvc();

            // 如果将 API 版本控制添加到现有的API项目中，则可以告知 ASP.NET Core 将默认的控制器和Action视为版本1.0
            // 像这样 http://localhost:5000/api/values 调用API ，不会导致任何错误
            services.AddApiVersioning(option =>
            {
                option.AssumeDefaultVersionWhenUnspecified = true;

                // 当发送HTTP请求时，在请求头中content-type指定API版本号，如下所示（content-type: application/json;v=2.0）
                //option.ApiVersionReader = new MediaTypeApiVersionReader();
                //option.ApiVersionSelector = new CurrentImplementationApiVersionSelector(option);

                // 下面不知道怎么用呢
                //option.ApiVersionReader = new QueryStringApiVersionReader(parameterNames:"version")
                //{
                //    ParameterNames = { "api-version", "x-ms-version" }
                //};
                //option.ReportApiVersions = true;
            });

            services
                .ConfigureRepositories()
                .ConfigureSupervisor()
                .AddMiddleware()
                .AddCorsConfiguration()
                .AddConnectionProvider(Configuration)
                .AddAppSettings(Configuration);

            // Swagger
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Title = "DFS API",
                    Description = "力众华援（开发部）-分布式文件系统API"
                });
                s.OperationFilter<SwaggerFileUploadFilter>();
                s.DocInclusionPredicate((docName, description) => true);

                // api界面新增authorize按钮，在弹出文本中输入 Bearer +token即可
                s.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Authorization format : Bearer {toekn}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

            });

            // 文件上传大小限制
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // 使用API版本控制 | 需添加 Microsoft.AspnetCore.mvc.versioning (NuGet) 
            app.UseApiVersioning();

            app.UseStaticFiles();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseMvc();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1 docs");
                //s.InjectJavascript("")
            });
        }
    }
}
