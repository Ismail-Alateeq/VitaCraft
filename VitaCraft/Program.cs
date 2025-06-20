using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System;
using VitaCraft.Data;
using VitaCraft.Interfaces;
using VitaCraft.Models;
using VitaCraft.Repository;
using VitaCraft.Services;

namespace VitaCraft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var key = builder.Configuration["OpenAi:key"];

            builder.Services.AddSingleton<Kernel>(sp =>
            {
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddOpenAIChatCompletion("gpt-4", key);
                return kernelBuilder.Build();
            });
            builder.Services.AddSingleton<IResumeOpenAiSrevice,ResumeOpenAiService>();
            builder.Services.AddSingleton<IPortFolioOpenAiService, PortFolioOpenAiService>();
            builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
            builder.Services.AddScoped<IPortFolioRepository, PortFolioRepository>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
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

            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                if (context.User?.Identity?.IsAuthenticated == true && context.Request.Path == "/")
                {
                    var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = await userManager.GetUserAsync(context.User);
                    if (user != null && await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        context.Response.Redirect("/Admin/Index"); 
                        return;
                    }

                    context.Response.Redirect("/Home/Index");
                    return;
                }

                await next();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
                

             
            app.Run();
        }
    }
}
