using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("product_parameters")]
    public class ProductParameters
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 产品SN号
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? machineCode { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 产品物料版本
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// 生产参数记录时间
        /// </summary>
        public string? paramTime { get; set; }

        /// <summary>
        /// 数据集合索引ID
        /// </summary>
        public Guid? dataId { get; set; }
    }

    [Table("product_parameters_data")]
    public class ProductParametersData
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 数据集合索引ID
        /// </summary>
        public Guid? dataId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string? paramCode { get; set; }

        /// <summary>
        /// 参数描述
        /// </summary>
        public string? paramName { get; set; }

        /// <summary>
        /// 标准值
        /// </summary>
        public string? standardValue { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public string? realValue { get; set; }

        /// <summary>
        /// 参数下限
        /// </summary>
        public string? paramRange1 { get; set; }

        /// <summary>
        /// 参数上限
        /// </summary>
        public string? paramRange2 { get; set; }

        /// <summary>
        /// 判定结果（0，不合格；1，合格）
        /// </summary>
        public string? checkResult { get; set; }
    }
}
