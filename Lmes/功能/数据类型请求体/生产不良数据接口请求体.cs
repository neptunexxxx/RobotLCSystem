using System;
using System.Collections.Generic;

namespace Lmes.功能.数据类型请求体
{
    /// <summary>
    /// 生产不良数据接口请求体
    /// </summary>
    public class 生产不良数据接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Quality002_BadData
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
        public 不良数据请求参数? data { get; set; }
    }

    /// <summary>
    /// 不良数据请求参数
    /// </summary>
    public class 不良数据请求参数
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
        /// 产品编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// SN编码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string? userId { get; set; }

        /// <summary>
        /// 不良数据列表
        /// </summary>
        public 不良数据参数[]? datalist { get; set; }
        
    }
    public class 不良数据参数
    {
        /// <summary>
        /// 不良代码
        /// </summary>
        public string? badCode { get; set; }

        /// <summary>
        /// 不良因素
        /// </summary>
        public string? badFactor { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        public string? badQty { get; set; }

        /// <summary>
        /// 不良发生时间
        /// </summary>
        public string? editTime { get; set; }
    }
    /// <summary>
    /// 生产不良数据接口返回体
    /// </summary>
    public class 生产不良数据接口返回体
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
        /// 返回数据
        /// </summary>
        public string? data { get; set; }

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
        /// 返回接收成功时间
        /// </summary>
        public string? time { get; set; }
    }

    /// <summary>
    /// 不良数据
    /// </summary>
    public class 不良数据
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
        /// 产品编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// SN编码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 不良代码
        /// </summary>
        public string? badCode { get; set; }

        /// <summary>
        /// 不良因素
        /// </summary>
        public string? badFactor { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        public int? badQty { get; set; }

        /// <summary>
        /// 不良发生时间
        /// </summary>
        public DateTime? editTime { get; set; }
    }
}