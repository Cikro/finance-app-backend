using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Requests.ApplicationAccounts;
using finance_app.Types.DataContracts.V1.Responses;
using System.Threading.Tasks;

namespace finance_app.Types.Services.V1.Interfaces {
    public interface IApplciationAccountService {
        Task<ApiResponse<AuthenticationUserDto>> CreateApplicationUser(CreateApplicationAccountRequest request);
    }
}
