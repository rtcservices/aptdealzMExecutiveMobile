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
    public class AuthenticationAPI
    {
        #region [ POST ]
        public async Task<Response> SendOtpByEmail(string email)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"email\":\"" + email + "\"}";
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.SendOtpByEmail, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await SendOtpByEmail(email);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/SendOtpByEmail: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> ExecutiveAuthEmail(AuthenticateEmail mAuthenticateEmail)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mAuthenticateEmail);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.ExecutiveAuthenticateEmail, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await ExecutiveAuthEmail(mAuthenticateEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/ExecutiveAuthEmail: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> ExecutiveAuthPhone(AuthenticatePhone mAuthenticatePhone)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mAuthenticatePhone);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.ExecutiveAuthenticatePhone, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await ExecutiveAuthPhone(mAuthenticatePhone);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/ExecutiveAuthEmail: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> RefreshToken(string refreshToken)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"refreshToken\":\"" + refreshToken + "\"}";

                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.RefreshToken);

                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await RefreshToken(refreshToken);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/RefreshToken: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> Logout(string refreshToken, string loginTrackingKey)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"refreshToken\":\"" + refreshToken + "\",\"loginTrackingKey\":\"" + loginTrackingKey + "\"}";
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.Logout);
                        var response = await hcf.PostAsync(url, requestJson);
                        mResponse = await DependencyService.Get<IAuthenticationRepository>().APIResponse(response);
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await Logout(refreshToken, loginTrackingKey);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Message = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/Logout: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
