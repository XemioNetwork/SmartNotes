using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Windows.Extensions;

namespace Xemio.SmartNotes.Client.Windows.Themes.Controls
{
    [TemplatePart(Name = TextBoxPart, Type = typeof(WatermarkTextBox))]
    [TemplatePart(Name = ButtonPart, Type = typeof(Button))]
    public class TagListViewItem : ListViewItem
    {
        #region Constants
        private const string TextBoxPart = "PART_TextBox";
        private const string ButtonPart = "PART_Button";
        #endregion

        #region Fields
        private WatermarkTextBox _textBox;
        private Button _button;
        #endregion

        #region Properties
        /// <summary>
        /// The dependency property of the <see cref="TagWatermark"/> property.
        /// </summary>
        public static readonly DependencyProperty TagWatermarkProperty = DependencyProperty.Register(
            "TagWatermark", typeof (string), typeof (TagListViewItem), 
            new PropertyMetadata(default(string), OnTagWatermarkChanged));
        /// <summary>
        /// Gets or sets the tag watermark.
        /// </summary>
        public string TagWatermark
        {
            get { return (string) GetValue(TagWatermarkProperty); }
            set { SetValue(TagWatermarkProperty, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="TagListViewItem"/> class.
        /// </summary>
        static TagListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TagListViewItem), new FrameworkPropertyMetadata(typeof(TagListViewItem)));
        }
        #endregion

        #region Overrides of FrameworkElement
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this._textBox != null)
                this._textBox.TextChanged -= this.TextBoxOnTextChanged;

            if (this._button != null)
                this._button.Click -= this.ButtonOnClick;

            this._textBox = (WatermarkTextBox) this.Template.FindName(TextBoxPart, this);
            this._textBox.TextChanged += this.TextBoxOnTextChanged;
            this._textBox.Watermark = this.TagWatermark;
            this._textBox.Text = (string)this.Content;

            this._button = (Button)this.Template.FindName(ButtonPart, this);
            this._button.Click += this.ButtonOnClick;
        }
        #endregion

        #region Overrides of ContentControl
        /// <summary>
        /// Called when the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
        /// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if (this._textBox == null)
                return;

            if (this.Content is string == false)
                return;

            string tag = (string) this.Content;
            if (this._textBox.Text != tag)
                this._textBox.Text = tag;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when the text in the textbox changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="textChangedEventArgs">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string tag = (string) this.Content;

            if (tag != this._textBox.Text)
                this.Content = this._textBox.Text;
        }
        /// <summary>
        /// Called when the user clicks the "delete" button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var listView = VisualTree.FindParentControl<TagListView>(this);

            if (listView.ItemsSource is IList == false)
                return;

            var items = (IList) listView.ItemsSource;
            items.Remove(this.Content);
        }
        /// <summary>
        /// Called when the <see cref="TagWatermark"/> property changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTagWatermarkChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var listViewItem = (TagListViewItem) dependencyObject;
            listViewItem._textBox.Watermark = listViewItem.TagWatermark;
        }
        #endregion
    }
}
