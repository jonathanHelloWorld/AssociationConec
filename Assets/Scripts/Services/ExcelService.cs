using InterativaSystem.Controllers;
//using Excel = Microsoft.Office.Interop.Excel;

namespace InterativaSystem.Services
{
    public class ExcelService : GenericService
    {
        /*
        private Excel.Application xlsApp;
        private Excel.Workbook xlsWorkbook;
        private Excel._Worksheet xlsWorksheet;
        private Excel.Range xlsRange;

        private IOController ioController;

        public string XLSFile;

        protected override void OnStart()
        {
            base.OnStart();

            ioController = _bootstrap.GetController(ControllerTypes.IO) as IOController;

            xlsApp = new Excel.Application();
            xlsWorkbook = xlsApp.Workbooks.Open(ioController.DataFolder + "/" + XLSFile, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlsWorksheet = (Excel._Worksheet)xlsWorkbook.Sheets[0];
            xlsRange = xlsWorksheet.UsedRange;

            int rowCount = xlsRange.Rows.Count;
            int colCount = xlsRange.Columns.Count;

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    DebugLog(xlsRange.Cells[i, j].ToString());
                }
            }
        }
        /**/
    }
}