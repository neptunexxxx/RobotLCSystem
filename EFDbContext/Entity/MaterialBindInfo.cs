using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
	[Table("material_bind_info")]
	public class MaterialBindInfo
	{
		[Key]
		public string 物料名称 { get; set; }
		public string? 正则表达式 { get; set; }
		public bool? 是否使用 { get; set; }
	}
}
