using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
	public class 工单路径数据接口请求体
	{
		/// <summary>
		/// Service服务ID，默认为Base005_Route
		/// </summary>
		public string? serviceId { get; set; } = "Base005_Route";

		/// <summary>
		/// 工厂编号
		/// </summary>
		public string? factoryCode { get; set; }

		/// <summary>
		/// 产线编号
		/// </summary>
		public string? lineCode { get; set; }

		/// <summary>
		///排程编码
		/// </summary>
		public string? scheduleNumber { get; set; }

		/// <summary>
		/// 工单编码	
		/// </summary>
		public string? orderCode { get; set; }

		/// <summary>
		/// 请求时间	
		/// </summary>
		public string? requestTime { get; set; }

		/// <summary>
		/// 数据更新时间
		/// </summary>
		public string? updateTime { get; set; }
	}

	public class 工单路径数据接口返回体
	{
		/// <summary>
		/// 000000代表请求成功，否则都代表请求失败	
		/// </summary>
		public string? code { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string? count { get; set; }

		/// <summary>
		/// 工单工艺路径信息集合
		/// </summary>
		public 工单路径数据返回体数据[]? data { get; set; }

		/// <summary>
		/// true/false
		/// </summary>
		public Boolean? fail { get; set; }

		/// <summary>
		/// true/false
		/// </summary>
		public string? mesg { get; set; }

		/// <summary>
		/// true/false
		/// </summary>
		public Boolean? success { get; set; }

		/// <summary>
		/// 返回接收成功时间
		/// </summary>
		public string? time { get; set; }
	}

	public class 工单路径数据返回体数据
	{
		/// <summary>
		/// 更新时间（yyyy-mm-dd hh24:mi:ss）
		/// </summary>
		public string? editTime { get; set; }

		/// <summary>
		/// 前一工序编码（数值为0则代表为该工序为首工序）
		/// </summary>
		public string? lastOperationCode { get; set; }
		/// <summary>
		///后一工序编码（为空，则代表该工序为末工序）
		/// </summary>
		public string? nextOperationCode { get; set; }

		/// <summary>
		/// 工序编码
		/// </summary>
		public string? operationCode { get; set; }
		/// <summary>
		/// 工序名称
		/// </summary>
		public string? operationName { get; set; }
		/// <summary>
		/// 工位编码
		/// </summary>
		public List<string>? stationCode { get; set; }
		/// <summary>
		/// 工单编号
		/// </summary>
		public string? orderCode { get; set; }

		/// <summary>
		/// 工艺路线编码
		/// </summary>
		public string? routeNumber { get; set; }
		/// <summary>
		/// 工艺路线版本
		/// </summary>
		public string? routeVersion { get; set; }
		/// <summary>
		/// 工序顺序
		/// </summary>
		public string? sort { get; set; }
	}

}
