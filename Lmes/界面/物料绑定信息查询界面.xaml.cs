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
using Lmes.功能;
using Lmes.全局变量;
using MahApps.Metro.Controls;
using System.ComponentModel;
using EFDbContext.Entity;
using AutoMapper;
using Lmes.工具;
using PropertyChanged;

namespace Lmes.界面
{
    /// <summary>
    /// 物料绑定信息查询界面.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class 物料绑定信息查询界面 : MetroWindow
    {
        public 物料绑定信息查询界面()
        {
            InitializeComponent();
            DataContext = this;
        }
        public BindingList<MaterialBindInfo> MaterialBindInfos { get; set; } = new BindingList<MaterialBindInfo>();
        public bool 是否时间筛选 { get; set; }
        public DateTime 开始时间 { get; set; }
        public DateTime 结束时间 { get; set; }
        public string 工位编号 { get; set; }
        public string 工单编号 { get; set; }
        public string 单体组件物料编码 { get; set; }
        public string 产品SN { get; set; }
        LmesApi lmesApi = new LmesApi(系统参数.设置.Lmes连接参数.产线MES地址);

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            开始时间 = DateTime.Now.Date;
            结束时间 = DateTime.Now.Date.AddDays(1);
            List<MaterialBind>? materialBinds = await lmesApi.查询物料绑定信息();
            if (materialBinds != null)
            {
                将数据添加至绑定信息中(materialBinds);
            }
        }

        private async void Button_查询_Click(object sender, RoutedEventArgs e)
        {
            MaterialBindInfos.Clear();
            List<MaterialBind>? materialBinds;
            if (是否时间筛选)
            {
                if (string.IsNullOrEmpty(单体组件物料编码))
                {
                    materialBinds = await lmesApi.查询物料绑定信息(产品SN, 工位编号, 工单编号, null, 开始时间.ToString(), 结束时间.ToString());
                }
                else
                {
                    var tmp = await lmesApi.查询物料绑定信息数据(单体组件物料编码);
                    materialBinds = await lmesApi.查询物料绑定信息(tmp[0].dataId);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(单体组件物料编码))
                {
                    materialBinds = await lmesApi.查询物料绑定信息(产品SN, 工位编号, 工单编号, null, 开始时间.ToString(), 结束时间.ToString());
                }
                else
                {
                    var tmp = await lmesApi.查询物料绑定信息数据(单体组件物料编码);
                    if (tmp != null && tmp.Count > 0)
                    {
                        materialBinds = await lmesApi.查询物料绑定信息(tmp[0].dataId);
                    }
                    else
                    {
                        materialBinds = new();
                    }
                }
            }
            if (materialBinds != null)
            {
                将数据添加至绑定信息中(materialBinds);
            }
        }

        private async void 将数据添加至绑定信息中(List<MaterialBind> materialBinds)
        {
            foreach (MaterialBind mb in materialBinds)
            {
                List<MaterialBindData>? materialBindDatas = await lmesApi.查询物料绑定信息数据(mb.dataId);
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<MaterialBind, MaterialBindInfo>();
                });
                var mapper = config.CreateMapper();
                MaterialBindInfo materialBindInfo = mapper.Map<MaterialBindInfo>(mb);
                materialBindInfo.data = new BindingList<MaterialBindData>();
                if (materialBindDatas != null)
                {
                    materialBindInfo.data = new BindingList<MaterialBindData>(materialBindDatas);
                }
                //MaterialBindInfo materialBindInfo + materialBindInfo.data => MaterialBindInfos
                MaterialBindInfos.Add(materialBindInfo);
            }
        }

        private void Button_导出_Click(object sender, RoutedEventArgs e)
        {
            DataHelper.ExportDataGridToExcel(materialBindInfos);
        }

        public class MaterialBindInfo
        {
            public string? snNumber { get; set; }

            /// <summary>
            /// 工位名称
            /// </summary>
            public string? stationCode { get; set; }

            /// <summary>
            /// 工单编号
            /// </summary>
            public string? workOrderNumber { get; set; }

            /// <summary>
            /// 单体组件物料编码
            /// </summary>
            public string? materialCode { get; set; }

            /// <summary>
            /// 单体组件物料版本
            /// </summary>
            public string? materialCodeVersion { get; set; }

            /// <summary>
            /// 单体组件物料条码
            /// </summary>
            public string? materialBarcode { get; set; }

            /// <summary>
            /// 用户ID
            /// </summary>
            public string? userId { get; set; }

            /// <summary>
            /// 请求时间
            /// </summary>
            public string? requestTime { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public BindingList<MaterialBindData>? data { get; set; }
        }

        private async void 解除子物料_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataItem = button?.DataContext as MaterialBindData;
            if (dataItem != null)
            {
                var confirmResult = MessageBox.Show("确定要解除该子物料绑定吗？", "确认操作", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        Mouse.OverrideCursor = Cursors.Wait;

                        // 调用API删除子物料绑定信息
                        bool result = await lmesApi.删除物料绑定数据(dataItem.Id);

                        Mouse.OverrideCursor = null;

                        if (result)
                        {
                            // 在UI上更新，找到父物料并从其子物料列表中移除
                            foreach (var bindInfo in MaterialBindInfos)
                            {
                                if (bindInfo.data != null && bindInfo.data.Contains(dataItem))
                                {
                                    bindInfo.data.Remove(dataItem);
                                    MessageBox.Show("子物料绑定解除成功！", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("解除子物料绑定失败，请稍后重试！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        Mouse.OverrideCursor = null;
                        MessageBox.Show($"操作异常：{ex.Message}", "系统错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void 解除主物料_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataItem = button?.DataContext as MaterialBindInfo;
            if (dataItem != null)
            {
                var confirmResult = MessageBox.Show("确定要解除该主物料及其所有子物料的绑定吗？", "确认操作", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        Mouse.OverrideCursor = Cursors.Wait;

                        // 调用API删除主物料绑定信息
                        bool result = await lmesApi.删除物料绑定信息(dataItem.snNumber);

                        Mouse.OverrideCursor = null;

                        if (result)
                        {
                            // 在UI上更新
                            MaterialBindInfos.Remove(dataItem);
                            MessageBox.Show("主物料及其所有子物料绑定解除成功！", "操作成功", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("解除主物料绑定失败，请稍后重试！", "操作失败", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        Mouse.OverrideCursor = null;
                        MessageBox.Show($"操作异常：{ex.Message}", "系统错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
