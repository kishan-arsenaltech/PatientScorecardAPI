﻿using Scorecard.API.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorecard.API.DataAccess
{
	public interface IDashboardDataAccess
	{
		#region PAP Dashboard DataAccess Interface
		public DashboardResponse.GetDashboardResult GetDashboardDetails(DashboardRequest.GetDashboardRequest RequestParam);
		public DashboardResponse.GetAdherenceBreakdownResult GetAdherenceBreakdownDetail(DashboardRequest.GetDashboardRequest RequestParam);
		public DashboardResponse.GetDeliveredAndComplaintChartResult GetDeliveredAndComplaintChartDetail(DashboardRequest.GetDeliveredAndComplaintChartRequest RequestParam);
		public DashboardResponse.GetSalesOrderByFilterResult GetSalesOrderDetailsFilterList(DashboardRequest.GetSalesOrderByFilterRequest RequestParam);
		public DashboardResponse.GetDoctorListResult GetDoctorList(DashboardRequest.GetDoctorsRequest RequestParam);
		public DashboardResponse.GetAveragePatientUsageDataResult GetAveragePatientUsageData(DashboardRequest.GetAveragePatientUsageDataRequest RequestParam);
		public DashboardResponse.GetReferralSearchResult GetReferralSearchList(DashboardRequest.GetReferralSearchRequest RequestParam);
		public Task<DashboardResponse.GetSummaryDocumentResponse> GetResmedSummaryDocument(DashboardRequest.GetSummaryDocumentRequest RequestParam);
		public Task<DashboardResponse.GetSummaryDocumentResponse> GetPhillipSummaryDocument(DashboardRequest.GetSummaryDocumentRequest RequestParam);
		public DashboardResponse.GetMaskTypeResult GetMaskTypeResult(DashboardRequest.GetMaskTypeRequest RequestParam);
		#endregion

		#region Non-PAP Dashboard DataAccess Interface
		public DashboardResponse.GetNonPAPDashboardResult GetNonPAPDashboardDetails(DashboardRequest.GetNonPAPDashboardRequest RequestParam);
		public DashboardResponse.GetNonPAPSalesOrderByStatusResult GetNonPAPSalesOrderDetailsListByStatus(DashboardRequest.GetNonPAPSalesOrderByStatusRequest RequestParam);
		public DashboardResponse.GetNonPAPDeliveredAndUnDeliveredChartResult GetNonPAPDeliveredAndUnDeliveredChartDetail(DashboardRequest.GetNonPAPDeliveredAndUnDeliveredChartRequest RequestParam);
		public DashboardResponse.GetNonPAPSalesOrderByFiltersResult GetNonPAPSalesOrderDetailsListByFilters(DashboardRequest.GetNonPAPSalesOrderByFiltersRequest RequestParam);
		public DashboardResponse.GetNonPAPPatientDataResult GetNonPAPPatientDetail(DashboardRequest.GetNonPAPPatientDataRequest RequestParam);
		#endregion

		#region Screened Patient
		public DashboardResponse.CheckSleepScreenerAcessResult CheckForMenuItemAccess(DashboardRequest.CheckSleepScreenerAccessRequest RequestParam);
		public DashboardResponse.GetSalesRepresentativesResult GetSalesRepresentatives(DashboardRequest.GetSalesRepresentativesRequest RequestParam);
		public DashboardResponse.GetScreenedPatientListResult GetScreenedPatientList(DashboardRequest.GetScreenedPatientListRequest RequestParam);
		public DashboardResponse.GetScreenerDataResult GetScreenedData(DashboardRequest.GetScreenerDataRequest RequestParam);
		public DashboardResponse.GetFlayerForSleepScreenerResult GetFlayerForSleepScreener(DashboardRequest.GetFlayerForSleepScreenerRequest RequestParam);
		public DashboardResponse.GetPatientNotesResult GetPatientNoteList(DashboardRequest.GetPatientNotesRequest RequestParam);
		public DashboardResponse.UpdateFollowUpStatusResponse UpdateFollowUpStatus(DashboardRequest.UpdateFollowUpStatusRequest RequestParam);
		public DashboardResponse.AddAndUpdatePatientNoteResponse AddAndUpdatePatientNote(DashboardRequest.AddAndUpdatePatientNoteRequest RequestParam);
		public DashboardResponse.DeletePatientNoteResponse DeletePatientNote(DashboardRequest.DeletePatientNoteRequest RequestParam);
		#endregion

		public bool RequestForPdf(CheckUserRequest RequestParam);
	}
}
