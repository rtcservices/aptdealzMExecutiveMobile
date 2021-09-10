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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AboutView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]

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