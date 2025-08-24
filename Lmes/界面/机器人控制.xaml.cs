//// MainWindow.xaml.cs
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Threading;
//using CRP;
//using MahApps.Metro.Controls;
//using PropertyChanged;

//namespace Lmes.界面
//{
//	[AddINotifyPropertyChangedInterface]
//	public partial class 机器人控制 : MetroWindow
//	{
//		private CRobotService robot_service;
//		private CIOService io_service;
//		private CFileService file_service;
//		private DispatcherTimer statusUpdateTimer;
//		private bool isConnected = false;

//		#region Properties for Data Binding
//		public string RobotIP { get; set; } = "127.0.0.1";
//		public string ConnectionStatus { get; set; } = "未连接";
//		public Brush ConnectionStatusBrush { get; set; } = Brushes.Red;
//		public string ServoStatus { get; set; } = "未知";
//		public string ProgramName { get; set; } = "robot_test.pro";
//		public string ProgramStatus { get; set; } = "停止";
//		public int ProgramLine { get; set; } = 0;
//		public int GRIndex { get; set; } = 0;
//		public double GRValue { get; set; } = 0.0;
//		public int GIIndex { get; set; } = 0;
//		public int GIValue { get; set; } = 0;
//		public int SpeedRatio { get; set; } = 50;
//		public string WorkMode { get; set; } = "未知";
//		public bool HasError { get; set; } = false;
//		public bool IsMoving { get; set; } = false;
//		public bool IsConnectedProperty { get; set; } = false;

//		public ObservableCollection<PositionItem> JointPositions { get; set; }
//		public ObservableCollection<PositionItem> CartesianPositions { get; set; }
//		public ObservableCollection<IOItem> IOStatus { get; set; }
//		public ObservableCollection<VariableItem> GRVariables { get; set; }
//		public ObservableCollection<VariableItem> GIVariables { get; set; }
//		public ObservableCollection<string> LogMessages { get; set; }
//		public ObservableCollection<ErrorItem> ErrorList { get; set; }

//		#endregion

//		public 机器人控制()
//		{
//			InitializeComponent();
//			InitializeData();
//			InitializeRobotService();
//			SetupStatusTimer();
//			DataContext = this;
//		}

//		private void InitializeData()
//		{
//			JointPositions = new ObservableCollection<PositionItem>();
//			CartesianPositions = new ObservableCollection<PositionItem>();
//			IOStatus = new ObservableCollection<IOItem>();
//			GRVariables = new ObservableCollection<VariableItem>();
//			GIVariables = new ObservableCollection<VariableItem>();
//			LogMessages = new ObservableCollection<string>();
//			ErrorList = new ObservableCollection<ErrorItem>();

//			// Initialize joint positions
//			for (int i = 0; i < 6; i++)
//			{
//				JointPositions.Add(new PositionItem { Name = $"J{i + 1}", Value = 0.0, Unit = "度" });
//			}

//			// Initialize cartesian positions
//			string[] axes = { "X", "Y", "Z", "Rx", "Ry", "Rz" };
//			string[] units = { "mm", "mm", "mm", "度", "度", "度" };
//			for (int i = 0; i < axes.Length; i++)
//			{
//				CartesianPositions.Add(new PositionItem { Name = axes[i], Value = 0.0, Unit = units[i] });
//			}

//			// Initialize IO status
//			for (int i = 0; i < 16; i++)
//			{
//				IOStatus.Add(new IOItem { Name = $"X{i}", Value = false, Type = "输入" });
//				IOStatus.Add(new IOItem { Name = $"Y{i}", Value = false, Type = "输出" });
//			}

//			// Initialize some GR/GI variables for display
//			for (int i = 0; i < 10; i++)
//			{
//				GRVariables.Add(new VariableItem { Index = i, Value = $"0.0", Type = "GR" });
//				GIVariables.Add(new VariableItem { Index = i, Value = "0", Type = "GI" });
//			}
//		}

