using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	//静音房检测、出站
	public class OP210信号名称 : 工站信号名称Base
	{
		#region 读取

		[信号参数属性(参数编码: "A210-1", 下限值: "0", 上限值: "55")]
		public readonly string 运行噪音检测数据_判定结果 = "运行噪音检测数据_判定结果";

		[信号参数属性(参数编码: "A210-2", 下限值: "0", 上限值: "70")]
		public readonly string 到位噪音检测数据_判定结果 = "到位噪音检测数据_判定结果";

		[信号参数属性(参数编码: "A210-3", 下限值: "41.5", 上限值: "47.5")]
		public readonly string 上停机坪初始高度检测数据_判定结果 = "上停机坪初始高度检测数据_判定结果";

		[信号参数属性(参数编码: "A210-4", 下限值: "34.5", 上限值: "40.5")]
		public readonly string 下停机坪初始高度检测数据_判定结果 = "下停机坪初始高度检测数据_判定结果";

		[信号参数属性(参数编码: "A210-5", 下限值: "156", 上限值: "162")]
		public readonly string 举升行程检测数据_判定结果 = "举升行程检测数据_判定结果";

		[信号参数属性(参数编码: "A210-6", 下限值: "0", 上限值: "3.5")]
		public readonly string 电流值测试数据_判定结果 = "电流值测试数据_判定结果";

		[信号参数属性(参数编码: "A210-7", 下限值: "4", 上限值: "5")]
		public readonly string 运行时间测试数据_判定结果 = "运行时间测试数据_判定结果";

		[信号参数属性(参数编码: "A210-8")]
		public readonly string _8平垫片有无检测判定结果 = "8-平垫片有无检测判定结果";

		[信号参数属性(参数编码: "A210-9")]
		public readonly string _8平垫片有无检测判定照片 = "8-平垫片有无检测判定照片";

		[信号参数属性(参数编码: "A210-10")]
		public readonly string _8鞍形弹性垫圈有无检测判定结果 = "8-鞍形弹性垫圈有无检测判定结果";

		[信号参数属性(参数编码: "A210-11")]
		public readonly string _8鞍形弹性垫圈有无检测判定照片 = "8-鞍形弹性垫圈有无检测判定照片";

		[信号参数属性(参数编码: "A210-12")]
		public readonly string _8开口挡圈有无检测判定结果 = "8-开口挡圈有无检测判定结果";

		[信号参数属性(参数编码: "A210-13")]
		public readonly string _8开口挡圈有无检测判定照片 = "8-开口挡圈有无检测判定照片";

		public readonly string 产品ID = "产品ID";

		#endregion
	}

}
