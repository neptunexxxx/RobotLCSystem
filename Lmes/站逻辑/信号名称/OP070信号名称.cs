using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 充电夹持组件测试（与OP060共用PLC）
	/// </summary>
	public class OP070信号名称 : 工站信号名称Base
	{
		#region 读取

		[信号参数属性(参数编码: "A70-1", 下限值: "10.48", 上限值: "10.58")]
		public readonly string 弹片外观尺寸10_53 = "弹片外观尺寸10.53";

		[信号参数属性(参数编码: "A70-2", 下限值: "6.95", 上限值: "7.05")]
		public readonly string 弹片外观尺寸7_1 = "弹片外观尺寸7_1";

		[信号参数属性(参数编码: "A70-3", 下限值: "6.95", 上限值: "7.05")]
		public readonly string 弹片外观尺寸7_2 = "弹片外观尺寸7_2";

		[信号参数属性(参数编码: "A70-4")]
		public readonly string 导通测试结果 = "导通测试结果";

		[信号参数属性(参数编码: "A70-5", 下限值: "51.7", 上限值: "54.6")]
		public readonly string 夹紧状态夹紧块距离测试数据_判定结果1 = "夹紧状态夹紧块距离测试数据_判定结果1";

		[信号参数属性(参数编码: "A70-6", 下限值: "263.5", 上限值: "272.5")]
		public readonly string 夹紧状态夹紧块距离测试数据_判定结果2 = "夹紧状态夹紧块距离测试数据_判定结果2";

		[信号参数属性(参数编码: "A70-7", 下限值: "0", 上限值: "3")]
		public readonly string 电流值测试数据_判定结果 = "电流值测试数据_判定结果";

		[信号参数属性(参数编码: "A70-8", 下限值: "32", 上限值: "50")]
		public readonly string 左侧夹紧力测试数据_判定结果 = "左侧夹紧力测试数据_判定结果";

		[信号参数属性(参数编码: "A70-9", 下限值: "32", 上限值: "50")]
		public readonly string 右侧夹紧力测试数据_判定结果 = "右侧夹紧力测试数据_判定结果";

		[信号参数属性(参数编码: "A70-10", 下限值: "5", 上限值: "30")]
		public readonly string _4单个夹紧伸缩块夹紧力 = "_4单个夹紧伸缩块夹紧力";

		[信号参数属性(参数编码: "A70-11", 下限值: "5.5", 上限值: "6.5")]
		public readonly string 运行时间测试数据_判定结果 = "运行时间测试数据_判定结果";

		[信号参数属性(参数编码: "A70-12", 下限值: "11.5", 上限值: "12.5")]
		public readonly string 导向销与夹紧块间距检测数据_判定结果 = "导向销与夹紧块间距检测数据_判定结果";

		[信号参数属性(参数编码: "A70-13", 下限值: "51.8", 上限值: "54.2")]
		public readonly string 左右夹紧块两橡胶夹紧面间距检测数据_判定结果 = "左右夹紧块两橡胶夹紧面间距检测数据_判定结果";

		[信号参数属性(参数编码: "A70-14")]
		public readonly string 防夹功能检测结果 = "防夹功能检测结果";

		#endregion

		#region 写入
		#endregion
	}

}
