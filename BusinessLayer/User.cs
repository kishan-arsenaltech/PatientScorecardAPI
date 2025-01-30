using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Scorecard.API.DataAccess;
using Scorecard.API.Model;

namespace Scorecard.API.BusinessLogic
{
	public class User : IUser
	{
		public IUserDataAccess _IUserDataAccess;
		public readonly IConfiguration _configuration;
		public User(IUserDataAccess user) => _IUserDataAccess = user;

		dynamic Response;

		public UserResponse.GetLoginResult Login(UserRequest.LoginRequest RequestParam)
		{
			Response = new JObject();
			Response = _IUserDataAccess.Login(RequestParam);
			if (Response == null)
			{
				Response.LiUserResponse = null;
				Response.ListCount = 0;
				Response.Message = "Something went wrong, please try again later.";
				Response.Success = false;
			}

			return Response;
		}
		public UserResponse.GetUserListingResult GetUserList(UserRequest.GetUserListRequest RequestParam)
		{
			Response = new JObject();
			Response = _IUserDataAccess.GetUserList(RequestParam);
			if (Response == null)
			{
				Response.LiUserResponse = null;
				Response.ListCount = 0;
				Response.Message = "Data not found while fetching user list.";
				Response.Success = false;
			}
			return Response;
		}

		public CommonResponse AddAndUpdateUser(UserRequest.AddAndUpdateUserRequest RequestParam)
		{
			Response = _IUserDataAccess.AddAndUpdateUser(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while adding new user.";
				Response.Success = false;
			}

			return Response;
		}

		public UserResponse.GetRoleAndOrganizationResponse GetRoleAndOrganization(UserRequest.GetRoleAndOrganizationRequest RequestParam)
		{
			Response = _IUserDataAccess.GetRoleAndOrganization(RequestParam);
			if (Response == null)
			{
				Response.Message = "Data not found while fetching role and organization.";
				Response.Success = false;
			}

			return Response;
		}
		public UserResponse.CommonDeleteResponse CommonDeleteRequest(UserRequest.CommonDeleteRequest RequestParam)
		{
			Response = _IUserDataAccess.CommonDeleteRequest(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while deleting.";
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.LockUnlockUserResponse LockUnlockUser(UserRequest.LockUnlockUserRequest RequestParam)
		{
			Response = _IUserDataAccess.LockUnlockUser(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while user set lock and unlock";
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.GetUserByIDResult GetUserByID(UserRequest.GetUserByIDRequest RequestParam)
		{
			Response = _IUserDataAccess.GetUserByID(RequestParam);
			if (Response == null)
			{
				Response.Message = "Data not found while fetching user detail.";
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.ForgotUserPasswordResponse ForgotUserPassword(UserRequest.ForgotUserPasswordRequest RequestParam)
		{
			Response = _IUserDataAccess.ForgotUserPassword(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while sending email for reset passowrd.";
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.CheckResetPasswordResponse CheckResetPasswordRequest(UserRequest.CheckResetPasswordRequest RequestParam)
		{
			Response = _IUserDataAccess.CheckResetPasswordRequest(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while check reset user passowrd.";
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.ResetpasswordResponse ResetPassword(UserRequest.ResetPasswordRequest RequestParam)
		{
			Response = _IUserDataAccess.ResetPassword(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while reset user passowrd.";
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.UpdateLoggedInUserDetailResponse UpdateLoggedInUserDetail(UserRequest.UpdateLoggedInUserDetailRequest RequestParam)
		{
			Response = _IUserDataAccess.UpdateLoggedInUserDetail(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while updating password.";
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.ResendEmailResponse ResendEmail(UserRequest.ResendEmailRequest RequestParam)
		{
			Response = _IUserDataAccess.ResendEmail(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while sending mail for resetting password.";
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.DeleteAndRestoreUserResponse DeleteAndRestoreUser(UserRequest.DeleteAndRestoreUserRequest RequestParam)
		{
			Response = _IUserDataAccess.DeleteAndRestoreUser(RequestParam);
			if (Response == null)
			{
				Response.Message = string.Format("Something went wrong while {0}ing user.", RequestParam.StatementType.ToLower() == "restore" ? "restor" : RequestParam.StatementType.ToLower() == "update" ? "updat" : string.Empty);
				Response.Success = false;
			}
			return Response;
		}
		public UserResponse.UpdateUserRoleResponse UpdateUserRole(UserRequest.UpdateUserRoleRequest RequestParam)
		{
			Response = _IUserDataAccess.UpdateUserRole(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while updating user role.";
				Response.Success = false;
			}
			return Response;
		}
		public CommonResponse UpdateFirstLogin(CheckUserRequest RequestParam)
		{
			Response = _IUserDataAccess.UpdateFirstLogin(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while updating first login.";
				Response.Success = false;
			}
			return Response;
		}

		public CommonResponse ChangePassword(UserRequest.ChangePasswordRequest RequestParam)
		{
			Response = _IUserDataAccess.ChangePassword(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while updating first login.";
				Response.Success = false;
			}
			return Response;
		}

		public UserResponse.GetLoginAsResult LoginAs(UserRequest.LoginAsRequest RequestParam)
		{
			Response = new JObject();
			Response = _IUserDataAccess.LoginAs(RequestParam);
			if (Response == null)
			{
				Response.LiUserResponse = null;
				Response.ListCount = 0;
				Response.Message = "Something went wrong, please try again later.";
				Response.Success = false;
			}

			return Response;
		}

	}
}
