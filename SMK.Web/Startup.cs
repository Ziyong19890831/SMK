using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMK.Data;
using SMK.Web.AppScope;
using SMK.Web.AppScope.Filters;
using SMK.Web.AppScope.Middlewares;
using SMK.Web.Middlewares;
using Yozian.DependencyInjectionPlus;

namespace SMK.Web
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
            //enable session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            ////Add primary key
            ////https://stackoverflow.com/questions/40783170/neither-user-profile-nor-hklm-registry-available-using-an-ephemeral-key-reposit
            //services.AddDataProtection();
            //    .SetApplicationName("SMK")
            //    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"C:\myKeys\"));

#if DEBUG
            services.AddMvc()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // 關閉驗證失敗時自動 HTTP 400 回應
                    options.SuppressModelStateInvalidFilter = true;
                });
#endif
            var mvc = services.AddControllersWithViews(
                                options =>
                                {
                                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                                    options.AddMyFilters();
                                })
                            .AddNewtonsoftJson()
                            .AddSessionStateTempDataProvider()
                            .AddFluentValidation();//Validations

#if (DEBUG)
            //hot reload
            mvc.AddRazorRuntimeCompilation();
#endif
            services.RegisterDb(Configuration.GetConnectionString("db"))
                .RegisterServices();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
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

            app.APIExceptionHandler();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMyFilters();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Use(async (context, next) =>
            {
                // 添加查閱者原則標頭
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");

                // 添加 X-Content-Type-Options 標頭
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                // 添加 Content-Security-Policy 標頭
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
                // 添加移除 "Server" 標頭、移除 "X-Powered-By" 標頭、移除 "X-AspNetMvc-Version" 標頭、移除 "X-AspNet-Version" 標頭
                // 添加使用 "Cache-Control" 標頭
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext)state;
                    httpContext.Response.Headers.Remove("Server");
                    httpContext.Response.Headers.Remove("X-Powered-By");
                    httpContext.Response.Headers.Remove("X-AspNetMvc-Version");
                    httpContext.Response.Headers.Remove("X-AspNet-Version");
                    httpContext.Response.Headers["Cache-Control"] = "no-store";
                    return Task.CompletedTask;
                }, context);

                await next();
            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SMKWEBContext>();
                context.Database.Migrate();
            }
        }
    }
}
