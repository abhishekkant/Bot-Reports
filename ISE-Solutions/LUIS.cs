using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ISE_Solutions
{
    public class LUIS
    {
        public async Task GetIntent()
        {
            var hcLUIS = new HttpClient();
            var strQuery = HttpUtility.ParseQueryString(string.Empty);

            var ID = "d0dcd219-050f-458f-83a5-df7da3757248";
            var Key = "709fe955f19e466785ed6567b1c3c7b4";

            hcLUIS.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Key);

            strQuery["q"] = "excuse me";

            strQuery["timezoneOffset"] = "0";
            strQuery["staging"] = "false";

            var URL = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + ID + "?" + strQuery;
            var res = await hcLUIS.GetAsync(URL);

            var strContent = await res.Content.ReadAsStringAsync();

            //  Console.WriteLine(strContent.ToString());
        }
    }
}