using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDbContext;
using EFDbContext.Entity;
using Lmes.功能.数据类型请求体;
using NPOI.OpenXmlFormats.Dml.Diagram;

namespace Lmes.Models
{
    public class 工艺路径
    {
        public enum 生产状态
        {
            就绪,
            产品上线,
            产品未上线,
            正常生产,
            不良品生产,
            产品下线,
            不良品下线,
            工序校验失败,
            生产数量超计划,
            重复过站,
            产品已经下线,
            排程编码不匹配
            //不良品返工
        }
        public List<string> 工序路径 { get; set; }
        /// <summary>
        /// 存储工位编号对应的工序编码
        /// Key : 工位编号
        /// </summary>
        public Dictionary<string, string> 工序字典 { get; set; }

        public 工艺路径()
        {
            工序路径 = new List<string>();
            工序字典 = new Dictionary<string, string>();
        }
        //public 工艺路径(工单路径数据接口返回体? 工单路径)
        //{
        //    if (工单路径 == null) return;
        //    工序路径 = new List<string>();
        //    工序字典 = new Dictionary<string, string>();
        //    var 当前站 = 工单路径.data.FirstOrDefault(x => x.lastOperationCode == "0");
        //    string 当前工序编码;
        //    string 下一工序编码;
        //    for (int i = 0; i < 工单路径.data.ToList().Count; i++)
        //    {
        //        foreach (var 工位编号 in 当前站.stationCode)
        //        {
        //            工序字典.Add(工位编号, 当前站.operationCode);
        //        }
        //        if (!string.IsNullOrEmpty(当前站.nextOperationCode))
        //        {
        //            当前工序编码 = 当前站.operationCode;
        //            下一工序编码 = 当前站.nextOperationCode;
        //            工序路径.Add(当前工序编码);
        //            当前站 = 工单路径.data.FirstOrDefault(x => x.operationCode == 下一工序编码);
        //        }
        //        else
        //        {
        //            当前工序编码 = 当前站.operationCode;
        //            工序路径.Add(当前工序编码);
        //            break;
        //        }
        //    }
        //}

        public 生产状态 工艺路径管控(string 产品SN, 产线信息类 产线信息, string 工位编号)
        {
            using (数据库连接 数据库 = new())
            {
                var 生产实时信息 = 数据库.生产实时信息.FirstOrDefault(x => x.snNumber == 产品SN);
                if (生产实时信息 == null)
                {
                    int 排程已生产数量 = 数据库.生产实时信息.Count(x => x.scheduleCode == 产线信息.排程编码);
                    if (!string.IsNullOrEmpty(产线信息.产品计划数量))
                    {
                        if (排程已生产数量 >= Convert.ToInt16(产线信息.产品计划数量))
                        {
                            return 生产状态.生产数量超计划;
                        }
                    }
                    if (工序字典[工位编号] != 工序路径[0])
                    {
                        return 生产状态.产品未上线;
                    }
                    数据库.Add(new RealTimeProductInfo()
                    {
                        snNumber = 产品SN,
                        stationCode = "0",
                        scheduleCode = 产线信息.排程编码,
                        operationCode = 工序字典[工位编号],
                        materialCode = 产线信息.产品编号,
                        lineCode = 产线信息.产线编号,
                        isbad = false
                    });
                    数据库.SaveChanges();
                    if (产线信息.工艺路径.工序路径.Count == 1)
                    {
                        return 生产状态.产品下线;
                    }
                    return 生产状态.产品上线;
                }
                //if(!string.IsNullOrEmpty(生产实时信息.scheduleCode) && 生产实时信息.scheduleCode != 产线信息.排程编码)
                //{
                //    return 生产状态.排程编码不匹配;
                //}
                if (生产实时信息.stationCode == "")
                {
                    return 生产状态.产品已经下线;
                }
                if (生产实时信息.isbad == true)
                {
                    if (产线信息.工艺路径.工序路径.Count == 1 || 工序字典[工位编号] == 工序路径[工序路径.Count - 1])
                    {
                        return 生产状态.不良品下线;
                    }
                    return 生产状态.不良品生产;
                }
                //良品
                else
                {
                    if (产线信息.工艺路径.工序路径.Count == 1 || 工序字典[工位编号] == 工序路径[工序路径.Count - 1])
                    {
                        return 生产状态.产品下线;
                    }
                    else if (生产实时信息.stationCode == "0")
                    {
                        if (工序字典[工位编号] == 工序路径[0])
                        {
                            return 生产状态.产品上线;
                        }
                        else
                        {
                            return 生产状态.工序校验失败;
                        }
                    }

                    string 前工序 = 工序字典[生产实时信息.stationCode];
                    string 当前工序 = 工序字典[工位编号];
                    int index = Array.IndexOf(工序路径.ToArray(), 前工序);
                    if (工序路径[index + 1] == 当前工序)
                    {
                        return 生产状态.正常生产;
                    }
                    else
                    {
                        return 生产状态.工序校验失败;
                    }
                }
            }
        }
    }
}
