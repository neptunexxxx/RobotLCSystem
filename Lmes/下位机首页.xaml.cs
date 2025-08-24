using MahApps.Metro.Controls;
using PropertyChanged;
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

namespace Lmes
{
    /// <summary>
    /// 下位机首页.xaml 的交互逻辑
    /// </summary>
    public partial class 下位机首页 : MetroWindow
    {
		public MainWindowViewModel ViewModel { get; }

		public 下位机首页()
		{
			InitializeComponent();
			ViewModel = new MainWindowViewModel();
			DataContext = ViewModel;

			// 设置窗口属性
			this.WindowState = WindowState.Maximized;
			this.WindowStyle = WindowStyle.SingleBorderWindow;

			// 可选：设置全屏模式（如果需要的话）
			// this.WindowStyle = WindowStyle.None;
			// this.ResizeMode = ResizeMode.NoResize;

			// 初始化定时器或其他需要的组件
			初始化组件();
		}

		private void 初始化组件()
		{
			// 这里可以初始化定时器用于更新实时数据
			var timer = new System.Windows.Threading.DispatcherTimer();
			timer.Interval = System.TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, System.EventArgs e)
		{
			// 模拟实时数据更新
			// 在实际应用中，这里应该从PLC或其他数据源获取数据

			// 示例：随机更新节拍时间
			var random = new System.Random();
			if (random.Next(0, 10) == 0) // 10%概率更新
			{
				ViewModel.当前节拍 = 40 + random.NextDouble() * 20; // 40-60秒范围
			}

			// 示例：模拟生产计数增加
			if (random.Next(0, 30) == 0) // 约3.3%概率增加产量
			{
				if (random.Next(0, 10) == 0) // 10%概率是NG
				{
					ViewModel.不合格数量++;
					ViewModel.更新NG信息($"P{System.DateTime.Now:yyyyMMddHHmmss}", $"T{System.DateTime.Now:yyyyMMddHHmmss}");
				}
				else
				{
					ViewModel.合格数量++;
				}

				// 更新条码
				ViewModel.更新条码信息($"P{System.DateTime.Now:yyyyMMddHHmmss}", $"T{System.DateTime.Now:yyyyMMddHHmmss}");
			}
		}

		// 处理窗口关闭事件
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			var result = MessageBox.Show("确定要退出系统吗？", "确认退出", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No)
			{
				e.Cancel = true;
			}
			base.OnClosing(e);
		}

		// 处理键盘快捷键
		//protected override void OnKeyDown(KeyEventArgs e)
		//{
		//	switch (e.Key)
		//	{
		//		case Key.F1:
		//			ViewModel.启动命令?.Execute(null);
		//			break;
		//		case Key.F2:
		//			ViewModel.暂停命令?.Execute(null);
		//			break;
		//		case Key.F3:
		//			ViewModel.复位命令?.Execute(null);
		//			break;
		//		case Key.F4:
		//			ViewModel.初始化命令?.Execute(null);
		//			break;
		//		case Key.F5:
		//			ViewModel.手动模式开关 = !ViewModel.手动模式开关;
		//			break;
		//		case Key.Escape:
		//			// ESC键最小化窗口
		//			this.WindowState = WindowState.Minimized;
		//			break;
		//	}
		//	base.OnKeyDown(e);
		//}

		// 公共方法供外部调用，用于更新界面数据
		public void 更新设备连接状态(bool PLC连接状态)
		{
			ViewModel.更新设备状态(true, true, PLC连接状态, false);
		}

		public void 更新生产统计(int 合格, int 不合格, double 节拍)
		{
			ViewModel.更新生产数据(合格, 不合格, 节拍);
		}

		public void 显示报警信息(string 报警内容)
		{
			ViewModel.报警状态颜色 = System.Windows.Media.Brushes.Red;
			MessageBox.Show(报警内容, "系统报警", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		public void 清除报警状态()
		{
			ViewModel.报警状态颜色 = System.Windows.Media.Brushes.Gray;
		}
	}
}
