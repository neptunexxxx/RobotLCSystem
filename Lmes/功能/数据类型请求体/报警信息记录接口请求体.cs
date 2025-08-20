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
    /// 报警信息记录接口请求体
    /// </summary>
    public class 报警信息记录接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为System003_Alarmlog
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
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 请求参数集合
        /// </summary>
        public 报警信息请求参数? data { get; set; }
    }

    /// <summary>
    /// 报警信息请求参数
    /// </summary>
    public class 报警信息请求参数
    {
        /// <summary>
        /// 报警ID
        /// </summary>
        public int? alarmId { get; set; }

        /// <summary>
        /// 报警信息
        /// </summary>
        public string? alarmMesg { get; set; }

        /// <summary>
        /// 报警状态（0:创建，1:关闭）
        /// </summary>
        public int? alarmState { get; set; }

        /// <summary>
        /// 报警开始时间
        /// </summary>
        public string? startTime { get; set; }

        /// <summary>
        /// 报警结束时间
        /// </summary>
        public string? endTime { get; set; }

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

        /// <summary>
        /// 报警创建时间
        /// </summary>
        public string? createTime { get; set; }

        /// <summary>
        /// 报警创建用户
        /// </summary>
        public string? createBy { get; set; }

        /// <summary>
        /// 报警更新时间
        /// </summary>
        public string? updateTime { get; set; }

        /// <summary>
        /// 报警更新用户
        /// </summary>
        public string? updateBy { get; set; }

        /// <summary>
        /// 工厂编号
        /// </summary>
        public string? factoryCode { get; set; }
    }
    /// <summary>
    /// 报警信息记录接口返回体
    /// </summary>
    public class 报警信息记录接口返回体
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
