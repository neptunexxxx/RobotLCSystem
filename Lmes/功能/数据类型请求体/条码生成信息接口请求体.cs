using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 条码生成信息接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Product001_SNCreate	
        /// </summary>
        public string? serviceId { get; set; }
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
        /// 条码申请数量
        /// </summary>
        public string? snQty { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }
    }

    public class 条码生成信息接口返回体
    {
        /// <summary>
        /// 000000代表请求成功，否则都代表请求失败
        /// </summary>
        public string? code { get; set; }
        /// <summary>
        /// 返回数据数量
        /// </summary>
        public int? count { get; set; }
        /// <summary>
        /// 返回SN集合
        /// </summary>
        public 条码生成信息接口返回体Data? data { get; set; }
        /// <summary>
        /// True/false
        /// </summary>
        public bool? fail { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string? mesg { get; set; }
        /// <summary>
        /// True/false
        /// </summary>
        public bool? success { get; set; }
        /// <summary>
        /// 返回接收成功时间
        /// </summary>
        public string? time { get; set; }
    }

    public class 条码生成信息接口返回体Data
    {
        /// <summary>
        /// 排程编号
        /// </summary>
        public string scheduleCode { get; set; }
        /// <summary>
        /// sn集合
        /// </summary>
        public List<string>? sn { get; set; }
	}
}
