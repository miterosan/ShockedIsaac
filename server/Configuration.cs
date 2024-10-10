using IniFile;

namespace ShockedIsaac
{
    public class Configuration
    {
        private const string settingsFile = "settings.ini";

        public string ApiKey { get; set; }
        public int HitDuration { get; set; }
        public int HitStrength { get; set; }
        public int IntentionalHitDuration { get; set; }
        public int IntentionalHitStrength { get; set; }
        public string IntentionalDamageMode { get; set; }
        public int DeathStrength { get; set; }
        public int DeathDuration { get; set; }

        public string[] Player1Shockers { get; set; }
        public string[] Player2Shockers { get; set; }
        public string[] Player3Shockers { get; set; }
        public string[] Player4Shockers { get; set; }

        public Configuration()
        {
            LoadOrSetDefaults();
        }

        public void LoadOrSetDefaults()
        {
            if (!File.Exists(settingsFile))
                File.Create(settingsFile).Close();
            var ini = new Ini(settingsFile);

            var settingsSection = ensureAndGetSection(ini, "Settings");

            ApiKey = ensureAndGetPropertyValue(settingsSection, nameof(ApiKey), "");

            HitDuration = ensureAndGetPropertyValue(settingsSection, nameof(HitDuration), "1000");
            HitStrength = ensureAndGetPropertyValue(settingsSection, nameof(HitStrength), "30");

            IntentionalHitDuration = ensureAndGetPropertyValue(settingsSection, nameof(IntentionalHitDuration), "300");
            IntentionalHitStrength = ensureAndGetPropertyValue(settingsSection, nameof(IntentionalHitStrength), "50");

            IntentionalDamageMode = ensureAndGetPropertyValue(settingsSection, nameof(IntentionalDamageMode), "Shock");

            if (IntentionalDamageMode != "Shock" && IntentionalDamageMode != "Vibrate")
            {
                Console.WriteLine("IntentionalDamageMode set to unrecognised value. Possible are: Shock, Vibrate");
                Console.WriteLine("Defaulting to Vibrate...");
                IntentionalDamageMode = "Vibrate";
            }

            DeathStrength = ensureAndGetPropertyValue(settingsSection, nameof(DeathStrength), "70");
            DeathDuration = ensureAndGetPropertyValue(settingsSection, nameof(DeathDuration), "1500");



            const string shockersProperty = "Shockers";

            var player1Section = ensureAndGetSection(ini, "Player1");
            Player1Shockers = ensureAndGetPropertyValue(player1Section, shockersProperty, "").ToString().Split(",");

            var player2Section = ensureAndGetSection(ini, "Player2");
            Player2Shockers = ensureAndGetPropertyValue(player2Section, shockersProperty, "").ToString().Split(",");

            var player3Section = ensureAndGetSection(ini, "Player3");
            Player3Shockers = ensureAndGetPropertyValue(player3Section, shockersProperty, "").ToString().Split(",");

            var player4Section = ensureAndGetSection(ini, "Player4");
            Player4Shockers = ensureAndGetPropertyValue(player4Section, shockersProperty, "").ToString().Split(",");

            ini.SaveTo(settingsFile);
        }

        private Section ensureAndGetSection(Ini ini, string name)
        {
            var section = ini[name];
            if (section == null)
            {
                ini.Add(section = new Section(name));
            }

            return section;
        }

        private PropertyValue ensureAndGetPropertyValue(Section section, string name, string defaultValue)
        {
            var val = section[name];

            if (val.IsEmpty())
            {
                return section[name] = defaultValue;
            }

            return val.ToString();
        }
    }
}