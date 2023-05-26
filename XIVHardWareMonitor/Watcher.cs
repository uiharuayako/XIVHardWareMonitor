using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Dalamud.Game.Gui.Dtr;

namespace XIVHardWareMonitor
{
    // 用于定时更新硬件信息
    // 为什么不在ImGui中实时访问而要采用迂回的方法？
    // 因为在ImGui中实时访问Computer对象会造成无法承受的资源损耗

    // 为了防止代码混乱，这里不直接引用plugin本体了
    public class Watcher : IDisposable
    {
        // 计时器，用于定时更新硬件信息
        private Timer timer;

        // 计时器，用于定时判断预警
        private Timer warningTimer;

        // 获取插件设置
        private Configuration configuration;

        // 获取Computer对象
        private Computer computer;

        // 获取任务栏实体
        private DtrBarEntry entry;

        // 获取访问器
        private HardwareVisitor hardwareVisitor;

        // 任务栏字符串列表
        private List<string> dtrStrList;

        public Watcher(Plugin plugin)
        {
            configuration = plugin.Configuration;
            computer = plugin.computer;
            entry = plugin.HardwareDtrBar;
            hardwareVisitor = new HardwareVisitor(Sensors.SensorsDictionary);
            timer = new Timer(configuration.RefreshRate);
            dtrStrList = new List<string>();
            // 刷新硬件信息
            timer.Elapsed += (sender, args) =>
            {
                computer.Traverse(Sensors.HardwareVisitor);
                if (Plugin.AfterBurner != null && configuration.UseAfterBurner)
                {
                    Plugin.AfterBurner.Refresh();
                }
                entry.Text = GetDtrStr();
            };
            timer.Start();
            // 预警
            warningTimer = new Timer(configuration.WarningRate * 1000);
            warningTimer.Elapsed += (sender, args) =>
            {
                // 遍历设置里受到监视的硬件列表
                foreach (var item in configuration.WatchedSensors)
                {
                    // 如果item被设为不预警，则跳过
                    if (!item.EnableWarning) continue;
                    if (Sensors.SensorsDictionary.ContainsKey(item.HardWare))
                    {
                        if (Sensors.SensorsDictionary[item.HardWare].ContainsKey(item.Identifier))
                        {
                            // 确定传感器真的存在之后，赋值
                            // 获取sensor
                            var sensor = Sensors.SensorsDictionary[item.HardWare][item.Identifier];
                            if (item.IsAboveThreshold(sensor))
                            {
                                Plugin.ChatGui.PrintError($"{sensor.SensorType}!!{sensor.Name}: {sensor.Value}");
                            }
                        }
                    }
                }
            };
            warningTimer.Start();
        }

        // 修改timer的间隔时间
        public void SetInterval(double interval)
        {
            if (timer.Enabled) timer.Enabled = false;
            timer.Interval = interval;
            timer.Enabled = true;
        }

        // 修改警报的间隔时间
        public void SetWarningInterval(double interval)
        {
            if (warningTimer.Enabled) warningTimer.Enabled = false;
            warningTimer.Interval = interval * 1000;
            warningTimer.Enabled = true;
        }

        // 暂停
        public void Pause()
        {
            if (timer.Enabled) timer.Enabled = false;
        }

        // 继续
        public void Continue()
        {
            if (!timer.Enabled) timer.Enabled = true;
        }

        public string GetDtrStr()
        {
            // 先清空
            dtrStrList.Clear();
            // 遍历设置里受到监视的硬件列表
            foreach (var item in configuration.WatchedSensors)
            {
                // 如果item被设为不可见，则跳过
                if (!item.IsVisible) continue;
                if (Sensors.SensorsDictionary.ContainsKey(item.HardWare))
                {
                    if (Sensors.SensorsDictionary[item.HardWare].ContainsKey(item.Identifier))
                    {
                        // 确定传感器真的存在之后，赋值
                        // 获取sensor
                        var sensor = Sensors.SensorsDictionary[item.HardWare][item.Identifier];
                        dtrStrList.Add(item.GetResult(sensor));
                    }
                }
            }

            return string.Join(configuration.Separator, dtrStrList);
        }

        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
            warningTimer.Stop();
            warningTimer.Dispose();
        }
    }
}
