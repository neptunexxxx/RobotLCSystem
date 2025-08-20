using DocumentFormat.OpenXml.Bibliography;
using EFDbContext;
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lmes.界面
{
    /// <summary>
    /// 生产过站信息查询.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class 生产管理 : MetroWindow
    {
        public 生产管理()
        {
            InitializeComponent();
            DataContext = this;
        }

        public BindingList<RealTimeProductInfo> 生产实时信息 { get; set; }
        public bool 是否时间筛选 { get; set; }
        public DateTime 开始时间 { get; set; }
        public DateTime 结束时间 { get; set; }
        public string 工位编号 { get; set; }
        public string 产品SN { get; set; }
        public string 托盘码 { get; set; }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            开始时间 = DateTime.Now.Date;
            结束时间 = DateTime.Now.Date.AddDays(1);
            using (数据库连接 数据库 = new())
            {
                生产实时信息 = new(数据库.生产实时信息.OrderByDescending(x => x.updateTime).Take(100).ToList());
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            List<RealTimeProductInfo>? temp;
            using (数据库连接 数据库 = new())
            {
                temp = 数据库.生产实时信息.ToList();
                if (是否时间筛选)
                {
                    temp = temp.Where(x => string.Compare(x.passEndTime, 结束时间.ToString("yyyy-MM-dd HH:mm:ss")) < 0 && string.Compare(x.passEndTime, 开始时间.ToString("yyyy-MM-dd HH:mm:ss")) >= 0).ToList();
                }
                if (工位编号 != null)
                {
                    temp = temp.Where(x => x.stationCode.Contains(工位编号)).ToList();
                }
                if (产品SN != null)
                {
                    temp = temp.Where(x => x.snNumber.Contains(产品SN)).ToList();
                }
                if (托盘码 != null)
                {
                    temp = temp.Where(x => x.virtualSN.Contains(托盘码)).ToList();
                }
                if (temp != null)
                {
                    生产实时信息 = new(temp);
                }
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            DataHelper.ExportDataGridToExcel(生产实时信息_DataGrid, "生产实时信息");
        }

        private async void 托盘解绑_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataItem = button?.DataContext as RealTimeProductInfo;
            if (dataItem != null)
            {
                var confirmResult = MessageBox.Show("确定执行托盘解绑操作", "确认操作", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    using (var db = new 数据库连接())
                    {
                        var entity = db.生产实时信息.FirstOrDefault(x => x.snNumber == dataItem.snNumber);
                        entity.virtualSN = "";
                        db.SaveChanges();
                    }
                }
                Search_Click(null, null);
            }
        }

        private async void 手动NG_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataItem = button?.DataContext as RealTimeProductInfo;
            if (dataItem != null)
            {
                using (var db = new 数据库连接())
                {
                    var entity = db.生产实时信息.FirstOrDefault(x => x.snNumber == dataItem.snNumber);
                    new 手动NG信息界面(entity.snNumber).Show();
                }
                Search_Click(null, null);
            }
        }

        private async void 产品返修_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataItem = button?.DataContext as RealTimeProductInfo;
            if (dataItem != null)
            {
                if (!(bool)dataItem.isbad)
                {
                    MessageBox.Show("良品不允许返工");
                    return;
                }
                var confirmResult = MessageBox.Show("确定执行产品返修操作", "确认操作", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    using (var db = new 数据库连接())
                    {
                        var entity = db.生产实时信息.FirstOrDefault(x => x.snNumber == dataItem.snNumber);
                        entity.isbad = false;
                        entity.stationCode = "";
                        entity.reProductCount += 1;
                        foreach (var 产线信息 in 系统参数.产线信息)
                        {
                            if (产线信息.运行工站列表.Contains(entity.ngStation))
                            {
                                int ngIndex = 产线信息.工艺路径.工序路径.IndexOf(产线信息.工艺路径.工序字典[entity.ngStation]);
                                if (ngIndex == 0)
                                {
                                    entity.stationCode = "0";
                                }
                                else
                                {
                                    entity.stationCode = 产线信息.工艺路径.工序字典.FirstOrDefault(x => x.Value == 产线信息.工艺路径.工序路径[ngIndex - 1]).Key;
                                }
                                entity.ngStation = "";
                            }
                        }
                        db.SaveChanges();
                    }
                }
                Search_Click(null, null);
            }
        }
    }
}
