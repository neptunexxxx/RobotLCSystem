using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lmes.功能
{
    public class 测试项目读取
    {
        public static List<测试项目> 读取测试项(int 序号 = 0)
        {
            if (Directory.Exists(@"./测试项目") == false)
            {
                Directory.CreateDirectory(@"./测试项目");
            }
            if (File.Exists(@$"./测试项目/{序号}.json"))
            {
                return JsonSerializer.Deserialize<List<测试项目>>(File.ReadAllText(@$"./测试项目/{序号}.json"));
            }
            return null;
        }
        public static void 创建测试项(List<测试项目> 项目,int 序号 = 0)
        {
            if (Directory.Exists(@"./测试项目") == false)
            {
                Directory.CreateDirectory(@"./测试项目");
            }
            File.WriteAllText(@"./测试项目",JsonSerializer.Serialize(项目, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
        }
    }
    public class 测试项目
    {
        public string method { get; set; }
        public string project { get; set; }
        public int reorder { get; set; }
        public string standard { get; set; }
        public int 小数位数 { get; set; }
        public string[] 对应字符串 { get; set; }
        public 数据类型_枚举 数据类型 { get; set; }
    }
    public enum 数据类型_枚举
    {
        数值,
        字符串,
    }
}
