using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 总装举升归中夹紧机构总成
	/// </summary>
	public class OP190信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 无人机举升归中夹紧机构总成 = "无人机举升归中夹紧机构总成";

		public readonly string 前外叉臂ID = "前外叉臂ID";

		public readonly string 前内叉臂ID = "前内叉臂ID";

		public readonly string 后外叉臂ID = "后外叉臂ID";

		public readonly string 后内叉臂ID = "后内叉臂ID";
		  
		public readonly string 机舱举升驱动电机防水护壳上壳体ID = "机舱举升驱动电机防水护壳上壳体ID";

		[信号参数属性(参数编码: "A190-1", 下限值: "19.8", 上限值: "20.8")]
		public readonly string _4M8X16螺钉拧紧力矩 = "4-M8*16螺钉拧紧力矩";

		[信号参数属性(参数编码: "A190-2", 下限值: "0.9", 上限值: "1.1")]
		public readonly string _6塑料用内六角花形盘头自攻螺钉拧紧扭矩 = "6-塑料用内六角花形盘头自攻螺钉拧紧扭矩（固定举升电机壳体）";

		#endregion
	}

}
