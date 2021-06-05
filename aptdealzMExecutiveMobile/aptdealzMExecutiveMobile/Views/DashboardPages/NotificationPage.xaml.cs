using aptdealzMExecutiveMobile.Model;
using aptdealzMExecutiveMobile.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        #region Objects
        // create objects here
        public List<Notification> Notifications = new List<Notification>();
        #endregion

        #region Constructor
        public NotificationPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // write methods here
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindNotification();
        }

        public void BindNotification()
        {
            lstNotification.ItemsSource = null;
            Notifications = new List<Notification>()
            {
                new Notification{ NotificationTitle="New quote received for REQ#123", NotificationDesc=""},
                new Notification{ NotificationTitle="New quote received for REQ#121", NotificationDesc=""},
                new Notification{ NotificationTitle="New response received for your grienvance GR#01", NotificationDesc=""},
                new Notification{ NotificationTitle="Free Requirements post limit reached. Make payment to post further requirements.", NotificationDesc=""},
                new Notification{ NotificationTitle="Your order for INV#121 has been shipped.", NotificationDesc=""},
                new Notification{ NotificationTitle="New response received for your grienvance GR#03", NotificationDesc=""},
                new Notification{ NotificationTitle="Your order for INV#123 has been shipped.", NotificationDesc=""},
            };
            lstNotification.ItemsSource = Notifications.ToList();
        }
        #endregion

        #region Events        
        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(ImgMenu);
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgNotification);
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgQuestion);
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(null, null, null, null, null, ImgBack);
            Navigation.PushAsync(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void ImgClose_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}