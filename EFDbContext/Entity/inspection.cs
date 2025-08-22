using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("inspection")]
    public class Inspection
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
        /// 品质检测数据索引ID
        /// </summary>
        public Guid? dataId { get; set; }
    }

    [Table("inspection_data")]
    public class InspectionData
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 品质检测数据索引ID
        /// </summary>
        public Guid? dataId { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// SN编码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? operationCode { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        public string? paramCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string? paramName { get; set; }

        /// <summary>
        /// 标准值
        /// </summary>
        public string? standardValue { get; set; }

        /// <summary>
        /// 参数范围下限
        /// </summary>
        public string? paramRange1 { get; set; }

        /// <summary>
        /// 参数范围上限
        /// </summary>
        public string? paramRange2 { get; set; }
        /// <summary>
        /// 实际值
        /// </summary>
        public string? realValue { get; set; }

        /// <summary>
        /// 测试开始时间
        /// </summary>
        public string? checkStartTime { get; set; }

        /// <summary>
        /// 测试结束时间
        /// </summary>
        public string? checkEndTime { get; set; }
    }
}
