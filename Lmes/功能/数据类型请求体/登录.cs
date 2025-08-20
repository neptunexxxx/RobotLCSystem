using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能.数据类型请求体
{
    public class 登录信息类
    {
        public string 用户名 { get; set; }
        public string 权限 { get; set; }
        public DateTime 登录时间 { get; set; }
        public string Token { get; set; }
    }
    public class 用户名密码类
    {
        public string 用户名 { get; set; }
        public string 密码 { get; set; }
    }
    public class 登录返回类
    {
        public int 登录状态 { get; set; }
        public string 用户名 { get; set; }
        public string 权限 { get; set; }
        public string token { get; set; }
    }
    public class 创建用户请求类
    {
        public int Id { get; set; }
        public string 用户名 { get; set; }
        public string? 密码 { get; set; }
        public string? 权限 { get; set; }
        public string? 备注 { get; set; }

    }
}
