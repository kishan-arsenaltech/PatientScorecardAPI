using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scorecard.API.BusinessLogic;
using Scorecard.API.Model;
using Microsoft.Extensions.Configuration;

namespace Scorecard.API.Controllers
{
	[Route("api/dashboard")]
	[ApiController]
	public class DashboardController : Controller
	{
		private readonly IDashboard _IDashboard;
		public IConfiguration _configuration { get; }

		public DashboardController(IDashboard dashboard) => _IDashboard = dashboard;

		#region PAP Dashboard Action
		/// <summary>
		/// This action is use to get permission based modules list and chart details for dashboard.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getdashboarddetail")]
		public DashboardResponse.GetDashboardResult GetDashboardDetail([FromQuery] DashboardRequest.GetDashboardRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.GetDashboardDetails(RequestParam);
		}

		/// <summary>
		/// This action is use to get Adherence Breakdown Detail for showing on pie chart while user change option from dropdown.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getadherencebreakdowndetail")]
		public DashboardResponse.GetAdherenceBreakdownResult GetAdherenceBreakdownDetail([FromQuery] DashboardRequest.GetDashboardRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.GetAdherenceBreakdownDetail(RequestParam);
		}

		/// <summary>
		/// This action is use to get chart details for delivered and undelivered orders.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getdeliveredandcomplaintchartdetail")]
		public DashboardResponse.GetDeliveredAndComplaintChartResult GetDeliveredAndComplaintChartDetail([FromQuery] DashboardRequest.GetDeliveredAndComplaintChartRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetDeliveredAndComplaintChartDetail(RequestParam);
		}

		/// <summary>
		/// This action is use to get salesorder list based on search criteria.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getsalesorderfilterlist")]
		public DashboardResponse.GetSalesOrderByFilterResult GetSalesOrderDetailsFilterList([FromQuery] DashboardRequest.GetSalesOrderByFilterRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetSalesOrderDetailsFilterList(RequestParam);
		}

		/// <summary>
		/// This action is use to get list of doctor names based on search criteria.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getdoctorlist")]
		public DashboardResponse.GetDoctorListResult GetDoctorList([FromQuery] DashboardRequest.GetDoctorsRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetDoctorList(RequestParam);
		}

		/// <summary>
		/// This action is use to get average patient usage data.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getaveragepatientusagedata")]
		public DashboardResponse.GetAveragePatientUsageDataResult GetAveragePatientUsageData([FromQuery] DashboardRequest.GetAveragePatientUsageDataRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetAveragePatientUsageData(RequestParam);
		}

		/// <summary>
		/// This action is use to get Referrals data.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getreferralsearchlist")]
		public DashboardResponse.GetReferralSearchResult GetReferralSearchList([FromQuery] DashboardRequest.GetReferralSearchRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetReferralSearchList(RequestParam);
		}

		/// <summary>
		/// This action is use to download pdf document based on patient's airview flag
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getresmedsummarydocument")]
		public async Task<CommonResponse> GetResmedSummaryDocument([FromQuery] DashboardRequest.GetSummaryDocumentRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return await _IDashboard.GetResmedSummaryDocument(RequestParam);
		}

		[HttpGet]
		[Route("getphillipsummarydocument")]
		public async Task<DashboardResponse.GetSummaryDocumentResponse> GetPhillipSummaryDocument([FromQuery] DashboardRequest.GetSummaryDocumentRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return await _IDashboard.GetPhillipSummaryDocument(RequestParam);
		}

		[HttpGet]
		[Route("GetMaskTypeResult")]
		public DashboardResponse.GetMaskTypeResult GetMaskTypeResult([FromQuery] DashboardRequest.GetMaskTypeRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetMaskTypeResult(RequestParam);
		}
		#endregion PAP Dashboard Controller

		#region Non-PAP Dashboard Action 
		/// <summary>
		/// This action is use to get permission based modules list and chart details for dashboard.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getnonpapdashboarddetails")]
		public DashboardResponse.GetNonPAPDashboardResult GetNonPAPDashboardDetails([FromQuery] DashboardRequest.GetNonPAPDashboardRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.GetNonPAPDashboardDetails(RequestParam);
		}

		/// <summary>
		/// This action is use to get Non-PAP salesorder list based on status.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getnonpapsalesorderdetailslistbystatus")]
		public DashboardResponse.GetNonPAPSalesOrderByStatusResult GetNonPAPSalesOrderDetailsListByStatus([FromQuery] DashboardRequest.GetNonPAPSalesOrderByStatusRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetNonPAPSalesOrderDetailsListByStatus(RequestParam);
		}

