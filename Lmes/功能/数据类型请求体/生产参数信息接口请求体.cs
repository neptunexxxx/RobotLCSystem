using System;
using System.Collections.Generic;

namespace Lmes.功能.数据类型请求体
{
	/// <summary>
	/// 生产参数信息接口请求体
	/// </summary>
	public class 生产参数信息接口请求体
	{
		/// <summary>
		/// Service服务ID，固定值为Product006_ProductParameters
		/// </summary>
		public string? serviceId { get; set; } = "Product006_ProductParameters";

		/// <summary>
		/// 工厂编号
		/// </summary>
		public string? factoryCode { get; set; }

		/// <summary>
		/// 请求时间，为发起请求的当前时间
		/// </summary>
		public string? requestTime { get; set; }

		/// <summary>
		/// 请求参数集合
		/// </summary>
		public 生产参数信息请求体Data? data { get; set; } = new();
	}

	/// <summary>
	/// 生产参数信息请求参数
	/// </summary>
	public class 生产参数信息请求体Data
	{
		/// <summary>
		/// 产线编码
		/// </summary>
		public string? lineCode { get; set; }

		/// <summary>
		/// 设备编码
		/// </summary>
		public string? machineCode { get; set; }

		/// <summary>
		/// 产品SN号
		/// </summary>
		public string? snNumber { get; set; }

		/// <summary>
		/// 工位名称
		/// </summary>
		public string? stationCode { get; set; }

		/// <summary>
		/// 产品物料编码
		/// </summary>
		public string? materialCode { get; set; }

		/// <summary>
		/// 产品物料名称
		/// </summary>
		public string? materialName { get; set; }

		/// <summary>
		/// 产品物料版本
		/// </summary>
		public string? materialVersion { get; set; }

		/// <summary>
		/// 生产参数记录时间
		/// </summary>
		public string? paramTime { get; set; }

		/// <summary>
		/// 参数集合
		/// </summary>
		public List<生产参数>? dataList { get; set; } = new List<生产参数> { };
	}

	/// <summary>
	/// 生产参数
	/// </summary>
	public class 生产参数
	{
		/// <summary>
		/// 参数编码
		/// </summary>
		public string? paramCode { get; set; }

		/// <summary>
		/// 参数描述
		/// </summary>
		public string? paramName { get; set; }

		/// <summary>
		/// 标准值
		/// </summary>
		public string? standardValue { get; set; }

		/// <summary>
		/// 实际值
		/// </summary>
		public string? realValue { get; set; }

		/// <summary>
		/// 参数下限
		/// </summary>
		public string? paramRange1 { get; set; }

		/// <summary>
		/// 参数上限
		/// </summary>
		public string? paramRange2 { get; set; }

		/// <summary>
		/// 判定结果（0，不合格；1，合格）
		/// </summary>
		public string? checkResult { get; set; }
	}

	public class 生产参数信息接口返回体
	{
		/// <summary>
		/// 状态编码，000000代表返回成功	
		/// </summary>
		public string? code { get; set; }

		/// <summary>
		/// 返回数据数量	
		/// </summary>
		public int? count { get; set; }

		/// <summary>
		/// 返回数据	
		/// </summary>
		public string? data { get; set; }

		/// <summary>
		/// 是否失败	
		/// </summary>
		public bool? fail { get; set; }

		/// <summary>
		/// 接口返回信息	
		/// </summary>
		public string? mesg { get; set; }

		/// <summary>
		/// 是否成功	
		/// </summary>
		public bool success { get; set; }

		/// <summary>
		/// 接口调用时间
		/// </summary>
		public string? time { get; set; }
	}
}