using BMS_API.Models;
using BMS_API.Models.Utility;
using BMS_API.Services.Interface;
using BudgetManagement.Controllers;
using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using static BMS_API.Services.Interface.ICommon;

namespace BudgetManagement.Services
{
    public class AuthService : AuthCommonController, IAuthService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public AuthService(BMSContext context) : base(context)
        {
            _context = context;
        }
        public AuthorisedUser ObjUser { get; set; }
        #region Login
        [NonAction]
        public async Task<LoginResponse> Login(AuthLogin userInfo, string ipAddress)
        {

            var response = new LoginResponse();

            if (userInfo == null)
            {
                response.Status = 0;
                response.Data = null;
                response.Message = "User information is required.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(userInfo.UserName) ||
                string.IsNullOrWhiteSpace(userInfo.Password) ||
                userInfo.UserType < 0)
            {
                response.Status = 0;
                response.Data = null;
                response.Message = "Username, password, and user type are required.";
                return response;
            }

            try
            {
                string strResponse = "";

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUser_LoginNew";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Value = userInfo.UserName });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Password", SqlDbType = SqlDbType.NVarChar, Value = userInfo.Password });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserType", SqlDbType = SqlDbType.TinyInt, Value = userInfo.UserType });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@RefrenceLink", SqlDbType = SqlDbType.NVarChar, Value = userInfo.RefrenceLink == "null" ? "" : userInfo.RefrenceLink });

                    _context.Database.OpenConnection();

                    using (DbDataReader ddr = await command.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(ddr);
                        dt.TableName = "User";

                        // Check if the DataTable has rows
                        if (dt.Rows.Count == 0)
                        {
                            response.Status = 0;
                            response.Data = new object[] { };
                            response.Message = "Invalid credentials.";
                        }
                        else
                        {
                            // Extract Email, MobileNumber, and LoginToken from the DataTable
                            var userRow = dt.Rows[0];
                            var userData = new
                                {
                                    FullName = userRow["FullName"].ToString(),
                                    Email = userRow["EmailID"].ToString(),
                                    //MobileNo = userRow["MobileNo"] != DBNull.Value ? userRow["MobileNo"].ToString() : null,
                                    LoginToken = userRow["LoginToken"].ToString(),
                                    UserIDP = userRow["UserIDP"].ToString()
                                };

                                response.Status = 200;
                                response.Data = userData;  // Set the structured data
                                response.Message = "Login successful.";
                         }

                            //response.Status = 1;
                            //response.Data = userData;  // Set the structured data
                            //response.Message = "Login successful.";
                        
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                response.Status = 0;
                response.Data = null;
                response.Message = "Invalid credentials." ;
                return response;
            }
        }

        #endregion Examination  Generate Result

        #region vendor login by token
        [NonAction]
        public async Task<AuthorisedUser> VendorLogin_By_Token(Int64 token, UserType userType)
        {
            AuthorisedUser objVendor = new AuthorisedUser();
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspVendorLogin_By_Token";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Token", SqlDbType = SqlDbType.BigInt, Value = token });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserType", SqlDbType = SqlDbType.TinyInt, Value = (Byte)userType});
                     _context.Database.OpenConnection();

                    DbDataReader ddr = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);
                    dt.TableName = "Vendor";
                   

                        if (dt.Rows.Count > 0)
                        {
                            objVendor = new AuthorisedUser
                            {
                                UserID = Convert.ToInt64(dt.Rows[0]["UserID"]),
                                PersonName = dt.Rows[0]["PersonName"].ToString(),
                                UserType = ICommon.UserType.Vendor,
                                CurrentToken = Convert.ToInt64(dt.Rows[0]["GeneratedToken"])
                            };
                        }
                    else
                    {
                        return null;
                    }
                }
                return objVendor;
            }

            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, "uspVendorLogin", 1);
                return null;
            }   
        }

        #endregion
        #region Login By Token
        [NonAction]
        public async Task<AuthorisedUser> Login_By_Token(Int64 token, UserType userType)
        {
            try
            {
                AuthorisedUser objUser = new AuthorisedUser();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUser_Login_ByToken";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Token", SqlDbType = SqlDbType.BigInt, Value = token });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserType", SqlDbType = SqlDbType.TinyInt, Value = (Byte)userType });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@IPAddress", SqlDbType = SqlDbType.NVarChar, Value = "" });
                    _context.Database.OpenConnection();

                    DbDataReader ddr = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);
                    dt.TableName = "User";

                    if (dt.Rows.Count > 0)
                    {
                        objUser.UserID = Convert.ToInt64(dt.Rows[0]["UserID"]);
                        //objUser.FranchiseeIDF = Convert.ToInt64(dt.Rows[0]["FranchiseeIDF"]);
                        objUser.PersonName = dt.Rows[0]["PersonName"].ToString();
                        objUser.UserType = (ICommon.UserType)Convert.ToByte(dt.Rows[0]["UserType"]);
                        objUser.CurrentToken = Convert.ToInt64(dt.Rows[0]["CurrentToken"]);
                    }
                    else
                    {
                        return null;
                    }
                    //strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
                return objUser;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Login_By_Token", 1);
                return null;
            }
        }
        #endregion Login By Token

        #region auth forgot password
        [NonAction]
        public async Task<string> Auth_PasswordForgot(string userName, Int32 userType)
        {
            try
            {
                string strResponse = "";

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUser_ForgetPassword";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Value = userName });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserType", SqlDbType = SqlDbType.TinyInt, Value = userType });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@IPAddress", SqlDbType = SqlDbType.NVarChar, Value = "" });

                    _context.Database.OpenConnection();

                    DbDataReader ddr = await command.ExecuteReaderAsync();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);
                    dt.TableName = "User";

                    if (dt.Rows.Count > 0)
                    {
                        strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    }
                }
                return strResponse;
            }
            catch (Exception e)
            {
                return "Error, Something wrong!";
            }
        }
        #endregion auth forgot password

        #region auth forgot password
        [NonAction]
        public async Task<bool> Auth_PasswordChange(AuthReset newPass)
        {
            try
            {
                SqlParameter paramRowGUID = new SqlParameter() { ParameterName = "@RowGUID", SqlDbType = SqlDbType.UniqueIdentifier, Value = newPass.RowGUID };
                SqlParameter paramUserType = new SqlParameter() { ParameterName = "@userType", SqlDbType = SqlDbType.Int, Value = newPass.UserType };
                SqlParameter paramNewPassword = new SqlParameter() { ParameterName = "@NewPassword", SqlDbType = SqlDbType.NVarChar, Value = newPass.NewPassword };
                SqlParameter paramIsDone = new SqlParameter
                {
                    ParameterName = "@IsDone",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspUser_RecoverPassword @RowGUID, @UserType, @NewPassword, @IsDone OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramRowGUID, paramUserType, paramNewPassword, paramIsDone);

                return Convert.ToBoolean(paramIsDone.Value);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion AUTH FORGOT PASSWORD

        #region AUTH OTP VERIFICATION
        [NonAction]
        public async Task<bool> Auth_OTPVarification(AuthOTPVarification oTPVarification)
        {
            try
            {
                SqlParameter paramOTP = new SqlParameter() { ParameterName = "@OTP", SqlDbType = SqlDbType.Int, Value = oTPVarification.OTP };
                SqlParameter paramToken = new SqlParameter() { ParameterName = "@Token", SqlDbType = SqlDbType.BigInt, Value = (ObjUser == null) ? DBNull.Value : ObjUser.CurrentToken };
                SqlParameter paramUserType = new SqlParameter() { ParameterName = "@UserType", SqlDbType = SqlDbType.TinyInt, Value = oTPVarification.UserType };
                SqlParameter paramIsDone = new SqlParameter
                {
                    ParameterName = "@IsDone",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspUser_OTPVarification @OTP, @Token, @UserType, @IsDone OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramOTP, paramToken, paramUserType, paramIsDone);

                return Convert.ToBoolean(paramIsDone.Value);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion AUTH OTP VERIFICATION

        #region AUTH OTP REQUEST
        [NonAction]
        public async Task<string> Auth_OTPRequest()
        {
            try
            {
                /*
                SqlParameter paramToken = new SqlParameter() { ParameterName = "@Token", SqlDbType = SqlDbType.BigInt, Value = ObjUser.CurrentToken };
                SqlParameter paramOTP = new SqlParameter
                {
                    ParameterName = "@OTP",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspsysUser_OTPRequest @Token, @OTP OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramToken, paramOTP);

                //return Convert.ToBoolean(paramIsDone.Value);
                return Convert.ToInt32(paramOTP.Value);
                */
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUser_OTP_Resend";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Token", SqlDbType = SqlDbType.BigInt, Value = ObjUser.CurrentToken });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserType", SqlDbType = SqlDbType.TinyInt, Value = ObjUser.UserType });

                    _context.Database.OpenConnection();

                    DbDataReader ddr = await command.ExecuteReaderAsync();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);
                    dt.TableName = "User";

                    if (dt.Rows.Count > 0)
                    {
                        strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    }
                }
                return strResponse;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        #endregion AUTH OTP VERIFICATION

        [NonAction]
        public async Task<Int64> Auth_Admin_Change_Password(string userName, string password, string firstName, string lastName, String oldPassword)
        {
            try
            {
                SqlParameter paramUserName = new SqlParameter() { ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Value = userName };
                SqlParameter paramPassword = new SqlParameter() { ParameterName = "@Password", SqlDbType = SqlDbType.NVarChar, Value = password };
                SqlParameter paramFirstName = new SqlParameter() { ParameterName = "@FirstName", SqlDbType = SqlDbType.NVarChar, Value = firstName };
                SqlParameter paramLastName = new SqlParameter() { ParameterName = "@LastName", SqlDbType = SqlDbType.NVarChar, Value = lastName };
                SqlParameter paramOldPassword = new SqlParameter() { ParameterName = "@OldPassword", SqlDbType = SqlDbType.NVarChar, Value = oldPassword };
                SqlParameter paramIsDone = new SqlParameter
                {
                    ParameterName = "@IsDone",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspsysAdmin_Update_Credential @UserName, @Password, @FirstName, @LastName, @OldPassword, @IsDone OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramUserName, paramPassword, paramFirstName, paramLastName, paramOldPassword, paramIsDone);
                return Convert.ToInt32(paramIsDone.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspsysAdmin_Update_Credential", 1);
                return 0;
            }
        }
        [NonAction]
        public async Task<bool> Auth_Admin_Change_Settings(SysAdmin adminInfo)
        {
            try
            {
                SqlParameter paramSMTPEmailAddress = new SqlParameter() { ParameterName = "@SMTPEmailAddress", SqlDbType = SqlDbType.NVarChar, Value = adminInfo.SMTPEmailAddress };
                SqlParameter paramSMTPUserName = new SqlParameter() { ParameterName = "@SMTPUserName", SqlDbType = SqlDbType.NVarChar, Value = adminInfo.SMTPUserName };
                SqlParameter paramSMTPPassword = new SqlParameter() { ParameterName = "@SMTPPassword", SqlDbType = SqlDbType.NVarChar, Value = adminInfo.SMTPPassword };
                SqlParameter paramSMTPHost = new SqlParameter() { ParameterName = "@SMTPHost", SqlDbType = SqlDbType.NVarChar, Value = adminInfo.SMTPHost };
                SqlParameter paramSMTPPort = new SqlParameter() { ParameterName = "@SMTPPort", SqlDbType = SqlDbType.Int, Value = adminInfo.SMTPPort };
                SqlParameter paramSMTPSSL = new SqlParameter() { ParameterName = "@SMTPSSL", SqlDbType = SqlDbType.Bit, Value = adminInfo.SMTPSSL };
                SqlParameter paramGoogleMapKey = new SqlParameter() { ParameterName = "@GoogleMapKey", SqlDbType = SqlDbType.NVarChar, Value = adminInfo.GoogleMapKey };
                SqlParameter paramGoogleLocationKey = new SqlParameter() { ParameterName = "@GoogleLocationKey", SqlDbType = SqlDbType.NVarChar, Value = adminInfo.GoogleLocationKey };
                SqlParameter paramPaymentGatewayKey = new SqlParameter() { ParameterName = "@PaymentGatewayKey", SqlDbType = SqlDbType.NVarChar, Value = adminInfo.PaymentGatewayKey };
                SqlParameter paramRevenueCommission = new SqlParameter() { ParameterName = "@RevenueCommission", SqlDbType = SqlDbType.NVarChar, Value = adminInfo.RevenueCommission };

                var paramSqlQuery = "EXECUTE dbo.uspsysAdmin_Update_SMTP @SMTPEmailAddress, @SMTPUserName, @SMTPPassword, @SMTPHost, @SMTPPort, @SMTPSSL, @GoogleMapKey, @GoogleLocationKey, @PaymentGatewayKey, @RevenueCommission";

                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramSMTPEmailAddress, paramSMTPUserName, paramSMTPPassword, paramSMTPHost, paramSMTPPort, paramSMTPSSL, paramGoogleMapKey, paramGoogleLocationKey, paramPaymentGatewayKey, paramRevenueCommission);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #region AUTH LogOut
        [NonAction]
        public async Task<bool> Auth_Logout()
        {
            try
            {
                SqlParameter paramToken = new SqlParameter() { ParameterName = "@Token", SqlDbType = SqlDbType.BigInt, Value = ObjUser.CurrentToken };
                //SqlParameter paramIsDone = new SqlParameter
                //{
                //    ParameterName = "@IsDone",
                //    SqlDbType = System.Data.SqlDbType.Bit,
                //    Direction = System.Data.ParameterDirection.Output,
                //};

                var paramSqlQuery = "EXECUTE dbo.uspsysUser_Logout @Token";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramToken);

                //return Convert.ToBoolean(paramIsDone.Value);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion AUTH OTP VARIFICATION

    }
}
