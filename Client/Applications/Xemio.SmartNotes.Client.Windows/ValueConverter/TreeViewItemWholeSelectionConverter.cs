using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Xemio.SmartNotes.Client.Windows.Extensions;
using Xemio.SmartNotes.Shared.Common;

namespace Xemio.SmartNotes.Client.Windows.ValueConverter
{
    [ValueConversion(typeof(TreeViewItem), typeof(Thickness), ParameterType = typeof(double))]
    public class TreeViewItemWholeSelectionConverter : MarkupExtension, IValueConverter
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
            var treeViewItem = (TreeViewItem) value;

            if (value == null)
                return DependencyProperty.UnsetValue;

            var length = double.Parse(parameter.ToString());
            
            return new Thickness(-length * this.GetDepth(treeViewItem), 0, 0, 0);
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

        #region Private Methods
        /// <summary>
        /// Gets the depth of the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        private int GetDepth(TreeViewItem item)
        {
            TreeViewItem parent;

            while ((parent = (VisualTree.FindParentControl<TreeViewItem>(item))) != null)
            {
                return GetDepth(parent) + 1;
            }
            return 0;
        }
        #endregion
    }
}
