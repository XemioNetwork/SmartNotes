using System.Windows;
using System.Windows.Controls;

namespace Xemio.SmartNotes.Client.UserInterface.Views.Header
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