//		private void InitializeRobotService()
//		{
//			try
//			{
//				robot_service = new CRobotService();
//				if (robot_service.IsAvailable())
//				{
//					io_service = new CIOService(ref robot_service);
//					file_service = new CFileService(ref robot_service);
//					AddLogMessage("机器人服务初始化成功");
//				}
//				else
//				{
//					AddLogMessage("机器人服务初始化失败 - 检查环境部署");
//					CheckEnvironmentDeployment();
//				}
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"初始化异常: {ex.Message}");
//			}
//		}

//		private void SetupStatusTimer()
//		{
//			statusUpdateTimer = new DispatcherTimer
//			{
//				Interval = TimeSpan.FromMilliseconds(500)
//			};
//			statusUpdateTimer.Tick += StatusUpdateTimer_Tick;
//		}

//		private async void StatusUpdateTimer_Tick(object sender, EventArgs e)
//		{
//			if (!isConnected) return;

//			try
//			{
//				await UpdateRobotStatus();
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"状态更新异常: {ex.Message}");
//			}
//		}

//		private async Task UpdateRobotStatus()
//		{
//			await Task.Run(() =>
//			{
//				try
//				{
//					// Update connection status
//					bool connected = robot_service.IsConnected();
//					Dispatcher.Invoke(() =>
//					{
//						IsConnectedProperty = connected;
//						if (!connected && isConnected)
//						{
//							// Connection lost
//							isConnected = false;
//							ConnectionStatus = "连接丢失";
//							ConnectionStatusBrush = Brushes.Orange;
//							AddLogMessage("机器人连接丢失");
//						}
//					});

//					if (!connected) return;

//					// Update servo status
//					bool isServoOn = robot_service.IsServoOn();
//					Dispatcher.Invoke(() => ServoStatus = isServoOn ? "上电" : "下电");

//					// Update work mode
//					int mode = robot_service.GetWorkMode();
//					string modeText = mode switch
//					{
//						(int)ERobotMode.RM_Manual => "手动模式",
//						(int)ERobotMode.RM_Playing => "自动模式",
//						(int)ERobotMode.RM_Remote => "远程模式",
//						_ => "未知模式"
//					};
//					Dispatcher.Invoke(() => WorkMode = modeText);

//					// Update error status
//					bool hasError = robot_service.HasError();
//					Dispatcher.Invoke(() => HasError = hasError);

//					// Update moving status
//					bool isMoving = false;
//					robot_service.IsMoving(ref isMoving);
//					Dispatcher.Invoke(() => IsMoving = isMoving);

//					// Update speed ratio
//					int speed = robot_service.GetSpeedRatio();
//					Dispatcher.Invoke(() => SpeedRatio = speed);

//					// Update positions
//					UpdatePositions();

//					// Update program status
//					UpdateProgramStatus();

//					// Update IO status
//					UpdateIOStatus();

//					// Update errors
//					UpdateErrors();

//					// Update some variables
//					UpdateVariables();
//				}
//				catch (Exception ex)
//				{
//					Dispatcher.Invoke(() => AddLogMessage($"状态更新错误: {ex.Message}"));
//				}
//			});
//		}

//		private void UpdatePositions()
//		{
//			try
//			{
//				// Update joint positions
//				var jointPos = new List<double>();
//				if (robot_service.GetCurrentPosition(0, ref jointPos))
//				{
//					Dispatcher.Invoke(() =>
//					{
//						for (int i = 0; i < Math.Min(jointPos.Count, JointPositions.Count); i++)
//						{
//							JointPositions[i].Value = Math.Round(jointPos[i], 3);
//						}
//					});
//				}

//				// Update cartesian positions
//				var cartPos = new List<double>();
//				if (robot_service.GetCurrentPosition(2, ref cartPos))
//				{
//					Dispatcher.Invoke(() =>
//					{
//						for (int i = 0; i < Math.Min(cartPos.Count, CartesianPositions.Count); i++)
//						{
//							CartesianPositions[i].Value = Math.Round(cartPos[i], 3);
//						}
//					});
//				}
//			}
//			catch (Exception ex)
//			{
//				Dispatcher.Invoke(() => AddLogMessage($"位置更新错误: {ex.Message}"));
//			}
//		}

