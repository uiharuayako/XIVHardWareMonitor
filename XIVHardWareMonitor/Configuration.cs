using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace XIVHardWareMonitor
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        // 刷新频率 单位ms
        public double RefreshRate = 1000;

        // 警报频率 单位s
        public double WarningRate = 300;

        // 被监控的硬件项目列表
        public List<WatchedSensor> WatchedSensors { get; set; } = new List<WatchedSensor>();

        // 语言
        public string Language = "English";

        // 分隔符
        public string Separator = " | ";

        // 启用Afterburner作为数据源
        public bool UseAfterBurner = false;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
