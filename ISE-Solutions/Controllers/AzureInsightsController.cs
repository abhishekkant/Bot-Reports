using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {

            var webAddr = "https://api.applicationinsights.io/v1/apps/DEMO_APP/metrics/users/count?timespan=P1D&interval=PT3H&top=10";
            var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create(webAddr);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("x-api-key", "DEMO_KEY");
            httpWebRequest.ContentLength = 0;
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                result = result.Replace("users/count", "userscount");
                var model = JsonConvert.DeserializeObject<RootObject>(result);
            }

            return View();
        }


        public class UsersCount
        {
            public int unique { get; set; }
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
    }

    }
}