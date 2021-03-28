using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ewoxej_gitsame
{
        [DataContract]
        public class Settings
        {
            [DataMember]
            public List<string> filters { get; set; }
            [DataMember]
            public bool allowRegExp { get; set; }
            [DataMember]
            public int AnalysisDepth{ get; set; }
            private static Settings instance;
            private Settings()
            {
                filters = new List<string>();
                AnalysisDepth = 2;
            }

            public bool isInFilters(string str)
            {
                var tokens = str.Split('/');
                foreach (var i in tokens)
                {
                    if (allowRegExp)
                    {
                        foreach (var filter in filters)
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(i, filter))
                                return true;
                        }
                    }
                    else
                    {
                        if (filters.Contains(i))
                            return true;
                    }
                }
                return false;
            }

            public static Settings getInstance()
            {
                if (instance == null)
                {
                    instance = new Settings();
                    try
                    {
                        if (System.IO.File.Exists(Helper.SettingsPath))
                        {
                            using (FileStream file = new FileStream(Helper.SettingsPath, FileMode.Open, System.IO.FileAccess.Read))
                            {
                                var ser = new DataContractJsonSerializer(instance.GetType());
                                instance = ser.ReadObject(file) as Settings;
                                if (instance == null)
                                    instance = new Settings();
                            }
                        }
                    }
                    catch
                    {

                    }
                }

                return instance;
            }
            ~Settings()
            {
                using (FileStream file = new FileStream(Helper.SettingsPath, FileMode.Create, System.IO.FileAccess.Write))
                {
                    var ser = new DataContractJsonSerializer(typeof(Settings));
                    ser.WriteObject(file, instance);
                }

            }
        }
}
