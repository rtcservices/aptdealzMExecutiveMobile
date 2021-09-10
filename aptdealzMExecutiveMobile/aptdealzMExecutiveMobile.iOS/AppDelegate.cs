using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;

namespace aptdealzMExecutiveMobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            FlowListView.Init();
            CachedImageRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
