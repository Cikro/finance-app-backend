using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Requests.Authentication;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;
using Microsoft.AspNetCore.Http;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<AuthenticationUserDto>> Login(LoginRequest loginRequest, HttpContext httpContext);
        Task<ApiResponse<AuthenticationUserDto>> CreateAuthUser(CreateAuthenticationUserRequest createUserRequest);
        }
}
