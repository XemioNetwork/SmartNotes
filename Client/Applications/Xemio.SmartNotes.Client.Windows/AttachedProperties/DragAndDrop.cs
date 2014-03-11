using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xemio.SmartNotes.Client.Windows.Extensions;

namespace Xemio.SmartNotes.Client.Windows.AttachedProperties
{
    public static class DragAndDrop
    {
        #region Properties
        /// <summary>
        /// The attached property indicating whether automatic scrolling is enabled.
        /// </summary>
        public static readonly DependencyProperty IsAutoScrollingEnabledProperty = DependencyProperty.RegisterAttached(
            "IsAutoScrollingEnabled", typeof (bool), typeof (DragAndDrop), 
            new PropertyMetadata(default(bool), IsAutoScrollingEnabledChanged));
        /// <summary>
        /// The setter for the attached property "IsAutoScrollingEnabled".
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetIsAutoScrollingEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsAutoScrollingEnabledProperty, value);
        }
        /// <summary>
        /// The getter for the attached property "IsAutoScrollingEnabled".
        /// </summary>
        /// <param name="element">The element.</param>
        public static bool GetIsAutoScrollingEnabled(DependencyObject element)
        {
            return (bool) element.GetValue(IsAutoScrollingEnabledProperty);
        }
        /// <summary>
        /// The attached property declaring the offset you scroll automatically.
        /// </summary>
        public static readonly DependencyProperty AutoScrollingOffsetProperty = DependencyProperty.RegisterAttached(
            "AutoScrollingOffset", typeof (double), typeof (DragAndDrop), 
            new PropertyMetadata(3.0));
        /// <summary>
        /// The setter for the attached property "AutoScrollingOffset".
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoScrollingOffset(DependencyObject element, double value)
        {
            element.SetValue(AutoScrollingOffsetProperty, value);
        }
        /// <summary>
        /// The getter for the attached property "AutoScrollingOffset".
        /// </summary>
        /// <param name="element">The element.</param>
        public static double GetAutoScrollingOffset(DependencyObject element)
        {
            return (double) element.GetValue(AutoScrollingOffsetProperty);
        }
        /// <summary>
        /// The attached property declaring the distance you need to start scrolling automatically.
        /// </summary>
        public static readonly DependencyProperty AutoScrollingDistanceProperty = DependencyProperty.RegisterAttached(
            "AutoScrollingDistance", typeof (double), typeof (DragAndDrop), 
            new PropertyMetadata(10.0));
        /// <summary>
        /// The setter for the attached property "AutoScrollingDistance".
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetAutoScrollingDistance(DependencyObject element, double value)
        {
            element.SetValue(AutoScrollingDistanceProperty, value);
        }
        /// <summary>
        /// The getter for the attached property "AutoScrollingDistance".
        /// </summary>
        /// <param name="element">The element.</param>
        public static double GetAutoScrollingDistance(DependencyObject element)
        {
            return (double) element.GetValue(AutoScrollingDistanceProperty);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when the attached property "IsAutoScrollingEnabled" has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void IsAutoScrollingEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Control control = sender as Control;
            if (control == null)
                throw new InvalidOperationException("The attached property 'AutoScrollingDistance' is only valid on objects of type 'Control'.");

            if ((bool)e.NewValue)
                control.DragOver += ControlOnDragOver;
            else
                control.DragOver -= ControlOnDragOver;
        }
        /// <summary>
        /// Called when you drag something over the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private static void ControlOnDragOver(object sender, DragEventArgs e)
        {
            var control = (Control)sender;
            var scrollViewer = VisualTree.FindChildControl<ScrollViewer>(control);

            double verticalPosition = e.GetPosition(scrollViewer).Y;
            double distanceForScrolling = GetAutoScrollingDistance(control);
            double offset = GetAutoScrollingOffset(control);

            if (verticalPosition < distanceForScrolling)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
            }
            else if (verticalPosition > control.ActualHeight - distanceForScrolling)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
            }
        }
        #endregion
    }
}
