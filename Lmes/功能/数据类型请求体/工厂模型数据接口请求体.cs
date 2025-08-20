using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 工厂模型数据接口请求体
    {
        /// <summary>
        /// 服务ID，必填：Base001_Model
        /// </summary>
        public string? serviceId { get; set; } = "Base001_Model";

        /// <summary>
        /// 工厂代码，必填
        /// </summary>
        public string? factoryCode { get; set; }

        /// <summary>
        /// 产线编码，非必须
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 请求时间，必填，填写请求的当前时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 增量更新时间，非必须，填写返回该时间之后更新过的数据，不填写返回所有数据
        /// </summary>
        public string? updateTime { get; set; }
    }


    public class 工厂模型数据接口返回体
    {
        /// <summary>
        /// 请求结果标识码，000000代表请求成功，否则为请求失败
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 返回的数据数量
        /// </summary>
        public int? count { get; set; }

        /// <summary>
        /// 工厂模型信息集合
        /// </summary>
        public 工厂模型数据接口返回体object[] data { get; set; }

        /// <summary>
        /// 请求是否失败，true/false
        /// </summary>
        public bool? fail { get; set; }

        /// <summary>
        /// 请求失败的原因
        /// </summary>
        public string? mesg { get; set; }

        /// <summary>
        /// 请求是否成功，true/false
        /// </summary>
        public bool? success { get; set; }

        /// <summary>
        /// 返回接收成功时间，13位时间戳
        /// </summary>
        public string? time { get; set; }
    }

    public class 工厂模型数据接口返回体object
    {
        /// <summary>
        /// 数据创建时间
        /// </summary>
        public string? addTime { get; set; }

        /// <summary>
        /// 数据删除状态：0-正常，1-删除
        /// </summary>
        public string? dataStatus { get; set; }

        /// <summary>
        /// 数据修改时间
        /// </summary>
        public string? editTime { get; set; }

        /// <summary>
        /// 启用状态：0-启用，1-禁用
        /// </summary>
        public string? enableStatus { get; set; }

        /// <summary>
        /// 模型编号
        /// </summary>
        public string? modelCode { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string? modelName { get; set; }

        /// <summary>
        /// 模型类型
        /// </summary>
        public string? modelTypeCode { get; set; }

        /// <summary>
        /// 父级模型编号
        /// </summary>
        public string? parentModelCode { get; set; }
    }
}
