using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 总装举升底座组件
	/// </summary>
	public class OP180信号名称 : 工站信号名称Base
	{
		#region 读取

		[信号参数属性(参数编码: "A180-1")]
		public readonly string _2弹簧有无判定照片_判定结果 = "2-弹簧有无判定照片&判定结果";

		[信号参数属性(参数编码: "A180-2")]
		public readonly string _6机舱举升限位保护套有无检测判定照片 = "6-机舱举升限位保护套有无检测判定照片";

		[信号参数属性(参数编码: "A180-3", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _8M5X16螺钉拧紧力矩 = "8-M5*16螺钉拧紧力矩";

		#endregion
	}

}
