using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Repository
{
    public interface IAuthenticationRepository
    {
        Task<bool> RefreshToken();

        Task DoLogout();

        Task<bool> SendOtpByEmail(string UserAuth);
    }
}
