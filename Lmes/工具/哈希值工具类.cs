using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Lmes.工具
{
    public static class 哈希值工具类
    {
        public static string 计算哈希值(this string 字符串)
        {

            // 生成一个128位的盐
            //byte[] salt = new byte[16];
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(salt);
            //}
            var salt = Encoding.UTF8.GetBytes("bydqaq6589jk");
            // 使用PBKDF2算法进行哈希，指定较高的迭代次数和较新的哈希算法（例如 SHA256）
            var pbkdf2 = new Rfc2898DeriveBytes(字符串, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 哈希结果长度为32字节（256位）

            // 将盐和哈希组合成一个字节数组
            byte[] hashBytes = new byte[32 + salt.Length]; // 盐长度16字节 + 哈希结果长度32字节
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, 32);

            // 将组合后的字节数组转换成Base64字符串
            return Convert.ToBase64String(hashBytes);

        }
        public static bool 验证哈希值(this string 字符串, string 哈希值)
        {
            // 获取存储的哈希值的字节数组
            byte[] hashBytes = Convert.FromBase64String(哈希值);

            var salt = Encoding.UTF8.GetBytes("bydqaq6589jk");

            // 使用提取出的盐、指定的迭代次数和哈希算法重新生成哈希
            var pbkdf2 = new Rfc2898DeriveBytes(字符串, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 哈希结果长度为32字节（256位）

            // 比较生成的哈希值和存储的哈希值
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }
    }
}
