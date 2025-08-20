using Lmes.Models;
using Lmes.全局变量;
using Lmes.功能;
using Lmes.功能.数据类型请求体;
using Lmes.工具;
using Lmes.站逻辑.数据类型;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using Path = System.IO.Path;

namespace Lmes
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class 登录界面 : MetroWindow
	{

		// 预设的固定用户名和密码
		private const string 固定用户名 = "admin";
		private const string 固定密码 = "123456";

		public string 用户名 { get; set; } = "admin";

		public 登录界面()
		{
			InitializeComponent();
			DataContext = this;
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			// 获取密码框中输入的密码
			var 输入密码 = 密码框.Password;

			// 验证用户名和密码是否与预设的固定值匹配
			if (用户名 == 固定用户名 && 输入密码 == 固定密码)
			{
				// 创建一个模拟的令牌，用于保持与原有系统的兼容性
				//系统参数.ApiKey = "本地模拟Token_" + Guid.NewGuid().ToString();

				// 登录成功，打开主界面
				new 主界面().Show();

				// 关闭当前登录窗口
				Close();
			}
			else
			{
				// 显示登录失败的消息
				await this.ShowMessageAsync("错误", "用户名或密码不正确!");
				return;
			}
		}
	}
}