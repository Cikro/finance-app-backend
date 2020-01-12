using Microsoft.Extensions.Configuration;

namespace finance_app.Types.EFContexts
{
    public class AccountContext : EFContext
    {
        public AccountContext(IConfiguration configuration) : base(configuration) {}

    }
}
