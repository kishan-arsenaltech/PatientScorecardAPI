using Scorecard.API.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Scorecard.API.DataAccess
{
	public class UserDataAccess : IUserDataAccess
	{
		protected string StoredProcedure = string.Empty;
		public IConfiguration _configuration { get; }

		public UserDataAccess(IConfiguration configuration) => _configuration = configuration;

		public UserResponse.GetLoginResult Login(UserRequest.LoginRequest RequestParam)
		{
			string EncryptedSalt = string.Empty;
			UserResponse.GetLoginResult Response = new UserResponse.GetLoginResult();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;

			try
			{
				List<SqlParameter> ParamCollection = new List<SqlParameter>();
				ParamCollection.Add(new SqlParameter("Email", RequestParam.Email.HandleDBNull()));
				StoredProcedure = "SP_Check_LoginRequest";

				DataSet TableSet = Data.GetDataSet(StoredProcedure, ParamCollection);
				if (TableSet.Tables.Count > 0)
				{
					if (Convert.ToString(TableSet.Tables[0].Rows[0]["RoleStatus"]) == "no-role")
					{
						Response.Message = "There is no role assigned to you. Please contact administrator!";
					}
					else if (Convert.ToString(TableSet.Tables[0].Rows[0]["RoleStatus"]) == "no-user")
					{
						Response.Message = "Incorrect email or password";
					}
					else
					{
						if (TableSet.Tables[0].Rows.Count > 0)
						{
							if (Convert.ToString(TableSet.Tables[1].Rows[0]["status"]) == "1")
							{
								EncryptedSalt = Convert.ToString(TableSet.Tables[0].Rows[0]["PasswordSalt"]).Trim();
								if (RequestParam.Password.ValidateToken(EncryptedSalt))
								{
									if (TableSet.Tables[0].Rows.Count > 0)
										Response.LiUserResponse = StaticUtilities.DataTableToList<UserResponse.GetLoginResponse>(TableSet.Tables[0]);

									if (TableSet.Tables[2].Rows.Count > 0)
										Response.LiUserReferralResponse = StaticUtilities.DataTableToList<UserResponse.GetUserReferrals>(TableSet.Tables[2]);

									if (TableSet.Tables[3].Rows.Count > 0)
										Response.LiOrganizationResponse = StaticUtilities.DataTableToList<UserResponse.GetOrganizations>(TableSet.Tables[3]);

									SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

									StoredProcedure = "SP_Update_LastLoginTime";
									int i = Data.JustExecute(StoredProcedure, ParamCollection);
									string UserId = TableSet.Tables[0].Rows[0]["UserId"].ToString();
									string RoleId = TableSet.Tables[0].Rows[0]["RoleId"].ToString();

									SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

									ParamCollection.Remove(ParamCollection[0]);
									ParamCollection.Add(new SqlParameter("RoleId", RoleId));
									StoredProcedure = "SP_Get_ModuleNameByRoleId";
									DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
									if (TableData.Rows.Count > 0)
										Response.LiModuleNameByRoleIdResponse = StaticUtilities.DataTableToList<UserResponse.GetModuleNameByRoleId>(TableData);

									SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

									Response.Message = string.Empty;
									Response.Success = true;
									DataAccess.AddTransactionLog(UserId, RequestParam.UserAgent, "Login", "Login", SqlCommand);
								}
								else
									Response.Message = "Incorrect email or password";
							}
							else
								Response.Message = "This user has been locked, Please contact administrator";
						}
						else
							Response.Message = "The credentials did not match with our system";
					}
				}
				else
					Response.Message = "Something went wrong, try again later";

			}
			catch (Exception e)
			{
				if (e.Message.Contains("Input string was not in a correct format."))
					Response.Message = "Incorrect email or password";
				else
					Response.Message = e.Message;

				Response.Success = false;
			}

			return Response;
		}

		public UserResponse.GetUserListingResult GetUserList(UserRequest.GetUserListRequest RequestParam)
		{
			UserResponse.GetUserListingResult ObjResponse = new UserResponse.GetUserListingResult();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{

					if (Data.CheckUserHasAccessPermission(RequestParam.UserId, 1)) //1 = list user permission
					{
						List<SqlParameter> ParamCollection = new List<SqlParameter>();
						ParamCollection.Add(new SqlParameter("ModuleName", RequestParam.ModuleName.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("LoggedInUserID", RequestParam.UserId.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("IsSuperAdmin", RequestParam.IsSuperAdmin.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Search", RequestParam.Search.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("OrderByFieldParam", RequestParam.OrderByFieldParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("SortOrderParam", RequestParam.SortOrderParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("LimitParam", RequestParam.PageSize.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("OffsetParam", RequestParam.RowsToSkip.HandleDBNull()));
						StoredProcedure = "SP_Get_UserList";

						DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
						if (TableData.Rows.Count > 0)
						{
							ObjResponse.LiUserResponse = StaticUtilities.DataTableToList<UserResponse.GetUserResponse>(TableData);

							SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

							// added code to get user list count
							ParamCollection.Remove(ParamCollection[0]);
							ParamCollection.Add(new SqlParameter("ModuleName", "UsersCount"));
							ObjResponse.ListCount = Data.JustExecuteScalar(StoredProcedure, ParamCollection);
							ObjResponse.Message = string.Empty;
							ObjResponse.Success = true;
							DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "User Listing", "Users List", SqlCommand);
						}
						else
						{
							ObjResponse.LiUserResponse = StaticUtilities.DataTableToList<UserResponse.GetUserResponse>(TableData);
							ObjResponse.ListCount = 0;
							ObjResponse.Message = "Data not found while fetching user list.";
							ObjResponse.Success = false;
						}
					}
					else
					{
						ObjResponse.Success = false;
						ObjResponse.Message = "Access denied.";
					}
				}
				else
				{
					ObjResponse.LiUserResponse = null;
					ObjResponse.ListCount = 0;
					ObjResponse.Message = "Access denied.";
					ObjResponse.Success = false;
				}
			}
			catch (Exception e)
			{
				ObjResponse.LiUserResponse = null;
				ObjResponse.ListCount = 0;
				ObjResponse.Message = e.Message;
				ObjResponse.Success = false;
			}
			return ObjResponse;
		}

		public CommonResponse AddAndUpdateUser(UserRequest.AddAndUpdateUserRequest RequestParam)
		{
			CommonResponse ObjResponse = new CommonResponse();
			DataClasses Data = new DataClasses(_configuration);
			SendEmailForCommon ObjSendEmail = new SendEmailForCommon(_configuration);
			GenerateRandomPassword RandomPassword = new GenerateRandomPassword();
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string Password = string.Empty;
			string FileName = string.Empty;

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					if (Data.CheckUserHasAccessPermission(RequestParam.UserId, 2) || Data.CheckUserHasAccessPermission(RequestParam.UserId, 3)) //2 = add user permission, 3 = edit user permission
					{
						List<SqlParameter> ParamCollection = new List<SqlParameter>();
						ParamCollection.Add(new SqlParameter("StatementType", RequestParam.StatementType.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Id", RequestParam.Id.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Name", RequestParam.Name.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Email", RequestParam.Email.HandleDBNull()));
						if (RequestParam.StatementType.ToLower() == "insert")
						{
							Password = RandomPassword.RandomString(10);
							ParamCollection.Add(new SqlParameter("Password", Password.GenerateToken()));
						}
						else
							ParamCollection.Add(new SqlParameter("Password", RequestParam.Password.GenerateToken()));

						ParamCollection.Add(new SqlParameter("Title", RequestParam.Title.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Phone", RequestParam.Phone.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Extension", RequestParam.Extension.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Is_pap_user", RequestParam.Is_pap_user.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Avatar", string.Empty));
						ParamCollection.Add(new SqlParameter("RoleId", RequestParam.RoleId.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Organizations", RequestParam.Organizations.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Referrals", RequestParam.Referrals.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("GlobalSummaryAccess", RequestParam.GlobalSummaryAccess.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("SleepScreenerAccess", RequestParam.SleepScreenerAccess.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("ScreenerEmailFrequency", RequestParam.ScreenerEmailFrequency.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("AirviewName", RequestParam.AirviewName.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("AirviewOrganizationName", RequestParam.AirviewOrganizationName.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("EncoreName", RequestParam.EncoreName.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("EncoreSleepLab", RequestParam.EncoreName.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("PapType", RequestParam.PapType.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("LoggedInUserId", RequestParam.UserId.HandleDBNull()));
						if (RequestParam.StatementType.ToLower() == "insert")
							ParamCollection.Add(new SqlParameter("AccessToken", Guid.NewGuid().ToString().GenerateToken()));
						StoredProcedure = "SP_InsertAndUpdate_User";

						int Result = Data.JustExecuteScalar(StoredProcedure, ParamCollection);
						if (Result > 0)
						{
							if (Result == 2)
							{
								ObjResponse.Success = false;
								ObjResponse.Message = "Entered email is already exists, please use another email address.";
								return ObjResponse;
							}
							if (Result == 3)
							{
								ObjResponse.Success = false;
								ObjResponse.Message = "Access Denied";
								return ObjResponse;
							}

							if (RequestParam.StatementType.ToLower() == "insert")
							{
								string Email = RequestParam.Email.HandleDBNull().ToString();

								string Subject = "Referral Portal: New User Registered";
								string EmailBody = Data.ReadContentFromTemplate("NewRegistration");
								EmailBody = EmailBody.Replace("{application_name}", _configuration.GetSection("SiteSettings:ApplicationName").Value).Replace("{api_url}", _configuration.GetSection("SiteSettings:ScorecardApiUrl").Value).Replace("{name}", RequestParam.Name).Replace("{email}", RequestParam.Email).Replace("{password}", Password);
								EmailBody = EmailBody.Replace("{site_url}", string.Format("{0}auth/login", _configuration.GetSection("SiteSettings:ScorecardBaseUrl").Value)).Replace("{current_year}", DateTime.Now.Year.ToString());


								bool success = ObjSendEmail.SendEmail(Subject, EmailBody, Email);
								if (success == true)
								{
									ObjResponse.Success = true;
									ObjResponse.Message = string.Format("User {0}ed successfully and email sent for username and password.", RequestParam.StatementType);
									DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, RequestParam.StatementType.ToLower() == "insert" ? "Add" : "Update" + "User", RequestParam.StatementType.ToLower() == "insert" ? "Add" : "Update" + "User", Data.BuildSqlCommand(ParamCollection, StoredProcedure));
								}
							}
							else
							{
								ObjResponse.Success = true;
								ObjResponse.Message = string.Format("User {0}ed successfully.", RequestParam.StatementType.ToLower() == "update" ? "updat" : RequestParam.StatementType);
								DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, RequestParam.StatementType.ToLower() == "insert" ? "Add " : "Update " + "User", RequestParam.StatementType.ToLower() == "insert" ? "Add " : "Update " + "User", Data.BuildSqlCommand(ParamCollection, StoredProcedure));
							}
						}
						else
						{
							ObjResponse.Success = false;
							ObjResponse.Message = string.Format("Something went wrong while {0}ing user details.", RequestParam.StatementType.ToLower() == "update" ? "updat" : RequestParam.StatementType);
						}
					}
					else
					{
						ObjResponse.Success = false;
						ObjResponse.Message = "Access denied.";
					}
				}
				else
				{
					ObjResponse.Success = false;
					ObjResponse.Message = "Access denied.";
				}
			}
			catch (Exception e)
			{
				ObjResponse.Message = e.Message;
				ObjResponse.Success = false;
			}
			return ObjResponse;
		}

		public UserResponse.GetRoleAndOrganizationResponse GetRoleAndOrganization(UserRequest.GetRoleAndOrganizationRequest RequestParam)
		{
			UserResponse.GetRoleAndOrganizationResponse ObjResponse = new UserResponse.GetRoleAndOrganizationResponse();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("Search", RequestParam.Search.HandleDBNull()));
					StoredProcedure = "SP_Get_Role";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						ObjResponse.LiRoleResponse = StaticUtilities.DataTableToList<UserResponse.GetRolesResponse>(TableData);
						ObjResponse.Message = string.Empty;
						ObjResponse.Success = true;
					}
					else
					{
						ObjResponse.LiRoleResponse = null;
						ObjResponse.Message = "Data not found while fetching role.";
						ObjResponse.Success = false;
					}

					// start code to get organization data
					StoredProcedure = "SP_Get_Organizations";
					ParamCollection = new List<SqlParameter>();

					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						ObjResponse.LiOrganizationResponse = StaticUtilities.DataTableToList<UserResponse.GetOrganizationResponse>(TableData);
						ObjResponse.Message = string.Empty;
						ObjResponse.Success = true;
					}
					else
					{
						ObjResponse.LiOrganizationResponse = null;
						ObjResponse.Message = "Data not found while fetching organization.";
						ObjResponse.Success = false;
					}
				}
				else
				{
					ObjResponse.Success = false;
					ObjResponse.Message = "Access denied.";
				}
			}
			catch (Exception e)
			{
				ObjResponse.Message = e.Message;
				ObjResponse.Success = false;
			}

			return ObjResponse;
		}

		public UserResponse.CommonDeleteResponse CommonDeleteRequest(UserRequest.CommonDeleteRequest RequestParam)
		{
			UserResponse.CommonDeleteResponse Response = new UserResponse.CommonDeleteResponse();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("ModuleName", RequestParam.ModuleName.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Search ", RequestParam.Search.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("LoggedInUserID", RequestParam.UserId.HandleDBNull()));
					StoredProcedure = "SP_Delete_CommonQueries";

					int Result = Data.JustExecute(StoredProcedure, ParamCollection);
					if (Result > 0)
					{
						Response.Success = true;
						Response.Message = RequestParam.ModuleName + " deleted successfully.";
						DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, RequestParam.ModuleName + "Delete", RequestParam.ModuleName + "Delete", Data.BuildSqlCommand(ParamCollection, StoredProcedure));

					}
					else
					{
						Response.Success = false;
						Response.Message = "Something went wrong while deleting " + RequestParam.ModuleName.ToLower() + " detail.";
					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public UserResponse.LockUnlockUserResponse LockUnlockUser(UserRequest.LockUnlockUserRequest RequestParam)
		{
			UserResponse.LockUnlockUserResponse Response = new UserResponse.LockUnlockUserResponse();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("Id", RequestParam.Id.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Status", RequestParam.Status.HandleDBNull()));
					StoredProcedure = "SP_LockAndUnlock_User";

					int Result = Data.JustExecuteScalar(StoredProcedure, ParamCollection);
					if (Result > 0)
					{
						Response.Success = true;
						if (RequestParam.Status == 1) { Response.Message = " User unlock successfully"; }
						else { Response.Message = " User Lock successfully"; }

					}
					else
					{
						Response.Success = false;
						Response.Message = "Something went wrong while user set lock or unlock";
					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;

		}

		public UserResponse.GetUserByIDResult GetUserByID(UserRequest.GetUserByIDRequest RequestParam)
		{
			UserResponse.GetUserByIDResult Response = new UserResponse.GetUserByIDResult();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string ImagePath = string.Empty;

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					if (Convert.ToInt32(RequestParam.UserId) == RequestParam.Id || Data.CheckUserHasAccessPermission(RequestParam.UserId, 3)) //3 = edit user permission
					{
						List<SqlParameter> ParamCollection = new List<SqlParameter>();
						ParamCollection.Add(new SqlParameter("Id", RequestParam.Id.HandleDBNull()));
						StoredProcedure = "SP_Get_UserByID";

						DataSet TableSet = Data.GetDataSet(StoredProcedure, ParamCollection);
						if (TableSet.Tables.Count > 0)
						{
							DataTable TableData = TableSet.Tables[0];
							if (TableData.Rows.Count > 0)
							{
								string FileNameWithPath = Path.Combine(_configuration.GetSection("FolderPath:ProfileImageFolder").Value, TableData.Rows[0]["Avatar"].ToString());
								Response.LiUserByIDResponse = StaticUtilities.DataTableToList<UserResponse.GetUserByIDResponse>(TableData);
								Response.ImagePath = FileNameWithPath;
								Response.Message = string.Empty;
								Response.Success = true;
								DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Edit User", "Edit User", Data.BuildSqlCommand(ParamCollection, StoredProcedure));
							}
							else
							{
								Response.LiUserByIDResponse = null;
								Response.ImagePath = null;
								Response.Message = "Data not found while fetching user detail.";
								Response.Success = false;
							}
						}


						if (TableSet.Tables[1] != null)
						{
							DataTable TableData = TableSet.Tables[1];
							Response.LiReferralList = StaticUtilities.DataTableToList<UserResponse.GetReferralList>(TableData);
						}
					}
					else
					{
						Response.Success = false;
						Response.Message = "Access denied.";
					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public UserResponse.ForgotUserPasswordResponse ForgotUserPassword(UserRequest.ForgotUserPasswordRequest RequestParam)
		{
			UserResponse.ForgotUserPasswordResponse Response = new UserResponse.ForgotUserPasswordResponse();
			DataClasses Data = new DataClasses(_configuration);
			SendEmailForCommon ObjSendEmail = new SendEmailForCommon(_configuration);

			try
			{
				List<SqlParameter> ParamCollection = new List<SqlParameter>();
				ParamCollection.Add(new SqlParameter("Email", RequestParam.Email.HandleDBNull()));
				StoredProcedure = "SP_Check_ForgotPasswordRequest";

				DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
				Response.Success = true;
				string Token = Guid.NewGuid().ToString();
				string Email = RequestParam.Email;
				if (!string.IsNullOrEmpty(RequestParam.BaseUrl))
				{
					string LinkForResetPassword = string.Format("{0}?emailid={1}&Token={2}", RequestParam.BaseUrl, Email, Token);
					string Subject = _configuration.GetSection("SiteSettings:ApplicationName").Value + " - Reset Password Request";
					string EmailBody = Data.ReadContentFromTemplate("ResetPassword");
					EmailBody = EmailBody.Replace("{application_name}", _configuration.GetSection("SiteSettings:ApplicationName").Value).Replace("{resetpassword_url}", LinkForResetPassword).Replace("{current_year}", DateTime.Now.Year.ToString());

					bool Success = ObjSendEmail.SendEmail(Subject, EmailBody, Email);

					if (Success)
					{
						ParamCollection.Add(new SqlParameter("Token", Token));
						StoredProcedure = "SP_Insert_ResetPasswordDetail";

						int Result = Data.JustExecute(StoredProcedure, ParamCollection);
					}
					else
					{
						Response.Success = false;
						Response.Message = "Something went wrong. Please try again after after sometime.";
					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Base URL not found with this request.";
				}

				Response.Success = true;
				Response.Message = "You will receive an email with reset password instructions if there is an account for entered email.";
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public UserResponse.CheckResetPasswordResponse CheckResetPasswordRequest(UserRequest.CheckResetPasswordRequest RequestParam)
		{
			UserResponse.CheckResetPasswordResponse Response = new UserResponse.CheckResetPasswordResponse();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				List<SqlParameter> ParamCollection = new List<SqlParameter>();
				ParamCollection.Add(new SqlParameter("Email", RequestParam.Email.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("Token", RequestParam.Token.HandleDBNull()));
				StoredProcedure = "SP_Check_ResetPasswordRequest";

				DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
				var Count = TableData.Rows[0]["Count"];
				if (Count.ToString() == "1")
				{
					Response.Message = string.Empty;
					Response.Success = true;
				}
				else
				{
					Response.Message = "Your password reset link has been expired.";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public UserResponse.ResetpasswordResponse ResetPassword(UserRequest.ResetPasswordRequest RequestParam)
		{
			UserResponse.ResetpasswordResponse Response = new UserResponse.ResetpasswordResponse();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				List<SqlParameter> ParamCollection = new List<SqlParameter>();
				ParamCollection.Add(new SqlParameter("Email", RequestParam.Email.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("Token", RequestParam.Token.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("Password", RequestParam.Password.GenerateToken()));
				ParamCollection.Add(new SqlParameter("AccessToken", Guid.NewGuid().ToString().GenerateToken()));

				StoredProcedure = "SP_Update_ResetPassword";

				int Result = Data.JustExecute(StoredProcedure, ParamCollection);
				if (Result > 0)
				{
					if (Result == 1)
						Response.Message = "Password reset successfully";
					else
						Response.Message = "Reset password token has been expired. Please re-send email again.";
					Response.Success = true;
				}
				else
				{
					Response.Success = false;
					Response.Message = "Something went wrong while resetting password";
				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public UserResponse.UpdateLoggedInUserDetailResponse UpdateLoggedInUserDetail(UserRequest.UpdateLoggedInUserDetailRequest RequestParam)
		{
			UserResponse.UpdateLoggedInUserDetailResponse Response = new UserResponse.UpdateLoggedInUserDetailResponse();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string FileName = string.Empty;

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("StatementType", RequestParam.StatementType.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Id", RequestParam.UserId.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Name", RequestParam.Name.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Title", RequestParam.Title.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Phone", RequestParam.Phone.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Extension", RequestParam.Extension.HandleDBNull()));
					if (RequestParam.IsDeleteImage == false && RequestParam.ImageData != "" && RequestParam.CurrentImageName != "")
						ParamCollection.Add(new SqlParameter("Avatar", string.Empty));
					else if (RequestParam.IsDeleteImage == true && RequestParam.ImageData != "" && RequestParam.CurrentImageName != "")
						ParamCollection.Add(new SqlParameter("Avatar", string.Empty));
					ParamCollection.Add(new SqlParameter("isPapUser", RequestParam.isPapUser.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("PapType", RequestParam.PapType.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("AirviewName", RequestParam.AirviewName.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("AirviewOrganizationName", RequestParam.AirviewOrganizationName.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("EncoreName", RequestParam.EncoreName.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("EncoreSleepLab", RequestParam.EncoreSleepLab.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("NPI", RequestParam.NPI.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ScreenerEmailFrequency", RequestParam.ScreenerEmailFrequency.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("GlobalSummaryAccess", RequestParam.GlobalSummaryAccess.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("SleepScreenerAccess", RequestParam.SleepScreenerAccess.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Organizations", RequestParam.Organizations.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Referrals", RequestParam.Referrals.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ReferralType", RequestParam.ReferralType.HandleDBNull()));
					StoredProcedure = "SP_Update_LoggedInUserDetail";

					int Result = Data.JustExecute(StoredProcedure, ParamCollection);
					if (Result > 0)
					{
						Response.Success = true;
						Response.Message = "Profile details saved successfully!";
						DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Update Profile", "Update User Profile", Data.BuildSqlCommand(ParamCollection, StoredProcedure));
					}
					else
					{
						Response.Success = false;
						Response.Message = "Something went wrong while updating profile details.";
					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";
				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;

			}
			return Response;
		}

		public UserResponse.ResendEmailResponse ResendEmail(UserRequest.ResendEmailRequest RequestParam)
		{
			UserResponse.ResendEmailResponse Response = new UserResponse.ResendEmailResponse();
			DataClasses Data = new DataClasses(_configuration);
			SendEmailForCommon ResetPassword = new SendEmailForCommon(_configuration);
			GenerateRandomPassword RandomPassword = new GenerateRandomPassword();
			string NewPassword = string.Empty;

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("Id", RequestParam.Id.HandleDBNull()));
					NewPassword = RandomPassword.RandomString(10);
					ParamCollection.Add(new SqlParameter("Password", NewPassword.GenerateToken()));
					ParamCollection.Add(new SqlParameter("AccessToken", Guid.NewGuid().ToString().GenerateToken()));
					StoredProcedure = "SP_Update_NewPassword";

					string Email = RequestParam.Email.HandleDBNull().ToString();
					string Subject = "New User Registration";
					string EmailBody = Data.ReadContentFromTemplate("NewRegistration");
					EmailBody = EmailBody.Replace("{application_name}", _configuration.GetSection("SiteSettings:ApplicationName").Value).Replace("{name}", RequestParam.Name).Replace("{email}", RequestParam.Email).Replace("{password}", NewPassword);
					EmailBody = EmailBody.Replace("{site_url}", string.Format("{0}auth/login", _configuration.GetSection("SiteSettings:ScorecardBaseUrl").Value)).Replace("current_year", DateTime.Now.Year.ToString());

					bool success = ResetPassword.SendEmail(Subject, EmailBody, Email);
					if (success == true)
					{
						int Result = Data.JustExecute(StoredProcedure, ParamCollection);
						if (Result > 0)
						{
							Response.Success = true;
							Response.Message = "Email send successfully for reset password !";
						}
						else
						{
							Response.Success = false;
							Response.Message = "Something went wrong while sending mail for reset password.";
						}
					}
					else
					{
						Response.Success = false;
						Response.Message = "Failed to send mail for reset password.";

					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";
				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public UserResponse.DeleteAndRestoreUserResponse DeleteAndRestoreUser(UserRequest.DeleteAndRestoreUserRequest RequestParam)
		{
			UserResponse.DeleteAndRestoreUserResponse Response = new UserResponse.DeleteAndRestoreUserResponse();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					if (Data.CheckUserHasAccessPermission(RequestParam.UserId, 4)) //4 = delete user permission
					{
						List<SqlParameter> ParamCollection = new List<SqlParameter>();
						ParamCollection.Add(new SqlParameter("StatementType", RequestParam.StatementType.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("Id", RequestParam.Id.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("LoggedInUserId", RequestParam.UserId.HandleDBNull()));
						StoredProcedure = "SP_DeleteAndRestore_User";

						int Result = Data.JustExecute(StoredProcedure, ParamCollection);
						if (Result > 0)
						{
							Response.Success = true;
							Response.Message = string.Format("User {0}d successfully.", RequestParam.StatementType);
						}
						else
						{
							Response.Success = false;
							Response.Message = string.Format("Something went wrong while {0}ing user.", RequestParam.StatementType.ToLower() == "restore" ? "restor" : RequestParam.StatementType.ToLower() == "update" ? "updat" : string.Empty);
						}
					}
					else
					{
						Response.Success = false;
						Response.Message = "Access denied.";
					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";

				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public UserResponse.UpdateUserRoleResponse UpdateUserRole(UserRequest.UpdateUserRoleRequest RequestParam)
		{
			UserResponse.UpdateUserRoleResponse Response = new UserResponse.UpdateUserRoleResponse();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("Id", RequestParam.Id.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("RoleId", RequestParam.RoleId.HandleDBNull()));
					StoredProcedure = "SP_Update_UserRole";

					int Result = Data.JustExecute(StoredProcedure, ParamCollection);
					if (Result > 0)
					{
						Response.Success = true;
						Response.Message = "User role updated successfully.";
					}
					else
					{
						Response.Success = false;
						Response.Message = "Something went wrong while updating user role.";
					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";

				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public CommonResponse UpdateFirstLogin(CheckUserRequest RequestParam)
		{
			CommonResponse Response = new CommonResponse();
			DataClasses Data = new DataClasses(_configuration);
			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("Id", RequestParam.UserId.HandleDBNull()));
					StoredProcedure = "SP_Update_FirstLogin";

					int Result = Data.JustExecuteScalar(StoredProcedure, ParamCollection);
					Response.Success = true;
					Response.Message = Result.ToString();
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";
				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public CommonResponse ChangePassword(UserRequest.ChangePasswordRequest RequestParam)
		{
			CommonResponse Response = new CommonResponse();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("Id", RequestParam.UserId.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Password", RequestParam.Password.GenerateToken()));
					StoredProcedure = "SP_Update_ChangePassword";

					int Result = Data.JustExecute(StoredProcedure, ParamCollection);
					if (Result > 0)
					{
						Response.Success = true;
						Response.Message = "Password changed successfully.";
					}
					else
					{
						Response.Success = false;
						Response.Message = "Something went wrong while changing password.";
					}
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";

				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public UserResponse.GetLoginAsResult LoginAs(UserRequest.LoginAsRequest RequestParam)
		{
			string EncryptedSalt = string.Empty;
			UserResponse.GetLoginAsResult Response = new UserResponse.GetLoginAsResult();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("LogginUserId", RequestParam.UserId.HandleDBNull()));
					StoredProcedure = "SP_Check_SuperAdmin";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					string IsSuperAdmin = TableData.Rows[0]["IsSuperAdmin"].ToString();
					if (IsSuperAdmin == "1")
					{
						SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
						ParamCollection = new List<SqlParameter>();
						ParamCollection.Add(new SqlParameter("Email", RequestParam.Email.HandleDBNull()));
						StoredProcedure = "SP_Check_LoginRequest";

						DataSet TableSet = Data.GetDataSet(StoredProcedure, ParamCollection);
						if (TableSet.Tables.Count > 0)
						{
							if (Convert.ToString(TableSet.Tables[0].Rows[0]["RoleStatus"]) == "no-role")
							{
								Response.Message = "There is no role assigned to this user. Please contact your administrator!";
							}
							else
							{

								if (TableSet.Tables[0].Rows.Count > 0)
								{
									if (Convert.ToString(TableSet.Tables[1].Rows[0]["status"]) == "1")
									{

										if (TableSet.Tables[0].Rows.Count > 0)
										{
											Response.LiLoginAsUserResponse = StaticUtilities.DataTableToList<UserResponse.GetLoginAsResponse>(TableSet.Tables[0]);
											if (Response.LiLoginAsUserResponse.Count == 1 && string.IsNullOrEmpty(Response.LiLoginAsUserResponse[0].AccessToken))
											{
												string LoggedInUserEmail = Response.LiLoginAsUserResponse[0].Email;
												string AccessToken = Guid.NewGuid().ToString().GenerateToken();
												ParamCollection.Add(new SqlParameter("AccessToken", AccessToken));
												StoredProcedure = "SP_Update_AccessToken";
												Data.JustExecute(StoredProcedure, ParamCollection);
												SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
												Response.LiLoginAsUserResponse[0].AccessToken = AccessToken;
												ParamCollection.Remove(ParamCollection[1]);
											}
										}

										if (TableSet.Tables[2].Rows.Count > 0)
											Response.LiLoginAsUserReferralResponse = StaticUtilities.DataTableToList<UserResponse.GetLoginAsUserReferrals>(TableSet.Tables[2]);

										if (TableSet.Tables[3].Rows.Count > 0)
											Response.LiLoginAsOrganizationResponse = StaticUtilities.DataTableToList<UserResponse.GetLoginAsOrganizations>(TableSet.Tables[3]);

										SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
										string UserId = TableSet.Tables[0].Rows[0]["UserId"].ToString();
										string RoleId = TableSet.Tables[0].Rows[0]["RoleId"].ToString();
										ParamCollection.Remove(ParamCollection[0]);
										ParamCollection.Add(new SqlParameter("RoleId", RoleId));
										StoredProcedure = "SP_Get_ModuleNameByRoleId";
										TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
										if (TableData.Rows.Count > 0)
											Response.LiLoginAsModuleNameByRoleIdResponse = StaticUtilities.DataTableToList<UserResponse.GetLoginAsModuleNameByRoleId>(TableData);
										SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

										Response.Message = string.Empty;
										Response.Success = true;
										DataAccess.AddTransactionLog(UserId, RequestParam.UserAgent, "LoginAs", "LoginAs", SqlCommand);
									}
									else
										Response.Message = "This user has been locked, Please contact your administrator!";
								}
								else
									Response.Message = "Your account has been deleted! Please contact your administrator!";
							}
						}
						else
							Response.Message = "Something went wrong, try again later";

					}
					else
						Response.Message = "You don't have a rights for loginas functionality";
				}
				else
				{
					Response.Success = false;
					Response.Message = "Access denied.";
				}
			}

			catch (Exception e)
			{
				if (e.Message.Contains("Input string was not in a correct format."))
					Response.Message = "Incorrect email or password";
				else
					Response.Message = e.Message;

				Response.Success = false;
			}

			return Response;
		}

		public string GetImageNameWithId(string Email, string ImageStartingFrom)
		{
			String Result = string.Empty;
			DataClasses Data = new DataClasses(_configuration);

			List<SqlParameter> ParamCollection = new List<SqlParameter>();
			ParamCollection.Add(new SqlParameter("Email", Email.HandleDBNull()));
			ParamCollection.Add(new SqlParameter("ImageStartingFrom", ImageStartingFrom.HandleDBNull()));
			StoredProcedure = "SP_UpdateAndGet_UserImage";

			DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
			Result = TableData.Rows[0]["Avatar"].ToString();
			if (Result != string.Empty)
			{

				return Result;
			}
			else
			{

				return Result;
			}
		}
	}
}
