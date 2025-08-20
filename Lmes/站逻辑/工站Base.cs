using Lmes.Models;
using Lmes.全局变量;
using Lmes.功能;
using Lmes.功能.枚举;
using Lmes.站逻辑.数据类型;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using DateTime = System.DateTime;

namespace Lmes.站逻辑
{
	public class 工站Base
	{

		#region 变量声明

		public ConcurrentDictionary<string, DataItem> 信号表 { get; set; } = new();
		public ConcurrentDictionary<string, DateTime> 更新时间 { get; set; } = new();
		public Plc plc;
		internal List<DataItem> 写入信号 { get; set; } = new();
		internal List<DataItem> 读取信号 { get; set; } = new();
		internal readonly PeriodicTimer 采集间隔 = new(TimeSpan.FromMilliseconds(300));
		DateTime 错误显示时间 = DateTime.Now;
		public MesApi mesApi;
		public LmesApi lmesApi;
		public IOTApi iotApi;
		public 产线信息类 产线信息;
		public 工位信息 工位信息;

		public virtual 工站_枚举 当前站站别 { get; set; } = 工站_枚举.未选择;
		public int 单次读取长度上限 { get; set; } = 960;

		#endregion

		public 工站Base(产线信息类 产线信息, 工位信息 工位信息, Plc plc)
		{
			this.plc = plc;
			this.产线信息 = 产线信息;
			this.工位信息 = 工位信息;
			foreach (var item in 工位信息.信号连接表)
			{
				信号表[item.信号名称] = item.信号地址;
			}
			mesApi = new MesApi(系统参数.设置.Lmes连接参数.LMES地址 + @"api/", 系统参数.设置.Lmes连接参数.appId, 系统参数.设置.Lmes连接参数.appKey);
			lmesApi = new(系统参数.设置.Lmes连接参数.产线MES地址);
		}

		/// <summary>
		/// PLC连接
		/// </summary>
		public virtual async Task 连接(CpuType cpuType, string ip, short rack, short slot, CancellationToken cancellationToken)
		{
			if (plc != null)
			{
				if (plc.IsConnected)
				{
					try
					{
						plc.Close();
					}
					catch (Exception)
					{

					}
				}
			}
			plc = new Plc(cpuType, ip, rack, slot);
			plc.WriteTimeout = 3000;
			plc.ReadTimeout = 3000;
			await plc.OpenAsync(cancellationToken);
		}

		/// <summary>
		/// 关闭PLC连接
		/// </summary>
		public virtual void 关闭连接()
		{
			if (plc != null)
			{
				plc.Close();
			}
		}

		public virtual async Task 运行(CancellationToken cancellationToken)
		{
			while (cancellationToken.IsCancellationRequested == false)
			{
				try
				{
					await 信号采集(cancellationToken);
					await 运行逻辑(cancellationToken);
					await 信号写入(cancellationToken);
					await 采集间隔.WaitForNextTickAsync(cancellationToken);
				}
				catch (OperationCanceledException)
				{
					// 处理取消操作
					日志写入.写入("操作已取消");
					break;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					通讯出错(ex);
				}

			}
			日志写入.写入("断开链接");
		}

		public virtual async Task 运行初始化(CancellationToken cancellationToken)
		{

		}

		public void 读取信号值(DataItem item)
		{
             plc.ReadMultipleVars([item]);
        }

        /// <summary>
        /// 读取plc的信号值
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task 信号采集(CancellationToken cancellationToken)
		{
			if (读取信号.Count <= 0) return;
			读取信号 = 读取信号.Distinct().ToList();
			try
			{
				List<List<DataItem>> 读取队列 = [[]];
				int 读取长度 = 16;
				int 使用队列 = 0;

				foreach (DataItem item in 读取信号)
				{
					int 长度 = 获取读取长度(item.VarType, item.Count, item.BitAdr);
					if (长度 > 单次读取长度上限)
					{
						throw new ArgumentOutOfRangeException($"信号长度过长!限制为{单次读取长度上限},实际长度以为{长度}\r\n{JsonSerializer.Serialize(item)}");
					}
					if (读取长度 + 长度 > 单次读取长度上限)
					{
						读取队列.Add([]);
						使用队列++;
						读取长度 = 16;
					}
					读取队列[使用队列].Add(item);
					读取长度 += 长度;

				}
				foreach (var item in 读取队列)
				{
					await plc.ReadMultipleVarsAsync(item, cancellationToken);
				}

				foreach (var v in 读取信号)
				{
					var temp = 信号表.FirstOrDefault(x => x.Value == v);
					更新时间[temp.Key] = DateTime.Now;
				}
				读取信号.Clear();
			}
			catch (Exception ex)
			{

				throw;
			}

		}

