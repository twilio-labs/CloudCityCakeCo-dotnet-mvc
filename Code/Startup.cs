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
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading.Tasks;

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
			services.Configure<VerifySettings>(Configuration.GetSection("VerifySettings"));

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultSql")));

			services.AddAuthentication()
				.AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureB2CSettings"));

			services.Configure<MicrosoftIdentityOptions>(x => {
				x.ResponseType = OpenIdConnectResponseType.Code;
				x.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
				{
					OnRedirectToIdentityProvider = ctx =>
					{
						ctx.ProtocolMessage.Scope += " https://laylastream.onmicrosoft.com/0c4226f4-84e7-4158-83d8-0021226ee498/access-as-user";
						return Task.CompletedTask;
					}
				};
			});

			services.AddAuthorization(x => x.FallbackPolicy = x.DefaultPolicy);

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICakeOrderRepository, CakeOrderRepository>();

			services.AddScoped<ICakeOrderService, CakeOrderService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<IMessagingService, MessagingService>();
			services.AddScoped<IVerifyService, VerifyService>();


			services.AddScoped<IStatusNotificationRule, CompletedNotificationRule>();
			services.AddScoped<IStatusNotificationRule, AcceptedNotificationRule>();
			services.AddScoped<INotificationHandler, NotificationHandler>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest)
								.AddRazorPagesOptions(options =>
								{
									//options.Conventions.AuthorizeAreaFolder("MicrosoftIdentity", "/Account/Manage");
									//options.Conventions.AuthorizeAreaPage("MicrosoftIdentity", "/Account/Logout");
								});


			services.AddHttpClient();
			services.AddRazorPages();
			services.AddControllersWithViews()
					.AddMicrosoftIdentityUI();
			
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
