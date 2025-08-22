using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("material_bind")]
    public class MaterialBind
    {
        [Key]
        public Guid? Id { get; set; }
        /// <summary>
        /// SN条码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 工位编号
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string? workOrderNumber { get; set; }

        /// <summary>
        /// 单体组件物料编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 单体组件物料版本
        /// </summary>
        public string? materialCodeVersion { get; set; }

        /// <summary>
        /// 单体组件物料条码
        /// </summary>
        public string? materialBarcode { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string? userId { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 物料绑定数据集合ID
        /// </summary>
        public Guid? dataId { get; set; }
    }

    [Table("material_bind_data")]
    public class MaterialBindData
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 数据索引ID
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
        /// 工单编号
        /// </summary>
        public string? orderCode { get; set; }

        /// <summary>
        /// 排程编号
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// SN条码
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 子物料条码
        /// </summary>
        public string? assemblyMaterialCode { get; set; }

        /// <summary>
        /// 子物料数量
        /// </summary>
        public string? assemblyMaterialQty { get; set; }

        /// <summary>
        /// 子物料排序
        /// </summary>
        public string? assemblySort { get; set; }

        /// <summary>
        /// 子物料组装时间
        /// </summary>
        public string? assemblyTime { get; set; }

        /// <summary>
        /// 子物料名称
        /// </summary>
        public string? assemblyMaterialName { get; set; }

        /// <summary>
        /// 子物料版本
        /// </summary>
        public string? assemblyMaterialVersion { get; set; }

        /// <summary>
        /// 子物料追溯码
        /// </summary>
        public string? assemblyMaterialSn { get; set; }
    }
}
