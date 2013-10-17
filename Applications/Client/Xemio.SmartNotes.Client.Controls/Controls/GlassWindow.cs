using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Xemio.SmartNotes.Client.Controls.Native;

namespace Xemio.SmartNotes.Client.Controls.Controls
{
    public class GlassWindow : Window
    {
        #region Properties
        /// <summary>
        /// The dependency property of the <see cref="GlassThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty GlassThicknessProperty = DependencyProperty.Register(
            "GlassThickness", typeof(Thickness), typeof(GlassWindow),
            new PropertyMetadata(new Thickness(0, 0, 0, 0), GlassThicknessChanged));
        /// <summary>
        /// Gets or sets the glass thickness.
        /// </summary>
        public Thickness GlassThickness
        {
            get { return (Thickness)GetValue(GlassThicknessProperty); }
            set { SetValue(GlassThicknessProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="IsAllGlass"/> property.
        /// </summary>
        public static readonly DependencyProperty IsAllGlassProperty = DependencyProperty.Register(
            "IsAllGlass", typeof(bool), typeof(GlassWindow),
            new PropertyMetadata(OnIsAllGlassChanged));
        /// <summary>
        /// Gets or sets a value indicating whether the whole window is glassy.
        /// </summary>
        public bool IsAllGlass
        {
            get { return (bool)GetValue(IsAllGlassProperty); }
            set { SetValue(IsAllGlassProperty, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="GlassWindow"/> class.
        /// </summary>
        static GlassWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlassWindow), new FrameworkPropertyMetadata(typeof(GlassWindow)));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when the <see cref="GlassThickness"/> property changed.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void GlassThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GlassWindow)d).UpdateGlassState();
        }
        /// <summary>
        /// Calles when the <see cref="IsAllGlass"/> property changed.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsAllGlassChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GlassWindow)d).UpdateGlassState();
        }
        /// <summary>
        /// Updates the state of the glass.
        /// </summary>
        private void UpdateGlassState()
        {
            if (!IsInitialized) return;

            if (IsAllGlass)
                this.AllGlassWindow();
            else
                this.ExtendFrameIntoClientArea(GlassThickness);
        }
        #endregion

        #region Overrides of Window
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.SourceInitialized" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            UpdateGlassState();
        }
        #endregion
    }
}
