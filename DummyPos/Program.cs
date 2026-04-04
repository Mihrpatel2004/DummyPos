
/*
using DummyPos.Data;
using DummyPos.Repositories.Interface;
using DummyPos.Repositories.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies; // <--- ADDED: Required for Cookie Auth

namespace DummyPos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<DbHelper>();

            // Your existing repositories
            builder.Services.AddScoped<IStaffRepo, StaffRepo>();
            builder.Services.AddScoped<IRoleRepo, RoleRepo>();
            builder.Services.AddScoped<IBranchRepo, BranchRepo>();
            builder.Services.AddScoped<IServiceTypeRepo, ServiceTypeRepo>();
            builder.Services.AddScoped<IToppingRepo, ToppingRepo>();
            builder.Services.AddScoped<IOfferRepo, OfferRepo>();
            builder.Services.AddScoped<IItemCategoryRepo, ItemCategoryRepo>();
            builder.Services.AddScoped<IItemRepo, ItemRepo>();
            builder.Services.AddScoped<IPaymentTypeRepo, PaymentTypeRepo>();
            builder.Services.AddScoped<IKitchenStationRepo, KitchenStationRepo>();
            builder.Services.AddScoped<IScreenRepo, ScreenRepo>();
            builder.Services.AddScoped<IFeedbackTypeRepo, FeedbackTypeRepo>();
            builder.Services.AddScoped<IItemGstRateRepo, ItemGstRateRepo>();
            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<IOrderStatusRepo, OrderStatusRepo>();
            builder.Services.AddScoped<IOrderSourceRepo, OrderSourceRepo>();
            builder.Services.AddScoped<IStaffsRepos, StaffsRepos>();
            builder.Services.AddScoped<ITableRepo, TableRepo>();
            builder.Services.AddScoped<IAdminRepo, AdminRepo>();


            // <--- ADDED: Register the new Auth Repository
            builder.Services.AddScoped<IAuthRepo, AuthRepo>();

            // <--- ADDED: Configure Cookie Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";

                // CHANGED THIS LINE: Do not send them to Login!
                options.AccessDeniedPath = "/Auth/AccessDenied";

                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();   // important
            app.UseRouting();

            // <--- ADDED: This MUST come exactly before UseAuthorization()
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Pizza}/{id?}");

            app.Run();
        }
    }
}*/
using DummyPos.Data;
using DummyPos.Repositories.Interface;
using DummyPos.Repositories.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using DummyPos.Filters;
using DummyPos.Repositories; // <--- ADDED: Tells Program.cs where your filters live!

namespace DummyPos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // <--- ADDED: This plugs the Custom Exception Filter into EVERY page globally!
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<CustomExceptionFilter>();
            });

            builder.Services.AddSingleton<DbHelper>();

            // Your existing repositories
            builder.Services.AddScoped<IStaffRepo, StaffRepo>();
            builder.Services.AddScoped<IRoleRepo, RoleRepo>();
            builder.Services.AddScoped<IBranchRepo, BranchRepo>();
            builder.Services.AddScoped<IServiceTypeRepo, ServiceTypeRepo>();
            builder.Services.AddScoped<IToppingRepo, ToppingRepo>();
            builder.Services.AddScoped<IOfferRepo, OfferRepo>();
            builder.Services.AddScoped<IItemCategoryRepo, ItemCategoryRepo>();
            builder.Services.AddScoped<IItemRepo, ItemRepo>();
            builder.Services.AddScoped<IPaymentTypeRepo, PaymentTypeRepo>();
            builder.Services.AddScoped<IKitchenStationRepo, KitchenStationRepo>();
            builder.Services.AddScoped<IScreenRepo, ScreenRepo>();
            builder.Services.AddScoped<IFeedbackTypeRepo, FeedbackTypeRepo>();
            builder.Services.AddScoped<IItemGstRateRepo, ItemGstRateRepo>();
            builder.Services.AddScoped<IOrderStatusRepo, OrderStatusRepo>();
            builder.Services.AddScoped<IOrderSourceRepo, OrderSourceRepo>();
            builder.Services.AddScoped<IStaffsRepos, StaffsRepos>();
            builder.Services.AddScoped<ITableRepo, TableRepo>();
            builder.Services.AddScoped<IAdminRepo, AdminRepo>();
            builder.Services.AddScoped<IMenuRepo, MenuRepo>();

            // Auth Repository
            builder.Services.AddScoped<IAuthRepo, AuthRepo>();

            // Configure Cookie Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Security Pipeline
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Pizza}/{id?}");

            app.Run();
        }
    }
}