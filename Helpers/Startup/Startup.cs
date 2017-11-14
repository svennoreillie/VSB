using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VSBaseAngular.Controllers;
using VSBaseAngular.Helpers.Options;

namespace VSBaseAngular
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
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
            services.Configure<ServiceConfig>(Configuration.GetSection("ServiceConfig"));
            services.Configure<ApiConfig>(Configuration.GetSection("Api"));

            DependencyInjection.AddServices(services);
            Automapper.AddMapper(services);

            //services.AddCors();

            services.AddMvc();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.Use(async (context, next) =>
            {
                var path = context.Request.Path.Value;
                if (path.StartsWith("/api")) await next();
                else if (Path.HasExtension(path)) await next();
                else
                {
                    context.Request.Path = "/";
                    await next();
                }
            });

            //app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                //Default MVC
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // routes.MapSpaFallbackRoute(
                //  name: "spa-fallback",
                //  defaults: new { controller = "Home", action = "Index" });

            });
        }
    }
}
