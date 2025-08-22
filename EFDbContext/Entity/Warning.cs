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
    /// 预警信息
    /// </summary>
    [Table("warning")]
    public class Warning
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 预警信息数据索引ID
        /// </summary>
        public Guid? dataId { get; set; }
    }

    [Table("warning_data")]
    public class WarningData
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 预警信息数据索引ID
        /// </summary>
        public Guid? dataId { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }

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
}
