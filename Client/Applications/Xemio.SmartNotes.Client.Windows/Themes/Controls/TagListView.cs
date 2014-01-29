using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Xemio.SmartNotes.Client.Windows.Themes.Controls
{
    public class TagListView : ListView
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="TagListView"/> class.
        /// </summary>
        static TagListView()
        {
        }
        #endregion

        #region Overrides of ListView
        /// <summary>
        /// Creates and returns a new <see cref="T:System.Windows.Controls.ListViewItem" /> container.
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TagListViewItem();
        }
        #endregion
    }
}
