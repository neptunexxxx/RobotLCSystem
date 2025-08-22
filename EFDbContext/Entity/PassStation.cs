using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("pass_station")]
    public class PassStation
    {
        /// <summary>
        /// 生产过站信息ID
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 产品SN号，为产品的唯一标识
        /// </summary>
        public string? snNumber { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 该产品对应的工单编码，如果产线MES不接收排程则无需填写，如接收排程则需要填写接收到的排程中的工单编码。
        /// </summary>
        public string? workOrderNumber { get; set; }

        /// <summary>
        /// 该产品对应的排程编码，如果产线MES不接收排程则无需填写，如接收排程则需要填写接收到的排程中的排程编码。
        /// </summary>
        public string? scheduleNumber { get; set; }

        /// <summary>
        /// 托盘号（未使用，默认为空）
        /// </summary>
        public string? trayNumber { get; set; }

        /// <summary>
        /// 类型(进站:1/出站:2)，固定填写2
        /// </summary>
        public string? reqType { get; set; }

        /// <summary>
        /// 是否首工位进站(是:1/否:2)，首工位进站为：1，否则都为2
        /// </summary>
        public string? startStationCode { get; set; }

        /// <summary>
        /// 工位编号
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 1.托盘号绑定 SN 2.托盘号解绑 SN 3.不处理
        /// </summary>
        public int? snBindState { get; set; }

        /// <summary>
        /// 打印条码（总成码/镭雕码）
        /// </summary>
        public string? barCode { get; set; }

        /// <summary>
        /// 状态（pass出站填写），固定值pass	
        /// </summary>
        public string? state { get; set; }

        /// <summary>
        /// 用户名称，为用户工号
        /// </summary>
        public string? userId { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string? requestTime { get; set; }

        /// <summary>
        /// 入站时间
        /// </summary>
        public string? passBeginTime { get; set; }

        /// <summary>
        /// 出站时间
        /// </summary>
        public string? passEndTime { get; set; }

        /// <summary>
        /// 过站数量
        /// </summary>
        public string? passNum { get; set; }
    }
}
