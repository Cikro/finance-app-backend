
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace finance_app.Types.Repositories.Authentication
{
    public class AuthenticationContext : DbContext
    {
        private readonly IConfiguration _configuration;

        internal DbSet<AuthenticationUser> Users { get; set; }
        internal DbSet<AuthenticationUserInfo> UserInfo { get; set; }


        public  AuthenticationContext() : base() { }
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options){}

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured){                
                options.UseMySql(_configuration.GetConnectionString("MainDB"), ServerVersion.AutoDetect(_configuration.GetConnectionString("MainDB")), null);
            }
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region AuthenticationUsers
            modelBuilder.Entity<AuthenticationUser>().ToTable("authentication_users");
            modelBuilder.Entity<AuthenticationUser>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<AuthenticationUser>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<AuthenticationUser>()
                .Property(e => e.DateLastEdited)
                .ValueGeneratedOnUpdate();
            #endregion AuthenticationUsers

            #region AuthenticationUserInfo
            modelBuilder.Entity<AuthenticationUserInfo>().ToTable("authentication_users_info");
            modelBuilder.Entity<AuthenticationUserInfo>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<AuthenticationUserInfo>()
                .HasOne(p => p.AuthenticationUser);
            modelBuilder.Entity<AuthenticationUserInfo>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<AuthenticationUserInfo>()
                .Property(e => e.DateLastEdited)
                .ValueGeneratedOnUpdate();
            #endregion AuthenticationUserInfo

        }
        
    }
}
