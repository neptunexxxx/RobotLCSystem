using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lmes
{
    public partial class App : Application
    {
        public App()
        {
            // 注册各层级异常处理
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        // UI线程未处理异常
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, "UI线程");
            e.Handled = true; // 标记为已处理防止崩溃
        }

        // 非UI线程未处理异常
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "非UI线程");
            }
        }

        // 任务未观察异常
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception, "后台任务");
            e.SetObserved(); // 标记为已观察
        }

        private void HandleException(Exception ex, string source)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"【异常来源】{source}");
            sb.AppendLine($"【异常类型】{ex.GetType().Name}");
            sb.AppendLine($"【异常信息】{ex.Message}");
            sb.AppendLine($"【堆栈跟踪】{ex.StackTrace}");


            // 显示给用户
            Dispatcher.Invoke(() =>
            {
                var result = MessageBox.Show(
                    $"程序发生未处理异常:\n{sb}\n\n是否尝试继续运行？",
                    "系统错误",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result != MessageBoxResult.Yes)
                {
                    Shutdown();
                }
            });
        }
    }
}