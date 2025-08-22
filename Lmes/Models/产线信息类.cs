using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Lmes.全局变量;

namespace Lmes.Models
{
    public class 产线信息类
    {
        public string 工厂编号 { get; set; }
        public string 产线编号 { get; set; }
        public List<string> 运行工站列表 { get; set; } = new();
        public 工艺路径 工艺路径 { get; set; } = new();
        public string? 排程编码 { get; set; }
        public List<string> 排程列表 = new();
        public string? 产品名称 { get; set; }
        public string? 产品编号 { get; set; }

        public string? 工单编码 { get; set; }

		public string? 产品计划数量 { get; set; }
	}
}