//		private void UpdateProgramStatus()
//		{
//			try
//			{
//				int status = robot_service.GetProgramStatus();
//				string statusText = status switch
//				{
//					(int)EProgramStatus.PS_Stop => "停止",
//					(int)EProgramStatus.PS_Running => "运行",
//					(int)EProgramStatus.PS_Pause => "暂停",
//					_ => "未知"
//				};

//				int line = 0;
//				robot_service.GetProgramLine(ref line);

//				Dispatcher.Invoke(() =>
//				{
//					ProgramStatus = statusText;
//					ProgramLine = line;
//				});
//			}
//			catch (Exception ex)
//			{
//				Dispatcher.Invoke(() => AddLogMessage($"程序状态更新错误: {ex.Message}"));
//			}
//		}

//		private void UpdateIOStatus()
//		{
//			try
//			{
//				Dispatcher.Invoke(() =>
//				{
//					for (int i = 0; i < Math.Min(16, IOStatus.Count / 2); i++)
//					{
//						// Update X (input)
//						var xItem = IOStatus.FirstOrDefault(io => io.Name == $"X{i}" && io.Type == "输入");
//						if (xItem != null)
//						{
//							bool value = false;
//							if (io_service.GetX((uint)i, ref value))
//							{
//								xItem.Value = value;
//							}
//						}

//						// Update Y (output)
//						var yItem = IOStatus.FirstOrDefault(io => io.Name == $"Y{i}" && io.Type == "输出");
//						if (yItem != null)
//						{
//							bool value = false;
//							if (io_service.GetY((uint)i, ref value))
//							{
//								yItem.Value = value;
//							}
//						}
//					}
//				});
//			}
//			catch (Exception ex)
//			{
//				Dispatcher.Invoke(() => AddLogMessage($"IO状态更新错误: {ex.Message}"));
//			}
//		}

//		private void UpdateErrors()
//		{
//			try
//			{
//				var errors = new List<SErrorMessage>();
//				if (robot_service.GetErrors(ref errors))
//				{
//					Dispatcher.Invoke(() =>
//					{
//						ErrorList.Clear();
//						foreach (var error in errors)
//						{
//							ErrorList.Add(new ErrorItem
//							{
//								Id = $"0x{error.Id:X8}",
//								Message = error.Message?.ToString() ?? "未知错误",
//								Timestamp = DateTime.Now
//							});
//						}
//					});
//				}
//			}
//			catch (Exception ex)
//			{
//				Dispatcher.Invoke(() => AddLogMessage($"错误更新异常: {ex.Message}"));
//			}
//		}

//		private void UpdateVariables()
//		{
//			try
//			{
//				// Update first few GR variables
//				var grData = new List<double>();
//				if (robot_service.GetGR(0, ref grData, 10))
//				{
//					Dispatcher.Invoke(() =>
//					{
//						for (int i = 0; i < Math.Min(grData.Count, GRVariables.Count); i++)
//						{
//							GRVariables[i].Value = grData[i].ToString("F3");
//						}
//					});
//				}

//				// Update first few GI variables
//				var giData = new List<int>();
//				if (robot_service.GetGI(0, ref giData, 10))
//				{
//					Dispatcher.Invoke(() =>
//					{
//						for (int i = 0; i < Math.Min(giData.Count, GIVariables.Count); i++)
//						{
//							GIVariables[i].Value = giData[i].ToString();
//						}
//					});
//				}
//			}
//			catch (Exception ex)
//			{
//				Dispatcher.Invoke(() => AddLogMessage($"变量更新错误: {ex.Message}"));
//			}
//		}

//		#region Button Event Handlers

//		private async void BtnConnect_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				bool result = await Task.Run(() => robot_service.Connect(RobotIP));

//				if (result)
//				{
//					isConnected = true;
//					IsConnectedProperty = true;
//					ConnectionStatus = "已连接";
//					ConnectionStatusBrush = Brushes.Green;
//					statusUpdateTimer.Start();
//					AddLogMessage($"成功连接到机器人: {RobotIP}");
//				}
//				else
//				{
//					ConnectionStatus = "连接失败";
//					ConnectionStatusBrush = Brushes.Red;
//					AddLogMessage($"连接机器人失败: {RobotIP}");
//				}
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"连接异常: {ex.Message}");
//			}
//		}

