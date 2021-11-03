using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutView : ContentView
    {
        #region [ Objects ]
        #endregion

        #region [ Constructor ]
        public AboutView()
        {
            try
            {
                InitializeComponent();
                BindAboutApzdealz();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AboutView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        async void BindAboutApzdealz()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading...");
                AppSettingsAPI appSettingsAPI = new AppSettingsAPI();
                var mResponse = await appSettingsAPI.AboutAptdealzMEApp();
                UserDialogs.Instance.HideLoading();

                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mAboutAptDealz = jObject.ToObject<AboutAptDealz>();
                        if (mAboutAptDealz != null)
                        {
                            lblAbout.Text = mAboutAptDealz.About;
                            lblAddress1.Text = mAboutAptDealz.ContactAddressLine1;
                            lblAddress2.Text = mAboutAptDealz.ContactAddressLine2;
                            lblPincode.Text = "PIN - " + mAboutAptDealz.ContactAddressPincode;
                            lblEmail.Text = "Email : " + mAboutAptDealz.ContactAddressEmail;
                            lblPhoneNo.Text = "Phone : " + mAboutAptDealz.ContactAddressPhone;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region [ Events ]
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }
        #endregion
    }
}