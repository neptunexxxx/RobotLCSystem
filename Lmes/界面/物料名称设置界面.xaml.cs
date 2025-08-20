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
using EFDbContext;
using EFDbContext.Entity;
using Lmes.Models;
using Lmes.全局变量;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using S7.Net;

namespace Lmes.界面
{
	/// <summary>
	/// 物料名称设置界面.xaml 的交互逻辑
	/// </summary>
	public partial class 物料名称设置界面 : MetroWindow
	{
		public BindingList<MaterialBindInfo> 物料信息表 { get; set; }
		public 数据库连接 数据库 { get; set; }
		public 物料名称设置界面()
		{
			InitializeComponent();
			DataContext = this;
			物料信息表 = new BindingList<MaterialBindInfo>();
			数据库 = new 数据库连接();
			物料信息表 = new BindingList<MaterialBindInfo>(数据库.物料名称信息.ToList());
		}
		private void Button_取消_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private async void Button_保存_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				foreach (var item in 物料信息表)
				{
					var existingItem = 数据库.物料名称信息.FirstOrDefault(x => x.物料名称 == item.物料名称);
					if (existingItem != null)
					{
						// 更新现有项目
						existingItem.物料名称 = item.物料名称;
						existingItem.正则表达式 = item.正则表达式;
						existingItem.是否使用 = item.是否使用;
					}
					else
					{
						// 添加新项目
						数据库.物料名称信息.Add(new MaterialBindInfo
						{
							物料名称 = item.物料名称,
							正则表达式 = item.正则表达式,
							是否使用 = item.是否使用,
						});
					}
				}

				// 提交更改
				数据库.SaveChanges();
				await this.ShowMessageAsync("保存成功", "物料信息已成功保存到数据库。");
			}
			catch (Exception ex)
			{
				await this.ShowMessageAsync("保存失败", $"保存过程中发生错误：{ex.Message}");
			}
		}


		private void MenuItem_添加_Click(object sender, RoutedEventArgs e)
		{
			物料信息表.Insert(物料信息列表.SelectedIndex + 1, new MaterialBindInfo()
			{
				物料名称 = "",
				正则表达式 = "",
				是否使用 = false,
			});
			刷新();
		}

		private void 刷新()
		{
			物料信息列表.ItemsSource = null;
			物料信息列表.ItemsSource = 物料信息表;
		}
	}
}
