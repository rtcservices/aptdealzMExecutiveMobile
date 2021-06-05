using aptdealzMExecutiveMobile.Model;
using aptdealzMExecutiveMobile.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactSupportPage : ContentPage
    {
        #region Objects
        List<MessageList> mMessageList = new List<MessageList>();
        #endregion

        #region Constructor
        public ContactSupportPage()
        {
            InitializeComponent();

        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindMessages();
        }

        public void BindMessages()
        {
            mMessageList.Clear();
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(30, 30, 0, 0),
                MessagePosition = LayoutOptions.EndAndExpand,
                UserName = "Michal Beven",
                Time = "10:57 am",
                IsBuyer = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(0, 30, 30, 0),
                MessagePosition = LayoutOptions.StartAndExpand,
                UserName = "Customer Support",
                Time = "10:57 am",
                IsContact = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(30, 30, 0, 0),
                MessagePosition = LayoutOptions.EndAndExpand,
                UserName = "Michal Beven",
                Time = "10:57 am",
                IsBuyer = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(0, 30, 30, 0),
                MessagePosition = LayoutOptions.StartAndExpand,
                UserName = "Customer Support",
                Time = "10:57 am",
                IsContact = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(30, 30, 0, 0),
                MessagePosition = LayoutOptions.EndAndExpand,
                UserName = "Michal Beven",
                Time = "10:57 am",
                IsBuyer = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(0, 30, 30, 0),
                MessagePosition = LayoutOptions.StartAndExpand,
                UserName = "Customer Support",
                Time = "10:57 am",
                IsContact = true
            });
            flvMain.FlowItemsSource = mMessageList.ToList();
        }
        #endregion

        #region events
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
            Navigation.PopAsync();
        }

        private void ImgSend_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}