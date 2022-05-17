using aptdealzMExecutiveMobile.Repository;
using aptdealzMExecutiveMobile.Services;
using aptdealzMExecutiveMobile.Utility;
using aptdealzMExecutiveMobile.Views.MasterData;
using aptdealzMExecutiveMobile.Views.SplashScreen;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace aptdealzMExecutiveMobile
{
    public partial class App : Application
    {
        #region [ Objects ]
        public static int latitude = 0;
        public static int longitude = 0;
        //public static StoppableTimer stoppableTimer;
        //public static StoppableTimer chatStoppableTimer;
        //public static bool IsNotification = false;
        const string androidKey = "ad393bbb-48f0-4889-a6b9-d785849c694e";
        const string iosKey = "618cb0dc-b648-453e-b052-f4a49dbe6da5";
        const string LogTag = "AppCenterQuotesoukPro";
        #endregion

        #region [ Constructor ]
        public App()
        {
            try
            {
                Device.SetFlags(new string[]
                {
                    "MediaElement_Experimental",
                    "AppTheme_Experimental",
                    "FastRenderers_Experimental",
                    "CollectionView_Experimental"
                });
                Crashes.SendingErrorReport += SendingErrorReportHandler;
                Crashes.SentErrorReport += SentErrorReportHandler;
                Crashes.FailedToSendErrorReport += FailedToSendErrorReportHandler;

                InitializeComponent();

                if (Settings.IsDarkMode)
                {
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                }
                else
                {
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                }

                RegisterDependencies();
                GetCurrentLocation();
                BindCrossFirebasePushNotification();

                //if (DeviceInfo.Platform == DevicePlatform.iOS)
                //{
                //    MainPage = new Views.SplashScreen.Spalshscreen();
                //}
                //else
                //{
                if (!Settings.IsNotification)
                {
                    MainPage = new SplashScreen();
                }
                else
                {
                    MainPage = new MasterDataPage();
                    //Settings.IsNotification = false;
                }
                //}
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/Constructor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        public static void RegisterDependencies()
        {
            Xamarin.Forms.DependencyService.Register<IAuthenticationRepository, AuthenticationRepository>();
            Xamarin.Forms.DependencyService.Register<IFileUploadRepository, FileUploadRepository>();
            Xamarin.Forms.DependencyService.Register<IProfileRepository, ProfileRepository>();
            Xamarin.Forms.DependencyService.Register<INotificationRepository, NotificationRepository>();
            Xamarin.Forms.DependencyService.Register<ISellerRepository, SellerRepository>();
        }

        private async void GetCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    latitude = (int)location.Latitude;
                    longitude = (int)location.Longitude;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/GetCurrentLocation: " + ex.Message);
            }
        }

        private void BindCrossFirebasePushNotification()
        {
            try
            {
                CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                    if (DeviceInfo.Platform == DevicePlatform.iOS && !Common.EmptyFiels(p.Token))
                    {
                        Utility.Settings.fcm_token = p.Token;
                    }
                };

                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {
                    //work when platform is iOS
                    System.Diagnostics.Debug.WriteLine("Received");
                    MessagingCenter.Send<string>(string.Empty, Constraints.NotificationReceived);
                    //if (Settings.IsNotification)
                    //{
                    //    if (Common.mExecutiveDetails != null && !Common.EmptyFiels(Common.Token))
                    //    {
                    //        MainPage = new MasterDataPage();
                    //    }
                    //    else
                    //    {
                    //        MainPage = new Views.SplashScreen.SplashScreen();
                    //    }
                    //}
                };

                CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                {
                    Settings.IsNotification = true;
                    if (Settings.IsNotification)
                    {
                        if (Common.mExecutiveDetails != null && !Common.EmptyFiels(Common.mExecutiveDetails.UserId) && !Common.EmptyFiels(Common.Token))
                        {
                            MainPage = new MasterDataPage();
                        }
                        else
                        {
                            MainPage = new SplashScreen();
                        }
                    }
                    Settings.IsNotification = false;
                };

                CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Action");

                    if (!string.IsNullOrEmpty(p.Identifier))
                    {
                        System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                        foreach (var data in p.Data)
                        {
                            System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                        }
                    }
                };

                CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Deleted");
                };
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/BindCrossFirebasePushNotification: " + ex.Message);
            }
        }


        public static void PushNotificationForiOS(string title, string message)
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = 100,
                        Title = title,
                        Description = message,
                        BadgeNumber = 1,
                    };
                    NotificationCenter.Current.Show(notification);
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("App/PushNotificationForiOS: " + ex.Message);
            }
        }

        static void SendingErrorReportHandler(object sender, SendingErrorReportEventArgs e)
        {
            AppCenterLog.Info(LogTag, "Sending error report");

            var args = e as SendingErrorReportEventArgs;
            ErrorReport report = args.Report;

            //test some values
            if (report.StackTrace != null)
            {
                AppCenterLog.Info(LogTag, report.StackTrace.ToString());
            }
            else if (report.AndroidDetails != null)
            {
                AppCenterLog.Info(LogTag, report.AndroidDetails.ThreadName);
            }
        }

        static void SentErrorReportHandler(object sender, SentErrorReportEventArgs e)
        {
            AppCenterLog.Info(LogTag, "Sent error report");

            var args = e as SentErrorReportEventArgs;
            ErrorReport report = args.Report;

            //test some values
            if (report.StackTrace != null)
            {
                AppCenterLog.Info(LogTag, report.StackTrace.ToString());
            }
            else
            {
                AppCenterLog.Info(LogTag, "No system exception was found");
            }

            if (report.AndroidDetails != null)
            {
                AppCenterLog.Info(LogTag, report.AndroidDetails.ThreadName);
            }
        }

        static void FailedToSendErrorReportHandler(object sender, FailedToSendErrorReportEventArgs e)
        {
            AppCenterLog.Info(LogTag, "Failed to send error report");

            var args = e as FailedToSendErrorReportEventArgs;
            ErrorReport report = args.Report;

            //test some values
            if (report.StackTrace != null)
            {
                AppCenterLog.Info(LogTag, report.StackTrace.ToString());
            }
            else if (report.AndroidDetails != null)
            {
                AppCenterLog.Info(LogTag, report.AndroidDetails.ThreadName);
            }

            if (e.Exception != null)
            {
                AppCenterLog.Info(LogTag, "There is an exception associated with the failure");
            }
        }
        protected override void OnStart()
        {
            AppCenter.LogLevel = LogLevel.Verbose;
            Crashes.ShouldProcessErrorReport = ShouldProcess;
            //Crashes.ShouldAwaitUserConfirmation = ConfirmationHandler;
            //Crashes.GetErrorAttachments = GetErrorAttachments;
            AppCenter.Start($"android={androidKey};ios={iosKey}", typeof(Analytics), typeof(Crashes));

            AppCenter.GetInstallIdAsync().ContinueWith(installId =>
            {
                AppCenterLog.Info(LogTag, "AppCenter.InstallId=" + installId.Result);
            });
            Crashes.HasCrashedInLastSessionAsync().ContinueWith(hasCrashed =>
            {
                AppCenterLog.Info(LogTag, "Crashes.HasCrashedInLastSession=" + hasCrashed.Result);
            });
            Crashes.GetLastSessionCrashReportAsync().ContinueWith(report =>
            {
                AppCenterLog.Info(LogTag, "Crashes.LastSessionCrashReport.StackTrace=" + report.Result?.StackTrace);
            });
        }
        bool ShouldProcess(ErrorReport report)
        {
            AppCenterLog.Info(LogTag, "Determining whether to process error report");
            return true;
        }

        protected override void OnSleep()
        {
            //if (App.stoppableTimer != null)
            //    stoppableTimer.Stop();
        }

        protected override void OnResume()
        {
            //if (App.stoppableTimer != null)
            //    stoppableTimer.Start();
        }
        #endregion
    }
}
