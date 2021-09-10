using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.Login;
using System;
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

                    Settings.EmailAddress = string.Empty;
                    Settings.UserToken = string.Empty;
                    Settings.RefreshToken = string.Empty;
                    Settings.UserId = string.Empty;
                    Settings.LoginTrackingKey = string.Empty;
                    MessagingCenter.Unsubscribe<string>(this, "NotificationCount");
                    App.stoppableTimer.Stop();
                    //Settings.fcm_token = string.Empty; don't empty this token
                    App.Current.MainPage = new NavigationPage(new LoginPage());
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
    }
}
