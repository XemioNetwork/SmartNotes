using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xemio.SmartNotes.Client.Windows.Extensions;
using Xemio.SmartNotes.Client.Windows.Themes.ResourceDictionaries.Brushes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.AllNotes
{
    /// <summary>
    /// Interaction logic for AllNotesView.xaml
    /// </summary>
    public partial class AllNotesView : UserControl
    {
        #region Properties
        /// <summary>
        /// Gets the view model.
        /// </summary>
        private AllNotesViewModel ViewModel
        {
            get { return (AllNotesViewModel)this.DataContext; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AllNotesView"/> class.
        /// </summary>
        public AllNotesView()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when the mouse moves on a <see cref="TreeViewItem"/>.
        /// Starts the Drag and Drop operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void TreeViewItemOnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (this.Folders.SelectedItem == null)
                return;

            var data = new DataObject(this.Folders.SelectedItem);
            DragDrop.DoDragDrop(this.Folders, data, DragDropEffects.Move);
        }
        /// <summary>
        /// Called when a Drag operation enters a <see cref="TreeViewItem"/>.
        /// Display effects on the hovered <see cref="TreeViewItem"/> here.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void TreeViewItemOnDragEnter(object sender, DragEventArgs eventArgs)
        {
            var treeViewItem = sender as TreeViewItem;

            if (treeViewItem == null)
                return;

            treeViewItem.Foreground = new DefaultColorTheme().DarkerBlueColorBrush;

            eventArgs.Handled = true;
        }
        /// <summary>
        /// Called when a Drag operation leaves a <see cref="TreeViewItem"/>.
        /// Remove effects on the hovered <see cref="TreeViewItem"/> here.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void TreeViewItemOnDragLeave(object sender, DragEventArgs eventArgs)
        {
            var treeViewItem = sender as TreeViewItem;

            if (treeViewItem == null)
                return;

            treeViewItem.ClearValue(ForegroundProperty);

            if (eventArgs != null)
                eventArgs.Handled = true;
        }
        /// <summary>
        /// Called when the dragged item is dropped.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void TreeViewOnDrop(object sender, DragEventArgs e)
        {
            var newParentItem = VisualTree.FindParentControl<TreeViewItem>((DependencyObject)e.OriginalSource);

            //Reset the foreground if we drop
            this.TreeViewItemOnDragLeave(newParentItem, null);

            FolderViewModel newParentFolder = newParentItem != null ? (FolderViewModel)newParentItem.DataContext : null;

            var draggedFolder = (FolderViewModel)e.Data.GetData(typeof(FolderViewModel));
            this.ViewModel.MoveFolder(draggedFolder, newParentFolder);
        }
        #endregion
    }
}
