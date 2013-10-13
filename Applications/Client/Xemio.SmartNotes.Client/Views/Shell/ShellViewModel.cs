using Caliburn.Micro;
using Xemio.SmartNotes.Client.Views.Header;

namespace Xemio.SmartNotes.Client.Views.Shell
{
    public class ShellViewModel : Screen
    {
        public ShellViewModel(HeaderViewModel headerViewModel)
        {
            this.Header = headerViewModel;
        }

        private HeaderViewModel _header;

        public HeaderViewModel Header
        {
            get { return this._header; }
            set
            {
                if (this._header != value)
                {
                    this._header = value;
                    this.NotifyOfPropertyChange(() => this.Header);
                }
            }
        }
    }
}
