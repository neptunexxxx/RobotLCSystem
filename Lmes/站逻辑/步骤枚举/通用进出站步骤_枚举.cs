using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.站逻辑.步骤枚举
{
    public enum 通用进出站步骤_枚举
    {
        等待触发,

        等待产品进站,

        检验设备状态,

        参数上传,

        等待产品出站,

        出站,

        等待PLC信号复位,
		物料绑定
	}
}
