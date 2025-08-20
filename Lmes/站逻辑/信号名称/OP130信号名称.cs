using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 总装夹持及归中总成、夹持及归中总成测试（公用PLC）
	/// </summary>
	public class OP130信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 夹紧组件ID绑定归中组件ID = "夹紧组件ID绑定归中组件ID";

		[信号参数属性(参数编码: "A130-1", 下限值: "7.8", 上限值: "8.8")]
		public readonly string _8M6X16螺钉拧紧力矩 = "8-M6*16螺钉拧紧力矩";

		[信号参数属性(参数编码: "A130-2", 下限值: "235.5", 上限值: "238.5")]
		public readonly string 归中关闭后机坪挡板间距数据_判定结果 = "归中关闭后机坪挡板间距数据_判定结果";

		[信号参数属性(参数编码: "A130-3", 下限值: "532.5", 上限值: "541.5")]
		public readonly string 归中展开后机坪挡板间距数据_判定结果 = "归中展开后机坪挡板间距数据_判定结果";

		[信号参数属性(参数编码: "A130-4", 下限值: "0", 上限值: "3")]
		public readonly string 电流值测试数据_判定结果 = "电流值测试数据_判定结果";

		[信号参数属性(参数编码: "A130-5", 下限值: "2.5", 上限值: "3.5")]
		public readonly string 运行时间测试数据_判定结果 = "运行时间测试数据_判定结果";

		[信号参数属性(参数编码: "A130-6")]
		public readonly string 防夹功能检测结果 = "防夹功能检测结果";

		[信号参数属性(参数编码: "A130-7", 下限值: "98", 上限值: "99")]
		public readonly string 归中后挡板与夹紧块导向销间距检测数据_判定结果 = "归中后挡板与夹紧块导向销间距检测数据_判定结果";

		[信号参数属性(参数编码: "A130-8", 下限值: "30.6", 上限值: "33.6")]
		public readonly string 上停机坪平面与夹紧块导向销间距检测数据_判定结果 = "上停机坪平面与夹紧块导向销间距检测数据_判定结果";

		[信号参数属性(参数编码: "A130-9", 下限值: "37.6", 上限值: "40.6")]
		public readonly string 下停机坪平面与夹紧块导向销间距检测数据_判定结果 = "下停机坪平面与夹紧块导向销间距检测数据_判定结果";

		#endregion
	}

}
