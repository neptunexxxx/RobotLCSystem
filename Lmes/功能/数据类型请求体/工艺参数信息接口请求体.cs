using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 工艺参数信息接口请求体
    {
        /// <summary>
        /// 服务ID，必填：Base006_Routeparameters
        /// </summary>
        public string? serviceId { get; set; } = "Base006_Routeparameters";

        /// <summary>
        /// 工厂编号，必填
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 机台编码，非必填
        /// </summary>
        public string? machineCode { get; set; }

        /// <summary>
        /// 产线编号，非必须
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 工序编号，非必须
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 请求时间，必填
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 数据更新时间，必填
        /// </summary>
        public string? updateTime { get; set; }
    }

    public class 工艺参数信息接口返回体
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
        /// 工单工艺路径信息集合，非必须
        /// </summary>
        public 工艺参数信息接口返回体object[]? data { get; set; }

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

    public class 工艺参数信息接口返回体object
    {
        /// <summary>
        /// 添加时间，非必须
        /// </summary>
        public string? addTime { get; set; }

        /// <summary>
        /// 编辑时间，非必须
        /// </summary>
        public string? editTime { get; set; }

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
        /// 设备编码，必填
        /// </summary>
        public string? machineCode { get; set; }

        /// <summary>
        /// 产品编号，必填
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 产品名称，必填
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 产品版本，必填
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// 工序编码，必填
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 参数编码，必填
        /// </summary>
        public string? paramCode { get; set; }

        /// <summary>
        /// 参数名称，必填
        /// </summary>
        public string? paramName { get; set; }

        /// <summary>
        /// 下限值，必填
        /// </summary>
        public string? standardRange1 { get; set; }

        /// <summary>
        /// 上限值，必填
        /// </summary>
        public string? standardRange2 { get; set; }

        /// <summary>
        /// 标准值，必填
        /// </summary>
        public string? standardValue { get; set; }

        /// <summary>
        /// 目标值，必填
        /// </summary>
        public string? targetValue { get; set; }
    }

}
