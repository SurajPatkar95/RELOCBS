using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Enquiry;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace RELOCBS.BL.Enquiry
{
	public class EnquiryBL
	{
		private EnquiryDL _enquiryDAL;

		public EnquiryDL enquiryDAL
		{

			get
			{
				if (this._enquiryDAL == null)
					this._enquiryDAL = new EnquiryDL();
				return this._enquiryDAL;
			}
		}

		public bool Insert(Entities.Enquiry enquiry, out string result)
		{
			try
			{
				return enquiryDAL.Insert(enquiry, out result);
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

		}

		public Entities.Enquiry GetDetailById(int EnqDetailID, out IEnumerable<Entities.EnquiryDetail> empList, out DataTable dtenquiry, int EnquiryID = -1)
		{
			Entities.Enquiry surveyObj = new Entities.Enquiry();
			empList = null;
			try
			{
				DataSet enquiryDt = enquiryDAL.GetDetailById(EnqDetailID, EnquiryID);
				dtenquiry = enquiryDt.Tables[0];
				//Session[ReportSession]
				surveyObj.EnqDate = System.DateTime.Now.Date;
				surveyObj.CompareTentEnqDate = System.DateTime.Now.AddDays(-1);
				surveyObj.CompareContEnqDate = System.DateTime.Now.AddDays(1);
				if (enquiryDt != null && enquiryDt.Tables.Count > 0 && enquiryDt.Tables[0].Rows.Count > 0)
				{

					surveyObj = (from rw in enquiryDt.Tables[0].AsEnumerable()
								 select new Entities.Enquiry()
								 {
									 ReloSmrtEnqNo = rw["ReloSmrtEnqNo"] == DBNull.Value ? null : Convert.ToString(rw["ReloSmrtEnqNo"]),
									 EnqID = rw["EnqID"] == DBNull.Value ? (Int64)0 : Convert.ToInt64(rw["EnqID"]),
									 EnqNo = rw["EnqNo"] == DBNull.Value ? null : Convert.ToString(rw["EnqNo"]),
									 EnqSourceID = rw["EnqSourceID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["EnqSourceID"]),
									 //AccountID = rw["AgentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["AgentID"]),
									 AgentID = rw["AgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["AgentID"]),
									 ChangeAcctMgrID = rw["ChangeAcctMgrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["ChangeAcctMgrID"]),
									 EnqDate = rw["EnqDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(rw["EnqDate"]),
									 CompareTentEnqDate = rw["EnqDate"] == DBNull.Value ? DateTime.Now.Date.AddDays(-1) : Convert.ToDateTime(rw["EnqDate"]).Date.AddDays(-1),
									 CompareContEnqDate = rw["EnqDate"] == DBNull.Value ? DateTime.Now.Date.AddDays(1) : Convert.ToDateTime(rw["EnqDate"]).Date.AddDays(1),
									 EnqFrom = rw["EnqFrom"] == DBNull.Value ? null : Convert.ToString(rw["EnqFrom"]),
									 EnqReceivedby = rw["ReceivedBy"] == DBNull.Value ? null : Convert.ToString(rw["ReceivedBy"]),
									 ContactDate = rw["EnqReceivedDate"] == DBNull.Value ? (DateTime)default(DateTime) : Convert.ToDateTime(rw["EnqReceivedDate"]),
									 TentativeDate = rw["TendativeMoveDate"] == DBNull.Value ? (DateTime)default(DateTime) : Convert.ToDateTime(rw["TendativeMoveDate"]),
									 BussinessLineID = rw["BussinessLineID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["BussinessLineID"]),
									 BussLineName = rw["BussLineName"] == DBNull.Value ? null : Convert.ToString(rw["BussLineName"]),
									 MoveQuoteID = rw["MoveQuoteID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["MoveQuoteID"]),
									 RevenueBrId = rw["RevenueBrId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["RevenueBrId"]),
									 RevenueBr = rw["RevenueBr"] == DBNull.Value ? null : Convert.ToString(rw["RevenueBr"]),
									 ShipperTitle = rw["ShipperTitle"] == DBNull.Value ? null : Convert.ToString(rw["ShipperTitle"]).ToUpper().Trim(),
									 ShipperFName = rw["ShipperFName"] == DBNull.Value ? null : Convert.ToString(rw["ShipperFName"]),
									 ShipperLName = rw["ShipperLName"] == DBNull.Value ? null : Convert.ToString(rw["ShipperLName"]),
									 Designation = rw["Designation"] == DBNull.Value ? null : Convert.ToString(rw["Designation"]),
									 Nationality = rw["Nationality"] == DBNull.Value ? null : Convert.ToString(rw["Nationality"]),
									 ShipCategoryID = rw["ShipCategoryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["ShipCategoryID"]),
									 ShipTypeID = rw["ShipperTypeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["ShipperTypeID"]),
									 Address1 = rw["Address1"] == DBNull.Value ? null : Convert.ToString(rw["Address1"]),
									 Address2 = rw["Address2"] == DBNull.Value ? null : Convert.ToString(rw["Address2"]),
									 AddressCityID = rw["AddressCityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["AddressCityID"]),
									 PIN = rw["pin"] == DBNull.Value ? null : Convert.ToString(rw["pin"]),
									 Phone1 = rw["phone1"] == DBNull.Value ? null : Convert.ToString(rw["phone1"]),
									 Phone2 = rw["Phone2"] == DBNull.Value ? null : Convert.ToString(rw["Phone2"]),
									 Email = rw["Email"] == DBNull.Value ? null : Convert.ToString(rw["Email"]),
									 Remarks = rw["Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Remarks"]),
									 Isactive = rw["Isactive"] == DBNull.Value ? false : Convert.ToBoolean(rw["Isactive"]),
									 LostDate = rw["LostDate"] == DBNull.Value ? (DateTime)default(DateTime) : Convert.ToDateTime(rw["LostDate"]),
									 LostToCompitID = rw["LostToCompitID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["LostToCompitID"]),
									 LostTo = rw["LostToText"] == DBNull.Value ? null : Convert.ToString(rw["LostToText"]),
									 LostReasonID = rw["EnqLostReasonID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["EnqLostReasonID"]),
									 LostRemarks = rw["LostRemarks"] == DBNull.Value ? null : Convert.ToString(rw["LostRemarks"]),
									 ClientRef = rw["ClientRef"] == DBNull.Value ? null : Convert.ToString(rw["ClientRef"]),
									 ClientCP = rw["ClientCP"] == DBNull.Value ? null : Convert.ToString(rw["ClientCP"]),
									 CPEmail = rw["CPEmail"] == DBNull.Value ? null : Convert.ToString(rw["CPEmail"]),
									 IsEnqLost = Convert.ToBoolean(rw["IsEnqLost"]),
									 Createdby = rw["CreatedBy"] == DBNull.Value ? null : Convert.ToString(rw["CreatedBy"]),
									 //ContactDate = rw["ContactDate"] == DBNull.Value ? (DateTime)default(DateTime) : Convert.ToDateTime(rw["ContactDate"]),

									 ClientDetails = new Entities.ClientDetails
									 {
										 Account = rw["AgentName"] == DBNull.Value ? null : Convert.ToString(rw["AgentName"]),
										 Client = rw["AccountID"] == DBNull.Value ? (Int32)0 : Convert.ToInt32(rw["AccountID"])
									 }
								 }).First();


					if (enquiryDt.Tables[1] != null && enquiryDt.Tables[1].Rows.Count > 0)
					{
						empList = enquiryDt.Tables[1].AsEnumerable().
							Select(dataRow => new Entities.EnquiryDetail
							{
								EnqDetailID = dataRow["EnqDetailid"] == DBNull.Value ? 0 : Convert.ToInt64(dataRow["EnqDetailid"]),
								EnqSequenceID = dataRow["EnqDetSequenceID"] == DBNull.Value ? 0 : Convert.ToInt64(dataRow["EnqDetSequenceID"]),
								EnqID = dataRow["EnqID"] == DBNull.Value ? 0 : Convert.ToInt64(dataRow["EnqID"]),
								HandlingBrId = dataRow["HandlingBranchId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["HandlingBranchId"]),
								HandlingBr = dataRow["HandlingBranch"] == DBNull.Value ? null : Convert.ToString(dataRow["HandlingBranch"]),
								ServiceLineID = dataRow["ServiceLineID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ServiceLineID"]),
								ServiceLine = dataRow["ServiceLine"] == DBNull.Value ? null : Convert.ToString(dataRow["ServiceLine"]),
								FromCity = dataRow["FromCityId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["FromCityId"]),
								FCity = dataRow["FromCity"] == DBNull.Value ? null : Convert.ToString(dataRow["FromCity"]),
								ToCityID = dataRow["ToCityId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ToCityId"]),
								ToCity = dataRow["ToCity"] == DBNull.Value ? null : Convert.ToString(dataRow["ToCity"]),
								Mode = dataRow["TransportModeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TransportModeID"]),
								TMode = dataRow["TransportModeName"] == DBNull.Value ? null : Convert.ToString(dataRow["TransportModeName"]),
								ShipmentTypeID = dataRow["ShipmentTypeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ShipmentTypeID"]),
								ShipmentType = dataRow["ShipmentType"] == DBNull.Value ? null : Convert.ToString(dataRow["ShipmentType"]),
								GoodsDescId = dataRow["GoodsDescID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["GoodsDescID"]),
								GoodsDesc = dataRow["GoodsDescName"] == DBNull.Value ? null : Convert.ToString(dataRow["GoodsDescName"]),
								WtUnitid = dataRow["WtUnitId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["WtUnitId"]),
								WtUnit = dataRow["WtUnit"] == DBNull.Value ? null : Convert.ToString(dataRow["WtUnit"]),
								WtNet = dataRow["WtNet"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["WtNet"]),
								WtGross = dataRow["WtGross"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["WtGross"]),
								WtACWT = dataRow["WtACWT"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["WtACWT"]),
								VolumeUnitID = dataRow["VolumeUnitID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["VolumeUnitID"]),
								VolumeUnit = dataRow["VolumeUnit"] == DBNull.Value ? null : Convert.ToString(dataRow["VolumeUnit"]),
								VolumeToPack = dataRow["VolumeToPack"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["VolumeToPack"]),
								VolumeNet = dataRow["VolumeNet"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["VolumeNet"]),
								VolumeGross = dataRow["VolumeGross"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["VolumeGross"]),
								LooseCased = dataRow["LooseCased"] == DBNull.Value ? null : Convert.ToString(dataRow["LooseCased"]),
								DensiyFactor = dataRow["DensityFact"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DensityFact"]),
								LCLFCL = dataRow["DensityFact"] == DBNull.Value ? null : Convert.ToString(dataRow["lclorfcl"]),
								ContainerTypeID = dataRow["ContainerSizeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ContainerSizeID"]),
								ContainerType = dataRow["ContainerSizeName"] == DBNull.Value ? null : Convert.ToString(dataRow["ContainerSizeName"]),
								Remarks = dataRow["Remarks"] == DBNull.Value ? null : Convert.ToString(dataRow["Remarks"]),
								TentativeMovedate = dataRow["TentativeMovedate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["TentativeMovedate"]),
								SchSurveyorID = dataRow["SchSurveyorID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["SchSurveyorID"]),
								SchSurveyor = dataRow["SchSurveyor"] == DBNull.Value ? null : Convert.ToString(dataRow["SchSurveyor"]),
								SchSurveyorRemark = dataRow["SchSurveyorRemarks"] == DBNull.Value ? null : Convert.ToString(dataRow["SchSurveyorRemarks"]),
								SchSurveyDate = dataRow["SchSurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SchSurveyDate"]),
								IsShowSurvey = Convert.ToBoolean(dataRow["IsShowSurvey"]),
								AllowEdit = Convert.ToBoolean(dataRow["AllowEdit"]),
								ShowSendToMobile = Convert.ToBoolean(dataRow["ShowSendToMobile"]),
								ShowFollowUp = Convert.ToBoolean(dataRow["ShowFollowUp"]),
								ShpStatus = Convert.ToString(dataRow["ShipmentStatus"])
							}).ToList();


					}

					if (enquiryDt.Tables[2] != null && enquiryDt.Tables[2].Rows.Count > 0)
					{
						surveyObj.FollowUpList = enquiryDt.Tables[2].AsEnumerable().
							Select(dataRow => new Entities.EnqFollowUpDetails
							{
								EnqDetID = dataRow["EnqDetID"] == DBNull.Value ? 0 : Convert.ToInt64(dataRow["EnqDetID"]),
								FollowUpDate = dataRow["FollowUpDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["FollowUpDate"]),
								FollowUpRemark = dataRow["FollowUpRemarks"] == DBNull.Value ? null : Convert.ToString(dataRow["FollowUpRemarks"]),
								IsClose = Convert.ToBoolean(dataRow["IsClose"]),
								CreatedDate = dataRow["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedDate"]),
								CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? null : Convert.ToString(dataRow["CreatedBy"])
							}).ToList();


					}
					surveyObj.RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
					surveyObj.CompId = UserSession.GetUserSession().CompanyID;
					return surveyObj;
				}


			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return surveyObj;

		}

		public IEnumerable<Entities.Enquiry> GetenquiryList(DateTime? FromDate, DateTime? Todate, string sort, string sortdir, int skip, int pageSize, out int totalCount, string EnqID = null, string Shipper = null)
		{
			totalCount = 0;

			try
			{
				int RMCBuss = 0;
				if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
				{
					RMCBuss = 0;
				}
				else
				{
					RMCBuss = 1;
				}
				IQueryable<Entities.Enquiry> EnquiryList = enquiryDAL.GetEnquiryList(FromDate, Todate, EnqID, RMCBuss == 1, Shipper);
				if (EnquiryList != null)
				{
					totalCount = EnquiryList.Count();
					EnquiryList = EnquiryList.OrderBy(sort + " " + sortdir);
					if (pageSize > 0)
					{
						EnquiryList = EnquiryList.Skip(skip).Take(pageSize);
					}
					return EnquiryList.ToList();
				}
				else
				{
					return new List<Entities.Enquiry>();
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EnquiryBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

		}

		public string GetProjectByServiceLine(int ServiceLineID)
		{
			string project = null;
			try
			{
				DataTable ProjectDT = enquiryDAL.GetProjectByServiceLine(ServiceLineID);
				project = Convert.ToString(ProjectDT.Rows[0]["Project"]);
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return project;
		}
		
		public bool SentToMobile(int? EnqID, int? EnquiryDetailID, out string result)
		{
			//string project = null;
			bool res = false;
			try
			{
				res = enquiryDAL.SentToMobile(EnqID, EnquiryDetailID, out result);
				//project = Convert.ToString(ProjectDT.Rows[0]["Project"]);
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return res;
		}

        public bool UpdateSurveyorIdGMMS(JobSurveyForMobile model, out string message)
        {
            int LoginID = UserSession.GetUserSession().LoginID;
            message = string.Empty;
            try
            {
                bool res = enquiryDAL.UpdateSurveyorIdGMMS(model, LoginID, out message);

                if (res)
                {
                    res = enquiryDAL.SentToMobile(model.EnqID, model.EnqDetailID, out message);
                }

                return res;

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "EnquiryBL", "SentToMobile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

		public bool InsertFollowUpDetials(Entities.Enquiry SaveData, out string result)
		{
			result = String.Empty;
			int LoginID = UserSession.GetUserSession().LoginID;
			try
			{

				return enquiryDAL.InsertFollowUpDetials(SaveData, LoginID, out result);

			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(LoginID), "EnquiryBL", "InsertFollowUpDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

		}
	}
}