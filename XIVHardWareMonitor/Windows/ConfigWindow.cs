using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace XIVHardWareMonitor.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;
    private Plugin plugin;

    public ConfigWindow(Plugin plugin) : base(
        "硬件监控设置",
        ImGuiWindowFlags.NoCollapse)
    {
        this.plugin=plugin;
        Size = new Vector2(320, ImGui.GetTextLineHeightWithSpacing() + ImGui.GetTextLineHeight() * 13);
        SizeCondition = ImGuiCond.Once;

        this.configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (ImGui.Button("主界面"))
        {
            plugin.DrawMainUI();
        }
        ImGui.SameLine();
        if (ImGui.Button("状态栏设置"))
        {
            plugin.DrawDtrConfigUI();
        }
        // 修改获取间隔
        double refreshRate = configuration.RefreshRate;
        if(ImGui.InputDouble("刷新间隔",ref refreshRate, 100, 500, "%.1f"))
        {
            configuration.RefreshRate = refreshRate;
        }

        bool enableCpu = configuration.IsCpuEnabled;
        if (ImGui.Checkbox("监控CPU信息", ref enableCpu))
        {
            configuration.IsCpuEnabled = enableCpu;
        }

        bool enableGpu = configuration.IsGpuEnabled;
        if (ImGui.Checkbox("监控GPU信息", ref enableGpu))
        {
            configuration.IsGpuEnabled = enableGpu;
        }

        bool enableMemory = configuration.IsMemoryEnabled;
        if (ImGui.Checkbox("监控内存信息", ref enableMemory))
        {
            configuration.IsMemoryEnabled = enableMemory;
        }

        bool enableMotherboard = configuration.IsMotherboardEnabled;
        if (ImGui.Checkbox("监控主板信息", ref enableMotherboard))
        {
            configuration.IsMotherboardEnabled = enableMotherboard;
        }

        bool enableController = configuration.IsControllerEnabled;
        if (ImGui.Checkbox("监控控制器信息", ref enableController))
        {
            configuration.IsControllerEnabled = enableController;
        }

        bool enableNetwork = configuration.IsNetworkEnabled;
        if (ImGui.Checkbox("监控网络信息", ref enableNetwork))
        {
            configuration.IsNetworkEnabled = enableNetwork;
        }

        bool enableStorage = configuration.IsStorageEnabled;
        if (ImGui.Checkbox("监控存储信息", ref enableStorage))
        {
            configuration.IsStorageEnabled = enableStorage;
        }

        bool enableBattery = configuration.IsBatteryEnabled;
        if (ImGui.Checkbox("监控电池信息", ref enableBattery))
        {
            configuration.IsBatteryEnabled = enableBattery;
        }

        bool enablePsu = configuration.IsPsuEnabled;
        if (ImGui.Checkbox("监控电源信息", ref enablePsu))
        {
            configuration.IsPsuEnabled = enablePsu;
        }


        if (ImGui.Button("保存"))
        {
            plugin.computer.IsCpuEnabled = enableCpu;
            plugin.computer.IsGpuEnabled = enableGpu;
            plugin.computer.IsMemoryEnabled = enableMemory;
            plugin.computer.IsMotherboardEnabled = enableMotherboard;
            plugin.computer.IsControllerEnabled = enableController;
            plugin.computer.IsNetworkEnabled = enableNetwork;
            plugin.computer.IsStorageEnabled = enableStorage;
            plugin.computer.IsBatteryEnabled = enableBattery;
            plugin.computer.IsPsuEnabled = enablePsu;
            plugin.watcher.SetInterval(refreshRate);
            configuration.Save();
        }
    }
}
