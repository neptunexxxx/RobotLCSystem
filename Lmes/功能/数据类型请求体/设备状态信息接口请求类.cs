using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    /// <summary>
    /// 设备状态信息接口请求体
    /// </summary>
    public class 设备状态信息接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Product005_EquipmentStatus
        /// </summary>
        public string? serviceId { get; set; } = "Product005_EquipmentStatus";

        /// <summary>
        /// 工厂编号
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 请求时间，为发起请求的当前时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 请求参数集合
        /// </summary>
        public List<设备状态信息请求参数>? data { get; set; } = new();
    }

    /// <summary>
    /// 设备状态信息请求参数
    /// </summary>
    public class 设备状态信息请求参数
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string? machineCode { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工位编号
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 设备状态编码(数据字典：机台状态machine_status)
        /// </summary>
        public string? machineStatusCode { get; set; }

        /// <summary>
        /// 设备状态开始时间
        /// </summary>
        public string? machineStatusBegin { get; set; }

        /// <summary>
        /// 设备状态结束时间
        /// </summary>
        public string? machineStatusEnd { get; set; }
    }

    /// <summary>
    /// 设备状态信息接口返回体
    /// </summary>
    public class 设备状态信息接口返回体
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
        public bool success { get; set; }

        /// <summary>
        /// 接口调用时间
        /// </summary>
        public string? time { get; set; }
    }
}
