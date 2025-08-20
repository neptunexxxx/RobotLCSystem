using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 人员登录信息接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为System002_Login
        /// </summary>
        public string serviceId { get; set; } = "System002_Login";

        /// <summary>
        /// 工厂编号，必填
        /// </summary>
        public string factoryCode { get; set; }

        /// <summary>
        /// 产线编号，必填
        /// </summary>
        public string lineCode { get; set; }

        /// <summary>
        /// 请求时间，必填
        /// </summary>
        public string requestTime { get; set; }

        /// <summary>
        /// 用户工号，必填
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 用户姓名，必填
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 工位编号，必填
        /// </summary>
        public string stationCode { get; set; }

        /// <summary>
        /// 登录状态，必填，1代表上班，0代表下班
        /// </summary>
        public string loginState { get; set; }

        /// <summary>
        /// 登录时间（上班或下班时间），必填
        /// </summary>
        public string loginTime { get; set; }
    }

    public class 人员登录信息接口返回体
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
        /// 返回的数据，非必须
        /// </summary>
        public string? data { get; set; }

        /// <summary>
        /// 请求是否失败，true/false，非必须
        /// </summary>
        public bool? fail { get; set; }

        /// <summary>
        /// 请求失败原因，非必须
        /// </summary>
        public string? mesg { get; set; }

        /// <summary>
        /// 请求是否成功，必填，true表示成功，false表示失败
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 返回接收成功时间，13位时间戳，非必须
        /// </summary>
        public string? time { get; set; }
    }
}
