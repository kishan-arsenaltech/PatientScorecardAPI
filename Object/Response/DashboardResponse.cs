using System;
using System.Collections.Generic;
using System.Text;

namespace Scorecard.API.Model
{
	public class DashboardResponse
	{
		#region PAP Dashboard Response Model
		public class GetDashboardResult : CommonResponse
		{
			public List<PermissonRoleResponse> LiPermissionResponse { get; set; }
			public List<GetSalesOrderBreakDownResponse> LiSalesOrderBreakDownResponse { get; set; }
			public List<GetComplianceBreakDownResponse> LiComplianceBreakDownResponse { get; set; }
			public List<GetInsuranceNameResponse> LiInsuranceNameResponse { get; set; }
			public List<GetSalesRepresentativesResponse> LiSalesRepresentativesResponse { get; set; }
		}

		public class PermissonRoleResponse
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string DisplayName { get; set; }
			public string Description { get; set; }
		}

		public class GetSalesOrderBreakDownResponse
		{
			public string OrderStatus { get; set; }
			public int Total { get; set; }
			public string Color { get; set; }
		}

		public class GetComplianceBreakDownResponse
		{
			public int? Totals { get; set; }
			public string Risk { get; set; }
			public int? AverageAdherence30 { get; set; }
			public int? AverageAdherence60 { get; set; }
			public int? AverageAdherence90 { get; set; }
		}

		public class GetAdherenceBreakdownResult : CommonResponse
		{
			public List<GetComplianceBreakDownResponse> LiComplianceBreakDownResponse { get; set; }
		}

		public class GetDeliveredAndComplaintChartResult : CommonResponse
		{
			public List<GetDeliveredAndUndeliveredResponse> LiDeliveredAndUndeliveredResponse { get; set; }
			public List<GetComplaintCountResponse> LiComplaintCountResponse { get; set; }
		}

		public class GetDeliveredAndUndeliveredResponse
		{
			public string UnDeliveredOrderStatus { get; set; }
			public int UnDeliveredTotal { get; set; }
			public int UnDeliveredOrderDay { get; set; }
			public int UnDeliveredOrderMonth { get; set; }
			public int UnDeliveredOrderYear { get; set; }
			public string DeliveredOrderStatus { get; set; }
			public int DeliveredTotal { get; set; }
			public int DeliveredOrderDay { get; set; }
			public int DeliveredOrderMonth { get; set; }
			public int DeliveredOrderYear { get; set; }
		}

		public class GetComplaintCountResponse
		{
			public int CurrentCompliance { get; set; }
			public int Totals { get; set; }
			public int OrderMonth { get; set; }
			public int OrderYear { get; set; }
			public int ReferralKey { get; set; }
			public string NickName { get; set; }
		}

		public class GetSalesOrderByFilterResult : CommonResponse
		{
			public List<GetSalesOrderByFilterResponse> LiSalesOrderFilterResponse { get; set; }

			public List<GetSalesOrderByVoidReasonResponse> LiSalesOrderByVoidReasonResponse { get; set; }

			public int ListCount { get; set; }
		}

		public class GetSalesOrderByFilterResponse
		{
			private double _CompliantPctSinceSetup;
			public long RowId { get; set; }
			public string MaskType { get; set; }
			public int PtKey { get; set; }
			public int ReferralKey { get; set; }
			public string PatientName { get; set; }
			public string DoctorName { get; set; }
			public string DoctorFirstName { get; set; }
			public string PatientFirstName { get; set; }
			public string PatientLastName { get; set; }
			public string DOB { get; set; }
			public string LastAttempt { get; set; }
			public int SOKey { get; set; }
			public DateTime? SOCreateDT { get; set; }
			public string OrderStatus { get; set; }
			public string OrderNote { get; set; }
			public string Pt_MobileNumber { get; set; }
			public string Pt_PhoneNumber { get; set; }
			public DateTime? DeliveredDt { get; set; }
			public string PAPMachineType { get; set; }
			public string Equipment { get; set; }
			public string Referral { get; set; }
			public string Primary_Insurance { get; set; }
			public string EncoreFlag { get; set; }
			public string AirviewFlag { get; set; }
			public string SalesRep { get; set; }
			public string Risk { get; set; }
			public string CompliantDaysSinceSetup { get; set; }
			public double CompliantPctSinceSetup
			{
				get
				{
					return _CompliantPctSinceSetup;
				}
				set
				{
					_CompliantPctSinceSetup = (value * 100);
				}
			}
			public double DaysSinceSetUp { get; set; }
			public string CompliancePct { get; set; }
			public DateTime? Best30StartDT { get; set; }
			public DateTime? Best30EndDT { get; set; }
			public double Best30Count { get; set; }
			public decimal Best30Pct { get; set; }
			public double Compliant30 { get; set; }
			public double Compliant60 { get; set; }
			public double Compliant90 { get; set; }
			public int? TotalAttempts { get; set; }
		}

