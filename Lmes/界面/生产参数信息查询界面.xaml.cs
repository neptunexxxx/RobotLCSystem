using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using EFDbContext.Entity;
using Lmes.全局变量;
using Lmes.功能;
using Lmes.工具;
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
    /// 生产参数信息查询界面.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class 生产参数信息查询界面 : MetroWindow
    {
        public 生产参数信息查询界面()
        {
            InitializeComponent();
            DataContext = this;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductParameters, ProductParameterInfo>();
            });
            _mapper = config.CreateMapper();
        }

        public BindingList<ProductParameterInfo> ProductParameterInfos { get; set; } = new BindingList<ProductParameterInfo>();
        public bool 是否时间筛选 { get; set; }
        public DateTime 开始时间 { get; set; }
        public DateTime 结束时间 { get; set; }
        public string 工位编号 { get; set; }
        public string 设备编码 { get; set; }
        public string 产品SN { get; set; }
        LmesApi lmesApi = new LmesApi(系统参数.设置.Lmes连接参数.产线MES地址);
        private readonly IMapper _mapper;

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            开始时间 = DateTime.Now.Date;
            结束时间 = DateTime.Now.Date.AddDays(1);
            Button_Click(null, null);
        }
        private void NestedDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid?.ItemsSource == null) return;

            // 查找参数名对应的列
            var paramNameColumn = dataGrid.Columns
                .FirstOrDefault(c => c.Header?.ToString() == "参数名");

            if (paramNameColumn != null)
            {
                // 设置初始排序方向
                paramNameColumn.SortDirection = ListSortDirection.Descending;

                // 强制触发排序逻辑
                var args = new DataGridSortingEventArgs(paramNameColumn);
                PART_DataGrid_Sorting(dataGrid, args);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductParameterInfos.Clear();
            List<ProductParameters>? productParameters;
            if (是否时间筛选)
            {
                productParameters = await lmesApi.查询生产参数信息(产品SN, 设备编码, 工位编号, 开始时间.ToString(), 结束时间.ToString());
            }
            else
            {
                productParameters = await lmesApi.查询生产参数信息(产品SN, 设备编码, 工位编号, null, null);
            }
            if (productParameters != null)
            {
                foreach (var item in productParameters)
                {
                    // 关键点：使用局部变量捕获当前循环状态
                    var currentDataId = item.dataId; // 创建局部变量
                    var pp = item;                    // 捕获当前循环变量

                    // 获取子数据
                    List<ProductParametersData>? productParametersDatas =
                        await lmesApi.查询生产参数信息数据(currentDataId);

                    // 映射逻辑...
                    ProductParameterInfo tmp = _mapper.Map<ProductParameterInfo>(pp);
                    if (productParametersDatas != null)
                    {
                        tmp.data = new BindingList<ProductParametersData>(productParametersDatas);
                    }
                    ProductParameterInfos.Add(tmp);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataHelper.ExportDataGridToExcel(productParaInfo);
        }

        private void PART_DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            if (e.Column.Header.ToString() != "参数名") return;

            var dataGrid = (DataGrid)sender;
            var direction = e.Column.SortDirection switch
            {
                ListSortDirection.Ascending => ListSortDirection.Descending,
                _ => ListSortDirection.Ascending
            };

            ApplyCustomSorting(dataGrid, direction);
            e.Column.SortDirection = direction;
        }

        // 新增通用排序方法
        private void ApplyCustomSorting(DataGrid dataGrid, ListSortDirection direction)
        {
            if (dataGrid.ItemsSource is not BindingList<ProductParametersData> items) return;

            var sorted = new BindingList<ProductParametersData>(
                items.OrderBy(x => GetSortKey(x.paramName).Prefix)
                     .ThenBy(x => GetSortKey(x.paramName).Number)
                     .ToList());

            if (direction == ListSortDirection.Descending)
            {
                sorted = new BindingList<ProductParametersData>(
                    sorted.Reverse().ToList());
            }

            dataGrid.ItemsSource = sorted;
            dataGrid.Items.Refresh();
        }

        // 辅助方法：分解参数名前缀和数字部分
        private (string Prefix, int Number) GetSortKey(string paramName)
        {
            var match = System.Text.RegularExpressions.Regex.Match(paramName, @"^(.*?)_(\d+)$");
            if (match.Success && int.TryParse(match.Groups[2].Value, out int number))
            {
                return (match.Groups[1].Value, number);
            }
            // 如果没有数字后缀，返回原名称和0
            return (paramName, 0);
        }
    }

    public class ProductParameterInfo
    {
        public string? snNumber { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? lineCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? machineCode { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string? stationCode { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        public string? materialName { get; set; }

        /// <summary>
        /// 产品物料版本
        /// </summary>
        public string? materialVersion { get; set; }

        /// <summary>
        /// 生产参数记录时间
        /// </summary>
        public string? paramTime { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public BindingList<ProductParametersData>? data { get; set; }
    }
}
