using System;
using System.Numerics;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using LibreHardwareMonitor.Hardware;

namespace XIVHardWareMonitor.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin;
    private Computer computer;
    private Configuration configuration;

    public MainWindow(Plugin plugin) : base(
        "硬件监视", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(400, 400),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        
        this.Plugin = plugin;

        computer = plugin.computer;
        configuration = plugin.Configuration;
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        if (ImGui.Button("设置"))
        {
            this.Plugin.DrawConfigUI();
        }
        ImGui.SameLine();
        if (ImGui.Button("状态栏设置"))
        {
            this.Plugin.DrawDtrConfigUI();
        }
        // 遍历硬件信息，以表的形式显示
        // 遍历传感器信息并输出表格
        int i = 0;
        foreach (var hardware in Sensors.SensorsDictionary)
        {
            if (ImGui.CollapsingHeader(hardware.Key))
            {
                if (ImGui.BeginChild(hardware.Key))
                {
                    ImGui.Columns(7);
                    ImGui.Text("序号");
                    ImGui.NextColumn();
                    ImGui.Text("名称");
                    ImGui.NextColumn();
                    ImGui.Text("单位");
                    ImGui.NextColumn();
                    ImGui.Text("值");
                    ImGui.NextColumn();
                    ImGui.Text("Min");
                    ImGui.NextColumn();
                    ImGui.Text("Max");
                    ImGui.NextColumn();
                    ImGui.Text("状态栏");
                    ImGui.NextColumn();
                    foreach (var sensor in hardware.Value)
                    {
                        i++;
                        ImGui.TextColored(ImGuiColors.TankBlue, i.ToString());
                        ImGui.NextColumn();
                        ImGui.Text(sensor.Value.Name);
                        ImGui.NextColumn();
                        ImGui.TextColored(ImGuiColors.DPSRed, StaticUtils.UnitDictionary[sensor.Value.SensorType.ToString()]);
                        ImGui.NextColumn();
                        ImGui.Text(sensor.Value.Value.ToString());
                        ImGui.NextColumn();
                        ImGui.Text(sensor.Value.Min.ToString());
                        ImGui.NextColumn();
                        ImGui.Text(sensor.Value.Max.ToString());
                        ImGui.NextColumn();
                        if (ImGui.Button($"添加##{hardware.Key}-{sensor.Key}"))
                        {
                            configuration.WatchedSensors.Add(new WatchedSensor(hardware.Key, sensor.Key,sensor.Value.Name));
                            configuration.Save();
                        }
                        ImGui.NextColumn();

                    }
                    ImGui.EndChild();
                }

            }
        }
    }
}
