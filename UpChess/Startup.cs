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
                // development
                services.AddDbContext<WebApplication4Context>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("WebApplication4Context")));
            }
            else
            {
                // staging e production
                // Heroku fornece uma connection URL para PostgreSQL via variável de ambiente
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                // Como essa url de conexão é feita para o driver JDBC, precisamos quebrar a URL em pedaços
                connUrl = connUrl.Replace("postgres://", string.Empty);

                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];

                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];
                // Depois de quebrar, precismos remontar no formato connection string
                string connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}";
                services.AddDbContext<WebApplication4Context>(options =>
                        options.UseNpgsql(connStr));
            }
            services.AddScoped<IJogoService, JogoService>();
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<WebApplication4Context>())
                {
                    context.Database.Migrate();
                }
            }
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
                // TODO: debug
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error");
                //app.UseHsts();
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
