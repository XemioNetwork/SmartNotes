using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Common
{
    public static class Singleton<T> where T : new()
    {
        #region Fields
        private static readonly Lazy<T> _instance = new Lazy<T>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static T Instance
        {
            get { return _instance.Value; }
        }
        #endregion
    }
}
