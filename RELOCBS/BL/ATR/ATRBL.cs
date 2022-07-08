using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.ATR;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.ATR
{
    public class ATRBL
    {

        private ATRDAL _aTRDAL;

        public ATRDAL aTRDAL
        {

            get
            {
                if (this._aTRDAL == null)
                    this._aTRDAL = new ATRDAL();
                return this._aTRDAL;
            }
        }

        public bool Insert(RELOCBS.Entities.ATRPoint model, out string result)
        {
            try
            {
                return aTRDAL.Insert(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<Entities.ATRPointGrid> GetForGrid(int? DepartmentId, string IssueHeading, int? ComplianceStatusId, DateTime? IssuedMonth, string sort, string sortdir, int skip, int pageSize, out int totalCount)
        {
            try
            {
                IQueryable<Entities.ATRPointGrid> grids = aTRDAL.GetForGrid(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, DepartmentId, IssueHeading, ComplianceStatusId, IssuedMonth);

                totalCount = grids.Count();
                if (pageSize > 1)
                {
                    grids = grids.Skip((skip * (pageSize - 1))).Take(skip);
                }
                else
                {
                    grids = grids.Take(skip);
                }

                //AllocationList = AllocationList.OrderBy(sort + " " + sortdir);

                return grids.ToList();

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public RELOCBS.Entities.ATRPoint GetDetailById(Int64 Id)
        {

            ATRPoint AObj = new ATRPoint();
            try
            {
                
                DataSet CostDs = aTRDAL.GetDetailById(UserSession.GetUserSession().LoginID, Id);

                if (CostDs != null && CostDs.Tables.Count >= 1)
                {

                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {

                        AObj.ATRPointId = Convert.ToInt64(CostDs.Tables[0].Rows[0]["ATRPointId"]);

                        AObj.IssueHeading = Convert.ToString(CostDs.Tables[0].Rows[0]["IssueHeading"]);
                        AObj.IssueDescription = Convert.ToString(CostDs.Tables[0].Rows[0]["IssueDescription"]);
                        AObj.AuditReportSource = Convert.ToString(CostDs.Tables[0].Rows[0]["AuditReportSource"]);
                        AObj.MonthOfIssue = Convert.ToDateTime(CostDs.Tables[0].Rows[0]["MonthOfIssue"]);
                        AObj.Remark = Convert.ToString(CostDs.Tables[0].Rows[0]["Remark"]);
                        AObj.RiskId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RiskId"]);
                        AObj.CategoryId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["CategoryId"]);
                        AObj.ComplianceStatusId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ComplianceStatusId"]);
                        AObj.DepartmentId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["DepartmentId"]);
                        AObj.FirstPersonRespLoginId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["FirstPersonLoginId"]);
                        AObj.SecondPersonRespLoginId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["SecondPersonLoginId"]);
                    }

                    ////// Is HO
                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {

                        AObj.IsHO = Convert.ToBoolean(CostDs.Tables[1].Rows[0]["IsHO"]);
                    }

                }


            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return AObj;


        }


        public RELOCBS.Entities.ATRPointReponse GetMgtResponseDetailById(Int64 Id)
        {

            ATRPointReponse AObj = new ATRPointReponse();
            try
            {

                DataSet CostDs = aTRDAL.GetMgtResponseDetailById(UserSession.GetUserSession().LoginID, Id);

                if (CostDs != null && CostDs.Tables.Count >= 1)
                {

                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {

                        AObj.aTRPoint.ATRPointId = Convert.ToInt64(CostDs.Tables[0].Rows[0]["ATRPointId"]);

                        AObj.aTRPoint.IssueHeading = Convert.ToString(CostDs.Tables[0].Rows[0]["IssueHeading"]);
                        AObj.aTRPoint.IssueDescription = Convert.ToString(CostDs.Tables[0].Rows[0]["IssueDescription"]);
                        AObj.aTRPoint.AuditReportSource = Convert.ToString(CostDs.Tables[0].Rows[0]["AuditReportSource"]);
                        AObj.aTRPoint.MonthOfIssue = Convert.ToDateTime(CostDs.Tables[0].Rows[0]["MonthOfIssue"]);
                        AObj.aTRPoint.Remark = Convert.ToString(CostDs.Tables[0].Rows[0]["Remark"]);
                        AObj.aTRPoint.RiskId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RiskId"]);
                        AObj.aTRPoint.CategoryId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["CategoryId"]);
                        AObj.aTRPoint.ComplianceStatusId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ComplianceStatusId"]);
                        AObj.aTRPoint.DepartmentId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["DepartmentId"]);
                        AObj.aTRPoint.FirstPersonRespLoginId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["FirstPersonLoginId"]);
                        AObj.aTRPoint.SecondPersonRespLoginId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["SecondPersonLoginId"]);
                        AObj.aTRPoint.AuditeeStatusId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["AuditeeStatusId"]);
                        AObj.AuditeeStatusId = AObj.aTRPoint.AuditeeStatusId;
                        AObj.MgtReponse = Convert.ToString(CostDs.Tables[0].Rows[0]["LastMgtResponse"]);

                        AObj.CommittedDate = CostDs.Tables[0].Rows[0]["LastCommitedDate"] !=DBNull.Value ? Convert.ToDateTime(CostDs.Tables[0].Rows[0]["LastCommitedDate"]):(DateTime?)null;
                        
                    }

                    ////Files
                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {

                        AObj.docLists = (from item in CostDs.Tables[1].AsEnumerable()
                                              select new ATRPointDoc()
                                              {
                                                  FileID = Convert.ToInt32(item["FileID"]),
                                                  DocDescription = Convert.ToString(item["Description"]),
                                                  UploadBy = Convert.ToString(item["CreatedBy"]),
                                                  UploadDate = Convert.ToDateTime(item["CreatedDate"]),
                                                  FileName = Convert.ToString(item["FileName"])

                                              }).ToList();
                    }


                    ////Comm
                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {

                        AObj.history = (from item in CostDs.Tables[2].AsEnumerable()
                                        select new ATRPointHistory()
                                        {
                                            Status = Convert.ToString(item["StatusName"]),
                                            StatusType = Convert.ToString(item["StatusType"]),
                                            Response = Convert.ToString(item["Remark"]),
                                            CommitedDate = item["CommitedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["CommitedDate"]),
                                            SentBy = Convert.ToString(item["SentBy"]),
                                            SentDate = Convert.ToDateTime(item["Sentdate"]),
                                            UploadFiles = Convert.ToString(item["Files"]),
                                            ResponseType = Convert.ToString(item["ResponseType"])
                                        }).ToList();
                    }


                    ////// Is HO
                    if (CostDs.Tables.Count > 3 && CostDs.Tables[3] != null && CostDs.Tables[3].Rows.Count > 0)
                    {
                        AObj.IsHO = Convert.ToBoolean(CostDs.Tables[3].Rows[0]["IsHO"]);
                    }

                }


            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRBL", "GetMgtResponseDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return AObj;


        }

        public bool InsertManagementResponse(ATRPointReponse model, out string result)
        {
            try
            {
                return aTRDAL.InsertManagementResponse(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRBL", "InsertManagementResponse", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool CheckIsHO()
        {
            try
            {
                return aTRDAL.CheckIsHO(UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRBL", "CheckIsHO", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public ATRPointDoc GetDownloadFile(int FileID)
        {
            ATRPointDoc obj = new ATRPointDoc();
            try
            {
                return aTRDAL.GetDownloadFile(FileID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        public bool DeleteDocument(Int64 ID, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                return aTRDAL.DeleteDocument(ID, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "ATRBL", "DeleteDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}