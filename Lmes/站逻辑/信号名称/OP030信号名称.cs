using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装驱动器总成
	/// </summary>
	public class OP030信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 机舱夹紧驱动器ID = "机舱夹紧驱动器ID";

		[信号参数属性(参数编码: "A30-1")]
		public readonly string 机舱夹紧滑块隔振垫有无判定结果 = "机舱夹紧滑块隔振垫有无判定结果";

		[信号参数属性(参数编码: "A30-2")]
		public readonly string 机舱夹紧滑块隔振垫有无判定照片 = "机舱夹紧滑块隔振垫有无判定照片";

		[信号参数属性(参数编码: "A30-3")]
		public readonly string 机舱夹紧丝杠中间固定座有无判定结果 = "机舱夹紧丝杠中间固定座有无判定结果";

		[信号参数属性(参数编码: "A30-4")]
		public readonly string 机舱夹紧丝杠中间固定座有无照片 = "机舱夹紧丝杠中间固定座有无照片";

		#endregion

		#region 写入
		#endregion
	}

}
