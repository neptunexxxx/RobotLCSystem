using CRP;

namespace RobotControlSystem
{
	public class RobotController
	{
		private CRobotService robotService;
		private CIOService ioService;
		private CFileService fileService;
		private string robotIP;
		private bool isConnected = false;

		public RobotController(string ip = "192.168.1.100")
		{
			robotIP = ip;
			InitializeServices();
		}

		/// <summary>
		/// 初始化服务
		/// </summary>
		private void InitializeServices()
		{
			robotService = new CRobotService();

			if (!robotService.IsAvailable())
			{
				throw new Exception("Robot Service不可用，请检查license.key和DLL文件");
			}

			ioService = new CIOService(ref robotService);
			fileService = new CFileService(ref robotService);
		}

		/// <summary>
		/// 连接机器人
		/// </summary>
		public bool Connect()
		{
			try
			{
				if (isConnected)
					return true;

				isConnected = robotService.Connect(robotIP);

				if (isConnected)
				{
					Console.WriteLine($"成功连接到机器人: {robotIP}");

					// 连接后的初始化操作
					ClearErrors();
					PrintRobotInfo();
				}
				else
				{
					Console.WriteLine($"连接机器人失败: {robotIP}");
				}

				return isConnected;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"连接异常: {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// 断开连接
		/// </summary>
		public void Disconnect()
		{
			if (isConnected)
			{
				robotService.Disconnect();
				isConnected = false;
				Console.WriteLine("已断开机器人连接");
			}
		}

		/// <summary>
		/// 清除错误
		/// </summary>
		public void ClearErrors()
		{
			if (robotService.HasError())
			{
				robotService.ClearError();
				Console.WriteLine("错误已清除");
			}
		}

		/// <summary>
		/// 打印机器人信息
		/// </summary>
		private void PrintRobotInfo()
		{
			Console.WriteLine("=== 机器人状态 ===");
			Console.WriteLine($"伺服状态: {(robotService.IsServoOn() ? "已上电" : "未上电")}");

			int mode = robotService.GetWorkMode();
			string modeStr = mode switch
			{
				(int)ERobotMode.RM_Manual => "手动模式",
				(int)ERobotMode.RM_Playing => "自动模式",
				(int)ERobotMode.RM_Remote => "远程模式",
				_ => "未知模式"
			};
			Console.WriteLine($"工作模式: {modeStr}");
			Console.WriteLine($"速度比例: {robotService.GetSpeedRatio()}%");
		}

		/// <summary>
		/// 伺服上电
		/// </summary>
		public bool ServoPowerOn()
		{
			if (!isConnected)
			{
				Console.WriteLine("请先连接机器人");
				return false;
			}

			bool result = robotService.ServoPowerOn();
			Console.WriteLine(result ? "伺服上电成功" : "伺服上电失败");
			return result;
		}

		/// <summary>
		/// 伺服下电
		/// </summary>
		public bool ServoPowerOff()
		{
			if (!isConnected)
				return false;

			bool result = robotService.ServoPowerOff();
			Console.WriteLine(result ? "伺服下电成功" : "伺服下电失败");
			return result;
		}

		/// <summary>
		/// 获取当前位置
		/// </summary>
		public RobotPosition GetCurrentPosition()
		{
			var position = new RobotPosition();

			// 获取关节位置
			var jointPos = new List<double>();
			if (robotService.GetCurrentPosition(0, ref jointPos))
			{
				position.Joints = jointPos.ToArray();
			}

			// 获取笛卡尔位置
			var cartesianPos = new List<double>();
			if (robotService.GetCurrentPosition(2, ref cartesianPos))
			{
				if (cartesianPos.Count >= 6)
				{
					position.X = cartesianPos[0];
					position.Y = cartesianPos[1];
					position.Z = cartesianPos[2];
					position.Rx = cartesianPos[3];
					position.Ry = cartesianPos[4];
					position.Rz = cartesianPos[5];
				}
			}

			return position;
		}

		/// <summary>
		/// 运行程序
		/// </summary>
		public bool RunProgram(string programPath, int startLine = 0)
		{
			if (!isConnected)
			{
				Console.WriteLine("请先连接机器人");
				return false;
			}

			// 检查并上传程序
			string programName = System.IO.Path.GetFileName(programPath);

			if (System.IO.File.Exists(programPath))
			{
				if (!fileService.Upload(programPath, programName))
				{
					Console.WriteLine("程序上传失败");
					return false;
				}
			}

			// 设置为自动模式
			robotService.SetWorkMode((int)ERobotMode.RM_Playing);

			// 启动程序
			bool result = robotService.StartProgram(programName, startLine);
			Console.WriteLine(result ? $"程序 {programName} 启动成功" : "程序启动失败");

			return result;
		}

		/// <summary>
		/// 停止程序
		/// </summary>
		public void StopProgram()
		{
			if (robotService.GetProgramStatus() == (int)EProgramStatus.PS_Running)
			{
				robotService.StopProgram();
				Console.WriteLine("程序已停止");
			}
		}

		/// <summary>
		/// 设置数字输出
		/// </summary>
		public bool SetDigitalOutput(uint index, bool value)
		{
			return ioService.SetY(index, value);
		}

		/// <summary>
		/// 读取数字输入
		/// </summary>
		public bool GetDigitalInput(uint index)
		{
			bool value = false;
			ioService.GetX(index, ref value);
			return value;
		}
	}

	/// <summary>
	/// 机器人位置数据类
	/// </summary>
	public class RobotPosition
	{
		public double[] Joints { get; set; } = new double[6];
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		public double Rx { get; set; }
		public double Ry { get; set; }
		public double Rz { get; set; }

		public override string ToString()
		{
			return $"Position: X={X:F2}, Y={Y:F2}, Z={Z:F2}, Rx={Rx:F2}, Ry={Ry:F2}, Rz={Rz:F2}";
		}
	}
}