using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EFDbContext;
using EFDbContext.Entity;
using Lmes.Models;
using Lmes.全局变量;
using Lmes.功能;
using Lmes.功能.数据类型请求体;
using Lmes.功能.枚举;
using Lmes.站逻辑.信号名称;
using Lmes.站逻辑.数据类型;
using Lmes.站逻辑.步骤枚举;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.Crypt.BinaryRC4;
using Org.BouncyCastle.Bcpg;
using S7.Net;
using SkiaSharp;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Windows;
using Timer = System.Timers.Timer;
using static Lmes.Models.工艺路径;
using System.Windows.Media.TextFormatting;
using NPOI.SS.Formula.Functions;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Cms;
using System.Runtime.CompilerServices;

namespace Lmes.站逻辑
{
    public class 通用站 : 工站Base
    {
        private readonly 工站信号名称Base 信号名称表;
        public 通用站(产线信息类 产线信息, 工位信息 工位信息, Plc plc) : base(产线信息, 工位信息, plc)
        {
            // 动态创建信号名称表实例
            信号名称表 = 创建信号名称表实例(工位信息.工位编号);
        }

        private 工站信号名称Base? 创建信号名称表实例(string? 工位编号)
        {
            //获取信号名称表类型
            var 信号名称表类型 = Type.GetType($"Lmes.站逻辑.信号名称.{工位编号}信号名称");
            if (信号名称表类型 == null)
            {
                throw new InvalidOperationException($"无法找到类型{工位编号}信号名称");
            }

            return (工站信号名称Base?)Activator.CreateInstance(信号名称表类型);
        }

        /// <summary>
        /// Key：参数编码
        /// </summary>
        //Dictionary<string, 工艺参数信息接口返回体object> 参数字典 = new();
        List<工艺参数设置> 参数字典 = new();

        ///// <summary>
        ///// Key：物料编码
        ///// </summary>
        //Dictionary<string, 物料绑定信息接口请求体Data> 物料字典 = new();

        /// <summary>
        /// Key：参数编码
        /// </summary>
        //Dictionary<string, string> 参数键值匹配 = new();

        /// <summary>
        /// 装配物料
        /// </summary>
        List<string> 子物料 = new();

        /// <summary>
        /// 小总成生产出的物料
        /// </summary>
        List<string> 总成物料 = new();

        public int 写入心跳时间 { get; set; } = 500;

        public int 读取心跳时间 { get; set; } = 1000;

        Stopwatch 心跳写入sw { get; set; } = new();

        bool 写入心跳状态;

        public bool 心跳状态 { get; set; } = false;

        Stopwatch 心跳采集sw { get; set; } = new();

        bool 读取上次心跳状态;

        Stopwatch stopwatch = new Stopwatch();

        生产过站信息接口请求体 生产过站信息 = new 生产过站信息接口请求体();

        生产参数信息接口请求体 生产参数信息 = new 生产参数信息接口请求体();

        设备状态信息接口请求体 设备状态信息 = new 设备状态信息接口请求体();

        物料绑定信息接口请求体 物料绑定信息 = new 物料绑定信息接口请求体();

        生产不良数据接口请求体 生产不良信息 = new 生产不良数据接口请求体();

        private Task<生产参数信息接口返回体?> 上传生产参数到MES = Task.FromResult<生产参数信息接口返回体?>(null);

        private Task<设备状态信息接口返回体?> 上传设备状态到MES = Task.FromResult<设备状态信息接口返回体?>(null);

        private Task<生产过站信息接口返回体?> 上传过站信息到MES = Task.FromResult<生产过站信息接口返回体?>(null);

        private Task<设备稼动时长数采接口返回体?> 上传设备稼动时长到MES = Task.FromResult<设备稼动时长数采接口返回体?>(null);

        private Task<物料绑定信息接口返回体?> 上传物料绑定信息到MES = Task.FromResult<物料绑定信息接口返回体?>(null);

        private Task<生产不良数据接口返回体?> 上传不良信息到MES = Task.FromResult<生产不良数据接口返回体?>(null);

        private Task<bool> 保存过站信息到数据库 = Task.FromResult(true);

        private Task<bool> 保存生产参数信息到数据库 = Task.FromResult(true);

        private Task<bool> 保存设备状态信息到数据库 = Task.FromResult(true);

        private Task<bool> 保存设备稼动时长到数据库 = Task.FromResult(true);

        private Task<bool> 保存物料绑定信息到数据库 = Task.FromResult(true);

        private Task<bool> 保存不良信息到数据库 = Task.FromResult(true);

        private 生产状态 生产状态;

        //private RealTimeProductInfo 当前产品;

        private bool 子物料绑定 { get; set; } = new();

        public List<string> 物料绑定集合 { get; set; } = new();

