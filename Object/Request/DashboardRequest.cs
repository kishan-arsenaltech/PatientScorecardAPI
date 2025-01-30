using System;
using System.Collections.Generic;
using System.Text;

namespace Scorecard.API.Model
{
	public class DashboardRequest
	{
		#region PAP Dashboard Request Model
		public class GetDashboardRequest : CheckUserRequest
		{
			public int RoleId { get; set; }
			public string OrganizationParam { get; set; }
			public string ReferralParam { get; set; }
			public string Type { get; set; }
		}

		public class GetDoctorsRequest : CheckUserRequest
		{
			public string DoctorName { get; set; }
		}

		public class GetDeliveredAndComplaintChartRequest : CheckUserRequest
		{
			public string Organization { get; set; }
			public string ReferralKey { get; set; }
		}

		public class GetSalesOrderByFilterRequest : CheckUserRequest
		{
			public string DefaultView { get; set; }
			public string StatusParam { get; set; }
			public string SalesRepParam { get; set; }
			public string DoctorParam { get; set; }
			public string DoctorNameParam { get; set; }
			public string DoctorFirstNameParam { get; set; }
			public string DoctorLastNameParam { get; set; }
			public string InsuranceParam { get; set; }
			public string MaskTypeParam { get; set; }
			public string ComplianceRiskParam { get; set; }
			public string EquipmentTypeParam { get; set; }
			public string DaysSinceSetupParam { get; set; }
			public string OrganizationParam { get; set; }
			public string ReferralParam { get; set; }
			public string LimitParam { get; set; }
			public string StartDate { get; set; }
			public string EndDate { get; set; }
			public string PatientNameParam { get; set; }
			public string PatientFirstNameParam { get; set; }
			public string PatientNameLastParam { get; set; }
			public string DateOfBirthParam { get; set; }
			public string RowsToSkip { get; set; }
			public string OrderByFieldParam { get; set; }
			public string SortOrderParam { get; set; }
			public string VoidReason { get; set; }
		}

		public class GetAveragePatientUsageDataRequest : CheckUserRequest
		{
			public string ptKey { get; set; }
		}
		public class GetReferralSearchRequest : CheckUserRequest
		{
			public string OrganizationParam { get; set; }
			public string FacilityAddress { get; set; }
			public string FacilityCity { get; set; }
			public string FacilityState { get; set; }
			public string FacilityZip { get; set; }
			public string FacilityPhone { get; set; }
			public string FacilityFax { get; set; }
			public string FacilityKey { get; set; }
			public int LimitParam { get; set; }
			public int OffsetParam { get; set; }
			public string OrderByFieldParam { get; set; }
			public string SortOrderParam { get; set; }
			public string SearchParam { get; set; }
			public string Id { get; set; }
			public int? UserIdForReferralSearch { get; set; }
			public int RequestForGetUserDetails { get; set; }

		}
		public class GetSummaryDocumentRequest : CheckUserRequest
		{
			public int Days { get; set; }
			public string PatientIdFlag { get; set; }
		}
		public class GetResmedSummaryDocument : CheckUserRequest
		{
			public int Days { get; set; }
			public string AirViewFlag { get; set; }
		}

		public class GetMaskTypeRequest : CheckUserRequest
		{
			public string OrganizationParam { get; set; }
			public string ReferralParam { get; set; }
		}
		#endregion

		#region Non-PAP Dashboard Request Model
		public class GetNonPAPDashboardRequest : CheckUserRequest
		{
			public int RoleId { get; set; }
			public string OrganizationParam { get; set; }
			public string ReferralParam { get; set; }
		}
		public class GetNonPAPSalesOrderByStatusRequest : CheckUserRequest
		{
			public bool IsStatusNameParam { get; set; }
			public string StatusNameParam { get; set; }
			public string ReferralParam { get; set; }
			public int DateRange { get; set; }
		}

		public class GetNonPAPDeliveredAndUnDeliveredChartRequest : CheckUserRequest
		{
			public string Organization { get; set; }
			public string ReferralKey { get; set; }
		}

		public class GetNonPAPSalesOrderByFiltersRequest : CheckUserRequest
		{
			public string StatusParam { get; set; }
			public string StartDate { get; set; }
			public string EndDate { get; set; }
			public string PatientNameParam { get; set; }
			public string PatientFirstNameParam { get; set; }
			public string PatientNameLastParam { get; set; }
			public string DateOfBirthParam { get; set; }
			public string OrganizationParam { get; set; }
			public string ReferralParam { get; set; }
			public string DoctorParam { get; set; }
			public string DoctorNameParam { get; set; }
			public string DoctorFirstNameParam { get; set; }
			public string DoctorLastNameParam { get; set; }
			public string InsuranceParam { get; set; }
			public string ComplianceRiskParam { get; set; }
			public string EquipmentTypeParam { get; set; }
		}
		public class GetNonPAPPatientDataRequest : CheckUserRequest
		{
			public string PatientNameParam { get; set; }
			public string ReferralKey { get; set; }
		}
		#endregion

		#region Screened Patient Request Model

		public class GetSalesRepresentativesRequest : CheckUserRequest
		{

		}

		public class CheckSleepScreenerAccessRequest : CheckUserRequest
		{

		}
		public class GetScreenedPatientListRequest : CheckUserRequest
		{
			public string ReferralParam { get; set; }
			public string Organization { get; set; }
			public string PatientFirstName { get; set; }
			public string PatientLastName { get; set; }
			public string SalesRep { get; set; }
			public string DateOfBirth { get; set; }
			public string FollowUpStatus { get; set; }
			public int IsAdmin { get; set; }
		}

		public class GetScreenerDataRequest : CheckUserRequest
		{
			public string PtKey { get; set; }
			public string NickName { get; set; }
			public string type { get; set; }
		}

		public class GetFlayerForSleepScreenerRequest : CheckUserRequest
		{
			public string FacilityName { get; set; }
			public string ScreenerLink { get; set; }
		}

		public class GetPatientNotesRequest : CheckUserRequest
		{
			public string PtKey { get; set; }
			public string NickName { get; set; }
		}

		public class UpdateFollowUpStatusRequest : CheckUserRequest
		{
			public int PtKey { get; set; }
			public string NickName { get; set; }
			public string FollowUpStatus { get; set; }
		}

		public class AddAndUpdatePatientNoteRequest : CheckUserRequest
		{
			public string StatementType { get; set; }
			public int? PatientNotesId { get; set; }
			public int PtKey { get; set; }
			public string NickName { get; set; }
			public string CreatedBy { get; set; }
			public string Description { get; set; }
		}

		public class DeletePatientNoteRequest : CheckUserRequest
		{
			public int? PatientNotesId { get; set; }
			public int PtKey { get; set; }
			public string NickName { get; set; }
		}
		#endregion
	}
}
