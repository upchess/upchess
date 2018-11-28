using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApplication4.Business;
using WebApplication4.Util;

namespace WebApplication4
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment{ get; set; } 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            if (CurrentEnvironment.IsDevelopment()) {
                services.AddDbContext<WebApplication4Context>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("WebApplication4Context")));
            }
            else
            {
                // staging e production
                services.AddDbContext<WebApplication4Context>(options =>
                        options.UseNpgsql(Configuration.GetConnectionString("WebApplication4Context")));
            }
            services.AddScoped<IJogoService, JogoService>();
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMyMiddleware();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default",
                    "mvc/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
