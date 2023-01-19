using System;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Requests.Authenticaiton;

namespace finance_app.Types.Mappers.Converters {
    public class CreateAuthenticationUserRequestToApiResponseConverter<request, response> :
        ITypeConverter<CreateAuthenticationUserRequest, ApiResponse<AuthenticationUserDto>>
    {

        public ApiResponse<AuthenticationUserDto> Convert(CreateAuthenticationUserRequest source, ApiResponse<AuthenticationUserDto> destination, ResolutionContext context) {

            var data = context.Mapper.Map<AuthenticationUserDto>(source);
            var dest = new ApiResponse<AuthenticationUserDto>(data, source.Status, source.ResponseMessage);
            return dest;
        }
    }    

}