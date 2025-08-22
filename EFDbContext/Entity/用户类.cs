using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    public class 用户类
    {
        [Key]
        public int Id { get; set; }
        public string 用户名 { get; set; }
        public string 密码 { get; set; }
        public string? 权限 { get; set; }
        public string? 备注 { get; set; }
    }
}
