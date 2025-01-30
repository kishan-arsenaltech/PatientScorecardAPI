using System;
using System.Collections.Generic;
using System.Text;

namespace Scorecard.API.Model
{
	public class UserRequest
	{

		public class LoginRequest : CheckUserRequest
		{
			public string Email { get; set; }
			public string Password { get; set; }
		}

		public class AddAndUpdateUserRequest : CheckUserRequest
		{
			public string StatementType { get; set; }
			public string Id { get; set; }
			public string Name { get; set; }
			public string Email { get; set; }
			public string Password { get; set; }
			public string Title { get; set; }
			public string Phone { get; set; }
			public string Extension { get; set; }
			public string Is_pap_user { get; set; }
			public string PapType { get; set; }
			public string AirviewName { get; set; }
			public string AirviewOrganizationName { get; set; }
			public string EncoreName { get; set; }
			public string EncoreSleepLab { get; set; }
			public string Avatar { get; set; }
			public int RoleId { get; set; }
			public string Organizations { get; set; }
			public string Referrals { get; set; }
			public string ScreenerEmailFrequency { get; set; }
			public int? GlobalSummaryAccess { get; set; }
			public int? SleepScreenerAccess { get; set; }
			public string ReferralType { get; set; }
		}

		public class CommonDeleteRequest : CheckUserRequest
		{
			public string ModuleName { get; set; }
			public string Search { get; set; }
		}
		public class LockUnlockUserRequest : CheckUserRequest
		{
			public int Id { get; set; }
			public int Status { get; set; }
		}
		public class ForgotUserPasswordRequest
		{
			public string Email { get; set; }
			public string BaseUrl { get; set; }
		}
		public class CheckResetPasswordRequest
		{
			public string Email { get; set; }
			public string Token { get; set; }
		}
		public class ResetPasswordRequest
		{
			public string Email { get; set; }
			public string Token { get; set; }
			public string Password { get; set; }

		}
		public class GetUserByIDRequest : CheckUserRequest
		{
			public int Id { get; set; }
		}
		public class UpdateLoggedInUserDetailRequest : CheckUserRequest
		{
			public string StatementType { get; set; }
			public string Name { get; set; }
			public string Title { get; set; }
			public string Phone { get; set; }
			public string Extension { get; set; }
			public string Password { get; set; }
			public string CurrentImageName { get; set; }
			public string ImageData { get; set; }
			public int isPapUser { get; set; }
			public string PapType { get; set; }
			public string AirviewName { get; set; }
			public string AirviewOrganizationName { get; set; }
			public string EncoreName { get; set; }
			public string EncoreSleepLab { get; set; }
			public bool IsDeleteImage { get; set; }
			public string NPI { get; set; }
			public string ScreenerEmailFrequency { get; set; }
			public int? GlobalSummaryAccess { get; set; }
			public int? SleepScreenerAccess { get; set; }
			public string Organizations { get; set; }
			public string Referrals { get; set; }
			public string ReferralType { get; set; }

		}
		public class ResendEmailRequest : CheckUserRequest
		{
			public int Id { get; set; }
			public string Email { get; set; }
			public string Name { get; set; }
		}
		public class DeleteAndRestoreUserRequest : CheckUserRequest
		{
			public string StatementType { get; set; }
			public int Id { get; set; }
		}
		public class UpdateUserRoleRequest : CheckUserRequest
		{
			public int Id { get; set; }
			public int RoleId { get; set; }
		}

		public class ChangePasswordRequest : CheckUserRequest
		{
			public string Password { get; set; }
		}
		public class LoginAsRequest : CheckUserRequest
		{
			public string Email { get; set; }
		}
		public class GetRoleAndOrganizationRequest : CheckUserRequest
		{
			public string Search { get; set; }
		}
		public class GetUserListRequest : CheckUserRequest
		{
			public string ModuleName { get; set; }
			public int LoggedInUserId { get; set; }
			public int IsSuperAdmin { get; set; }
			public string Search { get; set; }
			public string OrderByFieldParam { get; set; }
			public string SortOrderParam { get; set; }
			public int PageSize { get; set; }
			public int RowsToSkip { get; set; }
		}
	}
}
