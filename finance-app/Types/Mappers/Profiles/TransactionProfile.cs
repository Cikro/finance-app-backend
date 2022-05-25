using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.Mappers.Converters;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Mappers.Profiles
{
    // This is the approach starting with version 5
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            // TODO: Actually populate this profile
            
            SourceMemberNamingConvention  = new PascalUnderscoreNamingConvention();
            DestinationMemberNamingConvention  = new PascalCaseNamingConvention();
            CreateMap<Account, AccountDto>()
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
            .ReverseMap();

            CreateMap<AccountTypeEnum, AccountTypeDtoEnum>().ReverseMap();

            CreateMap<AccountTypeEnum, EnumDto<AccountTypeDtoEnum>>()
            .ConvertUsing(new EnumToEnumDtoConverter<AccountTypeEnum, AccountTypeDtoEnum>());

            CreateMap<EnumDto<AccountTypeDtoEnum>, AccountTypeEnum>()
            .ConvertUsing(new EnumDtoToEnumConverter<AccountTypeDtoEnum, AccountTypeEnum>());

            CreateMap<CreateAccountRequest, Account>();
            CreateMap<PostAccountRequest, Account>();
        }
    }

}
