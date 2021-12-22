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
    public partial class ContactSupportView : ContentView
    {
        #region [ Objects ]
        SupportChatAPI supportChatAPI;
        private List<ChatSupport> mMessageList;
        #endregion

        #region [ Constructor ]
        public ContactSupportView()
        {
            try
            {
                InitializeComponent();
                supportChatAPI = new SupportChatAPI();
                mMessageList = new List<ChatSupport>();

                if (DeviceInfo.Platform == DevicePlatform.Android)
                    txtMessage.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

                var backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += delegate
                {
                    if (App.chatStoppableTimer != null)
                    {
                        App.chatStoppableTimer.Stop();
                        App.chatStoppableTimer = null;
                    }

                    if (App.chatStoppableTimer == null)
                    {
                        App.chatStoppableTimer = new StoppableTimer(TimeSpan.FromSeconds(3), async () =>
                        {
                            await GetMessages();
                        });
                    }
                    App.chatStoppableTimer.Start();
                };
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                if (App.chatStoppableTimer != null)
                {
                    App.chatStoppableTimer.Stop();
                    App.chatStoppableTimer = null;
                }
                Common.DisplayErrorMessage("ContactSupportPage/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]     
        private async Task GetMessages()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await supportChatAPI.GetAllMyChat();

                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    if (result != null)
                    {
                        //txtMessage.Text = string.Empty;
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
                                        message.ChatMessageFromUserProfileImage = "imgContact.jpg";
                                    }
                                    else
                                    {
                                        message.ChatMessageFromUserProfileImage = "iconUserAccount.png";
                                    }
                                }
                            }
                            lstChar.IsVisible = true;
                            lblNoRecord.IsVisible = false;
                            lstChar.ItemsSource = mMessageList.ToList();

                            var mMessage = mMessageList.LastOrDefault();
                            if (mMessage != null)
                                lstChar.ScrollTo(mMessage, ScrollToPosition.End, true);
                        }
                        else
                        {
                            lstChar.IsVisible = false;
                            lblNoRecord.IsVisible = true;
                        }
                    }
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ContactSupportPage/GetMessages: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
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

            if (App.chatStoppableTimer != null)
            {
                App.chatStoppableTimer.Stop();
                App.chatStoppableTimer = null;
            }

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