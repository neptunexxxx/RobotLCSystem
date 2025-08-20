using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
using Lmes.全局变量;
using Lmes.功能;
using Lmes.工具;
using MahApps.Metro.Controls;
using PropertyChanged;

namespace Lmes.界面
{
	/// <summary>
	/// 设备状态消息查询界面.xaml 的交互逻辑
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class 设备状态信息查询界面 : MetroWindow
	{
		public 设备状态信息查询界面()
		{
			InitializeComponent();
			DataContext = this;
		}
		public BindingList<EquipmentStatus> EquipmentStatus { get; set; }

		public bool 是否时间筛选 { get; set; }
		public DateTime 开始时间 { get; set; }
		public DateTime 结束时间 { get; set; }
		public string 设备编码 { get; set; }
		public string 产线编码 { get; set; }
		public string 工位编号 { get; set; }
		public string 设备状态编码 { get; set; }
		//public string 产品SN { get; set; }

		LmesApi lmesApi = new LmesApi(系统参数.设置.Lmes连接参数.产线MES地址);

		private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
		{
			开始时间 = DateTime.Now.Date;
			结束时间 = DateTime.Now.Date.AddDays(1);
			var temp = await lmesApi.查询设备状态信息();
			if (temp != null)
			{
				EquipmentStatus = new(temp);
			}
		}
		/// <summary>
		/// 查询
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private async void Search_Click(object sender, RoutedEventArgs e)
		{
			List<EquipmentStatus>? temp;
			if (是否时间筛选)
			{
				temp = await lmesApi.查询设备状态信息(设备编码, 产线编码, 工位编号, 设备状态编码, 开始时间.ToString(), 结束时间.ToString());
			}
			else
			{
				temp = await lmesApi.查询设备状态信息(设备编码, 产线编码, 工位编号, 设备状态编码, null, null);
			}
			if (temp != null)
			{
				EquipmentStatus = new(temp);
			}
		}

		/// <summary>
		/// 导出表格
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Export_Click(object sender, RoutedEventArgs e)
		{
			DataHelper.ExportDataGridToExcel(equipmentStatusInfo, "设备状态信息");
		}
	}
}
