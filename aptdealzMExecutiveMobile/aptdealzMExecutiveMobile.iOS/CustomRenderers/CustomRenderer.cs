using aptdealzMExecutiveMobile.Extention;
using aptdealzMExecutiveMobile.iOS.CustomRenderers;
using aptdealzMExecutiveMobile.Utility;
using CoreGraphics;
using Foundation;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtEntry), typeof(CustomEntryRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomeDatePickerRenderer))]
[assembly: ExportRenderer(typeof(ContentPage), typeof(Xamarin.Forms.Platform.iOS.PageRenderer))]
[assembly: ExportRenderer(typeof(ExtKeyboard), typeof(KeyboardViewRenderer))]

namespace aptdealzMExecutiveMobile.iOS.CustomRenderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            try
            {
                base.OnElementChanged(e);
                if (Control != null)
                {
                    Control.LeftView = new UIView(new CGRect(0, 0, 5, 0));
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.BackgroundColor = UIColor.Clear;
                    Control.BorderStyle = UITextBorderStyle.None;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/EntryRender: " + ex.Message);
            }
        }
    }

    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            try
            {
                base.OnElementChanged(e);
                if (Control != null)
                {
                    Control.BorderStyle = UITextBorderStyle.None;
                    Control.BackgroundColor = UIColor.Clear;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/PickerRenderer: " + ex.Message);
            }
        }
    }

    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            try
            {
                base.OnElementChanged(e);
                if (Control != null)
                {
                    Control.Layer.CornerRadius = 0;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/EditorRenderer: " + ex.Message);
            }
        }
    }

    public class CustomeDatePickerRenderer : DatePickerRenderer
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public static void Init() { }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);

                Control.Layer.BorderWidth = 0;
                Control.BorderStyle = UITextBorderStyle.None;
            }
            catch (Exception)
            {
                //Common.DisplayErrorMessage("iOS/DatePickerRenderer: " + ex.Message);
            }
        }
    }

    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public PageRenderer()
        {

        }

        public override void WillMoveToParentViewController(UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);
            if (parent != null)
            {
                parent.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }
        }
    }

    public class KeyboardViewRenderer : ViewRenderer
    {
        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }
        }

        void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
                _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
            if (_keyboardHideObserver == null)
                _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
        }

        void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {

            NSValue result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            CGSize keyboardSize = result.RectangleFValue.Size;
            if (Element != null)
            {
                Element.Margin = new Thickness(0, 0, 0, keyboardSize.Height); //push the entry up to keyboard height when keyboard is activated

            }
        }

        void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                Element.Margin = new Thickness(0); //set the margins to zero when keyboard is dismissed
            }

        }

        void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }
    }
}