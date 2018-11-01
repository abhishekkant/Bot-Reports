using System;
using System.IO;
using System.Web;

namespace Utility
{
    /// <summary>
    /// Summary description for ECOMUtility
    /// </summary>
    public class Utility
    {
        public Utility()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        /// <summary>
        /// find client IP Adress
        /// </summary>
        public static string FindClientIP()
        {
            
            string clientIP = "";
            string[] temp = new string[0];
            string actualIP = "";
            if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) == false)
            {
                clientIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                temp = clientIP.Split(',');
                actualIP = temp[0].ToString();
            }
            else
            { actualIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); }
            temp = null;
            return actualIP;
        }

        /// <summary>Encode to base 64</summary>
        public static string base64Encode(string data)
        {
            string encodedData = null;
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                encodedData = Convert.ToBase64String(encData_byte);
            }
            catch (Exception e)
            {
                GenrateLog("Error in base64Encode" + e.Message);
            }
            return encodedData;
        }

        /// <summary>Decode From base 64</summary>
        public static string base64Decode(string data)
        {
            string result = null;
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                result = new String(decoded_char);
            }
            catch (Exception e)
            {
                GenrateLog("Error in base64Decode" + e.Message);
            }
            return result;
        }

        /// <summary>function to check integer variable.</summary>
        public static bool IsIntegerCheck(string getdata) 
        {
            if (string.IsNullOrEmpty(getdata))
                getdata = "0";
            bool result = true;
            try
            { int.Parse(getdata); }
            catch (FormatException)
            { result = false; }
            return result;
        }

        /// <summary>function to check 64 integer variable.</summary>
        public static bool IsInteger64Check(string getdata)
        {
            if (string.IsNullOrEmpty(getdata))
                getdata = "0";
            bool result = true;
            try
            { Int64.Parse(getdata); }
            catch (FormatException)
            { result = false; }
            return result;
        }

        // <summary>use to genrate Log.</summary>
        public static void GenrateLog(String Msg)
       {
           string servername = "";
           string url = "";
           string querystring = "";
           servername = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
           url = HttpContext.Current.Request.ServerVariables["URL"];
           querystring = HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
           String emailbody = "";
           emailbody += "\t" + HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
           emailbody += "\t" + "http://" + servername + url + "?" + querystring;
           if (HttpContext.Current.Request.UrlReferrer != null && HttpContext.Current.Request.UrlReferrer.ToString() != "")
           {
               emailbody += "\t" + HttpContext.Current.Request.UrlReferrer.ToString();
           }
           emailbody += "\t" + Msg.Replace("<br><br>", "\t");
           WriteLog(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"] + "\\dberror\\", emailbody);
       }


       /// <summary>use to Write Log.</summary>
       public static void WriteLog(string sPathName, string sErrMsg)
       {
           string sLogFormat;
           string sErrorTime;

           //sLogFormat used to create log files format :
           // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
           sLogFormat = DateTime.Now.ToShortDateString().ToString() + "\t" + DateTime.Now.ToLongTimeString().ToString() + "\t";

           //this variable used to create log filename format "
           //for example filename : LogYYYYMMDD
           string sYear = DateTime.Now.Year.ToString();
           string sMonth = DateTime.Now.Month.ToString();
           string sDay = DateTime.Now.Day.ToString();
           if (sDay.Length == 1) { sDay = '0' + sDay; }
           if (sMonth.Length == 1) { sMonth = '0' + sMonth; }
           sErrorTime = sDay + "-" + sMonth + "-" + sYear;

           StreamWriter sw = new StreamWriter(sPathName + sErrorTime + ".log", true);
           sw.WriteLine(sLogFormat + sErrMsg);
           sw.Flush();
           sw.Close();
       }


    }

    public class MsgNotification
    {
        public  string Status { get; set; }
        public  string Massage { get; set; }

        public string ReturnVal { get; set; }

    }

}
public class SQLProcessException : Exception
{
    public SQLProcessException()
        : base("The operation will not display any result.")
    {
    }

    public SQLProcessException(string message)
        : base(message)
    {
    }

    public SQLProcessException(string message, Exception inner)
        : base(message, inner)
    {
    }
}