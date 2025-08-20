using EFDbContext;
using EFDbContext.Entity;
using Lmes.全局变量;
using MahApps.Metro.Controls;
using PropertyChanged;
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

namespace Lmes.界面
{
    /// <summary>
    /// 异常绑定重置界面.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class 异常绑定重置界面 : MetroWindow
    {
        public BindingList<ExceptionBindingInformation> 异常绑定信息表格 { get; set; }

        public 异常绑定重置界面()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (数据库连接 数据库 = new 数据库连接())
            {
                异常绑定信息表格 = new BindingList<ExceptionBindingInformation>(
                    数据库.异常绑定信息
                        .OrderByDescending(x => x.time)
                        .Take(100)
                        .ToList()
                );
            }
            DataContext = this;
        }

        private void UnbindButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataItem = button?.DataContext as ExceptionBindingInformation;
            if (dataItem != null)
            {
                var confirmResult = MessageBox.Show("确定要解除该绑定吗？", "确认操作", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    using (var db = new 数据库连接())
                    {
                        var entity = db.异常绑定信息.Find(dataItem.ID);
                        if (entity != null)
                        {
                            if (entity.exceptionType == "托盘码重复")
                            {
                                var tmp = db.生产实时信息.FirstOrDefault(x => x.virtualSN == dataItem.exceptionCode);
                                if (tmp != null)
                                {
                                    tmp.virtualSN = "";
                                }
                            }
                            else if (entity.exceptionType == "物料码重复")
                            {
                                var tmp = db.物料绑定信息数据.FirstOrDefault(x => x.assemblyMaterialCode == dataItem.exceptionCode);
                                if (tmp != null)
                                {
                                    tmp.assemblyMaterialCode = "";
                                }
                            }
                            db.异常绑定信息.Remove(entity);
                            db.SaveChanges();
                        }
                    }
                    异常绑定信息表格.Remove(dataItem);
                    MessageBox.Show("解除绑定成功！");
                }
            }
        }
    }
}
