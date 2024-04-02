using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using IniFile;

namespace ShockedIsaac
{
    public class Configuration
    {
        static string settingsFile = "Settings.ini";

        private static Ini getIni() 
        {
            if (!File.Exists(settingsFile))
                File.Create(settingsFile).Close();

            return new Ini(settingsFile);
        }

        private static void write(string section, string name, string value) 
        {
            var ini = getIni();

            if (!ini.Any(s => s.Name == section))
                ini.Add(new Section(section));

            ini[section][name] = value;

            ini.SaveTo(settingsFile);
        }

        /// <summary>
        /// reads a value from a specific section.
        /// </summary>
        /// <param name="section">Section, which contains the name</param>
        /// <param name="name">The name of the setting</param>
        /// <returns>
        /// null when the section and name wasn't found.
        /// else the value
        /// </returns>
        private static string read(string section, string name) 
        {
            var ini = getIni();

            if (!ini.Any(s => s.Name == section))
                return null;

            return ini[section][name];
        }

        public static void EnsureDefaults() 
        {
            if (string.IsNullOrEmpty(ApiKey)) ApiKey = "YOUR_API_KEY_HERE";
            if (string.IsNullOrEmpty(OnHitDuration)) OnHitDuration = "YOUR_API_KEY_HERE";
            if (string.IsNullOrEmpty(OnHitIntensity)) OnHitIntensity = "YOUR_API_KEY_HERE";
            if (string.IsNullOrEmpty(OnIntentionalHitDuration)) OnIntentionalHitDuration = "YOUR_API_KEY_HERE";
            if (string.IsNullOrEmpty(OnIntentionalHitIntensity)) OnIntentionalHitIntensity = "YOUR_API_KEY_HERE";
        }

        public static string ApiKey
        {
            get => read("Settings", "apiKey");
            set => write("Settings", "apiKey", value);
        }
        
        public static string OnHitDuration
        {
            get => read("OnHit", "duration");
            set => write("OnHit", "duration", value);
        }

        public static string OnHitIntensity
        {
            get => read("OnHit", "intensity");
            set => write("OnHit", "intensity", value);
        }

        public static string OnIntentionalHitDuration
        {
            get => read("OnIntentionalHit", "duration");
            set => write("OnIntentionalHit", "duration", value);
        }

        public static string OnIntentionalHitIntensity
        {
            get => read("OnIntentionalHit", "intensity");
            set => write("OnIntentionalHit", "intensity", value);
        }

        public static string OnDeathIntensity
        {
            get => read("OnDeath", "intensity");
            set => write("OnDeath", "intensity", value);
        }

        public static string OnDeathDuration
        {
            get => read("OnDeath", "duration");
            set => write("OnDeath", "duration", value);
        }
    }
}