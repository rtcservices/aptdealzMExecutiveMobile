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
    public class RegisterAPI
    {
        #region [ POST ]
        public async Task<Response> IsUniquePhoneNumber(Model.Request.UniquePhoneNumber mUniquePhoneNumber)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mUniquePhoneNumber);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.IsUniquePhoneNumber, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await IsUniquePhoneNumber(mUniquePhoneNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("RegisterAPI/IsUniquePhoneNumber: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> IsUniqueEmail(Model.Request.UniqueEmail mUniqueEmail)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mUniqueEmail);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.IsUniqueEmail, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await IsUniqueEmail(mUniqueEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("RegisterAPI/IsUniqueEmail: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
