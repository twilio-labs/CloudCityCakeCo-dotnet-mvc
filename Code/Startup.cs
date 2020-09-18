using CloudCityCakeCo.Data;
using CloudCityCakeCo.Data.Repositories;
using CloudCityCakeCo.Models.DTO;
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Services.Implementations;
using CloudCityCakeCo.Services.Interfaces;
using CloudCityCakeCo.Services.NotificationRules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CloudCityCakeCo
{
    // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/mfa?view=aspnetcore-3.1
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
            services.Configure<SendGridAccount>(Configuration.GetSection("SendGridAccount"));
            services.Configure<TwilioAccount>(Configuration.GetSection("TwilioAccount"));
            services.Configure<AuthySettings>(Configuration.GetSection("AuthySettings"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultSql")));

            services.AddIdentity<User, ApplicationRole>(
                   options => options.SignIn.RequireConfirmedAccount = false)
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
                options.AddPolicy("TwoFactorEnabled",
                    x => x.RequireClaim("amr", "mfa")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICakeOrderRepository, CakeOrderRepository>();

            services.AddScoped<ICakeOrderService, CakeOrderService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMessagingService, MessagingService>();
            services.AddScoped<IAuthyService, AuthyService>();
            services.AddScoped<IPhoneNumberService, PhoneNumberService>();
            

            services.AddScoped<IStatusNotificationRule, CompletedNotificationRule>();
            services.AddScoped<IStatusNotificationRule, AcceptedNotificationRule>();
            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest)
                                .AddRazorPagesOptions(options =>
                                {
                                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    
            });

            services.AddHttpClient();
            services.AddRazorPages();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            });
        }
    }
}
