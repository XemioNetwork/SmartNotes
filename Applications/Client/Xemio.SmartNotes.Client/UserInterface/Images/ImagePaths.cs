using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Xemio.SmartNotes.Client.UserInterface.Images
{
    public static class ImagePaths
    {
        public static Uri Icon
        {
            get { return new Uri("pack://application:,,,/UserInterface/Images/Icon.png"); }
        }

        public static Uri Logo
        {
            get { return new Uri("pack://application:,,,/UserInterface/Images/Logo.png"); }
        }

        public static Uri DefaultUserIcon
        {
            get { return new Uri("pack://application:,,,/UserInterface/Images/DefaultUserIcon.png"); }
        }
    }
}
