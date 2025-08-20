using Lmes.全局变量;
using Lmes.功能;
using Lmes.功能.数据类型请求体;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class MesApi
{
	private readonly HttpClient httpClient;

	/// <summary>
	/// 工厂MES API
	/// </summary>
	/// <param name="基地址"></param>
	/// <param name="appId"></param>
	/// <param name="appKey"></param>
	public MesApi(string 基地址, string appId, string appKey)
	{
		httpClient = new HttpClient { BaseAddress = new Uri(基地址) };
		httpClient.DefaultRequestHeaders.Add("appId", appId);
		httpClient.DefaultRequestHeaders.Add("appKey", appKey);
	}



	//private string GenerateSignature(string requestUri, object body)
	//{
	//    // body 参数通过 JsonConvert.SerializeObject 方法被转换为 JSON 字符串。
	//    var bodyString = Newtonsoft.Json.JsonConvert.SerializeObject(body);

	//    // 将 AppId、AppKey、请求的 requestUri 和请求体的 bodyString 拼接成一个字符串 dataToSign，这是待签名的数据。
	//    var dataToSign = $"{AppId}{AppKey}{requestUri}{bodyString}";

	//    // 使用 HMACSHA256 算法计算该拼接字符串 dataToSign 的哈希值。HMACSHA256 使用 AppKey 作为密钥对数据进行哈希处理。
	//    using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(AppKey));
	//    //signatureBytes 存储计算出的哈希字节数组。
	//    var signatureBytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
	//    //通过 BitConverter.ToString 将字节数组转换为十六进制字符串，并使用 Replace("-", "") 移除中间的 -，然后 ToLower() 转为小写，得到最终的签名字符串。
	//    return BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
	//}

	public async Task<T?> PostAsync<T>(string 地址, object body)
	{

		// 发送 POST 请求
		var re = await httpClient.PostAsJsonAsync(地址, body);

		if (!re.IsSuccessStatusCode)
		{
			var error = await re.Content.ReadAsStringAsync();
			throw new Exception($"{re.StatusCode}, {error}");
		}

		return await re.Content.ReadFromJsonAsync<T>();
	}

	public async Task<工单路径数据接口返回体?> 工单路径数据接口(工单路径数据接口请求体 请求数据)
	{

		return await PostAsync<工单路径数据接口返回体>("工单路径数据接口", 请求数据);
	}

	public async Task<工厂模型数据接口返回体?> 工厂模型数据接口(工厂模型数据接口请求体 请求数据)
	{

		return await PostAsync<工厂模型数据接口返回体>("mesapi/工厂模型数据接口", 请求数据);
	}

	public async Task<工单排程信息查询接口返回体?> 工单排程信息查询接口(工单排程信息查询接口请求体 请求数据)
	{

		return await PostAsync<工单排程信息查询接口返回体>("工单排程信息查询接口", 请求数据);
	}

	public async Task<工单BOM数据接口返回体?> 工单BOM数据接口(工单BOM数据接口请求体 请求数据)
	{

		return await PostAsync<工单BOM数据接口返回体>("工单BOM数据接口", 请求数据);
	}

	public async Task<工单物料替代信息接口返回体?> 工单物料替代信息接口(工单物料替代信息接口请求体 请求数据)
	{

		return await PostAsync<工单物料替代信息接口返回体>("mesapi/工单物料替代信息接口", 请求数据);
	}

	public async Task<物料主数据接口返回体?> 物料主数据接口(物料主数据接口请求体 请求数据)
	{

		return await PostAsync<物料主数据接口返回体>("mesapi/物料主数据接口", 请求数据);
	}

	public async Task<工艺参数信息接口返回体?> 工艺参数信息接口(工艺参数信息接口请求体 请求数据)
	{

		return await PostAsync<工艺参数信息接口返回体>("工艺参数信息接口", 请求数据);
	}

	public async Task<工艺文件路径信息接口返回体?> 工艺文件路径信息接口(工艺文件路径信息接口请求体 请求数据)
	{

		return await PostAsync<工艺文件路径信息接口返回体>("mesapi/工艺文件路径信息接口", 请求数据);
	}

	public async Task<工序不良代码信息接口返回体?> 工序不良代码信息接口(工序不良代码信息接口请求体 请求数据)
	{

		return await PostAsync<工序不良代码信息接口返回体>("mesapi/工序不良代码信息接口", 请求数据);
	}

	public async Task<品质检验标准接口返回体?> 品质检验标准接口(品质检验标准接口请求体 请求数据)
	{

		return await PostAsync<品质检验标准接口返回体>("mesapi/品质检验标准接口", 请求数据);
	}

	public async Task<人员资质接口返回体?> 人员资质接口(人员资质接口请求体 请求数据)
	{

		return await PostAsync<人员资质接口返回体>("mesapi/人员资质接口", 请求数据);
	}

	public async Task<人员登录信息接口返回体?> 人员登录信息接口(人员登录信息接口返回体 请求数据)
	{

		return await PostAsync<人员登录信息接口返回体>("mesapi/人员登录信息接口", 请求数据);
	}

	public async Task<条码生成信息接口返回体?> 条码生成信息接口(条码生成信息接口请求体 请求数据)
	{
		return await PostAsync<条码生成信息接口返回体>("条码生成信息接口", 请求数据);
	}
	public async Task<生产过站信息接口返回体?> 生产过站信息接口(生产过站信息接口请求体 请求数据)
	{
		return await PostAsync<生产过站信息接口返回体>("生产过站信息接口", 请求数据);
	}
	public async Task<物料绑定信息接口返回体?> 物料绑定信息接口(物料绑定信息接口请求体 请求数据)
	{
		return await PostAsync<物料绑定信息接口返回体>("物料绑定信息接口", 请求数据);
	}
	public async Task<设备状态信息接口返回体?> 设备状态信息接口(设备状态信息接口请求体 请求数据)
	{
		return await PostAsync<设备状态信息接口返回体>("设备状态信息接口", 请求数据);
	}
    public async Task<设备稼动时长数采接口返回体?> 设备稼动时长数采接口(设备稼动时长数采接口请求体 请求数据)
    {
        return await PostAsync<设备稼动时长数采接口返回体>("设备稼动时长数采接口", 请求数据);
    }
    public async Task<生产参数信息接口返回体?> 生产参数信息接口(生产参数信息接口请求体 请求数据)
	{
		return await PostAsync<生产参数信息接口返回体>("生产参数信息接口", 请求数据);
	}
	public async Task<物料追溯及生产过站接口返回体?> 物料追溯及生产过站(物料追溯及生产过站接口请求体 请求数据)
	{
		return await PostAsync<物料追溯及生产过站接口返回体>("mesapi/物料追溯及生产过站接口", 请求数据);
	}
	public async Task<品质测试数据接口返回体?> 品质测试数据(品质测试数据接口请求体 请求数据)
	{
		return await PostAsync<品质测试数据接口返回体>("mesapi/品质测试数据接口", 请求数据);
	}
	public async Task<生产不良数据接口返回体?> 生产不良数据(生产不良数据接口请求体 请求数据)
	{
		return await PostAsync<生产不良数据接口返回体>("生产不良数据接口", 请求数据);
	}
	public async Task<预警信息接口返回体?> 预警信息(预警信息接口请求体 请求数据)
	{
		return await PostAsync<预警信息接口返回体>("mesapi/预警信息接口", 请求数据);
	}
	public async Task<报警信息记录接口返回体?> 报警信息记录(报警信息记录接口请求体 请求数据)
	{
		return await PostAsync<报警信息记录接口返回体>("mesapi/报警信息记录接口", 请求数据);
	}
}
