using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XF_Phoneword
{
    public partial class MainPage : ContentPage
    {
        string translatedNumber;

        public MainPage()
        {
            InitializeComponent();
        }
        void OnTranslate(object sender, EventArgs e)
        {
            translatedNumber = Core.PhoneTranslator.ToNumber(phoneNumberText.Text); 
            if (!string.IsNullOrWhiteSpace(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Call " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }

        async void OnCall(object sender, EventArgs e)
        {
            var call = await DisplayAlert(
                "Dial a Number",
                "Would you like to call " + translatedNumber + "?",
                "Yes",
                "No");
            if (call)
            {
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Phone);
                    if (status != PermissionStatus.Granted)
                    {
                        if(await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Phone))
                        {
                            await DisplayAlert("Need Permission", "It will get permission of phonecall", "OK");
                        }
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Phone });
                        status = results[Permission.Phone];
                    }
                    if(status == PermissionStatus.Granted)
                    {
                        var dialer = DependencyService.Get<IDialer>();
                        if (dialer != null)
                            dialer.Dial(translatedNumber);
                    }
                    else if(status != PermissionStatus.Unknown)
                    {
                        await DisplayAlert("Permission Denied", "Can not continue, try again.", "OK");
                    }

                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.InnerException);
                }
                
            }
        }
    }
}
