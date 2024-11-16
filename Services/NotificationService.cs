using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace HomoeopathyWorld.Services
{
    public class NotificationService :INotificationService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public NotificationService()
        {
        }
        
        
        public async Task<bool> Notification_Check()
        {
            try
            {
                /*
                SqlParameter paramUserType = new SqlParameter() { ParameterName = "@UserType", SqlDbType = SqlDbType.TinyInt, Value = ObjUser.UserType };
                SqlParameter paramUserID = new SqlParameter() { ParameterName = "@UserID", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID };
               */
                SqlParameter paramHasNotification = new SqlParameter
                {
                    ParameterName = "@HasNotification",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspNotification_Check @UserType, @UserID, @HasNotification OUTPUT";
                //_context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserType, paramUserID, paramHasNotification);

                return Convert.ToBoolean(paramHasNotification.Value);
            }
            catch (Exception e)
            {
                //await ErrorLog(1, e.Message, $"Notification_Check", 1);
                return false;
            }
        }

        
        public async Task<string> Notification_GetAll(bool IsViewAll)
        {
            try
            {
                var strResponse = "";
                /*
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspNotification_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserType", SqlDbType = SqlDbType.TinyInt, Value = (Int32)ObjUser.UserType });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserID", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@IsViewAll", SqlDbType = SqlDbType.Bit, Value = IsViewAll });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
                */
                return strResponse;
            }
            catch (Exception e)
            {
                return "Error, Something wrong!";
            }
        }
    }
}
