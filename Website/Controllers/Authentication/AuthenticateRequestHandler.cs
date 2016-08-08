using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Website.Controllers.Authentication
{
    public sealed class AuthenticateRequestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IEnumerable<string> keys;
            if (request.Headers.TryGetValues("X-ConsoleApplication3-Key", out keys))
            {
                var firstKey = keys.FirstOrDefault();

            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}