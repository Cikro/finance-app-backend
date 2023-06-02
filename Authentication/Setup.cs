using Authentication.AuthenticationUsers;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.Services.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication
{
    public class SetupOptions
    {
        public string AuthencationDatabaseConnectionString { get; set; }
    }
    public static class AuthenticationSetup
    {
        public static IServiceCollection AddAuthenticationUsers(this IServiceCollection services, SetupOptions authOptions)
        {
            services.TryAddTransient<IAuthenticationUserFactory, AuthenticationUserFactory>();
            services.TryAddTransient<IAuthenticationUserService, AuthenticationUserService>();
            services.TryAddTransient<IPasswordService, PasswordService>();

            services.AddDbContext<AuthenticationContext>(options => {
                options.UseMySql(authOptions.AuthencationDatabaseConnectionString, ServerVersion.AutoDetect(authOptions.AuthencationDatabaseConnectionString), null);
            });

            return services;

        }
    }
}
