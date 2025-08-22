using System.ClientModel.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDbContext.Entity
{
	/// <summary>
	/// 设备稼动时长数采
	/// </summary>
	[Table("device_status_and_duration")]
    public class DeviceStatusAndDuration
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 设备编码：不能和acquisitCode同时为空
        /// </summary>
        public string? machineCode { get; set; }
        /// <summary>
        /// 设备状态编码：requestMark=1时为必填：offline:离线; normal:正常; repair:报修; maintain:维修; scrap:报废;onMachine:上机; inMaintain:保养;inLibrary:在库; free:空闲; fault:故障;overhaul:检修; mothballed:封存;toBeScrap:待报废; loss:遗失; inUse:使用中;
        /// </summary>
        public string? machineStatusCode { get; set; }
        /// <summary>
        /// 运行时长单位
        /// </summary>
        public string? timeUnit { get; set; }
        /// <summary>
        /// 数采设备编码
        /// </summary>
        public string? acquisitCode { get; set; }
        /// <summary>
        /// 累计运行时长
        /// </summary>
        public string? totalRunningDuration { get; set; }
        /// <summary>
        /// 当次运行时长
        /// </summary>
        public string? curRunningDuration { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }
    }
}
