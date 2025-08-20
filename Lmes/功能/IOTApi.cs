using Lmes.功能.数据类型请求体;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace Lmes.功能
{
    public class IOTApi
    {
        public readonly HttpClient httpClient = new();
        public IOTApi(string 基地址)
        {
            httpClient.BaseAddress = new Uri(基地址);
        }

        public async Task<bool> 上传设备状态到IOT(string token, 设备状态_IOT请求体 data)
        {
            try
            {
                string jsonString = System.Text.Json.JsonSerializer.Serialize(data);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync($"/api/{token}/telemetry", content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
