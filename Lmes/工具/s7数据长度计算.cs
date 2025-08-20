using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.工具
{
    internal static class s7数据长度计算
    {
        public static int 获取长度(this VarType varType)
        {
            return varType switch
            {
                VarType.Bit => 1,
                VarType.Byte => 1,
                VarType.Word => 2,
                VarType.DWord => 4,
                VarType.Int => 2,
                VarType.DInt => 4,
                VarType.Real => 4,
                VarType.LReal => 8,
                VarType.String => 1,
                VarType.S7String => 1,
                VarType.S7WString => 1,
                VarType.Timer => 8,
                VarType.Counter => 8,
                VarType.DateTime => 12,
                VarType.DateTimeLong => 12,
                _ => 12,
            };
        }
    }
}
