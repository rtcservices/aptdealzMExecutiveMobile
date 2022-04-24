using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.Login;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aptdealzMExecutiveMobile.Services
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        AuthenticationAPI authenticationAPI = new AuthenticationAPI();

        public async Task<bool> RefreshToken()
        {
            bool result = false;
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await authenticationAPI.RefreshToken(Settings.RefreshToken);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mExecutive = jObject.ToObject<Model.Response.Executive>();
                        if (mExecutive != null)
                        {
                            Settings.UserId = mExecutive.Id;
                            Settings.UserToken = mExecutive.JwToken;
                            Common.Token = mExecutive.JwToken;
                            Settings.RefreshToken = mExecutive.RefreshToken;
                            Settings.LoginTrackingKey = mExecutive.LoginTrackingKey == Constraints.LoginTrackingString ? Settings.LoginTrackingKey : mExecutive.LoginTrackingKey;

                            result = true;
                        }
                    }
                }
                else
                {
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("AuthenticationRepository/RefreshToken: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return result;
        }

        public async Task DoLogout()
        {
            try
            {
                var isClose = await App.Current.MainPage.DisplayAlert(Constraints.Logout, Constraints.AreYouSureWantLogout, Constraints.Yes, Constraints.No);
                if (isClose)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await authenticationAPI.Logout(Settings.RefreshToken, Settings.LoginTrackingKey);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        //Common.DisplaySuccessMessage(mResponse.Message);
                    }
                    else
                    {
                        if (mResponse != null && !mResponse.Message.Contains("TrackingKey"))
                            Common.DisplayErrorMessage(mResponse.Message);
                    }

                    MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                    MessagingCenter.Unsubscribe<string>(this, Constraints.NotificationReceived);
                    Common.ClearAllData();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/DoLogout: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task<bool> SendOtpByEmail(string UserAuth)
        {
            bool result = false;
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await authenticationAPI.SendOtpByEmail(UserAuth);
                if (mResponse != null && mResponse.Succeeded)
                {
                    Common.DisplaySuccessMessage(mResponse.Message);
                    result = true;
                }
                else
                {
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AuthenticationRepository/SendOtpByEmail: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return result;
        }

        public async Task<Response> APIResponse(HttpResponseMessage httpResponseMessage)
        {
            Response mResponse = new Response();
            var responseJson = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage != null)
            {
                if (!Common.EmptyFiels(responseJson))
                {
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                        if (errorString == Constraints.Session_Expired)
                        {
                            mResponse.Message = Constraints.Session_Expired;
                            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                            Common.ClearAllData();
                        }
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        mResponse.Message = Constraints.ServiceUnavailable;
                        //MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                        //Common.ClearAllData();
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        if (responseJson.Contains(Constraints.Str_Duplicate))
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else
                        {
                            mResponse.Message = Constraints.Something_Wrong_Server;
                            //MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                            //Common.ClearAllData();
                        }
                    }
                    else if (responseJson.Contains(Constraints.Str_AccountDeactivated) && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        if (Common.mExecutiveDetails != null && !Common.EmptyFiels(Common.mExecutiveDetails.FullName))
                            mResponse.Message = "Hey " + Common.mExecutiveDetails.FullName + ", your account is deactivated.Please contact customer support.";
                        else
                            mResponse.Message = "Hey, your account is deactivated.Please contact customer support.";

                        MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                        Common.ClearAllData();
                    }
                    else
                    {
                        if (responseJson.Contains(Constraints.Str_TokenExpired) || httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            var isRefresh = await DependencyService.Get<IAuthenticationRepository>().RefreshToken();
                            if (!isRefresh)
                            {
                                mResponse.Message = Constraints.Session_Expired;
                                //MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                                //Common.ClearAllData();
                            }
                        }
                        else
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            Common.ClearAllData();
                            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                        }
                    }
                }
                else
                {
                    mResponse.Succeeded = false;
                    mResponse.Message = Constraints.Something_Wrong;
                }
            }
            else
            {
                mResponse.Succeeded = false;
                mResponse.Message = Constraints.Something_Wrong;
            }
            return mResponse;
        }
    }
}
