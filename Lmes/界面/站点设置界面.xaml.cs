using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DocumentFormat.OpenXml.Drawing.Charts;
using EFDbContext.Entity;
using Lmes.Models;
using Lmes.全局变量;
using Lmes.功能.数据类型请求体;
using Lmes.功能.枚举;
using Lmes.站逻辑.信号名称;
using Lmes.站逻辑.数据类型;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using PropertyChanged;
using S7.Net;
using static MaterialDesignThemes.Wpf.Theme;
using Path = System.IO.Path;

namespace Lmes.界面
{
	/// <summary>
	/// 站点设置界面.xaml 的交互逻辑
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class 站点设置界面 : MetroWindow
	{
		public List<string> 下拉框_工位编号 { get; set; }
		public List<string> 下拉框_工站类别 { get; set; }
		public int 偏移量 { get; set; }

		public int DB块 { get; set; }

		NotificationManager notificationManager = new NotificationManager(NotificationPosition.TopRight);

		public 工位信息? 当前工位信息 { get; set; }

		bool flag = false;
		private string 当前工位 { get; set; }
        private string 当前文件路径;

        public 站点设置界面()
		{
			InitializeComponent();
			DataContext = this;
		}

		public 站点设置界面(string 当前选择工站)
		{
			this.当前工位 = 当前选择工站;
			InitializeComponent();
			DataContext = this;
		}

		private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				下拉框_工位编号 = new(系统参数.工站列表);
				if (File.Exists(Path.Combine(系统参数.配置文件路径, "工位类型.json")) == false)
				{
					List<string> 默认工位类型 = new List<string>() { "通用站" };
					File.WriteAllText(Path.Combine(系统参数.配置文件路径, "工位类型.json"), JsonSerializer.Serialize(默认工位类型, new JsonSerializerOptions()
					{
						Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
						WriteIndented = true,
					}));
					下拉框_工站类别 = 默认工位类型;
				}
				else
				{
					下拉框_工站类别 = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, "工位类型.json")));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("读取配置文件发生错误\r\n" + ex.ToString());
			}
			//读取plc参数文件
			if (当前工位 != null)
			{
				ReadConfigurationPLCFile($"{当前工位}.信号表");
			}
			else
			{
				ReadConfigurationPLCFile("OP010_1.信号表");
			}
			flag = true;
			// 获取存储"生成默认模版"的MenuItem
			var 默认模版菜单 = this.FindName("生成默认模版菜单") as MenuItem;

			// 清空现有菜单项
			默认模版菜单.Items.Clear();

			// 动态添加菜单项
			foreach (var 工站 in 系统参数.工站列表)
			{
				var menuItem = new MenuItem
				{
					Header = 工站,
					Height = 30
				};
				menuItem.Click += MenuItem_生成默认模板_Click;
				默认模版菜单.Items.Add(menuItem);
			}
		}

		private async void ReadConfigurationPLCFile(string filepath)
		{
			try
			{
				if (File.Exists(Path.Combine(系统参数.配置文件路径, filepath)))
				{
					_ = notificationManager.ShowAsync(new NotificationContent
					{
						Title = "切换成功",
						Message = $"切换文件{filepath}成功",
						Type = NotificationType.Success
					}, areaName: "Area");
				}
				else
				{
					_ = notificationManager.ShowAsync(new NotificationContent
					{
						Title = "切换失败",
						Message = $"文件{filepath}不存在，生成默认模板。",
						Type = NotificationType.Warning
					}, areaName: "Area");
					生成默认模板(filepath);
					return;
				}
				当前工位信息 = JsonSerializer.Deserialize<工位信息>(File.ReadAllText(Path.Combine(系统参数.配置文件路径, filepath)));
				刷新();
			}
			catch (Exception ex)
			{
				MessageBox.Show("读取配置文件发生错误\r\n" + ex.ToString());
			}
		}

        private async void Button_保存_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(当前文件路径))
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "信号表文件 (*.信号表)|*.信号表";
                    sfd.DefaultExt = "信号表";
                    if (sfd.ShowDialog() == true)
                    {
                        当前文件路径 = sfd.FileName;
                    }
                    else
                    {
                        return;
                    }
                }

                File.WriteAllText(当前文件路径, JsonSerializer.Serialize(当前工位信息, 系统参数.整理格式));
                await notificationManager.ShowAsync(new NotificationContent
                {
                    Title = "保存成功",
                    Message = "保存成功",
                    Type = NotificationType.Success
                }, areaName: "Area");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存配置文件发生错误\r\n" + ex.ToString());
            }
        }

        private void Button_取消_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Button_一键增加_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in 当前工位信息.信号连接表)
			{
				item.信号地址.StartByteAdr += 偏移量;
			}
			刷新();
		}

		private void Button_一键设置_Click(object sender, RoutedEventArgs e)
		{
			foreach (var item in 当前工位信息.信号连接表)
			{
				item.信号地址.DB = DB块;
			}
			刷新();
		}

		private void MenuItem_生成默认模板_Click(object sender, RoutedEventArgs e)
		{
			生成默认模板((string)(sender as MenuItem).Header);
		}

        private void 生成默认模板(string 工位编号)
        {
            string 文件名 = $"{工位编号}_1.信号表";
            当前文件路径 = Path.Combine(系统参数.配置文件路径, 文件名);

            Type type = Type.GetType($"Lmes.站逻辑.信号名称.{工位编号}信号名称");

            HashSet<string> seenNames = new HashSet<string>();
			if (type == null)
			{
				_ = notificationManager.ShowAsync(new NotificationContent
				{
					Title = $"生成失败",
					Message = $"模板不存在",
					Type = NotificationType.Warning
				}, areaName: "Area");
				return;
			}

			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
				.Where(field => field.FieldType == typeof(string) && seenNames.Add(field.Name));

			if (当前工位信息 != null && 当前工位信息.信号连接表 != null)
			{
                当前工位信息.信号连接表.Clear();
            }

			foreach (var field in fields)
			{
				// 获取字段上的信号参数特性
				var paramAttr = field.GetCustomAttribute<信号参数属性>();

				当前工位信息.信号连接表.Add(new 信号连接()
				{
					信号名称 = field.Name,
					信号地址 = new()
					{
						DataType = DataType.DataBlock,
						DB = 1,
					},
					// 如果有参数特性，则添加参数信息
					参数编码 = paramAttr?.参数编码,
					下限值 = paramAttr?.下限值,
					上限值 = paramAttr?.上限值,
					标准值 = paramAttr?.标准值
				});
			}

			_ = notificationManager.ShowAsync(new NotificationContent
			{
				Title = $"成功生成",
				Message = $"生成模板成功",
				Type = NotificationType.Success
			}, areaName: "Area");

			// 通知UI刷新绑定的数据
			刷新();
		}

		private void 刷新()
		{
			信号列表.ItemsSource = null;
			信号列表.ItemsSource = 当前工位信息.信号连接表;
		}

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MenuItem_打开_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "信号表文件 (*.信号表)|*.信号表";
            ofd.DefaultExt = "信号表";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == true)
            {
                当前文件路径 = ofd.FileName;
                当前工位信息 = JsonSerializer.Deserialize<工位信息>(File.ReadAllText(当前文件路径));
                刷新();
            }
        }

        private async void MenuItem_另存为_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "信号表文件 (*.信号表)|*.信号表";
            sfd.DefaultExt = "信号表";
            if (sfd.ShowDialog() == true)
            {
                当前文件路径 = sfd.FileName;
                try
                {
                    using (FileStream fs = new FileStream(当前文件路径, FileMode.Create))
                    {
                        await JsonSerializer.SerializeAsync(fs, 当前工位信息, 系统参数.整理格式);
                    }
                }
                catch (Exception ex)
                {
                    await this.ShowMessageAsync("写入文件失败", ex.Message);
                }
            }
        }

        private void MenuItem_删除_Click(object sender, RoutedEventArgs e)
		{
			if (信号列表.SelectedItem is 信号连接 selectedItem)
			{
				当前工位信息.信号连接表.Remove(selectedItem);
			}
			刷新();
		}

		private void MenuItem_添加_Click(object sender, RoutedEventArgs e)
		{
			当前工位信息.信号连接表.Insert(信号列表.SelectedIndex + 1, new 信号连接()
			{
				信号名称 = "测试",
				参数编码 = "k8",
				物料编码 = "n1",
				信号地址 = new S7.Net.Types.DataItem()
				{
					DB = 1,
					DataType = DataType.DataBlock,
					BitAdr = 0,
					Count = 1,
					VarType = VarType.Int,
					StartByteAdr = 0,
					Value = null
				}
			});
			刷新();
		}
	}
}
