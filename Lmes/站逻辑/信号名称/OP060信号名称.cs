using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装夹紧罩盖、组装线束拖链
	/// </summary>
	public class OP060信号名称 : 工站信号名称Base
	{
		#region 读取

		[信号参数属性(参数编码: "A60-1", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _4M5X12螺钉拧紧力矩 = "4-M5*12螺钉拧紧力矩";

		[信号参数属性(参数编码: "A60-2", 下限值: "0.9", 上限值: "1.1")]
		public readonly string _4M3X10螺钉拧紧力矩 = "4-M3*10螺钉柠紧力矩";

		#endregion

		#region 写入
		#endregion
	}

}
