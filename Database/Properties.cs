using System.Linq;

namespace Database
{
    public class Properties
    {
        public string UpgradeConnectionString { get; set; }
        public string MasterConnectionString { get; set; }
        public string DatabaseFileLocation { get; set; }
        public string DatabaseName { get; set; }

        public string Quiet { get; set; }
        public string Drop { get; set; }

        public bool IsQuiet
        {
            get { return ParseArgAsBool(Quiet); }
        }
        public bool IsDrop
        {
            get { return ParseArgAsBool(Drop); }
        }

        private static bool ParseArgAsBool(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return false;
            arg = arg.ToLower();
            return (new[] { "y", "yes", "true", "t", "1", "q" }).Contains(arg);
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(UpgradeConnectionString) &&
                   !string.IsNullOrEmpty(MasterConnectionString) &&
                   !string.IsNullOrEmpty(DatabaseFileLocation) &&
                   !string.IsNullOrEmpty(DatabaseName);
        }
    }
}
