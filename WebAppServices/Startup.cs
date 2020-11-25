using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Repository;
using Core.Services;
using Core.Services.AppSystem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using WebAppServices.Middleware;
using WebAppServices.Model;

namespace WebAppServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //添加jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,//是否验证Issuer
                        ValidateAudience = false,//是否验证Audience
                        ValidateLifetime = false,//是否验证失效时间
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidateIssuerSigningKey = false,//是否验证SecurityKey
                        ValidAudience = PublicConst.Domain,//Audience
                        ValidIssuer = PublicConst.Domain,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PublicConst.SecurityKey))//拿到SecurityKey
                    };
                });


            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                   builder =>
                   {
                       builder.AllowAnyOrigin() //允许任何来源的主机访问
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();//指定处理cookie
                   });
            });

            //注入AutoMapper服务，Mappings就是自己创建的映射类
            services.AddAutoMapper(typeof(Mappings));
            services.AddScoped<UsersSrevices>();
            services.AddScoped<SystemServices>();
            services.AddScoped<DataBaseServices>();
            services.AddScoped<AppSystemServices>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            //services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; });

            //services.sa
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }); ;

          
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
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseRequestResponseLogging();
            app.UseMvc();

            //new InitDatabase(true);
        }
    }
}
