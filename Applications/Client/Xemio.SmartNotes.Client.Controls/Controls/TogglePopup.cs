using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;

namespace Xemio.SmartNotes.Client.Controls.Controls
{
    public class TogglePopup : Popup
    {
        protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            bool isOpen = this.IsOpen;
            base.OnPreviewMouseLeftButtonDown(e);

            if (isOpen && !this.IsOpen)
                e.Handled = true;
        }
    }
}
