using System.Data;
using ISE_Solutions.Model;
using Utility;

namespace DataLayer.Interface
{
    public interface IConfiguration
    {
      
        UserAuthViewModel UserAuthentication(string UserName, string Password);
        DataSet UserAuthenticationDetails(int UserId);

        MsgNotification IsAlreadyExists(string TableName, string WhereClause);
    }
}
