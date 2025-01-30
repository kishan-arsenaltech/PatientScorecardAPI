using Microsoft.Extensions.Configuration;
using Scorecard.API.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SelectPdf;
using QRCoder;
using System.Drawing;
using System.Net;
using System.Xml.Linq;

namespace Scorecard.API.DataAccess
{
	public class DashboardDataAccess : IDashboardDataAccess
	{
		protected string StoredProcedure = string.Empty;
		public IConfiguration _configuration { get; }
		public DashboardDataAccess(IConfiguration configuration) => _configuration = configuration;

		#region PAP Dashboard DataAccess
		public DashboardResponse.GetDashboardResult GetDashboardDetails(DashboardRequest.GetDashboardRequest RequestParam)
		{
			DashboardResponse.GetDashboardResult Response = new DashboardResponse.GetDashboardResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;
			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					#region GetSalesRepresentatives
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					StoredProcedure = "SP_Get_SalesRepresentatives";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiSalesRepresentativesResponse = StaticUtilities.DataTableToList<DashboardResponse.GetSalesRepresentativesResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion

					#region Get_PermissionModuleList
					ParamCollection.Add(new SqlParameter("RoleId", RequestParam.RoleId.HandleDBNull()));
					StoredProcedure = "SP_Get_PermissionModuleList";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiPermissionResponse = StaticUtilities.DataTableToList<DashboardResponse.PermissonRoleResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion

					#region GetInsuranceNames
					ParamCollection.Remove(ParamCollection[0]);
					ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.OrganizationParam.HandleDBNull()));
					StoredProcedure = "SP_Get_InsuranceNames";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiInsuranceNameResponse = StaticUtilities.DataTableToList<DashboardResponse.GetInsuranceNameResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion

					#region GetSalesOrderBreakDown
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
					StoredProcedure = "SP_Get_SalesOrderBreakDown";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiSalesOrderBreakDownResponse = StaticUtilities.DataTableToList<DashboardResponse.GetSalesOrderBreakDownResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion

