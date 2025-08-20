using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 总装夹紧机构总成
	/// </summary>
	public class OP050信号名称 : 工站信号名称Base
	{
		#region 读取

		[信号参数属性(参数编码: "A50-1")]
		public readonly string 机舱夹紧密封橡胶条有无和安装方向判定结果 = "机舱夹紧密封橡胶条有无和安装方向判定结果";

		[信号参数属性(参数编码: "A50-2")]
		public readonly string 机舱夹紧密封橡胶条有无和安装方向判定照片 = "机舱夹紧密封橡胶条有无和安装方向判定照片";

		[信号参数属性(参数编码: "A50-3")]
		public readonly string 机舱夹紧缓冲垫有无判定结果 = "机舱夹紧缓冲垫有无判定结果";

		[信号参数属性(参数编码: "A50-4")]
		public readonly string 机舱夹紧缓冲垫有无判定照片 = "机舱夹紧缓冲垫有无判定照片";

		[信号参数属性(参数编码: "A50-5")]
		public readonly string 机舱夹紧橡胶套有无判定结果 = "机舱夹紧橡胶套有无判定结果";

		[信号参数属性(参数编码: "A50-6")]
		public readonly string 机舱夹紧橡胶套有无判定照片 = "机舱夹紧橡胶套有无判定照片";

		[信号参数属性(参数编码: "A50-7", 下限值: "2", 上限值: "3")]
		public readonly string _6M4X12螺钉拧紧力矩 = "6-M4*12螺钉拧紧力矩";

		[信号参数属性(参数编码: "A50-8", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _4M5X16螺钉拧紧力矩 = "4-M5*16螺钉拧紧力矩";

		#endregion

		#region 写入
		#endregion
	}

}
