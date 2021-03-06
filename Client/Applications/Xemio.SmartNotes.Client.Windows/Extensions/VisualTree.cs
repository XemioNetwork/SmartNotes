﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Xemio.SmartNotes.Client.Windows.Extensions
{
    public static class VisualTree
    {
        /// <summary>
        /// Finds the child control of the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the child control.</typeparam>
        /// <param name="control">The control.</param>
        public static T FindChildControl<T>(DependencyObject control)
            where T : DependencyObject
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                if (child is T)
                    return (T)child;
                
                return VisualTree.FindChildControl<T>(child);
            }
            return null;
        }

        /// <summary>
        /// Finds the parent control of the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parent control.</typeparam>
        /// <param name="control">The control.</param>
        public static T FindParentControl<T>(DependencyObject control)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(control);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }        
            return (T)parent;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="child"/> is a child of the specified <paramref name="parent"/>.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="parent">The parent.</param>
        public static bool IsChildOf(DependencyObject child, DependencyObject parent)
        {
            DependencyObject foundParent = VisualTreeHelper.GetParent(child);
            while (foundParent != null && foundParent != parent)
            {
                foundParent = VisualTreeHelper.GetParent(foundParent);
            }

            return foundParent != null;
        }
    }
}
