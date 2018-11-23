using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace ISE_Solutions.Model
{
    public class SolutionProvidedReport : TableEntity
    {
        public string Issue { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp1 { get; set; }
        public bool? IsSolved { get; set; }
        public int Rating { get; set; }
        public string QueryCategory { get; set; }
        public string Category { get; set; }
        public string DetectedIntent { get; set; }

        public string EmployeeID { get; set; }
        public String FunctionLocation { get; set; }
        public String ExceptionMessage { get; set; }
        public String Query { get; set; }
        public List<SolutionProvidedReportValues> IsSolvedRecord { get; set; }
        public List<SolutionResult> SolutionResult { get; set; }

        public String Description { get; set; }
        public String TicketID { get; set; }
        public String SubCategory { get; set; }
        public String Location { get; set; }
        public String Status { get; set; }

    }
    public class SolutionProvidedReportValues : TableEntity
    {
        public String Timestamp1 { get; set; }
        public bool? IsSolved { get; set; }
        public int isSolvedTrue { get; set; }
        public int isSolvedFalse { get; set; }
        public int isRatingFalse { get; set; }
        public int isRatingTrue { get; set; }
        public int RatingCount { get; set; }
        public int RatingTotal { get; set; }
        public String Department { get; set; }
        public String Issue { get; set; }
        public String SolvedStatus { get; set; }
        public String EmployeeID { get; set; }
        public String DetectedIntent { get; set; }
        public int Values { get; set; }
        public int FailedTicket { get; set; }
        public int TicketRaised { get; set; }
        public int ExceptionLog { get; set; }
        public int IntentNotunderstood { get; set; }

    }
    public class SolutionResult : TableEntity
    {
        public String TotalSolved { get; set; }
        public String TotalUnSolved { get; set; }
        public String TotalRating { get; set; }
        public String TotalNoRating { get; set; }
        public String Dates { get; set; }
        public String EmployeeID { get; set; }
        public String DetectedIntent { get; set; }
        public String Issue { get; set; }

        public String SolvedStatus { get; set; }
        public int AvgRating { get; set; }

        public int FailedTicketCount { get; set; }
        public int TicketRaisedCount { get; set; }
        public String Userid { get; set; }
        public int QuestionCount { get; set; }
        public int ExceptionLogCount { get; set; }

        public int IntentNotunderstoodCount { get; set; }
        public int DetectedIntentCount { get; set; }

        public String FunctionLocation { get; set; }
        public String ExceptionMessage { get; set; }
        public String Query { get; set; }




    }
    public class PieChartSolutionResult : TableEntity
    {
        public String category { get; set; }
        public int value { get; set; }
        public String Department { get; set; }
        public String EmployeeID { get; set; }
        public String Description { get; set; }
        public String TicketID { get; set; }
        public String SubCategory { get; set; }
        public String Location { get; set; }
        public String Status { get; set; }
        public String QueryCategory { get; set; }

        public String Dates { get; set; }
    }
    public class ComplaintsEntity : TableEntity

    {
        public ComplaintsEntity() { }
        public ComplaintsEntity(string pkey, string rkey)
        {
            this.PartitionKey = pkey;
            this.RowKey = rkey;
        }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobileNo { get; set; }

    }
    public class HomeLoginViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
       
        public string Message { get; set; }
      
    }
    
}
