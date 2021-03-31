using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SysDVW.Models;
using SysDVW.Services.Email;
using SysDVW.Services.Print;
using SysDVW.Utilities;

namespace SysDVW
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddLocalization();
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddSession();

            var builder = services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Security/Login", "");
                //options.Conventions.AddPageRoute("/Security/Login", "");
                //options.Conventions.AuthorizeFolder("/Home");
                //options.Conventions.AuthorizeFolder("/Payment");
                //options.Conventions.AuthorizeFolder("/Payroll");
                //options.Conventions.AuthorizeFolder("/Queries");
                //options.Conventions.AuthorizeFolder("/Services");
                //options.Conventions.AuthorizeFolder("/Security/Profile");
            });

#if DEBUG
            if (WebHostEnvironment.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif

            var _emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(_emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IPrintFuntions, PrintFuntions>();
            services.AddScoped<ISecurityCodeGeneratorService, SecurityCodeGeneratorService>();

            services.AddDbContext<InvSysContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapGet("/{0}/", async context =>
                {
                    var path = $"/Home/Index?ReturnUrl={context.Request.Path}";
                    context.Response.Redirect(path);
                    return;
                });
            });
        }
    }
}
