﻿/*******************************************************
 * 
 * 作者：胡庆访
 * 创建日期：20150529
 * 运行环境：.NET 4.5
 * 版本号：1.0.0
 * 
 * 历史记录：
 * 创建文件 胡庆访 20150529 15:04
 * 
*******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Rafy;
using Rafy.ComponentModel;
using Rafy.Data;
using Rafy.Domain;
using Rafy.Domain.ORM;
using Rafy.Domain.ORM.Query;
using Rafy.Domain.Validation;
using Rafy.ManagedProperty;
using Rafy.MetaModel;
using Rafy.MetaModel.Attributes;
using Rafy.MetaModel.View;

namespace UT
{
    /// <summary>
    /// A的子实体
    /// </summary>
    [ChildEntity, Serializable]
    public partial class AChild : UnitTestEntity
    {
        #region 构造函数

        public AChild() { }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected AChild(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region 引用属性

        public static readonly IRefIdProperty AIdProperty =
            P<AChild>.RegisterRefId(e => e.AId, ReferenceType.Parent);
        public int AId
        {
            get { return (int)this.GetRefId(AIdProperty); }
            set { this.SetRefId(AIdProperty, value); }
        }
        public static readonly RefEntityProperty<A> AProperty =
            P<AChild>.RegisterRef(e => e.A, AIdProperty);
        public A A
        {
            get { return this.GetRefEntity(AProperty); }
            set { this.SetRefEntity(AProperty, value); }
        }

        #endregion

        #region 组合子属性

        #endregion

        #region 一般属性

        public static readonly Property<string> NameProperty = P<AChild>.Register(e => e.Name);
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }

        #endregion

        #region 只读属性

        public static readonly Property<string> RD_ANameProperty = P<AChild>.RegisterRedundancy(e => e.RD_AName,
            new RedundantPath(AProperty, A.NameProperty));
        public string RD_AName
        {
            get { return this.GetProperty(RD_ANameProperty); }
        }

        #endregion
    }

    /// <summary>
    /// A的子实体 列表类。
    /// </summary>
    [Serializable]
    public partial class AChildList : UnitTestEntityList { }

    /// <summary>
    /// A的子实体 仓库类。
    /// 负责 A的子实体 类的查询、保存。
    /// </summary>
    public partial class AChildRepository : UnitTestEntityRepository
    {
        /// <summary>
        /// 单例模式，外界不可以直接构造本对象。
        /// </summary>
        protected AChildRepository() { }
    }

    /// <summary>
    /// A的子实体 配置类。
    /// 负责 A的子实体 类的实体元数据的配置。
    /// </summary>
    internal class AChildConfig : UnitTestEntityConfig<AChild>
    {
        /// <summary>
        /// 配置实体的元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            //配置实体的所有属性都映射到数据表中。
            Meta.MapTable().MapAllProperties();
        }
    }
}