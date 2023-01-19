using AutoMapper;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Services.V1.ResponseMessages;
using System.Threading.Tasks;

namespace finance_app.Types.Requests {
    public abstract class Request : IRequest {

        public ApiResponseCodesEnum Status { get; set; } = ApiResponseCodesEnum.Success;
        public IResponseMessage ResponseMessage { get; set; } = new SuccessResponseMessage();

        public abstract T GetResponse<T>(IMapper mapper);


        public abstract Task ProcessRequest();
    }
}
