using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 工单物料替代信息接口请求体
    {
        /// <summary>
        /// 服务ID，必填：Base004_OrderMaterialReplace
        /// </summary>
        public string? serviceId { get; set; } = "Base004_OrderMaterialReplace";

        /// <summary>
        /// 工厂编号，必填
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 产线编号，必填
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 排程编码，必填
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// 请求时间，必填
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 工单编码，非必须
        /// </summary>
        public string? orderCode { get; set; }
    }

    public class 工单物料替代信息接口返回体
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
        /// 工单物料替代数据集合，必填
        /// </summary>
        public 工单物料替代信息接口返回体object[]? data { get; set; }

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
        /// 返回接收成功时间，13位时间戳，非必须
        /// </summary>
        public string? time { get; set; }
    }

    public class 工单物料替代信息接口返回体object
    {
        /// <summary>
        /// 组件数量
        /// </summary>
        public string? assemblyConsumption { get; set; }

        /// <summary>
        /// 组件行号
        /// </summary>
        public string? assemblyLine { get; set; }

        /// <summary>
        /// 组件物料号
        /// </summary>
        public string? assemblyMaterialCode { get; set; }

        /// <summary>
        /// 组件物料版本
        /// </summary>
        public string? assemblyVersion { get; set; }

        /// <summary>
        /// 启用状态，非必须
        /// </summary>
        public string? enableStatus { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string? measureUnitCode { get; set; }

        /// <summary>
        /// 装配工序编号
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string? orderCode { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string? productNumber { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string? productVersion { get; set; }

        /// <summary>
        /// 替换物料数量
        /// </summary>
        public string? replaceConsumption { get; set; }

        /// <summary>
        /// 替换物料编号
        /// </summary>
        public string? replaceMaterialCode { get; set; }

        /// <summary>
        /// 替换物料版本
        /// </summary>
        public string? replaceMaterialVersion { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public string? replacePriority { get; set; }

        /// <summary>
        /// 替换组ID，非必须
        /// </summary>
        public string? replacesItemId { get; set; }
    }
}
