using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Repositories.Authentication;

namespace finance_app.Types.Mappers.Profiles {
    public class AuthenticationProfile : Profile {
        public AuthenticationProfile() {
            SourceMemberNamingConvention = new PascalUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<AuthenticationUser, AuthenticationUserDto>()
                .ForMember(d => d.Email, o => o.MapFrom(s => s.AuthenticationUserInfo.Email))
            .ReverseMap();

            CreateMap<AuthenticationUserInfo, AuthenticationUserDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AuthenticationUser.UserName))
            .ReverseMap();

        }



    }

}
