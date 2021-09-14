using System.Text.RegularExpressions;
using AutoMapper;

namespace finance_app.Types.Mappers
{
    public class PascalUnderscoreNamingConvention : INamingConvention
    {
        private readonly Regex _splittingExpression = new Regex(@"[\p{Lu}0-9]+(?=_?)");

        public Regex SplittingExpression { get { return _splittingExpression; } }

        public string SeparatorCharacter { get { return "_"; } }

        public string ReplaceValue(Match match)
        {
        return match.Value.Equals("I") ? "ı" : match.Value.ToLowerInvariant();           
        }
    }
}