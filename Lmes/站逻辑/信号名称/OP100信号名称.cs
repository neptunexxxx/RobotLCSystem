using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装归中内滑轨组件
	/// </summary>
	public class OP100信号名称 : 工站信号名称Base
	{
		#region 读取

		[信号参数属性(参数编码: "A100-1", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _12M5X10螺钉拧紧力矩 = "12-M5*10螺钉拧紧力矩";

		#endregion
	}

}
