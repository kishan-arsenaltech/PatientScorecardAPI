using Scorecard.API.Model;

namespace Scorecard.API.BusinessLogic
{
	public interface IUser
	{
		public UserResponse.GetLoginResult Login(UserRequest.LoginRequest RequestParam);
		public UserResponse.GetUserListingResult GetUserList(UserRequest.GetUserListRequest RequestParam);
		public CommonResponse AddAndUpdateUser(UserRequest.AddAndUpdateUserRequest RequestParam);
		public UserResponse.GetRoleAndOrganizationResponse GetRoleAndOrganization(UserRequest.GetRoleAndOrganizationRequest RequestParam);
		public UserResponse.CommonDeleteResponse CommonDeleteRequest(UserRequest.CommonDeleteRequest RequestParam);
		public UserResponse.LockUnlockUserResponse LockUnlockUser(UserRequest.LockUnlockUserRequest RequestParam);
		public UserResponse.GetUserByIDResult GetUserByID(UserRequest.GetUserByIDRequest RequestParam);
		public UserResponse.ForgotUserPasswordResponse ForgotUserPassword(UserRequest.ForgotUserPasswordRequest RequestParam);
		public UserResponse.CheckResetPasswordResponse CheckResetPasswordRequest(UserRequest.CheckResetPasswordRequest RequestParam);
		public UserResponse.ResetpasswordResponse ResetPassword(UserRequest.ResetPasswordRequest RequestParam);
		public UserResponse.UpdateLoggedInUserDetailResponse UpdateLoggedInUserDetail(UserRequest.UpdateLoggedInUserDetailRequest RequestParam);
		public UserResponse.ResendEmailResponse ResendEmail(UserRequest.ResendEmailRequest RequestParam);
		public UserResponse.DeleteAndRestoreUserResponse DeleteAndRestoreUser(UserRequest.DeleteAndRestoreUserRequest RequestParam);
		public UserResponse.UpdateUserRoleResponse UpdateUserRole(UserRequest.UpdateUserRoleRequest RequestParam);
		public CommonResponse UpdateFirstLogin(CheckUserRequest RequestParam);
		public CommonResponse ChangePassword(UserRequest.ChangePasswordRequest RequestParam);
		public UserResponse.GetLoginAsResult LoginAs(UserRequest.LoginAsRequest RequestParam);
	}
}
