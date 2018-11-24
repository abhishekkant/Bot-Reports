using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ISE_Solutions.Controllers
{
    public class AzureInsightsController : Controller
    {
        // GET: AzureInsights
        String AzureInsightsApplicationID = Convert.ToString(ConfigurationManager.AppSettings["AzureInsightsApplicationID"]);
        String AzureInsightsAppKey = Convert.ToString(ConfigurationManager.AppSettings["AzureInsightsAppKey"]);
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetRequestCount(string Timespan, string Interval)
        {
            var webAddr = "https://api.applicationinsights.io/v1/apps/"+ AzureInsightsApplicationID + "/metrics/requests/count?timespan=" + Timespan + "&interval="+ Interval + "";//&top=10
            var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create(webAddr);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("x-api-key", ""+AzureInsightsAppKey+"");
            httpWebRequest.ContentLength = 0;
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            try
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = result.Replace("requests/count", "userscount");
                    var model = JsonConvert.DeserializeObject<RootObject>(result);
                   
                    foreach (var singleData in model.value.segments)
                    {
                       
                        SolutionResult resultdata = new SolutionResult();
                        resultdata.Uniquecount = singleData.userscount.sum;
                        resultdata.Date = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(singleData.end), INDIAN_ZONE).ToString("dd MMM, HH:MM"); 
                        ResultRecordJson.Add(resultdata);
                    }
                 
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
        public JsonResult GetsessionsCount(string Timespan, string Interval)
        {
            var webAddr = "https://api.applicationinsights.io/v1/apps/" + AzureInsightsApplicationID + "/metrics/sessions/count?timespan=" + Timespan + "&interval=" + Interval + "";//&top=10
            var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create(webAddr);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("x-api-key", "" + AzureInsightsAppKey + "");
            httpWebRequest.ContentLength = 0;
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            List<SolutionResult> ResultRecordJson = new List<SolutionResult>();
            try
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = result.Replace("sessions/count", "userscount");
                    var model = JsonConvert.DeserializeObject<RootObject>(result);

                    foreach (var singleData in model.value.segments)
                    {

                        SolutionResult resultdata = new SolutionResult();
                        resultdata.Uniquecount = singleData.userscount.unique;
                        resultdata.Date = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(singleData.end), INDIAN_ZONE).ToString("dd MMM, HH:MM");
                        ResultRecordJson.Add(resultdata);
                    }

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
        public class UsersCount
        {
            public int unique { get; set; }
            public int sum { get; set; }
            
        }

        public class Segment
        {
            public DateTime start { get; set; }
            public DateTime end { get; set; }
            public UsersCount userscount { get; set; }
        }

        public class Value
        {
            public DateTime start { get; set; }
            public DateTime end { get; set; }
            public string interval { get; set; }
            public List<Segment> segments { get; set; }
        }

        public class RootObject
        {
            public Value value { get; set; }
           // public List<SolutionResult> SolutionResult { get; set; }
        }
        public class SolutionResult
        {
            public int Uniquecount { get; set; }
            public string Date { get; set; }
        }
    }
}