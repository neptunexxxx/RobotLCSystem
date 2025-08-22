using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("material_trace_passStation")]
    public class MaterialTracePassStation
    {
        [Key]
        public Guid? Id { get; set; }
        /// <summary>
        /// SN编码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? orderNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? startStationCode { get; set; }

        /// <summary>
        /// 排程号
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string? userId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string? userName { get; set; }

        /// <summary>
        /// 过站状态
        /// </summary>
        public string? passStatus { get; set; }

        /// <summary>
        /// 入站时间
        /// </summary>
        public string? inStationTime { get; set; }

        /// <summary>
        /// 出站时间
        /// </summary>
        public string? outStationTime { get; set; }

        /// <summary>
        /// 报工数量
        /// </summary>
        public string? workReportQty { get; set; }

        /// <summary>
        /// 条码信息索引ID
        /// </summary>
        public Guid dataId { get; set; }
    }

    [Table("material_trace_passStation_data")]
    public class MaterialTracePassStationData
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 条码信息索引ID
        /// </summary>
        public Guid? dataId { get; set; }

        /// <summary>
        /// 条码信息
        /// </summary>
        public string? barCode { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public string? acquisitionTime { get; set; }
    }
}
