using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Xemio.SmartNotes.Client.Windows.Themes.AttachedProperties
{
    public static class WebBrowserHelper
    {
        #region Attached Properties
        /// <summary>
        /// The HTML attached property.
        /// </summary>
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof (string), typeof (WebBrowserHelper), 
            new PropertyMetadata(default(string), OnHtmlChanged));
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
        /// Called when the HTML has changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var browser = dependencyObject as WebBrowser;

            if (browser != null)
            {
                browser.NavigateToString((string)e.NewValue);
            }
        }
        #endregion
    }
}
