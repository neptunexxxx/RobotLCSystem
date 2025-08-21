using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UdpClient = NetCoreServer.UdpClient;

namespace UdpLowerMachine
{
	/// <summary>
	/// 自定义UDP客户端类，继承自NetCoreServer.UdpClient
	/// </summary>
	public class LowerMachineUdpClient : UdpClient
	{
		private MainWindow _mainWindow;
		private bool _stop = false;

		public LowerMachineUdpClient(string address, int port, MainWindow mainWindow) : base(address, port)
		{
			_mainWindow = mainWindow;
		}

		public void DisconnectAndStop()
		{
			_stop = true;
			Disconnect();
			while (IsConnected)
				Thread.Yield();
		}

		// 连接成功
		protected override void OnConnected()
		{
			_mainWindow?.AddLog($"UDP客户端已连接，会话ID: {Id}");
			// 开始接收数据包
			ReceiveAsync();
		}

		// 断开连接
		protected override void OnDisconnected()
		{
			_mainWindow?.AddLog($"UDP客户端已断开连接，会话ID: {Id}");

			// 如果没有手动停止，尝试重连
			if (!_stop)
			{
				Thread.Sleep(1000);
				Connect();
			}
		}

		// 接收到数据
		protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
		{
			try
			{
				// 创建实际数据数组
				byte[] data = new byte[size];
				Array.Copy(buffer, offset, data, 0, size);

				// 处理接收到的消息
				if (endpoint is IPEndPoint ipEndpoint)
				{
					_mainWindow?.ProcessReceivedMessage(data, ipEndpoint);
				}

				// 继续接收数据包
				ReceiveAsync();
			}
			catch (Exception ex)
			{
				_mainWindow?.AddLog($"处理接收数据时出错: {ex.Message}");
			}
		}

		// 发生错误
		protected override void OnError(SocketError error)
		{
			_mainWindow?.AddLog($"UDP客户端发生错误: {error}");
		}

		// 发送消息
		public void SendMessage(byte[] data)
		{
			Send(data);
		}
	}

	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	[AddINotifyPropertyChangedInterface]
	public partial class MainWindow : Window
	{
		private LowerMachineUdpClient? udpClient;
		private IPEndPoint? localEndPoint;
		private bool isListening = false;
		private byte messageCode = 0;
		private string localIP;

		public ObservableCollection<string> LogMessages { get; set; }

		public string LocalIP { get; set; } = "127.0.0.1";

		public string LocalPort { get; set; } = "";

		public string RemoteIP { get; set; } = "127.0.0.1";

		public string RemotePort { get; set; } = "5010";

		public string ConnectionStatus { get; set; } = "未连接";

		public MainWindow()
		{
			InitializeComponent();
			LogMessages = new ObservableCollection<string>();
			DataContext = this;
			AddLog("程序启动");
		}

		private void StartConnection_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (isListening)
				{
					StopConnection();
					return;
				}
				// 验证输入
				if (!IPAddress.TryParse(RemoteIP, out _) || !int.TryParse(RemotePort, out int remotePortNum))
				{
					AddLog("远程IP或端口格式错误");
					return;
				}
				// 创建UDP客户端，连接到远程服务器
				udpClient = new LowerMachineUdpClient(RemoteIP, remotePortNum, this);

				foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
				{
					if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4
					{
						localIP = ip.ToString();
						break;
					}
				}
				LocalIP = localIP;

