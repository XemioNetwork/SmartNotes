using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Xemio.SmartNotes.Client.Windows.Themes.Controls
{
    public class TogglePopup : Popup
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="TogglePopup"/> class.
        /// </summary>
        static TogglePopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TogglePopup), new FrameworkPropertyMetadata(typeof(TogglePopup)));
        }
        #endregion

        #region Overrides of Popup
        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown" /> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            bool isOpen = this.IsOpen;
            base.OnPreviewMouseLeftButtonDown(e);

            if (isOpen && !this.IsOpen)
                e.Handled = true;
        }
        #endregion
    }
}
