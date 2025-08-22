using DocumentFormat.OpenXml.Drawing.Charts;
using EFDbContext;
using EFDbContext.Entity;
using Lmes.Models;
using Lmes.全局变量;
using Lmes.功能;
using Lmes.功能.数据类型请求体;
using Lmes.功能.枚举;
using Lmes.工具;
using Lmes.界面;
using Lmes.站逻辑;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using PropertyChanged;
using S7.Net;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Brushes = System.Windows.Media.Brushes;
using DateTime = System.DateTime;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;
namespace Lmes
{
	/// <summary>
	/// 测试.xaml 的交互逻辑
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class 主界面 : MetroWindow
	{
		#region 变量声明
		public bool 屏蔽进站 { get; set; }
		public bool 屏蔽出站 { get; set; }

		public bool 屏蔽工厂MES { get; set; } = false;

		/// <summary>
		/// Key：工站编号
		/// </summary>
		public Dictionary<string, 工位信息> 工位信息表 { get; set; }
		public List<string> 工位运行列表 { get; set; } = new();

		Dictionary<string, 工站Base> 工站实例列表 = new();
		string 当前选择工站;
		public List<string> 运行工站列表 { get; set; }

		private MesApi mesApi;
		private LmesApi lmesApi;
		//mesApi = new MesApi(系统参数.设置.Lmes连接参数.LMES地址 + @"api/", 系统参数.设置.Lmes连接参数.appId, 系统参数.设置.Lmes连接参数.appKey);
		private List<工站Base> 工站列表 { get; set; }
		//private List<工站Base> 工站列表 { get; set; }

		CancellationTokenSource? 工站取消令牌;
		public 系统参数 系统参数 { get; } = new();
		public bool 调试模式
		{
			get
			{
				return 系统参数.设置.调试模式;
			}
			set 
			{
				系统参数.设置.调试模式 = value;

            }
		}


		public BindingList<工站对应物料信息> 物料信息表格 { get; set; } = new();

		#endregion

		public 主界面()
		{
			InitializeComponent();
			var version = Assembly.GetExecutingAssembly().GetName().Version;
			Title += $">>软件版本: {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
			系统参数.设置 = JsonSerializer.Deserialize<设置类>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, "配置文件.json")));
			if (Directory.Exists("D:\\物料保存") == false) Directory.CreateDirectory("D:\\物料保存");
			if (Directory.Exists("./文件变动测试") == false) Directory.CreateDirectory("./文件变动测试");

			// 订阅日志写入事件
			日志写入.日志写入事件 += (s, c) =>
			{
				c ??= Brushes.Black;
				Dispatcher.Invoke(() =>
				{
					if (日志区.Items.Count > 系统参数.设置.日志最大数量 + 10)
					{
						for (int i = 0; i < 10; i++)
						{
							日志区.Items.RemoveAt(i);
						}
					}
					var temp = new TextBlock() { Text = s, Foreground = c, FontSize = 24, TextWrapping = TextWrapping.WrapWithOverflow };
					日志区.Items.Add(temp);
					日志区.ScrollIntoView(temp);
				});
			};

			日志写入.工位日志写入事件 += (工位编号, s, c) =>
			{
				if (!当前选择工站.Contains(工位编号)) return;
				c ??= Brushes.Black;
				Dispatcher.Invoke(() =>
				{
					if (日志区.Items.Count > 系统参数.设置.日志最大数量 + 10)
					{
						for (int i = 0; i < 10; i++)
						{
							日志区.Items.RemoveAt(i);
						}
					}
					var temp = new TextBlock() { Text = s, Foreground = c, FontSize = 24, TextWrapping = TextWrapping.WrapWithOverflow };
					日志区.Items.Add(temp);
					日志区.ScrollIntoView(temp);
				});
			};

			//读取配置文件();
			工厂MES初始化();
			DataContext = this;
			fw.Created += (o, e) =>
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "文件创建",
					Message = $"{e.FullPath}",
					Type = NotificationType.Information
				}, areaName: "WindowArea");
			};
			fw.Changed += (o, e) =>
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "文件更新",
					Message = $"{e.FullPath}",
					Type = NotificationType.Information
				}, areaName: "WindowArea");
			};
			//注册信号表更新事件
			系统参数.信号表修改事件 += (工站, temp) =>
			{
				File.WriteAllText(Path.Combine(系统参数.配置文件路径, $"{工站}.信号表"), JsonSerializer.Serialize(temp, new JsonSerializerOptions()
				{
					Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
					WriteIndented = true,
				}));
			};
			//注册配置文件更新事件
			系统参数.配置文件修改事件 += (temp) =>
			{
				File.WriteAllText(Path.Combine(系统参数.配置文件路径, "配置文件.json"), JsonSerializer.Serialize(temp, new JsonSerializerOptions()
				{
					Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
					WriteIndented = true,
				}));
			};
			fw.EnableRaisingEvents = true;
			信号监视.IsEnabled = false;
		}
		private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await Task.Delay(1000);
			//_ = 自动更新程序();
			_ = 服务器hub初始化();
			await 产线初始化();
			await 加载物料信息();
		}

		private async Task 自动更新程序()
		{
			var 本地目录 = AppDomain.CurrentDomain.BaseDirectory;
			var 临时更新目录 = Path.Combine(本地目录, "Updates");

			// 确保临时更新目录存在
			if (!Directory.Exists(临时更新目录))
			{
				Directory.CreateDirectory(临时更新目录);
			}
			try
			{
				Title = "当前版本信息:" + await lmesApi.更新客户端程序();
			}
			catch (Exception ex)
			{
				// 更新失败，但允许程序继续运行
				日志写入.写入($"检查更新失败: {ex.Message}");
			}
		}

		private void 工厂MES初始化()
		{
			mesApi = new MesApi(系统参数.设置.Lmes连接参数.LMES地址 + @"api/", 系统参数.设置.Lmes连接参数.appId, 系统参数.设置.Lmes连接参数.appKey);
			lmesApi = new LmesApi(系统参数.设置.Lmes连接参数.LMES地址);
		}
		//监视文件系统更改的类
		FileSystemWatcher fw = new FileSystemWatcher(@"./");
		/// <summary>
		/// 参数设置
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			new 参数设置界面().Show();
		}

		public string 连接状态 { get; set; } = "PLC连接状态:未连接";

		private async void 菜单_连接_Click(object sender, RoutedEventArgs e)
		{
			菜单_连接.IsEnabled = false; // 禁用按钮
			try
			{
				if (连接状态 == "当前状态:已连接")
				{
					_ = notificationManager.ShowAsync(new NotificationContent
					{
						Title = "连接失败",
						Message = "连接失败,请勿重复连接!",
						Type = NotificationType.Information
					}, areaName: "WindowArea");
					return;
				}
				工站取消令牌 = new CancellationTokenSource();
				var cancellationToken = 工站取消令牌.Token;
				await Task.Run(async () => { await 工站初始化(cancellationToken); });
				foreach (var item in 工站实例列表)
				{
					_ = item.Value.运行(cancellationToken);
				}
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "连接成功",
					Message = "连接成功!",
					Type = NotificationType.Success
				}, areaName: "WindowArea");
				连接状态 = "当前状态:已连接";
				信号监视.IsEnabled = true;
			}
			catch (OperationCanceledException cex)
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "取消连接",
					Message = "取消连接!" + cex.Message,
					Type = NotificationType.Information
				}, areaName: "WindowArea");
				连接状态 = "当前状态:未连接";
				return;
			}
			catch (Exception ex)
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "连接失败",
					Message = "连接失败!" + ex.Message,
					Type = NotificationType.Error
				}, areaName: "WindowArea");
				连接状态 = "当前状态:未连接";
				return;
			}
			finally
			{
				菜单_连接.IsEnabled = true; // 恢复按钮
			}
		}

		NotificationManager notificationManager = new NotificationManager(NotificationPosition.TopRight);
		private async void 菜单_断开_Click(object sender, RoutedEventArgs e)
		{
			if (连接状态.Contains("未连接"))
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "断开失败",
					Message = "请先连接!",
					Type = NotificationType.Information
				}, areaName: "WindowArea");
				return;
			}
			日志写入.写入("已经断开", Brushes.Red);
			工站取消令牌?.Cancel();
			系统参数.物料信息表格.Clear();
			物料信息表格.Clear();
			//if (工站任务 != null) await 工站任务;
			var notificationContent = new NotificationContent
			{
				Title = "断开连接",
				Message = "断开连接成功.",
				Type = NotificationType.Success
			};
			连接状态 = "PLC连接状态:未连接";
			_ = notificationManager.ShowAsync(notificationContent, areaName: nameof(WindowArea));
			信号监视.IsEnabled = false;
			//await this.ShowMessageAsync("停止成功", "停止成功");
		}

		#region 产线初始化

		private async Task 产线初始化()
		{
			try
			{
				if (File.Exists(Path.Combine(系统参数.配置文件路径, "产线信息.json")) == false)
				{
					List<产线信息类> tmp = new();
					File.WriteAllText(Path.Combine(系统参数.配置文件路径, "产线信息.json"), JsonSerializer.Serialize(tmp, 系统参数.整理格式));
				}
				系统参数.产线信息 = JsonSerializer.Deserialize<List<产线信息类>>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, "产线信息.json")));
				mesApi = new MesApi(系统参数.设置.Lmes连接参数.LMES地址 + @"api/", 系统参数.设置.Lmes连接参数.appId, 系统参数.设置.Lmes连接参数.appKey);
				//await 获取排程信息();
				//await 获取工艺路径();
			}
			catch (Exception ex)
			{
				await this.ShowMessageAsync("产线初始化失败", ex.ToString());
			}
		}

		//private async Task 获取工艺路径()
		//{
		//	系统参数.产线信息.工艺路径 = new 工艺路径(await mesApi.工单路径数据接口(new 工单路径数据接口请求体
		//	{
		//		factoryCode = 系统参数.产线信息.工厂编号,
		//		scheduleNumber = 系统参数.产线信息.排程编码,
		//		requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
		//	}));
		//}

		#endregion

		#region 工站初始化
		private async Task 工站初始化(CancellationToken cancellationToken)
		{
			工位信息表 = new Dictionary<string, 工位信息>();
			工站列表 = new List<工站Base>();
			foreach (var 产线 in 系统参数.产线信息)
			{
				运行工站列表 = 产线.运行工站列表;
				当前选择工站 = 运行工站列表[0];
				foreach (var 运行工站 in 运行工站列表)
				{
					var 工位配置信息 = JsonSerializer.Deserialize<工位信息>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, $"{运行工站}_1.信号表")));
					var cpuType = CpuType.S71200;
					switch (工位配置信息.PLC连接设置.CPU类型)
					{
						case "S71500":
							cpuType = CpuType.S71500;
							break;
						case "S71200":
							cpuType = CpuType.S71200;
							break;
						default:
							break;
					}
					var plc = await 连接(cpuType, 工位配置信息.PLC连接设置.IP地址, 工位配置信息.PLC连接设置.机架, 工位配置信息.PLC连接设置.槽位, cancellationToken);
					for (int i = 1; i <= 工位配置信息.工站工位数; i++)
					{
						工位信息表.Add($"{运行工站}_{i}", JsonSerializer.Deserialize<工位信息>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, $"{运行工站}_{i}.信号表"))));
						工位运行列表.Add($"{运行工站}_{i}");
						var tmp1 = (工站Base)Activator.CreateInstance(Type.GetType($"Lmes.站逻辑.{工位信息表[$"{运行工站}_{i}"].工位类型}"), [产线, 工位信息表[$"{运行工站}_{i}"], plc]);
						tmp1.运行初始化(new CancellationToken());
						//工站实例列表.Add(运行工站, tmp1);
						if (!工站实例列表.ContainsKey($"{运行工站}_{i}"))
						{
							工站实例列表.Add($"{运行工站}_{i}", tmp1);
						}
						else
						{
							// 处理键已存在的情况，例如更新值或忽略
							工站实例列表[$"{运行工站}_{i}"] = tmp1; // 更新值
						}
					}
				}
			}
		}

		/// <summary>
		/// PLC连接
		/// </summary>
		public virtual async Task<Plc> 连接(CpuType cpuType, string ip, short rack, short slot, CancellationToken cancellationToken)
		{
			Plc plc = new(cpuType, ip, rack, slot);
			plc.WriteTimeout = 3000;
			plc.ReadTimeout = 3000;
			await plc.OpenAsync(cancellationToken);
			return plc;
		}
		#endregion

		private async void 按钮_切换_Click(object sender, RoutedEventArgs e)
		{
			var temp = await this.ShowInputAsync("请输入员工工号", "请输入员工工号");
		}

		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo(Path.Combine(系统参数.配置文件路径)) { UseShellExecute = true });

		}

		private void MenuItem_日志文件夹_Click(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo(日志写入.日志目录) { UseShellExecute = true });
		}

		private void Button_生产参数信息界面_Click(object sender, RoutedEventArgs e)
		{
			new 生产参数信息查询界面().Show();
		}

		private void MenuItem_Click_11(object sender, RoutedEventArgs e)
		{
			File.WriteAllText(Path.Combine(系统参数.配置文件路径, "配置文件.json"), JsonSerializer.Serialize(系统参数.设置, 系统参数.整理格式));

		}

		private void Button_Click_9(object sender, RoutedEventArgs e)
		{
			new 生产过站信息查询界面().Show();
		}

		private void Button_设备状态消息查询界面_Click(object sender, RoutedEventArgs e)
		{
			new 设备状态信息查询界面().Show();
		}

		private void Button_设备稼动时长数采信息查询界面_Click(object sender, RoutedEventArgs e)
		{
			new 设备稼动时长数采信息查询界面().Show();
		}

		private void Button_预警信息查询界面_Click(object sender, RoutedEventArgs e)
		{
			new 预警信息查询界面().Show();
		}
		private void Button_UDP通讯测试_Click(object sender, RoutedEventArgs e)
		{
			new UDP通讯测试().Show();
		}
		private void Button_机器人控制_Click(object sender, RoutedEventArgs e)
		{
			new 机器人控制().Show();
		}

		private void Button_站点设置界面_Click(object sender, RoutedEventArgs e)
		{
			new 站点设置界面(当前选择工站).Show();
		}

		private void Button_物料绑定信息查询界面_Click(object sender, RoutedEventArgs e)
		{
			new 物料绑定信息查询界面().Show();
		}

		private void Button_信号监视界面_Click(object sender, RoutedEventArgs e)
		{
			new 信号监视页面(工位信息表, 运行工站列表, 工站实例列表).Show();
		}

		private void Button_物料名称设置界面_Click(object sender, RoutedEventArgs e)
		{
			new 物料名称设置界面().Show();
		}
		private void Button_异常绑定重置界面_Click(object sender, RoutedEventArgs e)
		{
			new 异常绑定重置界面().Show();
		}

		private async Task 加载物料信息()
		{
			//获取工单BOM数据
			工单BOM数据接口请求体 tmp = new()
			{
				factoryCode = 系统参数.设置.工厂编号,
				requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
			};
			try
			{
                var 工单BOM数据 = await mesApi.工单BOM数据接口(tmp);
				if (工单BOM数据 == null || 工单BOM数据.code != "000000")
				{
                    系统参数.物料信息表格 = JsonSerializer.Deserialize<BindingList<工站对应物料信息>>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, "工单BOM.json")));
                    return;
				}
                foreach (var item in 工单BOM数据.data)
                {
                    系统参数.物料信息表格.Add(new 工站对应物料信息()
                    {
                        组件物料ID = item.assemblyMaterialId,
                        组件物料编号 = item.assemblyMaterialCode,
                        组件物料名称 = item.assemblyMaterialName,
                        组件物料用量 = item.assemblyConsumption,
                        组件物料版本 = item.assemblyMaterialVersion,
                        组件物料单位 = item.assemblyUnitCode,
                        对应工站 = item.operationCode,
                    });
                }
                File.WriteAllText(Path.Combine(系统参数.配置文件路径, "工单BOM.json"), JsonSerializer.Serialize(系统参数.物料信息表格, 系统参数.整理格式));
            }
			catch
			{
				系统参数.物料信息表格 = JsonSerializer.Deserialize<BindingList<工站对应物料信息>>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, "工单BOM.json")));
            }
            
        }

		private async void 加载当前工位物料信息(string? 当前选择工站)
		{
			物料信息表格.Clear();
			foreach (var item in 系统参数.物料信息表格)
			{
				if (当前选择工站.Contains(item.对应工站))
				{
					物料信息表格.Add(item);
				}
			}
		}

		private void 加载当前工位日志(string? 当前选择工站)
		{
			string logFilePath = Path.Combine(日志写入.日志目录, $"{当前选择工站}.log");
			if (File.Exists(logFilePath))
			{
				日志区.Items.Clear(); // 清空当前日志区内容

				var logLines = File.ReadAllLines(logFilePath);

				foreach (var line in logLines)
				{
					var logItem = new TextBlock()
					{
						Text = line,
						Foreground = Brushes.Black, // 默认颜色
						FontSize = 24,
						TextWrapping = TextWrapping.WrapWithOverflow
					};
					日志区.Items.Add(logItem);
				}

				if (日志区.Items.Count > 0)
				{
					日志区.ScrollIntoView(日志区.Items[^1]); // 滚动到最后一条日志
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
		private void 日志区_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			// 检查是否按下 Ctrl+C
			if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				var selectedItems = 日志区.SelectedItems;
				if (selectedItems.Count > 0)
				{
					// 将选中的项转换为字符串，使用换行符分隔
					var copiedText = string.Join(Environment.NewLine, selectedItems.Cast<TextBlock>()
						.Select(s => s.Text));

					// 将文本复制到剪贴板
					Clipboard.SetDataObject(copiedText);

					//MessageBox.Show("已复制到剪贴板！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}

		private void MenuItem_复制_Click(object sender, RoutedEventArgs e)
		{
			var selectedItems = 日志区.SelectedItems;
			if (selectedItems.Count > 0)
			{
				//将选中的项转换为字符串，使用换行符分割
				var copiedText = string.Join(Environment.NewLine, selectedItems.Cast<TextBlock>().Select(s => s.Text));

				// 将文本复制到剪贴板
				Clipboard.SetDataObject(copiedText);
			}
		}
		
		#region 设备工具
		private async void MenuItem_信号表切换_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog ofd = new OpenFileDialog();
				//设置文件过滤器
				ofd.Filter = "信号表文件 (*.信号表)|*.信号表";
				ofd.DefaultExt = "信号表";
				//不允许多选
				ofd.Multiselect = false;
				//显示对话框并检查用户是否选择了文件
				if (ofd.ShowDialog() == true)
				{
					string filePath = ofd.FileName;
					var str = await File.ReadAllTextAsync(filePath);

					var temp = JsonSerializer.Deserialize<工位信息>(str);
					await 服务器hub.InvokeAsync("请求切换信号表", temp.工位编号, str);
				}
			}
			catch (Exception ex)
			{
				await this.ShowMessageAsync("发送失败", ex.ToString());
			}
		}
		private async void MenuItem_配置文件切换_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog ofd = new OpenFileDialog();
				//设置文件过滤器，只显示名为"配置文件.json"的文件
				ofd.Filter = "JSON文件 (配置文件.json)|配置文件.json";
				ofd.DefaultExt = "json";
				//不允许多选
				ofd.Multiselect = false;
				//显示对话框并检查用户是否选择了文件
				if (ofd.ShowDialog() == true)
				{
					string filePath = ofd.FileName;
					var str = await File.ReadAllTextAsync(filePath);
					var temp = JsonSerializer.Deserialize<设置类>(str);
					await 服务器hub.InvokeAsync("请求切换配置文件", str);
				}
			}
			catch (Exception ex)
			{
				await this.ShowMessageAsync("发送失败", ex.ToString());
			}
		}
		#endregion
		//private async void 菜单_hub测试_Click(object sender, RoutedEventArgs e)
		//{
		//	var s = await this.ShowInputAsync("请输入广播内容", "请输入");
		//	await 服务器hub.InvokeAsync("SendMessage", $"{实时信息.工号}", s);
		//}
		#region 工具
		private void 屏蔽工厂MES_Click(object sender, RoutedEventArgs e)
		{
			if (屏蔽工厂MES == true)
			{
				系统参数.设置.Lmes连接参数.产线MES地址 = @"http://127.0.0.1:5000/";
			}
			else
			{
				try
				{
					系统参数.设置.Lmes连接参数.产线MES地址 = JsonSerializer.Deserialize<设置类>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, "配置文件.json"))).Lmes连接参数.产线MES地址;
				}
				catch (Exception ex)
				{
					this.ShowDialog("读取配置文件发生错误\r\n" + ex.ToString());
				}
			}
		}

		private async void 清空数据库_Click(object sender, RoutedEventArgs e)
		{
			using (数据库连接 数据库 = new())
			{
				数据库.生产实时信息.RemoveRange(数据库.生产实时信息);
				数据库.生产过站信息.RemoveRange(数据库.生产过站信息);
				数据库.生产参数信息.RemoveRange(数据库.生产参数信息);
				数据库.生产参数信息数据.RemoveRange(数据库.生产参数信息数据);
				数据库.物料绑定信息.RemoveRange(数据库.物料绑定信息);
				数据库.物料绑定信息数据.RemoveRange(数据库.物料绑定信息数据);
				数据库.设备实时信息.RemoveRange(数据库.设备实时信息);
				数据库.设备状态信息.RemoveRange(数据库.设备状态信息);
				数据库.设备稼动时长数据.RemoveRange(数据库.设备稼动时长数据);
				数据库.生产不良信息数据.RemoveRange(数据库.生产不良信息数据);
				数据库.生产不良信息数据列表.RemoveRange(数据库.生产不良信息数据列表);
                await 数据库.SaveChangesAsync();
			}
		}
		#endregion

		#region 服务器hub初始化
		private HubConnection 服务器hub;
		private async Task 服务器hub初始化()
		{
			服务器hub = new HubConnectionBuilder()
				.WithAutomaticReconnect()
				.WithUrl($"{系统参数.设置.Lmes连接参数.产线MES地址}Hub")
				.Build();
			//服务器hub.On<string, string>("ReceiveMessage", (user, message) =>
			//{
			//	_ = notificationManager.ShowAsync(new NotificationContent
			//	{
			//		Title = "收到来自服务器的消息",
			//		Message = $"{user}>>{message}",
			//		Type = NotificationType.Information
			//	}, areaName: "WindowArea");
			//});
			服务器hub.On<string, string>("切换信号表", (工站, 信号表) =>
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "收到切换信号表的请求",
					Message = $"{工站}>>{信号表}",
					Type = NotificationType.Information
				}, areaName: "WindowArea");
				日志写入.写入("收到服务器的切换信号表请求!");
				try
				{
					var temp = JsonSerializer.Deserialize<工位信息>(信号表);
					if (工位信息表 != null)
					{
						工位信息表[工站] = temp;
					}
					系统参数.信号表修改(工站, temp);
					日志写入.写入("切换信号表成功!");
				}
				catch (Exception ex)
				{
					日志写入.写入("切换信号表失败!\r\n" + ex.ToString());
				}
			});
			服务器hub.On<string>("切换配置文件", (配置文件) =>
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "收到切换配置文件的请求",
					Message = $"{配置文件}",
					Type = NotificationType.Information
				}, areaName: "WindowArea");
				日志写入.写入("收到服务器的修改配置文件请求!");
				try
				{
					var temp = JsonSerializer.Deserialize<设置类>(配置文件);
					if (系统参数.设置 != null)
					{
						系统参数.设置 = temp;
					}
					系统参数.配置文件修改(temp);
					日志写入.写入("修改配置文件成功!");
				}
				catch (Exception ex)
				{
					日志写入.写入("修改配置文件失败!\r\n" + ex.ToString());
				}
			});
			//服务器hub.On<工站_枚举, string>("切换站名称", (工站, 站名称) =>
			//{
			//	if (工站 != 系统参数.设置.当前工站) return;
			//	_ = notificationManager.ShowAsync(new NotificationContent
			//	{
			//		Title = "收到切换站名称的请求",
			//		Message = $"{工站}>>{站名称}",
			//		Type = NotificationType.Information
			//	}, areaName: "WindowArea");
			//	日志写入.写入($"收到服务器的切换站名称的请求!本站更名为:{站名称}");
			//	//实时信息.工位编号 = 站名称;
			//	File.WriteAllText(Path.Combine(系统参数.配置文件路径, "配置文件.json"), JsonSerializer.Serialize(系统参数.设置, 系统参数.整理格式));

			//});
			try
			{
				await 服务器hub.StartAsync();
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "hub连接成功",
					Message = $"hub连接成功",
					Type = NotificationType.Success
				}, areaName: "WindowArea");
			}
			catch (Exception ex)
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = "hub连接失败",
					Message = $"{ex}",
					Type = NotificationType.Error
				}, areaName: "WindowArea");
			}
		}

		#endregion


		private void 调试模式_Click(object sender, RoutedEventArgs e)
		{
			系统参数.设置.调试模式 = 调试模式;
			File.WriteAllText(Path.Combine(系统参数.配置文件路径, "配置文件.json"), JsonSerializer.Serialize(系统参数.设置, 系统参数.整理格式));
		}

		private void Button_产品NG管控_Click(object sender, RoutedEventArgs e)
		{
			new 生产管理().Show();
		}
	}
	[AddINotifyPropertyChangedInterface]
	public partial class 工站对应物料信息
	{
		public string? 组件物料编号 { get; set; }
		public string? 组件物料ID { get; set; }
		public string? 组件物料名称 { get; set; }
		public string? 组件物料用量 { get; set; }
		public string? 组件物料版本 { get; set; }
		public string? 组件物料单位 { get; set; }
		public bool 是否已经绑定 { get; set; } = false;
		public string? 对应工站 { get; set; }
	}
}