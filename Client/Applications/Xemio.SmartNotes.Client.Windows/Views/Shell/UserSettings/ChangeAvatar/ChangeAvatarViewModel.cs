using System;
using System.IO;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.UserSettings.ChangeAvatar
{
    public class ChangeAvatarViewModel : Screen
    {
        #region Fields
        private BitmapImage _currentAvatar;
        private BitmapImage _newAvatar;
        private string _filePath;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current avatar.
        /// </summary>
        public BitmapImage CurrentAvatar
        {
            get { return this._currentAvatar; }
            set
            {
                this._currentAvatar = value;
                this.NotifyOfPropertyChange(() => this.CurrentAvatar);
            }
        }
        /// <summary>
        /// Gets or sets the new avatar.
        /// </summary>
        public BitmapImage NewAvatar
        {
            get { return this._newAvatar; }
            set
            {
                this._newAvatar = value;
                this.NotifyOfPropertyChange(() => this.NewAvatar);
            }
        }
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath
        {
            get { return this._filePath; }
            set
            {
                this._filePath = value;
                this.NotifyOfPropertyChange(() => this.FilePath);

                if (File.Exists(this.FilePath))
                {
                    this.NewAvatar = new BitmapImage(new Uri(this.FilePath));
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Opens an OpenFileDialog so the user can select a new avatar.
        /// </summary>
        public void SelectFile()
        {
            this.FilePath = @"C:\Image.jpg";
        }
        #endregion
    }
}
