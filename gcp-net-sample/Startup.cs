using Google.Cloud.Logging.NLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Targets;

namespace gcp_net_sample
{
    public class Startup
    {
        static NLog.ILogger logger = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();


            var config = new NLog.Config.LoggingConfiguration();


            var googleTarget = new GoogleStackdriverTarget
            {                
                ProjectId = Configuration["projectid"],
                LogId = "gcp_net_sample",
                Layout = "${message}",
                IncludeCallSiteStackTrace = true,
                IncludeEventProperties = true,
                ContextProperties = { new TargetPropertyWithContext("exception", "${exception:format=toString}") },
            };

            config.AddRuleForAllLevels(googleTarget);

            LogManager.Configuration = config;
            logger = NLog.LogManager.GetLogger("debug");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
