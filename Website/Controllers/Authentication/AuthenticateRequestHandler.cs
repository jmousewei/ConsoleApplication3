using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Website.Controllers.Authentication
{
    public sealed class AuthenticateRequestHandler : DelegatingHandler
    {
        public static readonly ILookup<string, Uri> ConfiguredAuthenticationKeys;

        static AuthenticateRequestHandler()
        {
            var configKey = $"{typeof (AuthenticateRequestHandler).FullName}.{nameof(ConfiguredAuthenticationKeys)}";
            var configured = ConfigurationManager.AppSettings[configKey] ?? string.Empty;

            // Get the configured authentication key-host pairs.
            // Format: {key1}|<host1>,<host2>,...,<hostN>;{key2}|<host1>,<host2>,...,<hostN>
            var entries = configured.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            ConfiguredAuthenticationKeys = entries.SelectMany(
                entry =>
                {
                    var keyHosts = entry.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
                    if (keyHosts.Length != 2)
                    {
                        // Ignore if unknown structure.
                        return Enumerable.Empty<KeyValuePair<string, Uri>>();
                    }

                    var key = keyHosts[0];
                    var hosts = keyHosts[1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

                    return hosts.Select(
                        host => new KeyValuePair<string, Uri>(key, new UriBuilder(Uri.UriSchemeHttp, host).Uri));
                })
                .ToLookup(kvp => kvp.Key, kvp => kvp.Value, StringComparer.Ordinal);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            do
            {
                if (request.Headers.Referrer == null)
                {
                    break;
                }

                IEnumerable<string> keys;
                if (!request.Headers.TryGetValues("X-ConsoleApplication3-Key", out keys))
                {
                    break;
                }

                var userKey = keys.FirstOrDefault();
                var isAuthenticatedReferrer =
                    ConfiguredAuthenticationKeys[userKey].Any(host => host.IsBaseOf(request.Headers.Referrer));
                if (!isAuthenticatedReferrer)
                {
                    break;
                }

                var claims =
                    new[]
                    {
                        new Claim(ClaimTypes.Role, "Consumer"),
                        new Claim(ClaimTypes.Sid, userKey),
                    };
                var identity = new ClaimsIdentity(claims, "Consumer");
                var principal = new ClaimsPrincipal(identity);

                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
            }
            while (false);

            return base.SendAsync(request, cancellationToken);
        }
    }
}