		public class GetSalesOrderByVoidReasonResponse
		{
			public int ReferralKey { get; set; }
			public string PatientName { get; set; }
			public DateTime DOB { get; set; }
			public string Pt_PhoneNumber { get; set; }
			public DateTime CreateDt { get; set; }
			public string OrderStatus { get; set; }
			public DateTime VoidDt { get; set; }
			public string Equipment { get; set; }
			public string Void_Reason { get; set; }
			public string DoctorName { get; set; }
			public string DoctorNPI { get; set; }
			public string Primary_Insurance { get; set; }
			public string SalesRep { get; set; }
			public string Referral { get; set; }
			public string EncoreFlag { get; set; }
			public string AirviewFlag { get; set; }
		}

		public class GetDoctorListResult : CommonResponse
		{
			public List<GetDoctorResponse> ListDoctorResponse { get; set; }
			public int ListCount { get; set; }
		}

		public class GetDoctorResponse
		{
			public string DoctorName { get; set; }
		}
		public class GetSalesRepresentativesResponse
		{
			public string SalesRep { get; set; }
			public int MktRepKey { get; set; }
		}
		public class GetInsuranceNameResponse
		{
			public string PayorName { get; set; }
		}
		public class GetAveragePatientUsageDataResult : CommonResponse
		{
			public List<GetAveragePatientUsageDataResponse> LiGetAveragePatientUsageData { get; set; }
		}
		public class GetAveragePatientUsageDataResponse
		{
			public string Vendor { get; set; }
			public decimal AverageMaxLeaks30 { get; set; }
			public decimal AverageMaxLeaks60 { get; set; }
			public decimal AverageMaxLeaks90 { get; set; }
			public decimal AverageMaxAHI30 { get; set; }
			public decimal AverageMaxAHI60 { get; set; }
			public decimal AverageMaxAHI90 { get; set; }

		}
		public class GetReferralSearchResult : CommonResponse
		{
			public List<GetReferralSearchResponse> LiGetReferralSearch { get; set; }
		}
		public class GetReferralSearchResponse
		{
			public string NickName { get; set; }
			public string Referral { get; set; }
			public int ReferralKey { get; set; }
			public string ReferralType { get; set; }
		}

		public class GetResmedSummaryResponse : CommonResponse
		{
			public string DocumentPath { get; set; }
		}

		public class GetSummaryDocumentResponse : CommonResponse
		{
			public string DocumentPath { get; set; }
		}

		public class GetMaskTypeResult : CommonResponse
		{
			public List<GetMaskTypeResponse> LiMaskType { get; set; }
		}


		public class GetMaskTypeResponse : CommonResponse
		{
			public string MaskType { get; set; }
		}

		#endregion

		#region Non-PAP Dashboard Response Model

		public class GetNonPAPDashboardResult : CommonResponse
		{
			public List<PermissonRoleResponse> LiPermissionResponse { get; set; }
			public List<GetNonPAPSalesOrderBreakDownResponse> LiNonPAPSalesOrderBreakDownResponse { get; set; }
			public List<GetInsuranceNameResponse> LiInsuranceNameResponse { get; set; }
			public List<GetSalesRepresentativesResponse> LiSalesRepresentativesResponse { get; set; }
		}

		public class GetNonPAPSalesOrderBreakDownResponse
		{
			public string OrderStatus { get; set; }
			public string PatientName { get; set; }
			public int OrderNumber { get; set; }
			public int Total { get; set; }
			public string Color { get; set; }
		}

		public class GetNonPAPSalesOrderByStatusResult : CommonResponse
		{
			public List<GetNonPAPSalesOrderByStatusResponse> LiNonPAPSalesOrderByStatusResponse { get; set; }
			public int ListCount { get; set; }
		}
		public class GetNonPAPSalesOrderByStatusResponse
		{
			public string PatientName { get; set; }
			public string DOB { get; set; }
			public string Phone { get; set; }
			public string Physician { get; set; }
			public string DoctorNPI { get; set; }

		}

		public class GetNonPAPDeliveredAndUnDeliveredChartResult : CommonResponse
		{
			public List<GetNonPAPDeliveredAndUndeliveredResponse> LiNonPAPDeliveredAndUndeliveredResponse { get; set; }
		}
		public class GetNonPAPDeliveredAndUndeliveredResponse
		{
			public string UnDeliveredOrderStatus { get; set; }
			public int UnDeliveredTotal { get; set; }
			public int UnDeliveredOrderDay { get; set; }
			public int UnDeliveredOrderMonth { get; set; }
			public int UnDeliveredOrderYear { get; set; }
			public string DeliveredOrderStatus { get; set; }
			public int DeliveredTotal { get; set; }
			public int DeliveredOrderDay { get; set; }
			public int DeliveredOrderMonth { get; set; }
			public int DeliveredOrderYear { get; set; }
		}

