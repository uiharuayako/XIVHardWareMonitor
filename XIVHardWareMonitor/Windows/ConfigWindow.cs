using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Newtonsoft.Json;
using XIVHardWareMonitor.Properties;

namespace XIVHardWareMonitor.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;
    private Plugin plugin;
    private Dictionary<string, string> windowDic;
    public ConfigWindow(Plugin plugin) : base(
        "硬件监控设置",
        ImGuiWindowFlags.NoCollapse)
    {
        this.plugin=plugin;
        Size = new Vector2(320, ImGui.GetTextLineHeightWithSpacing() + ImGui.GetTextLineHeight() * 13);
        SizeCondition = ImGuiCond.Once;
        configuration = plugin.Configuration;
        // 读取语言文件
        string jsonStr = Resource.ResourceManager.GetString($"ConfigWindow_{configuration.Language}");
        windowDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
        WindowName = windowDic["WindowName"];
    }

    public void UpdateLanguage()
    {
        // 读取语言文件
        string jsonStr = Resource.ResourceManager.GetString($"ConfigWindow_{configuration.Language}");
        windowDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
        WindowName = windowDic["WindowName"];

    }
    public void Dispose() { }

    public override void Draw()
    {
        if (ImGui.Button(windowDic["Home"]))
        {
            plugin.DrawMainUI();
        }
        ImGui.SameLine();
        if (ImGui.Button(windowDic["DtrSet"]))
        {
            plugin.DrawDtrConfigUI();
        }
        // 修改语言
        int language=StaticUtils.LanguageDictionary[configuration.Language];
        string[] langStrings = { "zh", "en" };
        if (ImGui.Combo(windowDic["Language"], ref language,langStrings,langStrings.Length))
        {
            configuration.Language = langStrings[language];
            plugin.UpdateLanguage();
            configuration.Save();
        }
        // 修改获取间隔
        double refreshRate = configuration.RefreshRate;
        if (ImGui.InputDouble(windowDic["RefreshRate"], ref refreshRate, 100, 500, "%.1f"))
        {
            if (refreshRate < 100)
            {
                refreshRate = 100;
            }
            configuration.RefreshRate = refreshRate;
        }
        // 修改报警间隔
        double warningRate = configuration.WarningRate;
        if (ImGui.InputDouble(windowDic["WarningRate"], ref warningRate, 10, 50, "%.1f"))
        {
            if (warningRate < 5)
            {
                warningRate = 5;
            }
            configuration.WarningRate = warningRate;
        }

        bool enableCpu = configuration.IsCpuEnabled;
        if (ImGui.Checkbox(windowDic["EnableCPU"], ref enableCpu))
        {
            configuration.IsCpuEnabled = enableCpu;
            configuration.Save();
        }

        bool enableGpu = configuration.IsGpuEnabled;
        if (ImGui.Checkbox(windowDic["EnableGPU"], ref enableGpu))
        {
            configuration.IsGpuEnabled = enableGpu;
            configuration.Save();
        }

        bool enableMemory = configuration.IsMemoryEnabled;
        if (ImGui.Checkbox(windowDic["EnableMEM"], ref enableMemory))
        {
            configuration.IsMemoryEnabled = enableMemory;
            configuration.Save();
        }

        bool enableMotherboard = configuration.IsMotherboardEnabled;
        if (ImGui.Checkbox(windowDic["EnableMB"], ref enableMotherboard))
        {
            configuration.IsMotherboardEnabled = enableMotherboard;
            configuration.Save();
        }

        bool enableController = configuration.IsControllerEnabled;
        if (ImGui.Checkbox(windowDic["EnableController"], ref enableController))
        {
            configuration.IsControllerEnabled = enableController;
            configuration.Save();
        }

        bool enableNetwork = configuration.IsNetworkEnabled;
        if (ImGui.Checkbox(windowDic["EnableNetwork"], ref enableNetwork))
        {
            configuration.IsNetworkEnabled = enableNetwork;
            configuration.Save();
        }

        bool enableStorage = configuration.IsStorageEnabled;
        if (ImGui.Checkbox(windowDic["EnableStorage"], ref enableStorage))
        {
            configuration.IsStorageEnabled = enableStorage;
            configuration.Save();
        }

        bool enableBattery = configuration.IsBatteryEnabled;
        if (ImGui.Checkbox(windowDic["EnableBattery"], ref enableBattery))
        {
            configuration.IsBatteryEnabled = enableBattery;
            configuration.Save();
        }

        bool enablePsu = configuration.IsPsuEnabled;
        if (ImGui.Checkbox(windowDic["EnablePsu"], ref enablePsu))
        {
            configuration.IsPsuEnabled = enablePsu;
            configuration.Save();
        }
        // 设置分隔符
        string separator = configuration.Separator;
        if (ImGui.InputText(windowDic["Separator"], ref separator, 20))
        {
            configuration.Separator = separator;
            configuration.Save();
        }

        if (ImGui.Button(windowDic["Save"]))
        {
            Plugin.ChatGui.Print("保存设置");
            plugin.computer.IsCpuEnabled = enableCpu;
            plugin.computer.IsGpuEnabled = enableGpu;
            plugin.computer.IsMemoryEnabled = enableMemory;
            plugin.computer.IsMotherboardEnabled = enableMotherboard;
            plugin.computer.IsControllerEnabled = enableController;
            plugin.computer.IsNetworkEnabled = enableNetwork;
            plugin.computer.IsStorageEnabled = enableStorage;
            plugin.computer.IsBatteryEnabled = enableBattery;
            plugin.computer.IsPsuEnabled = enablePsu;
            Sensors.SensorsDictionary.Clear();
            plugin.watcher.SetInterval(refreshRate);
            plugin.watcher.SetWarningInterval(warningRate);
            configuration.Save();
        }
    }
}
