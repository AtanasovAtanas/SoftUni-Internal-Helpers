namespace StatsAggregator
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    using OfficeOpenXml;
    using MoreLinq;

    public class Program
    {

        public static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // /engine/1/0
            var filterUrl = Console.ReadLine();

            var usersInCourseInstances = GetUsersInCourseInstances("./UsersInCourseInstances.xlsx");

            var entriesByUrl = GetAllEntriesByUrl("./UsersOnPages.xlsx");

            var filteredEntries = entriesByUrl
                .Where(e => e.Key.Contains(filterUrl))
                .ToDictionary(x => x.Key, x => x.Value);

            GenerateExport(filteredEntries, usersInCourseInstances);
        }

        private static void GenerateExport(Dictionary<string, List<Entry>> entries,
            Dictionary<string, int> usersInCourseInstances)
        {
            var sessionCounter = 0;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                foreach (var entry in entries.OrderBy(e => e.Key))
                {
                    //create a datatable
                    DataTable dataTable = new DataTable();

                    //add three colums to the datatable
                    dataTable.Columns.Add(nameof(ExportModel.Username), typeof(string));
                    dataTable.Columns.Add(nameof(Entry.Url), typeof(string));
                    dataTable.Columns.Add(nameof(ExportModel.RegionCode), typeof(string));
                    dataTable.Columns.Add(nameof(ExportModel.CourseInstanceId), typeof(int));
                    dataTable.Columns.Add(nameof(ExportModel.CreatedOn), typeof(DateTime));

                    //add some rows
                    foreach (Entry e in entry.Value.OrderBy(e => e.CreatedOn).DistinctBy(e => e.Email))
                    {
                        if (!usersInCourseInstances.ContainsKey(e.Email))
                        {
                            continue;
                        }

                        dataTable.Rows.Add(e.Email, e.Url, e.Region, usersInCourseInstances[e.Email], e.CreatedOn);
                    }

                    //create a WorkSheet
                    var workSheetName = entry.Key.Replace('/', '-');
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(workSheetName);
                    
                    worksheet.Cells[$"E1:E{entry.Value.Count}"].Style.Numberformat.Format = "dd-MM-yyyy HH:mm";


                    //add all the content from the DataTable, starting at cell A1
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                    //excelPackage.Workbook.Worksheets[$"Session-{sessionCounter++}"].Cells["A1:E1"].AutoFitColumns();
                }

                var excelFile = new FileInfo("./Export2.xlsx");
                excelPackage.SaveAs(excelFile);
            }
        }

        private static Dictionary<string, List<Entry>> GetAllEntriesByUrl(string path)
        {
            var result = new Dictionary<string, List<Entry>>();
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet firstSheet = package.Workbook.Worksheets.FirstOrDefault();

                int lastRow = firstSheet.Dimension.End.Row;

                // C - Url
                // D - Region code
                // E - Created on
                // F - Email
                for (int row = 2; row <= lastRow; row++)
                {
                    var url = firstSheet.Cells[$"C{row}"].Text;
                    var regionCode = firstSheet.Cells[$"D{row}"].Text;
                    var createdOn = DateTime.Parse(firstSheet.Cells[$"E{row}"].Text);
                    var email = firstSheet.Cells[$"F{row}"].Text;

                    if (email.Contains("softuni") || email.Contains("nakov"))
                    {
                        continue;
                    }

                    if (!result.ContainsKey(url))
                    {
                        result.Add(url, new List<Entry>());
                    }

                    result[url].Add(new Entry(email, url, regionCode, createdOn));
                }
            }

            return result;
        }

        private static Dictionary<string, int> GetUsersInCourseInstances(string path)
        {
            var result = new Dictionary<string, int>();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet firstSheet = package.Workbook.Worksheets.FirstOrDefault();

                int lastRow = firstSheet.Dimension.End.Row;

                // C - Course instance id
                // F - Email
                for (int row = 2; row <= lastRow; row++)
                {
                    var courseInstanceId = int.Parse(firstSheet.Cells[$"C{row}"].Text);
                    var email = firstSheet.Cells[$"F{row}"].Text;

                    result[email] = courseInstanceId;
                }
            }

            return result;
        }
    }
}