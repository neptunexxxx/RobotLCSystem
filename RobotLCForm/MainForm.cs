using RobotControlSystem;

namespace RobotLCForm
{
	public partial class MainForm : Form
	{
		private RobotController robot;
		private System.Windows.Forms.Timer updateTimer;

		// UI控件
		private Button btnConnect;
		private Button btnDisconnect;
		private Button btnServoOn;
		private Button btnServoOff;
		private TextBox txtIP;
		private Label lblStatus;
		private Label lblPosition;

		public MainForm()
		{
			InitializeComponent();
			InitializeRobot();
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			txtIP = new TextBox();
			btnConnect = new Button();
			btnDisconnect = new Button();
			btnServoOn = new Button();
			btnServoOff = new Button();
			lblStatus = new Label();
			lblPosition = new Label();
			updateTimer = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			// 
			// txtIP
			// 
			txtIP.Location = new Point(0, 0);
			txtIP.Name = "txtIP";
			txtIP.Size = new Size(100, 23);
			txtIP.TabIndex = 0;
			// 
			// btnConnect
			// 
			btnConnect.Location = new Point(0, 0);
			btnConnect.Name = "btnConnect";
			btnConnect.Size = new Size(75, 23);
			btnConnect.TabIndex = 1;
			btnConnect.Click += BtnConnect_Click;
			// 
			// btnDisconnect
			// 
			btnDisconnect.Location = new Point(0, 0);
			btnDisconnect.Name = "btnDisconnect";
			btnDisconnect.Size = new Size(75, 23);
			btnDisconnect.TabIndex = 2;
			btnDisconnect.Click += BtnDisconnect_Click;
			// 
			// btnServoOn
			// 
			btnServoOn.Location = new Point(0, 0);
			btnServoOn.Name = "btnServoOn";
			btnServoOn.Size = new Size(75, 23);
			btnServoOn.TabIndex = 3;
			btnServoOn.Click += BtnServoOn_Click;
			// 
			// btnServoOff
			// 
			btnServoOff.Location = new Point(0, 0);
			btnServoOff.Name = "btnServoOff";
			btnServoOff.Size = new Size(75, 23);
			btnServoOff.TabIndex = 4;
			btnServoOff.Click += BtnServoOff_Click;
			// 
			// lblStatus
			// 
			lblStatus.Location = new Point(0, 0);
			lblStatus.Name = "lblStatus";
			lblStatus.Size = new Size(100, 23);
			lblStatus.TabIndex = 5;
			// 
			// lblPosition
			// 
			lblPosition.Location = new Point(0, 0);
			lblPosition.Name = "lblPosition";
			lblPosition.Size = new Size(100, 23);
			lblPosition.TabIndex = 6;
			// 
			// updateTimer
			// 
			updateTimer.Interval = 500;
			updateTimer.Tick += UpdateTimer_Tick;
			// 
			// MainForm
			// 
			ClientSize = new Size(584, 361);
			Controls.Add(txtIP);
			Controls.Add(btnConnect);
			Controls.Add(btnDisconnect);
			Controls.Add(btnServoOn);
			Controls.Add(btnServoOff);
			Controls.Add(lblStatus);
			Controls.Add(lblPosition);
			Name = "MainForm";
			Text = "机器人控制系统";
			ResumeLayout(false);
			PerformLayout();
		}

		private void InitializeRobot()
		{
			try
			{
				robot = new RobotController();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"初始化失败1: {ex.Message}", "错误",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void BtnConnect_Click(object sender, EventArgs e)
		{
			robot = new RobotController(txtIP.Text);

			await Task.Run(() =>
			{
				if (robot.Connect())
				{
					this.Invoke(new Action(() =>
					{
						btnConnect.Enabled = false;
						btnDisconnect.Enabled = true;
						btnServoOn.Enabled = true;
						btnServoOff.Enabled = true;
						lblStatus.Text = "状态: 已连接";
						updateTimer.Start();
					}));
				}
				else
				{
					this.Invoke(new Action(() =>
					{
						MessageBox.Show("连接失败", "错误");
					}));
				}
			});
		}

		private void BtnDisconnect_Click(object sender, EventArgs e)
		{
			updateTimer.Stop();
			robot.Disconnect();

			btnConnect.Enabled = true;
			btnDisconnect.Enabled = false;
			btnServoOn.Enabled = false;
			btnServoOff.Enabled = false;
			lblStatus.Text = "状态: 未连接";
			lblPosition.Text = "位置: --";
		}

		private async void BtnServoOn_Click(object sender, EventArgs e)
		{
			await Task.Run(() => robot.ServoPowerOn());
		}

		private async void BtnServoOff_Click(object sender, EventArgs e)
		{
			await Task.Run(() => robot.ServoPowerOff());
		}

		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				var pos = robot.GetCurrentPosition();
				this.Invoke(new Action(() =>
				{
					lblPosition.Text = pos.ToString();
				}));
			});
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			updateTimer?.Stop();
			robot?.Disconnect();
			base.OnFormClosed(e);
		}
	}
}
