using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Xemio.SmartNotes.Client.Views.Header
{
    /// <summary>
    /// Interaction logic for HeaderView.xaml
    /// </summary>
    public partial class HeaderView : UserControl
    {
        public HeaderView()
        {
            InitializeComponent();
        }

        private void UserIconClick(object sender, RoutedEventArgs e)
        {
            userActionsPopup.IsOpen = true;
        }
    }
}
