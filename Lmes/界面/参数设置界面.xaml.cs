using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using EFDbContext;
using EFDbContext.Entity;
using Lmes.Models;
using Lmes.全局变量;
using Lmes.功能;
using Lmes.功能.数据类型请求体;
using Lmes.功能.枚举;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
//using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Lmes
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class 参数设置界面 : MetroWindow
    {
        public List<工站选择类> 运行工站列表 { get; set; }
        public static Action<string, 工单排程信息查询接口返回体object> 排程变更事件;
        private bool 排程变更 { get; set; } = false;
        public List<string> 排程列表
        {
            get
            {
                var tmp = 系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).排程列表;
                if (tmp == null)
                {
                    return new List<string>();
                }
                return tmp;
            }
            set
            {

            }
        }
        public string 当前选择排程
        {
            get
            {
                return 系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).排程编码;
            }
            set
            {
                if (系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).排程编码 != value.Split("  ")[0])
                {
                    系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).排程编码 = value.Split("  ")[0];
                    排程变更 = true;
                }
            }
        }
        public string 排程计划数量
        {
            get
            {
                return 系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).产品计划数量;
            }
            set
            {
                if (系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).产品计划数量 != value)
                {
                    系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).产品计划数量 = value;
                    排程变更 = true;
                }
            }
        }
        public string 当前产线编号 { get; set; } = 系统参数.产线信息.FirstOrDefault(x => !string.IsNullOrEmpty(x.产线编号)).产线编号;

        public Visibility 排程下拉框_可见 { get; set; } = Visibility.Visible;
        public Visibility 手动下发排程_可见 { get; set; } = Visibility.Collapsed;
        private bool _手动下发排程使能 { get; set; }
        public bool 手动下发排程使能 
        { 
            get
            {
                return _手动下发排程使能;
            }
            set
            {
                _手动下发排程使能 = value;
                if (_手动下发排程使能)
                {
                    排程下拉框_可见 = Visibility.Collapsed;
                    手动下发排程_可见 = Visibility.Visible;
                }
                else
                {
                    手动下发排程_可见 = Visibility.Collapsed;
                    排程下拉框_可见 = Visibility.Visible;
                }
            }
        }
        private 产线信息类 _当前产线信息;
        private MesApi mesApi;
        private 工单排程信息查询接口返回体? 排程信息列表;

        public 参数设置界面()
        {
            InitializeComponent();
            DataContext = this;

            // 初始化默认产线
            if (系统参数.产线信息.Count == 0)
            {
                系统参数.产线信息.Add(new 产线信息类());
            }

            LoadLineSetting();
            LoadCurrentRunningStation(系统参数.产线信息.First());
        }

        private void LoadLineSetting()
        {
            cbLineInfo.ItemsSource = 系统参数.产线信息;
            cbLineInfo.SelectedIndex = 0;

            // 加载全局设置
            boxFMESAddress.Text = 系统参数.设置.Lmes连接参数.LMES地址;
            boxLMESAddress.Text = 系统参数.设置.Lmes连接参数.产线MES地址;
        }

        private void CbLineInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _当前产线信息 = cbLineInfo.SelectedItem as 产线信息类;
            if (_当前产线信息 != null)
            {
                // 加载产线信息
                boxFactoryCode.Text = _当前产线信息.工厂编号;
                boxLineCode.Text = _当前产线信息.产线编号;

                // 加载该产线的工站配置
                LoadCurrentRunningStation(_当前产线信息);
            }
        }

        private async void LoadCurrentRunningStation(产线信息类 产线信息)
        {
            try
            {
                运行工站列表 = new List<工站选择类>();
                foreach (var code in 系统参数.工站列表)
                {
                    运行工站列表.Add(new 工站选择类
                    {
                        stationCode = code,
                        isSelected = 产线信息.运行工站列表?.Contains(code) ?? false
                    });
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("错误", "加载工站列表发生错误\r\n" + ex.ToString());
            }
        }

        private async void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 保存全局设置
                系统参数.设置.Lmes连接参数.LMES地址 = boxFMESAddress.Text;
                系统参数.设置.Lmes连接参数.产线MES地址 = boxLMESAddress.Text;

                // 保存当前产线信息
                if (_当前产线信息 == null) return;

                _当前产线信息.工厂编号 = boxFactoryCode.Text;
                _当前产线信息.产线编号 = boxLineCode.Text;

                // 保存运行工站配置
                var selectedStations = 运行工站列表
                    .Where(x => x.isSelected)
                    .Select(x => x.stationCode)
                    .ToList();

                _当前产线信息.运行工站列表 = selectedStations;

                if (排程变更)
                {
                    排程变更 = false;
                    if (手动下发排程使能)
                    {
                        if (排程变更事件 != null)
                        {
                            排程变更事件.Invoke(当前产线编号, new() { scheduleCode = 当前选择排程, scheduleQty = 排程计划数量 });
                        }
                        return;
                    }
                    var 变更后排程编码 = 系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).排程编码.Split("  ")[0];
                    排程信息列表 = await mesApi.工单排程信息查询接口(new 工单排程信息查询接口请求体
                    {
                        factoryCode = 系统参数.设置.工厂编号,
                        scheduleCode = 变更后排程编码,
                        requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                    系统参数.产线信息.FirstOrDefault(x => x.产线编号 == 当前产线编号).产品名称 = 排程信息列表.data.FirstOrDefault(x => x.scheduleCode == 变更后排程编码).productName;
                    if (排程变更事件 != null)
                    {
                        排程变更事件.Invoke(当前产线编号, 排程信息列表.data.FirstOrDefault(x => x.scheduleCode == 变更后排程编码));
                    }
                }

                // 生成工艺路径
                //_当前产线信息.工艺路径 = new 工艺路径
                //{
                //    工序字典 = selectedStations.ToDictionary(
                //        s => s,
                //        s => "GX" + s.Substring(2)),
                //    工序路径 = selectedStations.Select(s => "GX" + s.Substring(2)).ToList()
                //};

                // 序列化保存
                File.WriteAllText(
                    Path.Combine(系统参数.配置文件路径, "配置文件.json"),
                    JsonSerializer.Serialize(系统参数.设置, 系统参数.整理格式)
                );
                File.WriteAllText(
                    Path.Combine(系统参数.配置文件路径, "产线信息.json"),
                    JsonSerializer.Serialize(系统参数.产线信息, 系统参数.整理格式)
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置时出错：{ex.Message}");
                return;
            }
            //this.Close();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mesApi = new(系统参数.设置.Lmes连接参数.LMES地址 + @"api/", 系统参数.设置.Lmes连接参数.appId, 系统参数.设置.Lmes连接参数.appKey);
            await 获取排程信息();
        }

        private async Task 获取排程信息()
        {
            try
            {
                foreach (var tmp in 系统参数.产线信息)
                {
                    if (!string.IsNullOrEmpty(tmp.产线编号))
                    {
                        排程信息列表 = await mesApi.工单排程信息查询接口(new 工单排程信息查询接口请求体
                        {
                            factoryCode = tmp.工厂编号,
                            lineCode = tmp.产线编号,
                            requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            updateTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss")
                        });
                        if (排程信息列表.code != "000000")
                        {
                            日志写入.写入($"获取排程信息错误：{排程信息列表.mesg}");
                            手动下发排程使能 = true;
                            break;
                        }
                        手动下发排程使能 = false;
                        tmp.排程列表.Clear();
                        foreach (var 排程信息 in 排程信息列表.data)
                        {
                            tmp.排程列表.Add($"{排程信息.scheduleCode}  {排程信息.startTime}");
                            using (数据库连接 数据库 = new())
                            {
                                if(数据库.排程信息.FirstOrDefault(x=>x.scheduleCode == 排程信息.scheduleCode) == null)
                                {
                                    数据库.排程信息.Add(new()
                                    {
                                        Id = Guid.NewGuid(),
                                        scheduleCode = 排程信息.scheduleCode,
                                        scheduleQty = 排程信息.scheduleQty,
                                        startTime = 排程信息.startTime,
                                        endTime = 排程信息.endTime,
                                        scheduleStatusCode = 排程信息.scheduleStateCode
                                    });
                                    数据库.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                手动下发排程使能 = true;
            }
            系统参数.设置.工厂编号 = 系统参数.产线信息[0].工厂编号;
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class 工站选择类
    {
        public bool isSelected { get; set; }
        public string stationCode { get; set; }
    }
}