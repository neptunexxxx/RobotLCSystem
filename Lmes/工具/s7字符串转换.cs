using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.工具
{
    public static class s7字符串转换
    {
        public static byte[] 转换为s7string(this string str, int 总长度) => 转换为s7string(str, (byte)总长度);
        public static byte[] 转换为s7string(this string str, byte 总长度)
        {
            str = str.Length > 总长度 ? str.Substring(0, 总长度) : str;
            byte[] re = new byte[str.Length + 2];
            Span<byte> temp = new Span<byte>(re);
            temp[0] = 总长度;
            temp[1] = (byte)str.Length;
            Encoding.ASCII.GetBytes(str).CopyTo(temp.Slice(2));
            return re;
        }
        public static byte[] 转换为s7Wstring(this string str, int Wstring容量) => 转换为s7Wstring(str, (short)Wstring容量);
        public static byte[] 转换为s7Wstring(this string str,short Wstring容量)
        {
            str = str.Length > Wstring容量 ? str.Substring(0,Wstring容量) : str;
            byte[] re = new byte[Wstring容量 * 2 + 4];
            Span<byte> temp = new Span<byte>(re);
            WriteInt16BigEndian(temp.Slice(0, 2), Wstring容量);
            WriteInt16BigEndian(temp.Slice(2, 2), (short)str.Length);
            Encoding.BigEndianUnicode.GetBytes(str).CopyTo(temp.Slice(4));
            return re;
        }
        public static string s7string转换为字符串(this byte[] bytes)
        {
            ReadOnlySpan<byte> temp = new(bytes);
            byte 字符串长度 = temp[1];
            if (字符串长度 == 0) return string.Empty;
            if (字符串长度 > bytes.Length - 1) 
                字符串长度 = (byte)(bytes.Length - 2);
            return Encoding.ASCII.GetString(temp.Slice(2, 字符串长度));
        }
        public static string s7Wstring转换为字符串(this byte[] bytes)
        {
            Span<byte> temp = new Span<byte>(bytes);
            byte[] 字符串长度数组 = [bytes[3], bytes[2]];
            int 字符串长度 = BitConverter.ToInt16(字符串长度数组);

            if (字符串长度 == 0) return string.Empty;
            int 字符串最长 = (bytes.Length - 4) / 2;
            if (字符串长度> 字符串最长|| 字符串长度<0) 
                字符串长度 = 字符串最长;
            return Encoding.BigEndianUnicode.GetString(temp.Slice(4, 字符串长度 * 2));
        }
        private static void WriteInt16BigEndian(Span<byte> span, short value)
        {
            span[0] = (byte)(value >> 8); // 高字节
            span[1] = (byte)(value & 0xFF); // 低字节
        }
    }
}