//		private async void BtnDisconnect_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				statusUpdateTimer.Stop();
//				bool result = await Task.Run(() => robot_service.Disconnect());

//				isConnected = false;
//				IsConnectedProperty = false;
//				ConnectionStatus = result ? "已断开" : "断开失败";
//				ConnectionStatusBrush = Brushes.Red;
//				AddLogMessage(result ? "机器人连接已断开" : "断开连接失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"断开连接异常: {ex.Message}");
//			}
//		}

//		private async void BtnServoOn_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				bool result = await Task.Run(() => robot_service.ServoPowerOn());
//				AddLogMessage(result ? "伺服上电成功" : "伺服上电失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"伺服上电异常: {ex.Message}");
//			}
//		}

//		private async void BtnServoOff_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				bool result = await Task.Run(() => robot_service.ServoPowerOff());
//				AddLogMessage(result ? "伺服下电成功" : "伺服下电失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"伺服下电异常: {ex.Message}");
//			}
//		}

//		private async void BtnClearError_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				await Task.Run(() =>
//				{
//					if (robot_service.HasError())
//					{
//						robot_service.ClearError();
//					}
//				});
//				AddLogMessage("错误已清除");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"清除错误异常: {ex.Message}");
//			}
//		}

//		private async void BtnSetSpeedRatio_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				bool result = await Task.Run(() => robot_service.SetSpeedRatio(SpeedRatio));
//				AddLogMessage(result ? $"设置速度比例 {SpeedRatio}% 成功" : "设置速度比例失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"设置速度比例异常: {ex.Message}");
//			}
//		}

//		private async void BtnUploadProgram_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				string localPath = $"./program/{ProgramName}";
//				bool result = await Task.Run(() => file_service.Upload(localPath, ProgramName));
//				AddLogMessage(result ? $"程序 {ProgramName} 上传成功" : $"程序 {ProgramName} 上传失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"上传程序异常: {ex.Message}");
//			}
//		}

//		private async void BtnStartProgram_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				bool result = await Task.Run(() =>
//				{
//					// Set work mode to playing
//					robot_service.SetWorkMode((int)ERobotMode.RM_Playing);
//					return robot_service.StartProgram(ProgramName, 0);
//				});
//				AddLogMessage(result ? $"程序 {ProgramName} 启动成功" : $"程序 {ProgramName} 启动失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"启动程序异常: {ex.Message}");
//			}
//		}

//		private async void BtnStopProgram_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				bool result = await Task.Run(() => robot_service.StopProgram());
//				AddLogMessage(result ? "程序停止成功" : "程序停止失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"停止程序异常: {ex.Message}");
//			}
//		}

//		private async void BtnPauseProgram_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				bool result = await Task.Run(() => robot_service.PauseProgram());
//				AddLogMessage(result ? "程序暂停成功" : "程序暂停失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"暂停程序异常: {ex.Message}");
//			}
//		}

//		private async void BtnSetGR_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				var data = new List<double> { GRValue };
//				bool result = await Task.Run(() => robot_service.SetGR(GRIndex, data));
//				AddLogMessage(result ? $"设置 GR[{GRIndex}] = {GRValue} 成功" : $"设置 GR[{GRIndex}] 失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"设置GR异常: {ex.Message}");
//			}
//		}

//		private async void BtnGetGR_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				var data = new List<double>();
//				bool result = await Task.Run(() => robot_service.GetGR(GRIndex, ref data, 1));
//				if (result && data.Count > 0)
//				{
//					GRValue = data[0];
//					AddLogMessage($"获取 GR[{GRIndex}] = {data[0]} 成功");
//				}
//				else
//				{
//					AddLogMessage($"获取 GR[{GRIndex}] 失败");
//				}
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"获取GR异常: {ex.Message}");
//			}
//		}

