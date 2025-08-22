using MahApps.Metro.Controls;
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

namespace Lmes.界面
{
	/// <summary>
	/// 机器人控制.xaml 的交互逻辑
	/// </summary>
	public partial class 机器人控制 : MetroWindow
	{
		public 机器人控制()
		{
			InitializeComponent();
			DataContext = this;
		}
	}
}
