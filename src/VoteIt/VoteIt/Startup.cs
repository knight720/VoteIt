using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoteIt.Data;
using VoteIt.Filters;
using VoteIt.Models;
using VoteIt.Repositories;
using VoteIt.Services;

namespace VoteIt
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(config =>
                config.Filters.Add(new AuthorizationFilter())
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddRazorPagesOptions(options =>
            {
                //options.AllowAreas = true;
                options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
            });

            //// General
            services.AddScoped<FeedRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<NotifyService>();
            services.AddScoped<FeedService>();
            services.AddScoped<ReportService>();

            //// Database

            var useSQLite = bool.Parse(Configuration["UseSQLite"]);

            if (useSQLite)
            {
                services.AddEntityFrameworkSqlite();

                //// VoteItDBContext
                services.AddDbContextPool<VoteItDBContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("VoteItDB.sqlite")));

                var option = new DbContextOptionsBuilder().UseSqlite(Configuration.GetConnectionString("VoteItDB.sqlite")).Options;
                services.AddSingleton(option).AddScoped<VoteItDBContext>();

                //// ApplicationDbContext
                services.AddDbContextPool<ApplicationDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("VoteItDB.sqlite")));

                var applicationDbContextOptions = new DbContextOptionsBuilder().UseSqlite(Configuration.GetConnectionString("VoteItDB.sqlite")).Options;
                services.AddSingleton(applicationDbContextOptions).AddScoped<ApplicationDbContext>();
            }
            else
            {
                services.AddEntityFrameworkSqlServer();

                services.AddDbContextPool<VoteItDBContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("VoteItDBDatabase")));

                var option = new DbContextOptionsBuilder().UseSqlServer(Configuration.GetConnectionString("VoteItDBDatabase")).Options;
                services.AddSingleton(option).AddScoped<VoteItDBContext>();
            }

            //// Auth
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters = null;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddAuthentication()
                .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });

            //// HttpClient
            services.AddHttpClient();

            //// Host
            services.AddHostedService<TimedHostedService>();
            services.AddScoped<IScopedProcessingService, ReportService>();
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=VoteIt}/{action=Index}/{id?}");
            });
        }
    }
}