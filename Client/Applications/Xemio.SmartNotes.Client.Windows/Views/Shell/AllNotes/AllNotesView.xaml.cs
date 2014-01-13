using System.Windows;
using System.Windows.Controls;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.AllNotes
{
    /// <summary>
    /// Interaction logic for AllNotesView.xaml
    /// </summary>
    public partial class AllNotesView : UserControl
    {
        public AllNotesViewModel ViewModel
        {
            get { return (AllNotesViewModel)this.DataContext; }
        }

        public AllNotesView()
        {
            InitializeComponent();
        }
    }
}
