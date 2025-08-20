using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能
{
    
    public class PZ码计算
    {
        static readonly Dictionary<string, int> PZ对应表 = new()
        {
            {"0",0 },
            {"1",1},
            {"2",2},
            {"3",3},
            {"4",4},
            {"5",5},
            {"6",6},
            {"7",7 },
            {"8",8 },
            {"9",9},
            {"A",10},
            {"B",11},
            {"C",12},
            {"D",13},
            {"E",14},
            {"F",15},
            {"G",16},
            {"H",17 },
            {"I",18},
            {"J",19 },
            {"K",20},
            {"L",21},
            {"M",22},
            {"N",23},
            {"O",24},
            {"P",25},
            {"Q",26},
            {"R",27},
            {"S",28},
            {"T",29},
            {"U",30},
            {"V",31},
            {"W",32},
            {"X",33 },
            {"Y",34 },
            {"Z",35 },
            {"-",36 },
            {".",37 },
            {" ",38 },
            {"_",38 },
            {"$",39 },
            {"/",40 },
            {"+",41 },
            {"%",42 },
        };
        public static string 计算PZ值(string s)
        {
            long 计算 = 0;
            foreach (char item in s)
            {
                PZ对应表.ContainsKey(item.ToString());
                计算 += PZ对应表[item.ToString()];
            }
            return PZ对应表.FirstOrDefault(s => s.Value == 计算 % 43).Key;
        }
    }
}