		/// <summary>
		/// This action is use to get chart details for Non-PAP delivered and undelivered orders.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getnonpapdeliveredandundeliveredchartdetail")]
		public DashboardResponse.GetNonPAPDeliveredAndUnDeliveredChartResult GetNonPAPDeliveredAndUnDeliveredChartDetail([FromQuery] DashboardRequest.GetNonPAPDeliveredAndUnDeliveredChartRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetNonPAPDeliveredAndUnDeliveredChartDetail(RequestParam);
		}

		/// <summary>
		/// This action is use to get Non-PAP salesorder list based on filter.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getnonpapsalesorderdetailslistbyfilters")]
		public DashboardResponse.GetNonPAPSalesOrderByFiltersResult GetNonPAPSalesOrderDetailsListByFilters([FromQuery] DashboardRequest.GetNonPAPSalesOrderByFiltersRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetNonPAPSalesOrderDetailsListByFilters(RequestParam);
		}

		/// <summary>
		/// This action is use to get Non-PAP patient details.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getnonpappatientdetail")]
		public DashboardResponse.GetNonPAPPatientDataResult GetNonPAPPatientDetail([FromQuery] DashboardRequest.GetNonPAPPatientDataRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			return _IDashboard.GetNonPAPPatientDetail(RequestParam);
		}
		#endregion Non-PAP Dashboard Controller

		#region Screened Patient
		/// <summary>
		/// This action is use to check that user is eligible for sleep screener access.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("checkformenuitemaccess")]
		public DashboardResponse.CheckSleepScreenerAcessResult CheckForMenuItemAccess([FromQuery] DashboardRequest.CheckSleepScreenerAccessRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.CheckForMenuItemAccess(RequestParam);
		}

		/// <summary>
		/// This action is use to get list of sales representatives.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getsalesrepresentatives")]
		public DashboardResponse.GetSalesRepresentativesResult GetSalesRepresentatives([FromQuery] DashboardRequest.GetSalesRepresentativesRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.GetSalesRepresentatives(RequestParam);
		}


		/// <summary>
		/// This action is use to get list of screened patient for showing into UI.
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getscreenedpatientlist")]
		public DashboardResponse.GetScreenedPatientListResult GetScreenedPatientList([FromQuery] DashboardRequest.GetScreenedPatientListRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.GetScreenedPatientList(RequestParam);
		}

		/// <summary>
		/// This action is use to get screened data for particular patient based on ptkey and nickname
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getscreeneddata")]
		public DashboardResponse.GetScreenerDataResult GetScreenedData([FromQuery] DashboardRequest.GetScreenerDataRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.GetScreenedData(RequestParam);
		}

		/// <summary>
		/// This action is use to create pdf document for selected sleep screener
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getflayerforsleepscreener")]
		public DashboardResponse.GetFlayerForSleepScreenerResult GetFlayerForSleepScreener([FromQuery] DashboardRequest.GetFlayerForSleepScreenerRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.GetFlayerForSleepScreener(RequestParam);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("getpatientnotelist")]
		public DashboardResponse.GetPatientNotesResult GetPatientNoteList([FromQuery] DashboardRequest.GetPatientNotesRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.GetPatientNoteList(RequestParam);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("updatefollowupstatus")]
		public DashboardResponse.UpdateFollowUpStatusResponse UpdateFollowUpStatus(DashboardRequest.UpdateFollowUpStatusRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.UpdateFollowUpStatus(RequestParam);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("addandupdatepatientnote")]
		public DashboardResponse.AddAndUpdatePatientNoteResponse AddAndUpdatePatientNote(DashboardRequest.AddAndUpdatePatientNoteRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.AddAndUpdatePatientNote(RequestParam);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="RequestParam"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("deletepatientnote")]
		public DashboardResponse.DeletePatientNoteResponse DeletePatientNote(DashboardRequest.DeletePatientNoteRequest RequestParam)
		{
			RequestParam.AccessToken = Request.Headers["AccessToken"];
			RequestParam.UserId = Request.Headers["UserId"];
			RequestParam.UserAgent = Request.Headers["User-Agent"];
			return _IDashboard.DeletePatientNote(RequestParam);
		}
		#endregion
	}
}
