using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lmes.功能
{
	public class UpdateManager
	{
		private readonly string _programPath;
		private readonly string _updatePath;
		private readonly string _backupPath;
		private readonly VersionManager _versionManager;

		public UpdateManager()
		{
			// 程序主目录
			_programPath = AppDomain.CurrentDomain.BaseDirectory;
			// 更新文件夹
			_updatePath = Path.Combine(_programPath, "Update");
			// 备份文件夹
			_backupPath = Path.Combine(_programPath, "Backup");
			_versionManager = new VersionManager();
		}

		public async Task ExtractAndUpdate(string zipPath, string newVersion)
		{
			try
			{
				// 准备更新目录
				if (Directory.Exists(_updatePath))
					Directory.Delete(_updatePath, true);
				Directory.CreateDirectory(_updatePath);

				// 解压新版本到更新目录
				await Task.Run(() => ZipFile.ExtractToDirectory(zipPath, _updatePath));




				// 创建更新批处理文件
				string batchContent = @"
@echo off
:: 等待原程序退出
timeout /t 1 /nobreak

:: 复制新文件
xcopy /Y /E ""%~dp0Update\*.*"" ""%~dp0.""

:: 删除更新文件夹
rd /S /Q ""%~dp0Update""

:: 清理 Updates 文件夹
if exist ""%~dp0Updates"" (
    rd /S /Q ""%~dp0Updates""
)

:: 启动程序
start """" ""%~dp0LMES.exe""

:: 删除自身
del ""%~f0""
";

				string batchPath = Path.Combine(_programPath, "update.bat");
				await File.WriteAllTextAsync(batchPath, batchContent);

				// 启动更新批处理并退出程序
				ProcessStartInfo startInfo = new ProcessStartInfo
				{
					FileName = batchPath,
					UseShellExecute = true,
					CreateNoWindow = true,
					Verb = "runas", // 请求管理员权限
				};
				Process.Start(startInfo);
				// 更新版本文件
				_versionManager.UpdateVersion(newVersion);
				// 退出应用程序
				Application.Current.Shutdown();
			}
			catch (Exception ex)
			{
				throw new Exception($"更新失败: {ex.Message}");
			}
		}
	}

	public class VersionManager
	{
		private readonly string _versionFilePath;

		public VersionManager()
		{
			_versionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version.txt");
		}

		public string GetCurrentVersion()
		{
			// First try to read from version file
			if (File.Exists(_versionFilePath))
			{
				try
				{
					return File.ReadAllText(_versionFilePath).Trim();
				}
				catch (Exception ex)
				{
					日志写入.写入($"读取版本文件失败: {ex.Message}");
				}
			}

			// Fall back to assembly version if file doesn't exist
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public void UpdateVersion(string newVersion)
		{
			try
			{
				File.WriteAllText(_versionFilePath, newVersion);
			}
			catch (Exception ex)
			{
				日志写入.写入($"写入版本文件失败: {ex.Message}");
				throw;
			}
		}
	}
}
