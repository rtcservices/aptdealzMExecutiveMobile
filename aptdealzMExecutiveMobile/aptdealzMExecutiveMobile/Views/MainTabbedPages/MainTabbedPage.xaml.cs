using aptdealzMExecutiveMobile.DependencyServices;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.DashboardPages;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        #region Objects       
        private string selectedView;
        #endregion

        #region Constructor    
        public MainTabbedPage(string OpenView)
        {
            InitializeComponent();
            //UnselectTab();
            //RedirectoHomeView();
            selectedView = OpenView;
            BindViews(selectedView);
        }
        #endregion

        #region Methods
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await DisplayAlert("Alert", "Do you really want to exit?", "Yes", "No");
                    if (result)
                    {
                        if (DeviceInfo.Platform == DevicePlatform.Android)
                        {
                            Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        public void BindViews(string view)
        {
            if (view == "Home")
            {
                UnselectTab();
                grdMain.Children.Clear();
                imgHome.Source = "iconHomeActive.png";
                lblHome.TextColor = Color.FromHex("FC9200");
                grdMain.Children.Add(new HomeView());
            }
            else if (view == "AddSeller")
            {
                UnselectTab();
                grdMain.Children.Clear();
                imgAddSeller.Source = "iconRequirementsActive.png";
                lblAddSeller.TextColor = Color.FromHex("FC9200");
                grdMain.Children.Add(new AddSellerView());
            }
            else if (view == "ManageSeller")
            {
                UnselectTab();
                grdMain.Children.Clear();
                imgManage.Source = "iconOrdersActive.png";
                lblManage.TextColor = Color.FromHex("FC9200");
                grdMain.Children.Add(new AddSellerView(true));
            }
            else if (view == "Manage")
            {
                UnselectTab();
                grdMain.Children.Clear();
                imgManage.Source = "iconOrdersActive.png";
                lblManage.TextColor = Color.FromHex("FC9200");
                grdMain.Children.Add(new ManageView());
            }
            else if (view == "Account")
            {
                UnselectTab();
                grdMain.Children.Clear();
                imgAccount.Source = "iconAccountActive.png";
                lblAccount.TextColor = Color.FromHex("FC9200");
                grdMain.Children.Add(new AccountView());
            }
            else if (view == "About")
            {
                UnselectTab();
                grdMain.Children.Clear();
                grdMain.Children.Add(new AboutView());
            }
            else
            {
                UnselectTab();
                grdMain.Children.Clear();
                imgHome.Source = "iconHomeActive.png";
                lblHome.TextColor = Color.FromHex("FC9200");
                grdMain.Children.Add(new HomeView());
            }
        }

        public void UnselectTab()
        {
            imgHome.Source = "iconHome.png";
            imgAddSeller.Source = "iconRequirements.png";
            imgManage.Source = "iconOrders.png";
            imgAccount.Source = "iconAccount.png";
            lblHome.TextColor = Color.FromHex("191818");
            lblAddSeller.TextColor = Color.FromHex("191818");
            lblManage.TextColor = Color.FromHex("191818");
            lblAccount.TextColor = Color.FromHex("191818");
        }
        #endregion

        #region Events
        private void StkHome_Tapped(object sender, EventArgs e)
        {
            BindViews("Home");
        }

        private void StkAddSeller_Tapped(object sender, EventArgs e)
        {
            BindViews("AddSeller");
        }

        private void StkManage_Tapped(object sender, EventArgs e)
        {
            BindViews("Manage");
        }

        private void StkAccount_Tapped(object sender, EventArgs e)
        {
            BindViews("Account");
        }
        #endregion
    }
}