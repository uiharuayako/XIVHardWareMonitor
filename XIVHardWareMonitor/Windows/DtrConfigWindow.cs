using Dalamud.Interface.Windowing;
using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ECommons.LanguageHelpers;

namespace XIVHardWareMonitor.Windows
{
    internal class DtrConfigWindow : Window, IDisposable
    {
        private Configuration configuration;
        private static string windowName= "DtrBar Settings";

        public DtrConfigWindow(Plugin plugin) : base(
            windowName.Loc(),
            ImGuiWindowFlags.NoCollapse)
        {
            Size = new Vector2(640, ImGui.GetTextLineHeightWithSpacing() + ImGui.GetTextLineHeight() * 13);
            SizeCondition = ImGuiCond.Once;

            this.configuration = plugin.Configuration;
            WindowName = windowName.Loc();
        }
        public void UpdateTitle()
        {
            WindowName = windowName.Loc();
        }
        public void Dispose() { }

        public override void Draw()
        {
            if (ImGui.BeginChild("HardwareDtrBarSets"))
            {
                ImGui.Columns(7);
                ImGui.Text("Enable".Loc());
                ImGui.NextColumn();
                ImGui.Text("Name".Loc());
                ImGui.NextColumn();
                ImGui.Text("Display".Loc());
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Using placeholders to refer info\n<name>:name\n<value>:value\n<unit>:unit".Loc());
                ImGui.NextColumn();
                ImGui.Text("Decimal".Loc());
                ImGui.NextColumn();
                // 预警相关
                ImGui.Text("Enable Warning".Loc());
                ImGui.NextColumn();
                ImGui.Text("Threshold".Loc());
                ImGui.NextColumn();

                ImGui.Text("Operation".Loc());
                ImGui.NextColumn();
                for (int i = 0; i < configuration.WatchedSensors.Count; i++)
                {
                    bool enableSensor = configuration.WatchedSensors[i].IsVisible;
                    if (ImGui.Checkbox($"##enable{i}",ref enableSensor))
                    {
                        configuration.WatchedSensors[i].IsVisible= enableSensor;
                        configuration.Save();
                    }
                    ImGui.NextColumn();
                    ImGui.Text(configuration.WatchedSensors[i].Name);
                    ImGui.NextColumn();
                    string text = configuration.WatchedSensors[i].InfoStr;
                    if (ImGui.InputText($"##infoStr{i}", ref text, 200))
                    {
                        configuration.WatchedSensors[i].InfoStr = text;
                        configuration.Save();
                    }
                    ImGui.NextColumn();
                    int decimalPlaces = configuration.WatchedSensors[i].Decimal;
                    if (ImGui.InputInt($"##decimal{i}", ref decimalPlaces,1))
                    {
                        // 限制范围
                        if (decimalPlaces < 0)
                        {
                            decimalPlaces = 0;
                        }
                        else if (decimalPlaces > 5)
                        {
                            decimalPlaces = 5;
                        }
                        configuration.WatchedSensors[i].Decimal = decimalPlaces;
                        configuration.Save();
                    }
                    ImGui.NextColumn();

                    bool enableWarning = configuration.WatchedSensors[i].EnableWarning;
                    if (ImGui.Checkbox($"##enableWarning{i}", ref enableWarning))
                    {
                        configuration.WatchedSensors[i].EnableWarning = enableWarning;
                        configuration.Save();
                    }
                    ImGui.NextColumn();
                    double threshold = configuration.WatchedSensors[i].WarningThreshold;
                    if (ImGui.InputDouble($"##threshold{i}", ref threshold, 100, 500, "%.1f"))
                    {
                        configuration.WatchedSensors[i].WarningThreshold = threshold;
                        configuration.Save();
                    }
                    ImGui.NextColumn();

                    // 删除按钮
                    if (ImGui.Button($"{"Delete".Loc()}##delete{i}"))
                    {
                        configuration.WatchedSensors.RemoveAt(i);
                        configuration.Save();
                    }
                    ImGui.SameLine();
                    // 往上移，不能是第一个元素
                    if (ImGui.Button($"↑##ItemUp{i}") && i != 0)
                    {
                        WatchedSensor temp = configuration.WatchedSensors[i];
                        configuration.WatchedSensors[i] = configuration.WatchedSensors[i - 1];
                        configuration.WatchedSensors[i - 1] = temp;
                        configuration.Save();
                    }

                    ImGui.SameLine();
                    // 往下移，不能是最后一个元素
                    if (ImGui.Button($"↓##ItemDown{i}") && i != configuration.WatchedSensors.Count() - 1)
                    {
                        WatchedSensor temp = configuration.WatchedSensors[i];
                        configuration.WatchedSensors[i] = configuration.WatchedSensors[i + 1];
                        configuration.WatchedSensors[i + 1] = temp;
                        configuration.Save();
                    }
                    ImGui.NextColumn();
                }
                ImGui.Columns(1);
                ImGui.EndChild();
            }
        }
    }
}
