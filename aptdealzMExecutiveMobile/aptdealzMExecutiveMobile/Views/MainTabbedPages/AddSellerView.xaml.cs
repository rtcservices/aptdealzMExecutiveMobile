using aptdealzMExecutiveMobile.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzMExecutiveMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSellerView : ContentView
    {
        public event EventHandler isRefresh;
        private bool IsManage = false;
        public AddSellerView()
        {
            InitializeComponent();
        }

        public AddSellerView(bool isManage)
        {
            InitializeComponent();
            IsManage = isManage;
            BindSellerdetails();
        }

        void BindSellerdetails()
        {
            if (IsManage)
            {
                lblHeader.Text = "Manage Sellers";
                imgBack.IsVisible = true;
                entrFullName.Text = "Carmen Benedict";
                entrEmail.Text = "carmenBenedict@gmail.com";
                entrPhone.Text = "+ 91 1234567890";
                entrAPhone.Text = "+ 91 9876543210";
                entrBuildingNo.Text = "ABC, 123";
                entrStreet.Text = "XYZ Road";
                entrCity.Text = "Kochi";
                entrLandmark.Text = "+ 91 9876543210";
                lblNationality.Text = "Indian";
                edtrDescription.Text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text.";
                lblCategory.Text = "Electricals and XYZ";
                entrCategoryIf.PlaceholderColor = Color.FromHex("#bbbbbb");
                entrSubCategoryIf.PlaceholderColor = Color.FromHex("#bbbbbb");
            }
        }
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

        private void pckNationality_Unfocused(object sender, FocusEventArgs e)
        {
            Xamarin.Forms.Picker picker = (Xamarin.Forms.Picker)sender;
            if (picker.SelectedItem != null)
            {
                lblNationality.Text = picker.SelectedItem.ToString();
                lblNationality.TextColor = Color.FromHex("#1A1818");
            }
        }

        private void frmNationality_Tapped(object sender, EventArgs e)
        {
            pckNationality.Focus();
        }

        private void StkTerms_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(null, null, null, StkTerms);
                var selectedImage = imgCheck.Source.ToString().Replace("File: ", string.Empty);
                if (selectedImage == "iconCheck.png")
                    imgCheck.Source = "iconUncheck.png";
                else
                    imgCheck.Source = "iconCheck.png";
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/StkTerms_Tapped" + ex.Message);
            }
        }

        private void GrdCompanyProfile_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (GrdCompanyProfile.IsVisible == false)
                {
                    GrdCompanyProfile.IsVisible = true;
                    ImgDownDownCompanyProfile.Rotation = 180;
                }
                else
                {
                    GrdCompanyProfile.IsVisible = false;
                    ImgDownDownCompanyProfile.Rotation = 0;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/GrdCompanyProfile_Tapped: " + ex.Message);
            }
        }

        private void GrdGSTInfo_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (GrdGSTInfo.IsVisible == false)
                {
                    GrdGSTInfo.IsVisible = true;
                    ImgDropDownGSTInfo.Rotation = 180;
                }
                else
                {
                    GrdGSTInfo.IsVisible = false;
                    ImgDropDownGSTInfo.Rotation = 0;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AddSellerView/GrdGSTInfo_Tapped: " + ex.Message);
            }
        }

        private void frmCategory_Tapped(object sender, EventArgs e)
        {
            pckCategory.Focus();
        }

        private void pckCategory_Unfocused(object sender, FocusEventArgs e)
        {
            Xamarin.Forms.Picker picker = (Xamarin.Forms.Picker)sender;
            if (picker.SelectedItem != null)
            {
                lblCategory.Text = picker.SelectedItem.ToString();
                lblCategory.TextColor = Color.FromHex("#1A1818");
            }
        }

        private void frmSubCategory_Tapped(object sender, EventArgs e)
        {
            pckSubCategory.Focus();
        }

        private void pckSubCategory_Unfocused(object sender, FocusEventArgs e)
        {
            Xamarin.Forms.Picker picker = (Xamarin.Forms.Picker)sender;
            if (picker.SelectedItem != null)
            {
                lblSubCategory.Text = picker.SelectedItem.ToString();
                lblSubCategory.TextColor = Color.FromHex("#1A1818");
            }
        }

        private void BtnUplodeDocs_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(BtnUplodeDocs);
        }

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(null, BtnSubmit);
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            isRefresh?.Invoke(true, EventArgs.Empty);
            Navigation.PushAsync(new MainTabbedPages.MainTabbedPage("Manage"));
        }
    }
}