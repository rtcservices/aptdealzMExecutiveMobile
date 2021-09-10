using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Repository
{
    public interface IFileUploadRepository
    {
        Task<string> UploadFile(int fileUploadCategory);
    }
}
