using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             * L/U admin123
             * P admin123
             * E admin@company.com
             */
            /*
                Digital Markbook
                Description:
                h = home
                i = Item
                u = User
                cl = Class()
                ------------
                I = Interface
                S = Service
                C = Controller
                Cntx = Context
                _L = Layout
                Sh = Shared
            */

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ISi, Si>();
            builder.Services.AddDbContext<iCntx>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<uCntx>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentityCore<IdentityUser>(options =>
            {

            }).AddEntityFrameworkStores<uCntx>();
            builder.Services.AddAuthentication()
                .AddCookie(options => {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<uCntx>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=h}/{action=Index}/{id?}"
           );
            app.Run();
        }
    }
}
