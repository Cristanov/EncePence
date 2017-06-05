using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncePence
{
    static class Paths
    {
        public static string GetMyAppDataPath()
        {
            string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appName = "EncePence";
            string myAppDataDirPath = System.IO.Path.Combine(applicationDataPath, appName);
            return myAppDataDirPath;
        }

        public static string GetMySettingsXmlFilePath()
        {
            return Path.Combine(Paths.GetMyAppDataPath(), "Settings.xml");
        }

        public static string GetCategoryVisibilityXmlFile()
        {
            return Path.Combine(GetMyAppDataPath(), "categoryVisibility.xml");
        }

        public static string GetWordVisibilityXmlFile()
        {
            return Path.Combine(GetMyAppDataPath(), "wordsVisibility.xml");
        }

        public static string GetWordVisibilityCounterXmlFile()
        {
            return Path.Combine(GetMyAppDataPath(), "wordsVisibilityCounter.xml");
        }
    }
}
