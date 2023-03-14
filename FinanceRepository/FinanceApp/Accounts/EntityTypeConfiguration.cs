
namespace finance_app.Types.Repositories.Accounts {

    public class AccountEntityConfig : DatabaseObjectEntityConfig<Account>
    {
        public AccountEntityConfig(): base("accounts"){ }
    }  
}