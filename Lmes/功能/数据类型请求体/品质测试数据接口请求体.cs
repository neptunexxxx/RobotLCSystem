using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    /// <summary>
    /// 品质测试数据接口请求体
    /// </summary>
    public class 品质测试数据接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Quality001_inspectiondata
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
        public 品质测试请求参数[]? data { get; set; }
    }

    /// <summary>
    /// 品质测试请求参数
    /// </summary>
    public class 品质测试请求参数
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
        /// 产品编号
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
        /// 参数编码
        /// </summary>
        public string? paramCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string? paramName { get; set; }

        /// <summary>
        /// 标准值
        /// </summary>
        public string? standardValue { get; set; }

        /// <summary>
        /// 参数范围下限
        /// </summary>
        public string? paramRange1 { get; set; }

        /// <summary>
        /// 参数范围上限
        /// </summary>
        public string? paramRange2 { get; set; }
        /// <summary>
        /// 实际值
        /// </summary>
        public string? realValue { get; set; }

        /// <summary>
        /// 测试开始时间
        /// </summary>
        public string? checkStartTime { get; set; }

        /// <summary>
        /// 测试结束时间
        /// </summary>
        public string? checkEndTime { get; set; }
    }
    public class 品质测试数据接口返回体
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
        /// 接口调用时间
        /// </summary>
        public string? time { get; set; }
    }
}
