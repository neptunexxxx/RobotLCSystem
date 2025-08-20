using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装夹紧块滑柱
	/// </summary>
	public class OP020信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 机舱夹紧块ID = "2-机舱夹紧块ID（左右）";

		[信号参数属性(参数编码: "A20-1")]
		public readonly string 机舱夹紧块隔震垫有无判定结果 = "机舱夹紧块隔震垫有无判定结果";

		[信号参数属性(参数编码: "A20-2")]
		public readonly string 机舱夹紧块隔震垫有无判定照片 = "机舱夹紧块隔震垫有无判定照片";

		[信号参数属性(参数编码: "A20-3")]
		public readonly string 机舱夹紧块滑柱有无判定结果 = "机舱夹紧块滑柱有无判定结果";

		[信号参数属性(参数编码: "A20-4")]
		public readonly string 机舱夹紧块滑柱有无判定照片 = "机舱夹紧块滑柱有无判定照片";

		[信号参数属性(参数编码: "A20-5")]
		public readonly string 机舱夹紧块导槽衬套有无判定结果 = "机舱夹紧块导槽衬套有无判定结果";

		[信号参数属性(参数编码: "A20-6")]
		public readonly string 机舱夹紧块导槽衬套有无判定照片 = "机舱夹紧块导槽衬套有无判定照片";

		[信号参数属性(参数编码: "A20-7")]
		public readonly string 卡簧有无判定结果 = "卡簧有无判定结果";

		[信号参数属性(参数编码: "A20-8")]
		public readonly string 卡簧有无图片 = "卡簧有无图片";

		#endregion

		#region 写入
		#endregion
	}

}
