using AutoMapper;
using System.Threading.Tasks;

namespace finance_app.Types.Requests {
    public interface IRequest {

        public Task ProcessRequest();
        public T GetResponse<T>(IMapper mapper);


    }
}
