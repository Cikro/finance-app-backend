
using System.Text.RegularExpressions;
using FluentValidation;

namespace finance_app.Types.Validators {
    public static class Validators {
        private const string VALID_CHARACTERS_REGEX = @"[^a-zA-Z0-9 ""\(\)\.\'\-\+=_\/\?,\!@#$%\^&\*]";

        public static IRuleBuilderOptions<T, uint> IsValidDatabaseId<T>(this IRuleBuilder<T, uint> ruleBuilder) {
            return ruleBuilder.Must(id => id != 0).WithMessage("Invalid Id for {PropertyName}.");
        }
        public static IRuleBuilderOptions<T, uint?> IsValidDatabaseId<T>(this IRuleBuilder<T, uint?> ruleBuilder) {
            return ruleBuilder.Must(id => id != 0).WithMessage("Invalid Id for {PropertyName}.");
        }

        /// <summary>
        /// Validate that the string contains only valid characters to protect against XSS
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> ContainsOnlyValidCharacters<T>(this IRuleBuilder<T, string> ruleBuilder) {
            var re = new Regex(VALID_CHARACTERS_REGEX);
            return ruleBuilder.Must(str => !re.IsMatch(str)).WithMessage($"{{Property name}} contains invalid characters. Valid characters are: {VALID_CHARACTERS_REGEX}");
        }
    }
}