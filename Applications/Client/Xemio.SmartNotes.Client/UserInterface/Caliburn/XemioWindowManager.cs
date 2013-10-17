using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Controls.Controls;
using Xemio.SmartNotes.Client.Controls.Native;
using Xemio.SmartNotes.Client.UserInterface.Images;

namespace Xemio.SmartNotes.Client.UserInterface.Caliburn
{
    public class XemioWindowManager : WindowManager
    {
        /// <summary>
        /// Ensures the window.
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
                    Icon = new BitmapImage(ImagePaths.Icon)
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
    }
}
