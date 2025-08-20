using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    /// <summary>
    /// 物料绑定信息接口请求体
    /// </summary>
    public class 物料绑定信息接口请求体
    {
        /// <summary>
        /// Service服务ID，固定为Product004_MaterialBind
        /// </summary>
        public string? serviceId { get; set; }

        /// <summary>
        /// 工厂编号
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 产线编号
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工位编号
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// SN条码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string? workOrderNumber { get; set; }

        /// <summary>
        /// 单体组件物料编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 单体组件物料版本
        /// </summary>
        public string? materialCodeVersion { get; set; }

        /// <summary>
        /// 单体组件物料条码
        /// </summary>
        public string? materialBarcode { get; set; }

        /// <summary>
        /// 物料绑定数据集合
        /// </summary>
        public List<物料绑定信息接口请求体Data>? data { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string? userId { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }
    }

    /// <summary>
    /// 物料绑定数据
    /// </summary>
    public class 物料绑定信息接口请求体Data
    {
        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工位编号
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string? orderCode { get; set; }

        /// <summary>
        /// 排程编号
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// SN条码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 子物料条码
        /// </summary>
        public string? assemblyMaterialCode { get; set; }

        /// <summary>
        /// 子物料数量
        /// </summary>
        public string? assemblyMaterialQty { get; set; }

        /// <summary>
        /// 子物料排序
        /// </summary>
        public string? assemblySort { get; set; }

        /// <summary>
        /// 子物料组装时间
        /// </summary>
        public string? assemblyTime { get; set; }

        /// <summary>
        /// 子物料名称
        /// </summary>
        public string? assemblyMaterialName { get; set; }

        /// <summary>
        /// 子物料版本
        /// </summary>
        public string? assemblyMaterialVersion { get; set; }

        /// <summary>
        /// 子物料追溯码
        /// </summary>
        public string? assemblyMaterialSn { get; set; }
    }
    /// <summary>
    /// 物料绑定信息接口返回体
    /// </summary>
    public class 物料绑定信息接口返回体
    {
        /// <summary>
        /// 000000代表请求成功，否则都代表请求失败
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 返回数据数量
        /// </summary>
        public int? count { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public string? data { get; set; }

        /// <summary>
        /// 请求是否失败
        /// </summary>
        public bool? fail { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string? mesg { get; set; }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool? success { get; set; }

        /// <summary>
        /// 返回接收成功时间
        /// </summary>
        public string? time { get; set; }
    }
}
