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
    [Table("equipment_status")]
    public class EquipmentStatus
    {
        [Key]
        public Guid Id { get; set; }

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
}
