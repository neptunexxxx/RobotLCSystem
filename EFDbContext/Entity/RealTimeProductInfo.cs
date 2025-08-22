using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("real_time_product_info")]
    public class RealTimeProductInfo
    {
        /// <summary>
        /// 产品SN
        /// </summary>
        [Key]
        public string? snNumber { get; set; }
		/// <summary>
		/// 托盘码
		/// </summary>
		public string? virtualSN { get; set; }

		/// <summary>
		/// 当前工位编号（过站后更新）
		/// </summary>
		public string? stationCode { get; set; }

        /// <summary>
        /// 排程编号
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// 当前工序编号
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 是否为不良品
        /// </summary>
        public bool? isbad {  get; set; }

        /// <summary>
        /// NG站
        /// </summary>
        public string? ngStation { get; set; }

        /// <summary>
        /// 重新加工次数
        /// </summary>
        public int? reProductCount { get; set; } = 0;

        /// <summary>
        /// 进站时间
        /// </summary>
        public string passBeginTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 出站时间
        /// </summary>
        public string passEndTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 更新时间
        /// </summary>
        public string updateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 产线编号
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string? materialCode { get; set; }
    }
}
