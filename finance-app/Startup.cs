using finance_app.Middleware;
using finance_app.Types.Configurations;
using finance_app.Types.Mappers.Profiles;
using finance_app.Types.Repositories.Accounts;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Repositories.FinanceApp;
using finance_app.Types.Repositories.Transactions;
using finance_app.Types.Services.V1;
using finance_app.Types.Services.V1.Accounts;
using finance_app.Types.Services.V1.Authorization;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.Services.V1.JournalEntries;
using finance_app.Types.Services.V1.Services.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using finance_app.Types.Services.V1.Transactions;
using finance_app.Types.Validators;
using finance_app.Types.Validators.Accounts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;

namespace finance_app {
    public class Startup
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region MVC Pipeline
            services.AddMvc(setup => {
                
                setup.Filters.Add(typeof(ExceptionResponseMapperFilter));
                setup.Filters.Add(typeof(ValidationResponseMapperFilter));
                if (_env.EnvironmentName == Environments.Development) {
                    setup.Filters.Add(typeof(LocalAuthenticationFilter));
                }

            })
            .AddFluentValidation( fv =>
            {
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;

                fv.RegisterValidatorsFromAssemblyContaining<UserResourceIdentifierValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<PaginationInfoValidator>();
                
                // Accounts
                fv.RegisterValidatorsFromAssemblyContaining<AccountResourceIdentifierValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<GetAccountsRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<CreateAccountRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<PostAccountRequestValidator>();

                // Transactions
                fv.RegisterValidatorsFromAssemblyContaining<TransactionResourceIdentifierValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<GetTransactionsRequestValidator>();

                // Journal Entries
                fv.RegisterValidatorsFromAssemblyContaining<JournalEntryResourceIdentifierValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<GetRecentJournalEntriesRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<CorrectJournalEntryRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<CreateJournalEntryRequestValidator>();
            });


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion MVC Pipeline

            services.AddApiVersioning(cfg => {
                cfg.DefaultApiVersion = new ApiVersion(1,0);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
                cfg.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("X-Version"),
                    new QueryStringApiVersionReader("v")
                );
            });

            services.AddAutoMapper(
                typeof(AccountProfile),
                typeof(TransactionProfile),
                typeof(JournalEntryProfile),
                typeof(AuthenticationProfile),
                typeof(StatusCodeProfile)
            );


            services.AddControllersWithViews();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/Account/Unauthorized/";
                    options.AccessDeniedPath = "/Account/Forbidden/";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.Name = "FinanceAppAuthCookie";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.CanAccessResource, policy =>
                    policy.Requirements.Add(new UserOwnsResource()));
            });

            #region Swagger
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
            services.AddSwaggerGen( c => {

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.ExampleFilters();

                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
                // or use the generic method, e.g. c.OperationFilter<AppendAuthorizeToSummaryOperationFilter<MyCustomAttribute>>();

                // add Security information to each operation for OAuth2
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                // or use the generic method, e.g. c.OperationFilter<SecurityRequirementsOperationFilter<MyCustomAttribute>>();

                // if you're using the SecurityRequirementsOperationFilter, you also need to tell Swashbuckle you're using OAuth2
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            });
            #endregion Swagger

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
            services.AddDbContext<AuthenticationContext>(options => {
                options.UseMySql(_configuration.GetConnectionString("MainDB"));
            });
            services.AddDbContext<FinanceAppContext>(options => {
                options.EnableSensitiveDataLogging();
                options.UseMySql(_configuration.GetConnectionString("MainDB"));
            });


            #region ConfigurationOptions
            services.Configure<LocalUserOptions>(_configuration.GetSection(LocalUserOptions.LocalUser));
            #endregion

            #region Services
            services.AddSingleton<IAuthorizationHandler, DatabaseObjectAuthorizationHandler>();
            services.AddTransient<IFinanceAppAuthorizationService, FinanceAppAuthorizationService>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAccountRepository, AccountRepository>();

            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IJournalEntryService, JournalEntryService>();

            services.AddTransient<ITransactionRepository, TransactionRepository>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IApplciationAccountService, ApplciationAccountService>();
            services.AddTransient<IPasswordService, PasswordService>();

            services.AddHttpContextAccessor();
            services.AddCookieManager(options =>
            {
                //allow cookie data to encrypt by default it allow encryption
                options.AllowEncryption = false;
                //Throw if not all chunks of a cookie are available on a request for re-assembly.
                options.ThrowForPartialCookies = true;
                // set null if not allow to devide in chunks
                options.ChunkSize = null;
                //Default Cookie expire time if expire time set to null of cookie
                //default time is 1 day to expire cookie 
                options.DefaultExpireTimeInDays = 10;
            });
            #endregion Services

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            #endregion Swagger

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            
            // app.UseMiddleware<MockAuthenticationFilter>();

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
