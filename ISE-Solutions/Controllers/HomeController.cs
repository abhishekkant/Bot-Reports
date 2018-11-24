using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ISE_Solutions.Model;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace ISE_Solutions.Controllers
{
    public class HomeController : Controller//  : BaseController
    {
        private CloudTable table;
        public ActionResult Index()
        {
           
            return View("Login");
        }
        public ActionResult addrow()
        {
            return View();
        }

        #region LoginAuth
        public Boolean UserAuthentication(string UserId, string Password)
        {
            Boolean _result = false;
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();

            try
            {
                string a = Convert.ToString(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

                Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference("ReportsUsers");

               // await table.CreateIfNotExistsAsync();

                List<HomeLoginViewModel> SutdentListObj = RetrieveEntity<HomeLoginViewModel>();

                var SutdentListObj1 = SutdentListObj.Where(item => item.Username == UserId && item.Password == Password).ToList();

               if(SutdentListObj1.Count >0)
                {
                    _result = true;
                }
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {

            }

            return _result;
        }
        #endregion
        #region DashboardChart
        [HttpGet]
        public async Task<JsonResult> GetIsSolvedReport(string SDate, string EDate)
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

                string StartdateString = SDate; //"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate;// "2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();

                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.Timestamp.Date).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.Timestamp1 = (singleData.Key).ToString();
                    foreach (var result in singleData)
                    {
                        if (result.IsSolved == true)
                        {
                            DataList.isSolvedTrue += 1;
                        }
                        else
                        {
                            DataList.isSolvedFalse += 1;
                        }
                    }
                    resultdata.TotalSolved += DataList.isSolvedTrue; //+ ", ";
                    resultdata.TotalUnSolved += DataList.isSolvedFalse;// + ", ";
                    resultdata.Dates += Convert.ToDateTime(DataList.Timestamp1).ToString("dd MMM");// + ", ";

                    ResultRecordJson.Add(resultdata);
                    //IsSolvedRecordJson.Add(DataList);
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
              return Json(output,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetRatingReport(string SDate, string EDate)
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

                string StartdateString = SDate; //"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate;// "2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.Timestamp.Date).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.Timestamp1 = (singleData.Key).ToString();
                    foreach (var result in singleData)
                    {
                        if (result.Rating > 0)
                        {
                            DataList.isRatingTrue += 1;
                        }
                        else
                        {
                            DataList.isRatingFalse += 1;
                        }
                    }
                    resultdata.TotalNoRating += DataList.isRatingFalse; //+ ", ";
                    resultdata.TotalRating += DataList.isRatingTrue;// + ", ";
                    resultdata.Dates += Convert.ToDateTime(DataList.Timestamp1).ToString("dd MMM");// + ", ";

                    ResultRecordJson.Add(resultdata);
                    //IsSolvedRecordJson.Add(DataList);
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

        [HttpGet]
        public async Task<JsonResult> GetAvgRatingReport(string SDate, string EDate)
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
                
                table = tableClient.GetTableReference("SolutionProvidedReport");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate; //"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate;// "2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.Timestamp.Date).ToList();
                //    .Select(g => new {
                //    Date = g.Key,
                //    Count = g.Count(),
                //    Total = g.Sum(i => i.Rating),
                //    Average = g.Average(i => i.Rating)
                //}).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.Timestamp1 = (singleData.Key).ToString();
                    foreach (var result in singleData)
                    {
                        if (result.Rating > 0)
                        {
                            DataList.RatingTotal += result.Rating;
                            DataList.RatingCount += 1;
                        }
                        else
                        {
                            DataList.isSolvedFalse += 1;
                        }
                    }
                    if (DataList.RatingTotal > 0 && DataList.RatingCount > 0)
                    {
                        resultdata.AvgRating += (DataList.RatingTotal / DataList.RatingCount);
                    }
                    else resultdata.AvgRating = 0;
                   
                    resultdata.Dates += Convert.ToDateTime(DataList.Timestamp1).ToString("dd MMM");// + ", ";
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
        public async Task<JsonResult> GetTicketRaiseHistoryReport(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<PieChartSolutionResult> ResultRecordJson = new List<PieChartSolutionResult>();

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
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.QueryCategory).ToList();
               
                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    PieChartSolutionResult resultdata = new PieChartSolutionResult();
                    DataList.Department = (singleData.Key).ToString();
                    DataList.Values = singleData.Count();
                    resultdata.value += DataList.Values;
                    resultdata.category += DataList.Department;
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
        public async Task<JsonResult> GetFailedTicketSubmissionReport(string SDate, string EDate)
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
                table = tableClient.GetTableReference("FailedTicketSubmission");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate; //"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate;// "2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.Timestamp.Date).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.Timestamp1 = (singleData.Key).ToString();
                    DataList.FailedTicket = singleData.Count();

                   // resultdata.TotalNoRating += DataList.FailedTicket; //+ ", ";
                    resultdata.FailedTicketCount += DataList.FailedTicket;// + ", ";
                    resultdata.Dates += Convert.ToDateTime(DataList.Timestamp1).ToString("dd MMM");// + ", ";

                    ResultRecordJson.Add(resultdata);
                    //IsSolvedRecordJson.Add(DataList);
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
        public async Task<JsonResult> GetTicketRaisedChart(string SDate, string EDate)
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

                string StartdateString = SDate; //"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate;// "2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.Timestamp.Date).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.Timestamp1 = (singleData.Key).ToString();
                    DataList.TicketRaised = singleData.Count();

                    // resultdata.TotalNoRating += DataList.FailedTicket; //+ ", ";
                    resultdata.TicketRaisedCount += DataList.TicketRaised;// + ", ";
                    resultdata.Dates += Convert.ToDateTime(DataList.Timestamp1).ToString("dd MMM");// + ", ";

                    ResultRecordJson.Add(resultdata);
                    //IsSolvedRecordJson.Add(DataList);
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
        public async Task<JsonResult> GetExceptionLogChart(string SDate, string EDate)
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
                table = tableClient.GetTableReference("ExceptionLog");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate; //"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate;// "2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.Timestamp.Date).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.Timestamp1 = (singleData.Key).ToString();
                    DataList.ExceptionLog = singleData.Count();

                    // resultdata.TotalNoRating += DataList.FailedTicket; //+ ", ";
                    resultdata.ExceptionLogCount += DataList.ExceptionLog;// + ", ";
                    resultdata.Dates += Convert.ToDateTime(DataList.Timestamp1).ToString("dd MMM");// + ", ";

                    ResultRecordJson.Add(resultdata);
                    //IsSolvedRecordJson.Add(DataList);
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

        [HttpGet]
        public async Task<JsonResult> GetIntentNotunderstoodChart(string SDate, string EDate)
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
                table = tableClient.GetTableReference("IntentNotunderstood");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate; //"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate;// "2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).GroupBy(item => item.Timestamp.Date).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    DataList.Timestamp1 = (singleData.Key).ToString();
                    DataList.IntentNotunderstood = singleData.Count();

                    // resultdata.TotalNoRating += DataList.FailedTicket; //+ ", ";
                    resultdata.IntentNotunderstoodCount += DataList.IntentNotunderstood;// + ", ";
                    resultdata.Dates += Convert.ToDateTime(DataList.Timestamp1).ToString("dd MMM");// + ", ";

                    ResultRecordJson.Add(resultdata);
                    //IsSolvedRecordJson.Add(DataList);
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

        #endregion
        #region DashboardChartinTable
        public ActionResult answerquery()
        {

            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetAnswerqueryReport(string SDate, string EDate)
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
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.IsSolved).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    // DataList.EmployeeID = (singleData.Key).ToString();
                    //DataList.Values = singleData.Count();
                    resultdata.DetectedIntent += singleData.DetectedIntent;
                    resultdata.Issue += singleData.Issue;
                    resultdata.SolvedStatus += Convert.ToString(singleData.IsSolved);
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
        public ActionResult tickets()
        {

            return View();
        }
        public async Task<JsonResult> GetTicketgrid(string SDate, string EDate)
        {
            string TotalSolved = String.Empty; string TotalUnSolved = String.Empty; string Dates = String.Empty;
            List<SolutionProvidedReportValues> IsSolvedRecordJson = new List<SolutionProvidedReportValues>();
            List<PieChartSolutionResult> ResultRecordJson = new List<PieChartSolutionResult>();

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
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    PieChartSolutionResult resultdata = new PieChartSolutionResult();
                    // DataList.Department = (singleData.Key).ToString();
                    // DataList.Values = singleData.Count();

                    resultdata.EmployeeID += singleData.EmployeeID;
                    resultdata.TicketID += singleData.TicketID;
                    resultdata.SubCategory += singleData.SubCategory;
                    resultdata.QueryCategory += singleData.QueryCategory;
                    resultdata.category += singleData.Category;
                    resultdata.Location += singleData.Location;
                    resultdata.Status += singleData.Status;
                    resultdata.Description += singleData.Description;
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
        public ActionResult intentnotunderstand()
        {

            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetIntentNotunderstoodGrid(string SDate, string EDate)
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
                table = tableClient.GetTableReference("IntentNotunderstood");

                await table.CreateIfNotExistsAsync();

                string StartdateString = SDate; //"2018-10-25T00:00:00.000Z";
                string EnddateString = EDate;// "2018-11-10T00:00:00.000Z";
                DateTime StartDate = DateTime.Parse(StartdateString, System.Globalization.CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.Parse(EnddateString, System.Globalization.CultureInfo.InvariantCulture);

                List<SolutionProvidedReport> SutdentListObj = RetrieveEntity<SolutionProvidedReport>();
                var SutdentListObj1 = SutdentListObj.Where(item => item.Timestamp >= StartDate && item.Timestamp <= EndDate).OrderByDescending(item => item.Timestamp).ToList();

                foreach (var singleData in SutdentListObj1)
                {
                    SolutionProvidedReportValues DataList = new SolutionProvidedReportValues();
                    SolutionResult resultdata = new SolutionResult();
                    //DataList.Timestamp1 = (singleData.Key).ToString();
                    //DataList.IntentNotunderstood = singleData.Count();

                    // resultdata.TotalNoRating += DataList.FailedTicket; //+ ", ";
                    resultdata.FunctionLocation += singleData.FunctionLocation;
                    resultdata.ExceptionMessage += singleData.ExceptionMessage;
                    resultdata.Query += singleData.Query;
                    resultdata.Dates += Convert.ToDateTime(singleData.Timestamp.DateTime).ToString("dd-MMM-yyyy");// + ", ";

                    ResultRecordJson.Add(resultdata);
                    //IsSolvedRecordJson.Add(DataList);
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
        public ActionResult DashBoard(HomeLoginViewModel objLogin)
        {

            return View();

        }
        #endregion

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