
using Android.App;
using Android.Content;
using aptdealzMExecutiveMobile.Droid.DependencService;
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
            base.OnMessageReceived(message);
            new NotificationHelper().ScheduleNotification(message.GetNotification().Title, message.GetNotification().Body);
        }
    }
}