					#region GetComplianceBreakDownUpdated
					ParamCollection.Add(new SqlParameter("Type", RequestParam.Type.HandleDBNull()));
					StoredProcedure = "SP_Get_ComplianceBreakDownUpdated";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiComplianceBreakDownResponse = StaticUtilities.DataTableToList<DashboardResponse.GetComplianceBreakDownResponse>(TableData);
						if (RequestParam.Type == "by_time_period")
						{
							Response.LiComplianceBreakDownResponse[0].AverageAdherence30 = Response.LiComplianceBreakDownResponse[0].AverageAdherence30 == 0 ? null : Response.LiComplianceBreakDownResponse[0].AverageAdherence30;

							Response.LiComplianceBreakDownResponse[0].AverageAdherence60 = Response.LiComplianceBreakDownResponse[0].AverageAdherence60 == 0 ? null : Response.LiComplianceBreakDownResponse[0].AverageAdherence60;

							Response.LiComplianceBreakDownResponse[0].AverageAdherence90 = Response.LiComplianceBreakDownResponse[0].AverageAdherence90 == 0 ? null : Response.LiComplianceBreakDownResponse[0].AverageAdherence90;

							Response.LiComplianceBreakDownResponse[0].Totals = null;
							Response.LiComplianceBreakDownResponse[0].Risk = null;
						}
						else
						{
							Response.LiComplianceBreakDownResponse[0].AverageAdherence30 = null;
							Response.LiComplianceBreakDownResponse[0].AverageAdherence60 = null;
							Response.LiComplianceBreakDownResponse[0].AverageAdherence90 = null;
						}
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion
					Response.Message = string.Empty;
					Response.Success = true;
					bool Sucess = DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Dashboard", "Page Load", SqlCommand);
				}

			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetAdherenceBreakdownResult GetAdherenceBreakdownDetail(DashboardRequest.GetDashboardRequest RequestParam)
		{
			DashboardResponse.GetAdherenceBreakdownResult Response = new DashboardResponse.GetAdherenceBreakdownResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.OrganizationParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Type", RequestParam.Type.HandleDBNull()));

					StoredProcedure = "SP_Get_ComplianceBreakDownUpdated";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiComplianceBreakDownResponse = StaticUtilities.DataTableToList<DashboardResponse.GetComplianceBreakDownResponse>(TableData);
						if (RequestParam.Type == "by_time_period")
						{
							Response.LiComplianceBreakDownResponse[0].AverageAdherence30 = Response.LiComplianceBreakDownResponse[0].AverageAdherence30 == 0 ? null : Response.LiComplianceBreakDownResponse[0].AverageAdherence30;

							Response.LiComplianceBreakDownResponse[0].AverageAdherence60 = Response.LiComplianceBreakDownResponse[0].AverageAdherence60 == 0 ? null : Response.LiComplianceBreakDownResponse[0].AverageAdherence60;

							Response.LiComplianceBreakDownResponse[0].AverageAdherence90 = Response.LiComplianceBreakDownResponse[0].AverageAdherence90 == 0 ? null : Response.LiComplianceBreakDownResponse[0].AverageAdherence90;

							Response.LiComplianceBreakDownResponse[0].Totals = null;
							Response.LiComplianceBreakDownResponse[0].Risk = null;
						}
						else
						{
							Response.LiComplianceBreakDownResponse[0].AverageAdherence30 = null;
							Response.LiComplianceBreakDownResponse[0].AverageAdherence60 = null;
							Response.LiComplianceBreakDownResponse[0].AverageAdherence90 = null;
						}
					}

					Response.Message = string.Empty;
					Response.Success = true;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetDeliveredAndComplaintChartResult GetDeliveredAndComplaintChartDetail(DashboardRequest.GetDeliveredAndComplaintChartRequest RequestParam)
		{
			DashboardResponse.GetDeliveredAndComplaintChartResult Response = new DashboardResponse.GetDeliveredAndComplaintChartResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.Organization.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralKey.HandleDBNull()));

					DataSet TableSet = Data.GetDataSet("SP_Get_DeliveredVSUndeliveredAndComplaintVSNonComplaintChart", ParamCollection);
					if (TableSet.Tables.Count > 0)
					{
						for (int i = 0; i < TableSet.Tables.Count; i++)
						{
							if (i == 0)
								TableData = TableSet.Tables[i].Copy();
							else
								TableData.Merge(TableSet.Tables[i]);
						}
						Response.LiDeliveredAndUndeliveredResponse = StaticUtilities.DataTableToList<DashboardResponse.GetDeliveredAndUndeliveredResponse>(TableData);
						Response.LiComplaintCountResponse = StaticUtilities.DataTableToList<DashboardResponse.GetComplaintCountResponse>(TableSet.Tables[2]);
						Response.Message = string.Empty;
						Response.Success = true;
					}
					else
					{
						Response.LiDeliveredAndUndeliveredResponse = null;
						Response.Message = "Data not found while fetching chart details.";
						Response.Success = false;
					}
				}
				else
				{
					Response.LiDeliveredAndUndeliveredResponse = null;
					Response.Message = "Access denied";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.LiDeliveredAndUndeliveredResponse = null;
				Response.Message = e.Message;
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetSalesOrderByFilterResult GetSalesOrderDetailsFilterList(DashboardRequest.GetSalesOrderByFilterRequest RequestParam)
		{
			DashboardResponse.GetSalesOrderByFilterResult Response = new DashboardResponse.GetSalesOrderByFilterResult();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					if (string.IsNullOrEmpty(RequestParam.VoidReason))
					{
						List<SqlParameter> ParamCollection = new List<SqlParameter>();
						ParamCollection.Add(new SqlParameter("StatusNameParam", RequestParam.StatusParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("SalesRepParam", RequestParam.SalesRepParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("DoctorsParam", RequestParam.DoctorParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("DoctorNameParam", RequestParam.DoctorNameParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("DoctorFirstNameParam", RequestParam.DoctorFirstNameParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("DoctorLastNameParam", RequestParam.DoctorLastNameParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("InsuranceParam", RequestParam.InsuranceParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("MaskTypeParam", RequestParam.MaskTypeParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("ComplianceRiskParam", RequestParam.ComplianceRiskParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("EquipmentTypeParam", RequestParam.EquipmentTypeParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("DaysSinceSetupParam", RequestParam.DaysSinceSetupParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.OrganizationParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("LimitParam", RequestParam.LimitParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("StartDate", RequestParam.StartDate.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("EndDate", RequestParam.EndDate.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("PatientNameParam", RequestParam.PatientNameParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("PatientFirstNameParam", RequestParam.PatientFirstNameParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("PatientLastNameParam", RequestParam.PatientNameLastParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("DateOfBirthParam", RequestParam.DateOfBirthParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("OffsetParam", RequestParam.RowsToSkip.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("OrderByFieldParam", RequestParam.OrderByFieldParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("SortOrderParam", RequestParam.SortOrderParam.HandleDBNull()));
						StoredProcedure = "SP_Get_SalesOrderByFilters";

						DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
						if (TableData.Rows.Count > 0)
						{
							Response.LiSalesOrderFilterResponse = StaticUtilities.DataTableToList<DashboardResponse.GetSalesOrderByFilterResponse>(TableData);
							string SalesOrderDetailsFilterList = TableData.Rows[0][0].ToString();
							Response.ListCount = Convert.ToInt32(SalesOrderDetailsFilterList);
							Response.Message = string.Empty;
							Response.Success = true;
						}
						else
						{
							Response.LiSalesOrderFilterResponse = null;
							Response.ListCount = 0;
							Response.Message = "Salesorder data not found for this referral.";
							Response.Success = false;
						}
					}
					else
					{
						List<SqlParameter> ParamCollection = new List<SqlParameter>();
						ParamCollection.Add(new SqlParameter("VoidReason", RequestParam.VoidReason.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.OrganizationParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("PatientNameParam", RequestParam.PatientNameParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("PatientFirstNameParam", RequestParam.PatientFirstNameParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("PatientLastNameParam", RequestParam.PatientNameLastParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("DateOfBirthParam", RequestParam.DateOfBirthParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("SortOrderParam", RequestParam.SortOrderParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("OrderByFieldParam", RequestParam.OrderByFieldParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("LimitParam", RequestParam.LimitParam.HandleDBNull()));
						ParamCollection.Add(new SqlParameter("OffsetParam", RequestParam.RowsToSkip.HandleDBNull()));
						StoredProcedure = "SP_Get_SalesOrderByVoidReason";

						DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
						if (TableData.Rows.Count > 0)
						{
							Response.LiSalesOrderByVoidReasonResponse = StaticUtilities.DataTableToList<DashboardResponse.GetSalesOrderByVoidReasonResponse>(TableData);
							string SalesOrderByVoidReasonList = TableData.Rows[0][0].ToString();
							Response.ListCount = Convert.ToInt32(SalesOrderByVoidReasonList);
							Response.Message = string.Empty;
							Response.Success = true;
						}
						else
						{
							Response.LiSalesOrderByVoidReasonResponse = null;
							Response.ListCount = 0;
							Response.Message = "Salesorder data not found for this referral.";
							Response.Success = false;
						}
					}
				}
				else
				{
					Response.LiSalesOrderFilterResponse = null;
					Response.ListCount = 0;
					Response.Message = "Access denied";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.LiSalesOrderFilterResponse = null;
				Response.ListCount = 0;
				Response.Message = e.Message;
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetDoctorListResult GetDoctorList(DashboardRequest.GetDoctorsRequest RequestParam)
		{
			DashboardResponse.GetDoctorListResult Response = new DashboardResponse.GetDoctorListResult();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("DoctorName", RequestParam.DoctorName.HandleDBNull()));
					StoredProcedure = "SP_Get_Doctors";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.ListDoctorResponse = StaticUtilities.DataTableToList<DashboardResponse.GetDoctorResponse>(TableData);
						Response.ListCount = TableData.Rows.Count;
						Response.Message = string.Empty;
						Response.Success = true;
					}
					else
					{
						Response.ListDoctorResponse = null;
						Response.ListCount = 0;
						Response.Message = "Data not found while fetching doctor list";
						Response.Success = false;
					}
				}
				else
				{
					Response.ListDoctorResponse = null;
					Response.ListCount = 0;
					Response.Message = "Access denied";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.ListDoctorResponse = null;
				Response.ListCount = 0;
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetAveragePatientUsageDataResult GetAveragePatientUsageData(DashboardRequest.GetAveragePatientUsageDataRequest RequestParam)
		{
			DashboardResponse.GetAveragePatientUsageDataResult Response = new DashboardResponse.GetAveragePatientUsageDataResult();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("ptKey", RequestParam.ptKey.HandleDBNull()));
					StoredProcedure = "SP_Get_AveragePatientUsageData";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiGetAveragePatientUsageData = StaticUtilities.DataTableToList<DashboardResponse.GetAveragePatientUsageDataResponse>(TableData);
						Response.Message = string.Empty;
						Response.Success = true;
					}
					else
					{
						Response.LiGetAveragePatientUsageData = null;
						Response.Message = "Data not found while fetching average patient usage data";
						Response.Success = false;
					}
				}
				else
				{
					Response.LiGetAveragePatientUsageData = null;
					Response.Message = "Access denied";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetReferralSearchResult GetReferralSearchList(DashboardRequest.GetReferralSearchRequest RequestParam)
		{
			DashboardResponse.GetReferralSearchResult Response = new DashboardResponse.GetReferralSearchResult();
			DataClasses Data = new DataClasses(_configuration);
			int i = 0;
			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.OrganizationParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("FacilityAddress", RequestParam.FacilityAddress.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("FacilityCity", RequestParam.FacilityCity.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("FacilityState", RequestParam.FacilityState.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("FacilityZip", RequestParam.FacilityZip.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("FacilityPhone", RequestParam.FacilityPhone.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("FacilityFax", RequestParam.FacilityFax.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("FacilityKey", RequestParam.FacilityKey.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("LimitParam", RequestParam.LimitParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("OffsetParam", RequestParam.OffsetParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("OrderByFieldParam", RequestParam.OrderByFieldParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("SortOrderParam", RequestParam.SortOrderParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("SearchParam", RequestParam.SearchParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("UserIdForReferralSearch", RequestParam.UserIdForReferralSearch.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Id", RequestParam.Id.HandleDBNull()));
					StoredProcedure = @"SP_Get_ReferralsByOrganization";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						if (RequestParam.RequestForGetUserDetails > 0)
						{

							//foreach (DataRow row in TableData.Rows)
							while (i < TableData.Rows.Count)
							{
								DataRow row = TableData.Rows[i];
								ParamCollection = new List<SqlParameter>();
								ParamCollection.Add(new SqlParameter("ReferralKey", row["ReferralKey"]));
								ParamCollection.Add(new SqlParameter("ReferralType", row["ReferralType"]));
								ParamCollection.Add(new SqlParameter("UserId", RequestParam.RequestForGetUserDetails.HandleDBNull()));
								StoredProcedure = @"SP_Check_ReferralMappingBasedOnType";
								int j = Data.JustExecuteScalar(StoredProcedure, ParamCollection);
								if (j == 0)
								{
									row.Delete();
									TableData.AcceptChanges();
								}
								i++;
							}
						}
						Response.LiGetReferralSearch = StaticUtilities.DataTableToList<DashboardResponse.GetReferralSearchResponse>(TableData);
						Response.Message = "";
						Response.Success = true;
					}
					else
					{
						Response.Message = "Data not found while fetching Referrals list.";
						Response.Success = false;
					}
				}
				else
				{
					Response.Message = "Access denied.";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public async Task<DashboardResponse.GetSummaryDocumentResponse> GetResmedSummaryDocument(DashboardRequest.GetSummaryDocumentRequest RequestParam)
		{
			DashboardResponse.GetSummaryDocumentResponse Response = new DashboardResponse.GetSummaryDocumentResponse();
			DateTime CurrentDate = DateTime.Now.AddDays(-RequestParam.Days).Date;
			string EndDate = CurrentDate.ToString("yyyy-MM-dd");
			string ApiUrl = string.Empty;
			string DocumentWithFolderPath = string.Format("{0}{1}.pdf", _configuration.GetSection("FolderPath:DocumentFolder").Value, Guid.NewGuid());

			bool IsPdfGenerated = false;
			try
			{
				ApiUrl = string.Format("https://api.resmed.com/airview/v1/patients/{0}/reports/compliance?periodType=USERDEFINED&endDate={1}&noOfDays={2}", RequestParam.PatientIdFlag, EndDate, RequestParam.Days);
				using (var httpClient = new HttpClient())
				{
					var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("QMES_INTEGRATOR:dH7R4HAnOhyf");
					string val = System.Convert.ToBase64String(plainTextBytes);
					httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

					using (var response = await httpClient.GetAsync(ApiUrl))
					{
						var ResponseData = response.Content.ReadAsStringAsync();
						if (ResponseData.Result.ToString().Contains("%PDF-"))
						{
							using (var file = File.Create(Directory.GetCurrentDirectory() + DocumentWithFolderPath))
							{
								var contentStream = await response.Content.ReadAsStreamAsync();
								var temo = response.Content.ReadAsStringAsync();
								await contentStream.CopyToAsync(file);
								IsPdfGenerated = true;
							}
						}
					}
				}

				if (IsPdfGenerated)
				{
					Response.DocumentPath = DocumentWithFolderPath;
					Response.Message = string.Empty;
					Response.Success = true;
				}
				else
				{
					Response.DocumentPath = string.Empty;
					Response.Message = "No document found.";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public async Task<DashboardResponse.GetSummaryDocumentResponse> GetPhillipSummaryDocument(DashboardRequest.GetSummaryDocumentRequest RequestParam)
		{
			string UrlToEncrypt = string.Empty;
			string TimeStamp = string.Empty;
			string PatientDetailApiUrl = string.Empty;
			string DocumentApiUrl = string.Empty;
			string EncodedPdfString = string.Empty;
			string IntegratorKey = "E1C45384-9258-45E8-80FF-F0C95FC4091A";
			string StartDate = DateTime.Now.AddDays(-RequestParam.Days).Date.ToString("yyyy-MM-dd");
			string EndDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
			string DocumentWithFolderPath = string.Format("{0}{1}.pdf", _configuration.GetSection("FolderPath:DocumentFolder").Value, Guid.NewGuid());
			DashboardResponse.GetSummaryDocumentResponse Response = new DashboardResponse.GetSummaryDocumentResponse();

			var KeyToBytes = Encoding.UTF8.GetBytes(IntegratorKey.ToLower());
			var EncryptedKey = new System.Security.Cryptography.HMACSHA256(KeyToBytes);

			UrlToEncrypt = "/PayerIntegrationExternal.v002/Patient";
			TimeStamp = string.Format("{0}{1}{2}{3}{4}{5}", DateTime.UtcNow.Year, DateTime.UtcNow.Month.ToString("D2"), DateTime.UtcNow.Day.ToString("D2"), DateTime.UtcNow.Hour.ToString("D2"), DateTime.UtcNow.Minute.ToString("D2"), DateTime.UtcNow.Second.ToString("D2"));
			var SignatureInBytes = Encoding.UTF8.GetBytes(string.Concat(UrlToEncrypt, TimeStamp));
			var Signature = Convert.ToBase64String(EncryptedKey.ComputeHash(SignatureInBytes));

			PatientDetailApiUrl = string.Format("https://INTEGRATIONURL.com/PayerIntegrationExternal.v002/Patient?PatientGuid={0}", RequestParam.PatientIdFlag);
			var HttpRequestForPatientDetail = new HttpClient();
			HttpRequestForPatientDetail.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "QMES:" + Signature.ToString());
			HttpRequestForPatientDetail.DefaultRequestHeaders.Add("Accept", "application/json");
			HttpRequestForPatientDetail.DefaultRequestHeaders.Add("Timestamp", TimeStamp);
			HttpRequestForPatientDetail.DefaultRequestHeaders.Add("Signature", Signature);

			var RequestMessageForPatientDetail = new HttpRequestMessage(HttpMethod.Get, new Uri(PatientDetailApiUrl));
			var ResponseOfRequest = await HttpRequestForPatientDetail.SendAsync(RequestMessageForPatientDetail);
			var PatientDetailResponse = ResponseOfRequest.Content.ReadAsStringAsync().Result;

			dynamic JsonResponse = new JObject();
			JsonResponse = JObject.Parse(PatientDetailResponse);

			if (JsonResponse.Result == "Success")
			{
				UrlToEncrypt = string.Format("/ReportIntegrationExternal.v001/Patient/{0}/Report/Therapy/Summary", JsonResponse.Token);
				TimeStamp = string.Format("{0}{1}{2}{3}{4}{5}", DateTime.UtcNow.Year, DateTime.UtcNow.Month.ToString("D2"), DateTime.UtcNow.Day.ToString("D2"), DateTime.UtcNow.Hour.ToString("D2"), DateTime.UtcNow.Minute.ToString("D2"), DateTime.UtcNow.Second.ToString("D2"));
				SignatureInBytes = Encoding.UTF8.GetBytes(String.Concat(UrlToEncrypt, TimeStamp));
				Signature = Convert.ToBase64String(EncryptedKey.ComputeHash(SignatureInBytes));

				DocumentApiUrl = string.Format("https://INTEGRATIONURL.com/ReportIntegrationExternal.v001/Patient/{0}/Report/Therapy/Summary?StartDate={1}&EndDate={2}", JsonResponse.Token, StartDate, EndDate);
				var HttpRequestForDocument = new HttpClient();
				HttpRequestForDocument.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "QMES:" + Signature.ToString());
				HttpRequestForDocument.DefaultRequestHeaders.Add("Timestamp", TimeStamp);
				HttpRequestForDocument.DefaultRequestHeaders.Add("Signature", Signature);
				HttpRequestForDocument.DefaultRequestHeaders.Add("Accept", "application/xhtml+xml");

				var RequestMessageForDocument = new HttpRequestMessage(HttpMethod.Get, new Uri(DocumentApiUrl));
				ResponseOfRequest = await HttpRequestForDocument.SendAsync(RequestMessageForDocument);
				PatientDetailResponse = ResponseOfRequest.Content.ReadAsStringAsync().Result;

				dynamic DocumentDataInXml = new XDocument();
				DocumentDataInXml = XDocument.Parse(PatientDetailResponse);

				if (DocumentDataInXml.ToString().Contains("<ResultCode>Success</ResultCode>"))
				{
					int BeginIndex = DocumentDataInXml.ToString().IndexOf("<ReportData>") + 12;
					EncodedPdfString = DocumentDataInXml.ToString().Substring(BeginIndex);
					int EndIndex = EncodedPdfString.ToString().IndexOf("</ReportData>");
					EncodedPdfString = EncodedPdfString.ToString().Substring(0, EndIndex);

					byte[] DecodedContentInBytes = Convert.FromBase64String(EncodedPdfString);
					File.WriteAllBytes(Directory.GetCurrentDirectory() + DocumentWithFolderPath, DecodedContentInBytes);
					Response.DocumentPath = string.Format("{0}?userid={1}&token={2}", DocumentWithFolderPath, RequestParam.UserId, RequestParam.AccessToken);
					Response.Success = true;
				}
				else if (DocumentDataInXml.ToString().Contains("<ResultCode>NoData</ResultCode>"))
				{
					Response.Message = "No report available";
					Response.Success = false;
				}
				else
				{
					Response.Message = PatientDetailResponse;
					Response.Success = false;
				}
			}
			else
			{
				Response.Message = JsonResponse.Result;
				Response.Success = false;
			}
			return Response;
		}
		#endregion

		public DashboardResponse.GetMaskTypeResult GetMaskTypeResult(DashboardRequest.GetMaskTypeRequest RequestParam)
		{
			DashboardResponse.GetMaskTypeResult Response = new DashboardResponse.GetMaskTypeResult();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.OrganizationParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
					StoredProcedure = @"SP_Get_MaskTypeList";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiMaskType = StaticUtilities.DataTableToList<DashboardResponse.GetMaskTypeResponse>(TableData);
						Response.Success = true;
					}
					else
					{
						Response.Message = "";
						Response.Success = false;
					}
				}
				else
				{
					Response.Message = "Access denied.";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		#region Non-PAP Dashboard DataAccess
		public DashboardResponse.GetNonPAPDashboardResult GetNonPAPDashboardDetails(DashboardRequest.GetNonPAPDashboardRequest RequestParam)
		{
			DashboardResponse.GetNonPAPDashboardResult Response = new DashboardResponse.GetNonPAPDashboardResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;
			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					#region GetSalesRepresentatives
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					StoredProcedure = "SP_Get_SalesRepresentatives";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiSalesRepresentativesResponse = StaticUtilities.DataTableToList<DashboardResponse.GetSalesRepresentativesResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion

					#region Get_PermissionModuleList
					ParamCollection.Add(new SqlParameter("RoleId", RequestParam.RoleId.HandleDBNull()));
					StoredProcedure = "SP_Get_PermissionModuleList";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiPermissionResponse = StaticUtilities.DataTableToList<DashboardResponse.PermissonRoleResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion

					#region GetInsuranceNames
					ParamCollection.Remove(ParamCollection[0]);
					ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.OrganizationParam.HandleDBNull()));
					StoredProcedure = "SP_Get_InsuranceNames";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiInsuranceNameResponse = StaticUtilities.DataTableToList<DashboardResponse.GetInsuranceNameResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion

					#region GetNonPAPSalesOrderBreakDown
					ParamCollection.Remove(ParamCollection[0]);
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
					StoredProcedure = "SP_Get_NonPAPSalesOrderBreakDown";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiNonPAPSalesOrderBreakDownResponse = StaticUtilities.DataTableToList<DashboardResponse.GetNonPAPSalesOrderBreakDownResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					#endregion

					Response.Message = string.Empty;
					Response.Success = true;
					bool Sucess = DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Dashboard", "Page Load", SqlCommand);
				}

			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetNonPAPSalesOrderByStatusResult GetNonPAPSalesOrderDetailsListByStatus(DashboardRequest.GetNonPAPSalesOrderByStatusRequest RequestParam)
		{
			DashboardResponse.GetNonPAPSalesOrderByStatusResult Response = new DashboardResponse.GetNonPAPSalesOrderByStatusResult();
			DataClasses Data = new DataClasses(_configuration);
			if (RequestParam.IsStatusNameParam == false)
			{
				RequestParam.StatusNameParam = "Shipped,In Process,In Process - Backorder,In Process - Scheduled Order,On Hold," +
					"Order Cancelled - Authorization Not Approved,Order Cancelled - Canceled per Doc Policy,Order Cancelled - Eligibility Issue," +
					"Order Cancelled - Insufficient Medical Documents,Order Cancelled - Insufficient Patient Documents," +
					"Order Cancelled - No Response from Sales Rep,Order Cancelled - Non-Covered Items,Order Cancelled - Non-Participating Payer," +
					"Order Cancelled - Order Cancelled Before Shipment,Order Cancelled - Order Created in Error," +
					"Order Cancelled - Order Created Previously,Order Cancelled - Outsourced Order,Order Cancelled - ParachuteHealth," +
					"Order Cancelled - Patient - OOP Cost,Order Cancelled - Patient Cancelled Order," +
					"Order Cancelled - Patient Deceased,Order Cancelled - Patient Never Responded,Order Cancelled - Referral Cancelled Order," +
					"Order Cancelled - Reorder too soon (Order Exceeds 14 Days),Order Cancelled - RRB Created in Error," +
					"Order Cancelled - Wound Care Rekey,Pending - Authorization Needed,Pending - Documentation Needed," +
					"Pending - Information Needed,Pending - Authorization,Pending - Authorization Needed,Pending - Documentation Needed," +
					"Pending - Information Needed,Pending,Order Cancelled";
			}
			RequestParam.DateRange = 300;
			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("StatusNameParam", RequestParam.StatusNameParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("DateRange", RequestParam.DateRange.HandleDBNull()));
					StoredProcedure = "SP_Get_NonPAPSalesOrderDataByStatus";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiNonPAPSalesOrderByStatusResponse = StaticUtilities.DataTableToList<DashboardResponse.GetNonPAPSalesOrderByStatusResponse>(TableData);
						Response.ListCount = TableData.Rows.Count;
						Response.Message = string.Empty;
						Response.Success = true;
					}
					else
					{
						Response.LiNonPAPSalesOrderByStatusResponse = null;
						Response.ListCount = 0;
						Response.Message = "Non-PAP Salesorder data not found by status for this referral.";
						Response.Success = false;
					}
				}
				else
				{
					Response.Message = "Access denied";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{

				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetNonPAPDeliveredAndUnDeliveredChartResult GetNonPAPDeliveredAndUnDeliveredChartDetail(DashboardRequest.GetNonPAPDeliveredAndUnDeliveredChartRequest RequestParam)
		{
			DashboardResponse.GetNonPAPDeliveredAndUnDeliveredChartResult Response = new DashboardResponse.GetNonPAPDeliveredAndUnDeliveredChartResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.Organization.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralKey.HandleDBNull()));
					StoredProcedure = "SP_Get_NonPAPDeliveredAndUnDeliveredChart";

					DataSet TableSet = Data.GetDataSet(StoredProcedure, ParamCollection);
					if (TableSet.Tables.Count > 0)
					{
						for (int i = 0; i < TableSet.Tables.Count; i++)
						{
							if (i == 0)
								TableData = TableSet.Tables[i].Copy();
							else
								TableData.Merge(TableSet.Tables[i]);
						}
						Response.LiNonPAPDeliveredAndUndeliveredResponse = StaticUtilities.DataTableToList<DashboardResponse.GetNonPAPDeliveredAndUndeliveredResponse>(TableData);
						Response.Message = string.Empty;
						Response.Success = true;
					}
					else
					{
						Response.LiNonPAPDeliveredAndUndeliveredResponse = null;
						Response.Message = "Data not found while fetching chart data.";
						Response.Success = false;
					}
				}
				else
				{
					Response.Message = "Access denied";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}

			return Response;
		}

		public DashboardResponse.GetNonPAPSalesOrderByFiltersResult GetNonPAPSalesOrderDetailsListByFilters(DashboardRequest.GetNonPAPSalesOrderByFiltersRequest RequestParam)
		{
			DashboardResponse.GetNonPAPSalesOrderByFiltersResult Response = new DashboardResponse.GetNonPAPSalesOrderByFiltersResult();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("StatusNameParam", RequestParam.StatusParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("DoctorsParam", RequestParam.DoctorParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("DoctorNameParam", RequestParam.DoctorNameParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("DoctorFirstNameParam", RequestParam.DoctorFirstNameParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("DoctorLastNameParam", RequestParam.DoctorLastNameParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("InsuranceParam", RequestParam.InsuranceParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ComplianceRiskParam", RequestParam.ComplianceRiskParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("EquipmentTypeParam", RequestParam.EquipmentTypeParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("OrganizationParam", RequestParam.OrganizationParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("StartDate", RequestParam.StartDate.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("EndDate", RequestParam.EndDate.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("PatientNameParam", RequestParam.PatientNameParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("PatientFirstNameParam", RequestParam.PatientFirstNameParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("PatientLastNameParam", RequestParam.PatientNameLastParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("DateOfBirthParam", RequestParam.DateOfBirthParam.HandleDBNull()));
					StoredProcedure = "SP_Get_NonPAPSalesOrderDataByFilters";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiNonPAPSalesOrderByFiltersResponse = StaticUtilities.DataTableToList<DashboardResponse.GetNonPAPSalesOrderByFiltersResponse>(TableData);
						Response.ListCount = TableData.Rows.Count;
						Response.Message = string.Empty;
						Response.Success = true;
					}
					else
					{
						Response.LiNonPAPSalesOrderByFiltersResponse = null;
						Response.ListCount = 0;
						Response.Message = "Salesorder data not found for this referral.";
						Response.Success = false;
					}
				}
				else
				{
					Response.Message = "Access denied";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetNonPAPPatientDataResult GetNonPAPPatientDetail(DashboardRequest.GetNonPAPPatientDataRequest RequestParam)
		{
			DashboardResponse.GetNonPAPPatientDataResult Response = new DashboardResponse.GetNonPAPPatientDataResult();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("PatientNameParam", RequestParam.PatientNameParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("ReferralKey", RequestParam.ReferralKey.HandleDBNull()));
					StoredProcedure = "SP_Get_NonPAPPatientData";

					DataTable TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiNonPAPPatientDataResponse = StaticUtilities.DataTableToList<DashboardResponse.GetNonPAPPatientDataResponse>(TableData);
						DashboardResponse.GetNonPAPPatientDataResponse ExtraOrder = new DashboardResponse.GetNonPAPPatientDataResponse();
						ExtraOrder.OrderNumber = 1;
						Response.LiNonPAPPatientDataResponse.Add(ExtraOrder);

						ExtraOrder = new DashboardResponse.GetNonPAPPatientDataResponse();
						ExtraOrder.OrderNumber = 2;
						Response.LiNonPAPPatientDataResponse.Add(ExtraOrder);

						Response.Message = string.Empty;
						Response.Success = true;

						if (Response.LiNonPAPPatientDataResponse.Count > 0)
						{
							ParamCollection = new List<SqlParameter>();
							ParamCollection.Add(new SqlParameter("UserId", RequestParam.UserId));
							ParamCollection.Add(new SqlParameter("PatientName", RequestParam.PatientNameParam));

							StoredProcedure = "SP_Get_CancelledAndOutsourcedOrderDetails";

							DataSet TableSet = Data.GetDataSet(StoredProcedure, ParamCollection);

							if (TableSet.Tables.Count == 3)
							{
								Response.LiCancelledOrderResponse = TableSet.Tables[0].Rows.Count > 0 ? StaticUtilities.DataTableToList<DashboardResponse.GetCancelledOrderResponse>(TableSet.Tables[0]) : null;

								Response.LiOutsourcedOrderResponse = TableSet.Tables[1].Rows.Count > 0 ? StaticUtilities.DataTableToList<DashboardResponse.GetOutsourcedOrderResponse>(TableSet.Tables[1]) : Response.LiOutsourcedOrderResponse;

								Response.IsInternalRole = TableSet.Tables[2].Rows.Count > 0 ? Convert.ToBoolean(TableSet.Tables[2].Rows[0]["IsInternalRole"]) : false;
							}

							ParamCollection = new List<SqlParameter>();
							ParamCollection.Add(new SqlParameter("Nickname", Response.LiNonPAPPatientDataResponse[0].NickName));
							ParamCollection.Add(new SqlParameter("PatientName", RequestParam.PatientNameParam));

							StoredProcedure = "SP_GET_PatientNotes";

							TableData = new DataTable();
							TableData = Data.GetDataTable(StoredProcedure, ParamCollection);

							Response.LiPatientNotesDetailResponse = TableData.Rows.Count > 0 ? StaticUtilities.DataTableToList<DashboardResponse.GetPatientNotesDetail>(TableData) : null;
						}
					}
					else
					{
						Response.LiNonPAPPatientDataResponse = new List<DashboardResponse.GetNonPAPPatientDataResponse>();
						Response.Message = "Patient information not found for this patient.";
						Response.Success = false;
					}
				}
				else
				{
					Response.Message = "Access denied";
					Response.Success = false;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}
		#endregion

		#region Screened Patient    
		public DashboardResponse.CheckSleepScreenerAcessResult CheckForMenuItemAccess(DashboardRequest.CheckSleepScreenerAccessRequest RequestParam)
		{
			DashboardResponse.CheckSleepScreenerAcessResult Response = new DashboardResponse.CheckSleepScreenerAcessResult();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			DataTable TableData = new DataTable();
			string SqlCommand = string.Empty;

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("UserId", RequestParam.UserId.HandleDBNull()));
					StoredProcedure = "SP_Check_SleepScreenerAccess";

					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.SleepScreenerAccess = Convert.ToInt32(TableData.Rows[0][0]);
						Response.GlobalSummaryAccess = Convert.ToInt32(TableData.Rows[0][1]);
					}

					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

					Response.Message = string.Empty;
					Response.Success = true;
					bool Sucess = DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Screened Patient", "Check Sleep Screener Access", SqlCommand);
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetSalesRepresentativesResult GetSalesRepresentatives(DashboardRequest.GetSalesRepresentativesRequest RequestParam)
		{
			DashboardResponse.GetSalesRepresentativesResult Response = new DashboardResponse.GetSalesRepresentativesResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;
			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					StoredProcedure = "SP_Get_SalesRepresentatives";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiSalesRepresentativesResponse = StaticUtilities.DataTableToList<DashboardResponse.GetSalesRepresentativesResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

					Response.Message = string.Empty;
					Response.Success = true;
					bool Sucess = DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Sales Representatives", "List", SqlCommand);
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetScreenedPatientListResult GetScreenedPatientList(DashboardRequest.GetScreenedPatientListRequest RequestParam)
		{
			DashboardResponse.GetScreenedPatientListResult Response = new DashboardResponse.GetScreenedPatientListResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;
			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("ReferralParam", RequestParam.ReferralParam.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("Organization", RequestParam.Organization.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("PatientFirstName", RequestParam.PatientFirstName.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("PatientLastName", RequestParam.PatientLastName.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("SalesRep", RequestParam.SalesRep.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("DateOfBirth", RequestParam.DateOfBirth.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("FollowUpStatus", RequestParam.FollowUpStatus.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("IsAdmin", RequestParam.IsAdmin.HandleDBNull()));
					StoredProcedure = "SP_Get_ScreenedPatientList";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiScreenedPatientListResponse = StaticUtilities.DataTableToList<DashboardResponse.GetScreenedPatientListResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

					Response.Message = string.Empty;
					Response.Success = true;
					bool Sucess = DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Screened Patient", "List", SqlCommand);
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetScreenerDataResult GetScreenedData(DashboardRequest.GetScreenerDataRequest RequestParam)
		{
			DashboardResponse.GetScreenerDataResult Response = new DashboardResponse.GetScreenerDataResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;
			string DocumentWithFolderPath = string.Empty;

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("PtKey", RequestParam.PtKey.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("NickName", RequestParam.NickName.HandleDBNull()));
					StoredProcedure = "SP_Get_ScreenerData";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiScreenerDataResponse = StaticUtilities.DataTableToList<DashboardResponse.GetScreenerDataResponse>(TableData);
					}


					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);
					if (RequestParam.type == "pdf")
					{
						StreamReader ObjReader;
						ObjReader = new StreamReader(string.Format("{0}\\Content\\EmailTemplate\\SleepScreenerTemplateForPdf.html", Directory.GetCurrentDirectory()));
						string EmailContent = ObjReader.ReadToEnd();
						string ImageData = string.Empty;

						ImageData = Convert.ToBase64String(File.ReadAllBytes(string.Format("{0}\\Content\\Logo_Image\\SleepScreener_logo.png", Directory.GetCurrentDirectory())));
						ImageData = "data:image/png;base64," + ImageData;
						EmailContent = EmailContent.Replace("{screenerlogoimagedata}", ImageData);

						ImageData = Convert.ToBase64String(File.ReadAllBytes(string.Format("{0}\\Content\\Logo_Image\\graph-popup.png", Directory.GetCurrentDirectory())));
						ImageData = "data:image/png;base64," + ImageData;
						EmailContent = EmailContent.Replace("{graphimagedata}", ImageData);

						if (TableData.Rows.Count > 0)
						{
							Response.LiScreenerDataResponse = null;
							string ExistingConditions = TableData.Rows[0]["ExistingConditions"].ToString();
							string PatientName = TableData.Rows[0]["PatientName"].ToString();
							string DOB = TableData.Rows[0]["DOB"].ToString();
							string Risk = TableData.Rows[0]["OSARisk"].ToString();

							int Diabetes = Convert.ToInt32(TableData.Rows[0]["Diabetes"]);
							int Hypertension = Convert.ToInt32(TableData.Rows[0]["Hypertension"]);
							int GeneralFatigue = Convert.ToInt32(TableData.Rows[0]["GeneralFatigue"]);
							int Obesity = Convert.ToInt32(TableData.Rows[0]["Obesity"]);
							int CongestiveHeartFailure = Convert.ToInt32(TableData.Rows[0]["CongestiveHeartFailure"]);
							int Pacemakers = Convert.ToInt32(TableData.Rows[0]["Pacemakers"]);
							int AtrialFibrillation = Convert.ToInt32(TableData.Rows[0]["AtrialFibrillation"]);
							int CoronaryArteryDisease = Convert.ToInt32(TableData.Rows[0]["CoronaryArteryDisease"]);
							int Stroke = Convert.ToInt32(TableData.Rows[0]["Stroke"]);
							int SevereBackPain = Convert.ToInt32(TableData.Rows[0]["SevereBackPain"]);
							int JointPain = Convert.ToInt32(TableData.Rows[0]["JointPain"]);
							int HighCholesterol = Convert.ToInt32(TableData.Rows[0]["HighCholesterol"]);

							int Snoring = Convert.ToInt32(TableData.Rows[0]["Snoring"]);
							int Tired = Convert.ToInt32(TableData.Rows[0]["Tired"]);
							int Observed = Convert.ToInt32(TableData.Rows[0]["Observed"]);
							int Pressure = Convert.ToInt32(TableData.Rows[0]["Pressure"]);
							int BodyMassIndex = Convert.ToInt32(TableData.Rows[0]["BodyMass"]);
							int AgeOlder = Convert.ToInt32(TableData.Rows[0]["AgeOlder"]);
							int NeckSize = Convert.ToInt32(TableData.Rows[0]["NeckSize"]);
							int GenderMale = Convert.ToInt32(TableData.Rows[0]["GenderMale"]);

							DocumentWithFolderPath = string.Format("{0}SleepScreenerResults For {1}.pdf", _configuration.GetSection("FolderPath:PdfFolder").Value, PatientName);

							EmailContent = !string.IsNullOrEmpty(PatientName) ? EmailContent.Replace("{Patient Name}", " " + PatientName) : EmailContent.Replace("{Patient Name}", string.Empty);
							EmailContent = !string.IsNullOrEmpty(DOB) ? EmailContent.Replace("{DOB}", " " + DOB) : EmailContent.Replace("{DOB}", string.Empty);
							EmailContent = !string.IsNullOrEmpty(Risk) ? EmailContent.Replace("{Risk}", " " + Risk) : EmailContent.Replace("{Risk}", string.Empty);

							EmailContent = EmailContent.Replace("{apiurl}", _configuration.GetSection("SiteSettings:ScorecardApiUrl").Value);

							if (!string.IsNullOrEmpty(ExistingConditions))
							{
								EmailContent = Diabetes == 1 ? EmailContent.Replace("{chkDiabetes}", "checked") : EmailContent.Replace("{chkDiabetes}", "");
								EmailContent = Hypertension == 1 ? EmailContent.Replace("{chkHypertension}", "checked") : EmailContent.Replace("{chkHypertension}", "");
								EmailContent = GeneralFatigue == 1 ? EmailContent.Replace("{chkGeneralFatigue}", "checked") : EmailContent.Replace("{chkGeneralFatigue}", "");
								EmailContent = Obesity == 1 ? EmailContent.Replace("{chkObesity}", "checked") : EmailContent.Replace("{chkObesity}", "");
								EmailContent = CongestiveHeartFailure == 1 ? EmailContent.Replace("{chkCongestiveHeartFailure}", "checked") : EmailContent.Replace("{chkCongestiveHeartFailure}", "");
								EmailContent = Pacemakers == 1 ? EmailContent.Replace("{chkPacemakers}", "checked") : EmailContent.Replace("{chkPacemakers}", "");
								EmailContent = AtrialFibrillation == 1 ? EmailContent.Replace("{chkAtrialFibrillation}", "checked") : EmailContent.Replace("{chkAtrialFibrillation}", "");
								EmailContent = CoronaryArteryDisease == 1 ? EmailContent.Replace("{chkCoronaryArteryDisease}", "checked") : EmailContent.Replace("{chkCoronaryArteryDisease}", "");
								EmailContent = Stroke == 1 ? EmailContent.Replace("{chkStroke}", "checked") : EmailContent.Replace("{chkStroke}", "");
								EmailContent = SevereBackPain == 1 ? EmailContent.Replace("{chkSevereBackPain}", "checked") : EmailContent.Replace("{chkSevereBackPain}", "");
								EmailContent = JointPain == 1 ? EmailContent.Replace("{chkJointPain}", "checked") : EmailContent.Replace("{chkJointPain}", "");
								EmailContent = HighCholesterol == 1 ? EmailContent.Replace("{chkHighCholesterol}", "checked") : EmailContent.Replace("{chkHighCholesterol}", "");


								EmailContent = Snoring == 1 ? EmailContent.Replace("checked={snoreLoudlyYes}", "checked").Replace("checked={snoreLoudlyNo}", "") : EmailContent.Replace("checked={snoreLoudlyYes}", "").Replace("checked={snoreLoudlyNo}", "checked");
								EmailContent = Tired == 1 ? EmailContent.Replace("checked={feelTiredYes}", "checked").Replace("checked={feelTiredNo}", "") : EmailContent.Replace("checked={feelTiredYes}", "").Replace("checked={feelTiredNo}", "checked");
								EmailContent = Observed == 1 ? EmailContent.Replace("checked={stopBreathingYes}", "checked").Replace("checked={stopBreathingNo}", "") : EmailContent.Replace("checked={stopBreathingYes}", "").Replace("checked={stopBreathingNo}", "checked");
								EmailContent = Pressure == 1 ? EmailContent.Replace("checked={highBloodPressureYes}", "checked").Replace("checked={highBloodPressureNo}", "") : EmailContent.Replace("checked={highBloodPressureYes}", "").Replace("checked={highBloodPressureNo}", "checked");
								EmailContent = BodyMassIndex == 1 ? EmailContent.Replace("checked={bodyMassIndexYes}", "checked").Replace("checked={bodyMassIndexNo}", "") : EmailContent.Replace("checked={bodyMassIndexYes}", "").Replace("checked={bodyMassIndexNo}", "checked");
								EmailContent = AgeOlder == 1 ? EmailContent.Replace("checked={OlderYes}", "checked").Replace("checked={OlderNo}", "") : EmailContent.Replace("checked={OlderYes}", "").Replace("checked={OlderNo}", "checked");
								EmailContent = NeckSize == 1 ? EmailContent.Replace("checked={neckSizeLargeYes}", "checked").Replace("checked={neckSizeLargeNo}", "") : EmailContent.Replace("checked={neckSizeLargeYes}", "").Replace("checked={neckSizeLargeNo}", "checked");
								EmailContent = GenderMale == 1 ? EmailContent.Replace("checked={genderYes}", "checked").Replace("checked={genderNo}", "") : EmailContent.Replace("checked={genderYes}", "").Replace("checked={genderNo}", "checked");
							}
						}

						HtmlToPdf ObjHtmlToPdf = new HtmlToPdf();
						//ObjHtmlToPdf.Options.EmbedFonts = true;

						PdfDocument ObjPdfDocument = ObjHtmlToPdf.ConvertHtmlString(EmailContent);
						byte[] ByteArrayForPdf = ObjPdfDocument.Save();
						ObjPdfDocument.Close();

						File.WriteAllBytes(string.Format("{0}{1}", Directory.GetCurrentDirectory(), DocumentWithFolderPath), ByteArrayForPdf);

						Response.Message = File.Exists(string.Format("{0}{1}", Directory.GetCurrentDirectory(), DocumentWithFolderPath)) ? string.Format("{0}?userid={1}&token={2}",
					DocumentWithFolderPath, RequestParam.UserId, RequestParam.AccessToken) : "No document found.";
						Response.LiScreenerDataResponse = null;
						Response.Success = true;

						DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Screener Result", "Pdf for sleep screener result", SqlCommand);
					}
					else
					{
						Response.Message = string.Empty;
						Response.Success = true;

						DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Screener Result", "Screener Data", SqlCommand);
					}
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetFlayerForSleepScreenerResult GetFlayerForSleepScreener(DashboardRequest.GetFlayerForSleepScreenerRequest RequestParam)
		{
			DashboardResponse.GetFlayerForSleepScreenerResult Response = new DashboardResponse.GetFlayerForSleepScreenerResult();
			DataClasses Data = new DataClasses(_configuration);

			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					string FacilityName = RequestParam.FacilityName;
					string ScreenerLink = RequestParam.ScreenerLink;
					string ImageData = string.Empty;

					string DocumentWithFolderPath = string.Format("{0}Flyer for {1}.pdf", _configuration.GetSection("FolderPath:PdfFolder").Value, FacilityName);

					StreamReader ObjReader;
					ObjReader = new StreamReader(string.Format("{0}\\Content\\EmailTemplate\\StopBangTemplateForPdf.html", Directory.GetCurrentDirectory()));
					string EmailContent = ObjReader.ReadToEnd();

					ImageData = Convert.ToBase64String(File.ReadAllBytes(string.Format("{0}\\Content\\Logo_Image\\AH-SleepScreener.png", Directory.GetCurrentDirectory())));
					ImageData = "data:image/png;base64," + ImageData;
					EmailContent = EmailContent.Replace("{headingimagedata}", ImageData);

					QRCodeGenerator ObjQRCodeGen = new QRCodeGenerator();
					QRCodeData ObjQRCodeData = ObjQRCodeGen.CreateQrCode(ScreenerLink, QRCodeGenerator.ECCLevel.Q);
					QRCode ObjQRCode = new QRCode(ObjQRCodeData);
					Bitmap CodeImage = ObjQRCode.GetGraphic(40);
					Image image = (Image)CodeImage;
					File.Delete(string.Format("{0}\\Content\\Logo_Image\\qrcode.png", Directory.GetCurrentDirectory()));
					CodeImage.Save(string.Format("{0}\\Content\\Logo_Image\\qrcode.png", Directory.GetCurrentDirectory()));

					ImageData = Convert.ToBase64String(File.ReadAllBytes(string.Format("{0}\\Content\\Logo_Image\\qrcode.png", Directory.GetCurrentDirectory())));
					ImageData = "data:image/png;base64," + ImageData;
					EmailContent = EmailContent.Replace("{qrcodeimagedata}", ImageData);

					HtmlToPdf ObjHtmlToPdf = new HtmlToPdf();
					PdfDocument ObjPdfDocument = ObjHtmlToPdf.ConvertHtmlString(EmailContent);
					byte[] ByteArrayForPdf = ObjPdfDocument.Save();
					ObjPdfDocument.Close();

					File.WriteAllBytes(string.Format("{0}{1}", Directory.GetCurrentDirectory(), DocumentWithFolderPath), ByteArrayForPdf);

					Response.Message = File.Exists(string.Format("{0}{1}", Directory.GetCurrentDirectory(), DocumentWithFolderPath)) ? string.Format("{0}?userid={1}&token={2}",
					DocumentWithFolderPath, RequestParam.UserId, RequestParam.AccessToken) : "No document found.";
					Response.Success = true;
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.GetPatientNotesResult GetPatientNoteList(DashboardRequest.GetPatientNotesRequest RequestParam)
		{
			DashboardResponse.GetPatientNotesResult Response = new DashboardResponse.GetPatientNotesResult();
			DataClasses Data = new DataClasses(_configuration);
			DataTable TableData = new DataTable();
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);
			string SqlCommand = string.Empty;
			try
			{
				if (Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken))
				{
					List<SqlParameter> ParamCollection = new List<SqlParameter>();
					ParamCollection.Add(new SqlParameter("PtKey", RequestParam.PtKey.HandleDBNull()));
					ParamCollection.Add(new SqlParameter("NickName", RequestParam.NickName.HandleDBNull()));
					StoredProcedure = "SP_Get_PatientNotesList";
					TableData = Data.GetDataTable(StoredProcedure, ParamCollection);
					if (TableData.Rows.Count > 0)
					{
						Response.LiPatientNotesResponse = StaticUtilities.DataTableToList<DashboardResponse.GetPatientNotesResponse>(TableData);
					}
					SqlCommand += Data.BuildSqlCommand(ParamCollection, StoredProcedure);

					Response.Message = string.Empty;
					Response.Success = true;
					bool Sucess = DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Screened Patient", "Page Load", SqlCommand);
				}
			}
			catch (Exception e)
			{
				Response.Message = e.Message;
				Response.Success = false;
			}
			return Response;
		}

		public DashboardResponse.UpdateFollowUpStatusResponse UpdateFollowUpStatus(DashboardRequest.UpdateFollowUpStatusRequest RequestParam)
		{
			DashboardResponse.UpdateFollowUpStatusResponse Response = new DashboardResponse.UpdateFollowUpStatusResponse();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);

			try
			{
				List<SqlParameter> ParamCollection = new List<SqlParameter>();
				ParamCollection.Add(new SqlParameter("PtKey", RequestParam.PtKey.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("NickName", RequestParam.NickName.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("FollowUpStatus", RequestParam.FollowUpStatus.HandleDBNull()));
				StoredProcedure = "SP_Update_FollowUpStatus";

				int Result = Data.JustExecute(StoredProcedure, ParamCollection);
				if (Result > 0)
				{
					DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Follow-Up Status", "Update Follow-Up Status", Data.BuildSqlCommand(ParamCollection, StoredProcedure));

					Response.Message = "Follow-Up Status updated successfully.";
					Response.Success = true;
				}
				else
				{
					Response.Success = false;
					Response.Message = "Something went wrong while updating Follow-Up Status.";
				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}

			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

			string strBaseURL = _configuration.GetSection("SiteSettings:PatientNoteUrl").Value;
			string strMethod = "Location/CreatePatientNote";

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

			string strFullURL = string.Format("{0}", strBaseURL);

			try
			{

				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(strFullURL);
					var responseTask = client.GetAsync(string.Format("{0}?NickName={1}&PtKey={2}&FollowUpStatus={3}", strMethod, RequestParam.NickName, RequestParam.PtKey, RequestParam.FollowUpStatus));
					responseTask.Wait();

					var result = responseTask.Result;
				}
			}
			catch (WebException ex)
			{
				throw;
			}

			return Response;
		}

		public DashboardResponse.AddAndUpdatePatientNoteResponse AddAndUpdatePatientNote(DashboardRequest.AddAndUpdatePatientNoteRequest RequestParam)
		{
			DashboardResponse.AddAndUpdatePatientNoteResponse Response = new DashboardResponse.AddAndUpdatePatientNoteResponse();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);

			try
			{
				List<SqlParameter> ParamCollection = new List<SqlParameter>();
				ParamCollection.Add(new SqlParameter("StatementType", RequestParam.StatementType.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("PatientNotesId", RequestParam.PatientNotesId.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("PtKey", RequestParam.PtKey.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("NickName", RequestParam.NickName.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("CreatedBy", RequestParam.CreatedBy.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("Description", RequestParam.Description.HandleDBNull()));
				StoredProcedure = "SP_InsertAndUpdate_PatientNote";

				int Result = Data.JustExecute(StoredProcedure, ParamCollection);
				if (Result > 0)
				{
					DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, RequestParam.StatementType.ToLower() == "insert" ? "Add" : "Update" + " Patient Note", RequestParam.StatementType.ToLower() == "insert" ? "Add" : "Update" + " Patient Note", Data.BuildSqlCommand(ParamCollection, StoredProcedure));

					Response.Message = string.Format("Patient note {0}ed successfully.", RequestParam.StatementType.ToLower() == "update" ? "updat" : RequestParam.StatementType);
					Response.Success = true;
				}
				else
				{
					Response.Success = false;
					Response.Message = string.Format("Something went wrong while {0}ing patient note.", RequestParam.StatementType.ToLower() == "update" ? "updat" : RequestParam.StatementType);
				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}

		public DashboardResponse.DeletePatientNoteResponse DeletePatientNote(DashboardRequest.DeletePatientNoteRequest RequestParam)
		{
			DashboardResponse.DeletePatientNoteResponse Response = new DashboardResponse.DeletePatientNoteResponse();
			DataClasses Data = new DataClasses(_configuration);
			TransactionLogDataAccess DataAccess = new TransactionLogDataAccess(_configuration);

			try
			{
				List<SqlParameter> ParamCollection = new List<SqlParameter>();
				ParamCollection.Add(new SqlParameter("PatientNotesId", RequestParam.PatientNotesId.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("PtKey", RequestParam.PtKey.HandleDBNull()));
				ParamCollection.Add(new SqlParameter("NickName", RequestParam.NickName.HandleDBNull()));
				StoredProcedure = "SP_Delete_PatientNote";

				int Result = Data.JustExecute(StoredProcedure, ParamCollection);
				if (Result > 0)
				{
					DataAccess.AddTransactionLog(RequestParam.UserId, RequestParam.UserAgent, "Patient Note", "Delete Patient Note", Data.BuildSqlCommand(ParamCollection, StoredProcedure));

					Response.Message = "Patient note deleted successfully.";
					Response.Success = true;
				}
				else
				{
					Response.Success = false;
					Response.Message = "Something went wrong while deleting patient note.";
				}
			}
			catch (Exception e)
			{
				Response.Success = false;
				Response.Message = e.Message;
			}
			return Response;
		}
		#endregion

		public bool RequestForPdf(CheckUserRequest RequestParam)
		{
			DataClasses Data = new DataClasses(_configuration);
			return Data.IsValidRequest(RequestParam.UserId, RequestParam.AccessToken);
		}
	}
}
