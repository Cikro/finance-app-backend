using FluentValidation;
using finance_app.Types.Models.ResourceIdentifiers;

namespace finance_app.Types.Validators
{
    public class UserResourceIdentifierValidator : AbstractValidator<UserResourceIdentifier>
    {       
        public UserResourceIdentifierValidator()
        {
            RuleFor(r => r.Id).IsValidDatabaseId();
        }
    }
    public class AccountResourceIdentifierValidator : AbstractValidator<AccountResourceIdentifier>
    {       
        public AccountResourceIdentifierValidator()
        {
            RuleFor(r => r.Id).IsValidDatabaseId();
        }
    }
    
    public class TransactionResourceIdentifierValidator : AbstractValidator<AccountResourceIdentifier>
    {       
        public TransactionResourceIdentifierValidator()
        {
            RuleFor(r => r.Id).IsValidDatabaseId();
        }
    }

    public class JournalEntryResourceIdentifierValidator : AbstractValidator<AccountResourceIdentifier>
    {       
        public JournalEntryResourceIdentifierValidator()
        {
            RuleFor(r => r.Id).IsValidDatabaseId();
        }
    }
}
