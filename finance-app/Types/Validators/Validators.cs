
using FluentValidation;

namespace finance_app.Types.Validators {
    public static class Validators {
        public static IRuleBuilderOptions<T, uint> IsValidDatabaseId<T>(this IRuleBuilder<T, uint> ruleBuilder) {
            return ruleBuilder.Must(id => id != 0).WithMessage("Invalid Id for {PropertyName}.");
        }
        public static IRuleBuilderOptions<T, uint?> IsValidDatabaseId<T>(this IRuleBuilder<T, uint?> ruleBuilder) {
            return ruleBuilder.Must(id => id != 0).WithMessage("Invalid Id for {PropertyName}.");
        }
    }
}