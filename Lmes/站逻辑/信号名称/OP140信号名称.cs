using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装举升连杆组件
	/// </summary>
	public class OP140信号名称 : 工站信号名称Base
	{
		#region 读取

		[信号参数属性(参数编码: "A140-1", 下限值: "10.5", 上限值: "11")]
		public readonly string _2平头铆钉旋铆铆头直径 = "2-平头铆钉旋铆铆头直径";

		[信号参数属性(参数编码: "A140-2", 下限值: "10.5", 上限值: "11")]
		public readonly string _4机舱举升叉臂旋转轴旋铆铆头直径 = "4-机舱举升叉臂旋转轴旋铆铆头直径";

		[信号参数属性(参数编码: "A140-3", 下限值: "0.3", 上限值: "2.1")]
		public readonly string 举升前连杆组件旋转扭矩 = "举升前连杆组件旋转扭矩";

		[信号参数属性(参数编码: "A140-4", 下限值: "0.3", 上限值: "2.1")]
		public readonly string 举升后连杆组件旋转扭矩 = "举升后连杆组件旋转扭矩";

		#endregion
	}

}
