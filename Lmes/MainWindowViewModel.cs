using System;
using System.Windows.Input;
using System.Windows.Media;
using PropertyChanged;

namespace Lmes
{
	[AddINotifyPropertyChangedInterface]
	public class MainWindowViewModel
	{
		#region 顶部状态栏属性
		public string 产品名称 { get; set; } = "IGBT模组测试产品";
		public string 工站名称 { get; set; } = "OP030-IGBT模组上线";
		public string 当前用户 { get; set; } = "操作员001";

		public Brush 安全门状态颜色 { get; set; } = Brushes.Green;
		public Brush 气阀状态颜色 { get; set; } = Brushes.Green;
		public Brush PLC状态颜色 { get; set; } = Brushes.Green;
		public Brush 紧急停止状态颜色 { get; set; } = Brushes.Gray;
		#endregion

		#region 工位信息属性
		public string 产品条码 { get; set; } = "P202501240001";
		public string 托盘条码 { get; set; } = "T202501240001";
		public string NG产品条码 { get; set; } = "";
		public string NG托盘条码 { get; set; } = "";
		#endregion

		#region 生产数据属性
		public double 当前节拍 { get; set; } = 45.2;
		public int 合格数量 { get; set; } = 1250;
		public int 不合格数量 { get; set; } = 15;
		public double 合格率 => 合格数量 + 不合格数量 > 0 ? (double)合格数量 / (合格数量 + 不合格数量) : 0;
		public int 总数量 => 合格数量 + 不合格数量;
		#endregion

		#region 设备状态属性
		public Brush 自动状态颜色 { get; set; } = Brushes.Green;
		public Brush 手动状态颜色 { get; set; } = Brushes.Gray;
		public Brush 运行状态颜色 { get; set; } = Brushes.Green;
		public Brush 初始化完成状态颜色 { get; set; } = Brushes.Green;
		public Brush 报警状态颜色 { get; set; } = Brushes.Gray;
		#endregion

		#region 状态显示属性
		public Brush OK状态颜色 { get; set; } = Brushes.Green;
		public Brush NG状态颜色 { get; set; } = Brushes.Gray;
		public Brush MESOK状态颜色 { get; set; } = Brushes.Green;
		public Brush MESNG状态颜色 { get; set; } = Brushes.Gray;
		#endregion

		#region 控制属性
		public bool 手动模式开关 { get; set; } = false;
		#endregion

		#region 命令属性
		public ICommand 产量清零命令 { get; set; }
		public ICommand 按钮1命令 { get; set; }
		public ICommand 按钮2命令 { get; set; }
		public ICommand 按钮3命令 { get; set; }
		public ICommand 按钮4命令 { get; set; }
		public ICommand 按钮5命令 { get; set; }
		public ICommand 按钮6命令 { get; set; }

		public ICommand 启动命令 { get; set; }
		public ICommand 暂停命令 { get; set; }
		public ICommand 复位命令 { get; set; }
		public ICommand 初始化命令 { get; set; }

		public ICommand 菜单命令 { get; set; }
		public ICommand 主页命令 { get; set; }
		public ICommand 手动命令 { get; set; }
		public ICommand IO监控命令 { get; set; }
		public ICommand 报警记录命令 { get; set; }
		public ICommand 统计命令 { get; set; }
		#endregion

		public MainWindowViewModel()
		{
			初始化所有命令(); // 重命名方法
		}

