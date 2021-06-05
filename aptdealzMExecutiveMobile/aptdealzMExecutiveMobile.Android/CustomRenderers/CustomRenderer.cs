using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using aptdealzMExecutiveMobile.Droid.CustomRenderers;
using aptdealzMExecutiveMobile.Extention;
using aptdealzMExecutiveMobile.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtEntry), typeof(CustomEntryRender))]
[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRender))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(CustomButtonRender))]
[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]

namespace aptdealzMExecutiveMobile.Droid.CustomRenderers
{
    public class CustomEntryRender : EntryRenderer
    {
        public CustomEntryRender(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            try
            {
                base.OnElementChanged(e);
                string fontFamily = e.NewElement?.FontFamily;
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    var label = (TextView)Control; // for example
#pragma warning disable CS0618 // Type or member is obsolete
                    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
#pragma warning restore CS0618 // Type or member is obsolete
                    label.Typeface = font;
                }

                var nativeedittextfield = (Android.Widget.EditText)this.Control;
                GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(0);
                gd.SetColor(Android.Graphics.Color.Transparent);
                nativeedittextfield.Background = gd;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CustomEntryRender: " + ex.Message);
            }

        }
    }

    public class CustomLabelRender : LabelRenderer
    {
        public CustomLabelRender(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            try
            {
                base.OnElementChanged(e);
                if (e.OldElement != null)
                    return;

                if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".otf");
#pragma warning restore CS0618 // Type or member is obsolete
                    Control.Typeface = font;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/CustomLabelRender: " + ex.Message);
            }

        }
    }

    public class CustomButtonRender : ButtonRenderer
    {
        public CustomButtonRender(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            try
            {
                base.OnElementChanged(e);
                var button = Control;
                button.SetAllCaps(false);

                if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".otf");
#pragma warning restore CS0618 // Type or member is obsolete
                    Control.Typeface = font;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/CustomButtonRender: " + ex.Message);
            }
        }
    }

    public class CustomPickerRenderer : PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            try
            {
                base.OnElementChanged(e);
                string fontFamily = e.NewElement?.FontFamily;
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    var label = (TextView)Control;
#pragma warning disable CS0618 // Type or member is obsolete
                    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
#pragma warning restore CS0618 // Type or member is obsolete
                    label.Typeface = font;
                }

                var nativeedittextfield = (Android.Widget.EditText)this.Control;
                GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(0);
                gd.SetColor(Android.Graphics.Color.Transparent);
                nativeedittextfield.Background = gd;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/CustomPickerRenderer: " + ex.Message);
            }
        }
    }

    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            try
            {
                base.OnElementChanged(e);
                var fontFamily = e.NewElement?.FontFamily;
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    var label = (TextView)Control; // for example
#pragma warning disable CS0618 // Type or member is obsolete
                    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
#pragma warning restore CS0618 // Type or member is obsolete
                    label.Typeface = font;
                }
                var nativeedittextfield = (Android.Widget.EditText)this.Control;
                GradientDrawable gd = new GradientDrawable();
                nativeedittextfield.Background = gd;

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/CustomEditorRenderer: " + ex.Message);
            }
        }
    }
}