using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CefSharp.Wpf;

namespace Xemio.SmartNotes.Client.Windows.AttachedProperties
{
    public static class WebViewBindings
    {
        #region Properties
        /// <summary>
        /// The attached property of the Html property.
        /// </summary>
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof (string), typeof (WebViewBindings), 
            new PropertyMetadata(default(string), PropertyChangedCallback));
        /// <summary>
        /// Sets the HTML.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetHtml(DependencyObject element, string value)
        {
            element.SetValue(HtmlProperty, value);
        }
        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="element">The element.</param>
        public static string GetHtml(DependencyObject element)
        {
            return (string) element.GetValue(HtmlProperty);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Called when the Html property changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var webView = dependencyObject as WebView;

            if (webView == null)
                throw new InvalidOperationException("You can only use the attached property 'Html' on the 'WebView' from CefSharp.");

            SpinWait.SpinUntil(() => webView.IsBrowserInitialized, TimeSpan.FromSeconds(1));

            if (webView.IsBrowserInitialized == false)
            {
                throw new ApplicationException("The CEF browser did not initialize in time.");
            }

            webView.LoadHtml((string) e.NewValue);
        }
        #endregion
    }
}
