using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Yourthome.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace Yourthome
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
            /*string con = "Server=(localdb)\\mssqllocaldb;Database=YourthomeDb;Trusted_Connection=True;";
            // устанавливаем контекст данных
            services.AddDbContext<YourthomeContext>(options => options.UseSqlServer(con));*/
            services.AddEntityFrameworkNpgsql().AddDbContext<YourthomeContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("MyWebApiConection")));
            services.AddControllers();
            services.AddSwaggerGen(c=>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version="v1",
                    Title="Yourthome",
                    Description="Simple documentation for ASP.NET CORE API"
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
