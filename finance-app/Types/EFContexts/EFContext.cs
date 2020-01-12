using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace finance_app.Types.EFContexts
{
    public class EFContext : DbContext
    {
        IConfiguration _configuration;
        public EFContext(IConfiguration configuration){
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(_configuration.GetConnectionString("MainDB"));
    }
}
