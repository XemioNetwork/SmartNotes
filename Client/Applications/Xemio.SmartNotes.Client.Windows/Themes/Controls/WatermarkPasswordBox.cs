using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Xemio.SmartNotes.Client.Windows.Themes.Controls
{
    /// <summary>
    /// A custom <see cref="Control"/> wrapping a <see cref="PasswordBox"/> and adding the ability to display a watermark.
    /// </summary>
    [TemplatePart(Name = PasswordBoxPart, Type = typeof(PasswordBox))]
    [TemplateVisualState(Name = WatermarkHiddenState, GroupName = WatermarkStatesGroup)]
    [TemplateVisualState(Name = WatermarkVisibleState, GroupName = WatermarkStatesGroup)]
    public class WatermarkPasswordBox : Control
    {
        #region Constants
        private const string WatermarkStatesGroup = "WatermarkStates";
        private const string WatermarkHiddenState = "WatermarkHidden";
        private const string WatermarkVisibleState = "WatermarkVisible";

        private const string PasswordBoxPart = "PART_PasswordBox";
        #endregion

        #region Fields
        private PasswordBox _passwordBox;
        private bool _isWatermarkVisible = true;
        #endregion

        #region Properties
        /// <summary>
        /// The dependency property of the <see cref="Password"/> property.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            "Password", typeof(string), typeof(WatermarkPasswordBox),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PasswordPropertyChanged));
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="Watermark"/> property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark", typeof(string), typeof(WatermarkPasswordBox),
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
        /// Initializes the <see cref="WatermarkPasswordBox"/> class.
        /// </summary>
        static WatermarkPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkPasswordBox), new FrameworkPropertyMetadata(typeof(WatermarkPasswordBox)));
        }
        #endregion

        #region Overrides of UserControl
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this._passwordBox != null)
            {
                this._passwordBox.PasswordChanged -= PasswordBoxPasswordChanged;
            }

            this._passwordBox = (PasswordBox)Template.FindName(PasswordBoxPart, this);
            this._passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
        }
        /// <summary>
        /// Invoked whenever an unhandled <see cref="E:System.Windows.UIElement.GotFocus" /> event reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            Keyboard.Focus(this._passwordBox);
        }
        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.GotKeyboardFocus" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyboardFocusChangedEventArgs" /> that contains the event data.</param>
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            Keyboard.Focus(this._passwordBox);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when the <see cref="Password"/> property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WatermarkPasswordBox watermarkPasswordBox = (WatermarkPasswordBox)sender;

            string newPassword = (string)e.NewValue;

            if (watermarkPasswordBox._passwordBox.Password != newPassword)
                watermarkPasswordBox._passwordBox.Password = newPassword;

            watermarkPasswordBox.UpdateVisualState();
        }
        /// <summary>
        /// Called when the <see cref="PasswordBox.Password"/> property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (this.Password != this._passwordBox.Password)
                this.Password = this._passwordBox.Password;
        }
        /// <summary>
        /// Updates the visual state.
        /// </summary>
        private void UpdateVisualState()
        {
            if (string.IsNullOrWhiteSpace(this.Password) && this._isWatermarkVisible == false)
            {
                VisualStateManager.GoToState(this, WatermarkVisibleState, true);
                this._isWatermarkVisible = true;
            }

            if (string.IsNullOrWhiteSpace(this.Password) == false && this._isWatermarkVisible)
            {
                VisualStateManager.GoToState(this, WatermarkHiddenState, true);
                this._isWatermarkVisible = false;
            }
        }
        #endregion
    }
}
