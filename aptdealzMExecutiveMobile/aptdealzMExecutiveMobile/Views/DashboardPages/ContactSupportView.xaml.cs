using Acr.UserDialogs;
using aptdealzMExecutiveMobile.API;
using aptdealzMExecutiveMobile.Model.Response;
using aptdealzMExecutiveMobile.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactSupportView : ContentView, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #region [ Objects ]
        SupportChatAPI supportChatAPI;
        private List<ChatSupport> _mMessageList;
        public List<ChatSupport> mMessageList
        {
            get { return _mMessageList; }
            set
            {
                _mMessageList = value;
                OnPropertyChanged("mMessageList");
            }
        }
        #endregion


        #region [ Constructor ]
        public ContactSupportView()
        {
            try
            {
                InitializeComponent();
                supportChatAPI = new SupportChatAPI();
                mMessageList = new List<ChatSupport>();

                Binding binding = new Binding("mMessageList", mode: BindingMode.TwoWay, source: this);
                lstChar.SetBinding(ListView.ItemsSourceProperty, binding);

                if (DeviceInfo.Platform == DevicePlatform.Android)
                    txtMessage.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                
                var backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += async delegate
                {
                    await GetMessages();
                };
                GetMessages().ConfigureAwait(false);
                MessagingCenter.Unsubscribe<string>(this, Constraints.NotificationReceived);
                MessagingCenter.Subscribe<string>(this, Constraints.NotificationReceived, (count) =>
                {
                    backgroundWorker.RunWorkerAsync();
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private async Task GetMessages()
        {
            try
            {
                var mResponse = await supportChatAPI.GetAllMyChat();
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    if (result != null)
                    {
                        mMessageList = result.ToObject<List<ChatSupport>>();
                        if (mMessageList != null && mMessageList.Count > 0)
                        {
                            foreach (var message in mMessageList)
                            {
                                if (!message.IsMessageFromSupportTeam)
                                {
                                    //User Data
                                    message.IsContact = message.IsMessageFromSupportTeam;
                                    message.IsUser = true;
                                }
                                if (message.ChatMessageFromUserProfileImage == "")
                                {
                                    if (message.IsMessageFromSupportTeam)
                                    {
                                        message.ChatMessageFromUserProfileImage = Constraints.ImgContact;
                                    }
                                    else
                                    {
                                        message.ChatMessageFromUserProfileImage = Constraints.ImgUserAccount;
                                    }
                                }
                            }

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                lstChar.IsVisible = true;
                                lblNoRecord.IsVisible = false;
                                //lstChar.ItemsSource = mMessageList.ToList();

                                var mMessage = mMessageList.LastOrDefault();
                                if (mMessage != null)
                                    lstChar.ScrollTo(mMessage, ScrollToPosition.End, false);
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                lstChar.IsVisible = false;
                                lblNoRecord.IsVisible = true;
                            });
                        }
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        lstChar.IsVisible = false;
                        lblNoRecord.IsVisible = true;
                        if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                            lblNoRecord.Text = mResponse.Message;
                        else
                            lblNoRecord.Text = Constraints.Something_Wrong;
                    });
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/GetMessages: " + ex.Message);
            }
        }

        private async Task SentMessage()
        {
            try
            {
                if (!Common.EmptyFiels(txtMessage.Text))
                {
                    var mResponse = await supportChatAPI.SendChatSupportMessage(txtMessage.Text);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        txtMessage.Text = string.Empty;
                        await GetMessages();
                    }
                    else
                    {
                        lstChar.IsVisible = false;
                        lblNoRecord.IsVisible = true;
                        if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                            lblNoRecord.Text = mResponse.Message;
                        else
                            lblNoRecord.Text = Constraints.Something_Wrong;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/SentMessage: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private void ImgBack_Tapped(object sender, EventArgs e)
        {

            //if (App.chatStoppableTimer != null)
            //{
            //    App.chatStoppableTimer.Stop();
            //    App.chatStoppableTimer = null;
            //}
            MessagingCenter.Unsubscribe<string>(this, Constraints.NotificationReceived);
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }

        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(imageButton: BtnSend);
                await SentMessage();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/BtnSend_Clicked: " + ex.Message);
            }
        }

        private async void lstChar_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstChar.IsRefreshing = true;
                mMessageList.Clear();
                await GetMessages();
                lstChar.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/lstChar_Refreshing: " + ex.Message);
            }
        }
        #endregion
    }
}