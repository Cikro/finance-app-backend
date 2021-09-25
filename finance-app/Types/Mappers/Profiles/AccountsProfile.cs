using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.Mappers.Converters;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Mappers.Profiles
{
    // This is the approach starting with version 5
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            SourceMemberNamingConvention  = new PascalUnderscoreNamingConvention();
            DestinationMemberNamingConvention  = new PascalCaseNamingConvention();
            CreateMap<Account, AccountDto>().ReverseMap();

            CreateMap<AccountTypeEnum, AccountTypeDtoEnum>().ReverseMap();

            CreateMap<AccountTypeEnum, EnumDto<AccountTypeDtoEnum>>()
            .ConvertUsing(new EnumToEnumDtoConverter<AccountTypeEnum, AccountTypeDtoEnum>());

            CreateMap<EnumDto<AccountTypeDtoEnum>, AccountTypeEnum>()
            .ConvertUsing(new EnumDtoToEnumConverter<AccountTypeDtoEnum, AccountTypeEnum>());
        }
    }

}
