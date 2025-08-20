using System;
using System.Collections.Generic;

namespace Lmes.功能.数据类型请求体
{
    /// <summary>
    /// 物料追溯及生产过站接口请求体
    /// </summary>
    public class 物料追溯及生产过站接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Product009_MaterialTracePassStation
        /// </summary>
        public string? serviceId { get; set; }

        /// <summary>
        /// 工厂编号
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 请求参数集合
        /// </summary>
        public 物料追溯及生产过站请求参数[]? data { get; set; }
    }

    /// <summary>
    /// 物料追溯及生产过站请求参数
    /// </summary>
    public class 物料追溯及生产过站请求参数
    {
        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// SN编码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? orderNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? startStationCode { get; set; }

        /// <summary>
        /// 排程号
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string? userId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string? userName { get; set; }

        /// <summary>
        /// 过站状态
        /// </summary>
        public string? passStatus { get; set; }

        /// <summary>
        /// 入站时间
        /// </summary>
        public string? inStationTime { get; set; }

        /// <summary>
        /// 出站时间
        /// </summary>
        public string? outStationTime { get; set; }

        /// <summary>
        /// 报工数量
        /// </summary>
        public string? workReportQty { get; set; }

        /// <summary>
        /// 条码信息集合
        /// </summary>
        public 条码信息[]? barCodeList { get; set; }
    }

    /// <summary>
    /// 条码信息
    /// </summary>
    public class 条码信息
    {
        /// <summary>
        /// 条码信息
        /// </summary>
        public string? barCode { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public string? acquisitionTime { get; set; }
    }
    /// <summary>
    /// 物料追溯及生产过站接口返回体
    /// </summary>
    public class 物料追溯及生产过站接口返回体
    {
        /// <summary>
        /// 状态编码，000000代表返回成功
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 返回数据数量
        /// </summary>
        public int? count { get; set; }

        /// <summary>
        /// 请求是否失败
        /// </summary>
        public bool? fail { get; set; }

        /// <summary>
        /// 接口返回信息
        /// </summary>
        public string? mesg { get; set; }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool? success { get; set; }

        /// <summary>
        /// 接口调用时间
        /// </summary>
        public string? time { get; set; }
    }
}
