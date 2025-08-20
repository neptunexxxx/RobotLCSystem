using AutoMapper;
using Azure;
using DocumentFormat.OpenXml.Drawing;
using EFDbContext.Entity;
using Lmes.功能.数据类型请求体;
using Lmes.功能.枚举;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace Lmes.功能
{
	public class LmesApi
	{
		public readonly HttpClient httpClient = new();
		public LmesApi(string 基地址)
		{
			httpClient.BaseAddress = new Uri(基地址);
		}

		public async Task<List<PassStation>?> 查询过站信息
			(string? 产品SN = null, string? 工单编码 = null, string? 排程编码 = null, string? 工位编号 = null,
			 string? 用户ID = null, string? 筛选时间1 = null, string? 筛选时间2 = null)
		{
			var re = await httpClient.GetAsync($"/api/生产过站信息/查询?产品SN={Uri.EscapeDataString(产品SN ?? string.Empty)}&工单编码={Uri.EscapeDataString(工单编码 ?? string.Empty)}" +
				$"&排程编码={Uri.EscapeDataString(排程编码 ?? string.Empty)}&工位编号={Uri.EscapeDataString(工位编号 ?? string.Empty)}&用户ID={Uri.EscapeDataString(用户ID ?? string.Empty)}&筛选时间1={Uri.EscapeDataString(筛选时间1 ?? string.Empty)}&筛选时间2={Uri.EscapeDataString(筛选时间2 ?? string.Empty)}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<PassStation>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}

		public async Task<bool> 新增过站信息(生产过站信息接口请求体 data)
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<生产过站信息接口请求体, PassStation>();
			});
			var mapper = config.CreateMapper();
			PassStation entity = mapper.Map<PassStation>(data);
			entity.Id = Guid.NewGuid();
			var re = await httpClient.PostAsJsonAsync("/api/生产过站信息/新增", entity);
			try
			{
				re.EnsureSuccessStatusCode();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> 删除过站信息(string 产品SN)
		{
			var re = await httpClient.DeleteAsync($"/api/生产过站信息/删除/?产品SN={产品SN}");
			try
			{
				re.EnsureSuccessStatusCode();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<List<ProductParameters>?> 查询生产参数信息
			(string? 产品SN = null, string? 设备编码 = null, string? 工位编号 = null,
			string? 筛选时间1 = null, string? 筛选时间2 = null)
		{
			var re = await httpClient.GetAsync($"api/生产参数信息/查询?产品SN={Uri.EscapeDataString(产品SN ?? string.Empty)}&设备编码={Uri.EscapeDataString(设备编码 ?? string.Empty)}&工位编号={Uri.EscapeDataString(工位编号 ?? string.Empty)}&筛选时间1={Uri.EscapeDataString(筛选时间1 ?? string.Empty)}&筛选时间2={Uri.EscapeDataString(筛选时间2 ?? string.Empty)}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<ProductParameters>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}

		public async Task<List<ProductParametersData>?> 查询生产参数信息数据(Guid? dataId)
		{
			var re = await httpClient.GetAsync($"api/生产参数信息数据/查询?dataId={dataId}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<ProductParametersData>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}
		public async Task<bool> 新增生产参数信息(生产参数信息接口请求体 data)
		{
			var config1 = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<生产参数信息请求体Data, ProductParameters>();
			});
			var mapper1 = config1.CreateMapper();
			ProductParameters entity1 = mapper1.Map<ProductParameters>(data.data);
			var dataId = Guid.NewGuid();
			entity1.Id = Guid.NewGuid();
			entity1.dataId = dataId;
			var config2 = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<生产参数, ProductParametersData>();
			});
			var mapper2 = config2.CreateMapper();
			List<ProductParametersData> entity2 = new();
			foreach (var item in data.data.dataList)
			{
				var tmp = mapper2.Map<ProductParametersData>(item);
				tmp.Id = Guid.NewGuid();
				tmp.dataId = dataId;
				entity2.Add(tmp);
			}
			var re1 = await httpClient.PostAsJsonAsync($"api/生产参数信息/新增", entity1);
			var re2 = await httpClient.PostAsJsonAsync($"api/生产参数信息数据/新增", entity2);
			try
			{
				re1.EnsureSuccessStatusCode();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		#region 设备状态信息

		public async Task<bool> 新增设备状态信息(设备状态信息接口请求体 data)
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<设备状态信息请求参数, EquipmentStatus>();
			});
			var mapper = config.CreateMapper();
			try
			{
				foreach (var item in data.data)
				{
					EquipmentStatus entity = mapper.Map<EquipmentStatus>(item);
					entity.Id = Guid.NewGuid();
					var re = await httpClient.PostAsJsonAsync("/api/设备状态信息/新增", entity);

					re.EnsureSuccessStatusCode();
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<List<EquipmentStatus>?> 查询设备状态信息(string? 设备编码 = null, string? 产线编码 = null, string? 工位编号 = null, string? 设备状态编码 = null, string? 开始时间 = null, string? 结束时间 = null)
		{
			var re = await httpClient.GetAsync($"api/设备状态信息/查询?设备编码={Uri.EscapeDataString(设备编码 ?? string.Empty)}&产线编码={Uri.EscapeDataString(产线编码 ?? string.Empty)}&工位编号={Uri.EscapeDataString(工位编号 ?? string.Empty)}&设备状态编码={Uri.EscapeDataString(设备状态编码 ?? string.Empty)}&开始时间={Uri.EscapeDataString(开始时间 ?? string.Empty)}&结束时间={Uri.EscapeDataString(结束时间 ?? string.Empty)}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<EquipmentStatus>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}
		#endregion

		#region 设备稼动时长数采信息
		public async Task<bool> 新增设备稼动时长数采信息(设备稼动时长数采接口请求体 data)
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<设备稼动时长数采接口请求体, DeviceStatusAndDuration>();
			});
			var mapper = config.CreateMapper();
			DeviceStatusAndDuration entity = mapper.Map<DeviceStatusAndDuration>(data);
			entity.Id = Guid.NewGuid();
			var re = await httpClient.PostAsJsonAsync("/api/设备稼动时长数采信息/新增", entity);
			try
			{
				re.EnsureSuccessStatusCode();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<List<DeviceStatusAndDuration>?> 查询设备稼动时长数采信息(string? 设备编码 = null, string? 设备状态编码 = null, string? 数采设备编码 = null)
		{
			var re = await httpClient.GetAsync($"api/设备稼动时长数采信息/查询?设备编码={Uri.EscapeDataString(设备编码 ?? string.Empty)}&设备状态编码={Uri.EscapeDataString(设备状态编码 ?? string.Empty)}&数采设备编码={Uri.EscapeDataString(数采设备编码 ?? string.Empty)}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<DeviceStatusAndDuration>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}
		#endregion

		#region 查询预警信息
		public async Task<List<WarningData>?> 查询预警信息(string? 工位编码 = null, string? 设备编码 = null, string? 筛选时间1 = null, string? 筛选时间2 = null)
		{
			var re = await httpClient.GetAsync($"api/预警信息/查询?工位编码={Uri.EscapeDataString(工位编码 ?? string.Empty)}&设备编码={Uri.EscapeDataString(设备编码 ?? string.Empty)}&筛选时间1={Uri.EscapeDataString(筛选时间1 ?? string.Empty)}&筛选时间2={Uri.EscapeDataString(筛选时间2 ?? string.Empty)}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<WarningData>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}
		#endregion


		public async Task 上传文件(string 要上传的文件路径, string 上传文件夹)
		{
			string filePath = 要上传的文件路径;
			string fileName = Path.GetFileName(要上传的文件路径);

			using var form = new MultipartFormDataContent();
			// 读取文件内容
			var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
			fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

			// 添加文件数据到请求
			form.Add(fileContent, "file", fileName);

			// 发送 POST 请求到 API
			var response = await httpClient.PostAsync($"质量数据/upload?路径={Uri.EscapeDataString(上传文件夹)}", form);

			response.EnsureSuccessStatusCode();
		}

		#region 物料
		public async Task<List<MaterialBind>?> 查询物料绑定信息(string? 产品SN = null, string? 工位编号 = null, string? 工单编号 = null, string? 单体组件物料编码 = null, string? 筛选时间1 = null, string? 筛选时间2 = null)
		{
			var re = await httpClient.GetAsync($"api/物料绑定信息/查询?产品SN={Uri.EscapeDataString(产品SN ?? string.Empty)}&工位编号={Uri.EscapeDataString(工位编号 ?? string.Empty)}&工单编号={Uri.EscapeDataString(工单编号 ?? string.Empty)}&单体组件物料编码={Uri.EscapeDataString(单体组件物料编码 ?? string.Empty)}&筛选时间1={Uri.EscapeDataString(筛选时间1 ?? string.Empty)}&筛选时间2={Uri.EscapeDataString(筛选时间2 ?? string.Empty)}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<MaterialBind>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}
        public async Task<List<MaterialBind>?> 查询物料绑定信息(Guid? dataId)
        {
            var re = await httpClient.GetAsync($"api/物料绑定信息/查询?dataId={dataId}");
            try
            {
                re.EnsureSuccessStatusCode();
                return await re.Content.ReadFromJsonAsync<List<MaterialBind>>();
            }
            catch (Exception ex)
            {
                throw (new Exception("查询失败：" + ex.Message));
            }
        }
        public async Task<List<MaterialBindData>?> 查询物料绑定信息数据(Guid? dataId)
        {
            var re = await httpClient.GetAsync($"api/物料绑定信息数据/查询?dataId={dataId}");
            try
            {
                re.EnsureSuccessStatusCode();
                return await re.Content.ReadFromJsonAsync<List<MaterialBindData>>();
            }
            catch (Exception ex)
            {
                throw (new Exception("查询失败：" + ex.Message));
            }
        }
        public async Task<List<MaterialBindData>?> 查询物料绑定信息数据(string? assemblyMaterialCode)
		{
			var re = await httpClient.GetAsync($"api/物料绑定信息数据/物料查询?assemblyMaterialCode={assemblyMaterialCode}");
            if (re.StatusCode == HttpStatusCode.NotFound)
            {
                // 明确处理404：返回空列表表示无数据
                return new List<MaterialBindData>();
            }
            try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<MaterialBindData>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}
        public async Task<bool> 新增物料绑定信息(物料绑定信息接口请求体 data)
		{
			var config1 = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<物料绑定信息接口请求体, MaterialBind>();
			});
			var mapper = config1.CreateMapper();
			var dataId = Guid.NewGuid();
			MaterialBind entity1 = mapper.Map<MaterialBind>(data);
			entity1.Id = Guid.NewGuid();
			entity1.dataId = dataId;
			var config2 = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<物料绑定信息接口请求体Data, MaterialBindData>();
			});
			var mapper2 = config2.CreateMapper();
			List<MaterialBindData> entity2 = new();
			foreach (var item in data.data)
			{
				var tmp = mapper2.Map<MaterialBindData>(item);
				tmp.Id = Guid.NewGuid();
				tmp.dataId = dataId;
				entity2.Add(tmp);
			}
			var re1 = await httpClient.PostAsJsonAsync($"api/物料绑定信息/新增", entity1);
			var re2 = await httpClient.PostAsJsonAsync($"api/物料绑定信息数据/新增", entity2);
			try
			{
				re1.EnsureSuccessStatusCode();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// 删除子物料绑定数据
		/// </summary>
		/// <param name="id">子物料绑定数据ID</param>
		/// <returns>是否删除成功</returns>
		public async Task<bool> 删除物料绑定数据(Guid? id)
		{
			try
			{
				string api = $"/api/物料绑定信息数据/删除?id={id}";
				var response = await httpClient.DeleteAsync(api);

				if (response.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"删除物料绑定数据失败: {errorContent}");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"删除物料绑定数据异常: {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// 删除主物料绑定信息及其所有子物料
		/// </summary>
		/// <param name="snNumber">产品SN</param>
		/// <returns>是否删除成功</returns>
		/// <summary>
		/// 删除主物料绑定信息及其所有子物料
		/// </summary>
		/// <param name="snNumber">产品SN</param>
		/// <returns>是否删除成功</returns>
		public async Task<bool> 删除物料绑定信息(string snNumber)
		{
			try
			{
				string api = $"/api/物料绑定信息/删除?snNumber={Uri.EscapeDataString(snNumber)}";
				var response = await httpClient.DeleteAsync(api);

				if (response.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"删除物料绑定信息失败: {errorContent}");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"删除物料绑定信息异常: {ex.Message}");
				return false;
			}
		}

		#endregion

		#region 不良信息

		public async Task<List<BadInformationData>?> 查询不良信息(string? 产品SN, string? 工序编码, string? 工位编号, string? 筛选时间1, string? 筛选时间2)
		{
			var re = await httpClient.GetAsync($"api/生产不良信息/查询?产品SN={Uri.EscapeDataString(产品SN ?? string.Empty)}&工序编码={Uri.EscapeDataString(工序编码 ?? string.Empty)}&工位编号={Uri.EscapeDataString(工位编号 ?? string.Empty)}&筛选时间1={Uri.EscapeDataString(筛选时间1 ?? string.Empty)}&筛选时间2={Uri.EscapeDataString(筛选时间2 ?? string.Empty)}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<BadInformationData>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}

		public async Task<List<BadInformationDataList>?> 查询不良信息数据(Guid? dataId)
		{
			var re = await httpClient.GetAsync($"api/生产不良信息数据/查询?dataId={dataId}");
			try
			{
				re.EnsureSuccessStatusCode();
				return await re.Content.ReadFromJsonAsync<List<BadInformationDataList>>();
			}
			catch (Exception ex)
			{
				throw (new Exception("查询失败：" + ex.Message));
			}
		}

		public async Task<bool> 新增不良信息(生产不良数据接口请求体 生产不良信息)
		{
			var config1 = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<不良数据请求参数, BadInformationData>();
			});
			var mapper1 = config1.CreateMapper();
			BadInformationData entity1 = mapper1.Map<BadInformationData>(生产不良信息.data);
			var dataId = Guid.NewGuid();
			entity1.Id = Guid.NewGuid();
			entity1.datalistId = dataId;
			var config2 = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<不良数据参数, BadInformationDataList>();
			});
			var mapper2 = config2.CreateMapper();
			List<BadInformationDataList> entity2 = new();
			foreach (var item in 生产不良信息.data.datalist)
			{
				var tmp = mapper2.Map<BadInformationDataList>(item);
				tmp.Id = Guid.NewGuid();
				tmp.datalistId = dataId;
				entity2.Add(tmp);
			}
			var re1 = await httpClient.PostAsJsonAsync($"api/生产不良信息/新增", entity1);
			var re2 = await httpClient.PostAsJsonAsync($"api/生产不良信息数据/新增", entity2);
			try
			{
				re1.EnsureSuccessStatusCode();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		#endregion

		#region 更新程序
		public async Task<string> 更新客户端程序()
		{
			try
			{
				// 获取当前版本信息（从程序集或配置文件中读取）
				var versionManager = new VersionManager();
				var currentVersion = versionManager.GetCurrentVersion();

				// 检查最新版本
				var latestVersion = await 获取最新版本();
				if (latestVersion == null || latestVersion.Version == currentVersion)
				{
					return currentVersion; // 无需更新
				}

				// 下载新版本
				var downloadPath = await 下载新版本(latestVersion);
				if (string.IsNullOrEmpty(downloadPath))
				{
					throw new Exception("下载更新包失败");
				}

				// 验证下载文件的CRC
				if (!VerifyCRC(downloadPath, latestVersion.CRC))
				{
					throw new Exception("文件校验失败，可能已损坏");
				}

				// 使用新的更新管理器
				var updater = new UpdateManager();
				await updater.ExtractAndUpdate(downloadPath, latestVersion.Version);
				return latestVersion.Version;
			}
			catch (Exception ex)
			{
				// 记录错误日志
				日志写入.写入($"更新失败: {ex.Message}");
				throw;
			}
		}

		private async Task<VersionInfo> 获取最新版本()
		{
			var response = await httpClient.GetAsync("api/自动更新/获取最新版本信息");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			return await response.Content.ReadFromJsonAsync<VersionInfo>();
		}

		private async Task<string> 下载新版本(VersionInfo versionInfo)
		{
			var downloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updates", versionInfo.FileName);

			using var response = await httpClient.GetAsync($"api/自动更新/下载指定版本/{versionInfo.FileName}");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			using var fileStream = new FileStream(downloadPath, FileMode.Create);
			await response.Content.CopyToAsync(fileStream);

			return downloadPath;
		}

		private bool VerifyCRC(string filePath, string expectedCRC)
		{
			using var fs = System.IO.File.OpenRead(filePath);
			using var sha = System.Security.Cryptography.SHA256.Create();
			var hash = sha.ComputeHash(fs);
			var actualCRC = BitConverter.ToString(hash).Replace("-", "");

			return string.Equals(actualCRC, expectedCRC, StringComparison.OrdinalIgnoreCase);
		}
		public class VersionInfo
		{
			public string Version { get; set; }
			public string CRC { get; set; }
			public string FileName { get; set; }
		}
		#endregion
	}
}
