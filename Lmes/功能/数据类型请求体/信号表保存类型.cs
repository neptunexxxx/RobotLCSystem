using Lmes.Models;
using Lmes.功能.枚举;
using Lmes.站逻辑.数据类型;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 信号表保存类型
    {
        public string 名称 { get; set; }
        public string 版本号 { get; set; }
        public 工站_枚举 站点类别 { get; set; }
        public List<信号连接> 信号表 { get; set; }
    }
}
