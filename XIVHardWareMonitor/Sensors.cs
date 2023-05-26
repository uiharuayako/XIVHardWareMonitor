using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSIAfterburnerNET.HM;

namespace XIVHardWareMonitor
{
    public static class Sensors
    {
        // 建立一个二重字典来储存传感器信息
        // 第一层字典:Dictionary<string, Dictionary<string, ISensor>>
        // 以硬件名称为键，以硬件下的传感器字典为值
        // 第二层字典:Dictionary<string, ISensor>
        public static Dictionary<string, Dictionary<string, ISensor>> SensorsDictionary =
            new();

        public static HardwareVisitor HardwareVisitor = new(SensorsDictionary);
        public static Dictionary<string, HardwareMonitorEntry> AfterburnerEntries = new();
        public static void RefreshMsi()
        {
            for (uint i = 0; i < Plugin.AfterCount; i++)
            {
                Plugin.AfterBurner.RefreshEntry(i);
                AfterburnerEntries.TryAdd(Plugin.AfterBurner.Entries[i].SrcName, Plugin.AfterBurner.Entries[i]);
            }
        }
    }
    public class HardwareVisitor : IVisitor
    {
        private Dictionary<string, Dictionary<string, ISensor>> _sensors;

        public HardwareVisitor(Dictionary<string, Dictionary<string, ISensor>> sensors)
        {
            _sensors = sensors;
        }

        public void VisitComputer(IComputer computer) { }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (var subHardware in hardware.SubHardware)
            {
                subHardware.Accept(this);
            }

            foreach (var sensor in hardware.Sensors)
            {
                // 不存在则初始化
                if (!_sensors.ContainsKey(sensor.Hardware.Name))
                {
                    _sensors[sensor.Hardware.Name] = new Dictionary<string, ISensor>
                    {
                        [sensor.Identifier.ToString()] = sensor
                    };
                }

                _sensors[sensor.Hardware.Name][sensor.Identifier.ToString()] = sensor;
            }
        }

        public void VisitParameter(IParameter parameter) { }

        public void VisitSensor(ISensor sensor) { }
    }
}
