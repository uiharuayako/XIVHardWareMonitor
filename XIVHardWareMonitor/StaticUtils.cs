using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;

namespace XIVHardWareMonitor
{
    public static class StaticUtils
    {
        public static string[] PlaceHolders = new string[]
        {
            "<name>",
            "<value>",
            "<unit>"
        };

        public static string ReplaceStrings(string inputString, string[] originalStrings, string[] replacementStrings)
        {
            if (originalStrings.Length != replacementStrings.Length)
            {
                throw new ArgumentException(
                    "The number of original strings must match the number of replacement strings.");
            }

            for (int i = 0; i < originalStrings.Length; i++)
            {
                inputString = inputString.Replace(originalStrings[i], replacementStrings[i]);
            }

            return inputString;
        }

        // 保留n位小数
        public static string FormatFloatString(string str, int n)
        {
            if (!float.TryParse(str, out float result))
            {
                throw new ArgumentException("输入的字符串不是浮点数！");
            }

            int decimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)result)[3])[2];
            if (decimalPlaces <= n)
            {
                return str;
            }

            result = (float)Math.Round(result, n);
            return result.ToString();
        }

        // 读取json文件为Dictionary<string, string>
        public static Dictionary<string, string> ReadJsonFile(string filePath)
        {
            string jsonStr = File.ReadAllText(filePath);
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
            return dict;
        }

        public static Dictionary<string, string> UnitDictionary = new Dictionary<string, string>
        {
            { SensorType.Voltage.ToString(), "V" },
            { SensorType.Current.ToString(), "A" },
            { SensorType.Power.ToString(), "W" },
            { SensorType.Clock.ToString(), "MHz" },
            { SensorType.Temperature.ToString(), "°C" },
            { SensorType.Load.ToString(), "%%" },
            { SensorType.Frequency.ToString(), "Hz" },
            { SensorType.Fan.ToString(), "RPM" },
            { SensorType.Flow.ToString(), "L/h" },
            { SensorType.Control.ToString(), "%%" },
            { SensorType.Level.ToString(), "%%" },
            { SensorType.Factor.ToString(), "MHz" },
            { SensorType.Data.ToString(), "GB" },
            { SensorType.SmallData.ToString(), "MB" },
            { SensorType.Throughput.ToString(), "B/s" },
            { SensorType.TimeSpan.ToString(), "S" },
            { SensorType.Energy.ToString(), "mWh" }
        };

        public static Dictionary<string, int> LanguageDictionary = new Dictionary<string, int>
        {
            { "zh", 0 },
            { "en", 1 }
        };
        // 储存插件地址
        public static string PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
