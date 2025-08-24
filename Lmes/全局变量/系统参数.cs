using EFDbContext.Entity;
using Lmes.Models;
using Lmes.功能.枚举;
using Lmes.站逻辑.数据类型;
using S7.Net;
using System.ComponentModel;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;


namespace Lmes.全局变量
{

	public class 系统参数
	{
		public static string 配置文件路径
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lmes_1.0");
			}
		}


		public static JsonSerializerOptions 整理格式 { get; set; } = new JsonSerializerOptions()
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			WriteIndented = true,
		};

		public static 设置类 设置 { get; set; } = new();

		public static List<产线信息类> 产线信息 { get; set; }
		public static string ApiKey { get; set; }
		public static List<信号连接> 信号表 { get; set; } = new();
		public static event Action<string, 工位信息?> 信号表修改事件;
		public static event Action<设置类?> 配置文件修改事件;
		public static BindingList<工站对应物料信息> 物料信息表格 { get; set; } = new();
		public static BindingList<ExceptionBindingInformation> 异常绑定信息表格 { get; set; } = new();
		public static void 信号表修改(string 工站, 工位信息? temp)
		{
			信号表修改事件?.Invoke(工站, temp);
		}

		public static void 配置文件修改(设置类? temp)
		{
			配置文件修改事件?.Invoke(temp);
		}

		public static readonly List<string> 工站列表 = new List<string>
	{
		"OP010",
		"OP020",
		"OP030",
		"OP040",
		"OP050",
		"OP060",
		"OP070",
		"OP080",
		"OP090",
		"OP100",
		"OP110",
		"OP120",
		"OP130",
		"OP140",
		"OP150",
		"OP160",
		"OP170",
		"OP180",
		"OP190",
		"OP200",
		"OP210"
	};
	}

    public class 设置类
	{
		public int 日志最大数量 { get; set; } = 3000;
		public Lmes连接参数类 Lmes连接参数 { get; set; } = new();
		public Plc连接参数类 Plc连接参数 { get; set; } = new();
		public 工站_枚举 当前工站 { get; set; } = 工站_枚举.未选择;




		public string 工厂编号 { get; set; } = "";
		public string 产线编号 { get; set; } = "";
		public string 工位编号 { get; set; } = "";


		public bool 调试模式 { get; set; } = false;
		public bool 屏蔽进站校验 { get; set; } = false;
        public bool 屏蔽出站校验 { get; set; } = false;




		public List<班次信息类> 班次信息 { get; set; } =
		[new 班次信息类()
		{
			班次名称  ="早班",
			开始时间 = new TimeSpan(8, 30, 0)
		},
		new 班次信息类()
		{
			班次名称  ="晚班",
			开始时间 = new TimeSpan(20, 30, 0)
		}];
	}


	public class 班次信息类
	{
		public string 班次名称 { get; set; } = "未命名班次";
		public TimeSpan 开始时间 { get; set; }
	}

	public class Lmes连接参数类
	{

		public string 上位机地址 { get; set; } = @"http://127.0.0.1:5000/";

		public string 产线MES地址 { get; set; } = @"http://127.0.0.1:5000/";

		public string IOT地址 { get; set; } = @"http://127.0.0.1:5000/";

		public string appId { get; set; } = "1";

		public string appKey { get; set; } = "1";
		public string 机器人IP地址 { get; set; } = @"http://127.0.0.1:5000/";
		public string LMES地址 { get; set; } = @"http://127.0.0.1:5000/";
	}
	public class Plc连接参数类
	{
		public string CPU类型 { get; set; } = "";
		public string IP地址 { get; set; } = "192.168.0.1";
		public int 端口 { get; set; } = 102;
		public short 机架 { get; set; } = 0;
		public short 槽位 { get; set; } = 1;
		public int 采集间隔 { get; set; } = 100;
	}
	public class 工艺参数设置
	{
		public string? 参数名称 { get; set; }
		public string? 参数编码 { get; set; }
		public string? 下限值 { get; set; }
		public string? 上限值 { get; set; }
		public string? 标准值 { get; set; }
	}
}
