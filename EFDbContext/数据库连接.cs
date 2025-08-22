using EFDbContext.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EFDbContext
{
	public class 数据库连接 : DbContext
	{
		public 数据库连接()
		{

		}

		public 数据库连接(DbContextOptions<数据库连接> options) : base(options)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//optionsBuilder.UseNpgsql("Host=192.168.31.105;Port=5432;Database=postgres;Username=postgres;Password=123456");
			optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123456");
		}
		public DbSet<用户类> 用户 { get; set; }
		public DbSet<系统设置类> 设置 { get; set; }
		public DbSet<SNCreate> 条码生成信息 { get; set; }
		public DbSet<PassStation> 生产过站信息 { get; set; }
		public DbSet<MaterialBind> 物料绑定信息 { get; set; }
		public DbSet<MaterialBindData> 物料绑定信息数据 { get; set; }
		public DbSet<EquipmentStatus> 设备状态信息 { get; set; }
		public DbSet<ProductParameters> 生产参数信息 { get; set; }
		public DbSet<ProductParametersData> 生产参数信息数据 { get; set; }
		public DbSet<MaterialTracePassStation> 物料追溯及生产过站信息 { get; set; }
		public DbSet<MaterialTracePassStationData> 物料追溯及生产过站信息数据 { get; set; }
		public DbSet<Inspection> 品质测试信息 { get; set; }
		public DbSet<InspectionData> 品质测试信息数据 { get; set; }
		public DbSet<BadInformation> 生产不良信息 { get; set; }
		public DbSet<BadInformationData> 生产不良信息数据 { get; set; }
		public DbSet<BadInformationDataList> 生产不良信息数据列表 { get; set; }
		public DbSet<Warning> 预警信息 { get; set; }
		public DbSet<WarningData> 预警信息数据 { get; set; }
		public DbSet<Alarmlog> 报警信息记录 { get; set; }
		public DbSet<AlarmlogData> 报警信息记录数据 { get; set; }
		public DbSet<DeviceStatusAndDuration> 设备稼动时长数据 { get; set; }
		public DbSet<RealTimeProductInfo> 生产实时信息 { get; set; }
		public DbSet<RealTimeEquipmentStatus> 设备实时信息 { get; set; }
		public DbSet<ScheduleInfo> 排程信息 { get; set; }
		public DbSet<StationTempo> 生产节拍信息 { get; set; }
		public DbSet<MaterialBindInfo> 物料名称信息 { get; set; } 
		public DbSet<ExceptionBindingInformation> 异常绑定信息 { get; set; }
	}
}