        public override async Task 运行初始化(CancellationToken cancellationToken)
        {
            信号表 = new();
            foreach (var item in 工位信息.信号连接表)
            {
                信号表.TryAdd(item.信号名称, item.信号地址);
            }
            switch (plc.CPU)
            {
                case CpuType.S71500:
                    单次读取长度上限 = 960;
                    break;
                case CpuType.S71200:
                    单次读取长度上限 = 240;
                    break;
                default:
                    break;
            }
            //Task task1 = base.连接(cpuType, 工位信息.PLC连接设置.IP地址, 工位信息.PLC连接设置.机架, 工位信息.PLC连接设置.槽位, cancellationToken);
            Task task2 = 初始化工艺参数(cancellationToken);
            Task task4 = 初始化物料编码(cancellationToken);
            Task<条码生成信息接口返回体?> task3 = Task.FromResult<条码生成信息接口返回体?>(null);
            //int 产品生产数量;
            //using (数据库连接 数据库 = new())
            //{
            //    产品生产数量 = 数据库.生产实时信息.Count(x => x.scheduleCode == 产线信息.排程编码);
            //}
            //if (产品生产数量 != Convert.ToInt32(产线信息.产品计划数量))
            //{
            //    条码生成信息接口请求体 tmp = new()
            //    {
            //        factoryCode = 产线信息.工厂编号,
            //        lineCode = 产线信息.产线编号,
            //        stationCode = 工位信息.工位编号,
            //        materialCode = "",
            //        scheduleCode = 产线信息.排程编码,
            //        userId = 工位信息.工号,
            //        snQty = (Convert.ToInt32(产线信息.产品计划数量) - 产品生产数量).ToString(),
            //        requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            //    };
            //    //初始化产品
            //    task3 = mesApi.条码生成信息接口(tmp);
            //    日志写入.写入("当前排程生产已完成");
            //}
            while (/*!task1.IsCompleted ||*/ !task2.IsCompleted /*|| !task3.IsCompleted*/)
            {
                await Task.Delay(100);
            }
            //var data = task3.Result;
            //if (data != null && data.data != null && data.data.sn != null)
            //{
            //	if (产线信息.工艺路径.工序字典[工位信息.工位编号] == 产线信息.工艺路径.工序路径[0])
            //	{
            //		List<RealTimeProductInfo> 生产实时信息 = new();
            //		foreach (var sn in data.data.sn)
            //		{
            //			生产实时信息.Add(new()
            //			{
            //				snNumber = sn,
            //				snNumber = "",
            //				stationCode = "0",
            //				scheduleCode = 产线信息.排程编码,
            //				operationCode = 产线信息.工艺路径.工序字典[工位信息.工位编号],
            //				isbad = false
            //			});
            //		}
            //		using (数据库连接 数据库 = new())
            //		{
            //			数据库.生产实时信息.AddRange(生产实时信息);
            //			数据库.SaveChanges();
            //		}
            //	}
            //}
            参数设置界面.排程变更事件 += ((string 产线编号, 工单排程信息查询接口返回体object 排程信息) =>
            {
                if (产线信息.产线编号 == 产线编号)
                {
                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"当前排程：{排程信息.scheduleCode}  {排程信息.startTime}");
                    产线信息.排程编码 = 排程信息.scheduleCode;
                    产线信息.产品计划数量 = 排程信息.scheduleQty;
                }
            });
            if (工位信息.工位号 == "1")
            {
                iotApi = new(系统参数.设置.Lmes连接参数.IOT地址);
                Timer timer = new(300000);
                timer.Elapsed += 上传设备状态到IOT;
                timer.AutoReset = true;
                timer.Enabled = true;
                Timer timer1 = new(3000);
                timer1.Elapsed += 更新设备状态;
                timer1.AutoReset = true;
                timer1.Enabled = true;
            }
            日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"工站运行初始化成功");
        }

        /// <summary>
        /// 单位：ms
        /// </summary>
        private int 设备状态上传间隔时间 = 300000;
        private async void 更新设备状态(object? sender, System.Timers.ElapsedEventArgs e)
        {
            await plc.ReadMultipleVarsAsync([信号表[信号名称表.读取_设备状态]]);
            if (信号表[信号名称表.读取_设备状态].Value != null)
            {
                string 设备实时状态 = 设备状态匹配(Convert.ToInt16(信号表[信号名称表.读取_设备状态].Value));
                using (数据库连接 数据库 = new())
                {
                    try
                    {
                        var 当前设备 = 数据库.设备实时信息.FirstOrDefault(x => x.machineCode == 工位信息.设备编码);
                        if (当前设备 == null)
                        {
                            当前设备 = new()
                            {
                                machineCode = 工位信息.设备编码,
                                machineStatusCode = 设备状态匹配(Convert.ToInt16(信号表[信号名称表.读取_设备状态].Value)),
                                machineStatusBegin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                machineStatusEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            };
                            数据库.Add(当前设备);
                        }
                        string 设备状态 = 当前设备.machineStatusCode;
                        设备稼动时长数采接口请求体 设备稼动时长数采;
                        var 单次状态持续时长 = DateTime.ParseExact(当前设备.machineStatusEnd, "yyyy-MM-dd HH:mm:ss", null) - DateTime.ParseExact(当前设备.machineStatusBegin, "yyyy-MM-dd HH:mm:ss", null);
                        if (设备状态 == 设备实时状态)
                        {
                            if (单次状态持续时长.TotalSeconds >= 设备状态上传间隔时间 / 1000)
                            {
                                当前设备.machineStatusBegin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                if (设备状态 == "inUse")
                                {
                                    当前设备.totalRunningDuration = (Convert.ToInt32(当前设备.totalRunningDuration) + 单次状态持续时长.TotalSeconds).ToString();
                                    当前设备.totalOnLineDuration = (Convert.ToInt32(当前设备.totalOnLineDuration) + 单次状态持续时长.TotalSeconds).ToString();
                                }
                                else if (设备状态 == "free" || 设备状态 == "normal")
                                {
                                    当前设备.totalOnLineDuration = (Convert.ToInt32(当前设备.totalOnLineDuration) + 单次状态持续时长.TotalSeconds).ToString();
                                }
                                else if (设备状态 == "fault")
                                {
                                    当前设备.curFaultDuration = (Convert.ToInt32(当前设备.curFaultDuration) + 单次状态持续时长.TotalSeconds).ToString();
                                }
                            }
                            当前设备.machineStatusEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            if (设备实时状态 == "fault")
                            {
                                当前设备.curFaultDuration = "0";
                            }
                            当前设备.machineStatusEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            单次状态持续时长 = DateTime.ParseExact(当前设备.machineStatusEnd, "yyyy-MM-dd HH:mm:ss", null) - DateTime.ParseExact(当前设备.machineStatusBegin, "yyyy-MM-dd HH:mm:ss", null);
                            if (设备状态 == "inUse")
                            {
                                当前设备.totalRunningDuration = (Convert.ToDouble(当前设备.totalRunningDuration) + 单次状态持续时长.TotalSeconds).ToString();
                                当前设备.totalOnLineDuration = (Convert.ToDouble(当前设备.totalOnLineDuration) + 单次状态持续时长.TotalSeconds).ToString();
                                设备稼动时长数采 = new()
                                {
                                    factoryCode = 产线信息.工厂编号,
                                    requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    machineCode = 工位信息.设备编码,
                                    machineStatusCode = 当前设备.machineStatusCode,
                                    requestMark = "2",
                                    timeUnit = "second",
                                    totalRunningDuration = 当前设备.totalRunningDuration
                                };
                                上传设备稼动时长到MES = mesApi.设备稼动时长数采接口(设备稼动时长数采);
                                设备稼动时长数采.totalRunningDuration = 当前设备.totalRunningDuration;
                                保存设备稼动时长到数据库 = lmesApi.新增设备稼动时长数采信息(设备稼动时长数采);
                            }
                            else
                            {
                                if (设备状态 == "fault")
                                {
                                    当前设备.curFaultDuration = (Convert.ToInt32(当前设备.curFaultDuration) + 单次状态持续时长.TotalSeconds).ToString();
                                }
                                设备稼动时长数采 = new()
                                {
                                    factoryCode = 产线信息.工厂编号,
                                    requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    machineCode = 工位信息.设备编码,
                                    machineStatusCode = 当前设备.machineStatusCode,
                                    requestMark = "1",
                                    acquisitCode = ""
                                };
                                上传设备稼动时长到MES = mesApi.设备稼动时长数采接口(设备稼动时长数采);
                                保存设备稼动时长到数据库 = lmesApi.新增设备稼动时长数采信息(设备稼动时长数采);
                            }
                            设备状态信息.factoryCode = 产线信息.工厂编号;
                            设备状态信息.requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            设备状态信息.data.Clear();
                            设备状态信息.data.Add(new()
                            {
                                machineCode = 工位信息.设备编码,
                                lineCode = 产线信息.产线编号,
                                stationCode = 工位信息.工位编号,
                                machineStatusCode = 当前设备.machineStatusCode,
                                machineStatusBegin = 当前设备.machineStatusBegin,
                                machineStatusEnd = 当前设备.machineStatusEnd
                            });
                            上传设备状态到MES = mesApi.设备状态信息接口(设备状态信息);
                            保存设备状态信息到数据库 = lmesApi.新增设备状态信息(设备状态信息);
                            当前设备.machineStatusCode = 设备状态匹配(Convert.ToInt16(信号表[信号名称表.读取_设备状态].Value));
                            当前设备.machineStatusBegin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            当前设备.machineStatusEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        await 数据库.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private async void 上传设备状态到IOT(object? sender, System.Timers.ElapsedEventArgs e)
        {
            await plc.ReadMultipleVarsAsync([信号表[信号名称表.读取_设备状态]]);
            using (数据库连接 数据库 = new())
            {
                var 设备实时信息 = 数据库.设备实时信息.FirstOrDefault(x => x.machineCode == 工位信息.设备编码);
                if (设备实时信息 == null) return;
                设备状态_IOT请求体 tmp = new()
                {
                    machineCode = 工位信息.设备编码,
                    lineCode = 产线信息.产线编号,
                    stationCode = 工位信息.工位编号,
                    machineStatusCode = 设备状态匹配(Convert.ToInt16(信号表[信号名称表.读取_设备状态].Value)),
                    totalRunningDuration = 设备实时信息.totalRunningDuration,
                    totalOnLineDuration = 设备实时信息.totalOnLineDuration,
                    updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                bool re = await iotApi.上传设备状态到IOT(工位信息.IOT上传Token, tmp);
                if (re)
                {
                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"上传设备状态到IOT");
                }
                else
                {
                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"上传设备状态到IOT失败");
                }
            }
        }

        private async Task<bool> 初始化工艺参数(CancellationToken cancellationToken)
        {
            //工艺参数信息接口请求体 tmp1 = new()
            //{
            //	factoryCode = 产线信息.工厂编号,
            //	machineCode = 工位信息.设备编码,
            //	requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //	updateTime = ""
            //};
            //工艺参数信息接口返回体? 工艺参数 = await mesApi.工艺参数信息接口(tmp1);
            //if (工艺参数 == null || 工艺参数.code != "000000") return false;
            //var tmp2 = 工艺参数.data;
            //foreach (var tmp3 in tmp2)
            //{
            //	参数字典.Add(tmp3.paramCode, tmp3);
            //}
            foreach (var item in 工位信息.信号连接表)
            {
                if (!string.IsNullOrEmpty(item.参数编码))
                {
                    //参数键值匹配.TryAdd(item.参数编码, item.信号名称);
                    参数字典.Add(new 工艺参数设置()
                    {
                        参数名称 = item.信号名称,
                        参数编码 = item.参数编码,
                        下限值 = item.下限值,
                        上限值 = item.上限值,
                        标准值 = item.标准值
                    });
                }
            }
            日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"工艺参数初始化完成");
            return true;
        }

        private async Task<bool> 初始化物料编码(CancellationToken cancellationToken)
        {
            foreach (var item in 工位信息.信号连接表)
            {
                //小总成物料
                if (item.物料编码 == "1")
                {
                    总成物料.Add(item.信号名称);
                }
                //子物料
                if (item.物料编码 == "2")
                {
                    子物料.Add(item.信号名称);
                }
            }
            return true;
        }

        public override async Task 信号采集(CancellationToken cancellationToken)
        {
            心跳采集_采集();
            校验逻辑_采集();
            await base.信号采集(cancellationToken);
        }

        public override async Task 运行逻辑(CancellationToken cancellationToken)
        {
            心跳写入sw.Start();
            心跳写入();
            心跳采集sw.Start();
            心跳采集();
            校验逻辑();
        }

        private void 心跳采集_采集()
        {
            读取信号.Add(信号表[信号名称表.读取_心跳]);
        }

        private void 校验逻辑_采集()
        {
            读取信号.Add(信号表[信号名称表.读取_PLC状态触发]);
            读取信号.Add(信号表[信号名称表.读取_事件代码]);
            读取信号.Add(信号表[信号名称表.读取_SN]);
            读取信号.Add(信号表[信号名称表.读取_托盘码]);
            读取信号值(信号表[信号名称表.读取_事件代码]);
            if (信号表[信号名称表.读取_事件代码].Value != null)
            {
                switch ((short)信号表[信号名称表.读取_事件代码].Value)
                {
                    case 0:
                        break;
                    case 100:
                        break;
                    case 200:
                        foreach (var item in 参数字典)
                        {
                            读取信号.Add(信号表[item.参数名称]);
                        }
                        break;
                    case 300:
                        foreach (var item in 总成物料)
                        {
                            读取信号.Add(信号表[item]);
                        }
                        foreach (var item in 子物料)
                        {
                            读取信号.Add(信号表[item]);
                        }
                        break;
                    case 400:
                        break;
                    case 500:
                        break;
                }
            }
        }

        private void 心跳写入()
        {
            if (心跳写入sw.ElapsedMilliseconds > 写入心跳时间)
            {
                心跳写入sw.Restart();
                写入心跳状态 = !写入心跳状态;
                信号表[信号名称表.写入_心跳].Value = 写入心跳状态;
                写入信号.Add(信号表[信号名称表.写入_心跳]);
            }
        }

        private void 心跳采集()
        {
            if (信号表.TryGetValue(信号名称表.读取_心跳, out var 信号值) && 信号值?.Value is bool temp)
            {
                if (读取上次心跳状态 != temp)
                {
                    读取上次心跳状态 = temp;
                    心跳状态 = true;
                    心跳采集sw.Restart();
                }
                else if (心跳采集sw.ElapsedMilliseconds > 读取心跳时间)
                {
                    心跳状态 = false;
                }
            }
            else
            {
                心跳状态 = false;
            }
        }

        private string 产品SN = "";
        private string 托盘码 = "";
        private bool 流程控制 = true;
        async void 校验逻辑()
        {
            if (信号表[信号名称表.读取_PLC状态触发].Value != null && 信号表[信号名称表.读取_事件代码].Value != null)
            {
                if ((bool)信号表[信号名称表.读取_PLC状态触发].Value && 流程控制)
                {
                    流程控制 = false;
                    short PLC事件代码 = (short)信号表[信号名称表.读取_事件代码].Value;
                    switch (PLC事件代码)
                    {
                        //复位
                        case 0:
                            日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"LMES复位");
                            break;
                        //进站
                        case 100:
                        //进站，MES生成SN
                        case 500:
                            信号表[信号名称表.写入_MESReady].Value = false;
                            写入信号.Add(信号表[信号名称表.写入_MESReady]);
                            产品SN = 信号表[信号名称表.读取_SN].Value.ToString().Trim('\0');
                            托盘码 = 信号表[信号名称表.读取_托盘码].Value.ToString().Trim('\0');
                            //await Task.Delay(1000);
                            foreach (var item in 物料绑定集合)
                            {
                                var 物料 = 系统参数.物料信息表格.FirstOrDefault(x => x.组件物料名称 == item);
                                if (物料 != null)
                                {
                                    物料.是否已经绑定 = false;
                                }
                            }
                            if (string.IsNullOrEmpty(产品SN) && PLC事件代码 == 100)
                            {
                                信号表[信号名称表.写入_MESCode].Value = (short)101;
                                信号表[信号名称表.写入_错误信息].Value = "产品参数缺失";
                                写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                break;
                            }
                            else if (PLC事件代码 == 500)
                            {
                                if (产线信息.工艺路径.工序字典[工位信息.工位编号] == 产线信息.工艺路径.工序路径[0])
                                {
                                    产品SN = 生成SN();
                                }
                            }
                            //if (产线信息.工艺路径.工序字典[工位信息.工位编号] == 产线信息.工艺路径.工序路径[0] && !string.IsNullOrEmpty(产线信息.产线编号) && 工位信息.工位号 == "1")
                            //{
                            //    await 获取排程信息();
                            //}
                            生产状态 = 产线信息.工艺路径.工艺路径管控(产品SN, 产线信息, 工位信息.工位编号);
                            
                            //if (生产状态 == 生产状态.排程编码不匹配)
                            //{
                            //    信号表[信号名称表.写入_SN].Value = 产品SN;
                            //    信号表[信号名称表.写入_MESCode].Value = (short)101;
                            //    信号表[信号名称表.写入_错误信息].Value = "排程编码不匹配";
                            //    写入信号.Add(信号表[信号名称表.写入_SN]);
                            //    写入信号.Add(信号表[信号名称表.写入_MESCode]);
                            //    写入信号.Add(信号表[信号名称表.写入_错误信息]);
                            //    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}不在当前排程{产线信息.排程编码}中");
                            //    break;
                            //}
                            #region 托盘码与主码验证匹配
                            using (数据库连接 数据库 = new())
                            {
                                var tmp = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                                if (tmp != null)
                                {
                                    tmp.virtualSN = 托盘码;
                                    数据库.SaveChanges();
                                }
                            }
                            //using (数据库连接 数据库 = new())
                            //{
                            //    //当前产品 = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                            //    if (!string.IsNullOrEmpty(托盘码))
                            //    {
                            //        var tmp = 数据库.生产实时信息.FirstOrDefault(x => x.virtualSN == 托盘码);
                            //        if (!系统参数.设置.调试模式)
                            //        {
                            //            if (tmp != null && 产品SN != tmp.snNumber)
                            //            {
                            //                信号表[信号名称表.写入_SN].Value = 产品SN;
                            //                信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)101 : (short)501;
                            //                信号表[信号名称表.写入_错误信息].Value = "托盘重复使用";
                            //                日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"托盘{托盘码}重复使用：已与产品{tmp.snNumber}绑定");
                            //                数据库.异常绑定信息.Add(new()
                            //                {
                            //                    snNumber = tmp.snNumber,
                            //                    exceptionType = "托盘码重复",
                            //                    exceptionCode = 托盘码,
                            //                    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            //                });
                            //                写入信号.Add(信号表[信号名称表.写入_SN]);
                            //                写入信号.Add(信号表[信号名称表.写入_MESCode]);
                            //                写入信号.Add(信号表[信号名称表.写入_错误信息]);
                            //                数据库.SaveChanges();
                            //                break;
                            //            }
                            //            else if (tmp == null)
                            //            {
                            //                tmp = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                            //                if (tmp != null)
                            //                {
                            //                    tmp.virtualSN = 托盘码;
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            tmp = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                            //            if (tmp != null)
                            //            {
                            //                tmp.virtualSN = 托盘码;
                            //            }
                            //        }
                            //        数据库.SaveChanges();
                            //    }
                            //}
                            #endregion
                            switch (生产状态)
                            {
                                case 生产状态.产品上线:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)100 : (short)500;
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}上线");
                                    break;
                                case 生产状态.正常生产:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)100 : (short)500;
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}进站");
                                    break;
                                case 生产状态.不良品生产:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)101 : (short)501;
                                    信号表[信号名称表.写入_错误信息].Value = "不良品进站";
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}不良品进站");
                                    break;
                                case 生产状态.产品下线:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)100 : (short)500;
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}进站");
                                    break;
                                case 生产状态.不良品下线:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)101 : (short)501;
                                    信号表[信号名称表.写入_错误信息].Value = "不良品进站";
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}不良品进站");
                                    break;
                                case 生产状态.工序校验失败:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)101 : (short)501;
                                    信号表[信号名称表.写入_错误信息].Value = "工序校验失败";
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}工序校验失败");
                                    break;
                                case 生产状态.产品未上线:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)101 : (short)501;
                                    信号表[信号名称表.写入_错误信息].Value = "产品未上线";
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}工序校验失败：产品未上线");
                                    break;
                                case 生产状态.生产数量超计划:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)101 : (short)501;
                                    信号表[信号名称表.写入_错误信息].Value = "当前排程在制数量已达计划数量";
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"排程{产线信息.排程编码}在制数量已达计划数量");
                                    break;
                                case 生产状态.重复过站:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)101 : (short)501;
                                    信号表[信号名称表.写入_错误信息].Value = "重复过站";
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}重复过站");
                                    break;
                                case 生产状态.产品已经下线:
                                    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)101 : (short)501;
                                    信号表[信号名称表.写入_错误信息].Value = "当前产品已经下线";
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}已经下线");
                                    break;
                                    //case 生产状态.不良品返工:
                                    //    信号表[信号名称表.写入_MESCode].Value = PLC事件代码 == 100 ? (short)100 : (short)500;
                                    //    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"不良品{产品SN}第{当前产品.reProductCount}次返工");
                                    //    break;
                            }
                            信号表[信号名称表.写入_SN].Value = 产品SN;
                            写入信号.Add(信号表[信号名称表.写入_SN]);
                            写入信号.Add(信号表[信号名称表.写入_MESCode]);
                            if (信号表[信号名称表.写入_错误信息].Value == null)
                            {
                                信号表[信号名称表.写入_错误信息].Value = "";
                            }
                            写入信号.Add(信号表[信号名称表.写入_错误信息]);
                            using (数据库连接 数据库 = new())
                            {
                                var tmp = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                                if (tmp != null)
                                {
                                    tmp.passBeginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    数据库.SaveChanges();
                                }
                            }
                            break;
                        //数据采集
                        case 200:
                            if ((bool)工位信息.是否需要上传参数)
                            {
                                信号表[信号名称表.写入_MESReady].Value = false;
                                写入信号.Add(信号表[信号名称表.写入_MESReady]);
                                产品SN = 信号表[信号名称表.读取_SN].Value.ToString().Trim('\0');
                                托盘码 = 信号表[信号名称表.读取_托盘码].Value.ToString().Trim('\0');
                                if (!SN与托盘码校验(201)) break;
                                日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}准备进行参数上传");
                                生产参数信息.factoryCode = 产线信息.工厂编号;
                                生产参数信息.requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                生产参数信息.data.lineCode = 产线信息.产线编号;
                                生产参数信息.data.materialCode = 产线信息.产品编号;
                                生产参数信息.data.snNumber = 产品SN;
                                生产参数信息.data.stationCode = 工位信息.工位编号;
                                生产参数信息.data.paramTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                List<生产参数> 上传参数列表 = new();
                                foreach (var item in 参数字典)
                                {
                                    // 生产参数-Real
                                    if (item.参数编码.StartsWith("A") || item.参数编码.StartsWith("a"))
                                    {
                                        if (信号表[item.参数名称].Count == 1)
                                        {
                                            上传参数列表.Add(new 生产参数
                                            {
                                                paramCode = item.参数编码,
                                                paramName = item.参数名称,
                                                standardValue = item.标准值,
                                                realValue = (string)信号表[item.参数名称].Value.ToString().Trim('\0'),
                                                paramRange1 = item.下限值,
                                                paramRange2 = item.上限值,
                                                checkResult = 参数检验((string)信号表[item.参数名称].Value.ToString().Trim('\0'), item.标准值, item.下限值, item.上限值)
                                            });
                                        }
                                        else
                                        {
                                            List<float> realValueList = new((float[])信号表[item.参数名称].Value);
                                            int index = 1;
                                            foreach (var realValue in realValueList)
                                            {
                                                上传参数列表.Add(new 生产参数
                                                {
                                                    paramCode = item.参数编码,
                                                    paramName = item.参数名称 + $"_{index}",
                                                    standardValue = item.标准值,
                                                    realValue = realValue.ToString(),
                                                    paramRange1 = item.下限值,
                                                    paramRange2 = item.上限值,
                                                    checkResult = 参数检验(realValue.ToString(), item.标准值, item.下限值, item.上限值)
                                                });
                                                index++;
                                            }
                                        }
                                    }
                                    // 生产结果-Int
                                    else if (item.参数编码.StartsWith("B") || item.参数编码.StartsWith("b"))
                                    {
                                        if (信号表[item.参数名称].Count == 1)
                                        {
                                            上传参数列表.Add(new 生产参数
                                            {
                                                paramCode = item.参数编码,
                                                paramName = item.参数名称,
                                                standardValue = item.标准值,
                                                realValue = (string)信号表[item.参数名称].Value.ToString().Trim('\0') == "1" ? "OK" : "NG",
                                                paramRange1 = item.下限值,
                                                paramRange2 = item.上限值,
                                                checkResult = (string)信号表[item.参数名称].Value.ToString().Trim('\0')
                                            });
                                        }
                                        else
                                        {
                                            List<Int16> realValueList = new((Int16[])信号表[item.参数名称].Value);
                                            int index = 1;
                                            foreach (var realValue in realValueList)
                                            {
                                                上传参数列表.Add(new 生产参数
                                                {
                                                    paramCode = item.参数编码,
                                                    paramName = item.参数名称 + $"_{index}",
                                                    standardValue = item.标准值,
                                                    realValue = realValue == 1 ? "OK" : "NG",
                                                    paramRange1 = item.下限值,
                                                    paramRange2 = item.上限值,
                                                    checkResult = realValue.ToString()
                                                });
                                                index++;
                                            }
                                        }
                                    }
                                    // 有无照片-Int
                                    else if (item.参数编码.StartsWith("C") || item.参数编码.StartsWith("c"))
                                    {
                                        if (信号表[item.参数名称].Count == 1)
                                        {
                                            上传参数列表.Add(new 生产参数
                                            {
                                                paramCode = item.参数编码,
                                                paramName = item.参数名称,
                                                standardValue = item.标准值,
                                                realValue = (string)信号表[item.参数名称].Value.ToString().Trim('\0') == "1" ? "有" : "无",
                                                paramRange1 = item.下限值,
                                                paramRange2 = item.上限值,
                                                checkResult = "1"
                                            });
                                        }
                                        else
                                        {
                                            List<Int16> realValueList = new((Int16[])信号表[item.参数名称].Value);
                                            int index = 1;
                                            foreach (var realValue in realValueList)
                                            {
                                                上传参数列表.Add(new 生产参数
                                                {
                                                    paramCode = item.参数编码,
                                                    paramName = item.参数名称 + $"_{index}",
                                                    standardValue = item.标准值,
                                                    realValue = realValue == 1 ? "有" : "无",
                                                    paramRange1 = item.下限值,
                                                    paramRange2 = item.上限值,
                                                    checkResult = "1"
                                                });
                                                index++;
                                            }
                                        }
                                    }
                                }
                                bool 产品NG = false;
                                using (数据库连接 数据库 = new())
                                {
                                    foreach (var item in 上传参数列表)
                                    {
                                        var tmp = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                                        //if (tmp.isbad == true)
                                        //{
                                        //    tmp.isbad = false;
                                        //    tmp.reProductCount += 1;
                                        //}
                                        if (上传参数列表.Count != 0)
                                        {
                                            if (item.checkResult == "0")
                                            {
                                                tmp.isbad = true;
                                                tmp.ngStation = 工位信息.工位编号;
                                                产品NG = true;
                                                break;
                                            }
                                        }
                                    }
                                    数据库.SaveChanges();
                                }
                                生产参数信息.data.dataList = 上传参数列表;
                                上传生产参数到MES = mesApi.生产参数信息接口(生产参数信息);
                                保存生产参数信息到数据库 = lmesApi.新增生产参数信息(生产参数信息);
                                if (产品NG)
                                {
                                    var data = new 生产不良数据接口请求体()
                                    {
                                        factoryCode = 系统参数.设置.工厂编号,
                                        requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        data = new()
                                        {
                                            lineCode = 产线信息.产线编号,
                                            stationCode = 工位信息.工位编号,
                                            datalist = [
                                                new()
                                                {
                                                    badCode = "ZCBL",
                                                    badQty = "1",
                                                    badFactor = "制程不良",
                                                    editTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                                }]
                                        }
                                    };
                                    Task<bool> task1 = lmesApi.新增不良信息(data);
                                    Task<生产不良数据接口返回体> task2 = mesApi.生产不良数据(data);
                                    await Task.WhenAll(task1, task2).ConfigureAwait(false);
                                    if (task1.Result && task2.Result.code == "000000")
                                    {
                                        日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", "不良数据上传成功");
                                    }
                                    if (task2.Result.code != "000000")
                                    {
                                        日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", "不良数据上传MES失败");
                                    }
                                    if (!task1.Result)
                                    {
                                        日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", "不良数据保存到数据库失败");
                                    }
                                    if (!系统参数.设置.调试模式)
                                    {
                                        日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}NG");
                                        信号表[信号名称表.写入_错误信息].Value = "产品NG";
                                        信号表[信号名称表.写入_MESCode].Value = (short)201;
                                        信号表[信号名称表.写入_SN].Value = 产品SN;
                                        写入信号.Add(信号表[信号名称表.写入_SN]);
                                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                        写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                    }
                                    else
                                    {
                                        信号表[信号名称表.写入_SN].Value = 产品SN;
                                        写入信号.Add(信号表[信号名称表.写入_SN]);
                                        信号表[信号名称表.写入_MESCode].Value = (short)200;
                                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                    }
                                }
                                else
                                {
                                    信号表[信号名称表.写入_SN].Value = 产品SN;
                                    写入信号.Add(信号表[信号名称表.写入_SN]);
                                    信号表[信号名称表.写入_MESCode].Value = (short)200;
                                    写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                }
                                break;
                            }
                            else
                            {
                                日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", "不需要上传参数");
                                信号表[信号名称表.写入_SN].Value = 产品SN;
                                写入信号.Add(信号表[信号名称表.写入_SN]);
                                信号表[信号名称表.写入_MESCode].Value = (short)200;
                                写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                break;
                            }
                        //物料绑定
                        case 300:
                            if ((bool)工位信息.是否需要绑定物料)
                            {
                                信号表[信号名称表.写入_MESReady].Value = false;
                                写入信号.Add(信号表[信号名称表.写入_MESReady]);
                                产品SN = 信号表[信号名称表.读取_SN].Value.ToString().Trim('\0');
                                托盘码 = 信号表[信号名称表.读取_托盘码].Value.ToString().Trim('\0');
                                if (!SN与托盘码校验(301)) break;
                                List<string> 绑定物料列表 = 子物料.Concat(总成物料).ToList();
                                //物料重复绑定查询
                                if (!系统参数.设置.调试模式)
                                {
                                    using (数据库连接 数据库 = new())
                                    {
                                        bool 物料重复绑定 = false;
                                        foreach (var item in 绑定物料列表)
                                        {
                                            string 物料码 = (string)信号表[item].Value.ToString().Trim('\0');
                                            var 物料查询 = 数据库.物料绑定信息数据.FirstOrDefault(x => x.assemblyMaterialCode == 物料码);
                                            if (物料查询 != null)
                                            {
                                                信号表[信号名称表.写入_MESCode].Value = (short)301;
                                                信号表[信号名称表.写入_错误信息].Value = "物料重复使用";
                                                信号表[信号名称表.写入_SN].Value = 产品SN;
                                                日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"物料{物料码}重复使用：已与产品{物料查询.snNumber}绑定");
                                                写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                                写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                                写入信号.Add(信号表[信号名称表.写入_SN]);
                                                数据库.异常绑定信息.Add(new()
                                                {
                                                    snNumber = 物料查询.snNumber,
                                                    exceptionType = "物料码重复",
                                                    exceptionCode = 物料码,
                                                    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                                });
                                                物料重复绑定 = true;
                                            }
                                        }
                                        if (物料重复绑定)
                                        {
                                            数据库.SaveChanges();
                                            break;
                                        }
                                    }
                                }
                                物料绑定集合.Clear();
                                物料绑定信息.factoryCode = 产线信息.工厂编号;
                                物料绑定信息.lineCode = 产线信息.产线编号;
                                物料绑定信息.stationCode = 工位信息.工位编号;
                                物料绑定信息.snNumber = 产品SN;
                                物料绑定信息.materialCode = 产线信息.产品编号;
                                物料绑定信息.requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                List<物料绑定信息接口请求体Data> 上传物料列表 = new();
                                foreach (var item in 绑定物料列表)
                                {
                                    上传物料列表.Add(
                                    new 物料绑定信息接口请求体Data
                                    {
                                        lineCode = 产线信息.产线编号,
                                        stationCode = 工位信息.工位编号,
                                        snNumber = 产品SN,
                                        assemblyMaterialCode = (string)信号表[item].Value.ToString().Trim('\0'),
                                        assemblyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        assemblyMaterialName = item,
                                        assemblyMaterialSn = (string)信号表[item].Value.ToString().Trim('\0'),
                                    });
                                }
                                //物料匹配
                                using (数据库连接 数据库 = new())
                                {
                                    var 物料名称信息 = 数据库.物料名称信息.ToList();
                                    int 物料匹配数 = 0;
                                    string 错误信息 = "";
                                    foreach (var item in 上传物料列表)
                                    {
                                        if (总成物料.Contains(item.assemblyMaterialName))
                                        {
                                            var 总成物料加工数据 = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == item.assemblyMaterialCode);
                                            if (!系统参数.设置.调试模式)
                                            {
                                                if (总成物料加工数据 == null)
                                                {
                                                    错误信息 = "未找到合装产品信息";
                                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"不存在条码为：{item.assemblyMaterialCode}的加工产品");
                                                    break;
                                                }
                                                else if ((bool)总成物料加工数据.isbad)
                                                {
                                                    错误信息 = "合装产品NG";
                                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{item.assemblyMaterialCode}NG，不允许合装");
                                                    break;
                                                }
                                                else if (!string.IsNullOrEmpty(总成物料加工数据.stationCode))
                                                {
                                                    错误信息 = "合装产品未加工完成";
                                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{item.assemblyMaterialCode}未产出，不允许合装");
                                                    break;
                                                }
                                            }
                                        }
                                        string 物料名称 = RemoveTrailingUnderscoreAndDigits(item.assemblyMaterialName);
                                        var 目标物料 = 物料名称信息.FirstOrDefault(x => x.物料名称 == 物料名称);
                                        if (目标物料 == null)
                                        {
                                            日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"不存在名称为：{物料名称}的物料");
                                            break;
                                        }
                                        if ((bool)目标物料.是否使用)
                                        {
                                            if (Regex.IsMatch(item.assemblyMaterialCode, 目标物料.正则表达式))
                                            {
                                                物料匹配数++;
                                                var 物料 = 系统参数.物料信息表格.FirstOrDefault(x => x.组件物料名称 == item.assemblyMaterialName && x.是否已经绑定 == false);
                                                if (物料 != null)
                                                {
                                                    物料.是否已经绑定 = true;
                                                }
                                                物料绑定集合.Add(目标物料.物料名称);
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(错误信息))
                                    {
                                        信号表[信号名称表.写入_错误信息].Value = 错误信息;
                                        信号表[信号名称表.写入_MESCode].Value = (short)301;
                                        信号表[信号名称表.写入_SN].Value = 产品SN;
                                        写入信号.Add(信号表[信号名称表.写入_SN]);
                                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                        写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                        break;
                                    }
                                    if (物料匹配数 != 上传物料列表.Count && !系统参数.设置.调试模式)
                                    {
                                        日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", "物料匹配失败");
                                        信号表[信号名称表.写入_错误信息].Value = "物料匹配失败";
                                        信号表[信号名称表.写入_MESCode].Value = (short)301;
                                        信号表[信号名称表.写入_SN].Value = 产品SN;
                                        写入信号.Add(信号表[信号名称表.写入_SN]);
                                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                        写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                        break;
                                    }
                                    else
                                    {
                                        子物料绑定 = true;
                                    }
                                    if (子物料绑定)
                                    {
                                        日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", "物料匹配成功");
                                        物料绑定信息.data = 上传物料列表;
                                        上传物料绑定信息到MES = mesApi.物料绑定信息接口(物料绑定信息);
                                        保存物料绑定信息到数据库 = lmesApi.新增物料绑定信息(物料绑定信息);
                                        信号表[信号名称表.写入_SN].Value = 产品SN;
                                        写入信号.Add(信号表[信号名称表.写入_SN]);
                                        信号表[信号名称表.写入_MESCode].Value = (short)300;
                                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                        // 等待1秒钟，这期间界面仍然保持响应
                                        //await Task.Delay(1000);
                                        //foreach (var item in 物料绑定集合)
                                        //{
                                        //    var 物料 = 系统参数.物料信息表格.FirstOrDefault(x => x.组件物料名称 == item);
                                        //    if (物料 != null)
                                        //    {
                                        //        物料.是否已经绑定 = false;
                                        //    }
                                        //}
                                        物料绑定集合.Clear();
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", "不需要绑定物料");
                                信号表[信号名称表.写入_MESCode].Value = (short)300;
                                信号表[信号名称表.写入_SN].Value = 产品SN;
                                写入信号.Add(信号表[信号名称表.写入_SN]);
                                写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                break;
                            }
                        //出站
                        case 400:
                            信号表[信号名称表.写入_MESReady].Value = false;
                            写入信号.Add(信号表[信号名称表.写入_MESReady]);
                            产品SN = 信号表[信号名称表.读取_SN].Value.ToString().Trim('\0');
                            托盘码 = 信号表[信号名称表.读取_托盘码].Value.ToString().Trim('\0');
                            生产状态 = 产线信息.工艺路径.工艺路径管控(产品SN, 产线信息, 工位信息.工位编号);
                            if (系统参数.设置.调试模式)
                            {
                                switch (生产状态)
                                {
                                    case 生产状态.产品未上线:
                                    case 生产状态.不良品生产:
                                    case 生产状态.工序校验失败:
                                    case 生产状态.生产数量超计划:
                                    case 生产状态.重复过站:
                                    case 生产状态.产品已经下线:
                                        //case 生产状态.排程编码不匹配:
                                        生产状态 = 生产状态.正常生产;
                                        break;
                                    case 生产状态.不良品下线:
                                        生产状态 = 生产状态.产品下线;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (!SN与托盘码校验(401)) break;
                            using (数据库连接 数据库 = new())
                            {
                                var tmp = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                                if (tmp != null)
                                {
                                    tmp.passEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    if (生产状态 == 生产状态.产品上线 || 生产状态 == 生产状态.正常生产 || 生产状态 == 生产状态.产品下线 || tmp.ngStation == 工位信息.工位编号/* || 生产状态 == 生产状态.不良品返工*/)
                                    {
                                        生产过站信息.factoryCode = 产线信息.工厂编号;
                                        生产过站信息.lineCode = 产线信息.产线编号;
                                        生产过站信息.snNumber = 产品SN;
                                        生产过站信息.materialCode = tmp.materialCode;
                                        生产过站信息.stationCode = 工位信息.工位编号;
                                        生产过站信息.workOrderNumber = 产线信息.工单编码;
                                        生产过站信息.scheduleNumber = tmp.scheduleCode;
                                        生产过站信息.trayNumber = 托盘码;
                                        生产过站信息.reqType = "2";
                                        生产过站信息.startStationCode = 产线信息.工艺路径.工序字典[工位信息.工位编号] == 产线信息.工艺路径.工序路径[0] ? "1" : "2";
                                        生产过站信息.state = "pass";
                                        生产过站信息.userId = 工位信息.工号;
                                        生产过站信息.requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        生产过站信息.passBeginTime = tmp.passBeginTime;
                                        生产过站信息.passEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        上传过站信息到MES = mesApi.生产过站信息接口(生产过站信息);
                                        保存过站信息到数据库 = lmesApi.新增过站信息(生产过站信息);
                                    }
                                    if (生产状态 == 生产状态.产品下线 || 生产状态 == 生产状态.不良品下线)
                                    {
                                        tmp.stationCode = "";
                                        //tmp.virtualSN = "";
                                        tmp.updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    else
                                    {
                                        tmp.stationCode = 工位信息.工位编号;
                                        tmp.updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                }
                                数据库.SaveChanges();
                            }
                            日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}出站");
                            if (系统参数.设置.调试模式)
                            {
                                信号表[信号名称表.写入_SN].Value = 产品SN;
                                写入信号.Add(信号表[信号名称表.写入_SN]);
                                信号表[信号名称表.写入_MESCode].Value = (short)400;
                                写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                break;
                            }
                            await Task.WhenAll([保存生产参数信息到数据库, 保存过站信息到数据库, 保存物料绑定信息到数据库]);
                            //while (!上传生产参数到MES.IsCompleted || !上传过站信息到MES.IsCompleted
                            //    || !保存生产参数信息到数据库.IsCompleted || !保存过站信息到数据库.IsCompleted
                            //    || !上传物料绑定信息到MES.IsCompleted || !保存物料绑定信息到数据库.IsCompleted)
                            //{
                            //    await Task.Delay(100);
                            //}
                            try
                            {
                                if ((bool)工位信息.是否需要上传参数)
                                {
                                    if ((生产状态 == 生产状态.产品上线 || 生产状态 == 生产状态.正常生产 || 生产状态 == 生产状态.产品下线
                                    /*|| 生产状态 == 生产状态.不良品返工*/) && 上传生产参数到MES.Result.code != "000000")
                                    {
                                        日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}上传生产参数到MES失败：{上传生产参数到MES.Result.mesg}");
                                        信号表[信号名称表.写入_MESCode].Value = (short)400;
                                        信号表[信号名称表.写入_错误信息].Value = "上传生产参数信息失败";
                                        信号表[信号名称表.写入_SN].Value = 产品SN;
                                        写入信号.Add(信号表[信号名称表.写入_SN]);
                                        写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                        break;
                                    }
                                }
                                if ((bool)工位信息.是否需要绑定物料)
                                {
                                    if ((生产状态 == 生产状态.产品上线 || 生产状态 == 生产状态.正常生产 || 生产状态 == 生产状态.产品下线
                                    /*|| 生产状态 == 生产状态.不良品返工*/) && (上传物料绑定信息到MES.Result == null || 上传物料绑定信息到MES.Result.code != "000000"))
                                    {
                                        日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}上传物料绑定信息到MES失败：{上传物料绑定信息到MES.Result.mesg}");
                                        信号表[信号名称表.写入_MESCode].Value = (short)400;
                                        信号表[信号名称表.写入_错误信息].Value = "上传物料绑定信息失败";
                                        信号表[信号名称表.写入_SN].Value = 产品SN;
                                        写入信号.Add(信号表[信号名称表.写入_SN]);
                                        写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                        break;
                                    }
                                }
                                if ((生产状态 == 生产状态.产品上线 || 生产状态 == 生产状态.正常生产 || 生产状态 == 生产状态.产品下线
                                        /*|| 生产状态 == 生产状态.不良品返工*/) && 上传过站信息到MES.Result.code != "000000")
                                {
                                    日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"产品{产品SN}上传过站信息到MES失败：{上传过站信息到MES.Result.mesg}");
                                    信号表[信号名称表.写入_MESCode].Value = (short)400;
                                    信号表[信号名称表.写入_错误信息].Value = "上传过站信息失败";
                                    信号表[信号名称表.写入_SN].Value = 产品SN;
                                    写入信号.Add(信号表[信号名称表.写入_SN]);
                                    写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                    写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                    break;
                                }
                                信号表[信号名称表.写入_SN].Value = 产品SN;
                                写入信号.Add(信号表[信号名称表.写入_SN]);
                                信号表[信号名称表.写入_MESCode].Value = (short)400;
                                写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                break;
                            }
                            catch (Exception ex)
                            {
                                信号表[信号名称表.写入_MESCode].Value = (short)400;
                                信号表[信号名称表.写入_SN].Value = 产品SN;
                                信号表[信号名称表.写入_错误信息].Value = "上传数据到MES失败";
                                写入信号.Add(信号表[信号名称表.写入_MESCode]);
                                写入信号.Add(信号表[信号名称表.写入_SN]);
                                写入信号.Add(信号表[信号名称表.写入_错误信息]);
                                日志写入.写入($"{工位信息.工位编号}_{工位信息.工位号}", $"上传信息到MES失败：{ex.Message}");
                                break;
                            }
                        //错误代码
                        default:
                            日志写入.写入($"PLC事件代码接收错误：未知的代码：{(short)信号表[信号名称表.读取_事件代码].Value}");
                            break;
                    }
                }
                else if (!(bool)信号表[信号名称表.读取_PLC状态触发].Value)
                {
                    信号表[信号名称表.写入_MESReady].Value = true;
                    信号表[信号名称表.写入_SN].Value = "";
                    信号表[信号名称表.写入_MESCode].Value = Convert.ToInt16(0);
                    信号表[信号名称表.写入_错误信息].Value = "";

                    写入信号.Add(信号表[信号名称表.写入_MESReady]);
                    写入信号.Add(信号表[信号名称表.写入_SN]);
                    写入信号.Add(信号表[信号名称表.写入_MESCode]);
                    写入信号.Add(信号表[信号名称表.写入_错误信息]);
                    流程控制 = true;
                }
            }
        }

        private bool SN与托盘码校验(int MES错误代码)
        {
            if (系统参数.设置.调试模式)
            {
                return true;
            }
            using (数据库连接 数据库 = new())
            {
                if (string.IsNullOrEmpty(产品SN) && !string.IsNullOrEmpty(托盘码))
                {
                    var tmp = 数据库.生产实时信息.FirstOrDefault(x => x.virtualSN == 托盘码);
                    if (tmp == null)
                    {
                        信号表[信号名称表.写入_错误信息].Value = "托盘码未投入使用";
                        信号表[信号名称表.写入_MESCode].Value = (short)MES错误代码;
                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                        写入信号.Add(信号表[信号名称表.写入_错误信息]);
                        return false;
                    }
                    else
                    {
                        产品SN = tmp.snNumber;
                        return true;
                    }
                }
                else
                {
                    var tmp = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                    if (tmp == null)
                    {
                        信号表[信号名称表.写入_错误信息].Value = "产品未上线";
                        信号表[信号名称表.写入_MESCode].Value = (short)MES错误代码;
                        写入信号.Add(信号表[信号名称表.写入_MESCode]);
                        写入信号.Add(信号表[信号名称表.写入_错误信息]);
                        return false;
                    }
                    //else if (tmp.virtualSN != 托盘码 && !string.IsNullOrEmpty(托盘码))
                    //{
                    //    信号表[信号名称表.写入_错误信息].Value = "产品SN与托盘码不匹配";
                    //    信号表[信号名称表.写入_MESCode].Value = (short)MES错误代码;
                    //    写入信号.Add(信号表[信号名称表.写入_MESCode]);
                    //    写入信号.Add(信号表[信号名称表.写入_错误信息]);
                    //    return false;
                    //}
                    return true;
                }
            }
        }

        private string 生成SN()
        {
            return "new SN";
        }

        private string 设备状态匹配(int 设备状态代码)
        {
            return 设备状态代码 switch
            {
                0 => "offline",        // 离线
                1 => "normal",         // 正常
                2 => "repair",         // 报修
                3 => "maintain",       // 维修
                4 => "scrap",          // 报废
                5 => "onMachine",      // 上机
                6 => "inMaintain",     // 保养
                7 => "inLibrary",      // 在库
                8 => "free",           // 空闲
                9 => "fault",          // 故障
                10 => "overhaul",      // 检修
                11 => "mothballed",    // 封存
                12 => "toBeScrap",     // 待报废
                13 => "loss",          // 遗失
                14 => "inUse",         // 使用中
                _ => "未知状态",        // 未知状态
            };
        }

        private static string 参数检验(string? realValue, string? standardValue, string? standardRange1, string? standardRange2)
        {
            if (!string.IsNullOrEmpty(standardRange1) && !string.IsNullOrEmpty(standardRange1))
            {
                if (float.Parse(realValue) >= float.Parse(standardRange1) && float.Parse(realValue) <= float.Parse(standardRange2)) return "1";
                else return "0";
            }

            if (!string.IsNullOrEmpty(standardValue))
            {
                if (realValue == standardValue) return "1";
                return "0";
            }

            else return "1";
        }
        private static string RemoveTrailingUnderscoreAndDigits(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            int i = input.Length - 1;
            for (; i >= 0; i--)
            {
                char c = input[i];
                if (c != '_' && !char.IsDigit(c))
                {
                    break;
                }
            }
            return i < 0 ? string.Empty : input.Substring(0, i + 1);
        }
    }
}
