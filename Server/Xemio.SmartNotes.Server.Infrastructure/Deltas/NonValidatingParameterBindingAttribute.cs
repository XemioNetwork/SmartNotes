using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Xemio.SmartNotes.Server.Infrastructure.Deltas
{
    internal sealed class NonValidatingParameterBindingAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            IEnumerable<MediaTypeFormatter> formatters = parameter.Configuration.Formatters;

            return new NonValidatingParameterBinding(parameter, formatters);
        }

        private sealed class NonValidatingParameterBinding : PerRequestParameterBinding
        {
            public NonValidatingParameterBinding(HttpParameterDescriptor descriptor, IEnumerable<MediaTypeFormatter> formatters)
                : base(descriptor, formatters)
            {
            }

            protected override HttpParameterBinding CreateInnerBinding(IEnumerable<MediaTypeFormatter> perRequestFormatters)
            {
                return Descriptor.BindWithFormatter(perRequestFormatters, null);
            }
        }
    }
}
