using EFDbContext.Entity;
using Lmes.全局变量;
using Lmes.功能;
using Lmes.工具;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PropertyChanged;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Lmes.界面
{
    /// <summary>
    /// 生产过站信息查询.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class 生产过站信息查询界面 : MetroWindow
    {
        public 生产过站信息查询界面()
        {
            InitializeComponent();
            DataContext = this;
        }

        public BindingList<PassStation> PassStation { get; set; }
        public bool 是否时间筛选 { get; set; }
        public DateTime 开始时间 { get; set; }
        public DateTime 结束时间 { get; set; }
        public string 用户ID { get; set; }
        public string 工单编码 { get; set; }
        public string 排程编码 { get; set; }
        public string 工位编号 { get; set; }
        public string 产品SN { get; set; }



        LmesApi lmesApi = new LmesApi(系统参数.设置.Lmes连接参数.产线MES地址);

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            开始时间 = DateTime.Now.Date;
            结束时间 = DateTime.Now.Date.AddDays(1);
            var temp = await lmesApi.查询过站信息();
            if (temp != null)
            {
                PassStation = new(temp);
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            List<PassStation>? temp;
            if (是否时间筛选)
            {
                temp = await lmesApi.查询过站信息(产品SN, 工单编码, 排程编码, 工位编号, 用户ID, 开始时间.ToString(), 结束时间.ToString());
            }
            else
            {
                temp = await lmesApi.查询过站信息(产品SN, 工单编码, 排程编码, 工位编号, 用户ID, null, null);
            }
            if (temp != null)
            {
                PassStation = new(temp);
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            DataHelper.ExportDataGridToExcel(passStatinInfo,"生产过站信息");
        }
    }
}
