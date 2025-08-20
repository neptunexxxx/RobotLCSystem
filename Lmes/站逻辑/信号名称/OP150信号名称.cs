using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装举升驱动电机
	/// </summary>
	public class OP150信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 举升电机ID = "举升电机ID";

		public readonly string 机舱举升驱动电机防水护壳下壳体ID = "机舱举升驱动电机防水护壳下壳体ID";

		[信号参数属性(参数编码: "A150-1", 下限值: "1", 上限值: "1")]
		public readonly string 机舱举升电机固定轴装配数量 = "机舱举升电机固定轴装配数量";

		[信号参数属性(参数编码: "A150-2", 下限值: "2", 上限值: "2")]
		public readonly string 开口挡圈装配数量 = "开口挡圈装配数量";

		#endregion
	}

}
