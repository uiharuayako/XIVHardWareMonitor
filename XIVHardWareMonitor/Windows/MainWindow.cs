using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ECommons.LanguageHelpers;
using ImGuiNET;
using ImGuiScene;
using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;

namespace XIVHardWareMonitor.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;
    private Configuration configuration;
    private static string windowName = "XIV Hardware Monitor";
    public MainWindow(Plugin plugin) : base(
        windowName.Loc(), ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(400, 400),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
        
        configuration = plugin.Configuration;
        WindowName = windowName.Loc();
    }
    public void UpdateTitle()
    {
        WindowName = windowName.Loc();
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (ImGui.Button("Setting".Loc()))
        {
            this.plugin.DrawConfigUI();
        }

        ImGui.SameLine();
        if (ImGui.Button("Dtr Settings".Loc()))
        {
            this.plugin.DrawDtrConfigUI();
        }

        ImGui.SameLine();
        if (ImGui.Button("HomePage".Loc()))
        {
            Dalamud.Utility.Util.OpenLink("https://github.com/uiharuayako/XIVHardWareMonitor");
        }

        ImGui.SameLine();
        // 遍历硬件信息，以表的形式显示
        // 遍历传感器信息并输出表格
        ImGui.Text($"{"Hardware Count".Loc()}：{Sensors.SensorsDictionary.Count}");
        if (ImGui.BeginChild("Hardware List", new Vector2(0f, -1f), true))
        {
            foreach (var hardware in Sensors.SensorsDictionary)
            {
                if (ImGui.CollapsingHeader(hardware.Key))
                {
                    int i = 0;
                    ImGui.Columns(7, hardware.Key);
                    ImGui.Text("Order".Loc());
                    ImGui.NextColumn();
                    ImGui.Text("Name".Loc());
                    ImGui.NextColumn();
                    ImGui.Text("Unit".Loc());
                    ImGui.NextColumn();
                    ImGui.Text("Value".Loc());
                    ImGui.NextColumn();
                    ImGui.Text("Min".Loc());
                    ImGui.NextColumn();
                    ImGui.Text("Max".Loc());
                    ImGui.NextColumn();
                    ImGui.Text("Status Bar".Loc());
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
                        if (ImGui.Button($"{"Add".Loc()}##{hardware.Key}-{sensor.Key}"))
                        {
                            configuration.WatchedSensors.Add(
                                new WatchedSensor(hardware.Key, sensor.Key, sensor.Value.Name));
                            configuration.Save();
                        }

                        ImGui.NextColumn();
                    }
                    ImGui.Columns(1);
                }
            }

            ImGui.EndChild();
        }
    }
}
