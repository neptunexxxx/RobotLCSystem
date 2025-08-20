using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 设备状态_IOT请求体
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
        /// 累计运行时长
        /// </summary>
        public string? totalRunningDuration { get; set; }

        /// <summary>
        /// 累计上电时长
        /// </summary>
        public string? totalOnLineDuration { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string? updateTime { get; set; }
    }
}
