using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using TaskTrackingSystem.Shared.Entities;
using Excel = Microsoft.Office.Interop.Excel;
using TaskTrackingSystem.Shared.Entities.PayListItem;

namespace TaskTrackingSystem.Logic.Files
{
    public static class Exel
    {
        private static readonly Dictionary<int, int> StatusColor = new Dictionary<int, int>()
        {
            { 1 , System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Khaki) },
            { 2 , System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green) },
            { 3 , System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red) },
        };

        /// <summary>
        /// Расчитывает премии, возвращает string - путь к файлу на сервере
        /// 
        ///  кроме тендеров и Разработки ПО ищет проекты у которых стоит статус завершен и не стоит галочка Премия выплачена.
        ///  Выгружает эти проекты в формате Excel со всеми теми же столбцами, но немного измененными.
        ///  Вместо суммы оценок пишется результат следующей формулы ((Сумма оценок)/100)*(Стоимость проекта).
        ///  После последней строки, содержащей данные, добавляем 1 пустую строку, а следом в колонке Название
        ///  проекта пишем Заработная плата и на пересечении с каждым пользователем подсасываем из профиля пользователя его зп.
        /// </summary>
        /// <returns></returns>
        public static string AwardsCalculate()
        {
            var ObjExcel = new Excel.Application();
            var ObjWorkBook = ObjExcel.Workbooks.Add(System.Reflection.Missing.Value);
            var ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];

            ObjWorkSheet.Columns[2].ColumnWidth = 4;
            ObjWorkSheet.Columns[3].ColumnWidth = 20;
            ObjWorkSheet.Columns[4].ColumnWidth = 12;
            ObjWorkSheet.Columns[5].ColumnWidth = 12;
            ObjWorkSheet.Columns[6].ColumnWidth = 16;
            ObjWorkSheet.Columns[7].ColumnWidth = 16;
            ObjWorkSheet.Columns[8].ColumnWidth = 16;
            ObjWorkSheet.Columns[9].ColumnWidth = 16;

            ObjWorkSheet.Cells[1, 2] = "Статус";
            ObjWorkSheet.Cells[1, 3] = "Название проекта";
            ObjWorkSheet.Cells[1, 4] = "Дата начала";
            ObjWorkSheet.Cells[1, 5] = "Дата завершения";
            ObjWorkSheet.Cells[1, 6] = "Стоимость";
            ObjWorkSheet.Cells[1, 7] = "Премия выплачена";
            ObjWorkSheet.Cells[1, 8] = "Все оценки";
            ObjWorkSheet.Cells[1, 9] = "Задач без оценок";

            var nextAccountCol = 10;
            var nextRow = 2;

            var AccountCell = new Dictionary<string, int>();

            foreach (var pt in Logic.Instance.ProjectType.GetMainPTs())
            {
                var projects = Logic.Instance.Project.GetAllForProjectType(pt.Id).Where(x => x.StatusId == Status.DoneId && !x.PremiumPaid).ToList();

                if (projects.Count != 0)
                {
                    nextRow++;
                    ObjWorkSheet.Cells[nextRow, 1] = pt.Name;
                }

                foreach (var p in projects)
                {
                    nextRow++;

                    var tasks = Logic.Instance.Task.GetAllForProject(p.Id);

                    var storyPoints = tasks.Where(x => x.StatusId != Status.DeletedId).Sum(x => x.StoryPoints);

                    ObjWorkSheet.Cells[nextRow, 2].Interior.Color = StatusColor[p.Status.Id];
                    ObjWorkSheet.Cells[nextRow, 2] = p.Status.Name;
                    ObjWorkSheet.Cells[nextRow, 3] = p.Name;
                    ObjWorkSheet.Cells[nextRow, 4] = p.DateStart;
                    ObjWorkSheet.Cells[nextRow, 5] = p.DateEnd;
                    ObjWorkSheet.Cells[nextRow, 6] = p.Cost;
                    ObjWorkSheet.Cells[nextRow, 7].Interior.Color = p.PremiumPaid ? System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green) : System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    ObjWorkSheet.Cells[nextRow, 7] = p.PremiumPaid ? "Да" : "Нет";
                    ObjWorkSheet.Cells[nextRow, 8] = storyPoints;
                    ObjWorkSheet.Cells[nextRow, 9] = tasks.FindAll(x => x.StoryPoints == 0 && x.Status.Id != Status.DeletedId).Count;
                    //ObjWorkSheet.Cells[nextRow, 10] = (decimal)storyPoints / (decimal)100 * p.Cost;

                    var r = tasks.GroupBy(u => u.Account.Name).Select(grp => grp.First()).ToList();

                    foreach (var acc in tasks.GroupBy(u => u.Account.Login).Select(grp => grp.First().Account))
                    {
                        var pointsDone = tasks.FindAll(t => t.Status.Id == Status.DoneId && t.Account.Login.Equals(acc.Login)).Sum(m => m.StoryPoints);

                        if (!AccountCell.ContainsKey(acc.Login))
                        {
                            ObjWorkSheet.Cells[1, nextAccountCol] = acc.Name;
                            ObjWorkSheet.Columns[nextAccountCol].ColumnWidth = 16;
                            AccountCell.Add(acc.Login, nextAccountCol++);
                        }

                        var x1 = ObjWorkSheet.Cells[nextRow, 6].Address;

                        //ObjWorkSheet.Cells[nextRow, AccountCell[acc.Login]] = pointsDone / 100m * p.Cost;

                        try
                        {
                            var value = $"={pointsDone}/100*{ObjWorkSheet.Cells[nextRow, 6].Address}";
                            ObjWorkSheet.Cells[nextRow, AccountCell[acc.Login]] = value;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            nextRow += 2;

            ObjWorkSheet.Cells[nextRow, 3] = "Заработная плата :";

            foreach (var accCell in AccountCell)
            {
                ObjWorkSheet.Cells[nextRow, accCell.Value] = Logic.Instance.Account.Get(accCell.Key).Wage;
            }


            var filePath = $"C:\\Server\\data\\Excel\\{Guid.NewGuid()}.xlsx";

            Logic.Instance.LogEvent.Log.Debug($"filePath {filePath}");
            ObjWorkBook.SaveAs(filePath);

            try
            {
                Logic.Instance.LogEvent.Log.Debug("ObjWorkBook.SaveAs Start");

                Logic.Instance.LogEvent.Log.Debug("ObjWorkBook.SaveAs End");
            }
            catch (Exception ex)
            {
                Logic.Instance.LogEvent.Log.Error($"ObjWorkBook.SaveAs Exception {ex.Message}");
                Logic.Instance.LogEvent.Log.Error(ex);
            }
            finally
            {
                Logic.Instance.LogEvent.Log.Debug("finally ObjWorkBook.Close start");
                ObjWorkBook.Close();
                Logic.Instance.LogEvent.Log.Debug("finally ObjWorkBook.Close end");
            }

            return filePath;
        }


        public static string EvaluationProjectReport(int projectTypeId, DateTime? startDate, DateTime? endDate)
        {
            var start = startDate == null ? DateTime.MinValue : (DateTime)startDate;
            var end = endDate == null ? DateTime.MaxValue : (DateTime)endDate;

            var ObjExcel = new Excel.Application();
            var ObjWorkBook = ObjExcel.Workbooks.Add(System.Reflection.Missing.Value);
            var ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];

            Logic.Instance.LogEvent.Log.Debug("ObjWorkBook Created");

            var projectType = Logic.Instance.ProjectType.Get(projectTypeId);
            var projects = Logic.Instance.Project.GetAllForProjectType(projectType.Id).FindAll(p => p.Status.Id != Status.DeletedId).OrderByDescending(p => p.Status.Id).Where(x => x.DateEnd >= start && x.DateEnd <= end);

            Logic.Instance.LogEvent.Log.Debug("SQL Got Data");

            ObjWorkSheet.Columns[1].ColumnWidth = 4;
            ObjWorkSheet.Columns[2].ColumnWidth = 20;
            ObjWorkSheet.Columns[3].ColumnWidth = 12;
            ObjWorkSheet.Columns[4].ColumnWidth = 12;
            ObjWorkSheet.Columns[5].ColumnWidth = 16;
            ObjWorkSheet.Columns[6].ColumnWidth = 16;
            ObjWorkSheet.Columns[7].ColumnWidth = 16;
            ObjWorkSheet.Columns[8].ColumnWidth = 16;

            ObjWorkSheet.Cells[1, 1] = "Статус";
            ObjWorkSheet.Cells[1, 2] = "Название проекта";
            ObjWorkSheet.Cells[1, 3] = "Дата начала";
            ObjWorkSheet.Cells[1, 4] = "Дата завершения";
            ObjWorkSheet.Cells[1, 5] = "Стоимость";
            ObjWorkSheet.Cells[1, 6] = "Премия выплачена";
            ObjWorkSheet.Cells[1, 7] = "Все оценки";
            ObjWorkSheet.Cells[1, 8] = "Задач без оценок";

            var nextAccountCol = 9;
            var nextProjectRow = 2;

            var AccountCell = new Dictionary<string, int>();

            Logic.Instance.LogEvent.Log.Debug("Excel Column Created");

            foreach (var project in projects)
            {
                var tasks = Logic.Instance.Task.GetAllForProject(project.Id).ToList();

                ObjWorkSheet.Cells[nextProjectRow, 1].Interior.Color = StatusColor[project.Status.Id];
                ObjWorkSheet.Cells[nextProjectRow, 1] = project.Status.Name;
                ObjWorkSheet.Cells[nextProjectRow, 2] = project.Name;
                ObjWorkSheet.Cells[nextProjectRow, 3] = project.DateStart;
                ObjWorkSheet.Cells[nextProjectRow, 4] = project.DateEnd;
                ObjWorkSheet.Cells[nextProjectRow, 5] = project.Cost;
                ObjWorkSheet.Cells[nextProjectRow, 6].Interior.Color = project.PremiumPaid ? System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green) : System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                ObjWorkSheet.Cells[nextProjectRow, 6] = project.PremiumPaid ? "Да" : "Нет";
                ObjWorkSheet.Cells[nextProjectRow, 7] = tasks.Sum(x => x.StoryPoints);
                ObjWorkSheet.Cells[nextProjectRow, 8] = tasks.FindAll(x => x.StoryPoints == 0 && x.Status.Id != Status.DeletedId).Count;


                var r = tasks.GroupBy(u => u.Account.Name).Select(grp => grp.First()).ToList();

                foreach (var acc in tasks.GroupBy(u => u.Account.Login).Select(grp => grp.First().Account))
                {
                    var pointsDone = tasks.FindAll(t => t.Status.Id == Status.DoneId && t.Account.Login.Equals(acc.Login)).Sum(m => m.StoryPoints);

                    if (!AccountCell.ContainsKey(acc.Name))
                    {
                        ObjWorkSheet.Cells[1, nextAccountCol] = acc.Name;
                        ObjWorkSheet.Columns[nextAccountCol].ColumnWidth = 16;
                        AccountCell.Add(acc.Name, nextAccountCol++);
                    }
                    ObjWorkSheet.Cells[nextProjectRow, AccountCell[acc.Name]] = pointsDone;
                }

                nextProjectRow++;
            }

            Logic.Instance.LogEvent.Log.Debug("Excel Column Filled");


            var filePath = $"C:\\Server\\data\\Excel\\{Guid.NewGuid()}.xlsx";

            Logic.Instance.LogEvent.Log.Debug($"filePath {filePath}");
            ObjWorkBook.SaveAs(filePath);

            try
            {
                Logic.Instance.LogEvent.Log.Debug("ObjWorkBook.SaveAs Start");

                Logic.Instance.LogEvent.Log.Debug("ObjWorkBook.SaveAs End");



            }
            catch (Exception ex)
            {

                Logic.Instance.LogEvent.Log.Error($"ObjWorkBook.SaveAs Exception {ex.Message}");
                Logic.Instance.LogEvent.Log.Error(ex);
            }
            finally
            {
                Logic.Instance.LogEvent.Log.Debug("finally ObjWorkBook.Close start");
                ObjWorkBook.Close();
                Logic.Instance.LogEvent.Log.Debug("finally ObjWorkBook.Close end");
            }

            return filePath;
        }

        public static string EvaluationProjectReportOpenXML(int projectTypeId)
        {
            var filePath = $"C:\\Server\\data\\Excel\\{Guid.NewGuid()}.xlsx";
            using (var document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {

                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                var fv = new FileVersion
                {
                    ApplicationName = "Microsoft Office Excel"
                };
                worksheetPart.Worksheet = new Worksheet(new SheetData());
                var wbsp = workbookPart.AddNewPart<WorkbookStylesPart>();

                // Задаем колонки и их ширину
                var lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
                var needToInsertColumns = false;
                if (lstColumns == null)
                {
                    lstColumns = new Columns();
                    needToInsertColumns = true;
                }
                lstColumns.Append(new Column() { Min = 1, Max = 10, Width = 20, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 2, Max = 10, Width = 20, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 3, Max = 10, Width = 20, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 4, Max = 10, Width = 20, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 5, Max = 10, Width = 20, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 6, Max = 10, Width = 20, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 7, Max = 10, Width = 20, CustomWidth = true });
                if (needToInsertColumns)
                    worksheetPart.Worksheet.InsertAt(lstColumns, 0);


                //Создаем лист в книге
                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Отчет по входящим" };
                sheets.Append(sheet);

                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                //Добавим заголовки в первую строку
                var row = new Row() { RowIndex = 1 };
                sheetData.Append(row);

                InsertCell(row, 1, "Стиль 1", CellValues.String, 5);
                InsertCell(row, 2, "Стиль 2", CellValues.String, 5);
                InsertCell(row, 3, "Стиль 3", CellValues.String, 5);
                InsertCell(row, 4, "Стиль 4", CellValues.String, 5);
                InsertCell(row, 5, "Стиль 5", CellValues.String, 5);
                InsertCell(row, 6, "Стиль 6", CellValues.String, 5);
                InsertCell(row, 7, "Стиль 7", CellValues.String, 5);



                workbookPart.Workbook.Save();
                document.Close();
            }

            return filePath;
        }

        public static List<PayList> ParcePayList(string path)
        {
            var payLists = new List<PayList>();

            try
            {
                //var objExcelApp = new Microsoft.Office.Interop.Excel.Application();
                ////var objBooks = (Microsoft.Office.Interop.Excel.Workbook)objExcelApp.Workbooks;
                //var objBook = objExcelApp.Workbooks.Open(path
                //    ,Missing.Value, Missing.Value
                //    ,Missing.Value, Missing.Value
                //    ,Missing.Value, Missing.Value
                //    ,Missing.Value, Missing.Value
                //    ,Missing.Value, Missing.Value
                //    ,Missing.Value, Missing.Value
                //    ,Missing.Value, Missing.Value);

                //var objSheets = objBook.Worksheets;
                //var objSheet = (Microsoft.Office.Interop.Excel.Worksheet)objSheets.get_Item(1);
                //var rngLast = objSheet.get_Range("A1");
                //long row = rngLast.Row;
                //long col = rngLast.Column;

                var App = new Excel.Application();
                var wb = App.Workbooks.Open(path);
                wb.Activate();

                var ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets.get_Item(1);

                //http://localhost:51571/AdminPage/PayList


                var x1 = ObjWorkSheet.Cells[1, 1];
                string s1 = x1.Value.ToString();

                var x2 = ObjWorkSheet.Cells[1, 2];
                string s2 = x2.Value;

                var x3 = ObjWorkSheet.Cells[2, 1];
                string s3 = x3.Value;

                var startPos = PayListFindStartPos(ObjWorkSheet);

                payLists = PayListParceWorkers(startPos, ObjWorkSheet);


                wb.Close();
            }
            catch(Exception ex)
            {

            }

            return payLists;
        }

        private static List<PayList> PayListParceWorkers(List<int> startPos, Excel.Worksheet objWorkSheet)
        {
            var payList = new List<PayList>();

            foreach (var sp in startPos)
            {
                var pl = new PayList();

                try
                {
                    pl.Title = (string)objWorkSheet.Cells[sp + 0, 1].Value;
                    pl.Name = (string)objWorkSheet.Cells[sp + 1, 1].Value;//Worker Name
                    try
                    {
                        pl.ToPay = objWorkSheet.Cells[sp + 1, 32].Value is null ? 0 : (float)objWorkSheet.Cells[sp + 1, 32].Value;//К выплате
                    }
                    catch (Exception ex)
                    {
                        pl.Error += $"ToPay - {ex.Message}";
                    }
                    pl.Organization = (string)objWorkSheet.Cells[sp + 2, 5].Value;
                    pl.Position = (string)objWorkSheet.Cells[sp + 2, 26].Value;//Должность
                    pl.Salary = float.Parse(objWorkSheet.Cells[sp + 3, 26].Value.ToString().Replace(" ", ""));//Оклад (тариф)
                    try
                    {
                        pl.Accrued = float.Parse(objWorkSheet.Cells[sp + 6, 18].Value.ToString().Replace(" ", ""));//Начислено
                    }
                    catch (Exception ex)
                    {
                        pl.Error += $"Accrued - {ex.Message}";
                    }
                    try
                    {
                        pl.Hold = float.Parse(objWorkSheet.Cells[sp + 6, 32].Value.ToString().Replace(" ", ""));//Удержано
                    }
                    catch (Exception ex)
                    {
                        pl.Error += $"Hold - {ex.Message}";
                    }

                    pl.AccruedItems = PayListParceAccrued(objWorkSheet, sp + 7);
                    pl.HoldItems = PayListParceHolded(objWorkSheet, sp + 7);
                    pl.PaidItems = PayListParcePaid(objWorkSheet, sp + 7 + pl.HoldItems.Count + 1);

                    var nextRow = Math.Max(sp + 7 + pl.AccruedItems.Count, sp + 7 + pl.HoldItems.Count + 1 + pl.PaidItems.Count) + 1;

                    pl.DebtCompanyStartMonth = float.Parse(objWorkSheet.Cells[nextRow, 18].Value.ToString().Replace(" ", "")); //Долг предприятия на начало
                    pl.DebtWorkerStartMonth = float.Parse(objWorkSheet.Cells[nextRow, 32].Value.ToString().Replace(" ", "")); //Долг работника на начало

                    pl.TotalTaxableIncome = (string)objWorkSheet.Cells[nextRow + 2, 1].Value; //Общий облагаемый доход
                }
                catch (Exception ex)
                {
                    pl.Error += ex.Message;
                }

                payList.Add(pl);
            }

            return payList;
        }

        private static List<PaidItem> PayListParcePaid(Excel.Worksheet objWorkSheet, int sp)
        {
            var items = new List<PaidItem>();

            //try
            //{
            //    b = objWorkSheet.Cells[sp, 22].Value is null;
            //    Debug.WriteLine($"bb {b}");
            //    s = objWorkSheet.Cells[sp, 22].Value.ToString();
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"PayListParcePaid Start {ex.Message}");
            //    return items;
            //}

            while (objWorkSheet.Cells[sp, 22].Value != null)
            {
                var pi = new PaidItem();

                pi.Type = (string)objWorkSheet.Cells[sp, 22].Value;
                pi.Period = (string)objWorkSheet.Cells[sp, 29].Value;
                pi.Summa = objWorkSheet.Cells[sp, 32].Value is null ? 0 : (float)objWorkSheet.Cells[sp, 32].Value;

                items.Add(pi);

                sp++;
            }

            return items;
        }

        private static List<HoldItem> PayListParceHolded(Excel.Worksheet objWorkSheet, int sp)
        {
            var items = new List<HoldItem>();

            while (objWorkSheet.Cells[sp, 22].Value.ToString() != "Выплачено:")
            {
                var hi = new HoldItem();

                try
                {
                    hi.Type = (string)objWorkSheet.Cells[sp, 22].Value;
                    hi.Period = (string)objWorkSheet.Cells[sp, 29].Value;
                    hi.Summa = objWorkSheet.Cells[sp, 32].Value is null ? 0 : (float)objWorkSheet.Cells[sp, 32].Value;
                }
                catch (Exception ex)
                {
                }

                items.Add(hi);

                sp++;
            }

            return items;
        }

        private static List<AccruedItem> PayListParceAccrued(Excel.Worksheet objWorkSheet, int sp)
        {
            var items = new List<AccruedItem>();

            while (objWorkSheet.Cells[sp, 1].Value != null)
            {
                var ai = new AccruedItem();

                try
                {
                    ai.Type = (string)objWorkSheet.Cells[sp, 1].Value;
                    ai.Period = (string)objWorkSheet.Cells[sp, 8].Value;
                    ai.Days = objWorkSheet.Cells[sp, 11].Value is null ? 0 : (int)objWorkSheet.Cells[sp, 11].Value;
                    ai.Hours = objWorkSheet.Cells[sp, 13].Value is null ? 0 : (int)objWorkSheet.Cells[sp, 13].Value;
                    ai.Summa = objWorkSheet.Cells[sp, 18].Value is null ? 0 : (float)objWorkSheet.Cells[sp, 18].Value;
                }
                catch (Exception ex)
                {
                }

                items.Add(ai);

                sp++;
            }

            return items;
        }

        private static List<int> PayListFindStartPos(Excel.Worksheet objWorkSheet)
        {
            var result = new List<int>();
            var missData = 0;
            var i = 0;

            while (missData < 10)
            {
                i++;
                if (objWorkSheet.Cells[i, 1].Value is null)
                {
                    missData++;
                    continue;
                }
                else
                {
                    missData = 0;
                }

                if (((string)objWorkSheet.Cells[i, 1].Value).Contains("РАСЧЕТНЫЙ ЛИСТОК"))
                {
                    result.Add(i);
                }
            }

            return result;
        }

        //Добавление Ячейки в строку (На вход подаем: строку, номер колонки, тип значения, стиль)
        private static void InsertCell(Row row, int cell_num, string val, CellValues type, uint styleIndex)
        {
            Cell refCell = null;
            var newCell = new Cell() { CellReference = cell_num.ToString() + ":" + row.RowIndex.ToString(), StyleIndex = styleIndex };
            row.InsertBefore(newCell, refCell);

            // Устанавливает тип значения.
            newCell.CellValue = new CellValue(val);
            newCell.DataType = new EnumValue<CellValues>(type);

        }
    }
}