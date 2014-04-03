using System;
using System.Collections.Generic;
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
            get { return (FacebookLoginViewModel)this.DataContext; }
        }

        public FacebookLoginView()
        {
            InitializeComponent();
        }

        private void WebView_OnLoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            Execute.OnUIThread(() =>
            {
                var uri = new Uri(e.Url);
                if (new Uri(RedirectUrl).IsBaseOf(uri))
                {

                    string code = HttpUtility.ParseQueryString(uri.Query)["code"];
                    this.ViewModel.UserLoggedIn(code);
                }
            });
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SpinWait.SpinUntil(() => this.WebView.IsBrowserInitialized, TimeSpan.FromSeconds(1));

            if (this.WebView.IsBrowserInitialized == false)
                throw new ApplicationException("The CEF browser did not initialize in time.");

            string url = this.GetLoginUrl();
            this.WebView.Load(url);
        }

        private string GetLoginUrl()
        {
            return string.Format(Format, this.ViewModel.AppId, Scope, RedirectUrl);
        }
    }
}
