using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Model.Request;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace aptdealzMExecutiveMobile.Services
{
    public class SellerRepository : ISellerRepository
    {
        SellerManagementAPI sellerManagementAPI = new SellerManagementAPI();

        public async Task<SellerDetails> GetSellerDetails(string SellerId)
        {
            SellerDetails mSellerDetails = new SellerDetails();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await sellerManagementAPI.GetSellerDetails(SellerId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mSellerDetails = jObject.ToObject<SellerDetails>();
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SellerRepository/GetSellerDetails: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return mSellerDetails;
        }
    }
}
