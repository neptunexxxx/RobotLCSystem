using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 工序不良代码信息接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Base008_BadCode
        /// </summary>
        public string serviceId { get; set; } = "Base008_BadCode";

        /// <summary>
        /// 工厂编号，必填
        /// </summary>
        public string factoryCode { get; set; }

        /// <summary>
        /// 产线编号，非必须
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工序编号，非必须
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 发起时间，必填
        /// </summary>
        public string requestTime { get; set; }

        /// <summary>
        /// 上次更新时间，非必须
        /// </summary>
        public string? updateTime { get; set; }
    }

    public class 工序不良代码信息接口返回体
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
        /// 物料信息集合，必填
        /// </summary>
        public 工序不良代码信息接口返回体object? data { get; set; }

        /// <summary>
        /// 请求是否失败，true/false，非必须
        /// </summary>
        public bool? fail { get; set; }

        /// <summary>
        /// 请求失败原因，非必须
        /// </summary>
        public string? mesg { get; set; }

        /// <summary>
        /// 请求是否成功，true/false，非必须
        /// </summary>
        public bool? success { get; set; }

        /// <summary>
        /// 返回接收成功时间，13位时间戳，非必须
        /// </summary>
        public string? time { get; set; }
    }

    public class 工序不良代码信息接口返回体object
    {
        /// <summary>
        /// 添加时间，必填
        /// </summary>
        public string? addTime { get; set; }

        /// <summary>
        /// 不良代码，必填
        /// </summary>
        public string? badCode { get; set; }

        /// <summary>
        /// 不良代码名称，必填
        /// </summary>
        public string? badName { get; set; }

        /// <summary>
        /// 不良类型编码，必填
        /// </summary>
        public string? badTypeCode { get; set; }

        /// <summary>
        /// 不良类型名称，必填
        /// </summary>
        public string? badTypeName { get; set; }

        /// <summary>
        /// 数据状态，0-正常，1-删除，必填
        /// </summary>
        public string? dataStatus { get; set; }

        /// <summary>
        /// 更新时间(yyyy-MM-dd HH:mm:ss)，必填
        /// </summary>
        public string? editTime { get; set; }

        /// <summary>
        /// 启用状态，0-启用，1-禁用，必填
        /// </summary>
        public string? enableStatus { get; set; }

        /// <summary>
        /// 备用字段1，非必须
        /// </summary>
        public string? field1 { get; set; }

        /// <summary>
        /// 备用字段2，非必须
        /// </summary>
        public string? field2 { get; set; }

        /// <summary>
        /// 备用字段3，非必须
        /// </summary>
        public string? field3 { get; set; }

        /// <summary>
        /// 备用字段4，非必须
        /// </summary>
        public string? field4 { get; set; }

        /// <summary>
        /// 备用字段5，非必须
        /// </summary>
        public string? field5 { get; set; }

        /// <summary>
        /// 工序编号，非必须
        /// </summary>
        public string? operationCode { get; set; }
    }

}
