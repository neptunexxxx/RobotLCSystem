using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("alarmlog")]
    public class Alarmlog
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
        /// 报警信息记录数据索引ID
        /// </summary>
        public Guid? dataId { get; set; }
    }

    [Table("alarmlog_data")]
    public class AlarmlogData
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 报警信息记录数据索引ID
        /// </summary>
        public Guid? dataId { get; set; }

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
}
