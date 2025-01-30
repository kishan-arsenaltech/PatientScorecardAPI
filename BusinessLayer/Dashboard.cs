using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Scorecard.API.DataAccess;
using Scorecard.API.Model;
using System.Threading.Tasks;

namespace Scorecard.API.BusinessLogic
{
	public class Dashboard : IDashboard
	{
		public IDashboardDataAccess _IDashboardDataAccess;
		public readonly IConfiguration _configuration;
		public Dashboard(IDashboardDataAccess dashbaordData) => _IDashboardDataAccess = dashbaordData;
		dynamic Response;

		#region PAP Dashboard Business Logic
		public DashboardResponse.GetDashboardResult GetDashboardDetails(DashboardRequest.GetDashboardRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetDashboardDetails(RequestParam);
			if (Response == null)
			{
				Response.LiPermissionResponse = null;
				Response.LiSalesOrderBreakDownResponse = null;
				Response.LiComplianceBreakDownResponse = null;
				Response.Message = "Data not found while fetching dashboard details.";
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetAdherenceBreakdownResult GetAdherenceBreakdownDetail(DashboardRequest.GetDashboardRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetAdherenceBreakdownDetail(RequestParam);
			if (Response == null)
			{
				Response.LiComplianceBreakDownResponse = null;
				Response.Message = "Data not found while fetching Adherence Breakdown Detail.";
				Response.Success = false;
			}

			return Response;
		}
		public DashboardResponse.GetDeliveredAndComplaintChartResult GetDeliveredAndComplaintChartDetail(DashboardRequest.GetDeliveredAndComplaintChartRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetDeliveredAndComplaintChartDetail(RequestParam);
			if (Response == null)
			{
				Response.LiChartResponse = null;
				Response.ListCount = 0;
				Response.Message = "Data not found while fetching chart details.";
				Response.Success = false;
			}

			return Response;
		}
		public DashboardResponse.GetSalesOrderByFilterResult GetSalesOrderDetailsFilterList(DashboardRequest.GetSalesOrderByFilterRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetSalesOrderDetailsFilterList(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Data not found while fetching salesorder search result.";
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetDoctorListResult GetDoctorList(DashboardRequest.GetDoctorsRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetDoctorList(RequestParam);
			if (Response == null)
			{
				Response.ListDoctorResponse = null;
				Response.ListCount = 0;
				Response.Message = "Data not found while fetching doctor list.";
				Response.Success = false;
			}

			return Response;
		}
		public DashboardResponse.GetAveragePatientUsageDataResult GetAveragePatientUsageData(DashboardRequest.GetAveragePatientUsageDataRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetAveragePatientUsageData(RequestParam);
			if (Response == null)
			{
				Response.ListDoctorResponse = null;
				Response.ListCount = 0;
				Response.Message = "Data not found while fetching average patient usage data.";
				Response.Success = false;
			}

			return Response;
		}
		public DashboardResponse.GetReferralSearchResult GetReferralSearchList(DashboardRequest.GetReferralSearchRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetReferralSearchList(RequestParam);
			if (Response == null)
			{
				Response.Message = "Data not found while fetching Referrals list.";
				Response.Success = false;
			}
			return Response;
		}

		public async Task<DashboardResponse.GetSummaryDocumentResponse> GetResmedSummaryDocument(DashboardRequest.GetSummaryDocumentRequest RequestParam)
		{
			Response = new JObject();
			Response = await _IDashboardDataAccess.GetResmedSummaryDocument(RequestParam);
			if (Response == null)
			{
				Response.Message = "Data not found while fetching document.";
				Response.Success = false;
			}

			return Response;
		}
		public async Task<DashboardResponse.GetSummaryDocumentResponse> GetPhillipSummaryDocument(DashboardRequest.GetSummaryDocumentRequest RequestParam)
		{
			Response = new JObject();
			Response = await _IDashboardDataAccess.GetPhillipSummaryDocument(RequestParam);
			if (Response == null)
			{
				Response.Message = "Data not found while fetching document.";
				Response.Success = false;
			}

			return Response;
		}
		public DashboardResponse.GetMaskTypeResult GetMaskTypeResult(DashboardRequest.GetMaskTypeRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetMaskTypeResult(RequestParam);
			if (Response == null)
			{
				Response.Message = "Data not found while fetching mask list.";
				Response.Success = false;
			}

			return Response;
		}
		#endregion

		#region Non-PAP Dashboard Business Logic
		public DashboardResponse.GetNonPAPDashboardResult GetNonPAPDashboardDetails(DashboardRequest.GetNonPAPDashboardRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetNonPAPDashboardDetails(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Data not found while fetching dashboard details.";
				Response.Success = false;
			}

			return Response;
		}
		public DashboardResponse.GetNonPAPSalesOrderByStatusResult GetNonPAPSalesOrderDetailsListByStatus(DashboardRequest.GetNonPAPSalesOrderByStatusRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetNonPAPSalesOrderDetailsListByStatus(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Non-PAP Salesorder data not found by status for this referral.";
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetNonPAPDeliveredAndUnDeliveredChartResult GetNonPAPDeliveredAndUnDeliveredChartDetail(DashboardRequest.GetNonPAPDeliveredAndUnDeliveredChartRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetNonPAPDeliveredAndUnDeliveredChartDetail(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Data not found while fetching chart data.";
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetNonPAPSalesOrderByFiltersResult GetNonPAPSalesOrderDetailsListByFilters(DashboardRequest.GetNonPAPSalesOrderByFiltersRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetNonPAPSalesOrderDetailsListByFilters(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Salesorder data not found for this referral.";
				Response.Success = false;
			}

			return Response;
		}
		public DashboardResponse.GetNonPAPPatientDataResult GetNonPAPPatientDetail(DashboardRequest.GetNonPAPPatientDataRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetNonPAPPatientDetail(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Patient information not found for this patient.";
				Response.Success = false;
			}

			return Response;
		}
		#endregion

		#region Screened Patient

		public DashboardResponse.CheckSleepScreenerAcessResult CheckForMenuItemAccess(DashboardRequest.CheckSleepScreenerAccessRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.CheckForMenuItemAccess(RequestParam);
			if (Response == null)
			{
				Response.SleepScreenerAcces = null;
				Response.Message = "Something went wrong while checking for sleep screener access.";
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetSalesRepresentativesResult GetSalesRepresentatives(DashboardRequest.GetSalesRepresentativesRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetSalesRepresentatives(RequestParam);
			if (Response == null)
			{
				Response.SleepScreenerAcces = null;
				Response.Message = "Something went wrong while getting list of sales representatives.";
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetScreenedPatientListResult GetScreenedPatientList(DashboardRequest.GetScreenedPatientListRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetScreenedPatientList(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Screened patient list not found.";
				Response.Success = false;
			}
			return Response;
		}
		public DashboardResponse.GetScreenerDataResult GetScreenedData(DashboardRequest.GetScreenerDataRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetScreenedData(RequestParam);
			if (Response == null)
			{
				Response.LiScreenerDataResponse = null;
				Response.ListCount = 0;
				Response.Message = "Screened data for this patient not found.";
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetFlayerForSleepScreenerResult GetFlayerForSleepScreener(DashboardRequest.GetFlayerForSleepScreenerRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetFlayerForSleepScreener(RequestParam);
			if (Response == null)
			{
				Response.Message = string.Format("Failed to create flayer for {0}.)", RequestParam.FacilityName);
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetPatientNotesResult GetPatientNoteList(DashboardRequest.GetPatientNotesRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.GetPatientNoteList(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Patient notes not found for this patient.";
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.UpdateFollowUpStatusResponse UpdateFollowUpStatus(DashboardRequest.UpdateFollowUpStatusRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.UpdateFollowUpStatus(RequestParam);
			if (Response == null)
			{
				Response.Message = "Something went wrong while updating Follow-Up Status.";
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.AddAndUpdatePatientNoteResponse AddAndUpdatePatientNote(DashboardRequest.AddAndUpdatePatientNoteRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.AddAndUpdatePatientNote(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Something went wrong while inserting/updating patient note.";
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.DeletePatientNoteResponse DeletePatientNote(DashboardRequest.DeletePatientNoteRequest RequestParam)
		{
			Response = new JObject();
			Response = _IDashboardDataAccess.DeletePatientNote(RequestParam);
			if (Response == null)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = "Something went wrong while inserting/updating patient note.";
				Response.Success = false;
			}
			return Response;
		}
		#endregion

		public bool RequestForPdf(CheckUserRequest RequestParam)
		{
			return _IDashboardDataAccess.RequestForPdf(RequestParam);
		}
	}
}
