using ISE_Solutions.Model;
using System.Web.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage.Table;
//using Microsoft.Azure.CosmosDB.Table; // Namespace for Table storage types
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;


namespace ISE_Solutions.Controllers
{
    public class top10Controller : Controller
    {
        private CloudTable table;
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Top10()
        {

            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetTop10TicketRaiseHistoryReport(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();

            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                // Create the table client.
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                // Retrieve a reference to the table.
                // CloudTable table = tableClient.GetTableReference("SolutionProvidedReport");
                table = tableClient.GetTableReference("TicketRaisedHistory");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate;
                string EnddateString = EDate;
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.EmployeeID).Take(10).OrderByDescending(g => g.Count()).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.EmployeeID = (singleData.Key).ToString();
                    DataList.Values = singleData.Count();
                    resultdata.EmployeeID += DataList.EmployeeID;
                    resultdata.TicketRaisedCount += DataList.Values;
                    ResultRecordJson.Add(resultdata);

                }
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {

            }
            var output = JsonConvert.SerializeObject(ResultRecordJson);
          
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetTop10DetectedIntent(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();

            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                // Create the table client.
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                // Retrieve a reference to the table.
                // CloudTable table = tableClient.GetTableReference("SolutionProvidedReport");
                table = tableClient.GetTableReference("SolutionProvidedReport");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate;
                string EnddateString = EDate;
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.DetectedIntent).Take(10).OrderByDescending(g => g.Count()).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.DetectedIntent = (singleData.Key).ToString();
                    DataList.Values = singleData.Count();
                    resultdata.DetectedIntent += DataList.DetectedIntent;
                    resultdata.DetectedIntentCount += DataList.Values;
                    ResultRecordJson.Add(resultdata);

                }
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {

            }
            var output = JsonConvert.SerializeObject(ResultRecordJson);
          
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetTop10UserQuestionHistoryReport(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();

            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                // Create the table client.
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                // Retrieve a reference to the table.
                // CloudTable table = tableClient.GetTableReference("SolutionProvidedReport");
                table = tableClient.GetTableReference("SolutionProvidedReport");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate;
                string EnddateString = EDate;
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.UserId).Take(10).OrderByDescending(g => g.Count()).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                   // DataList.EmployeeID = (singleData.Key).ToString();
                   // DataList.Values = singleData.Count();
                    resultdata.Userid += (singleData.Key).ToString();
                    resultdata.QuestionCount += singleData.Count();
                    ResultRecordJson.Add(resultdata);

                }
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {

            }
            var output = JsonConvert.SerializeObject(ResultRecordJson);
            
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public ActionResult userquestion(string id)
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetUserQuestionHistoryReport(string SDate, string EDate, string Userid)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();

            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                // Create the table client.
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                // Retrieve a reference to the table.
                // CloudTable table = tableClient.GetTableReference("SolutionProvidedReport");
                table = tableClient.GetTableReference("SolutionProvidedReport");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate;
                string EnddateString = EDate;
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => (item.Timestamp >= StartDate && item.Timestamp <= EndDate) && item.UserId== Userid).OrderByDescending(item => item.Timestamp).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    // DataList.EmployeeID = (singleData.Key).ToString();
                    // DataList.Values = singleData.Count();
                    resultdata.Userid += singleData.UserId;
                    resultdata.Issue += singleData.Issue;
                    resultdata.SolvedStatus += singleData.IsSolved;
                    resultdata.TotalRating += singleData.Rating;
                    resultdata.Dates += Convert.ToDateTime(singleData.Timestamp.DateTime).ToString("dd-MMM-yyyy");
                    ResultRecordJson.Add(resultdata);

                }
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {

            }
            var output = JsonConvert.SerializeObject(ResultRecordJson);
            // var resultData = new {TotalSolved = TotalSolved, TotalUnSolved = TotalUnSolved, Dates = Dates };

            // return Json(resultData, JsonRequestBehavior.AllowGet);
            //return Json(c, JsonRequestBehavior.AllowGet);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult intentquestion(string id)
        {
            return View();
        }

        public async Task<JsonResult> GetIntentIssesGrid(string SDate, string EDate, string DetectedIntent)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();

            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference("SolutionProvidedReport");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate;
                string EnddateString = EDate;
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => ((item.Timestamp >= StartDate && item.Timestamp <= EndDate) && item.DetectedIntent == DetectedIntent)).OrderByDescending(item => item.Timestamp).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();

                    resultdata.DetectedIntent += DetectedIntent;
                    resultdata.Issue += singleData.Issue;
                    resultdata.Dates += Convert.ToDateTime(singleData.Timestamp.DateTime).ToString("dd-MMM-yyyy");
                    resultdata.SolvedStatus += singleData.IsSolved;
                    ResultRecordJson.Add(resultdata);

                }
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {

            }
            var output = JsonConvert.SerializeObject(ResultRecordJson);
            return Json(output, JsonRequestBehavior.AllowGet);
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