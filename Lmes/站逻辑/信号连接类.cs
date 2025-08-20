
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.数据类型
{
	[AddINotifyPropertyChangedInterface]
	public partial class 信号连接类
	{
		public string 信号名称 { get; set; }
		public string? 参数编码 { get; set; }
		public string? 物料编码 { get; set; }
		public S7.Net.Types.DataItem 信号地址 { get; set; } = new();
	}

}