		public class GetNonPAPSalesOrderByFiltersResult : CommonResponse
		{
			public List<GetNonPAPSalesOrderByFiltersResponse> LiNonPAPSalesOrderByFiltersResponse { get; set; }
			public int ListCount { get; set; }
		}
		public class GetNonPAPSalesOrderByFiltersResponse
		{
			public string PatientName { get; set; }
			public DateTime DOB { get; set; }
			public string Phone { get; set; }
			public string Physician { get; set; }
			public string DoctorNPI { get; set; }
		}
		public class GetNonPAPPatientDataResult : CommonResponse
		{
			public List<GetNonPAPPatientDataResponse> LiNonPAPPatientDataResponse { get; set; }
			public List<GetCancelledOrderResponse> LiCancelledOrderResponse { get; set; }
			public List<GetOutsourcedOrderResponse> LiOutsourcedOrderResponse { get; set; }
			public List<GetPatientNotesDetail> LiPatientNotesDetailResponse { get; set; }
			public bool IsInternalRole { get; set; }
		}
		public class GetNonPAPPatientDataResponse
		{
			public string Classification { get; set; }
			public string NickName { get; set; }
			public string ReferralId { get; set; }
			public string Chain { get; set; }
			public string PatientName { get; set; }
			public string OrderType { get; set; }
			public string ReferralName { get; set; }
			public string DateEntered { get; set; }
			public string StatusDate { get; set; }
			public string OrderStatus { get; set; }
			public string TrackingNbr { get; set; }
			public string TrackingNbrUrl { get; set; }
			public string ProdDesc { get; set; }
			public string UMO { get; set; }
			public decimal OrderedQty { get; set; }
			public decimal ShippedQty { get; set; }
			public string Payer { get; set; }
			public int OrderNumber { get; set; }
			public string Physician { get; set; }
			public string DoctorNPI { get; set; }
			public string Phone { get; set; }
			public DateTime DOB { get; set; }
		}

		public class GetCancelledOrderResponse
		{
			public int PtKey { get; set; }
			public string Nickname { get; set; }
			public int NoteID { get; set; }
			public string Description { get; set; }
			public string CreateDt { get; set; }
		}

		public class GetOutsourcedOrderResponse
		{
			public int PtKey { get; set; }
			public string Nickname { get; set; }
			public int NoteID { get; set; }
			public string Description { get; set; }
			public string CreateDt { get; set; }
		}

		public class GetPatientNotesDetail
		{
			public int PatientNoteId { get; set; }
			public string Title { get; set; }
			public string NoteDescription { get; set; }
			public string CreateDt { get; set; }
		}
		#endregion

		#region Screened Patient Response Model

		public class CheckSleepScreenerAcessResult : CommonResponse
		{
			public int SleepScreenerAccess { get; set; }
			public int GlobalSummaryAccess { get; set; }
		}

		public class GetSalesRepresentativesResult : CommonResponse
		{
			public List<GetSalesRepresentativesResponse> LiSalesRepresentativesResponse { get; set; }
		}

		public class GetScreenedPatientListResult : CommonResponse
		{
			public List<GetScreenedPatientListResponse> LiScreenedPatientListResponse { get; set; }
		}

		public class GetScreenedPatientListResponse
		{
			public int PtKey { get; set; }
			public string PatientId { get; set; }
			public string NickName { get; set; }
			public string PatientName { get; set; }
			public string Phone { get; set; }
			public DateTime DOB { get; set; }
			public string ScreeningDate { get; set; }
			public string FacilityName { get; set; }
			public string HealthProvider { get; set; }
			public string FollowUpStatus { get; set; }
			public string OSARisk { get; set; }
			public string SalesOrderList { get; set; }
		}

		public class GetScreenerDataResult : CommonResponse
		{
			public List<GetScreenerDataResponse> LiScreenerDataResponse { get; set; }
		}

		public class GetScreenerDataResponse
		{
			public string ExistingConditions { get; set; }
			public int Diabetes { get; set; }
			public int Hypertension { get; set; }
			public int GeneralFatigue { get; set; }
			public int Obesity { get; set; }
			public int CongestiveHeartFailure { get; set; }
			public int Pacemakers { get; set; }
			public int AtrialFibrillation { get; set; }
			public int CoronaryArteryDisease { get; set; }
			public int Stroke { get; set; }
			public int SevereBackPain { get; set; }
			public int JointPain { get; set; }
			public int HighCholesterol { get; set; }

			public int Snoring { get; set; }
			public int Tired { get; set; }
			public int Observed { get; set; }
			public int Pressure { get; set; }
			public int BodyMass { get; set; }
			public int AgeOlder { get; set; }
			public int NeckSize { get; set; }
			public int GenderMale { get; set; }
		}

		public class GetFlayerForSleepScreenerResult : CommonResponse
		{

		}

		public class GetPatientNotesResult : CommonResponse
		{
			public List<GetPatientNotesResponse> LiPatientNotesResponse { get; set; }
		}

		public class GetPatientNotesResponse
		{
			public long PatientNotesId { get; set; }
			public int PtKey { get; set; }
			public string NickName { get; set; }
			public string CreatedBy { get; set; }
			public string Description { get; set; }
			public string UpdatedOnDate { get; set; }
		}

		public class UpdateFollowUpStatusResponse : CommonResponse { }
		public class AddAndUpdatePatientNoteResponse : CommonResponse { }

		public class DeletePatientNoteResponse : CommonResponse { }
		#endregion
	}
}
