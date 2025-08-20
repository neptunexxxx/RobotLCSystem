using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装夹紧块总成
	/// </summary>
	public class OP010信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 机舱夹紧块ID = "2-机舱夹紧块ID（左右）";

		//public readonly string 左充电拖链线束总成ID = "左充电拖链线束总成ID";

		//public readonly string 右充电拖链线束总成ID = "右充电拖链线束总成ID";

		[信号参数属性(参数编码: "A10-1", 下限值: "2", 上限值: "3")]
		public readonly string _2M5X10线束固定螺钉 = "2-M5*10螺钉拧紧力矩（线束固定螺钉）";

		[信号参数属性(参数编码: "A10-2", 下限值: "4.6", 上限值: "5.6")]
		public readonly string _2M5X10夹紧块定螺钉 = "2-M5*10螺钉拧紧力矩（夹紧块定螺钉）";

		[信号参数属性(参数编码: "A10-3", 下限值: "0.9", 上限值: "1.1")]
		public readonly string _4M3X10螺钉拧紧力矩 = "4-M3*10螺钉拧紧力矩";
		#endregion

		#region 写入
		#endregion
	}
}
