using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace finance_app.Types.Repositories
{
    public class EFContext : DbContext
    {
        IConfiguration _configuration;
        public EFContext(DbContextOptions options) : base(options){}
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured){                
                options.UseMySql(_configuration.GetConnectionString("MainDB"));
            }
        }
    }
}
