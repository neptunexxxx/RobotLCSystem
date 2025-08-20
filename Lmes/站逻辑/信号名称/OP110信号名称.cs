using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装上下机坪组件
	/// </summary>
	public class OP110信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 机舱伸缩停机坪上ID = "机舱伸缩停机坪上ID";

		public readonly string 机舱伸缩停机坪下ID = "机舱伸缩停机坪下ID";

		[信号参数属性(参数编码: "A110-1")]
		public readonly string 机舱伸缩停机坪弹桥有无判定结果 = "机舱伸缩停机坪弹桥有无判定结果";

		[信号参数属性(参数编码: "A110-2")]
		public readonly string 机舱伸缩停机坪弹桥有无判定照片 = "机舱伸缩停机坪弹桥有无判定照片";



		#endregion
	}

}
