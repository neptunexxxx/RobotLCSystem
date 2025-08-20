using MahApps.Metro.Controls;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lmes.工具
{
    public static class DataHelper
    {
        public static DataTable DataGridToDataTable(DataGrid dataGrid)
        {
            DataTable dt = new DataTable();

            // 添加列
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].Visibility == Visibility.Visible)
                {
                    dt.Columns.Add(dataGrid.Columns[i].Header.ToString());
                }
            }

            // 添加行数据
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                var item = dataGrid.Items[i];
                int columnsIndex = 0;
                DataRow row = dt.NewRow();

                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    if (dataGrid.Columns[j].Visibility == Visibility.Visible)
                    {
                        if (dataGrid.Columns[j] is DataGridTemplateColumn templateColumn)
                        {
                            // 获取当前行的绑定数据
                            var nestedData = (item as dynamic)?.data;
                            if (nestedData != null)
                            {
                                // 将嵌套数据转换为字符串
                                string nestedDataStr = GetNestedDataGridDataAsString(nestedData);
                                row[columnsIndex] = nestedDataStr;
                            }
                            else
                            {
                                row[columnsIndex] = "";
                            }
                        }
                        else
                        {
                            // 获取普通列的值
                            if (dataGrid.Items[i] != null && dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) is TextBlock textBlock)
                            {
                                row[columnsIndex] = textBlock.Text.ToString();
                            }
                            else
                            {
                                row[columnsIndex] = "";
                            }
                        }
                        columnsIndex++;
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        private static string GetNestedDataGridDataAsString(dynamic nestedData)
        {
            StringBuilder sb = new StringBuilder();

            // 添加表头信息
            sb.Append("参数名, 实际值, 检验结果");

            foreach (var item in nestedData)
            {
                sb.Append("\n");
                sb.Append($"{item.paramName},{item.realValue},{item.checkResult}");
            }

            return sb.ToString();
        }


        private static string? OpenExcelSaveFileDialog(string? excelTitle = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files(*.xls)|*.xls|Excel Files(*.xlsx)|*.xlsx";
            saveFileDialog.Title = $"导出{excelTitle}为表格";
            saveFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return saveFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        public static void ExportDataGridToExcel(DataGrid dataGrid, string? title = null)
        {
            try
            {
                string? filePath = OpenExcelSaveFileDialog(title);
                if (!string.IsNullOrEmpty(filePath))
                {
                    IWorkbook workbook = new HSSFWorkbook();
                    ISheet sheet = workbook.CreateSheet(title ?? "sheet1");
                    DataTable dataTable = DataGridToDataTable(dataGrid);
                    IRow headerRow = sheet.CreateRow(0);

                    // 创建表头
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        ICell cell = headerRow.CreateCell(i);
                        cell.SetCellValue(dataTable.Columns[i].ColumnName);

                        // 为表头设置垂直居中
                        ICellStyle cellStyle = workbook.CreateCellStyle();
                        cellStyle.VerticalAlignment = (NPOI.SS.UserModel.VerticalAlignment)System.Windows.VerticalAlignment.Center;  // 垂直居中
                        cell.CellStyle = cellStyle;
                    }

                    // 填充数据行
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        IRow row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            ICell cell = row.CreateCell(j);
                            var value = dataTable.Rows[i][j]?.ToString() ?? string.Empty;

                            // 设置单元格值
                            cell.SetCellValue(value);

                            // 创建并设置单元格样式
                            ICellStyle cellStyle = workbook.CreateCellStyle();

                            // 设置垂直居中
                            cellStyle.VerticalAlignment = (NPOI.SS.UserModel.VerticalAlignment)System.Windows.VerticalAlignment.Center;

                            // 启用换行
                            if (value.Contains("\n"))
                            {
                                cellStyle.WrapText = true;
                            }

                            // 应用样式
                            cell.CellStyle = cellStyle;
                        }
                    }

                    // 保存文件
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fileStream);
                        MessageBox.Show($"{title}表格导出成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{title}表格导出失败：{ex.Message}");
            }
        }
    }
}
