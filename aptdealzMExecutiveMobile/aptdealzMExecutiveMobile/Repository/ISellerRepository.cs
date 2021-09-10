using aptdealzMExecutiveMobile.Model.Request;
using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Repository
{
    public interface ISellerRepository
    {
        Task<SellerDetails> GetSellerDetails(string SellerId);
    }
}
