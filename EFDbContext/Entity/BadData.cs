using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("bad_information")]
    public class BadInformation
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime {  get; set; }

        /// <summary>
        /// 生产不良信息数据索引ID
        /// </summary>
        public Guid? dataId { get; set; }
    }

    [Table("bad_information_data")]
    public class BadInformationData
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 生产不良信息数据索引ID
        /// </summary>
        public Guid? dataId { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工位编号
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 产品编码
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
        /// 用户id
        /// </summary>
        public string? userId { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 不良数据列表索引ID
        /// </summary>
        public Guid datalistId { get; set; }
    }

    [Table("bad_information_datalist")]
    public class BadInformationDataList
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 不良数据列表索引ID
        /// </summary>
        public Guid? datalistId { get; set; }

        /// <summary>
        /// 不良代码
        /// </summary>
        public string? badCode { get; set; }

        /// <summary>
        /// 不良因素
        /// </summary>
        public string? badFactor { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        public string? badQty { get; set; }

        /// <summary>
        /// 不良发生时间
        /// </summary>
        public string? editTime { get; set; }
    }
}
