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
using EFDbContext.Entity;
using Lmes.功能;
using Lmes.全局变量;
using MahApps.Metro.Controls;
using PropertyChanged;
using Lmes.工具;

namespace Lmes.界面
{
	/// <summary>
	/// 设备稼动时长数采信息查询界面.xaml 的交互逻辑
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class 设备稼动时长数采信息查询界面 : MetroWindow
	{
		public 设备稼动时长数采信息查询界面()
		{
			InitializeComponent();
			DataContext = this;
		}
		public BindingList<DeviceStatusAndDuration> DeviceStatusAndDuration { get; set; }
		public string 设备编码 { get; set; }
		public string 设备状态编码 { get; set; }
		public string 数采设备编码 { get; set; }

		LmesApi lmesApi = new LmesApi(系统参数.设置.Lmes连接参数.产线MES地址);

		private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
		{
			var temp = await lmesApi.查询设备稼动时长数采信息();
			if (temp != null)
			{
				DeviceStatusAndDuration = new(temp);
			}
		}

		/// <summary>
		/// 查询
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Search_Click(object sender, RoutedEventArgs e)
		{
			List<DeviceStatusAndDuration>? temp;

			temp = await lmesApi.查询设备稼动时长数采信息(设备编码, 设备状态编码, 数采设备编码);

			if (temp != null)
			{
				DeviceStatusAndDuration = new(temp);
			}
		}

		/// <summary>
		/// 导出表格
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Export_Click(object sender, RoutedEventArgs e)
		{
			DataHelper.ExportDataGridToExcel(deviceStatusAndDurationInfo, "设备稼动时长数采信息");
		}


	}
}
