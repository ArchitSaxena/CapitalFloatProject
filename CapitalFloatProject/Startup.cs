using CapitalFloatProject.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapitalFloatProject
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
            services.AddControllers();
            //services.Configure<AppSettings>(Configuration);
            services.AddDbContext<CapitalFloatDataContext>(x => x.UseMySQL(Configuration.GetConnectionString("CapitalFloat_DB")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration.GetSection("Swagger")["Version"], new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = Configuration.GetSection("Swagger")["Title"],
                    Version = Configuration.GetSection("Swagger")["Version"],
                    Description = Configuration.GetSection("Swagger")["Description"]
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}{Configuration.GetSection("Swagger")["BasePath"]}" } };
                });
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(Configuration.GetSection("Swagger")["EndPoint"], Configuration.GetSection("Swagger")["Name"]);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
