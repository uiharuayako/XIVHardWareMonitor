using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ECommons.LanguageHelpers;
using Hardware.Info;
using ImGuiNET;
using Newtonsoft.Json;

namespace XIVHardWareMonitor.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;
    private Plugin plugin;
    private static string windowName = "XIV Hardware Monitor Settings";
    public ConfigWindow(Plugin plugin) : base(
        windowName.Loc(),
        ImGuiWindowFlags.NoCollapse)
    {
        this.plugin=plugin;
        Size = new Vector2(320, ImGui.GetTextLineHeightWithSpacing() + ImGui.GetTextLineHeight() * 13);
        SizeCondition = ImGuiCond.Once;
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
        if (ImGui.Button("Home".Loc()))
        {
            plugin.DrawMainUI();
        }
        ImGui.SameLine();
        if (ImGui.Button("Dtr Settings".Loc()))
        {
            plugin.DrawDtrConfigUI();
        }
        // 修改语言
        if (ImGui.BeginCombo("Language".Loc(), configuration.Language == null ? "Game Language".Loc() : configuration.Language.Loc()))
        {
            if (ImGui.Selectable("Game Language".Loc()))
            {
                configuration.Language = null;
                Localization.Init(Localization.GameLanguageString);
            }
            foreach (var x in Localization.GetAvaliableLanguages())
            {
                if (ImGui.Selectable(x.Loc()))
                {
                    configuration.Language = x;
                    Localization.Init(configuration.Language);
                    configuration.Save();
                }
            }
            plugin.UpdateTitles();
            ImGui.EndCombo();
        }
        // 修改获取间隔
        double refreshRate = configuration.RefreshRate;
        if (ImGui.InputDouble("Refresh Rate(ms)".Loc(), ref refreshRate, 100, 500, "%.1f"))
        {
            if (refreshRate < 100)
            {
                refreshRate = 100;
            }
            configuration.RefreshRate = refreshRate;
        }
        // 修改报警间隔
        double warningRate = configuration.WarningRate;
        if (ImGui.InputDouble("Warning Rate(s)".Loc(), ref warningRate, 10, 50, "%.1f"))
        {
            if (warningRate < 5)
            {
                warningRate = 5;
            }
            configuration.WarningRate = warningRate;
        }
        // 设置分隔符
        string separator = configuration.Separator;
        if (ImGui.InputText("Separator".Loc(), ref separator, 20))
        {
            configuration.Separator = separator;
            configuration.Save();
        }

        if (ImGui.Button("Save".Loc()))
        {
            Plugin.ChatGui.Print("保存设置");
            Sensors.SensorsDictionary.Clear();
            plugin.watcher.SetInterval(refreshRate);
            plugin.watcher.SetWarningInterval(warningRate);
            configuration.Save();
        }
    }
}
