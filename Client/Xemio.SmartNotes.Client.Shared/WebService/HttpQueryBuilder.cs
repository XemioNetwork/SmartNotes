using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Shared.WebService
{
    public class HttpQueryBuilder
    {
        private readonly StringBuilder _queryStringBuilder;

        public HttpQueryBuilder()
        {
            this._queryStringBuilder = new StringBuilder();
        }

        public void AddParameter(string key, object value)
        {
            if (this._queryStringBuilder.Length > 0)
                this._queryStringBuilder.Append("&");

            this._queryStringBuilder.AppendFormat("{0}={1}", key, value);
        }

        public override string ToString()
        {
            return this._queryStringBuilder.ToString();
        }
    }
}
