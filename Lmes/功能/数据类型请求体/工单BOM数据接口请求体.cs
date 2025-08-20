using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 工单BOM数据接口请求体
    {
        /// <summary>
        /// 服务ID，必填：Base004_OrderBom
        /// </summary>
        public string? serviceId { get; set; } = "Base004_OrderBom";

        /// <summary>
        /// 工厂编号，必填
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 产线编号，必填
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工单编码，非必须
        /// </summary>
        public string? orderCode { get; set; }

        /// <summary>
        /// 排程编码，非必须
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// 发起时间，必填
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 上次数据更新时间，非必须
        /// </summary>
        public string? updateTime { get; set; }
    }

    public class 工单BOM数据接口返回体
    {
        /// <summary>
        /// 请求结果标识码，000000代表请求成功，否则都代表请求失败
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 返回的数据数量，非必须
        /// </summary>
        public int? count { get; set; }

        /// <summary>
        /// 数据集合，必填
        /// </summary>
        public 工单BOM数据接口返回体object[] data { get; set; }

        /// <summary>
        /// 请求是否失败，true/false，必填
        /// </summary>
        public bool? fail { get; set; }

        /// <summary>
        /// 返回失败的原因，非必须
        /// </summary>
        public string? mesg { get; set; }

        /// <summary>
        /// 请求是否成功，true/false，必填
        /// </summary>
        public bool? success { get; set; }

        /// <summary>
        /// 返回接收成功时间，13位时间戳，必填
        /// </summary>
        public string? time { get; set; }
    }

    public class 工单BOM数据接口返回体object
    {
        /// <summary>
        /// 创建时间 (yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public string? addTime { get; set; }

        /// <summary>
        /// 组件物料用量
        /// </summary>
        public string? assemblyConsumption { get; set; }

        /// <summary>
        /// 组件物料编号
        /// </summary>
        public string? assemblyMaterialCode { get; set; }

        /// <summary>
        /// 组件物料ID
        /// </summary>
        public string? assemblyMaterialId { get; set; }

        /// <summary>
        /// 组件物料名称
        /// </summary>
        public string? assemblyMaterialName { get; set; }

        /// <summary>
        /// 组件物料版本
        /// </summary>
        public string? assemblyMaterialVersion { get; set; }

        /// <summary>
        /// 组件物料单位
        /// </summary>
        public string? assemblyUnitCode { get; set; }

        /// <summary>
        /// 更新时间 (yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public string? editTime { get; set; }

        /// <summary>
        /// 备用字段1，非必须
        /// </summary>
        public string? field1 { get; set; }

        /// <summary>
        /// 备用字段2，非必须
        /// </summary>
        public string? field2 { get; set; }

        /// <summary>
        /// 备用字段3，非必须
        /// </summary>
        public string? field3 { get; set; }

        /// <summary>
        /// 备用字段4，非必须
        /// </summary>
        public string? field4 { get; set; }

        /// <summary>
        /// 备用字段5，非必须
        /// </summary>
        public string? field5 { get; set; }

        /// <summary>
        /// 产线编号
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public string? lineId { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string? lineName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public string? materialId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? orderCode { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public string? orderId { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public string? orderQty { get; set; }

        /// <summary>
        /// BOM ID
        /// </summary>
        public string? productBomId { get; set; }

        /// <summary>
        /// BOM类型
        /// </summary>
        public string? productBomType { get; set; }

        /// <summary>
        /// BOM版本
        /// </summary>
        public string? productBomVersion { get; set; }

        /// <summary>
        /// 追溯方式
        /// </summary>
        public string? retroactive { get; set; }

        /// <summary>
        /// 排程编码
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// 标准工时，非必须
        /// </summary>
        public string? standWorkHours { get; set; }
    }
}
