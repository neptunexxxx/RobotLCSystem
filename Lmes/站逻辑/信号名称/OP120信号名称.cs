using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 总装上下机坪组件装配
	/// </summary>
	public class OP120信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 机舱伸缩停机坪上ID = "机舱伸缩停机坪上ID";

		public readonly string 机舱伸缩停机坪下ID = "机舱伸缩停机坪下ID";

		[信号参数属性(参数编码: "A120-1", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _10MX510螺钉拧紧力矩 = "10-M5*10螺钉拧紧力矩";

		[信号参数属性(参数编码: "A120-2")]
		public readonly string 机舱举升限位保护套有无判定结果 = "机舱举升限位保护套有无判定结果";

		[信号参数属性(参数编码: "A120-3")]
		public readonly string 机舱举升限位保护套有无判定照片 = "机舱举升限位保护套有无判定照片";

		#endregion
	}

}
