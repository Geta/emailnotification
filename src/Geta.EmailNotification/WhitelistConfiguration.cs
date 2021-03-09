using System.Configuration;
using System.Linq;

namespace Geta.EmailNotification
{
    public class WhitelistConfiguration
    {
        private string[] _emails = new string[0];
        private string[] _domains = new string[0];
        private bool _initialized;

        public string[] Emails
        {
            get
            {
                if (!_initialized)
                {
                    Initialize();
                }
                return _emails;
            }
        }

        public string[] Domains
        {
            get
            {
                if (!_initialized)
                {
                    Initialize();
                }
                return _domains;
            }
        }

        public bool HasWhitelist => Emails.Any() || Domains.Any();

        private void Initialize()
        {
            var config = ConfigurationManager.AppSettings["EmailNotification:Whitelist"];

            if (string.IsNullOrEmpty(config))
            {
                _initialized = true;
                return;
            }

            var items = config.Split(';');
            if (items.Length == 0)
            {
                _initialized = true;
                return;
            }

            _domains = items.Where(x => x.StartsWith("@")).ToArray();
            _emails = items.Where(x => !x.StartsWith("@")).ToArray();

            _initialized = true;
        }
    }
}