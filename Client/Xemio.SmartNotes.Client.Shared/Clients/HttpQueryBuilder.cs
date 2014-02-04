using System.Text;

namespace Xemio.SmartNotes.Client.Shared.Clients
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
            bool firstParameter = this._queryStringBuilder.Length == 0;
            this._queryStringBuilder.Append(firstParameter ? "?" : "&");

            this._queryStringBuilder.AppendFormat("{0}={1}", key, value);
        }

        public override string ToString()
        {
            return this._queryStringBuilder.ToString();
        }
    }
}
