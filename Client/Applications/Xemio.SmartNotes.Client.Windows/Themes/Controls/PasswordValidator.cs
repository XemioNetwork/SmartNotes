using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Castle.MicroKernel.Registration;

namespace Xemio.SmartNotes.Client.Windows.Themes.Controls
{
    /// <summary>
    /// A custom control providing a visual feedback of the password security.
    /// </summary>
    [TemplatePart(Name = TextBlockPart, Type = typeof(TextBlock))]
    [TemplatePart(Name = TitleTextBlockPart, Type = typeof(TextBlock))]
    [TemplateVisualState(Name = BlankState, GroupName = PasswordScoreStatesGroup)]
    [TemplateVisualState(Name = VeryWeakState, GroupName = PasswordScoreStatesGroup)]
    [TemplateVisualState(Name = WeakState, GroupName = PasswordScoreStatesGroup)]
    [TemplateVisualState(Name = MediumState, GroupName = PasswordScoreStatesGroup)]
    [TemplateVisualState(Name = StrongState, GroupName = PasswordScoreStatesGroup)]
    [TemplateVisualState(Name = VeryStrongState, GroupName = PasswordScoreStatesGroup)]
    public class PasswordValidator : Control
    {
        #region Constants
        private const string TextBlockPart = "PART_TextBlock";
        private const string TitleTextBlockPart = "PART_TitleTextBlock";

        private const string PasswordScoreStatesGroup = "PasswordScoreStates";
        private const string BlankState = "Blank";
        private const string VeryWeakState = "VeryWeak";
        private const string WeakState = "Weak";
        private const string MediumState = "Medium";
        private const string StrongState = "Strong";
        private const string VeryStrongState = "VeryStrong";
        #endregion

        #region Fields
        private TextBlock _textBlock;
        private TextBlock _titleTextBlock;
        #endregion

