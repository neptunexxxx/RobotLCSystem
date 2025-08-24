using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Lmes.界面
{
	/// <summary>
	/// 布尔值转换为字符串的转换器
	/// </summary>
	public class BoolToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolValue)
			{
				return boolValue ? "运动中" : "静止";
			}
			return "未知";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 布尔值转换为颜色画刷的转换器
	/// </summary>
	public class BoolToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolValue)
			{
				return boolValue ? Brushes.Orange : Brushes.Green;
			}
			return Brushes.Gray;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 布尔值转换为错误状态颜色的转换器
	/// </summary>
	public class BoolToErrorColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolValue)
			{
				return boolValue ? Brushes.Red : Brushes.Green;
			}
			return Brushes.Gray;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 布尔值转换为IO状态颜色的转换器
	/// </summary>
	public class BoolToIOColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolValue)
			{
				return boolValue ? Brushes.LimeGreen : Brushes.LightGray;
			}
			return Brushes.Gray;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 布尔值转换为IO状态文本的转换器
	/// </summary>
	public class BoolToIOTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolValue)
			{
				return boolValue ? "ON" : "OFF";
			}
			return "---";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 连接状态转换为可见性的转换器
	/// </summary>
	public class ConnectionStatusToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isConnected)
			{
				// 根据参数决定显示逻辑
				string param = parameter?.ToString() ?? "connected";

				if (param == "connected")
				{
					return isConnected ? Visibility.Visible : Visibility.Collapsed;
				}
				else if (param == "disconnected")
				{
					return isConnected ? Visibility.Collapsed : Visibility.Visible;
				}
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 连接状态转换为按钮可用性的转换器
	/// </summary>
	public class ConnectionStatusToEnabledConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isConnected)
			{
				// 根据参数决定启用逻辑
				string param = parameter?.ToString() ?? "connected";

				if (param == "connected")
				{
					return isConnected;
				}
				else if (param == "disconnected")
				{
					return !isConnected;
				}
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 数值范围验证转换器
	/// </summary>
	public class NumberValidationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string stringValue)
			{
				// 根据目标类型进行转换
				if (targetType == typeof(int) || targetType == typeof(int?))
				{
					if (int.TryParse(stringValue, out int intResult))
					{
						return Math.Max(0, Math.Min(999, intResult)); // 限制范围
					}
				}
				else if (targetType == typeof(double) || targetType == typeof(double?))
				{
					if (double.TryParse(stringValue, out double doubleResult))
					{
						return Math.Max(-999999, Math.Min(999999, doubleResult));
					}
				}
			}
			return value;
		}
	}

	/// <summary>
	/// 程序状态转换为颜色的转换器
	/// </summary>
	public class ProgramStatusToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string status)
			{
				return status switch
				{
					"运行" => Brushes.Green,
					"暂停" => Brushes.Orange,
					"停止" => Brushes.Red,
					_ => Brushes.Gray
				};
			}
			return Brushes.Gray;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 时间戳格式化转换器
	/// </summary>
	public class TimestampFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DateTime dateTime)
			{
				string format = parameter?.ToString() ?? "HH:mm:ss";
				return dateTime.ToString(format);
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 多值转换器：用于多个条件的组合判断
	/// </summary>
	public class MultiBooleanToEnabledConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values.Length == 0) return false;

			// 检查所有条件是否都为true
			foreach (var value in values)
			{
				if (value is bool boolValue && !boolValue)
				{
					return false;
				}
			}
			return true;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}