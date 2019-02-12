using DFS.API.Configurations;
using DFS.API.Controllers;
using DFS.API.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;

namespace DFS
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(c => c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMemoryCache();

            services.AddResponseCaching();

            services.AddMvcCore().AddApiExplorer();

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

            #region 版本控制
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            #endregion

            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new Info
                {
                    Title = "DFS 接口文档",
                    Description = "力众华援（开发部）-分布式文件系统API",
                    Version = "v1.0",
                    Contact = new Contact
                    {
                        Name = "Liu Zhuang",
                        Email = "liu.zhuang@lzassist.com"
                    }
                });

                options.SwaggerDoc("v2.0", new Info { Title = "DFS API -v2", Version = "V2" });

                options.OperationFilter<RemoveVersionFromParameter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                options.OperationFilter<SwaggerFileUploadFilter>();
                //options.DocInclusionPredicate((docName, description) => true);
                options.DocInclusionPredicate((version, desc) =>
                {
                    var versions = desc.ControllerAttributes()
                                    .OfType<ApiVersionAttribute>()
                                    .SelectMany(attr => attr.Versions);

                    var maps = desc.ActionAttributes()
                                .OfType<MapToApiVersionAttribute>()
                                .SelectMany(attr => attr.Versions)
                                .ToArray();

                    return versions.Any(v => $"v{v.ToString()}" == version) && (maps.Length == 0 || maps.Any(v => $"v{v.ToString()}" == version));
                });
                options.OperationFilter<AddHeaderParameter>();

                // api界面新增authorize按钮，在弹出文本中输入 Bearer +token即可
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Authorization format : Bearer {toekn}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });


                options.IgnoreObsoleteActions();
            });

            services.ConfigureSwaggerGen(c =>
            {
                // 配置生成的 xml 注释文档路径
                var rootPath = AppContext.BaseDirectory;
                c.IncludeXmlComments(Path.Combine(rootPath, "DFS.API.Doc.xml"));
                c.IncludeXmlComments(Path.Combine(rootPath, "DFS.Domain.xml"));
            });

            // 文件上传大小限制
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            // 授权验证

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Developer/Auth");
                    o.AccessDeniedPath = new PathString("/Error/Forbidden");
                });

            #region 跨域访问
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin()        //允许任何来源的主机访问
                                                    //builder.WithOrigins("http://localhost:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();             // 指定处理cookie
                });
            });
            #endregion

            #region 添加Redis缓存
            services.AddRedisConfiguration(Configuration);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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



            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "分布式文件系统web接口";

                c.SwaggerEndpoint($"/swagger/v1.0/swagger.json", "V1.0 Docs");
                c.SwaggerEndpoint($"/swagger/v2.0/swagger.json", "V2.0 Docs");

                //c.DefaultModelExpandDepth(4);
                c.DefaultModelRendering(ModelRendering.Model);
                //c.DefaultModelsExpandDepth(-1);                   // 隐藏展示前端实体
                //c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.MaxDisplayedTags(5);
                c.ShowExtensions();
                c.EnableValidator();
                c.SupportedSubmitMethods(SubmitMethod.Get, 
                                        SubmitMethod.Head,
                                        SubmitMethod.Post,
                                        SubmitMethod.Patch,
                                        SubmitMethod.Delete,
                                        SubmitMethod.Put);
            });

            app.UseAuthentication();


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
