/*******************************************************
 * 
 * 作者：胡庆访
 * 创建时间：20100408
 * 说明：此文件只包含一个类，具体内容见类型注释。
 * 运行环境：.NET 4.0
 * 版本号：1.0.0
 * 
 * 历史记录：
 * 创建文件 胡庆访 20100408
 * 不再使用类名DefaultUIFactory，同时删除Commands相关方法。 胡庆访 20100216
 * 
*******************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using AvalonDock;

using Itenso.Windows.Input;
using OEA.Editors;
using OEA.Library;

using OEA.MetaModel;
using OEA.MetaModel.View;
using OEA.Module.WPF.CommandAutoUI;
using OEA.Module.WPF.Controls;
using OEA.Module.WPF.Editors;


using OEA.WPF.Command;

using System.Windows.Controls.Primitives;
using OEA.Module.WPF.Automation;
using System.Windows.Input;

namespace OEA.Module.WPF
{
    /// <summary>
    /// 界面中最某一具体的UI块的控件生成工厂
    /// </summary>
    public class BlockUIFactory
    {
        public BlockUIFactory(PropertyEditorFactory propertyEditorFactory)
        {
            if (propertyEditorFactory == null) throw new ArgumentNullException("propertyEditorFactory");
            this.PropertyEditorFactory = propertyEditorFactory;

            this.TreeColumnFactory = new TreeColumnFactory(propertyEditorFactory);
        }

        /// <summary>
        /// 属性编辑器工厂
        /// </summary>
        public PropertyEditorFactory PropertyEditorFactory { get; private set; }

        /// <summary>
        /// 树型控件的列工厂
        /// </summary>
        public TreeColumnFactory TreeColumnFactory { get; private set; }

        /// <summary>
        /// 自动生成树形列表UI
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="showInWhere"></param>
        /// <returns></returns>
        public virtual MultiTypesTreeGrid CreateTreeListControl(EntityViewMeta vm, ShowInWhere showInWhere)
        {
            if (vm == null) throw new ArgumentNullException("vm");

            //装载多个对象的属性
            var propInfos = vm.OrderedEntityProperties().ToList();

            //使用MultiTypesTreeGrid作为TreeListControl
            var treeListControl = new MultiTypesTreeGrid(vm);

            //使用list里面的属性生成每一列
            var columns = treeListControl.Columns;
            foreach (var propertyViewInfo in propInfos)
            {
                if (propertyViewInfo.CanShowIn(showInWhere))
                {
                    var column = this.TreeColumnFactory.Create(propertyViewInfo);

                    columns.Add(column);
                }
            }

            return treeListControl;
        }

        /// <summary>
        /// 生成一个AutoGrid，用于承载所有的字段显示控件。
        /// </summary>
        /// <param name="detailView">逻辑视图</param>
        /// <returns></returns>
        public virtual FrameworkElement CreateDetailPanel(DetailObjectView detailView)
        {
            if (detailView == null) throw new ArgumentNullException("detailView");

            FrameworkElement result = null;

            var detailType = detailView.Meta.DetailPanelType;
            if (detailType == null)
            {
                result = new DefaultDetailPanel();
            }
            else
            {
                result = Activator.CreateInstance(detailType) as FrameworkElement;
            }

            EditorHost.SetDetailObjectView(result, detailView);

            return result;
        }

        public void AppendCommands(
            ItemsControl commandsContainer,
            object commandArg,
            params WPFCommand[] availableCommands
            )
        {
            this.AppendCommands(commandsContainer, commandArg, availableCommands as IEnumerable<WPFCommand>);
        }

        public virtual void AppendCommands(
            ItemsControl commandsContainer,
            object commandArg,
            IEnumerable<WPFCommand> availableCommands
            )
        {
            new CommandAutoUIManager().Generate(commandsContainer, commandArg, availableCommands);
        }
    }
}