using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lmes.Models;
using Lmes.工具;
using Lmes.站逻辑;
using MahApps.Metro.Controls;
using PropertyChanged;

namespace Lmes.界面
{
	/// <summary>
	/// 信号监视页面.xaml 的交互逻辑
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class 信号监视页面 : MetroWindow
	{
		public BindingList<表格显示> 信号表格 { get; set; } = new();
		public BindingList<string> 运行工站列表 { get; set; }

		Dictionary<string, 工站Base> 工站实例列表 { get; set; }

		public List<信号连接>? 当前选择信号表 { get; set; }

		CancellationTokenSource 信号监视Token { get; set; } = new();

		Dictionary<string, 工位信息> 工位信息表;
		public 信号监视页面()
		{

		}
		public 信号监视页面(Dictionary<string, 工位信息> 工位信息表, List<string> 运行工站列表, Dictionary<string, 站逻辑.工站Base> 工站实例列表)
		{
			InitializeComponent();
			DataContext = this;
			this.工位信息表 = 工位信息表;
			List<string> 信号监视列表 = new();
			foreach(string tmp in 工位信息表.Keys)
			{
				信号监视列表.Add(tmp);
            }
            this.运行工站列表 = new BindingList<string>(信号监视列表);
			this.工站实例列表 = 工站实例列表;
		}

		private void 当前工站_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			信号监视Token.Cancel();
			信号监视Token = new CancellationTokenSource();
			信号表格.Clear();
			当前选择信号表 = 工位信息表[当前工站.SelectedItem.ToString()].信号连接表;
			foreach (var item in 当前选择信号表)
			{
				信号表格.Add(new() { 信号名 = item.信号名称, 值 = "" });
			}
			Task.Run(async () =>
			{
				while (true)
				{
					更新表格();
					await Task.Delay(200);
				}
			}, 信号监视Token.Token);

		}

		void 更新表格()
		{
			//claude
			foreach (var item in 当前选择信号表)
			{
				try
				{
					var find = 信号表格.FirstOrDefault(s => s.信号名 == item.信号名称);
					if (find == null) continue;

					// 创建一个临时变量来存储从字典获取的时间
					DateTime? tempTime = null;

					// 在UI线程上安全地访问更新时间字典
					Dispatcher.Invoke(() =>
					{
						if (工站实例列表[当前工站.SelectedItem.ToString()].更新时间.TryGetValue(item.信号名称, out var 更新时间))
						{
							tempTime = 更新时间;
						}
					});

					if (item.信号地址.Value?.GetType() == typeof(byte[]))
					{
						string strtemp = "";
						// 将字节数组转换为字符串格式
						strtemp = ByteArrayToString((byte[])item.信号地址?.Value) + "\r\n";
						string s7str, s7wstr;
						try
						{
							s7str = ((byte[])item.信号地址?.Value).s7string转换为字符串();
						}
						catch (Exception)
						{
							s7str = null;
						}
						try
						{
							s7wstr = ((byte[])item.信号地址?.Value).s7Wstring转换为字符串();
						}
						catch (Exception)
						{
							s7wstr = null;
						}
						// 组合所有转换结果
						strtemp += $"string解析>>{s7str}" + "\r\n";
						strtemp += $"wstring解析>>{s7wstr}" + "\r\n";
						// 在UI线程上更新显示值和更新时间
						Dispatcher.Invoke(() =>
						{
							find.值 = strtemp;
							if (tempTime.HasValue)
							{
								find.更新时间 = tempTime.Value;
							}
						});
					}
					else
					{
						Dispatcher.Invoke(() =>
						{
							find.值 = item.信号地址?.Value?.ToString();
							if (tempTime.HasValue)
							{
								find.更新时间 = tempTime.Value;
							}
						});
					}
				}
				catch (Exception ex)
				{
					continue;
				}
			}
		}
		// 将 byte 数组转换成 "[1,2,3,4,5]" 格式的字符串
		static string ByteArrayToString(byte[] byteArray)
		{
			if (byteArray == null || byteArray.Length == 0)
				return "[]";

			// 使用 LINQ 的 Select 方法将每个 byte 转换为字符串，然后用 "," 连接起来
			string content = string.Join(",", byteArray.Select(b => b.ToString()));

			// 格式化为 "[1,2,3,4,5]" 的形式
			return $"[{content}]";
		}

		private void MetroWindow_Closed(object sender, EventArgs e)
		{
			信号监视Token.Cancel();
		}
	}

	[AddINotifyPropertyChangedInterface]
	public partial class 表格显示
	{
		public string 信号名 { get; set; }
		public string 值 { get; set; }
		public DateTime 更新时间 { get; set; }
	}
}
