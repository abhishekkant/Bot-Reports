using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ISE_Solutions.Model;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ISE_Solutions.Controllers
{
    public class ExcelExportController : Controller
    {
        // GET: ExcelExport
        private CloudTable table;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Top10TicketRaiseExport(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("EmployeeID");
            dt.Columns.Add("TicketRaisedCount");
            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference("TicketRaisedHistory");
                string StartdateString = SDate;//"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate; //"2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);
                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.EmployeeID).Take(10).OrderByDescending(g => g.Count()).ToList();
                
                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    DataList.EmployeeID = (singleData.Key).ToString();
                    DataList.Values = singleData.Count();
                    DataRow _rvi = dt.NewRow();
                    _rvi["EmployeeID"] = DataList.EmployeeID;
                    _rvi["TicketRaisedCount"] = DataList.Values;
                    dt.Rows.Add(_rvi);
                }
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {

            }
          
            HttpContext.Response.Clear();
            HttpContext.Response.ClearContent();
            HttpContext.Response.ClearHeaders();
            HttpContext.Response.Buffer = true;
            HttpContext.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=TOP10EmployeesRaisedTickets_" + SDate + "_to_" + EDate + ".xls");

            HttpContext.Response.Charset = "utf-8";
            HttpContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Response.Write("<BR><BR><BR>");
            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
            HttpContext.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> ");
            //am getting my grid's column headers
            int columnscount = dt.Columns.Count;

            //HttpContext.Response.Write("<TR>");
            //HttpContext.Response.Write("<Td>");
            //HttpContext.Response.Write("<B>");
            //HttpContext.Response.Write("Iserveinsure");
            //HttpContext.Response.Write("</B>");
            //HttpContext.Response.Write("</Td>");
            //HttpContext.Response.Write("</TR>");
            //HttpContext.Response.Write("<TR>");
            //HttpContext.Response.Write("<Td>");
            //HttpContext.Response.Write("<B>");
            //HttpContext.Response.Write("Refrence No : "+id+"");
            //HttpContext.Response.Write("</B>");
            //HttpContext.Response.Write("</Td>");
            //HttpContext.Response.Write("</TR>");
            HttpContext.Response.Write("<TR>");
            for (int j = 0; j < columnscount; j++)
            {
                //write in new column
                HttpContext.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Response.Write("<B>");
                HttpContext.Response.Write(dt.Columns[j].ToString());
                HttpContext.Response.Write("</B>");
                HttpContext.Response.Write("</Td>");
            }

            HttpContext.Response.Write("</TR>");
            foreach (DataRow row in dt.Rows)
            {//write in new row
                HttpContext.Response.Write("<TR>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    HttpContext.Response.Write("<Td>");
                    HttpContext.Response.Write(row[i].ToString());
                    HttpContext.Response.Write("</Td>");
                }

                HttpContext.Response.Write("</TR>");
            }
            HttpContext.Response.Write("</Table>");
            HttpContext.Response.Write("</font>");
            HttpContext.Response.Flush();
            HttpContext.Response.End();

            return Redirect("/home/top10");
        }

        public ActionResult Top10DetectedIntentExport(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("DetectedIntent");
            dt.Columns.Add("DetectedIntentCount");
            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference("SolutionProvidedReport");
                string StartdateString = SDate;//"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate; //"2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.DetectedIntent).Take(10).OrderByDescending(g => g.Count()).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    DataList.DetectedIntent = (singleData.Key).ToString();
                    DataList.Values = singleData.Count();
                    DataRow _rvi = dt.NewRow();
                    _rvi["DetectedIntent"] = DataList.DetectedIntent;
                    _rvi["DetectedIntentCount"] = DataList.Values;
                    dt.Rows.Add(_rvi);
                   
                }
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {

            }

            HttpContext.Response.Clear();
            HttpContext.Response.ClearContent();
            HttpContext.Response.ClearHeaders();
            HttpContext.Response.Buffer = true;
            HttpContext.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=TOP10DetectedIntent_"+ SDate +"_to_"+ EDate + ".xls");

            HttpContext.Response.Charset = "utf-8";
            HttpContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Response.Write("<BR><BR><BR>");
            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
            HttpContext.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> ");
            //am getting my grid's column headers
            int columnscount = dt.Columns.Count;

            //HttpContext.Response.Write("<TR>");
            //HttpContext.Response.Write("<Td>");
            //HttpContext.Response.Write("<B>");
            //HttpContext.Response.Write("Iserveinsure");
            //HttpContext.Response.Write("</B>");
            //HttpContext.Response.Write("</Td>");
            //HttpContext.Response.Write("</TR>");
            //HttpContext.Response.Write("<TR>");
            //HttpContext.Response.Write("<Td>");
            //HttpContext.Response.Write("<B>");
            //HttpContext.Response.Write("Refrence No : "+id+"");
            //HttpContext.Response.Write("</B>");
            //HttpContext.Response.Write("</Td>");
            //HttpContext.Response.Write("</TR>");
            HttpContext.Response.Write("<TR>");
            for (int j = 0; j < columnscount; j++)
            {
                //write in new column
                HttpContext.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Response.Write("<B>");
                HttpContext.Response.Write(dt.Columns[j].ToString());
                HttpContext.Response.Write("</B>");
                HttpContext.Response.Write("</Td>");
            }

            HttpContext.Response.Write("</TR>");
            foreach (DataRow row in dt.Rows)
            {//write in new row
                HttpContext.Response.Write("<TR>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    HttpContext.Response.Write("<Td>");
                    HttpContext.Response.Write(row[i].ToString());
                    HttpContext.Response.Write("</Td>");
                }

                HttpContext.Response.Write("</TR>");
            }
            HttpContext.Response.Write("</Table>");
            HttpContext.Response.Write("</font>");
            HttpContext.Response.Flush();
            HttpContext.Response.End();

            return Redirect("/home/top10");
        }
        public List<T> RetrieveEntity<T>(string Query = null) where T : TableEntity, new()
        {

            try
            {
                // Create the Table Query Object for Azure Table Storage  
                TableQuery<T> DataTableQuery = new TableQuery<T>();
                if (!String.IsNullOrEmpty(Query))
                {
                    DataTableQuery = new TableQuery<T>().Where(Query);
                }
                IEnumerable<T> IDataList = table.ExecuteQuery(DataTableQuery);
                List<T> DataList = new List<T>();
                foreach (var singleData in IDataList)
                    DataList.Add(singleData);
                return DataList;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
    }
}