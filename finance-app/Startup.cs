using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;

using finance_app.Types.EFContexts;
using finance_app.Types.Interfaces;
using finance_app.Types.Services;
using finance_app.Types.Validators;
using finance_app.Types.Validators.RequestValidators.Accounts;
using finance_app.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace finance_app
{
    public class Startup
    {

        public IConfiguration _configuration;
        public IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region MVC Pipline
            services.AddMvc(setup => {

                setup.Filters.Add(typeof(ExceptionResponseMapperFilter));
                setup.Filters.Add(typeof(UserAuthorizationFilter));
                setup.Filters.Add(typeof(ValidationResponseMapperFilter));

            })
            .AddFluentValidation( fv =>
            {
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                fv.RegisterValidatorsFromAssemblyContaining<GetAccountsRequestsValidator>();
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion MVC Pipline

            #region Validators
            services.AddTransient<PaginationInfoValidator>();
            #endregion Validators


            services.AddControllersWithViews(); 

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            if (_env.IsDevelopment()) {
                //services.AddDbContext<AccountContext>(options => options.UseInMemoryDatabase(databaseName: "localHost"));
            }

            services.AddDbContext<AccountContext>(options => {
                options.UseMySql(_configuration.GetConnectionString("MainDB"));
            });


            #region Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAccountServiceDbo, AccountServiceDbo>();
            #endregion Services



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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
