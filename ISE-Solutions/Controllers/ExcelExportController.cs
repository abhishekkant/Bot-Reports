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
        #region ExcelExport
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
            if (dt.Rows.Count > 0)
                ExcelExport(dt, SDate, EDate, "Top10TicketRaiseExport");

            return Redirect("/top10/top10");
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
            if(dt.Rows.Count > 0)
            ExcelExport(dt, SDate, EDate, "TOP10DetectedIntent");
            return Redirect("/top10/top10");
        }

        public ActionResult Top10UserRaisedQusetionExport(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Userid");
            dt.Columns.Add("QuestionCount");
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
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.UserId).Take(10).OrderByDescending(g => g.Count()).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    DataRow _rvi = dt.NewRow();
                    _rvi["Userid"] = singleData.Key;
                    _rvi["QuestionCount"] = singleData.Count();
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
            if (dt.Rows.Count > 0)
                ExcelExport(dt, SDate, EDate, "Top10UserRaisedQusetion");
            return Redirect("/top10/top10");
        }

        public ActionResult AnswerQurey(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("DetectedIntent");
            dt.Columns.Add("Issue");
            dt.Columns.Add("SolvedStatus");
            dt.Columns.Add("Dates");
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
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.IsSolved).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                  
                    DataRow _rvi = dt.NewRow();
                    _rvi["DetectedIntent"] += singleData.DetectedIntent;
                    _rvi["Issue"] += singleData.Issue;
                    _rvi["SolvedStatus"] += Convert.ToString(singleData.IsSolved);
                    _rvi["Dates"] += Convert.ToDateTime(singleData.Timestamp.DateTime).ToString("dd-MMM-yyyy");
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
            if (dt.Rows.Count > 0)
                ExcelExport(dt, SDate, EDate, "AnswerQurey");
            return Redirect("/top10/top10");
        }
        public ActionResult TicketsRaised(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("TicketID");
            dt.Columns.Add("Dates");
            dt.Columns.Add("QueryCategory");
            dt.Columns.Add("category");
            dt.Columns.Add("SubCategory");
            dt.Columns.Add("EmployeeID");
            dt.Columns.Add("Description");
            dt.Columns.Add("Status");
            dt.Columns.Add("Location");
            
            
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
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).ToList();

                foreach (var singleData in SutdentListObj1)
                {

                    DataRow _rvi = dt.NewRow();
                    _rvi["EmployeeID"] += singleData.EmployeeID;
                    _rvi["TicketID"] += singleData.TicketID;
                    _rvi["SubCategory"] += singleData.SubCategory;
                    _rvi["QueryCategory"] += singleData.QueryCategory;
                    _rvi["category"] += singleData.Category;
                    _rvi["Location"] += singleData.Location;
                    _rvi["Status"]  += singleData.Status;
                    _rvi["Description"]  += singleData.Description;
                    _rvi["Dates"]  += Convert.ToDateTime(singleData.Timestamp.DateTime).ToString("dd-MMM-yyyy");
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
            if (dt.Rows.Count > 0)
                ExcelExport(dt, SDate, EDate, "TicketsRaised");
            return Redirect("/top10/top10");
        }
        public ActionResult IntentnotunderstandExport(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Dates");
            dt.Columns.Add("FunctionLocation");
            dt.Columns.Add("ExceptionMessage");
            dt.Columns.Add("Query");
          
            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference("IntentNotunderstood");
                string StartdateString = SDate;//"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate; //"2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                  
                    DataRow _rvi = dt.NewRow();
                    _rvi["FunctionLocation"] = singleData.FunctionLocation;
                    _rvi["ExceptionMessage"] = singleData.ExceptionMessage;
                    _rvi["Query"] = singleData.Query;
                    _rvi["Dates"] = Convert.ToDateTime(singleData.Timestamp.DateTime).ToString("dd-MMM-yyyy");
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
            if (dt.Rows.Count > 0)
                ExcelExport(dt, SDate, EDate, "Intentnotunderstand");
            return Redirect("/top10/top10");
        }
        public ActionResult userquestion(string SDate, string EDate , string Userid)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Userid");
            dt.Columns.Add("Issue");
            dt.Columns.Add("Dates");
            dt.Columns.Add("Status");
            dt.Columns.Add("TotalRating");
           
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
                var SutdentListObj1 = SutdentListObj.Where(item => (item.Timestamp >= StartDate && item.Timestamp <= EndDate) && item.UserId == Userid).OrderByDescending(item => item.Timestamp).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    DataRow _rvi = dt.NewRow();
                    _rvi["Userid"] = singleData.UserId;
                    _rvi["Issue"] = singleData.Issue;
                    _rvi["Status"] = singleData.IsSolved;
                    _rvi["TotalRating"] = singleData.Rating;
                    _rvi["Dates"] = Convert.ToDateTime(singleData.Timestamp.DateTime).ToString("dd-MMM-yyyy");
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
            if (dt.Rows.Count > 0)
                ExcelExport(dt, SDate, EDate, "userquestion_"+ Userid + "_");
            return Redirect("/top10/top10");
        }
        public ActionResult intentquestion(string SDate, string EDate, string DetectedIntent)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("DetectedIntent");
            dt.Columns.Add("Issue");
            dt.Columns.Add("Dates");
            dt.Columns.Add("Status");
           

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
                var SutdentListObj1 = SutdentListObj.Where(item => ((item.Timestamp >= StartDate && item.Timestamp <= EndDate) && item.DetectedIntent == DetectedIntent)).OrderByDescending(item => item.Timestamp).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    DataRow _rvi = dt.NewRow();
                    _rvi["DetectedIntent"] = DetectedIntent;
                    _rvi["Issue"] = singleData.Issue;
                    _rvi["Status"] = singleData.IsSolved;
                    _rvi["Dates"] = Convert.ToDateTime(singleData.Timestamp.DateTime).ToString("dd-MMM-yyyy");
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
            if (dt.Rows.Count > 0)
                ExcelExport(dt, SDate, EDate, "intentquestion_"+DetectedIntent+"_");
            return Redirect("/top10/top10");
        }
        public void ExcelExport(DataTable dt, string SDate, string EDate,string filename)
        {
            HttpContext.Response.Clear();
            HttpContext.Response.ClearContent();
            HttpContext.Response.ClearHeaders();
            HttpContext.Response.Buffer = true;
            HttpContext.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename="+ filename + "_" + SDate + "_to_" + EDate + ".xls");

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

        #endregion
    }
}