using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace XF_Phoneword
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        void OpenWebView(object sender, EventArgs e)
        {
            var inputUrl = url.Text;
            var webView = new WebView
            {
                Source = inputUrl
            };
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            Content = webView;
        }
        async void ReadQR(object sender, EventArgs e)
        {
            var scanUrl = "";
            var scanPage = new ZXingScannerPage()
            {
                DefaultOverlayTopText = "バーコードを読み取ります",
                DefaultOverlayBottomText = "",
            };
            // スキャナページを表示
            await Navigation.PushAsync(scanPage);

            // データが取れると発火
            scanPage.OnScanResult += (result) =>
            {
                // スキャン停止
                scanPage.IsScanning = false;

                // PopAsyncで元のページに戻り、結果をダイアログで表示
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("スキャン完了", result.Text, "OK");
                    scanUrl = result.Text;
                    await Navigation.PushAsync(GetWebViewPage(scanUrl));
                });
                
            };
           
        }

        private ContentPage GetWebViewPage(string url)
        {
            return new ContentPage()
            {
                Content = new WebView()
                {
                    Source = url
                }
            };
        }

    }
}
