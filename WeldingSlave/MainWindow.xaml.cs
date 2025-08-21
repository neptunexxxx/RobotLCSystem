using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeldingSlave
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private WeldingSlaveService _slaveService;

		public MainWindow()
		{
			InitializeComponent();
			InitializeUI();
		}

		private void InitializeUI()
		{
			// 初始化子系统选择
			cmbSubSystem.Items.Add(new ComboBoxItem { Content = "机器人1", Tag = SubSystemCode.Robot1 });
			cmbSubSystem.Items.Add(new ComboBoxItem { Content = "机器人2", Tag = SubSystemCode.Robot2 });
			cmbSubSystem.Items.Add(new ComboBoxItem { Content = "机器人3", Tag = SubSystemCode.Robot3 });
			cmbSubSystem.Items.Add(new ComboBoxItem { Content = "工装", Tag = SubSystemCode.Fixture });
			cmbSubSystem.Items.Add(new ComboBoxItem { Content = "摄像头1", Tag = SubSystemCode.Camera1 });
			cmbSubSystem.Items.Add(new ComboBoxItem { Content = "摄像头2", Tag = SubSystemCode.Camera2 });
			cmbSubSystem.SelectedIndex = 0;

			txtHostIP.Text = "192.168.1.100";
			btnStart.IsEnabled = true;
			btnStop.IsEnabled = false;
		}

		/// <summary>
		/// 启动按钮点击事件处理
		/// </summary>
		private void BtnStart_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// 获取用户选择的子系统类型
				var selectedItem = (ComboBoxItem)cmbSubSystem.SelectedItem;
				var subSystem = (SubSystemCode)selectedItem.Tag;

				// 创建焊接下位机服务实例，传入上位机IP和子系统类型
				_slaveService = new WeldingSlaveService(txtHostIP.Text, subSystem);

				// 订阅服务的事件，用于UI更新
				_slaveService.MessageReceived += OnMessageReceived;  // 消息接收事件
				_slaveService.MessageSent += OnMessageSent;          // 消息发送事件
				_slaveService.StatusChanged += OnStatusChanged;      // 状态变化事件

				// 启动下位机服务
				_slaveService.Start();

				// 更新UI状态：禁用启动相关控件，启用停止按钮
				btnStart.IsEnabled = false;
				btnStop.IsEnabled = true;
				txtHostIP.IsEnabled = false;
				cmbSubSystem.IsEnabled = false;

				// 记录启动成功日志
				AddLog($"下位机启动成功 - 子系统: {selectedItem.Content}, 端口: {_slaveService.Port}");
			}
			catch (Exception ex)
			{
				// 启动失败时显示错误消息
				MessageBox.Show($"启动失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// 停止按钮点击事件处理
		/// </summary>
		private void BtnStop_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// 停止服务（?. 是空条件操作符，如果_slaveService为null则不执行）
				_slaveService?.Stop();
				_slaveService = null;  // 清空服务实例引用

				// 恢复UI状态：启用启动相关控件，禁用停止按钮
				btnStart.IsEnabled = true;
				btnStop.IsEnabled = false;
				txtHostIP.IsEnabled = true;
				cmbSubSystem.IsEnabled = true;

				// 记录停止日志
				AddLog("下位机已停止");
			}
			catch (Exception ex)
			{
				// 停止失败时显示错误消息
				MessageBox.Show($"停止失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// 消息接收事件处理（在后台线程中触发，需要切换到UI线程更新界面）
		/// </summary>
		private void OnMessageReceived(object sender, MessageEventArgs e)
		{
			// Dispatcher.Invoke确保在UI线程中执行，避免跨线程操作异常
			Dispatcher.Invoke(() =>
			{
				// 记录接收到的消息基本信息
				AddLog($"[接收] {e.ProtocolName} - 消息码:{e.MessageCode} - 来源:{e.RemoteEndPoint}");

				// 如果消息体不为空，则显示消息内容
				if (!string.IsNullOrEmpty(e.MessageBody))
				{
					AddLog($"    消息体: {e.MessageBody}");
				}
			});
		}

		/// <summary>
		/// 消息发送事件处理（在后台线程中触发，需要切换到UI线程更新界面）
		/// </summary>
		private void OnMessageSent(object sender, MessageEventArgs e)
		{
			// Dispatcher.Invoke确保在UI线程中执行
			Dispatcher.Invoke(() =>
			{
				// 记录发送的消息基本信息
				AddLog($"[发送] {e.ProtocolName} - 消息码:{e.MessageCode} - 目标:{e.RemoteEndPoint}");

				// 如果消息体不为空，则显示消息内容
				if (!string.IsNullOrEmpty(e.MessageBody))
				{
					AddLog($"    消息体: {e.MessageBody}");
				}
			});
		}

		/// <summary>
		/// 状态变化事件处理（在后台线程中触发）
		/// </summary>
		private void OnStatusChanged(object sender, string status)
		{
			// Dispatcher.Invoke确保在UI线程中更新状态标签
			Dispatcher.Invoke(() =>
			{
				lblStatus.Content = $"状态: {status}";
			});
		}

		/// <summary>
		/// 添加日志到文本框，带时间戳
		/// </summary>
		private void AddLog(string message)
		{
			// 获取当前时间的精确格式（小时:分钟:秒.毫秒）
			var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");

			// 在文本框末尾追加带时间戳的日志
			txtLog.AppendText($"[{timestamp}] {message}\r\n");

			// 自动滚动到最新日志
			txtLog.ScrollToEnd();
		}


		/// <summary>
		/// 窗口关闭事件重写，确保资源清理
		/// </summary>
		protected override void OnClosed(EventArgs e)
		{
			// 窗口关闭时停止服务，释放网络资源
			_slaveService?.Stop();
			base.OnClosed(e);  // 调用基类方法
		}
	}

	#region 数据结构和枚举定义

	public enum ProtocolType : byte
	{
		StatusFeedback = 1,      // 周期性状态反馈
		InitCmd = 20,            // 初始化命令
		InitAck = 21,            // 初始化确认
		InitComplete = 22,       // 初始化完成/失败
		InitCompleteAck = 23,    // 初始化完成确认
		VirtualWeldCmd = 30,     // 虚拟焊接命令
		VirtualWeldAck = 31,     // 虚拟焊接确认
		VirtualWeldComplete = 32, // 虚拟焊接完成
		VirtualWeldCompleteAck = 33, // 虚拟焊接完成确认
		WeldCmd = 40,            // 焊接命令
		WeldAck = 41,            // 焊接确认
		WeldComplete = 42,       // 焊接完成
		WeldCompleteAck = 43,    // 焊接完成确认
		ProcessAdjustCmd = 50,   // 焊接工艺调整命令
		ProcessAdjustAck = 51,   // 焊接工艺调整确认
		PostWeldCmd = 60,        // 焊后检测命令
		PostWeldAck = 61,        // 焊后检测确认
		PostWeldComplete = 62,   // 焊后检测完成
		PostWeldCompleteAck = 63 // 焊后检测完成确认
	}

	public enum SubSystemCode : byte
	{
		Host = 0,      // 上位机
		Robot1 = 1,    // 机器人1
		Robot2 = 2,    // 机器人2
		Robot3 = 3,    // 机器人3
		Fixture = 4,   // 工装
		Camera1 = 5,   // 摄像头1
		Camera2 = 6,   // 摄像头2
		Camera3 = 7    // 摄像头3
	}

	public enum SourceAddress : byte
	{
		Host = 0,        // 上位机
		PreWeld = 1,     // 焊前软件
		DuringWeld = 2,  // 焊中软件
		PostWeld = 3     // 焊后软件
	}

	/// <summary>
	/// 消息事件参数类，用于在事件中传递消息信息
	/// </summary>
	public class MessageEventArgs : EventArgs
	{
		public string ProtocolName { get; set; }    // 协议名称（中文描述）
		public byte MessageCode { get; set; }       // 消息码（用于消息匹配）
		public string MessageBody { get; set; }     // 消息体内容
		public EndPoint RemoteEndPoint { get; set; } // 远程终端点（IP和端口）
	}

	#endregion

	#region UDP消息类

	/// <summary>
	/// UDP消息封装类，负责消息的打包和解包
	/// </summary>
	public class UDPMessage
	{
		// 协议定义的固定数据头
		public const byte DataHeader = 0x55;      // 普通消息头
		public const byte DataHeaderAck = 0x5A;   // ACK消息头

		// 消息结构字段（严格按照协议文档定义）
		public byte Header { get; set; } = DataHeader;           // 数据头
		public ProtocolType ProtocolType { get; set; }           // 协议类型
		public SubSystemCode SubSystemCode { get; set; }         // 子系统代号
		public byte MessageCode { get; set; }                    // 消息码
		public SourceAddress SourceAddress { get; set; }         // 发送源地址
		public byte[] Reserved { get; set; } = new byte[3];      // 3字节预留
		public ushort DataLength { get; set; }                   // 数据长度
		public byte[] MessageBody { get; set; } = new byte[0];   // 消息体
		public ushort CRC { get; set; }                          // CRC校验码

		/// <summary>
		/// 将消息对象打包成字节数组，用于网络传输
		/// </summary>
		public byte[] Pack()
		{
			// 计算消息体长度
			var bodyLength = MessageBody?.Length ?? 0;

			// 计算总数据长度：头部10字节 + 消息体长度 + CRC2字节
			DataLength = (ushort)(10 + bodyLength + 2);

			// 构建消息字节流
			var buffer = new List<byte>
		{
			Header,                    // 数据头
            (byte)ProtocolType,        // 协议类型
            (byte)SubSystemCode,       // 子系统代号
            MessageCode,               // 消息码
            (byte)SourceAddress        // 发送源地址
        };

			// 添加3字节预留字段
			buffer.AddRange(Reserved);

			// 添加数据长度（小端序，低字节在前）
			buffer.AddRange(BitConverter.GetBytes(DataLength));

			// 添加消息体（如果存在）
			if (MessageBody != null && MessageBody.Length > 0)
			{
				buffer.AddRange(MessageBody);
			}

			// 计算CRC校验码（不包括CRC字段本身）
			var crcData = buffer.ToArray();
			CRC = CalculateCRC16(crcData);

			// 添加CRC校验码
			buffer.AddRange(BitConverter.GetBytes(CRC));

			return buffer.ToArray();
		}

		/// <summary>
		/// 从字节数组解包出消息对象
		/// </summary>
		public static UDPMessage Unpack(byte[] data)
		{
			// 最小长度检查：至少需要头部10字节 + CRC2字节
			if (data.Length < 12) return null;

			try
			{
				// 解析消息头字段
				var message = new UDPMessage
				{
					Header = data[0],                                    // 数据头
					ProtocolType = (ProtocolType)data[1],               // 协议类型
					SubSystemCode = (SubSystemCode)data[2],             // 子系统代号
					MessageCode = data[3],                              // 消息码
					SourceAddress = (SourceAddress)data[4],             // 发送源地址
					DataLength = BitConverter.ToUInt16(data, 8)         // 数据长度（从第8字节开始的2字节）
				};

				// 复制3字节预留字段
				Array.Copy(data, 5, message.Reserved, 0, 3);

				// 验证数据长度是否匹配
				if (data.Length != message.DataLength)
					return null;

				// 提取消息体（如果存在）
				var bodyLength = message.DataLength - 12;  // 总长度 - 头部10字节 - CRC2字节
				if (bodyLength > 0)
				{
					message.MessageBody = new byte[bodyLength];
					Array.Copy(data, 10, message.MessageBody, 0, bodyLength);  // 从第10字节开始复制
				}

				// 验证CRC校验码
				message.CRC = BitConverter.ToUInt16(data, data.Length - 2);    // 取最后2字节作为CRC
				var calculatedCRC = CalculateCRC16(data, data.Length - 2);     // 计算除CRC外的校验码

				// CRC校验失败
				if (message.CRC != calculatedCRC)
					return null;

				return message;
			}
			catch
			{
				// 解析过程中任何异常都返回null
				return null;
			}
		}

		/// <summary>
		/// 计算CRC-16校验码（使用Modbus标准）
		/// </summary>
		private static ushort CalculateCRC16(byte[] data, int length = -1)
		{
			// 如果未指定长度，则使用整个数组长度
			if (length == -1) length = data.Length;

			ushort crc = 0xFFFF;  // 初始值

			// 对每个字节进行CRC计算
			for (int i = 0; i < length; i++)
			{
				crc ^= data[i];   // 异或操作

				// 处理8位
				for (int j = 0; j < 8; j++)
				{
					if ((crc & 1) == 1)  // 如果最低位为1
						crc = (ushort)((crc >> 1) ^ 0xA001);  // 右移并异或多项式
					else
						crc >>= 1;  // 只右移
				}
			}
			return crc;
		}

		/// <summary>
		/// 获取协议类型的中文名称
		/// </summary>
		public string GetProtocolName()
		{
			// 协议类型到中文名称的映射
			var names = new Dictionary<ProtocolType, string>
		{
			{ ProtocolType.StatusFeedback, "状态反馈" },
			{ ProtocolType.InitCmd, "初始化命令" }, { ProtocolType.InitAck, "初始化确认" },
			{ ProtocolType.InitComplete, "初始化完成" }, { ProtocolType.InitCompleteAck, "初始化完成确认" },
			{ ProtocolType.VirtualWeldCmd, "虚拟焊接命令" }, { ProtocolType.VirtualWeldAck, "虚拟焊接确认" },
			{ ProtocolType.VirtualWeldComplete, "虚拟焊接完成" }, { ProtocolType.VirtualWeldCompleteAck, "虚拟焊接完成确认" },
			{ ProtocolType.WeldCmd, "焊接命令" }, { ProtocolType.WeldAck, "焊接确认" },
			{ ProtocolType.WeldComplete, "焊接完成" }, { ProtocolType.WeldCompleteAck, "焊接完成确认" },
			{ ProtocolType.ProcessAdjustCmd, "工艺调整命令" }, { ProtocolType.ProcessAdjustAck, "工艺调整确认" },
			{ ProtocolType.PostWeldCmd, "焊后检测命令" }, { ProtocolType.PostWeldAck, "焊后检测确认" },
			{ ProtocolType.PostWeldComplete, "焊后检测完成" }, { ProtocolType.PostWeldCompleteAck, "焊后检测完成确认" }
		};

			// 返回对应名称，如果找不到则返回"未知协议"
			return names.ContainsKey(ProtocolType) ? names[ProtocolType] : $"未知协议({(int)ProtocolType})";
		}
	}

	#endregion

	#region 焊接下位机服务类

	public class WeldingSlaveService
	{
		private UdpClient _udpClient;
		private IPEndPoint _hostEndPoint;
		private bool _isRunning;
		private byte _messageCounter = 0;
		private string _currentStatus = "初始化中";
		private Timer _statusTimer;

		public event EventHandler<MessageEventArgs> MessageReceived;
		public event EventHandler<MessageEventArgs> MessageSent;
		public event EventHandler<string> StatusChanged;

		public string HostIP { get; private set; }
		public SubSystemCode SubSystem { get; private set; }
		public int Port { get; private set; }

		public WeldingSlaveService(string hostIP, SubSystemCode subSystem)
		{
			HostIP = hostIP;
			SubSystem = subSystem;

			// 根据子系统选择端口
			Port = subSystem switch
			{
				SubSystemCode.Fixture => 5010,
				SubSystemCode.Robot1 or SubSystemCode.Robot2 or SubSystemCode.Robot3 => 5011,
				_ => 5012
			};
		}

		public void Start()
		{
			if (_isRunning) return;

			_udpClient = new UdpClient(Port);
			_hostEndPoint = new IPEndPoint(IPAddress.Parse(HostIP), Port);
			_isRunning = true;

			// 开始接收消息
			Task.Run(ReceiveLoop);

			// 开始状态反馈
			_statusTimer = new Timer(SendStatusFeedback, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));

			SetStatus("就绪");
		}

		public void Stop()
		{
			if (!_isRunning) return;

			_isRunning = false;
			_statusTimer?.Dispose();
			_udpClient?.Close();
			_udpClient?.Dispose();

			SetStatus("已停止");
		}

		private async Task ReceiveLoop()
		{
			while (_isRunning)
			{
				try
				{
					var result = await _udpClient.ReceiveAsync();
					var message = UDPMessage.Unpack(result.Buffer);

					if (message != null)
					{
						await HandleMessage(message, result.RemoteEndPoint);

						// 触发消息接收事件
						var bodyText = message.MessageBody?.Length > 0 ?
							Encoding.UTF8.GetString(message.MessageBody) : "";
						MessageReceived?.Invoke(this, new MessageEventArgs
						{
							ProtocolName = message.GetProtocolName(),
							MessageCode = message.MessageCode,
							MessageBody = bodyText,
							RemoteEndPoint = result.RemoteEndPoint
						});
					}
				}
				catch (ObjectDisposedException)
				{
					break; // UDP客户端已关闭
				}
				catch (Exception ex)
				{
					// 处理其他异常
					Console.WriteLine($"接收消息时出错: {ex.Message}");
				}
			}
		}

		/// <summary>
		/// 根据协议类型分发处理不同的消息
		/// </summary>
		private async Task HandleMessage(UDPMessage message, IPEndPoint remoteEndPoint)
		{
			// 使用switch语句根据协议类型调用相应的处理方法
			switch (message.ProtocolType)
			{
				case ProtocolType.InitCmd:              // 初始化命令
					await HandleInitCommand(message, remoteEndPoint);
					break;
				case ProtocolType.VirtualWeldCmd:       // 虚拟焊接命令
					await HandleVirtualWeldCommand(message, remoteEndPoint);
					break;
				case ProtocolType.WeldCmd:              // 焊接命令
					await HandleWeldCommand(message, remoteEndPoint);
					break;
				case ProtocolType.ProcessAdjustCmd:     // 工艺调整命令
					await HandleProcessAdjustCommand(message, remoteEndPoint);
					break;
				case ProtocolType.PostWeldCmd:          // 焊后检测命令
					await HandlePostWeldCommand(message, remoteEndPoint);
					break;
					// 其他协议类型暂不处理，可以在这里添加default分支记录日志
			}
		}

		private async Task HandleInitCommand(UDPMessage message, IPEndPoint remoteEndPoint)
		{
			// 发送ACK确认
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.InitAck,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.PreWeld
			}, remoteEndPoint);

			// 模拟初始化过程
			SetStatus("初始化中...");
			await Task.Delay(2000);

			// 发送初始化完成
			SetStatus("初始化完成");
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.InitComplete,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.PreWeld,
				MessageBody = Encoding.UTF8.GetBytes("0:success")
			}, remoteEndPoint);
		}

		private async Task HandleVirtualWeldCommand(UDPMessage message, IPEndPoint remoteEndPoint)
		{
			var bodyText = message.MessageBody?.Length > 0 ?
				Encoding.UTF8.GetString(message.MessageBody) : "";

			// 发送ACK确认
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.VirtualWeldAck,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.PreWeld
			}, remoteEndPoint);

			// 模拟虚拟焊接过程
			SetStatus("虚拟焊接中...");
			await Task.Delay(3000);

			// 发送虚拟焊接完成
			SetStatus("虚拟焊接完成");
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.VirtualWeldComplete,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.PreWeld,
				MessageBody = Encoding.UTF8.GetBytes("0:success:virtual_weld_result.ply")
			}, remoteEndPoint);
		}

		private async Task HandleWeldCommand(UDPMessage message, IPEndPoint remoteEndPoint)
		{
			var bodyText = message.MessageBody?.Length > 0 ?
				Encoding.UTF8.GetString(message.MessageBody) : "";

			// 发送ACK确认
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.WeldAck,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.DuringWeld
			}, remoteEndPoint);

			// 模拟焊接过程
			SetStatus("焊接中...");
			await Task.Delay(5000);

			// 发送焊接完成
			SetStatus("焊接完成");
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.WeldComplete,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.DuringWeld,
				MessageBody = Encoding.UTF8.GetBytes("0:success:weld_result.log")
			}, remoteEndPoint);
		}

		private async Task HandleProcessAdjustCommand(UDPMessage message, IPEndPoint remoteEndPoint)
		{
			var bodyText = message.MessageBody?.Length > 0 ?
				Encoding.UTF8.GetString(message.MessageBody) : "";

			// 发送ACK确认
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.ProcessAdjustAck,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.DuringWeld
			}, remoteEndPoint);

			SetStatus($"工艺已调整: {bodyText}");
		}

		private async Task HandlePostWeldCommand(UDPMessage message, IPEndPoint remoteEndPoint)
		{
			var bodyText = message.MessageBody?.Length > 0 ?
				Encoding.UTF8.GetString(message.MessageBody) : "";

			// 发送ACK确认
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.PostWeldAck,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.PostWeld
			}, remoteEndPoint);

			// 模拟焊后检测过程
			SetStatus("焊后检测中...");
			await Task.Delay(3000);

			// 发送检测完成
			SetStatus("焊后检测完成");
			await SendMessage(new UDPMessage
			{
				ProtocolType = ProtocolType.PostWeldComplete,
				SubSystemCode = SubSystem,
				MessageCode = message.MessageCode,
				SourceAddress = SourceAddress.PostWeld,
				MessageBody = Encoding.UTF8.GetBytes("0:success:post_weld_image.bmp:post_weld_cloud.ply")
			}, remoteEndPoint);
		}

		/// <summary>
		/// 定时发送状态反馈消息（由Timer定时器调用）
		/// </summary>
		private void SendStatusFeedback(object state)
		{
			// 检查服务是否还在运行
			if (!_isRunning) return;

			try
			{
				// 构造状态反馈消息
				var statusMessage = new UDPMessage
				{
					ProtocolType = ProtocolType.StatusFeedback,    // 状态反馈协议类型
					SubSystemCode = SubSystem,                     // 当前子系统代号
					MessageCode = GetNextMessageCode(),            // 获取下一个消息码
					SourceAddress = SourceAddress.PreWeld,         // 默认使用焊前软件地址
					MessageBody = Encoding.UTF8.GetBytes($"1:{_currentStatus}")  // 状态码:状态描述
				};

				// 发送状态消息到上位机
				SendMessage(statusMessage, _hostEndPoint);
			}
			catch (Exception ex)
			{
				// 发送状态反馈失败时输出错误信息
				Console.WriteLine($"发送状态反馈时出错: {ex.Message}");
			}
		}

		private async Task SendMessage(UDPMessage message, IPEndPoint endPoint)
		{
			try
			{
				var data = message.Pack();
				await _udpClient.SendAsync(data, data.Length, endPoint);

				// 触发消息发送事件
				var bodyText = message.MessageBody?.Length > 0 ?
					Encoding.UTF8.GetString(message.MessageBody) : "";
				MessageSent?.Invoke(this, new MessageEventArgs
				{
					ProtocolName = message.GetProtocolName(),
					MessageCode = message.MessageCode,
					MessageBody = bodyText,
					RemoteEndPoint = endPoint
				});
			}
			catch (Exception ex)
			{
				Console.WriteLine($"发送消息失败: {ex.Message}");
			}
		}

		private byte GetNextMessageCode()
		{
			return ++_messageCounter;
		}

		private void SetStatus(string status)
		{
			_currentStatus = status;
			StatusChanged?.Invoke(this, status);
		}
	}

	#endregion
}