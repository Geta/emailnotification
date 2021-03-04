using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Geta.EmailNotification.Core
{
    public class WhitelistConfiguration
    {
        private readonly string[] _emails;
        private readonly string[] _domains;

        public string[] Emails => _emails;

        public string[] Domains => _domains;

        public bool HasWhitelist => Emails.Any() || Domains.Any();

        public WhitelistConfiguration(IConfiguration configuration)
        {
            var config = configuration["EmailNotification:Whitelist"];

            var items = config?.Split(';');
            if (items == null || items.Length == 0)
            {
                _emails = new string[0];
                _domains = new string[0];
                return;
            }
            
            _domains = items.Where(x => x.StartsWith("@")).ToArray();
            _emails = items.Where(x => !x.StartsWith("@")).ToArray();
        }
    }
}