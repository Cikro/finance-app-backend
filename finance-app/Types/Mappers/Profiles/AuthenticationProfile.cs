using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Mappers.Converters;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Repositories.JournalEntries;
using finance_app.Types.Repositories.Transactions;

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

            CreateMap<Requests.Authenticaiton.CreateAuthenticationUserRequest, AuthenticationUserDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email));

            CreateMap<Requests.Authenticaiton.CreateAuthenticationUserRequest, ApiResponse<AuthenticationUserDto>>()
                .ConvertUsing(new CreateAuthenticationUserRequestToApiResponseConverter<Requests.Authenticaiton.CreateAuthenticationUserRequest, ApiResponse<AuthenticationUserDto>>());

            CreateMap<Requests.Authenticaiton.LoginRequest, ApiResponse<AuthenticationUserDto>>()
                .ConvertUsing(new LoginRequestToApiResponseConverter<Requests.Authenticaiton.LoginRequest, ApiResponse<AuthenticationUserDto>>());


        }



    }

}
