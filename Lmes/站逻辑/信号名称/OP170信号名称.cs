using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装举升底座
	/// </summary>
	public class OP170信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 举升组件ID = "举升组件ID";

		public readonly string 机舱举升底座ID = "机舱举升底座ID";

		public readonly string 举升电机ID = "举升电机ID";

		public readonly string 举升推动块ID = "举升推动块ID";

		public readonly string 举升滑轨组件ID = "举升滑轨组件ID";

		[信号参数属性(参数编码: "A170-1")]
		public readonly string 举升滑块组件有无判定照片_判定结果 = "举升滑块组件有无判定照片_判定结果";

		[信号参数属性(参数编码: "A170-2", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _4M5X16螺钉拧紧力矩 = "4-M5*16螺钉拧紧力矩";

		[信号参数属性(参数编码: "A170-3", 下限值: "9.9", 上限值: "10.9")]
		public readonly string _2M6X16螺钉柠紧力矩 = "2-M6*16螺钉柠紧力矩";

		#endregion
	}

}
