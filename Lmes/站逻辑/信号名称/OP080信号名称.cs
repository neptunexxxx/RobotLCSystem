using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
	/// <summary>
	/// 组装归中皮带组件
	/// </summary>
	public class OP080信号名称 : 工站信号名称Base
	{
		#region 读取

		public readonly string 归中组件ID= "归中组件ID";

		//public readonly string 机舱归中皮带ID = "机舱归中皮带ID";

		public readonly string 机舱伸缩停机坪上皮带夹ID = "机舱伸缩停机坪上皮带夹ID";

		public readonly string 机舱伸缩停机坪下皮带夹ID = "机舱伸缩停机坪下皮带夹ID";

		//public readonly string 机舱归中皮带夹扣ID = "机舱归中皮带夹扣ID";

		[信号参数属性(参数编码: "A80-1", 下限值: "7.8", 上限值: "8.8")]
		public readonly string _4M5X10螺钉拧紧力矩 = "4-M5*10螺钉拧紧力矩（皮带卡扣锁付）";

		#endregion

		#region 写入
		#endregion
	}

}
