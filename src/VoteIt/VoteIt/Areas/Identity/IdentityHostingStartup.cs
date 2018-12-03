using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoteIt.Data;

[assembly: HostingStartup(typeof(VoteIt.Areas.Identity.IdentityHostingStartup))]

namespace VoteIt.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ApplicationDbContextConnection")));

                //services.AddDefaultIdentity<IdentityUser>()
                //    .AddEntityFrameworkStores<ApplicationDbContext>();

                //services.AddIdentity<IdentityUser, IdentityRole>()
                //    .AddEntityFrameworkStores<ApplicationDbContext>()
                //    .AddDefaultTokenProviders();

                //services.AddAuthentication().AddGoogle(googleOptions =>
                //{
                //    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                //    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                //});
            });
        }
    }
}