
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finance_app.Types.Repositories.ApplicationAccounts {

    public class ApplicationAccountEntityConfig : DatabaseObjectEntityConfig<ApplicationAccount>
    {
        public ApplicationAccountEntityConfig(): base("application_accounts"){ }
    }  

    public class ApplicationUserEntityConfig : DatabaseObjectEntityConfig<ApplicationUser>
    {
        public ApplicationUserEntityConfig(): base("application_users"){ }
        new public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            base.Configure(builder) ;
            builder.HasOne(x => x.ApplicationAccount)
                .WithMany(x => x.ApplicationUsers)
                .HasForeignKey(x => x.ApplicationAccountId);
            
        }
        
    }  
}