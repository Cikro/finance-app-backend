
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finance_app.Types.Repositories.ApplicationAccounts {

    /// Application Accounts
    public class ApplicationAccountEntityConfig : DatabaseObjectEntityConfig<ApplicationAccount>
    {
        public ApplicationAccountEntityConfig(): base("application_accounts"){ }
    }

    /// Application Users
    public class ApplicationUserEntityConfig : DatabaseObjectEntityConfig<ApplicationUser>
    {
        public ApplicationUserEntityConfig(): base("application_users"){ }
        new public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            base.Configure(builder) ;
            builder.HasOne(x => x.ApplicationAccount)
                .WithMany(x => x.ApplicationUsers)
                .HasForeignKey(x => x.ApplicationAccountId);
            builder.HasMany(x => x.ApplicationRoles)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey(x => x.ApplicationUserId);

        }
    }

    // Application Roles
    public class ApplicationRolesConfig : DatabaseObjectEntityConfig<ApplicationRole> {
        public ApplicationRolesConfig() : base("application_roles") { }
        new public void Configure(EntityTypeBuilder<ApplicationRole> builder) {
            base.Configure(builder);
        }
    }


    // Application Users Roles
    public class ApplicationUserRoleConfig : DatabaseObjectEntityConfig<ApplicationUserRole> {
        public ApplicationUserRoleConfig() : base("application_user_roles") { }
        new public void Configure(EntityTypeBuilder<ApplicationUserRole> builder) {
            base.Configure(builder);
            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.ApplicationRoles)
                .HasForeignKey(x => x.ApplicationUserId);
            builder.HasOne(x => x.ApplicationRole);
        }
    }
}