using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Website.Controllers.Authentication
{
    public sealed class AuthenticateRequestHandler : DelegatingHandler
    {
        public static readonly ILookup<string, string> ConfiguredAuthenticationKeys;

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
                        return Enumerable.Empty<KeyValuePair<string, string>>();
                    }

                    var key = keyHosts[0];
                    var hosts = keyHosts[1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    return hosts.Select(host => new KeyValuePair<string, string>(key, host));
                })
                .ToLookup(kvp => kvp.Key, kvp => kvp.Value, StringComparer.Ordinal);
        }

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