        #region Properties
        /// <summary>
        /// The dependency property of the <see cref="Password"/> property.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            "Password", typeof(string), typeof(PasswordValidator),
            new PropertyMetadata(default(string), PasswordPropertyChanged));
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="Score"/> property.
        /// </summary>
        public static readonly DependencyProperty ScoreProperty = DependencyProperty.Register(
            "Score", typeof(PasswordScore), typeof(PasswordValidator),
            new PropertyMetadata(PasswordScore.Blank, ScorePropertyChanged));
        /// <summary>
        /// Gets or sets the password score.
        /// </summary>
        public PasswordScore Score
        {
            get { return (PasswordScore)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="TitleMessage"/> property.
        /// </summary>
        public static readonly DependencyProperty TitleMessageProperty = DependencyProperty.Register(
            "TitleMessage", typeof(string), typeof(PasswordValidator),
            new PropertyMetadata("Password strength"));
        /// <summary>
        /// Gets or sets the title message.
        /// </summary>
        public string TitleMessage
        {
            get { return (string)GetValue(TitleMessageProperty); }
            set { SetValue(TitleMessageProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="BlankMessage"/> property.
        /// </summary>
        public static readonly DependencyProperty BlankMessageProperty = DependencyProperty.Register(
            "BlankMessage", typeof(string), typeof(PasswordValidator),
            new PropertyMetadata(""));
        /// <summary>
        /// Gets or sets the "Blank" message.
        /// </summary>
        public string BlankMessage
        {
            get { return (string)GetValue(BlankMessageProperty); }
            set { SetValue(BlankMessageProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="VeryWeakMessage"/> property.
        /// </summary>
        public static readonly DependencyProperty VeryWeakMessageProperty = DependencyProperty.Register(
            "VeryWeakMessage", typeof(string), typeof(PasswordValidator),
            new PropertyMetadata("Very weak"));
        /// <summary>
        /// Gets or sets the "VeryWeak" message.
        /// </summary>
        public string VeryWeakMessage
        {
            get { return (string)GetValue(VeryWeakMessageProperty); }
            set { SetValue(VeryWeakMessageProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="WeakMessage"/> property.
        /// </summary>
        public static readonly DependencyProperty WeakMessageProperty = DependencyProperty.Register(
            "WeakMessage", typeof(string), typeof(PasswordValidator),
            new PropertyMetadata("Weak"));
        /// <summary>
        /// Gets or sets the "Weak" message.
        /// </summary>
        public string WeakMessage
        {
            get { return (string)GetValue(WeakMessageProperty); }
            set { SetValue(WeakMessageProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="MediumMessage"/> property.
        /// </summary>
        public static readonly DependencyProperty MediumMessageProperty = DependencyProperty.Register(
            "MediumMessage", typeof(string), typeof(PasswordValidator),
            new PropertyMetadata("Medium"));
        /// <summary>
        /// Gets or sets the "Medium" message.
        /// </summary>
        public string MediumMessage
        {
            get { return (string)GetValue(MediumMessageProperty); }
            set { SetValue(MediumMessageProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="StrongMessage"/> property.
        /// </summary>
        public static readonly DependencyProperty StrongMessageProperty = DependencyProperty.Register(
            "StrongMessage", typeof(string), typeof(PasswordValidator),
            new PropertyMetadata("Strong"));
        /// <summary>
        /// Gets or sets the "Strong" message.
        /// </summary>
        public string StrongMessage
        {
            get { return (string)GetValue(StrongMessageProperty); }
            set { SetValue(StrongMessageProperty, value); }
        }
        /// <summary>
        /// The dependency property of the <see cref="VeryStrongMessage"/> property.
        /// </summary>
        public static readonly DependencyProperty VeryStrongMessageProperty = DependencyProperty.Register(
            "VeryStrongMessage", typeof(string), typeof(PasswordValidator),
            new PropertyMetadata("Very strong"));
        /// <summary>
        /// Gets or sets the "VeryStrong" message.
        /// </summary>
        public string VeryStrongMessage
        {
            get { return (string)GetValue(VeryStrongMessageProperty); }
            set { SetValue(VeryStrongMessageProperty, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="PasswordValidator"/> class.
        /// </summary>
        static PasswordValidator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordValidator), new FrameworkPropertyMetadata(typeof(PasswordValidator)));
        }
        #endregion

        #region Overrides of UserControl
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._textBlock = (TextBlock)this.Template.FindName(TextBlockPart, this);
            this._titleTextBlock = (TextBlock)this.Template.FindName(TitleTextBlockPart, this);

            this._titleTextBlock.Text = this.TitleMessage;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Validates the password.
        /// </summary>
        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(this.Password))
            {
                this.Score = PasswordScore.Blank;
                return;
            }

            if (this.Password.Length < 4)
            {
                this.Score = PasswordScore.VeryWeak;
                return;
            }

            bool atleast8CharsLong = this.Password.Length >= 8;
            bool hasDigit = Regex.IsMatch(this.Password, @"\d");
            bool hasSpecialCharacter = Regex.IsMatch(this.Password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]");

            int score = 2;

            if (atleast8CharsLong)
                score++;

            if (hasDigit)
                score++;

            if (hasSpecialCharacter)
                score++;

            if (this.Password.Length > 14)
                score++;

            if (this.Password.Length > 20)
                score++;

            this.Score = (PasswordScore)Math.Min(score, 5);
        }
        /// <summary>
        /// Updates the visual state.
        /// </summary>
        private void UpdateVisualState()
        {
            switch (this.Score)
            {
                case PasswordScore.Blank:
                    VisualStateManager.GoToState(this, BlankState, true);
                    this._textBlock.Text = this.BlankMessage;
                    break;
                case PasswordScore.VeryWeak:
                    VisualStateManager.GoToState(this, VeryWeakState, true);
                    this._textBlock.Text = this.VeryWeakMessage;
                    break;
                case PasswordScore.Weak:
                    VisualStateManager.GoToState(this, WeakState, true);
                    this._textBlock.Text = this.WeakMessage;
                    break;
                case PasswordScore.Medium:
                    VisualStateManager.GoToState(this, MediumState, true);
                    this._textBlock.Text = this.MediumMessage;
                    break;
                case PasswordScore.Strong:
                    VisualStateManager.GoToState(this, StrongState, true);
                    this._textBlock.Text = this.StrongMessage;
                    break;
                case PasswordScore.VeryStrong:
                    VisualStateManager.GoToState(this, VeryStrongState, true);
                    this._textBlock.Text = this.VeryStrongMessage;
                    break;
                default:
                    VisualStateManager.GoToState(this, BlankState, true);
                    this._textBlock.Text = this.BlankMessage;
                    break;
            }
        }
        /// <summary>
        /// Called when the <see cref="Password"/> property changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PasswordPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            PasswordValidator validator = (PasswordValidator)dependencyObject;
            validator.ValidatePassword();
        }
        /// <summary>
        /// Called when the <see cref="Score"/> property changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ScorePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            PasswordValidator validator = (PasswordValidator)dependencyObject;
            validator.UpdateVisualState();
        }
        #endregion
    }
}
