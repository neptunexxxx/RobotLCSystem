using EFDbContext;
using Lmes.全局变量;
using Lmes.功能;
using Lmes.功能.数据类型请求体;
using MahApps.Metro.Controls;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
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

namespace Lmes.界面
{
    /// <summary>
    /// 手动NG信息界面.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class 手动NG信息界面 : MetroWindow
    {
        public List<string> 工站列表 { get; set; } = Enumerable.Range(1, 21).Select(n => $"OP{(n * 10):D3}").ToList();
        public string NG工站 { get; set; }
        public List<string> NG原因列表 { get; set; } = ["装配不良", "材质不良","尺寸不良", "外观不良", "物料不良", "检测不良", "制程不良"];
        public string NG原因 { get; set; }

        private string SN;
        public 手动NG信息界面(string SN)
        {
            InitializeComponent();
            DataContext = this;
            this.SN = SN;
        }
        public 手动NG信息界面()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Button_保存_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new 数据库连接())
            {
                var tmp = db.生产实时信息.FirstOrDefault(x => x.snNumber == SN);
                if (tmp != null)
                {
                    tmp.isbad = true;
                    tmp.ngStation = NG工站;
                    db.SaveChanges();
                }
                var lmesapi = new LmesApi(系统参数.设置.Lmes连接参数.LMES地址);
                var mesapi = new MesApi(系统参数.设置.Lmes连接参数.LMES地址 + @"api/", 系统参数.设置.Lmes连接参数.appId, 系统参数.设置.Lmes连接参数.appKey);
                var data = new 生产不良数据接口请求体()
                {
                    factoryCode = 系统参数.设置.工厂编号,
                    requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    data = new()
                    {
                        lineCode = tmp.lineCode,
                        stationCode = NG工站,
                        datalist = [
                            new()
                            {
                                badCode = 不良代码(),
                                badQty = "1",
                                badFactor = NG原因,
                                editTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            }]
                    }
                };
                Task<bool> task1 = lmesapi.新增不良信息(data);
                Task<生产不良数据接口返回体> task2 = mesapi.生产不良数据(data);
                await Task.WhenAll(task1, task2).ConfigureAwait(false);
                if(task1.Result && task2.Result.code == "000000")
                {
                    MessageBox.Show("不良数据上传成功");
                    日志写入.写入("不良数据上传成功");
                }
                if (task2.Result.code != "000000")
                {
                    MessageBox.Show("不良数据上传MES失败");
                    日志写入.写入("不良数据上传MES失败");
                }
                if (!task1.Result)
                {
                    MessageBox.Show("不良数据保存到数据库失败");
                    日志写入.写入("不良数据保存到数据库失败");
                }
            }
        }

        private string 不良代码()
        {
            switch (NG原因)
            {
                case "装配不良":
                    return "ZPBL";
                case "材质不良":
                    return "CZBL";
                case "尺寸不良":
                    return "CCBL";
                case "外观不良":
                    return "WGBL";
                case "物料不良":
                    return "WLBL";
                case "检测不良":
                    return "JCBL";
                case "制程不良":
                    return "ZCBL";
                default:
                    return "";
            }
        }

        private void Button_取消_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
