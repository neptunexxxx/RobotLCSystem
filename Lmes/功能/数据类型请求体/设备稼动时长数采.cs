using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 设备稼动时长数采接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Product018_deviceStatusAndDuration
        /// </summary>
        public string? serviceId { get; set; } = "Product018_deviceStatusAndDuration";

        /// <summary>
        /// 工厂编号
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 请求时间，为发起请求的当前时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? machineCode { get; set; }

        /// <summary>
        /// 设备状态编码(数据字典：机台状态machine_status)
        /// </summary>
        public string? machineStatusCode { get; set; }

        /// <summary>
        /// 1:状态上传，2：累计时长上传，3：单次状态持续时长上传
        /// </summary>
        public string? requestMark { get; set; }

        /// <summary>
        ///运行时长单位：hour,minute,second;不填时默认为hour
        /// </summary>
        public string? timeUnit { get; set; }

        /// <summary>
        /// 数采设备编码：不能和machineCode同时为空
        /// </summary>
        public string? acquisitCode { get; set; }

        /// <summary>
        /// 累计运行时长：requestMark=2时为必填
        /// </summary>
        public string? totalRunningDuration { get; set; }

        /// <summary>
        /// 当次运时长：requestMark=3时为必填
        /// </summary>
        public string? curRunningDuration { get; set; }
    }
    public class 设备稼动时长数采接口返回体
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
