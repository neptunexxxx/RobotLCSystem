using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 工艺文件路径信息接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Base007_RouteFiles
        /// </summary>
        public string serviceId { get; set; } = "Base007_RouteFiles";

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
        /// 请求时间，必填
        /// </summary>
        public string requestTime { get; set; }

        /// <summary>
        /// 数据更新时间，必填
        /// </summary>
        public string updateTime { get; set; }

        /// <summary>
        /// 工单编号，非必须
        /// </summary>
        public string? orderCode { get; set; }
    }
    public class 工艺文件路径信息接口返回体
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
        public 工艺文件路径信息接口返回体object[]? data { get; set; }

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

    public class 工艺文件路径信息接口返回体object
    {
        /// <summary>
        /// 创建时间(yyyy-MM-dd HH:mm:ss)，必填
        /// </summary>
        public string? addTime { get; set; }

        /// <summary>
        /// 更新时间(yyyy-MM-dd HH:mm:ss)，必填
        /// </summary>
        public string? editTime { get; set; }

        /// <summary>
        /// 文件路径，必填
        /// </summary>
        public string? fileLoad { get; set; }

        /// <summary>
        /// 文件名称，必填
        /// </summary>
        public string? fileName { get; set; }

        /// <summary>
        /// 文件来源：XX系统，手工创建，必填
        /// </summary>
        public string? fileSource { get; set; }

        /// <summary>
        /// 文件类型，必填
        /// </summary>
        public string? fileType { get; set; }

        /// <summary>
        /// 工序顺序，必填
        /// </summary>
        public string? itemSort { get; set; }

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
        /// 工序编号，必填
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 工单编号，非必须
        /// </summary>
        public string? orderCode { get; set; }

        /// <summary>
        /// 工艺编号，必填
        /// </summary>
        public string? routeCode { get; set; }

        /// <summary>
        /// 工艺名称，必填
        /// </summary>
        public string? routeName { get; set; }

        /// <summary>
        /// 工艺版本，必填
        /// </summary>
        public string? routeVersion { get; set; }
    }
}
