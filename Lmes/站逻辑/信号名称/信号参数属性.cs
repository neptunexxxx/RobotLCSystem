using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	// 创建一个特性类来存储参数信息
	[AttributeUsage(AttributeTargets.Field)]
	public class 信号参数属性 : Attribute
	{
		public string 参数编码 { get; set; }
		public string 下限值 { get; set; }
		public string 上限值 { get; set; }
		public string 标准值 { get; set; }

		public 信号参数属性(string 参数编码 = "", string 下限值 = "", string 上限值 = "", string 标准值 = "")
		{
			this.参数编码 = 参数编码;
			this.下限值 = 下限值;
			this.上限值 = 上限值;
			this.标准值 = 标准值;
		}
	}
}
