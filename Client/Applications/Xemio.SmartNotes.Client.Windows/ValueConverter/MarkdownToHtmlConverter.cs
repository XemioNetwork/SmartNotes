using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Caliburn.Micro;
using MarkdownSharp;
using Xemio.SmartNotes.Client.Shared.Interaction;
using Xemio.SmartNotes.Shared.Common;

namespace Xemio.SmartNotes.Client.Windows.ValueConverter
{
    [ValueConversion(typeof(string), typeof(string))]
    public class MarkdownToHtmlConverter : MarkupExtension, IValueConverter
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownToHtmlConverter"/> class.
        /// </summary>
        public MarkdownToHtmlConverter()
        {
        }
        #endregion

        #region Singleton
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static MarkdownToHtmlConverter Instance
        {
            get { return Singleton<MarkdownToHtmlConverter>.Instance; } 
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
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (string) value;

            if (input == null)
                return DependencyProperty.UnsetValue;

            return IoC.Get<IMarkdownConverter>().Convert(input);
        }
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }
}
