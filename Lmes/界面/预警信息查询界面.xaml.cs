using System;
using System.Collections.Generic;
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
using EFDbContext.Entity;
using Lmes.工具;
using Lmes.功能.数据类型请求体;
using Lmes.功能;
using MahApps.Metro.Controls;
using PropertyChanged;
using Lmes.全局变量;
using System.ComponentModel;

namespace Lmes.界面
{
	/// <summary>
	/// 预警信息查询界面.xaml 的交互逻辑
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class 预警信息查询界面 : MetroWindow
	{
		public 预警信息查询界面()
		{
			InitializeComponent();
			DataContext = this;
		}

		public BindingList<WarningData> WarningData { get; set; }

		public bool 是否时间筛选 { get; set; }
		public DateTime 开始时间 { get; set; }
		public DateTime 结束时间 { get; set; }
		public string 工位编码 { get; set; }
		public string 设备编码 { get; set; }

		LmesApi lmesApi = new LmesApi(系统参数.设置.Lmes连接参数.产线MES地址);

		private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
		{
			开始时间 = DateTime.Now.Date;
			结束时间 = DateTime.Now.Date.AddDays(1);
			var temp = await lmesApi.查询预警信息();
			if (temp != null)
			{
				WarningData = new(temp);
			}
		}
		/// <summary>
		/// 查询
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Search_Click(object sender, RoutedEventArgs e)
		{
			List<WarningData>? temp;
			if (是否时间筛选)
			{
				temp = await lmesApi.查询预警信息(工位编码, 设备编码, 开始时间.ToString(), 结束时间.ToString());
			}
			else
			{
				temp = await lmesApi.查询预警信息(工位编码, 设备编码, null, null);
			}
			if (temp != null)
			{
				WarningData = new(temp);
			}
		}

		/// <summary>
		/// 导出表格
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Export_Click(object sender, RoutedEventArgs e)
		{
			DataHelper.ExportDataGridToExcel(warningDataInfo, "预警信息");
		}
	}
}
