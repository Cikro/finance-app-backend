using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.Transactions;
using finance_app.Types.Mappers.Converters;
using finance_app.Types.Repositories.Transactions;

namespace finance_app.Types.Mappers.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            
            SourceMemberNamingConvention  = new PascalUnderscoreNamingConvention();
            DestinationMemberNamingConvention  = new PascalCaseNamingConvention();

            CreateMap<Transaction, TransactionDto>()
            .ReverseMap();

            CreateMap<Transaction, TransactionWithJournalDto>()
            .ReverseMap();

            CreateMap<TransactionTypeEnum, TransactionTypeDtoEnum>().ReverseMap();

            CreateMap<TransactionTypeEnum, EnumDto<TransactionTypeDtoEnum>>()
            .ConvertUsing(new EnumToEnumDtoConverter<TransactionTypeEnum, TransactionTypeDtoEnum>());

            CreateMap<EnumDto<TransactionTypeDtoEnum>, TransactionTypeEnum>()
            .ConvertUsing(new EnumDtoToEnumConverter<TransactionTypeDtoEnum, TransactionTypeEnum>());

            CreateMap<UpdateTransactionRequest, Transaction>()
            .ForMember(d => d.Notes, o => o.MapFrom(s => s.Notes));
        }
    }

}