//		private async void BtnSetGI_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				var data = new List<int> { GIValue };
//				bool result = await Task.Run(() => robot_service.SetGI(GIIndex, data));
//				AddLogMessage(result ? $"设置 GI[{GIIndex}] = {GIValue} 成功" : $"设置 GI[{GIIndex}] 失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"设置GI异常: {ex.Message}");
//			}
//		}

//		private async void BtnGetGI_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				var data = new List<int>();
//				bool result = await Task.Run(() => robot_service.GetGI(GIIndex, ref data, 1));
//				if (result && data.Count > 0)
//				{
//					GIValue = data[0];
//					AddLogMessage($"获取 GI[{GIIndex}] = {data[0]} 成功");
//				}
//				else
//				{
//					AddLogMessage($"获取 GI[{GIIndex}] 失败");
//				}
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"获取GI异常: {ex.Message}");
//			}
//		}

//		private async void BtnEmergencyStop_Click(object sender, RoutedEventArgs e)
//		{
//			try
//			{
//				bool result = await Task.Run(() => robot_service.EmergencyStop());
//				AddLogMessage(result ? "急停执行成功" : "急停执行失败");
//			}
//			catch (Exception ex)
//			{
//				AddLogMessage($"急停异常: {ex.Message}");
//			}
//		}

//		#endregion

//		private void AddLogMessage(string message)
//		{
//			Dispatcher.Invoke(() =>
//			{
//				string timestampedMessage = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
//				LogMessages.Insert(0, timestampedMessage);

//				// Keep only last 200 messages
//				while (LogMessages.Count > 200)
//				{
//					LogMessages.RemoveAt(LogMessages.Count - 1);
//				}
//			});
//		}

//		private void CheckEnvironmentDeployment()
//		{
//			string license_path = System.IO.Path.Combine(Environment.CurrentDirectory, "license.key");
//			string dll_path = System.IO.Path.Combine(Environment.CurrentDirectory, "RobotService.dll");
//			string dotnet_dll_path = System.IO.Path.Combine(Environment.CurrentDirectory, "RobotService-dotnet.dll");

//			if (!System.IO.File.Exists(license_path))
//			{
//				AddLogMessage($"许可证文件不存在: {license_path}");
//			}
//			else if (!System.IO.File.Exists(dll_path))
//			{
//				AddLogMessage($"DLL文件未找到: {dll_path}");
//			}
//			else if (!System.IO.File.Exists(dotnet_dll_path))
//			{
//				AddLogMessage($".NET DLL文件未找到: {dotnet_dll_path}");
//			}
//			else
//			{
//				AddLogMessage("许可证文件可能有误");
//			}
//		}

//		protected override void OnClosing(CancelEventArgs e)
//		{
//			statusUpdateTimer?.Stop();

//			if (isConnected)
//			{
//				try
//				{
//					if (robot_service.GetProgramStatus() == (int)EProgramStatus.PS_Running)
//					{
//						robot_service.StopProgram();
//					}
//					robot_service.Disconnect();
//				}
//				catch (Exception ex)
//				{
//					AddLogMessage($"关闭时清理异常: {ex.Message}");
//				}
//			}

//			base.OnClosing(e);
//		}
//	}

//	#region Data Models
//	[AddINotifyPropertyChangedInterface]
//	public class PositionItem
//	{
//		public string Name { get; set; }
//		public double Value { get; set; }
//		public string Unit { get; set; }
//	}

//	[AddINotifyPropertyChangedInterface]
//	public class IOItem
//	{
//		public string Name { get; set; }
//		public bool Value { get; set; }
//		public string Type { get; set; }
//	}

//	[AddINotifyPropertyChangedInterface]
//	public class VariableItem
//	{
//		public int Index { get; set; }
//		public string Value { get; set; }
//		public string Type { get; set; }
//	}

//	[AddINotifyPropertyChangedInterface]
//	public class ErrorItem
//	{
//		public string Id { get; set; }
//		public string Message { get; set; }
//		public DateTime Timestamp { get; set; }
//	}
//	#endregion
//}