				// 连接到服务器
				if (udpClient.Connect())
				{
					isListening = true;
					ConnectionStatus = "已连接";
					StartConnectionBtn.Content = "断开连接";
					AddLog($"正在连接到远程服务器 {RemoteIP}:{RemotePort}");
				}
				else
				{
					AddLog("连接到UDP服务器失败");
					StopConnection();
				}
			}
			catch (Exception ex)
			{
				AddLog($"连接失败: {ex.Message}");
				StopConnection();
			}
		}

		private void StopConnection()
		{
			try
			{
				isListening = false;
				udpClient?.DisconnectAndStop();
				udpClient = null;

				ConnectionStatus = "未连接";
				StartConnectionBtn.Content = "开始连接";
				AddLog("连接已断开");
			}
			catch (Exception ex)
			{
				AddLog($"断开连接时出错: {ex.Message}");
			}
		}

		public async void ProcessReceivedMessage(byte[] data, IPEndPoint senderEndPoint)
		{
			try
			{
				if (data.Length < 12) // 最小包长度
				{
					AddLog("收到的数据包长度不足");
					return;
				}

				// 解析协议头
				byte header = data[0];
				byte protocolType = data[1];
				byte subSystemId = data[2];
				byte msgCode = data[3];
				byte sendSource = data[4];
				ushort dataLength = BitConverter.ToUInt16(data, 8);

				AddLog($"收到消息 - 头:{header:X2} 协议类型:{protocolType} 子系统:{subSystemId} 消息码:{msgCode} 源:{sendSource} 长度:{dataLength}");

				// 根据协议类型处理消息
				await HandleProtocolMessage(protocolType, msgCode, data, senderEndPoint);
			}
			catch (Exception ex)
			{
				AddLog($"处理接收消息时出错: {ex.Message}");
			}
		}

		private async Task HandleProtocolMessage(byte protocolType, byte msgCode, byte[] data, IPEndPoint senderEndPoint)
		{
			switch (protocolType)
			{
				case 20: // 初始化命令
					AddLog("收到初始化命令(20)");
					await SendAckMessage(21, msgCode); // 发送ACK
													   //初始化
					await Task.Delay(1000); // 模拟初始化过程
					await SendInitCompleteMessage(22, msgCode); // 发送初始化完成
					break;

				case 30: // 虚拟焊接命令
					AddLog("收到虚拟焊接命令(30)");
					await SendAckMessage(31, msgCode);
					//虚拟焊接
					await Task.Delay(2000); // 模拟虚拟焊接过程
					await SendVirtualWeldCompleteMessage(32, msgCode);
					break;

				case 40: // 焊接命令
					AddLog("收到焊接命令(40)");
					await SendAckMessage(41, msgCode);
					//焊接
					await Task.Delay(3000); // 模拟焊接过程
					await SendWeldCompleteMessage(42, msgCode);
					break;

				case 50: // 焊接工艺调整命令
					AddLog("收到焊接工艺调整命令(50)");
					await SendAckMessage(51, msgCode);
					break;

				case 60: // 焊后检测命令
					AddLog("收到焊后检测命令(60)");
					await SendAckMessage(61, msgCode);
					//焊后检测
					await Task.Delay(2000); // 模拟检测过程
					await SendDetectionCompleteMessage(62, msgCode);
					break;

				default:
					AddLog($"未知协议类型: {protocolType}");
					break;
			}
		}

		private async Task SendAckMessage(byte protocolType, byte originalMsgCode)
		{
			try
			{
				var packet = CreateProtocolPacket(protocolType, originalMsgCode, "");
				await Task.Run(() => udpClient?.SendMessage(packet));
				AddLog($"发送ACK消息 - 协议类型:{protocolType} 消息码:{originalMsgCode}");
			}
			catch (Exception ex)
			{
				AddLog($"发送ACK消息失败: {ex.Message}");
			}
		}

		private async Task SendInitCompleteMessage(byte protocolType, byte originalMsgCode)
		{
			try
			{
				var packet = CreateProtocolPacket(protocolType, originalMsgCode, "0:success");
				await Task.Run(() => udpClient?.SendMessage(packet));
				AddLog($"发送初始化完成消息 - 协议类型:{protocolType}");
			}
			catch (Exception ex)
			{
				AddLog($"发送初始化完成消息失败: {ex.Message}");
			}
		}

		private async Task SendVirtualWeldCompleteMessage(byte protocolType, byte originalMsgCode)
		{
			try
			{
				var packet = CreateProtocolPacket(protocolType, originalMsgCode, "0:success:virtual_weld_file.pro");
				await Task.Run(() => udpClient?.SendMessage(packet));
				AddLog($"发送虚拟焊接完成消息 - 协议类型:{protocolType}");
			}
			catch (Exception ex)
			{
				AddLog($"发送虚拟焊接完成消息失败: {ex.Message}");
			}
		}

		private async Task SendWeldCompleteMessage(byte protocolType, byte originalMsgCode)
		{
			try
			{
				var packet = CreateProtocolPacket(protocolType, originalMsgCode, "0:success:weld_result_file.log");
				await Task.Run(() => udpClient?.SendMessage(packet));
				AddLog($"发送焊接完成消息 - 协议类型:{protocolType}");
			}
			catch (Exception ex)
			{
				AddLog($"发送焊接完成消息失败: {ex.Message}");
			}
		}

		private async Task SendDetectionCompleteMessage(byte protocolType, byte originalMsgCode)
		{
			try
			{
				var packet = CreateProtocolPacket(protocolType, originalMsgCode, "0:success:detection_image.bmp:pointcloud.ply");
				await Task.Run(() => udpClient?.SendMessage(packet));
				AddLog($"发送检测完成消息 - 协议类型:{protocolType}");
			}
			catch (Exception ex)
			{
				AddLog($"发送检测完成消息失败: {ex.Message}");
			}
		}

		private async void SendStatusMessage_Click(object sender, RoutedEventArgs e)
		{
			if (!isListening || udpClient == null || !udpClient.IsConnected)
			{
				AddLog("未连接，无法发送状态消息");
				return;
			}

			try
			{
				messageCode = (byte)((messageCode + 1) % 256);
				var packet = CreateProtocolPacket(1, messageCode, "1:设备正常运行");
				await Task.Run(() => udpClient.SendMessage(packet));
				AddLog($"发送周期性状态消息 - 消息码:{messageCode}");
			}
			catch (Exception ex)
			{
				AddLog($"发送状态消息失败: {ex.Message}");
			}
		}

		private byte[] CreateProtocolPacket(byte protocolType, byte msgCode, string messageBody)
		{
			byte[] bodyBytes = Encoding.UTF8.GetBytes(messageBody);
			int totalLength = 12 + bodyBytes.Length; // 10字节头 + 消息体 + 2字节CRC

			byte[] packet = new byte[totalLength];

			// 协议头
			packet[0] = 0x55; // 数据头
			packet[1] = protocolType; // 协议类型
			packet[2] = 1; // 子系统代号(下位机设为1)
			packet[3] = msgCode; // 消息码
			packet[4] = 1; // 发送源(下位机)
						   // 5-7预留字节已经是0

			// 数据长度(整个包的长度)
			byte[] lengthBytes = BitConverter.GetBytes((ushort)totalLength);
			packet[8] = lengthBytes[0];
			packet[9] = lengthBytes[1];

			// 消息体
			if (bodyBytes.Length > 0)
			{
				Array.Copy(bodyBytes, 0, packet, 10, bodyBytes.Length);
			}

			// CRC校验(简单实现，实际应用中需要正确的CRC-16算法)
			ushort crc = CalculateCRC16(packet, totalLength - 2);
			byte[] crcBytes = BitConverter.GetBytes(crc);
			packet[totalLength - 2] = crcBytes[0];
			packet[totalLength - 1] = crcBytes[1];

			return packet;
		}

		private ushort CalculateCRC16(byte[] data, int length)
		{
			// 简单的CRC16实现(实际应用中应该使用标准CRC16算法)
			ushort crc = 0xFFFF;
			for (int i = 0; i < length; i++)
			{
				crc ^= data[i];
				for (int j = 0; j < 8; j++)
				{
					if ((crc & 1) != 0)
						crc = (ushort)((crc >> 1) ^ 0xA001);
					else
						crc >>= 1;
				}
			}
			return crc;
		}

		public void AddLog(string message)
		{
			Dispatcher.Invoke(() =>
			{
				string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
				LogMessages.Add($"[{timestamp}] {message}");

				// 限制日志数量
				if (LogMessages.Count > 1000)
				{
					LogMessages.RemoveAt(0);
				}

				// 自动滚动到最新消息
				if (LogListBox.Items.Count > 0)
				{
					LogListBox.ScrollIntoView(LogListBox.Items[LogListBox.Items.Count - 1]);
				}
			});
		}

		private void ClearLog_Click(object sender, RoutedEventArgs e)
		{
			LogMessages.Clear();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			StopConnection();
			base.OnClosing(e);
		}
	}
}