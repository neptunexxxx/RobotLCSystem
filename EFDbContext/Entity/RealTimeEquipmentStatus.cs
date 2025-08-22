using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    /// <summary>
    /// 设备状态消息类
    /// </summary>
    [Table("real_time_equipment_status")]
    public class RealTimeEquipmentStatus
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        [Key]
        public string? machineCode { get; set; }

        /// <summary>
        /// 设备状态编码
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

        /// <summary>
        /// 累计运行时长
        /// </summary>
        public string? totalRunningDuration { get; set; }

        /// <summary>
        /// 累计上电时长
        /// </summary>
        public string? totalOnLineDuration { get; set; }

        /// <summary>
        /// 单次故障时长
        /// </summary>
        public string? curFaultDuration { get; set; }

    }
}
