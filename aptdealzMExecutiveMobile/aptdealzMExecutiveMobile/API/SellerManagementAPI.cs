using aptdealzMExecutiveMobile.Model.Request;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aptdealzMExecutiveMobile.API
{
    public class SellerManagementAPI
    {
        #region [ GET ]
        public async Task<Response> GetAllSellers(string SortBy = "", string SearchValue = "", bool? IsAscending = null, int PageNumber = 1, int PageSize = 10)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.ListOfSellers + "?PageNumber={1}&PageSize={2}", (int)App.Current.Resources["Version"], PageNumber, PageSize);
                        if (!Common.EmptyFiels(SearchValue))
                            url += "&SearchValue=" + SearchValue;
                        if (!Common.EmptyFiels(SortBy))
                            url += "&SortBy=" + SortBy;
                        if (IsAscending.HasValue)
                            url += "&IsAscending=" + IsAscending.Value;

                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetAllSellers(SortBy, SearchValue, IsAscending, PageNumber, PageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("SellerManagementAPI/ListOfSellers: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetSellerDetails(string SellerId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetSellerDetails, (int)App.Current.Resources["Version"], SellerId);
                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetSellerDetails(SellerId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("ProfileAPI/GetMyProfileData: " + ex.Message);
            }
            return mResponse;
        }
        #endregion

        #region [ POST ]
        public async Task<Response> CreateSeller(CreateSeller mCreateSeller)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = JsonConvert.SerializeObject(mCreateSeller);
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.CreateSeller, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await CreateSeller(mCreateSeller);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("SellerManagementAPI/CreateSeller: " + ex.Message);
            }
            return mResponse;
        }
        #endregion

        #region [ PUT ]
        public async Task<Response> UpdateSeller(UpdateSeller mUpdateSeller)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mUpdateSeller);
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.UpdateSeller, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PutAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await UpdateSeller(mUpdateSeller);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("ProfileAPI/UpdateSeller: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
