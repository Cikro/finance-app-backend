using System;
using System.Net;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Mappers.Converters;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Mappers.Profiles
{
    // This is the approach starting with version 5
    public class StatusCodeProfile : Profile
    {
        public StatusCodeProfile()
        {

            CreateMap<ApiResponseCodesEnum, int>().ConvertUsing(responsseCode => MapResponseCodeToHttpStatusCode(responsseCode));
        }

        private int MapResponseCodeToHttpStatusCode (ApiResponseCodesEnum responseCode)
        {
            return responseCode switch {
                ApiResponseCodesEnum.Success => (int)HttpStatusCode.OK,
                ApiResponseCodesEnum.BadRequest => (int)HttpStatusCode.BadRequest,
                ApiResponseCodesEnum.Unauthorized => (int)HttpStatusCode.Unauthorized,
                ApiResponseCodesEnum.InternalError => (int)HttpStatusCode.InternalServerError,
                ApiResponseCodesEnum.DuplicateResource => (int)HttpStatusCode.Conflict,
                ApiResponseCodesEnum.ResourceNotFound => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.OK,
            };
        }
    }

}
