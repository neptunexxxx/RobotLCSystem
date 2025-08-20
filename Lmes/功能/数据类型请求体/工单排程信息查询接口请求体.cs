using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 工单排程信息查询接口请求体
    {
        /// <summary>
        /// 服务ID，必填：Base003_OrderList
        /// </summary>
        public string? serviceId { get; set; } = "Base003_OrderList";

        /// <summary>
        /// 工厂编号，必填
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 产线编号，必填
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 排程编号，非必须，查询指定排程时使用
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// 发起时间，必填
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 上次更新时间，非必须
        /// </summary>
        public string? updateTime { get; set; }
    }

    public class 工单排程信息查询接口返回体
    {
        /// <summary>
        /// 请求结果标识码，非必须，000000代表请求成功，否则都代表请求失败
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 返回的数据数量，非必须
        /// </summary>
        public int? count { get; set; }

        /// <summary>
        /// 排程数据集合，非必须
        /// </summary>
        public 工单排程信息查询接口返回体object[]? data { get; set; }

        /// <summary>
        /// 请求是否失败，必填，true/false
        /// </summary>
        public bool? fail { get; set; }

        /// <summary>
        /// 返回失败的原因，非必须
        /// </summary>
        public string? mesg { get; set; }

        /// <summary>
        /// 请求是否成功，必填，true/false
        /// </summary>
        public bool? success { get; set; }

        /// <summary>
        /// 返回接收成功时间，必填，13位时间戳
        /// </summary>
        public string? time { get; set; }
    }

    public class 工单排程信息查询接口返回体object
    {
        /// <summary>
        /// 计划结束时间 (yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public string? endTime { get; set; }

        /// <summary>
        /// 自定义字段1
        /// </summary>
        public string? field1 { get; set; }

        /// <summary>
        /// 自定义字段2
        /// </summary>
        public string? field2 { get; set; }

        /// <summary>
        /// 自定义字段3
        /// </summary>
        public string? field3 { get; set; }

        /// <summary>
        /// 自定义字段4
        /// </summary>
        public string? field4 { get; set; }

        /// <summary>
        /// 自定义字段5
        /// </summary>
        public string? field5 { get; set; }

        /// <summary>
        /// 产线编号
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string? lineName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? orderCode { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public string? orderQty { get; set; }

        /// <summary>
        /// 工单状态编码，(5-新建, 30-已下达, 25-部分排程, 35-已排程, 45-完成, 55-完结, 65-取消)
        /// </summary>
        public string? orderStateCode { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public string? orderStatus { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public string? orderType { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string? productCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string? productName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string? productVersion { get; set; }

        /// <summary>
        /// 排程编码
        /// </summary>
        public string? scheduleCode { get; set; }

        /// <summary>
        /// 排程计划结束时间
        /// </summary>
        public string? scheduleEditTime { get; set; }

        /// <summary>
        /// 排程主键
        /// </summary>
        public string? scheduleId { get; set; }

        /// <summary>
        /// 排程数量，产线需执行的计划数量
        /// </summary>
        public string? scheduleQty { get; set; }

        /// <summary>
        /// 排程状态编码，(0-新建,1-下达,2-开线,3-生产,4-暂停,5-取消,6-完成,7-部分派工)
        /// </summary>
        public string? scheduleStateCode { get; set; }

        /// <summary>
        /// 排程状态
        /// </summary>
        public string? scheduleStatus { get; set; }

        /// <summary>
        /// 计划开始时间 (yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public string? startTime { get; set; }

        /// <summary>
        /// 车间编号
        /// </summary>
        public string? workshopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string? workshopName { get; set; }
    }

}
