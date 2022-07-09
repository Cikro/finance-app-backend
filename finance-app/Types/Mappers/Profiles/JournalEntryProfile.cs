using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.Mappers.Converters;
using finance_app.Types.Repositories.JournalEntry;

namespace finance_app.Types.Mappers.Profiles
{
    public class JournalEntryProfile : Profile
    {
        public JournalEntryProfile()
        {
            SourceMemberNamingConvention  = new PascalUnderscoreNamingConvention();
            DestinationMemberNamingConvention  = new PascalCaseNamingConvention();
            CreateMap<JournalEntry, JournalEntryDto>()
            .ReverseMap();

            
            // CreateMap<CreateAccountRequest, Account>();
            // CreateMap<PostAccountRequest, Account>();
        }
    }

}
