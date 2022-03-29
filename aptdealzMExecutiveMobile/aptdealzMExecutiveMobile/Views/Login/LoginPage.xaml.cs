using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.MasterData;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        #region [ Objects ]
        private bool isEmail = false;
        #endregion

        #region [ Constructor ]
        public LoginPage()
        {
            InitializeComponent();
            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
        }
        #endregion

        #region [ Methods ]
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            UserDialogs.Instance.HideLoading();
            base.OnDisappearing();
            Dispose();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await DisplayAlert(Constraints.Alert, Constraints.DoYouWantToExit, Constraints.Yes, Constraints.No);
                        if (result)
                        {
                            Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        private bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtUserAuth.Text))
                {
                    BoxUserAuth.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                    Common.DisplayErrorMessage(Constraints.Required_Email_Phone);
                }
                else if (txtUserAuth.Text.Contains("@") || txtUserAuth.Text.Contains("."))
                {
                    if (!txtUserAuth.Text.IsValidEmail())
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Email);
                    }
                    else
                    {
                        isEmail = true;
                        isValid = true;
                    }
                }
                else if (!txtUserAuth.Text.IsValidPhone())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_PhoneNumber);
                }
                else
                {
                    isEmail = false;
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/Validations: " + ex.Message);
            }
            return isValid;
        }

        private async Task AuthenticateUser()
        {
            try
            {
                if (Validations())
                {
                    txtUserAuth.Text = txtUserAuth.Text.Trim();

                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    if (isEmail)
                    {
                        UserDialogs.Instance.ShowLoading(Constraints.Loading);
                        var mResponse = await DependencyService.Get<IAuthenticationRepository>().SendOtpByEmail(txtUserAuth.Text);
                        if (mResponse)
                        {
                            await Navigation.PushAsync(new EnterOtpPage(txtUserAuth.Text));
                            txtUserAuth.Text = string.Empty;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading(Constraints.Loading);
                        var result = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().SendOtpCodeAsync(txtUserAuth.Text);
                        var keyValue = result.FirstOrDefault();

                        if (keyValue.Key)
                        {
                            if (keyValue.Value == Constraints.OTPSent)
                            {
                                await Navigation.PushAsync(new Views.Login.EnterOtpPage(txtUserAuth.Text, false));
                                txtUserAuth.Text = string.Empty;
                            }
                            else
                            {
                                Settings.PhoneAuthToken = keyValue.Value;

                                var mLogin = FillLogin();
                                var mResponse = await authenticationAPI.ExecutiveAuthPhone(mLogin);
                                NavigateToDashboard(mResponse);
                            }
                        }
                        else if (!string.IsNullOrEmpty(keyValue.Value))
                        {
                            Common.DisplayErrorMessage(keyValue.Value);
                        }
                        else
                        {
                            Common.DisplayErrorMessage(Constraints.CouldNotSentOTP);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/AuthenticateUser: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private Model.Request.AuthenticatePhone FillLogin()
        {
            Model.Request.AuthenticatePhone mLogin = new Model.Request.AuthenticatePhone();
            try
            {
                mLogin.PhoneNumber = txtUserAuth.Text;
                if (!Common.EmptyFiels(Settings.fcm_token))
                {
                    mLogin.FcmToken = Settings.fcm_token;
                }
                if (!Common.EmptyFiels(Settings.PhoneAuthToken))
                {
                    mLogin.FirebaseVerificationId = Settings.PhoneAuthToken;
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return mLogin;
        }

        private void NavigateToDashboard(Response mResponse)
        {
            try
            {
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mExecutive = jObject.ToObject<Model.Response.Executive>();
                        if (mExecutive != null)
                        {
                            Settings.UserId = mExecutive.Id;
                            Settings.RefreshToken = mExecutive.RefreshToken;
                            Settings.LoginTrackingKey = mExecutive.LoginTrackingKey == Constraints.LoginTrackingString ? Settings.LoginTrackingKey : mExecutive.LoginTrackingKey;
                            Common.Token = mExecutive.JwToken;
                            Settings.UserToken = mExecutive.JwToken;

                            App.Current.MainPage = new MasterDataPage();
                        }
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
                Common.DisplayErrorMessage("LoginPage/NavigateToDashboard: " + ex.Message);
            }
        }

        #endregion

        #region [ Events ]       
        private async void BtnGetOTP_Clicked(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(button: BtnGetOTP);
                await AuthenticateUser();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/BtnGetOtp_Clicked: " + ex.Message);
            }
        }

        private void txtUserAuth_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (Extention.ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                BoxUserAuth.BackgroundColor = (Color)App.Current.Resources["appColor8"];
            }
        }
        #endregion

    }
}