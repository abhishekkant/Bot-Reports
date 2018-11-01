using System;
using System.Data;
using System.Data.SqlClient;
using DataLayer.Interface;
using DBConnectivity;
using ISE_Solutions.Model;
using Utility;

namespace DataLayer
{
    public class ConfigurationDBL : IConfiguration
    {
        SQLConnection DBC;
        String Query;

        public ConfigurationDBL()
        {
            DBC = new SQLConnection();
        }


        public DataSet UserAuthenticationDetails(int UserId)
        {
            DataSet ds = null;
            try
            {
                if (!DBC.isConnected)
                {
                    DBC.Connect();
                    if (DBC.HasError()) { throw new SQLProcessException("Error in Connection: " + DBC.LastError); }
                }
                SqlParameter[] param = new SqlParameter[1];
                param[0] = DBC.CreateParameter("@UserId", SqlDbType.Int, UserId);
                if (DBC.isConnected)
                {
                    Query = "UserAuthenticationDetails";
                    DBC.ExecuteProcedure(Query, "UserAuth", ref ds, ref param);
                }

            }
            catch (SQLProcessException ex)
            {
                throw new SQLProcessException(ex.Message);
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {
                DBClose();
            }
            return ds;
        }


        public UserAuthViewModel UserAuthentication(string UserName,string Password)
        {
            DataSet ds = null;
            DataSet UserAuthDetailds = null;
            UserAuthViewModel model = null;
            try
            {
                if (!DBC.isConnected)
                {
                    DBC.Connect();
                    if (DBC.HasError()) { throw new SQLProcessException("Error in Connection: " + DBC.LastError); }
                }
                SqlParameter[] param = new SqlParameter[2];
                param[0] = DBC.CreateParameter("@UserName", SqlDbType.NVarChar, UserName);
                param[1] = DBC.CreateParameter("@Password", SqlDbType.NVarChar, Password);
                if (DBC.isConnected)
                {
                    Query = "UserAuthentication";
                    DBC.ExecuteProcedure(Query, "UserAuth", ref ds, ref param);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model = new UserAuthViewModel();
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAuthenticate"].ToString()))
                    {
                        model.IsAuthenticate = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAuthenticate"]);
                        model.ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                        param = new SqlParameter[1];
                        param[0] = DBC.CreateParameter("@ID", SqlDbType.Int, Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]));
                        Query = "UserAuthenticationDetails";
                        DBC.ExecuteProcedure(Query, "UserAuthDetail", ref UserAuthDetailds, ref param);

                        model.UserAuthDetail.FirstName = UserAuthDetailds.Tables[0].Rows[0]["FirstName"].ToString();
                        model.UserAuthDetail.LastName = UserAuthDetailds.Tables[0].Rows[0]["LastName"].ToString();
                        model.UserAuthDetail.UserName = UserAuthDetailds.Tables[0].Rows[0]["EmployeeId"].ToString();
                        model.UserAuthDetail.Email = UserAuthDetailds.Tables[0].Rows[0]["Email"].ToString();
                        model.UserAuthDetail.RoleID = Convert.ToInt32(UserAuthDetailds.Tables[0].Rows[0]["RollId"]);
                        model.UserAuthDetail.RoleName = UserAuthDetailds.Tables[0].Rows[0]["RoleName"].ToString();
                    }
                    else
                    {
                        model.IsAuthenticate = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAuthenticate"]);
                        model.ID = 0;
                    }
                }

            }
            catch (SQLProcessException ex)
            {
                throw new SQLProcessException(ex.Message);
            }
            catch (Exception ex)
            {
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {
                DBClose();
            }
            return model;
        }


        public MsgNotification IsAlreadyExists(string TableName, string WhereClause)
        {
            DataSet ds = null;
            MsgNotification msg = new MsgNotification();
            try
            {
                if (!DBC.isConnected)
                {
                    DBC.Connect();
                    if (DBC.HasError()) { throw new SQLProcessException("Error in Connection: " + DBC.LastError); }
                }
                SqlParameter[] param = new SqlParameter[2];
                param[0] = DBC.CreateParameter("@TableName", SqlDbType.NVarChar, TableName);
                param[1] = DBC.CreateParameter("@Where", SqlDbType.NVarChar, WhereClause);
                if (DBC.isConnected)
                {
                    Query = "IsAlreadyExists";
                    DBC.ExecuteProcedure(Query, "UserAuth", ref ds, ref param);
                    msg.Massage = ds.Tables[0].Rows[0]["Msg"].ToString();
                    msg.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                }

            }
            catch (SQLProcessException ex)
            {
                msg.Massage = ex.Message;
                msg.Status = "error";
                throw new SQLProcessException(ex.Message);
            }
            catch (Exception ex)
            {
                msg.Massage = ex.Message;
                msg.Status = "error";
                Utility.Utility.GenrateLog(ex.Message);
            }
            finally
            {
                DBClose();
            }
            return msg;
        }


        private void DBClose()
        {
            if (DBC != null)
            {
                DBC.Reset();
                DBC = null;
            }
        }
    }
   
}
