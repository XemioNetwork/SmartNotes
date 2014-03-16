using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xemio.SmartNotes.Client.Windows.Extensions;

namespace Xemio.SmartNotes.Client.Windows.Themes.ResourceDictionaries.NamedStyles
{
    partial class FolderTreeViewItemStyle : ResourceDictionary
    {
        /// <summary>
        /// Called before you press the right mouse button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = VisualTree.FindParentControl<TreeViewItem>(e.OriginalSource as DependencyObject);

            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }
    }
}
