
using Android.App;
using Android.Content;
using aptdealzMExecutiveMobile.Droid.DependencService;
using aptdealzMExecutiveMobile.Utility;
using Firebase.Messaging;

namespace aptdealzMExecutiveMobile.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public MyFirebaseMessagingService()
        {

        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                base.OnMessageReceived(message);
                if (!Utility.Settings.IsMuteMode)
                {
                    NotificationHelper notificationHelper = new NotificationHelper();
                    notificationHelper.ScheduleNotification(message.GetNotification().Title, message.GetNotification().Body);
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("MyFirebaseMessagingService/OnMessageReceived: " + ex.Message);
            }
        }
    }
}