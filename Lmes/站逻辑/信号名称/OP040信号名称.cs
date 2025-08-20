using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装夹紧导轨组件
	/// </summary>
	public class OP040信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 机舱上固定板ID = "机舱上固定板ID";

		public readonly string 机舱充电夹紧滑轨ID = "机舱充电夹紧滑轨ID";

		public readonly string 机舱夹紧块ID = "2-机舱夹紧块ID（左右）";

		public readonly string 机舱夹紧驱动器ID = "机舱夹紧驱动器ID";

		public readonly string 机舱夹紧驱动电机组件 = "机舱夹紧驱动电机组件";

		public readonly string 机舱夹紧总成 = "机舱夹紧总成";

		[信号参数属性(参数编码: "A40-1")]
		public readonly string 弹桥有无判定结果 = "弹桥有无判定结果";

		[信号参数属性(参数编码: "A40-2")]
		public readonly string 弹桥有无判定照片 = "弹桥有无判定照片";

		[信号参数属性(参数编码: "A40-3")]
		public readonly string 弹桥是否涂油脂 = "弹桥是否涂油脂";

		[信号参数属性(参数编码: "A40-4")]
		public readonly string 丝杆是否涂油脂 = "丝杆是否涂油脂";

		[信号参数属性(参数编码: "A40-5", 下限值: "1", 上限值: "1.5")]
		public readonly string 夹紧电机固定M5X16螺钉拧紧力矩 = "夹紧电机固定M5*16螺钉拧紧力矩";

		[信号参数属性(参数编码: "A40-6")]
		public readonly string 夹紧电机固定M5X16螺钉数量 = "夹紧电机固定M5*16螺钉数量";

		#endregion

		#region 写入
		#endregion
	}

}
