using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 人员资质接口请求体
    {
        /// <summary>
        /// Service服务ID，默认为Base010_Personnelqualification
        /// </summary>
        public string serviceId { get; set; } = "Base010_Personnelqualification";

        /// <summary>
        /// 工厂编号，必填
        /// </summary>
        public string factoryCode { get; set; }

        /// <summary>
        /// 产线编号，非必须
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 员工工号，非必须
        /// </summary>
        public string? empCode { get; set; }

        /// <summary>
        /// 工序编号，非必须
        /// </summary>
        public string? operationCode { get; set; }

        /// <summary>
        /// 请求时间，必填
        /// </summary>
        public string requestTime { get; set; }

        /// <summary>
        /// 人员信息更新时间，必填
        /// </summary>
        public string updateTime { get; set; }
    }

    public class 人员资质接口返回体
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
        /// 人员信息集合，必填
        /// </summary>
        public 人员资质接口返回体object[]? data { get; set; }

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

    public class 人员资质接口返回体object
    {
        /// <summary>
        /// 添加时间，必填
        /// </summary>
        public string? addTime { get; set; }

        /// <summary>
        /// 证书编号，非必须
        /// </summary>
        public string? certificateNo { get; set; }

        /// <summary>
        /// 控制状态编码，非必须
        /// </summary>
        public string? controlStatusCode { get; set; }

        /// <summary>
        /// 控制状态id（数据字典明细id），非必须
        /// </summary>
        public string? controlStatusId { get; set; }

        /// <summary>
        /// 数据状态，0为正常，1为删除，必填
        /// </summary>
        public string? dataStatus { get; set; }

        /// <summary>
        /// 更新时间，必填（格式为yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public string? editTime { get; set; }

        /// <summary>
        /// 员工工号，必填
        /// </summary>
        public string? empCode { get; set; }

        /// <summary>
        /// 员工ID，必填
        /// </summary>
        public string? empId { get; set; }

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
        /// 技能截止日期，非必须
        /// </summary>
        public string? skillEndDate { get; set; }

        /// <summary>
        /// 技能id，必填
        /// </summary>
        public string? skillId { get; set; }

        /// <summary>
        /// 技能等级编码（数据字典明细编码），必填
        /// </summary>
        public string? skillLevelCode { get; set; }

        /// <summary>
        /// 技能等级id（数据字典明细id），必填
        /// </summary>
        public string? skillLevelId { get; set; }

        /// <summary>
        /// 技能开始日期，必填
        /// </summary>
        public string? skillStartDate { get; set; }

        /// <summary>
        /// 技能有效期（月），必填
        /// </summary>
        public string? skillValidity { get; set; }
    }
}
