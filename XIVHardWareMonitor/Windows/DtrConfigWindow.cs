using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace XIVHardWareMonitor.Windows
{
    internal class DtrConfigWindow : Window, IDisposable
    {
        private Configuration configuration;
        private Plugin plugin;

        public DtrConfigWindow(Plugin plugin) : base(
            "状态栏设置",
            ImGuiWindowFlags.NoCollapse)
        {
            this.plugin = plugin;
            Size = new Vector2(640, ImGui.GetTextLineHeightWithSpacing() + ImGui.GetTextLineHeight() * 13);
            SizeCondition = ImGuiCond.Once;

            this.configuration = plugin.Configuration;
        }

        public void Dispose() { }

        public override void Draw()
        {
            if (ImGui.BeginChild("HardwareDtrBarSets"))
            {
                ImGui.Columns(5);
                ImGui.Text("启用");
                ImGui.NextColumn();
                ImGui.Text("名称");
                ImGui.NextColumn();
                ImGui.Text("状态栏显示");
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("<name>:传感器名称\n<value>:四舍五入的值\n<unit>:单位\n值暂时改不了，单位和名称可以改成你喜欢的");
                ImGui.NextColumn();
                ImGui.Text("小数位");
                ImGui.NextColumn();
                ImGui.Text("操作");
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
                    // 删除按钮
                    if (ImGui.Button($"删除##delete{i}"))
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
                ImGui.EndChild();
            }
        }
    }
}
