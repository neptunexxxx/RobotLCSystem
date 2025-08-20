using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lmes.功能
{
	public static class 日志写入
	{
		public static event Action<string, SolidColorBrush> 日志写入事件;
		public static event Action<string, string, SolidColorBrush> 工位日志写入事件;

		public static SemaphoreSlim 文件写入锁 { get; set; } = new(1, 1);

		public static readonly string 日志目录 = Path.Combine("D:\\日志", DateTime.Now.ToString("yyyy-MM-dd"));
		public static readonly string 总日志文件路径 = Path.Combine(日志目录, "全局日志.log");

		static 日志写入()
		{
			//在项目启动时创建 日志 文件夹
			if (!Directory.Exists(日志目录))
			{
				Directory.CreateDirectory(日志目录);
			}
		}

		public static void 写入(string 日志内容, SolidColorBrush? 颜色 = null)
		{
			文件写入锁.Wait();
			日志写入事件?.Invoke($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}>>{日志内容}", 颜色);
			写入到总日志文件(日志内容);
			文件写入锁.Release();
		}

		public static void 写入(string 工位编号, string 日志内容, SolidColorBrush? 颜色 = null)
		{
			文件写入锁.Wait();
			工位日志写入事件?.Invoke(工位编号, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}>>{日志内容}", 颜色);
			写入到文件(工位编号, 日志内容);
			写入到总日志文件($"{工位编号}>>{日志内容}");
			文件写入锁.Release();
		}

		private static void 写入到文件(string 文件名, string 日志内容)

		{
			string 文件路径 = Path.Combine(日志目录, $"{文件名}.log");
			using (StreamWriter sw = new StreamWriter(文件路径, true))
			{
				sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}>>{文件名}>>{日志内容}");
			}
		}

		private static void 写入到总日志文件(string 日志内容)
		{
			using (StreamWriter sw = new StreamWriter(总日志文件路径, true))
			{
				sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}>>{日志内容}");
			}
		}
	}
}
