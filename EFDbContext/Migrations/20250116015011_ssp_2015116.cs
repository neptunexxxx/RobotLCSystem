using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFDbContext.Migrations
{
    /// <inheritdoc />
    public partial class ssp_2015116 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "设置",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    参数名 = table.Column<string>(type: "text", nullable: true),
                    值 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_设置", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "用户",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    用户名 = table.Column<string>(type: "text", nullable: false),
                    密码 = table.Column<string>(type: "text", nullable: false),
                    权限 = table.Column<string>(type: "text", nullable: true),
                    备注 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_用户", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "alarmlog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    requestTime = table.Column<string>(type: "text", nullable: true),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alarmlog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "alarmlog_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true),
                    alarmId = table.Column<int>(type: "integer", nullable: true),
                    alarmMesg = table.Column<string>(type: "text", nullable: true),
                    alarmState = table.Column<int>(type: "integer", nullable: true),
                    startTime = table.Column<string>(type: "text", nullable: true),
                    endTime = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    machineCode = table.Column<string>(type: "text", nullable: true),
                    createTime = table.Column<string>(type: "text", nullable: true),
                    createBy = table.Column<string>(type: "text", nullable: true),
                    updateTime = table.Column<string>(type: "text", nullable: true),
                    updateBy = table.Column<string>(type: "text", nullable: true),
                    factoryCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alarmlog_data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bad_information",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    requestTime = table.Column<string>(type: "text", nullable: true),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bad_information", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bad_information_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    materialCode = table.Column<string>(type: "text", nullable: true),
                    materialName = table.Column<string>(type: "text", nullable: true),
                    materialVersion = table.Column<string>(type: "text", nullable: true),
                    snNumber = table.Column<string>(type: "text", nullable: true),
                    operationCode = table.Column<string>(type: "text", nullable: true),
                    userId = table.Column<string>(type: "text", nullable: true),
                    requestTime = table.Column<string>(type: "text", nullable: true),
                    datalistId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bad_information_data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bad_information_datalist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    datalistId = table.Column<Guid>(type: "uuid", nullable: true),
                    badCode = table.Column<string>(type: "text", nullable: true),
                    badFactor = table.Column<string>(type: "text", nullable: true),
                    badQty = table.Column<string>(type: "text", nullable: true),
                    editTime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bad_information_datalist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "device_status_and_duration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    machineCode = table.Column<string>(type: "text", nullable: true),
                    machineStatusCode = table.Column<string>(type: "text", nullable: true),
                    timeUnit = table.Column<string>(type: "text", nullable: true),
                    acquisitCode = table.Column<string>(type: "text", nullable: true),
                    totalRunningDuration = table.Column<string>(type: "text", nullable: true),
                    curRunningDuration = table.Column<string>(type: "text", nullable: true),
                    requestTime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_status_and_duration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "equipment_status",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    machineCode = table.Column<string>(type: "text", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    machineStatusCode = table.Column<string>(type: "text", nullable: true),
                    machineStatusBegin = table.Column<string>(type: "text", nullable: true),
                    machineStatusEnd = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inspection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    requestTime = table.Column<string>(type: "text", nullable: true),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inspection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inspection_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    materialCode = table.Column<string>(type: "text", nullable: true),
                    materialName = table.Column<string>(type: "text", nullable: true),
                    materialVersion = table.Column<string>(type: "text", nullable: true),
                    snNumber = table.Column<string>(type: "text", nullable: true),
                    operationCode = table.Column<string>(type: "text", nullable: true),
                    paramCode = table.Column<string>(type: "text", nullable: true),
                    paramName = table.Column<string>(type: "text", nullable: true),
                    standardValue = table.Column<string>(type: "text", nullable: true),
                    paramRange1 = table.Column<string>(type: "text", nullable: true),
                    paramRange2 = table.Column<string>(type: "text", nullable: true),
                    realValue = table.Column<string>(type: "text", nullable: true),
                    checkStartTime = table.Column<string>(type: "text", nullable: true),
                    checkEndTime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inspection_data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_bind",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    snNumber = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    workOrderNumber = table.Column<string>(type: "text", nullable: true),
                    materialCode = table.Column<string>(type: "text", nullable: true),
                    materialCodeVersion = table.Column<string>(type: "text", nullable: true),
                    materialBarcode = table.Column<string>(type: "text", nullable: true),
                    userId = table.Column<string>(type: "text", nullable: true),
                    requestTime = table.Column<string>(type: "text", nullable: true),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_bind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_bind_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    orderCode = table.Column<string>(type: "text", nullable: true),
                    scheduleCode = table.Column<string>(type: "text", nullable: true),
                    snNumber = table.Column<string>(type: "text", nullable: true),
                    assemblyMaterialCode = table.Column<string>(type: "text", nullable: true),
                    assemblyMaterialQty = table.Column<string>(type: "text", nullable: true),
                    assemblySort = table.Column<string>(type: "text", nullable: true),
                    assemblyTime = table.Column<string>(type: "text", nullable: true),
                    assemblyMaterialName = table.Column<string>(type: "text", nullable: true),
                    assemblyMaterialVersion = table.Column<string>(type: "text", nullable: true),
                    assemblyMaterialSn = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_bind_data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_bind_info",
                columns: table => new
                {
                    物料名称 = table.Column<string>(type: "text", nullable: false),
                    正则表达式 = table.Column<string>(type: "text", nullable: false),
                    是否使用 = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_bind_info", x => x.物料名称);
                });

            migrationBuilder.CreateTable(
                name: "material_trace_passStation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    snNumber = table.Column<string>(type: "text", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    orderNumber = table.Column<string>(type: "text", nullable: true),
                    startStationCode = table.Column<string>(type: "text", nullable: true),
                    scheduleCode = table.Column<string>(type: "text", nullable: true),
                    materialCode = table.Column<string>(type: "text", nullable: true),
                    operationCode = table.Column<string>(type: "text", nullable: true),
                    userId = table.Column<string>(type: "text", nullable: true),
                    userName = table.Column<string>(type: "text", nullable: true),
                    passStatus = table.Column<string>(type: "text", nullable: true),
                    inStationTime = table.Column<string>(type: "text", nullable: true),
                    outStationTime = table.Column<string>(type: "text", nullable: true),
                    workReportQty = table.Column<string>(type: "text", nullable: true),
                    dataId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_trace_passStation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_trace_passStation_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true),
                    barCode = table.Column<string>(type: "text", nullable: true),
                    acquisitionTime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_trace_passStation_data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pass_station",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    snNumber = table.Column<string>(type: "text", nullable: true),
                    materialCode = table.Column<string>(type: "text", nullable: true),
                    workOrderNumber = table.Column<string>(type: "text", nullable: true),
                    scheduleNumber = table.Column<string>(type: "text", nullable: true),
                    trayNumber = table.Column<string>(type: "text", nullable: true),
                    reqType = table.Column<string>(type: "text", nullable: true),
                    startStationCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    snBindState = table.Column<int>(type: "integer", nullable: true),
                    barCode = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    userId = table.Column<string>(type: "text", nullable: true),
                    requestTime = table.Column<string>(type: "text", nullable: true),
                    passBeginTime = table.Column<string>(type: "text", nullable: true),
                    passEndTime = table.Column<string>(type: "text", nullable: true),
                    passNum = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pass_station", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product_parameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    snNumber = table.Column<string>(type: "text", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    machineCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    materialCode = table.Column<string>(type: "text", nullable: true),
                    materialName = table.Column<string>(type: "text", nullable: true),
                    materialVersion = table.Column<string>(type: "text", nullable: true),
                    paramTime = table.Column<string>(type: "text", nullable: true),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_parameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product_parameters_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true),
                    paramCode = table.Column<string>(type: "text", nullable: true),
                    paramName = table.Column<string>(type: "text", nullable: true),
                    standardValue = table.Column<string>(type: "text", nullable: true),
                    realValue = table.Column<string>(type: "text", nullable: true),
                    paramRange1 = table.Column<string>(type: "text", nullable: true),
                    paramRange2 = table.Column<string>(type: "text", nullable: true),
                    checkResult = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_parameters_data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "real_time_equipment_status",
                columns: table => new
                {
                    machineCode = table.Column<string>(type: "text", nullable: false),
                    machineStatusCode = table.Column<string>(type: "text", nullable: true),
                    machineStatusBegin = table.Column<string>(type: "text", nullable: true),
                    machineStatusEnd = table.Column<string>(type: "text", nullable: true),
                    totalRunningDuration = table.Column<string>(type: "text", nullable: true),
                    totalOnLineDuration = table.Column<string>(type: "text", nullable: true),
                    curFaultDuration = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_real_time_equipment_status", x => x.machineCode);
                });

            migrationBuilder.CreateTable(
                name: "real_time_product_info",
                columns: table => new
                {
                    snNumber = table.Column<string>(type: "text", nullable: false),
                    virtualSN = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    scheduleCode = table.Column<string>(type: "text", nullable: true),
                    operationCode = table.Column<string>(type: "text", nullable: true),
                    isbad = table.Column<bool>(type: "boolean", nullable: true),
                    ngStation = table.Column<string>(type: "text", nullable: true),
                    reProductCount = table.Column<int>(type: "integer", nullable: true),
                    updateTime = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_real_time_product_info", x => x.snNumber);
                });

            migrationBuilder.CreateTable(
                name: "schedule_info",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    scheduleCode = table.Column<string>(type: "text", nullable: false),
                    scheduleQty = table.Column<string>(type: "text", nullable: false),
                    startTime = table.Column<string>(type: "text", nullable: false),
                    endTime = table.Column<string>(type: "text", nullable: false),
                    scheduleStatusCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sn_create",
                columns: table => new
                {
                    snNumber = table.Column<string>(type: "text", nullable: false),
                    factoryCode = table.Column<string>(type: "text", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    materialCode = table.Column<string>(type: "text", nullable: true),
                    scheduleCode = table.Column<string>(type: "text", nullable: true),
                    userId = table.Column<string>(type: "text", nullable: true),
                    requestTime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sn_create", x => x.snNumber);
                });

            migrationBuilder.CreateTable(
                name: "station_tempo",
                columns: table => new
                {
                    StationCode = table.Column<string>(type: "text", nullable: false),
                    TheoreticalTempo = table.Column<int>(type: "integer", nullable: false),
                    AverageTempo = table.Column<float>(type: "real", nullable: false),
                    TempoCount = table.Column<int>(type: "integer", nullable: false),
                    MachineTempo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_station_tempo", x => x.StationCode);
                });

            migrationBuilder.CreateTable(
                name: "warning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    requestTime = table.Column<string>(type: "text", nullable: true),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warning", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "warning_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dataId = table.Column<Guid>(type: "uuid", nullable: true),
                    requestTime = table.Column<string>(type: "text", nullable: true),
                    unusualAlarmCode = table.Column<string>(type: "text", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true),
                    workShopCode = table.Column<string>(type: "text", nullable: true),
                    stationCode = table.Column<string>(type: "text", nullable: true),
                    lineCode = table.Column<string>(type: "text", nullable: true),
                    machineCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warning_data", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "设置");

            migrationBuilder.DropTable(
                name: "用户");

            migrationBuilder.DropTable(
                name: "alarmlog");

            migrationBuilder.DropTable(
                name: "alarmlog_data");

            migrationBuilder.DropTable(
                name: "bad_information");

            migrationBuilder.DropTable(
                name: "bad_information_data");

            migrationBuilder.DropTable(
                name: "bad_information_datalist");

            migrationBuilder.DropTable(
                name: "device_status_and_duration");

            migrationBuilder.DropTable(
                name: "equipment_status");

            migrationBuilder.DropTable(
                name: "inspection");

            migrationBuilder.DropTable(
                name: "inspection_data");

            migrationBuilder.DropTable(
                name: "material_bind");

            migrationBuilder.DropTable(
                name: "material_bind_data");

            migrationBuilder.DropTable(
                name: "material_bind_info");

            migrationBuilder.DropTable(
                name: "material_trace_passStation");

            migrationBuilder.DropTable(
                name: "material_trace_passStation_data");

            migrationBuilder.DropTable(
                name: "pass_station");

            migrationBuilder.DropTable(
                name: "product_parameters");

            migrationBuilder.DropTable(
                name: "product_parameters_data");

            migrationBuilder.DropTable(
                name: "real_time_equipment_status");

            migrationBuilder.DropTable(
                name: "real_time_product_info");

            migrationBuilder.DropTable(
                name: "schedule_info");

            migrationBuilder.DropTable(
                name: "sn_create");

            migrationBuilder.DropTable(
                name: "station_tempo");

            migrationBuilder.DropTable(
                name: "warning");

            migrationBuilder.DropTable(
                name: "warning_data");
        }
    }
}
