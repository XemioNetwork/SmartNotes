using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Xemio.SmartNotes.Client.Windows.Extensions;

namespace Xemio.SmartNotes.Client.Windows.Themes.ResourceDictionaries.Brushes
{
    partial class DefaultColorTheme : ResourceDictionary
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultColorTheme"/> class.
        /// </summary>
        public DefaultColorTheme()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the darker blue color brush.
        /// </summary>
        public SolidColorBrush DarkerBlueColorBrush
        {
            get { return this.GetNamedResource<SolidColorBrush>(); }
        }
        #endregion
    }
}
