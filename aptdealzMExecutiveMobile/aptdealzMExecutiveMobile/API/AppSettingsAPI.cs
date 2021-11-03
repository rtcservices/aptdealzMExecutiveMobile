using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Repository;

namespace aptdealzMExecutiveMobile.API
{
    public class AppSettingsAPI
    {
        #region [ GET ]
        public async Task<Response> GetPrivacyPolicyTermsAndConditions()
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetPrivacyPolicyTermsAndConditions, (int)App.Current.Resources["Version"]);
                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetPrivacyPolicyTermsAndConditions();
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AppSettingsAPI/GetPrivacyPolicyTermsAndConditions: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetFAQ()
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetFAQ, (int)App.Current.Resources["Version"]);
                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetFAQ();
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AppSettingsAPI/GetPrivacyPolicyTermsAndConditions: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetAppPromoBar()
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetAppPromoBar, (int)App.Current.Resources["Version"]);
                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetAppPromoBar();
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AppSettingsAPI/GetPrivacyPolicyTermsAndConditions: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> AboutAptdealzMEApp()
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.AboutAptdealzMEApp, (int)App.Current.Resources["Version"]);
                        var response = await hcf.GetAsync(url);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await AboutAptdealzMEApp();
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AppSettingsAPI/AboutAptdealzMEApp: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
