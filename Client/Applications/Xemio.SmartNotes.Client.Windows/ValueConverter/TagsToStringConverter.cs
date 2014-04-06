using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using Xemio.SmartNotes.Shared.Common;

namespace Xemio.SmartNotes.Client.Windows.ValueConverter
{
    [ValueConversion(typeof(IEnumerable<string>), typeof(string))]
    public class TagsToStringConverter : MarkupExtension, IValueConverter
    {
        #region Overrides of MarkupExtension
        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
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
            var input = (IEnumerable<string>)value;

            if (input == null)
                return string.Empty;

            return string.Join(", ", input);
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
            var input = (string) value;

            if (input == null)
                return new string[0];

            return (from tag in input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    select tag.Trim()).ToArray();
        }
        #endregion
    }
}
