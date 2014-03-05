using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Xemio.SmartNotes.Client.Windows.Extensions;
using Xemio.SmartNotes.Shared.Common;

namespace Xemio.SmartNotes.Client.Windows.Themes.ValueConverter
{
    [ValueConversion(typeof(string), typeof(string))]
    public class MarkdownToStringPreviewConverter : MarkupExtension, IValueConverter
    {
        #region Constants
        /// <summary>
        /// The maximum character count.
        /// </summary>
        public const int MaxCharCount = 200;
        #endregion

        #region Singleton
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public MarkdownToStringPreviewConverter Instance
        {
            get { return Singleton<MarkdownToStringPreviewConverter>.Instance; }
        }
        #endregion

        #region Overrides of MarkupExtension
        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
        #endregion

        #region Implementation of IValueConverter
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (string)value;

            if (input == null)
                return DependencyProperty.UnsetValue;

            object converted = MarkdownToHtmlConverter.Instance.Convert(value, targetType, parameter, culture);

            var html = (string)converted;

            if (html == null)
                return DependencyProperty.UnsetValue;

            string result = html.StripHtmlTags().RemoveDoubleBreaks();

            if (result.Length > MaxCharCount - 3)
                result = result.Substring(0, MaxCharCount) + "...";

            return result;
        }
        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
}
