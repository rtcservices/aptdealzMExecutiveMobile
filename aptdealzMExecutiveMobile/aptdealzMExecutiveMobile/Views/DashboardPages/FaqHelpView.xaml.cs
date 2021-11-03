using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FaqHelpView : ContentView
    {
        #region [ Objects ]       
        List<FAQResponse> mFAQResponses = new List<FAQResponse>();
        #endregion

        public FaqHelpView()
        {
            try
            {
                InitializeComponent();
                BindFAQ();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FaqHelpView/Ctor: " + ex.Message);
            }
        }



        #region [ Methods ]
        private async void BindFAQ()
        {
            try
            {
                lstFaq.ItemsSource = null;
                AppSettingsAPI appSettingsAPI = new AppSettingsAPI();
                UserDialogs.Instance.ShowLoading("Loading...");
                var mResponse = await appSettingsAPI.GetFAQ();
                UserDialogs.Instance.HideLoading();

                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    if (result != null)
                    {
                        //txtMessage.Text = string.Empty;
                        mFAQResponses = result.ToObject<List<FAQResponse>>();
                    }
                }

                if (mFAQResponses != null && mFAQResponses.Count > 0)
                {
                    lstFaq.ItemsSource = mFAQResponses.ToList();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FaqHelpView/BindFaq: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
       
        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }

        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            try
            {
                var imgExp = (Grid)sender;
                var viewCell = (ViewCell)imgExp.Parent.Parent;
                if (viewCell != null)
                {
                    viewCell.ForceUpdateSize();
                }
                var faqModel = imgExp.BindingContext as FAQResponse;
                if (faqModel != null && faqModel.ArrowImage == Constraints.Img_GreenArrowDown)
                {
                    faqModel.ArrowImage = Constraints.Img_GreenArrowUp;
                    faqModel.ShowFaqDesc = true;
                }
                else
                {
                    faqModel.ArrowImage = Constraints.Img_GreenArrowDown;
                    faqModel.ShowFaqDesc = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FaqHelpView/ImgExpand_Tapped: " + ex.Message);
            }
        }

        private void lstFaq_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstFaq.SelectedItem = null;
        }
        #endregion

    }
}