using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("sn_create")]
    public class SNCreate
    {
        /// <summary>
        /// 产品SN号，为产品的唯一标识
        /// </summary>
        [Key]
        public string? snNumber { get; set; }
        /// <summary>
        /// 工厂编号
        /// </summary>
        public string? factoryCode { get; set; }
        /// <summary>
        /// 产线编号
        /// </summary>
        public string? lineCode { get; set; }
        /// <summary>
        /// 工位编号
        /// </summary>
        public string? stationCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? materialCode { get; set; }
        /// <summary>
        /// 排程编号
        /// </summary>
        public string? scheduleCode { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string? userId { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }
    }
}
