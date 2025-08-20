using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lmes.站逻辑.数据类型;
using PropertyChanged;

namespace Lmes.Models
{
	public class 工位信息
	{
		public string? 工位编号 { get; set; }
		public string? 工位类型 { get; set; }
		public int 工站工位数 { get; set; } = 1;
		public string? 工位号 { get; set; }
		public string? 设备编码 { get; set; }
		public string? IOT上传Token { get; set; }
		public string? 工号 { get; set; }
		public PLC连接设置? PLC连接设置 { get; set; }
		public List<信号连接>? 信号连接表 { get; set; }
		public bool? 是否需要上传参数 { get; set; }
		public bool? 是否需要绑定物料 { get; set; }
	}

	[AddINotifyPropertyChangedInterface]
	public partial class 信号连接
	{
		public string? 信号名称 { get; set; }
		public string? 物料编码 { get; set; }
		public S7.Net.Types.DataItem 信号地址 { get; set; } = new();
		#region 工艺参数
		public string? 参数编码 { get; set; }
		public string? 下限值 { get; set; }
		public string? 上限值 { get; set; }
		public string? 标准值 { get; set; }
		#endregion
	}

	[AddINotifyPropertyChangedInterface]
	public partial class PLC连接设置
	{
		public string? CPU类型 { get; set; }
		public string? IP地址 { get; set; }
		public short 端口 { get; set; }
		public short 机架 { get; set; }
		public short 槽位 { get; set; }
		public short 采集间隔 { get; set; }
	}
}
