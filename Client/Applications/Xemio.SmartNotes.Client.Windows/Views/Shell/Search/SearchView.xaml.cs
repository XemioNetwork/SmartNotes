using System.Windows.Controls;
using System.Windows.Input;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.Search
{
    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private SearchViewModel ViewModel
        {
            get { return (SearchViewModel)this.DataContext; }
        }

        private void SearchTextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ViewModel.Search();
            }
        }
    }
}