		public virtual async Task 运行逻辑(CancellationToken cancellationToken)
		{

		}

		/// <summary>
		/// 将信号值传给plc
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual async Task 信号写入(CancellationToken cancellationToken)
		{
			if (写入信号.Count <= 0) return;
			//写入信号 = 写入信号.Distinct().ToList();
			var 写入temp = 写入信号.Distinct();
			try
			{
				List<List<DataItem>> 写入队列 = [[]];
				int 写入长度 = 10;
				int index = 0;

				foreach (DataItem item in 写入temp)
				{
					int 长度 = 获取写入长度(item.VarType, item.Count, item.BitAdr);
					// if (读取长度>500)Debugger.Break();
					if (长度 > 单次读取长度上限)
					{
						throw new ArgumentOutOfRangeException($"信号长度过长!限制为{单次读取长度上限},实际长度以为{长度}\r\n{JsonSerializer.Serialize(item)}");
					}
					if (写入长度 + 长度 > 单次读取长度上限)
					{
						写入队列.Add([]);
						index++;
						写入长度 = 10;
					}
					写入队列[index].Add(item);
					写入长度 += 长度;

				}
				foreach (var item in 写入队列)
				{
					await plc.WriteAsync(item.ToArray());
				}

				foreach (var v in 写入temp)
				{
					var temp = 信号表.FirstOrDefault(x => x.Value == v);
					更新时间[temp.Key] = DateTime.Now;
				}
				写入信号.Clear();

			}
			catch (Exception ex)
			{
				Debugger.Break();
				throw;
			}
		}

		public int 获取写入长度(VarType varType, int 数量, int bit起始位)
		{
			return 16 + 获取数据长度(varType, 数量, bit起始位);
		}
		public int 获取读取长度(VarType varType, int 数量, int bit起始位)
		{
			return 4 + 获取数据长度(varType, 数量, bit起始位);
		}
		public int 获取数据长度(VarType varType, int 数量, int bit起始位)
		{
			switch (varType)
			{
				case VarType.Bit:
					return 2 * (int)Math.Ceiling(数量 / 16.0);
				case VarType.Byte:
					return 2 * (int)Math.Ceiling(数量 / 16.0);
				case VarType.Word:
					return 2 * 数量;
				case VarType.DWord:
					return 4 * 数量;
				case VarType.Int:
					return 2 * 数量;
				case VarType.DInt:
					return 4 * 数量;
				case VarType.Real:
					return 4 * 数量;
				case VarType.LReal:
					return 8 * 数量;
				case VarType.String:
					return 2 * (int)Math.Ceiling(数量 / 2.0);
				case VarType.S7String:
					return 2 + 2 * (int)Math.Ceiling(数量 / 2.0);
				case VarType.S7WString:
					return 4 + 2 * 数量;
				case VarType.Timer:
					return 4 * 数量;
				case VarType.Counter:
					return 4 * 数量;
				case VarType.DateTime:
					return 4 * 数量;
				case VarType.DateTimeLong:
					return 8;
				default:
					return 4;
			}

		}

		public virtual void 通讯出错(Exception ex)
		{
			if (错误显示时间 + TimeSpan.FromMinutes(1) < DateTime.Now)
			{
				日志写入.写入("通讯出错!" + ex.ToString());
				错误显示时间 = DateTime.Now;
			}
			Debugger.Break();
		}
	}
}
