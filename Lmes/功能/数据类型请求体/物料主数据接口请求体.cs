using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 物料主数据接口请求体
    {
        /// <summary>
        /// 服务ID，必填：Base002_Material
        /// </summary>
        public string? serviceId { get; set; } = "Base002_Material";

        /// <summary>
        /// 工厂编号，必填
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 产线编号，非必须
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 请求时间，必填
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 上次数据更新时间，非必须
        /// </summary>
        public string? updateTime { get; set; }
    }

    public class 物料主数据接口返回体
    {
        /// <summary>
        /// 请求结果标识码，000000代表请求成功，否则都代表请求失败
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 返回的数据数量，非必须
        /// </summary>
        public int? count { get; set; }

        /// <summary>
        /// 物料信息集合，非必须
        /// </summary>
        public 物料主数据接口返回体object data { get; set; }

        /// <summary>
        /// 请求是否失败，true/false，必填
        /// </summary>
        public bool? fail { get; set; }

        /// <summary>
        /// 返回失败的原因，必填
        /// </summary>
        public string? mesg { get; set; }

        /// <summary>
        /// 请求是否成功，true/false，必填
        /// </summary>
        public bool? success { get; set; }

        /// <summary>
        /// 返回接收成功时间，13位时间戳，必填
        /// </summary>
        public string? time { get; set; }
    }

    public class 物料主数据接口返回体object
    {
        /// <summary>
        /// 新增时间（yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public string? addTime { get; set; }

        /// <summary>
        /// 数据状态：0-正常，1-删除
        /// </summary>
        public string? dataStatus { get; set; }

        /// <summary>
        /// 更新时间（yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public string? editTime { get; set; }

        /// <summary>
        /// 物料状态：0-启用，1-禁用
        /// </summary>
        public string? enableStatus { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string? id { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 物料类型：原材料，半成品，成品，虚拟物料，外来成品
        /// </summary>
        public string? materialType { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string? measureUnitCode { get; set; }

        /// <summary>
        /// 计量单位名称
        /// </summary>
        public string? measureUnitName { get; set; }

        /// <summary>
        /// 产品模型编码
        /// </summary>
        public string? productModelCode { get; set; }

        /// <summary>
        /// 产品模型ID
        /// </summary>
        public string? productModelId { get; set; }

        /// <summary>
        /// 条码正则表达式
        /// </summary>
        public string? regular { get; set; }
    }


}
