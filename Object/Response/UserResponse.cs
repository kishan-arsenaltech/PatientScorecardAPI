using System;
using System.Collections.Generic;

namespace Scorecard.API.Model
{
	public class UserResponse
	{

		public class GetLoginResult : CommonResponse
		{
			public List<GetLoginResponse> LiUserResponse { get; set; }
			public List<GetUserReferrals> LiUserReferralResponse { get; set; }
			public List<GetOrganizations> LiOrganizationResponse { get; set; }
			public List<GetModuleNameByRoleId> LiModuleNameByRoleIdResponse { get; set; }
			public int ListCount { get; set; }
		}

		public class GetLoginResponse : CommonResponse
		{
			public int UserId { get; set; }
			public string Name { get; set; }
			public string Email { get; set; }
			public string AccessToken { get; set; }
			public string Title { get; set; }
			public string Phone { get; set; }
			public string Extension { get; set; }
			public string Avatar { get; set; }
			public string AvatarWithFullPath
			{
				get { return !string.IsNullOrEmpty(Avatar) ? string.Format("\\Content\\User_Image\\{0}", Avatar) : null; }
			}
			public int IsPapUser { get; set; }
			public int RoleId { get; set; }
			public string RoleName { get; set; }
			public string DisplayRoleName { get; set; }
			public string RoleDescription { get; set; }
			public int RoleIsBaa { get; set; }
			public byte FirstLogin { get; set; }
			public string DefaultView { get; set; }
			public int GlobalSummaryAccess { get; set; }
			public int SleepScreenerAccess { get; set; }
		}
		public class GetUserReferrals
		{
			public int ReferralKey { get; set; }
		}
		public class GetOrganizations
		{
			public string Organization { get; set; }
		}
		public class GetModuleNameByRoleId
		{
			public int ModuleId { get; set; }
			public string ModuleName { get; set; }
			public string DisplayName { get; set; }
		}

		public class GetUserListingResult : CommonResponse
		{
			public List<GetUserResponse> LiUserResponse { get; set; }
			public int ListCount { get; set; }
		}

		public class GetUserResponse
		{

			public int id { get; set; }
			public string name { get; set; }
			public string email { get; set; }
			public string password { get; set; }
			public int default_organization_id { get; set; }
			public string remember_token { get; set; }
			public string title { get; set; }
			public string phone { get; set; }
			public string extension { get; set; }
			public string avatar { get; set; }
			public string ImageName
			{
				get
				{
					return string.Format("\\Content\\User_Image\\{0}", avatar);
				}
			}
			public int status { get; set; }
			public DateTime? last_login { get; set; }
			public string is_baa_on_file { get; set; }
			public int is_pap_user { get; set; }
			public int FirstLogin { get; set; }
			public int role_id { get; set; }
			public DateTime? deleted_at { get; set; }
		}

		public class GetRoleAndOrganizationResponse : CommonResponse
		{
			public List<GetRolesResponse> LiRoleResponse { get; set; }
			public List<GetOrganizationResponse> LiOrganizationResponse { get; set; }
		}
		public class GetRolesResponse
		{
			public int id { get; set; }
			public string display_name { get; set; }
			public string name { get; set; }
			public int is_baa { get; set; }
		}

		public class GetOrganizationResponse
		{
			public string NickName { get; set; }
		}
		public class CommonDeleteResponse : CommonResponse
		{

		}
		public class LockUnlockUserResponse : CommonResponse
		{
		}
		public class GetUserByIDResult : CommonResponse
		{
			public List<GetUserByIDResponse> LiUserByIDResponse { get; set; }
			public List<GetReferralList> LiReferralList { get; set; }
			public string ImagePath { get; set; }
		}
		public class GetUserByIDResponse
		{
			public int UserId { get; set; }
			public string Name { get; set; }
			public string Title { get; set; }
			public int RoleId { get; set; }
			public string RoleName { get; set; }
			public string Email { get; set; }
			public string Phone { get; set; }
			public string Extension { get; set; }
			public int IsPAPUser { get; set; }
			public string Referral { get; set; }
			public string Organization { get; set; }
			public string Avatar { get; set; }
			public string ISBaaOnFile { get; set; }
			public int IsBaa { get; set; }
			public string DefaultView { get; set; }
			public string PapType { get; set; }
			public string AirviewName { get; set; }
			public string AirviewOrganizationName { get; set; }
			public string EncoreName { get; set; }
			public string EncoreSleepLab { get; set; }
			public string NPI { get; set; }
			public string ScreenerEmailFrequency { get; set; }
			public int GlobalSummaryAccess { get; set; }
			public int SleepScreenerAccess { get; set; }
		}

		public class GetReferralList
		{
			public int ReferralKey { get; set; }
			public string FacilityName { get; set; }
			public string SleepScreenerLink { get; set; }
		}

		public class ForgotUserPasswordResponse : CommonResponse
		{ }
		public class CheckResetPasswordResponse : CommonResponse
		{

		}
		public class ResetpasswordResponse : CommonResponse
		{

		}
		public class UpdateLoggedInUserDetailResponse : CommonResponse
		{

		}
		public class ResendEmailResponse : CommonResponse
		{

		}
		public class DeleteAndRestoreUserResponse : CommonResponse
		{

		}
		public class UpdateUserRoleResponse : CommonResponse
		{

		}

		public class GetLoginAsResult : CommonResponse
		{
			public List<GetLoginAsResponse> LiLoginAsUserResponse { get; set; }
			public List<GetLoginAsUserReferrals> LiLoginAsUserReferralResponse { get; set; }
			public List<GetLoginAsOrganizations> LiLoginAsOrganizationResponse { get; set; }
			public List<GetLoginAsModuleNameByRoleId> LiLoginAsModuleNameByRoleIdResponse { get; set; }
			public int ListCount { get; set; }
		}

		public class GetLoginAsResponse : CommonResponse
		{
			public int UserId { get; set; }
			public string Name { get; set; }
			public string Email { get; set; }
			public string PasswordSalt { get; set; }
			public string AccessToken { get; set; }
			public string Title { get; set; }
			public string Phone { get; set; }
			public string Extension { get; set; }
			public string Avatar { get; set; }
			public string AvatarWithFullPath
			{
				get { return !string.IsNullOrEmpty(Avatar) ? string.Format("\\Content\\User_Image\\{0}", Avatar) : null; }
			}
			public int IsPapUser { get; set; }
			public int RoleId { get; set; }
			public string RoleName { get; set; }
			public string DisplayRoleName { get; set; }
			public string RoleDescription { get; set; }
			public int RoleIsBaa { get; set; }
			public byte FirstLogin { get; set; }
			public int GlobalSummaryAccess { get; set; }
			public int SleepScreenerAccess { get; set; }
		}
		public class GetLoginAsUserReferrals
		{
			public int ReferralKey { get; set; }
		}
		public class GetLoginAsOrganizations
		{
			public string Organization { get; set; }
		}
		public class GetLoginAsModuleNameByRoleId
		{
			public int ModuleId { get; set; }
			public string ModuleName { get; set; }
			public string DisplayName { get; set; }
		}
	}
}
