using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Model.Request;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.MasterData;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterOtpPage : ContentPage
    {
        #region [ Objects ]     
        private string UserAuth;
        private bool isEmail;
        private string OTPString;
        AuthenticationAPI authenticationAPI;
        #endregion

        #region [ Constructor ]
        public EnterOtpPage(string UserAuth, bool isEmail = true)
        {
            InitializeComponent();
            this.UserAuth = UserAuth;
            this.isEmail = isEmail;
            authenticationAPI = new AuthenticationAPI();
            ResendButtonEnable();
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
            base.OnDisappearing();
            Dispose();
        }

        private void ResendButtonEnable()
        {
            try
            {
                BtnResentOtp.IsEnabled = false;
                int i = 120;

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    BtnResentOtp.Text = i + " sec";
                    if (i == 0)
                    {
                        BtnResentOtp.IsEnabled = true;
                        BtnResentOtp.Text = "Resend OTP";
                        return false;
                    }
                    i--;
                    return true;
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/ResendButtonEnable: " + ex.Message);
            }
        }

        private bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(TxtOtpOne.Text)
                  || Common.EmptyFiels(TxtOtpTwo.Text)
                  || Common.EmptyFiels(TxtOtpThree.Text)
                  || Common.EmptyFiels(TxtOtpFour.Text)
                  || Common.EmptyFiels(TxtOtpFive.Text)
                  || Common.EmptyFiels(TxtOtpSix.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_VerificationCode);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/Validations: " + ex.Message);
            }
            return isValid;
        }

        private Model.Request.AuthenticatePhone FillPhoneAuthentication()
        {
            Model.Request.AuthenticatePhone mAuthenticatePhone = new Model.Request.AuthenticatePhone();
            try
            {
                mAuthenticatePhone.PhoneNumber = UserAuth;
                if (!Common.EmptyFiels(Settings.fcm_token))
                {
                    mAuthenticatePhone.FcmToken = Settings.fcm_token;
                }
                if (!Common.EmptyFiels(Settings.PhoneAuthToken))
                {
                    mAuthenticatePhone.FirebaseVerificationId = Settings.PhoneAuthToken;
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
            return mAuthenticatePhone;
        }

        private Model.Request.AuthenticateEmail FillEmailAuthentication()
        {
            Model.Request.AuthenticateEmail mAuthenticateEmail = new Model.Request.AuthenticateEmail();
            try
            {
                mAuthenticateEmail.Email = UserAuth;
                OTPString = TxtOtpOne.Text + TxtOtpTwo.Text + TxtOtpThree.Text + TxtOtpFour.Text + TxtOtpFive.Text + TxtOtpSix.Text;
                mAuthenticateEmail.Otp = OTPString;
                if (!Common.EmptyFiels(Settings.fcm_token))
                {
                    mAuthenticateEmail.FcmToken = Settings.fcm_token;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return mAuthenticateEmail;
        }

        private async Task SubmitOTP()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    AuthenticateEmail mAuthenticateEmail = new AuthenticateEmail();


                    Response mResponse = new Response();
                    if (!this.isEmail)
                    {
                        OTPString = TxtOtpOne.Text + TxtOtpTwo.Text + TxtOtpThree.Text + TxtOtpFour.Text + TxtOtpFive.Text + TxtOtpSix.Text;

                        var token = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().VerifyOtpCodeAsync(OTPString);
                        if (!Common.EmptyFiels(token))
                        {
                            Settings.PhoneAuthToken = token;
                            var mLogin = FillPhoneAuthentication();
                            mResponse = await authenticationAPI.ExecutiveAuthPhone(mLogin);
                            NavigateToDashboard(mResponse);

                        }
                        else
                        {
                            Common.DisplayErrorMessage(Constraints.InValid_OTP);
                        }
                    }
                    else
                    {
                        var mLogin = FillEmailAuthentication();
                        mResponse = await authenticationAPI.ExecutiveAuthEmail(mLogin);
                        NavigateToDashboard(mResponse);
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Required_VerificationCode);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/FrmSubmit_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task ResentOTP()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await authenticationAPI.SendOtpByEmail(UserAuth);
                if (mResponse != null && mResponse.Succeeded)
                {
                    Common.DisplaySuccessMessage(mResponse.Message);
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }

                ResendButtonEnable();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/BtnResentOtp_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
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
                        var mExecutive = jObject.ToObject<Executive>();
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
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/NavigateToDashboard: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]      
        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private async void BtnSubmit_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(button: BtnSubmit);
                await SubmitOTP();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/BtnSubmit_Tapped: " + ex.Message);
            }
        }

        private async void BtnResentOtp_Tapped(object sender, EventArgs e)
        {
            await ResentOTP();
        }

        private void TxtOtpOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpOne.Text))
                TxtOtpTwo.Focus();
        }

        private void TxtOtpTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpTwo.Text))
                TxtOtpThree.Focus();
            else
                TxtOtpOne.Focus();
        }

        private void TxtOtpThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpThree.Text))
                TxtOtpFour.Focus();
            else
                TxtOtpTwo.Focus();
        }

        private void TxtOtpFour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpFour.Text))
                TxtOtpFive.Focus();
            else
                TxtOtpThree.Focus();
        }

        private void TxtOtpFive_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpFive.Text))
                TxtOtpSix.Focus();
            else
                TxtOtpFour.Focus();
        }

        private void TxtOtpSix_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpSix.Text))
            {
                TxtOtpSix.Unfocus();
                BtnSubmit.BackgroundColor = (Color)App.Current.Resources["appColor1"];
            }
            else
                TxtOtpFive.Focus();
        }
        #endregion
    }
}