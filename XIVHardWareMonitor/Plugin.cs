using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Interface.Windowing;
using LibreHardwareMonitor.Hardware;
using XIVHardWareMonitor.Windows;
using System.Reflection;
using System;
using System.CodeDom;

namespace XIVHardWareMonitor
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "XIV HardWare Monitor";
        private const string CommandName = "/hardware";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("XIVHardWareMonitor");

        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }

        private DtrConfigWindow DtrConfigWindow { get; init; }
        // 硬件监控
        public Computer computer;

        public Watcher watcher;

        // 状态栏实体
        public DtrBarEntry HardwareDtrBar { get; init; }

        // Service
        [PluginService]
        [RequiredVersion("1.0")]
        public static DtrBar DtrBar { get; private set; } = null!;

        [PluginService]
        [RequiredVersion("1.0")]
        public static ChatGui ChatGui { get; private set; } = null!;

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            ChatGui.Print("XIV HardWare Monitor loaded.^ ^");
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);
            // 获取硬件监控
            try
            {
                computer = new Computer()
                {
                    IsCpuEnabled = Configuration.IsCpuEnabled,
                    IsGpuEnabled = Configuration.IsGpuEnabled,
                    IsMemoryEnabled = Configuration.IsMemoryEnabled,
                    IsMotherboardEnabled = Configuration.IsMotherboardEnabled,
                    IsControllerEnabled = Configuration.IsControllerEnabled,
                    IsNetworkEnabled = Configuration.IsNetworkEnabled,
                    IsStorageEnabled = Configuration.IsStorageEnabled,
                    IsBatteryEnabled = Configuration.IsBatteryEnabled,
                    IsPsuEnabled = Configuration.IsPsuEnabled,
                };
                computer.Open();
            }
            catch (Exception e)
            {
                Dalamud.Logging.PluginLog.Log(e.Message);
            } 
            // 初始化状态栏
            HardwareDtrBar = DtrBar.Get(Name);
            HardwareDtrBar.Shown = true;
            HardwareDtrBar.Text = "硬件监控";
            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this);
            DtrConfigWindow = new DtrConfigWindow(this);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);
            WindowSystem.AddWindow(DtrConfigWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            watcher = new Watcher(this);
        }
        // 修改语言后，更新语言
        public void UpdateLanguage()
        {
            ConfigWindow.UpdateLanguage();
            MainWindow.UpdateLanguage();
            DtrConfigWindow.UpdateLanguage();
        }
        public void Dispose()
        {
            Dalamud.Logging.PluginLog.Log("开始卸载Hardware Monitor");
            watcher.Dispose();
            computer.Close();
            computer = null!;
            WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            DtrConfigWindow.Dispose();
            HardwareDtrBar.Remove();
            HardwareDtrBar.Dispose();
            CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawMainUI()
        {
            MainWindow.Toggle();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.Toggle();
        }

        public void DrawDtrConfigUI()
        {
            DtrConfigWindow.Toggle();
        }
    }
}
