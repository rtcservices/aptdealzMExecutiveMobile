using aptdealzMExecutiveMobile.Model.Response;
using System.Net.Http;
using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Repository
{
    public interface IAuthenticationRepository
    {
        Task<bool> RefreshToken();

        Task DoLogout();

        Task<bool> SendOtpByEmail(string UserAuth);

        Task<Response> APIResponse(HttpResponseMessage httpResponseMessage);
    }
}
