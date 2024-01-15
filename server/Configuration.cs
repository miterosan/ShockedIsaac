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



        public static string getApiKey() 
        {
            if (!File.Exists(settingsFile))
                File.Create(settingsFile).Close();

            var ini = new Ini(settingsFile);

            if (!ini.Any(s => s.Name == "Settings"))
                return string.Empty;

            var settings = ini["Settings"];
            


            return settings["apiKey"];
        }

        public static void setApiKey(string apiKey) {
            var ini = new Ini(settingsFile);

            if (!ini.Any(s => s.Name == "Settings"))
                ini.Add(new Section("Settings"));
            
            ini["Settings"]["apiKey"] = apiKey;
            
            ini.SaveTo(settingsFile);
        }
    }
}