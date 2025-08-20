using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.信号名称
{
    public class 工站信号名称Base
    {

        #region 读取

        public readonly string 读取_心跳 = "读取_心跳";

        public readonly string 读取_PLC状态触发 = "读取_PLC状态触发";

        public readonly string 读取_设备状态 = "读取_设备状态";

        public readonly string 读取_事件代码 = "读取_事件代码";

        public readonly string 读取_托盘码 = "读取_托盘码";

        public readonly string 读取_SN = "读取_SN";

        #endregion

        #region 写入

        public readonly string 写入_心跳 = "写入_心跳";

        public readonly string 写入_MESReady = "写入_MESReady";

        public readonly string 写入_SN = "写入_SN";

        public readonly string 写入_MESCode = "写入_MESCode";

        public readonly string 写入_错误信息 = "写入_错误信息";

        #endregion
    }
}
