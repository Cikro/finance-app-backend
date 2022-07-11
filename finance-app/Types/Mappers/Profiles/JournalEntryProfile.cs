using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;
using finance_app.Types.Mappers.Converters;
using finance_app.Types.Repositories.JournalEntry;
using finance_app.Types.Repositories.Transaction;

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

            CreateMap<CreateJournalEntryRequest, JournalEntry>()
            .ForMember(d => d.Transactions, o => o.MapFrom(s => s.Transactions))
            .ForAllOtherMembers(d => d.Ignore());

            CreateMap<CorrectJournalEntryRequest, JournalEntry>()
            .ForMember(d => d.Transactions, o => o.MapFrom(s => s.Transactions))
            .ForAllOtherMembers(d => d.Ignore());

            
            // CreateMap<CreateAccountRequest, Account>();
            // CreateMap<PostAccountRequest, Account>();
        }



    }

}
