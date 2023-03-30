using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;
using XIVHardWareMonitor.Properties;

namespace XIVHardWareMonitor.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;
    private Computer computer;
    private Configuration configuration;
    private Dictionary<string, string> windowDic;

    public MainWindow(Plugin plugin) : base(
        "硬件监视", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(400, 400),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;

        computer = plugin.computer;
        configuration = plugin.Configuration;
        // 读取语言文件
        string jsonStr = Resource.ResourceManager.GetString($"MainWindow_{configuration.Language}");
        windowDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
        WindowName = windowDic["WindowName"];
    }

    public void UpdateLanguage()
    {
        // 读取语言文件
        string jsonStr = Resource.ResourceManager.GetString($"MainWindow_{configuration.Language}");
        windowDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
        WindowName = windowDic["WindowName"];
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (ImGui.Button(windowDic["Setting"]))
        {
            this.plugin.DrawConfigUI();
        }

        ImGui.SameLine();
        if (ImGui.Button(windowDic["DtrSet"]))
        {
            this.plugin.DrawDtrConfigUI();
        }

        ImGui.SameLine();
        if (ImGui.Button(windowDic["HomePage"]))
        {
            Dalamud.Utility.Util.OpenLink("https://github.com/uiharuayako/DalamudPlugins");
            Plugin.ChatGui.Print("本插件目前还在测试阶段，还没放到这个仓库里");
        }

        ImGui.SameLine();
        // 遍历硬件信息，以表的形式显示
        // 遍历传感器信息并输出表格
        ImGui.Text($"{windowDic["HardwareCount"]}：{Sensors.SensorsDictionary.Count}");
        if (ImGui.BeginChild("Hardware List", new Vector2(0f, -1f), true))
        {
            foreach (var hardware in Sensors.SensorsDictionary)
            {
                if (ImGui.CollapsingHeader(hardware.Key))
                {
                    int i = 0;
                    ImGui.Columns(7, hardware.Key);
                    ImGui.Text(windowDic["Order"]);
                    ImGui.NextColumn();
                    ImGui.Text(windowDic["Name"]);
                    ImGui.NextColumn();
                    ImGui.Text(windowDic["Unit"]);
                    ImGui.NextColumn();
                    ImGui.Text(windowDic["Value"]);
                    ImGui.NextColumn();
                    ImGui.Text(windowDic["Min"]);
                    ImGui.NextColumn();
                    ImGui.Text(windowDic["Max"]);
                    ImGui.NextColumn();
                    ImGui.Text(windowDic["StatusBar"]);
                    ImGui.NextColumn();
                    foreach (var sensor in hardware.Value)
                    {
                        i++;
                        ImGui.TextColored(ImGuiColors.TankBlue, i.ToString());
                        ImGui.NextColumn();
                        ImGui.Text(sensor.Value.Name);
                        ImGui.NextColumn();
                        string unitStr = "";
                        if (StaticUtils.UnitDictionary.ContainsKey(sensor.Value.SensorType))
                        {
                            unitStr = StaticUtils.UnitDictionary[sensor.Value.SensorType];
                        }

                        ImGui.TextColored(ImGuiColors.DPSRed, unitStr);
                        ImGui.NextColumn();
                        ImGui.Text(sensor.Value.Value.ToString());
                        ImGui.NextColumn();
                        ImGui.Text(sensor.Value.Min.ToString());
                        ImGui.NextColumn();
                        ImGui.Text(sensor.Value.Max.ToString());
                        ImGui.NextColumn();
                        if (ImGui.Button($"{windowDic["Add"]}##{hardware.Key}-{sensor.Key}"))
                        {
                            configuration.WatchedSensors.Add(
                                new WatchedSensor(hardware.Key, sensor.Key, sensor.Value.Name));
                            configuration.Save();
                        }

                        ImGui.NextColumn();
                    }
                }
            }

            ImGui.EndChild();
        }
    }
}
