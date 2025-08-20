using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace Lmes.功能.数据类型请求体
{
    /// <summary>
    /// 预警信息接口请求体
    /// </summary>
    public class 预警信息接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为System001_Warning
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
        public 预警信息请求参数[]? data { get; set; }
    }

    /// <summary>
    /// 预警信息请求参数
    /// </summary>
    public class 预警信息请求参数
    {
        /// <summary>
        /// 异常预警时间编码
        /// </summary>
        public string? unusualAlarmCode { get; set; }

        /// <summary>
        /// 异常信息（报警文本+SN+工位+产线+设备名称）
        /// </summary>
        public string? message { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string? workShopCode { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? machineCode { get; set; }
    }
    /// <summary>
    /// 预警信息接口返回体
    /// </summary>
    public class 预警信息接口返回体
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
