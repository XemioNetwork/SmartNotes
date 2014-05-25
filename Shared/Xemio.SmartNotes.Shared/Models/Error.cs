using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Xemio.SmartNotes.Shared.Models
{
    public class Error
    {
        public static Error Create(string message, JObject data = null)
        {
            return new Error
            {
                Message = message,
                AdditionalData = data
            };
        }

        public string Message { get; set; }
        public JObject AdditionalData { get; set; }


        #region Overrides of Object
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return this.Message;
        }
        #endregion
    }
}
