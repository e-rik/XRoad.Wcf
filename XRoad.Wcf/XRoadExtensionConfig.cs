using System;
using System.Configuration;
using System.IO;

namespace XRoad.Wcf
{
    public class XRoadExtensionConfig : ConfigurationSection
    {
        [ConfigurationProperty("commentLanguage", DefaultValue = "en")]
        public string CommentLanguage
        {
            get { return (string)base["commentLanguage"]; }
            set { base["commentLanguage"] = value; }
        }

        public static XRoadExtensionConfig GetConfiguration()
        {
            return GetConfiguration(null);
        }

        public static XRoadExtensionConfig GetConfiguration(Configuration configuration)
        {
            XRoadExtensionConfig config = null;

            if (configuration != null)
                config = configuration.GetSection("xroadExtension") as XRoadExtensionConfig;

            if (config != null)
                return config;

            config = ConfigurationManager.GetSection("xroadExtension") as XRoadExtensionConfig;
            if (config != null)
                return config;

            var configFile = GetCommandLineConfigFile();
            if (configFile != null && File.Exists(configFile))
            {
                var c = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = configFile }, ConfigurationUserLevel.None);
                if (c != null)
                    config = c.GetSection("xroadExtension") as XRoadExtensionConfig;
            }

            return config;
        }

        private static string GetCommandLineConfigFile()
        {
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                var args = arg.Split(new char[] { ':', '=' }, 2);
                if (args.Length == 2 && args[0].IndexOf("svcutilConfig", StringComparison.OrdinalIgnoreCase) >= 0)
                    return args[1];
            }

            return null;
        }
    }
}
