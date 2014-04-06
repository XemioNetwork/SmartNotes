using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using CefSharp;

namespace Xemio.SmartNotes.Client.Windows.Views.FacebookLogin
{
    /// <summary>
    /// Interaction logic for FacebookLoginView.xaml
    /// </summary>
    public partial class FacebookLoginView : UserControl
    {
        #region Constants
        private const string Scope = "email";
        private const string RedirectUrl = "http://www.facebook.com/connect/login_success.html";
        private const string Format = "https://www.facebook.com/dialog/oauth?client_id={0}&scope={1}&display=popup&redirect_uri={2}&response_type=code";
        #endregion

        public FacebookLoginViewModel ViewModel
        {
            get { return  (FacebookLoginViewModel)this.DataContext; }
        }

        public FacebookLoginView()
        {
            InitializeComponent();
        }
        
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ResetBrowserCookie();

            string url = this.GetLoginUrl();
            this.WebBrowser.Navigate(url);
        }

        private string GetLoginUrl()
        {
            return string.Format(Format, this.ViewModel.AppId, Scope, RedirectUrl);
        }

        private void WebBrowser_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri != null && e.Uri.LocalPath == "/r.php")
            {
                Process.Start(e.Uri.ToString());
                e.Cancel = true;
            }

            if (e.Uri != null && new Uri(RedirectUrl).IsBaseOf(e.Uri))
            {
                Execute.OnUIThread(async () =>
                {
                    string code = HttpUtility.ParseQueryString(e.Uri.Query)["code"];

                    if (string.IsNullOrWhiteSpace(code))
                    {
                        this.ViewModel.UserCanceled();
                    }
                    else
                    {
                        this.WebBrowser.Visibility = Visibility.Hidden;
                        await this.ViewModel.UserLoggedIn(code, RedirectUrl);

                        //Reset it
                        this.OnLoaded(null, null);
                        this.WebBrowser.Visibility = Visibility.Visible;
                    }
                });
            }
        }

        private IntPtr WebBrowser_OnMessageHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == 0x82)
            {
                this.ViewModel.UserCanceled();
            }

            return IntPtr.Zero;
        }

        private void ResetBrowserCookie()
        {
            string cookie = String.Format("c_user=; expires={0:R}; path=/; domain=.facebook.com", DateTime.UtcNow.AddDays(-1).ToString("R"));
            Application.SetCookie(new Uri("https://www.facebook.com"), cookie);
        }
    }
}
