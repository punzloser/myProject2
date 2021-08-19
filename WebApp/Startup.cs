using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ApiClientCommon.Service;
using FluentValidation.AspNetCore;
using JacobDixon.AspNetCore.LiveSassCompile;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ViewModel.Catalog.Users;
using WebApp.Resources;

namespace WebApp
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
            services.AddHttpClient();
            services
                .AddFluentValidation(a => a.RegisterValidatorsFromAssemblyContaining<LoginRequest>())
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login/";
                    options.AccessDeniedPath = "/User/Forbidden/";
                });

            var cultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("vi")
            };
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddControllersWithViews().AddExpressLocalization<ExpressResources, ViewResources>(a =>
            {
                a.UseAllCultureProviders = false;
                a.ResourcesPath = "Resources";
                a.RequestLocalizationOptions = b =>
                {
                    b.SupportedCultures = cultures;
                    b.SupportedUICultures = cultures;
                    b.DefaultRequestCulture = new RequestCulture("vi");
                };
            });

            services.AddSession(a =>
            {
                a.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICarouselApiClient, CarouselApiClient>();
            services.AddTransient<IProductApiClient, ProductApiClient>();
            services.AddTransient<ICategoryApiClient, CategoryApiClient>();
            services.AddTransient<IProductSlideApiClient, ProductSlideApiClient>();
            services.AddTransient<IUserApiClient, UserApiClient>();
            services.AddTransient<IOrderApiClient, OrderApiClient>();

            services.AddLiveSassCompile();
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

            Routing.Include(app);
        }
    }
}
