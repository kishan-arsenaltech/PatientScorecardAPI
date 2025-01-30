using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Scorecard.API.BusinessLogic;
using Scorecard.API.DataAccess;
using Scorecard.API.Model;

namespace Scorecard.API.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly IUser _IUser;

		public UserController(IUser user) => _IUser = user;

		/// <summary>
		/// This action is use to check user and password for successful login
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("login")]
		public UserResponse.GetLoginResult Login(UserRequest.LoginRequest RequestParam) => _IUser.Login(RequestParam);

		/// <summary>
		/// This action is use to get list of users.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getuserlist")]
		public UserResponse.GetUserListingResult GetUserList([FromQuery] UserRequest.GetUserListRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IUser.GetUserList(RequestParam);
		}

		/// <summary>
		/// This action is use to add new user into application.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("addandupdateuser")]
		public CommonResponse AddAndUpdateUser(UserRequest.AddAndUpdateUserRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IUser.AddAndUpdateUser(RequestParam);
		}

		/// <summary>
		/// This action is use to get role and organiztion method into application.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getroleandorganization")]
		public UserResponse.GetRoleAndOrganizationResponse GetRoleAndOrganization([FromQuery] UserRequest.GetRoleAndOrganizationRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IUser.GetRoleAndOrganization(RequestParam);
		}

		/// <summary>
		/// This action is use to delete portal details into application.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("commondeleterequest")]
		public UserResponse.CommonDeleteResponse CommonDeleteRequest(UserRequest.CommonDeleteRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IUser.CommonDeleteRequest(RequestParam);
		}

		/// <summary>
		/// This action is use to set user active and lock into application.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("lockunlockuser")]
		public UserResponse.LockUnlockUserResponse LockUnlockUser(UserRequest.LockUnlockUserRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IUser.LockUnlockUser(RequestParam);
		}

		/// <summary>
		/// This action is use to get user detail by id into application.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getuserbyid")]
		public UserResponse.GetUserByIDResult GetUserByID([FromQuery] UserRequest.GetUserByIDRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IUser.GetUserByID(RequestParam);
		}

		/// <summary>
		/// This action is use to forgot user password
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("forgotuserpassword")]
		public UserResponse.ForgotUserPasswordResponse ForgotUserPassword(UserRequest.ForgotUserPasswordRequest RequestParam)
		{
			return _IUser.ForgotUserPassword(RequestParam);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Email"></param>
		/// <param name="Token"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("checkresetpasswordrequest")]
		public UserResponse.CheckResetPasswordResponse CheckResetPasswordRequest([FromQuery] string Email, string Token)
		{
			UserRequest.CheckResetPasswordRequest RequestParam = new UserRequest.CheckResetPasswordRequest();
			RequestParam.Email = Email;
			RequestParam.Token = Token;
			return _IUser.CheckResetPasswordRequest(RequestParam);
		}

		/// <summary>
		/// This action is use to Reset Password 
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("resetpassword")]
		public UserResponse.ResetpasswordResponse ResetPassword(string Email, string Token, UserRequest.ResetPasswordRequest RequestParam)
		{
			RequestParam.Email = Email;
			RequestParam.Token = Token;
			return _IUser.ResetPassword(RequestParam);

		}

		/// <summary>
		/// This action is use to Profile updated
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("updateloggedinuserdetail")]
		public UserResponse.UpdateLoggedInUserDetailResponse UpdateLoggedInUserDetail(UserRequest.UpdateLoggedInUserDetailRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IUser.UpdateLoggedInUserDetail(RequestParam);

		}

		/// <summary>
		/// This action is use to send mail for reset passowrd.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("resendemail")]
		public UserResponse.ResendEmailResponse ResendEmail(UserRequest.ResendEmailRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IUser.ResendEmail(RequestParam);
		}

		/// <summary>
		/// This action is use for delete and restore user.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("deleteandrestoreuser")]
		public UserResponse.DeleteAndRestoreUserResponse DeleteAndRestoreUser(UserRequest.DeleteAndRestoreUserRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IUser.DeleteAndRestoreUser(RequestParam);
		}

		/// <summary>
		/// This action is use for update user role.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("updateuserrole")]
		public UserResponse.UpdateUserRoleResponse UpdateUserRole(UserRequest.UpdateUserRoleRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IUser.UpdateUserRole(RequestParam);
		}

		/// <summary>
		/// This action is used for updating firstlogin flag in users table.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("updatefirstlogin")]
		public CommonResponse UpdateFirstLogin()
		{
			CheckUserRequest RequestParam = new CheckUserRequest();
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IUser.UpdateFirstLogin(RequestParam);
		}

		/// <summary>
		/// This action is used to change password when user logged in first time.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("changepassword")]
		public CommonResponse ChangePassword(UserRequest.ChangePasswordRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IUser.ChangePassword(RequestParam);
		}

		/// <summary>
		/// This action is use to login as select user
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("loginas")]
		public UserResponse.GetLoginAsResult LoginAs(UserRequest.LoginAsRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IUser.LoginAs(RequestParam);
		}

	}
}
