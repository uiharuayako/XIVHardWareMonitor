﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace XIVHardWareMonitor.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("XIVHardWareMonitor.Properties.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 {  
        ///  &quot;WindowName&quot;:&quot;XIV Hardware Monitor Settings&quot;,
        ///  &quot;Home&quot;:&quot;Home&quot;,
        ///  &quot;DtrSet&quot;:&quot;Dtr Settings&quot;,
        ///  &quot;Language&quot;:&quot;Language&quot;,
        ///  &quot;RefreshRate&quot;:&quot;Refresh Rate&quot;,
        ///  &quot;EnableCPU&quot;:&quot;Enable CPU Watcher&quot;,
        ///  &quot;EnableGPU&quot;:&quot;Enable GPU Watcher&quot;,
        ///  &quot;EnableMEM&quot;:&quot;Enable Memory Watcher&quot;,
        ///  &quot;EnableMB&quot;:&quot;Enable Motherboard Watcher&quot;,
        ///  &quot;EnableController&quot;:&quot;Enable Controller Watcher&quot;,
        ///  &quot;EnableNetwork&quot;:&quot;Enable Network Watcher&quot;,
        ///  &quot;EnableStorage&quot;:&quot;Enable Storage Watcher&quot;,
        ///  &quot;EnableBattery&quot;:&quot;Enable Battery Watcher&quot;,
        ///  &quot;Enable [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        public static string ConfigWindow_en {
            get {
                return ResourceManager.GetString("ConfigWindow_en", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {
        ///  &quot;WindowName&quot;:&quot;硬件监控设置&quot;,
        ///  &quot;Home&quot;:&quot;主界面&quot;,
        ///  &quot;DtrSet&quot;:&quot;状态栏设置&quot;,
        ///  &quot;Language&quot;:&quot;语言&quot;,
        ///  &quot;RefreshRate&quot;:&quot;刷新间隔&quot;,
        ///  &quot;EnableCPU&quot;:&quot;监控CPU信息&quot;,
        ///  &quot;EnableGPU&quot;:&quot;监控GPU信息&quot;,
        ///  &quot;EnableMEM&quot;:&quot;监控内存信息&quot;,
        ///  &quot;EnableMB&quot;:&quot;监控主板信息&quot;,
        ///  &quot;EnableController&quot;:&quot;监控控制器信息&quot;,
        ///  &quot;EnableNetwork&quot;:&quot;监控网络信息&quot;,
        ///  &quot;EnableStorage&quot;:&quot;监控储存信息&quot;,
        ///  &quot;EnableBattery&quot;:&quot;监控电池信息&quot;,
        ///  &quot;EnablePsu&quot;:&quot;监控电源信息&quot;,
        ///  &quot;Save&quot;:&quot;保存&quot;
        ///}
        /// 的本地化字符串。
        /// </summary>
        public static string ConfigWindow_zh {
            get {
                return ResourceManager.GetString("ConfigWindow_zh", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {
        ///    &quot;WindowName&quot;: &quot;DtrBar Settings&quot;,
        ///    &quot;Enable&quot;: &quot;Enable&quot;,
        ///    &quot;Name&quot;: &quot;Name&quot;,
        ///    &quot;Display&quot;: &quot;Display&quot;,
        ///    &quot;DisplayHelp&quot;: &quot;Using placeholders to refer info\n&lt;name&gt;:name\n&lt;value&gt;:value\n&lt;unit&gt;:unit&quot;,
        ///    &quot;Decimal&quot;: &quot;Decimal&quot;,
        ///    &quot;Operation&quot;: &quot;Operation&quot;,
        ///    &quot;EnableMB&quot;: &quot;Enable Motherboard Watcher&quot;,
        ///    &quot;EnableController&quot;: &quot;Enable Controller Watcher&quot;,
        ///    &quot;EnableNetwork&quot;: &quot;Enable Network Watcher&quot;,
        ///    &quot;EnableStorage&quot;: &quot;Enable Storage Watcher&quot;,
        ///    &quot;EnableBattery&quot;: &quot;Enable Battery Watcher&quot;, [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        public static string DtrConfigWindow_en {
            get {
                return ResourceManager.GetString("DtrConfigWindow_en", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {
        ///    &quot;WindowName&quot;: &quot;状态栏设置&quot;,
        ///    &quot;Enable&quot;: &quot;启用&quot;,
        ///    &quot;Name&quot;: &quot;名称&quot;,
        ///    &quot;Display&quot;: &quot;状态栏显示&quot;,
        ///    &quot;DisplayHelp&quot;: &quot;使用占位符指代信息\n&lt;name&gt;:传感器名称\n&lt;value&gt;:值\n&lt;unit&gt;:单位&quot;,
        ///    &quot;Decimal&quot;: &quot;小数位&quot;,
        ///    &quot;Operation&quot;: &quot;操作&quot;,
        ///    &quot;EnableMEM&quot;: &quot;监控内存信息&quot;,
        ///    &quot;EnableMB&quot;: &quot;监控主板信息&quot;,
        ///    &quot;EnableController&quot;: &quot;监控控制器信息&quot;,
        ///    &quot;EnableNetwork&quot;: &quot;监控网络信息&quot;,
        ///    &quot;EnableStorage&quot;: &quot;监控储存信息&quot;,
        ///    &quot;EnableBattery&quot;: &quot;监控内存信息&quot;,
        ///    &quot;EnablePsu&quot;: &quot;监控电源信息&quot;,
        ///    &quot;Save&quot;: &quot;保存&quot;
        ///}
        /// 的本地化字符串。
        /// </summary>
        public static string DtrConfigWindow_zh {
            get {
                return ResourceManager.GetString("DtrConfigWindow_zh", resourceCulture);
            }
        }
    }
}
