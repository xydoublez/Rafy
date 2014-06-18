﻿/*******************************************************
 * 
 * 作者：胡庆访
 * 创建时间：20110331
 * 说明：此文件只包含一个类，具体内容见类型注释。
 * 运行环境：.NET 4.0
 * 版本号：1.0.0
 * 
 * 历史记录：
 * 创建文件 胡庆访 20100331
 * 
*******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Rafy.MetaModel;
using System.Diagnostics;

namespace Rafy
{
    /// <summary>
    /// 表示某一个插件程序集
    /// </summary>
    [DebuggerDisplay("{Assembly.FullName}")]
    public class PluginAssembly
    {
        public static readonly IPlugin EmptyPlugin = new _EmptyPlugin();

        public PluginAssembly(Assembly assembly, IPlugin instance)
        {
            this.Instance = instance;
            this.Assembly = assembly;
        }

        /// <summary>
        /// 程序集当中的插件对象。
        /// 如果插件中没有定义，则此属性为 null。
        /// </summary>
        public IPlugin Instance { get; private set; }

        /// <summary>
        /// 程序集本身
        /// </summary>
        public Assembly Assembly { get; private set; }

        private class _EmptyPlugin : IPlugin
        {
            int IPlugin.SetupLevel { get { return ReuseLevel.Main; } }

            public void Initialize(IApp app) { }
        }
    }
}