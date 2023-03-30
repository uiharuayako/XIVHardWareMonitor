using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace XIVHardWareMonitor
{
    // 这个类用于存放被监视的Sensor信息
    public class WatchedSensor
    {
        public readonly string Identifier;
        public readonly string HardWare;
        public string InfoStr;
        public bool IsVisible;

        public readonly string Name;

        // 保留小数位数
        public int Decimal;

        public WatchedSensor(string hardWare, string identifier, string name)
        {
            Identifier = identifier;
            HardWare = hardWare;
            Name = name;
            IsVisible = true;
            InfoStr = "<name>: <value> <unit>";
            Decimal = 2;
        }

        public string GetResult(ISensor sensor)
        {
            string valueStr = "";
            if (sensor.Value != null)
            {
                valueStr = StaticUtils.FormatFloatString(sensor.Value.ToString(), Decimal);
            }

            string unit = StaticUtils.UnitDictionary[sensor.SensorType];
            // tmd，为什么会有这种情况......
            if (unit.Equals("%%")) unit = "%";
            string[] results =
                { sensor.Name, valueStr, unit };
            return StaticUtils.ReplaceStrings(InfoStr, StaticUtils.PlaceHolders, results);
        }
    }
}
