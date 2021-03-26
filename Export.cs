using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

using Ookii.Dialogs;

using Newtonsoft.Json.Linq;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace mysqlDbManager
{
    class Export
    {
        Main main;
        string path = "";
        Ookii.Dialogs.Wpf.VistaFolderBrowserDialog vistaFolderBrowserDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

        /// FUNCTION THAT GETS MAIN WINDOW TO SHOW LOG OUTPUT
        public void setMainWindow(Main window)
        {
            main = window;
        }
        /// ==========

        /// FUNCTION FOR TABLE EXPORT TO JSON
        public bool exportToJSON(List<string>[] tableList, string tableName, string databaseName)
        {
            string filename = databaseName+"_"+tableName;
            string filenameBuffer = filename;
            int fileNumber = 0;
            List<string> headersList = new List<string>();

            // GET HEADERS
            for (int i = 0; i < tableList.Length; i++)
            {
                string headerBuffer = tableList[i][0];
                if (headerBuffer[0] == '.')
                {
                    headerBuffer = headerBuffer.Substring(1);
                }
                headersList.Add(headerBuffer);
            }

            // CREATE JSON OBJECT
            JArray jsonArray = new JArray();
            for(int i=1; i<tableList[0].Count; i++)
            {
                JObject jsonRow = new JObject();
                for(int j=0; j<tableList.Length; j++)
                {
                    JProperty cell = new JProperty(headersList[j], tableList[j][i]);
                    jsonRow.Add(cell);
                }
                jsonArray.Add(jsonRow);
            }

            // GET FILE PATH
            getExportPath();

            // IF FILE EXIST CHANGE FILENAME
            while (File.Exists(path + "\\" + filenameBuffer + ".json"))
            {
                fileNumber++;
                filenameBuffer = filename + fileNumber.ToString();
            }

            // TRY WRITE TO FILE
            try
            {
                File.WriteAllText(path + "\\" + filenameBuffer + ".json", jsonArray.ToString());
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// ==========

        /// FUNCTION FOR TABLE EXPORT TO EXCEL SHEET
        public bool exportToExcel(List<string>[] tableList, string tableName, string databaseName)
        {
            string filename = databaseName + "_" + tableName;
            string filenameBuffer = filename;
            int fileNumber = 0;
            List<string> headersList = new List<string>();

            // GET HEADERS
            for (int i = 0; i < tableList.Length; i++)
            {
                string headerBuffer = tableList[i][0];
                if (headerBuffer[0] == '.')
                {
                    headerBuffer = headerBuffer.Substring(1);
                }
                headersList.Add(headerBuffer);
            }

            // GET FILE PATH
            getExportPath();

            // IF FILE EXIST CHANGE FILENAME
            while (File.Exists(path + "\\" + filenameBuffer + ".xlsx"))
            {
                fileNumber++;
                filenameBuffer = filename + fileNumber.ToString();
            }

            // TRY CREATE EXCEL FILE
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(path + "\\" + filenameBuffer + ".xlsx", SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = tableName };
                    sheets.Append(sheet);
                    workbookPart.Workbook.Save();
                    
                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    Row row = new Row();
                    int fCount = headersList.Count;
                    for (int i = 0; i < fCount; i++)
                    {
                        String fName = headersList[i];
                        row.Append(ConstructCell(fName.ToString(), CellValues.String));
                    }
                    sheetData.AppendChild(row);

                    for(int j=1; j<tableList[0].Count; j++)
                    {
                        row = new Row();
                        for (int i = 0; i < fCount; i++)
                        {
                            String col;
                            try
                            {
                                col = tableList[i][j];
                            }
                            catch (Exception e)
                            {
                                col = "NULL";
                            }
                            row.Append(ConstructCell(col.ToString(), CellValues.String));
                        }
                        sheetData.AppendChild(row);
                    }

                    worksheetPart.Worksheet.Save();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// ==========

        /// FUNCTION THAT HANDLES DIALOG BOXES AND INITIALIZE PATH
        private void getExportPath()
        {
            vistaFolderBrowserDialog.ShowDialog();
            path = vistaFolderBrowserDialog.SelectedPath;
        }
        /// ==========

        /// FUNCTION TO CREATE CELL IN EXCEL FILE
        static private Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }
        /// ==========
    }
}
