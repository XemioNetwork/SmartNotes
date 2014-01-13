using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Xemio.SmartNotes.Client.Windows.Themes.Controls
{
    /// <summary>
    /// A subclass of <see cref="TextBox"/> providing a watermark.
    /// </summary>
    [TemplateVisualState(Name = WatermarkVisibleState, GroupName = WatermarkStatesGroup)]
    [TemplateVisualState(Name = WatermarkHiddenState, GroupName = WatermarkStatesGroup)]
    public class WatermarkTextBox : TextBox
    {
        #region Constants
        private const string WatermarkStatesGroup = "WatermarkStates";
        private const string WatermarkVisibleState = "WatermarkVisible";
        private const string WatermarkHiddenState = "WatermarkHidden";
        #endregion
        
        #region Properties
        /// <summary>
        /// The dependency property of the <see cref="Watermark"/> property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark", typeof(string), typeof(WatermarkTextBox),
            new PropertyMetadata(default(string)));
        /// <summary>
        /// Gets or sets the watermark.
        /// </summary>
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="WatermarkTextBox"/> class.
        /// </summary>
        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkTextBox"/> class.
        /// </summary>
        public WatermarkTextBox()
        {
            this.TextChanged += this.OnTextChanged;
            this.IsVisibleChanged += async (s, e) =>
            {
                //For some reason it doesn't work synchronously, so we use a very low delay to update the visual state.
                await Task.Delay(1);
                this.OnTextChanged(this, null);
            };
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called by the <see cref="TextBox.TextChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="textChangedEventArgs">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (this.Text.Length == 0)
            {
                VisualStateManager.GoToState(this, WatermarkVisibleState, true);
            }
            else
            {
                VisualStateManager.GoToState(this, WatermarkHiddenState, true);
            }
        }
        #endregion
    }
}
