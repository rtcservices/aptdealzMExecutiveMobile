using aptdealzMExecutiveMobile.Model.Request;
using aptdealzMExecutiveMobile.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Repository
{
    public interface IProfileRepository
    {
        Task<List<Country>> GetCountry();
        Task<List<State>> GetStateByCountryId(int CountryId);

        Task<List<Category>> GetCategory();

        Task<List<SubCategory>> GetSubCategory(string CategortyId);

        Task<List<Category>> CreateCategory(string OtherCategory);

        Task<List<SubCategory>> CreateSubCategory(string OtherSubCategory, string CategoryId);

        Task CreateSubCategoryByCategoryId(string OtherSubCategory, string categoryId);

        Task<bool> ValidPincode(string pinCode);
        
        Task<ExecutiveDetails> GetMyProfileData();

        //Task DeactivateAccount();
    }
}
