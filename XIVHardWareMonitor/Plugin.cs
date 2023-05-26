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
using ECommons.DalamudServices;
using ECommons.LanguageHelpers;
using MSIAfterburnerNET.HM;

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
        // AfterBurner
        public static HardwareMonitor AfterBurner { get; set; }=null!;
        public static int AfterCount = 0;
        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            Svc.Init(pluginInterface);
            Localization.Init(Localization.GameLanguageString);
#if DEBUG
            ChatGui.Print("TestStr".Loc());
#endif
            ChatGui.Print("XIV HardWare Monitor loaded.^ ^");
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);
            Configuration.Language = Localization.GameLanguageString;
            // 获取硬件监控
            // 初始化小飞机
            TryInitAfterBurner();
            try
            {
                computer = new Computer()
                {
                    IsCpuEnabled = true,
                    IsGpuEnabled = true,
                    IsMemoryEnabled = true,
                    IsMotherboardEnabled = true,
                    IsControllerEnabled = true,
                    IsNetworkEnabled = true,
                    IsStorageEnabled = true,
                    IsBatteryEnabled = true,
                    IsPsuEnabled = true,
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
            HardwareDtrBar.Text = "Monitor".Loc();
            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this);
            DtrConfigWindow = new DtrConfigWindow(this);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);
            WindowSystem.AddWindow(DtrConfigWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "/hardware Open main window.".Loc()
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            watcher = new Watcher(this);
        }
        // 尝试初始化Afterburner
        public static void TryInitAfterBurner()
        {
            if (AfterBurner is not null)
            {
                ChatGui.PrintError("AfterBurner is not null. if an error occors, you may neet to restart FFXIV to use MSIAfterburner monitor".Loc());
                return;
            }
            try
            {

                AfterBurner = new HardwareMonitor();
            }
            catch (Exception e)
            {
                Dalamud.Logging.PluginLog.Error($"小飞机初始化失败{e.Message}");
                return;
            }
            AfterBurner.Refresh();
            AfterCount = AfterBurner.Entries.Length;
        }

        public void UpdateTitles()
        {
            MainWindow.UpdateTitle();
            DtrConfigWindow.UpdateTitle();
            ConfigWindow.UpdateTitle();
        }
        public void Dispose()
        {
            Dalamud.Logging.PluginLog.Log("开始卸载Hardware Monitor");
            watcher.Dispose();
            computer.Close();
            WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            DtrConfigWindow.Dispose();
            HardwareDtrBar.Remove();
            HardwareDtrBar.Dispose();
            CommandManager.RemoveHandler(CommandName);
            if (AfterBurner!=null)
            {
                AfterBurner.Dispose();
            }
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
