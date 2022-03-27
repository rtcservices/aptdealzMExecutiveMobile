using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using aptdealzMExecutiveMobile.Constants;
using aptdealzMExecutiveMobile.Droid.DependencService;
using aptdealzMExecutiveMobile.Interfaces;
using aptdealzMExecutiveMobile.Utility;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using Firebase;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Plugin.Permissions;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

namespace aptdealzMExecutiveMobile.Droid
{
    [Activity(Label = "Quotesouk Pro", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Android.Net.Uri DefaultNotificationSoundURI { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            UserDialogs.Init(this);

            base.OnCreate(savedInstanceState);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            FirebaseApp.InitializeApp(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            Xamarin.Forms.DependencyService.Register<IFirebaseAuthenticator, FirebaseAuthenticator>();

            FlowListView.Init();
            CachedImageRenderer.Init(true);
            Rg.Plugins.Popup.Popup.Init(this);
            GetPermission();
            CameraPermission();

            #region [ Get Notification Tone Name ]
            var notificationManager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);
            var activeChannel = notificationManager.GetNotificationChannel("fcm_fallback_notification_channel");
            if (activeChannel != null && activeChannel.Sound != null)
            {
                if (!Common.EmptyFiels(activeChannel.Sound.Query))
                {
                    Settings.NotificationToneName = activeChannel.Sound.Query.Split('&')[0].Replace("title=", "");
                }
            }
            else
            {
                activeChannel = notificationManager.GetNotificationChannel("default");
                if (activeChannel != null && activeChannel.Sound != null)
                {
                    if (!Common.EmptyFiels(activeChannel.Sound.Query))
                    {
                        Settings.NotificationToneName = activeChannel.Sound.Query.Split('&')[0].Replace("title=", "");
                    }
                }
            }
            #endregion

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        public void GetPermission()
        {
            //var name = Android.OS.Build.VERSION.SdkInt;     //Android Version Name like Kitkate etc... 
            string version = Android.OS.Build.VERSION.Release;    //Android Version No like 4.4.4 etc... 

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                const string AccessFineLocationpermission = Manifest.Permission.AccessFineLocation;
                const string AccessCoarseLocationpermission = Manifest.Permission.AccessCoarseLocation;
                const string AccessLocationExtraCommandspermission = Manifest.Permission.AccessLocationExtraCommands;
                //const string AccessMockLocationpermission = Manifest.Permission.AccessMockLocation;
                const string AccessNetworkStatepermission = Manifest.Permission.AccessNetworkState;
                const string ChangeWifiStatepermission = Manifest.Permission.ChangeWifiState;
                const string Internetpermission = Manifest.Permission.Internet;
                const string Camerapermission = Manifest.Permission.Camera;
                const string ReadExternalStoragepermission = Manifest.Permission.ReadExternalStorage;
                const string WriteExternalStoragepermission = Manifest.Permission.WriteExternalStorage;
                const string CallPhonepermission = Manifest.Permission.CallPhone;
                const string ReadContactspermission = Manifest.Permission.ReadContacts;
                const string WriteContactspermission = Manifest.Permission.WriteContacts;
                const string ReadCallLogpermission = Manifest.Permission.ReadCallLog;

                if (CheckSelfPermission(AccessFineLocationpermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(AccessCoarseLocationpermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(AccessLocationExtraCommandspermission) != (int)Android.Content.PM.Permission.Granted
                   //|| CheckSelfPermission(AccessMockLocationpermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(AccessNetworkStatepermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(ChangeWifiStatepermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(Internetpermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(Camerapermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(ReadExternalStoragepermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(WriteExternalStoragepermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(CallPhonepermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(ReadContactspermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(WriteContactspermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(ReadCallLogpermission) != (int)Android.Content.PM.Permission.Granted
                   )
                {
                    RequestPermissions(new string[]  {
                        Manifest.Permission.AccessFineLocation,
                        Manifest.Permission.AccessCoarseLocation,
                        Manifest.Permission.AccessLocationExtraCommands,
                        //Manifest.Permission.AccessMockLocation,
                        Manifest.Permission.AccessNetworkState,
                        Manifest.Permission.ChangeWifiState,
                        Manifest.Permission.Internet,
                        Manifest.Permission.Camera,
                        Manifest.Permission.ReadExternalStorage,
                        Manifest.Permission.WriteExternalStorage,
                        Manifest.Permission.CallPhone,
                        Manifest.Permission.ReadContacts,
                        Manifest.Permission.WriteContacts,
                        Manifest.Permission.ReadCallLog,
                    },
                101);
                }
            }
        }

        public void CameraPermission()
        {
            try
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    const string Camerapermission = Manifest.Permission.Camera;
                    if (CheckSelfPermission(Camerapermission) != (int)Android.Content.PM.Permission.Granted)
                    {
                        RequestPermissions(new string[] { Manifest.Permission.Camera, }, 101);
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }

        #region [ Firebase ]
        protected override void OnNewIntent(Intent intent)
        {
            FirebasePushNotificationManager.ProcessIntent(this, intent);
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                var isEnable = Preferences.Get(AppKeys.Notification, true);
                if (isEnable)
                {
                    string title = intent.Extras.GetString(NotificationHelper.TitleKey);
                    string message = intent.Extras.GetString(NotificationHelper.MessageKey);
                    DependencyService.Get<INotificationHelper>().ReceiveNotification(title, message);
                }
            }
        }
        #endregion
    }
}