		private void 初始化所有命令() // 方法重命名
		{
			产量清零命令 = new RelayCommand(执行产量清零);
			按钮1命令 = new RelayCommand(() => 执行预留按钮("按钮1"));
			按钮2命令 = new RelayCommand(() => 执行预留按钮("按钮2"));
			按钮3命令 = new RelayCommand(() => 执行预留按钮("按钮3"));
			按钮4命令 = new RelayCommand(() => 执行预留按钮("按钮4"));
			按钮5命令 = new RelayCommand(() => 执行预留按钮("按钮5"));
			按钮6命令 = new RelayCommand(() => 执行预留按钮("按钮6"));

			启动命令 = new RelayCommand(执行启动);
			暂停命令 = new RelayCommand(执行暂停);
			复位命令 = new RelayCommand(执行复位);
			初始化命令 = new RelayCommand(执行初始化); // 这里是属性赋值

			菜单命令 = new RelayCommand(() => 执行导航("菜单"));
			主页命令 = new RelayCommand(() => 执行导航("主页"));
			手动命令 = new RelayCommand(() => 执行导航("手动"));
			IO监控命令 = new RelayCommand(() => 执行导航("IO监控"));
			报警记录命令 = new RelayCommand(() => 执行导航("报警记录"));
			统计命令 = new RelayCommand(() => 执行导航("统计"));
		}

		#region 命令实现方法
		private void 执行产量清零()
		{
			合格数量 = 0;
			不合格数量 = 0;
			// 这里可以添加实际的清零逻辑
		}

		private void 执行预留按钮(string 按钮名称)
		{
			// 这里实现预留按钮的具体功能
			System.Windows.MessageBox.Show($"点击了{按钮名称}");
		}

		private void 执行启动()
		{
			运行状态颜色 = Brushes.Green;
			// 这里实现启动逻辑
		}

		private void 执行暂停()
		{
			运行状态颜色 = Brushes.Orange;
			// 这里实现暂停逻辑
		}

		private void 执行复位()
		{
			运行状态颜色 = Brushes.Gray;
			// 这里实现复位逻辑
		}

		private void 执行初始化()
		{
			初始化完成状态颜色 = Brushes.Orange;
			// 这里实现初始化逻辑
			System.Threading.Tasks.Task.Run(async () =>
			{
				await System.Threading.Tasks.Task.Delay(3000);
				初始化完成状态颜色 = Brushes.Green;
			});
		}

		private void 执行导航(string 页面名称)
		{
			// 这里实现页面导航逻辑
			System.Windows.MessageBox.Show($"导航到{页面名称}页面");
		}
		#endregion

		#region 公共方法 - 用于外部更新状态
		public void 更新设备状态(bool 安全门, bool 气阀, bool PLC连接, bool 紧急停止)
		{
			安全门状态颜色 = 安全门 ? Brushes.Green : Brushes.Red;
			气阀状态颜色 = 气阀 ? Brushes.Green : Brushes.Red;
			PLC状态颜色 = PLC连接 ? Brushes.Green : Brushes.Red;
			紧急停止状态颜色 = 紧急停止 ? Brushes.Red : Brushes.Gray;
		}

		public void 更新生产数据(int 新合格数量, int 新不合格数量, double 新节拍)
		{
			合格数量 = 新合格数量;
			不合格数量 = 新不合格数量;
			当前节拍 = 新节拍;
		}

		public void 更新条码信息(string 新产品条码, string 新托盘条码)
		{
			产品条码 = 新产品条码;
			托盘条码 = 新托盘条码;
		}

		public void 更新NG信息(string NG产品, string NG托盘)
		{
			NG产品条码 = NG产品;
			NG托盘条码 = NG托盘;
		}

		public void 更新状态显示(bool OK, bool NG, bool MESOK, bool MESNG)
		{
			OK状态颜色 = OK ? Brushes.Green : Brushes.Gray;
			NG状态颜色 = NG ? Brushes.Red : Brushes.Gray;
			MESOK状态颜色 = MESOK ? Brushes.Green : Brushes.Gray;
			MESNG状态颜色 = MESNG ? Brushes.Red : Brushes.Gray;
		}
		#endregion
	}

	// 简单的RelayCommand实现
	public class RelayCommand : ICommand
	{
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		public RelayCommand(Action execute, Func<bool> canExecute = null)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute?.Invoke() ?? true;
		}

		public void Execute(object parameter)
		{
			_execute();
		}
	}
}