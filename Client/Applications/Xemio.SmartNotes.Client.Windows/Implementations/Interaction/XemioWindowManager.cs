using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Windows.Themes.Controls;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Interaction
{
    public class XemioWindowManager : WindowManager
    {
        #region Overrides of WindowManager
        /// <summary>
        /// Makes sure the view is a window or is wrapped by one.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="view">The view.</param>
        /// <param name="isDialog">if set to <c>true</c> [is dialog].</param>
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = view as Window;

            if (window == null)
            {
                window = new GlassWindow
                {
                    Content = view,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    IsAllGlass = true,
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Xemio.SmartNotes.Client.Shared;Component/Resources/Icons/AppIcon.png"))
                };

                window.SetValue(View.IsGeneratedProperty, true);

                var owner = InferOwnerOf(window);
                if (owner != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                var owner = InferOwnerOf(window);
                if (owner != null && isDialog)
                {
                    window.Owner = owner;
                }
            }

            return window;
        }
        #endregion
    }
}
