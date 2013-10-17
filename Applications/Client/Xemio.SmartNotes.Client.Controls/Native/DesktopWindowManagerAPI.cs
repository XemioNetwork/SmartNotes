using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Xemio.SmartNotes.Client.Controls.Native.Resources;

namespace Xemio.SmartNotes.Client.Controls.Native
{
    public static class DesktopWindowManagerAPI
    {
        /// <summary>
        /// Makes the whole window glassy.
        /// </summary>
        /// <param name="window">The window.</param>
        public static void AllGlassWindow(this Window window)
        {
            ExtendFrameIntoClientArea(window, new Thickness(-1), false);
        }
        /// <summary>
        /// Extends the glass frame into the client area.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="thickness">The thickness.</param>
        public static void ExtendFrameIntoClientArea(this Window window, Thickness thickness)
        {
            ExtendFrameIntoClientArea(window, thickness, false);
        }
        /// <summary>
        /// Extends glass frame into the client area.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="thickness">The thickness of the glass frame.</param>
        /// <param name="exceptionOnFail">if set to <c>true</c> exceptions will be thrown.</param>
        public static void ExtendFrameIntoClientArea(this Window window, Thickness thickness, bool exceptionOnFail)
        {
            var compEnabled = IsCompositionEnabled();
            if (exceptionOnFail && !compEnabled)
                throw new InvalidOperationException(ExceptionMessages.DesktopWindowManagerUnavailable);

            if (exceptionOnFail && !window.IsInitialized)
                throw new InvalidOperationException(ExceptionMessages.UninitializedWindow);

            if (!compEnabled)
                return;

            var margins = thickness.ToDWMMargins();
            var windowPointer = new WindowInteropHelper(window).Handle;

            if (windowPointer == IntPtr.Zero)
                return;

            //convert the background to nondrawing
            var mainWindowHwnd = HwndSource.FromHwnd(windowPointer);
            if (mainWindowHwnd != null)
                mainWindowHwnd.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

            try
            {
                DwmExtendFrameIntoClientArea(windowPointer, ref margins);
            }
            catch (DllNotFoundException)
            {
                window.Background = Brushes.White;
            }
        }
        /// <summary>
        /// Determines whether [is composition enabled].
        /// </summary>
        public static bool IsCompositionEnabled()
        {
            try
            {
                return DwmIsCompositionEnabled();
            }
            catch (DllNotFoundException)
            {
                return false;
            }
        }
        #region WPF to Native
        /// <summary>
        /// Converts the given <paramref name="t"/> in an <see cref="DWMMargins"/> .
        /// </summary>
        /// <param name="t">The t.</param>
        private static DWMMargins ToDWMMargins(this Thickness t)
        {
            var rtrn = new DWMMargins
                           {
                               Top = (int)t.Top,
                               Bottom = (int)t.Bottom,
                               Left = (int)t.Left,
                               Right = (int)t.Right
                           };

            return rtrn;
        }
        #endregion

        #region Native Interop
        [StructLayout(LayoutKind.Sequential)]
        private struct DWMMargins
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }
        /// <summary>
        /// Extends an hwind's frame into the client area by the specified margins.
        /// </summary>
        /// <param name="hwnd">Integer pointer to the window to change the glass area on.</param>
        /// <param name="margins">Margins, what to set each side to</param>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref DWMMargins margins);
        /// <summary>
        /// Checks to see if the Desktop window manager is enabled.
        /// </summary>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern bool DwmIsCompositionEnabled();
        #endregion
    }
}
