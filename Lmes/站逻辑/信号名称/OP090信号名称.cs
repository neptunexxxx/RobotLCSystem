using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 总装归中驱动总成
	/// </summary>
	public class OP090信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 机舱停机坪底板组件ID = "机舱停机坪底板组件ID";

		public readonly string 机舱归中驱动电机ID = "机舱归中驱动电机ID";

		public readonly string _2机坪底板上滑轨组件ID = "2-机坪底板滑轨组件ID";

		[信号参数属性(参数编码: "A90-1", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _3M5X16自攻螺丝拧紧力矩 = "3-M5*16自攻螺丝拧紧力矩（归中电机固定螺钉）";

		[信号参数属性(参数编码: "A90-2", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _4M5X16螺钉拧紧力矩 = "4-M5*16螺钉拧紧力矩（滑轨组件固定螺钉）";

		[信号参数属性(参数编码: "A90-3", 下限值: "150", 上限值: "180")]
		public readonly string 皮带张紧力 = "皮带张紧力";

		[信号参数属性(参数编码: "A90-4", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _3M5X16螺钉拧紧力矩_辅助轮组件 = "3-M5*16螺钉拧紧力矩（辅助轮组件固定螺钉）";

		[信号参数属性(参数编码: "A90-5", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _3M5X16螺钉拧紧力矩_惰轮组件= "3-M5*16螺钉拧紧力矩（情轮组件固定螺钉）";

		#endregion
	}

}
