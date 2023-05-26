using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using LibreHardwareMonitor.Hardware.Cpu;
using MSIAfterburnerNET.HM;

namespace XIVHardWareMonitor
{
    // 这个类用于存放被监视的Sensor信息
    public class WatchedSensor
    {
        // 不能改
        public readonly string Identifier;
        public readonly string HardWare;
        public readonly string Name;
        // 可修改的
        public string InfoStr;
        public bool IsVisible;
        // 警戒阈值
        public double WarningThreshold;
        public bool EnableWarning;
 

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
            WarningThreshold = 100;
            EnableWarning = false;
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

        // 从小飞机获取信息
        public string GetResult(HardwareMonitorEntry entry)
        {
            string valueStr = "";
            if (entry.Data != null)
            {
                valueStr = Math.Round(entry.Data, Decimal).ToString();
            }
            string unit = entry.LocalizedSrcUnits;
            string[] results =
                { entry.LocalizedSrcName, valueStr, unit };
            return StaticUtils.ReplaceStrings(InfoStr, StaticUtils.PlaceHolders, results);
        }

        public bool IsAboveThreshold(ISensor sensor)
        {
            string valueStr = "";
            if (sensor.Value != null)
            {
                valueStr = StaticUtils.FormatFloatString(sensor.Value.ToString(), Decimal);
            }
            
            double.TryParse(valueStr, out double realValue);
            return realValue > WarningThreshold;
        }

        public bool IsAboveThreshold(HardwareMonitorEntry entry)
        {
            return entry.Data > WarningThreshold;
        }
    }
}
