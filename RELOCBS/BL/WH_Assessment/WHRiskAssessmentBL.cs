using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.WH_Assessment;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using RELOCBS.Entities;
using System.Data;

namespace RELOCBS.BL.WH_Assessment
{
    public class WHRiskAssessmentBL
    {

        private WHRiskAssessmentDAL _assessmentDAL;

        public WHRiskAssessmentDAL assessmentDAL
        {

            get
            {
                if (this._assessmentDAL == null)
                    this._assessmentDAL = new WHRiskAssessmentDAL();
                return this._assessmentDAL;
            }
        }


        public IEnumerable<Entities.WHAssessmentGrid> GetGrid(DateTime? FromDate, DateTime? Todate, string sort, string sortdir, int skip, int pageSize, out int totalCount, int? WarehouseId = null)
        {
            totalCount = 0;

            try
            {
                bool RMCBuss = false;
                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    RMCBuss = false;
                }
                else
                {
                    RMCBuss = true;
                }
                IQueryable<Entities.WHAssessmentGrid> List = assessmentDAL.GetGrid(FromDate, Todate, RMCBuss, WarehouseId);
                if (List != null)
                {
                    totalCount = List.Count();

                    if (pageSize > 1)
                    {
                        List = List.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        List = List.Take(skip);
                    }

                    if (!string.IsNullOrEmpty(sort))
                    {
                        List = List.OrderBy(sort + " " + sortdir);
                    }
                    return List.ToList();
                }
                else
                {
                    return new List<Entities.WHAssessmentGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHRiskAssessmentBL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public WHAssessmentViewModel GetDetail(Int64 TransId)
        {
            WHAssessmentViewModel model = new WHAssessmentViewModel();

            try
            {
                DataSet data = assessmentDAL.GetDetail(UserSession.GetUserSession().LoginID, TransId);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        model = (from item in data.Tables[0].AsEnumerable()
                                 select new WHAssessmentViewModel()
                                 {
                                     TransId = !string.IsNullOrWhiteSpace(Convert.ToString(item["TransId"])) ? Convert.ToInt64(item["TransId"]) : -1,
                                     WarehouseId = Convert.ToInt32(item["WarehouseId"]),
                                     Area = Convert.ToString(item["Area"]),
                                     NoOfLiftVan = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfLiftVan"])) ? Convert.ToInt32(item["NoOfLiftVan"]) : (int?)null,
                                     NoOfLiftVanStored = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfLiftVanStored"])) ? Convert.ToInt32(item["NoOfLiftVanStored"]) : (int?)null,
                                     NoOfPeople = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfPeople"])) ? Convert.ToInt32(item["NoOfPeople"]) : (int?)null,
                                     TotalVolCFT = !string.IsNullOrWhiteSpace(Convert.ToString(item["TotalVolCFT"])) ? Convert.ToInt64(item["TotalVolCFT"]) : (Int64?)null,
                                     AuditDate = Convert.ToDateTime(item["AuditDate"]),
                                     Remark = Convert.ToString(item["Remark"]),
                                     CreatedBy = Convert.ToString(item["CreatedBy"]),
                                     CreatedDate = Convert.ToDateTime(item["CreatedDate"])
                                 }).FirstOrDefault();

                    }

                    ////Job Details
                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        model.Participants = data.Tables[1].AsEnumerable().Select(s => Convert.ToInt32(Convert.ToString(s["EmpId"]))).ToArray();
                    }

                    ////From-To Traveled Locations
                    if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                    {
                        model.questions = (from item in data.Tables[2].AsEnumerable()
                                                 select new AssessmentQuestions()
                                                 {
                                                     TransId = !string.IsNullOrWhiteSpace(Convert.ToString(item["TransId"])) ? Convert.ToInt64(item["TransId"]) : -1,
                                                     TransDetailId = !string.IsNullOrWhiteSpace(Convert.ToString(item["TransDetailId"])) ? Convert.ToInt64(item["TransDetailId"]) : -1,
                                                     QuestionId = Convert.ToInt32(item["QuestionId"]),
                                                     CategoryName = Convert.ToString(item["CategoryName"]),
                                                     Parameter = Convert.ToString(item["Parameter"]),
                                                     Desired = Convert.ToString(item["Desired"]),
                                                     CategoryId= Convert.ToInt32(item["CategoryId"]),
                                                     CategoryOrder = Convert.ToInt32(item["CategoryOrder"]),
                                                     Comments = Convert.ToString(item["Remarks"]),
                                                     PriorityName = Convert.ToString(item["PriorityName"]),
                                                     QuestionOrder = Convert.ToInt16(item["QuestionOrder"]),
                                                     ResponsibilityName = Convert.ToString(item["ResponsibilityName"]),
                                                     Score = Convert.ToInt32(item["Score"]),
                                                     ScoreGiven =!string.IsNullOrWhiteSpace(Convert.ToString(item["ScoreGiven"])) ? Convert.ToInt32(item["ScoreGiven"]) : (int?)null,
                                                     ScoreObtained = !string.IsNullOrWhiteSpace(Convert.ToString(item["ScoreObtained"])) ? Convert.ToInt32(item["ScoreObtained"]) : (int?)null,
                                                     StatusId = !string.IsNullOrWhiteSpace(Convert.ToString(item["StatusId"])) ? Convert.ToInt32(item["StatusId"]) : (int?)null,
                                                     PriorityId = Convert.ToInt32(item["PriorityId"]),
                                                     ResponsibilityId = Convert.ToInt32(item["ResponsibilityId"]),
                                                 }).ToList();
                    }


                    if (data.Tables.Count > 3 && data.Tables[3] != null && data.Tables[3].Rows.Count > 0)
                    {
                        model.otherQuestions = (from item in data.Tables[3].AsEnumerable()
                                           select new AssessmentQuestions()
                                           {
                                               TransId = !string.IsNullOrWhiteSpace(Convert.ToString(item["TransId"])) ? Convert.ToInt64(item["TransId"]) : -1,
                                               TransDetailId = !string.IsNullOrWhiteSpace(Convert.ToString(item["TransDetailId"])) ? Convert.ToInt64(item["TransDetailId"]) : -1,
                                               //QuestionId = Convert.ToInt32(item["QuestionId"]),
                                               CategoryName = Convert.ToString(item["CategoryName"]),
                                               Parameter = Convert.ToString(item["Parameter"]),
                                               Desired = Convert.ToString(item["Desired"]),
                                               CategoryId = !string.IsNullOrWhiteSpace(Convert.ToString(item["CategoryId"])) ? Convert.ToInt32(item["CategoryId"]) : 0,
                                               CategoryOrder = Convert.ToInt32(item["CategoryOrder"]),
                                               Comments = Convert.ToString(item["Remarks"]),
                                               PriorityName = Convert.ToString(item["PriorityName"]),
                                               QuestionOrder = Convert.ToInt16(item["QuestionOrder"]),
                                               ResponsibilityName = Convert.ToString(item["ResponsibilityName"]),
                                               Score = Convert.ToInt32(item["Score"]),
                                               ScoreGiven = !string.IsNullOrWhiteSpace(Convert.ToString(item["ScoreGiven"])) ? Convert.ToInt32(item["ScoreGiven"]) : (int?)null,
                                               ScoreObtained = !string.IsNullOrWhiteSpace(Convert.ToString(item["ScoreObtained"])) ? Convert.ToInt32(item["ScoreObtained"]) : (int?)null,
                                               StatusId = !string.IsNullOrWhiteSpace(Convert.ToString(item["StatusId"])) ? Convert.ToInt32(item["StatusId"]) : (int?)null,
                                               PriorityId = !string.IsNullOrWhiteSpace(Convert.ToString(item["PriorityId"])) ? Convert.ToInt32(item["PriorityId"]) : 0,
                                               ResponsibilityId = !string.IsNullOrWhiteSpace(Convert.ToString(item["ResponsibilityId"])) ? Convert.ToInt32(item["ResponsibilityId"]) : 0,
                                           }).ToList();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHRiskAssessmentBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        //// Get Area Column based on the Warehouse Id from Comm.Warehouse Table
        public string GetWarehouseArea(Int64 WarehouseId)
        {
            try
            {
                DataTable data = assessmentDAL.GetWarehouseDetail(UserSession.GetUserSession().LoginID, WarehouseId);

                if (data != null && data.Rows.Count > 0)
                {
                    return data.Rows[0]["Area"].ToString();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHRiskAssessmentBL", "GetWarehouseArea", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return string.Empty;

        }

        public bool Insert(WHAssessmentViewModel model, out string result)
        {
            try
            {
                return assessmentDAL.Insert(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHRiskAssessmentBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Delete(Int64 ID, out string message)
        {
            try
            {
                return assessmentDAL.Delete(ID, UserSession.GetUserSession().LoginID, out message);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHRiskAssessmentBL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


        public bool DeleteDocument(int ID, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                return assessmentDAL.DeleteDocument(ID, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHRiskAssessmentBL", "InsertDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertDocument(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return assessmentDAL.InsertDocument(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHRiskAssessmentBL", "InsertDocument", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobDocument GetDownloadFile(int FileID)
        {
            JobDocument obj = new JobDocument();
            try
            {
                return assessmentDAL.GetDownloadFile(FileID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHRiskAssessmentBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        public JobDocUpload GetDocumentGrid(Int64 ID, string FromType, String DocFromType, int DocTypeID = -1, int DocNameID = -1, string DocDescription = "")
        {
            JobDocUpload jobDoc = new JobDocUpload();
            jobDoc.ID = ID;
            jobDoc.DocFromType = FromType;
            jobDoc.DocTypeID = DocTypeID;
            try
            {
                DataTable DocDs = assessmentDAL.GetDocumentGrid(ID, DocFromType, DocTypeID, DocNameID, DocDescription);


                if (DocDs != null)
                {

                    jobDoc.docLists = (from item in DocDs.AsEnumerable()
                                       select new JobDocument()
                                       {
                                           FileID = Convert.ToInt32(item["FileID"]),
                                           DocType = Convert.ToString(item["DocTypeName"]),
                                           DocName = Convert.ToString(item["DocName"]),
                                           DocDescription = Convert.ToString(item["Description"]),
                                           FileName = Convert.ToString(item["DocFileName"]),
                                           UploadBy = Convert.ToString(item["UploadBy"]),
                                           UploadById = Convert.ToInt32(item["CreatedBy"]),
                                           UploadDate = Convert.ToDateTime(item["CreatedDate"]),
                                           ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
                                       }).ToList();


                }

                return jobDoc;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHRiskAssessmentBL", "GetDocumentGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public WHAssessmentReport GetReport(int id)
        {
            WHAssessmentReport model = new WHAssessmentReport();

            try
            {
                DataSet data = assessmentDAL.GetReport(UserSession.GetUserSession().LoginID, id);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        model = (from item in data.Tables[0].AsEnumerable()
                                 select new WHAssessmentReport()
                                 {
                                     
                                     Warehouse = Convert.ToString(item["Warehoue_Name"]),
                                     Area = Convert.ToString(item["Area"]),
                                     NoOfLiftVan = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfLiftVan"])) ? Convert.ToInt32(item["NoOfLiftVan"]) : (int?)null,
                                     NoOfLiftVanStored = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfLiftVanStored"])) ? Convert.ToInt32(item["NoOfLiftVanStored"]) : (int?)null,
                                     NoOfPeople = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfPeople"])) ? Convert.ToInt32(item["NoOfPeople"]) : (int?)null,
                                     TotalVolCFT = !string.IsNullOrWhiteSpace(Convert.ToString(item["TotalVolCFT"])) ? Convert.ToInt64(item["TotalVolCFT"]) : (Int64?)null,
                                     AuditDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["AuditDate"])) ? Convert.ToDateTime(item["AuditDate"]) :(DateTime?)null,
                                     Remark = Convert.ToString(item["Remark"]),
                                     CreatedBy = Convert.ToString(item["CreatedBy"]),
                                     CreatedDate = Convert.ToDateTime(item["CreatedDate"])
                                 }).FirstOrDefault();

                    }

                    ////Emp Details
                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        model.Participants = data.Tables[1].AsEnumerable().Select(s => Convert.ToString(s["EmpName"])).ToArray();
                    }

                    
                    ////questions list
                    if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                    {
                        model.questions = (from item in data.Tables[2].AsEnumerable()
                                           select new AssessmentQuestions()
                                           {
                                               CategoryName = Convert.ToString(item["CategoryName"]),
                                               CategoryOrder = Convert.ToInt32(item["CategoryOrder"]),
                                               QuestionOrder =string.IsNullOrWhiteSpace(Convert.ToString(item["QuestionOrder"])) ? 0 : Convert.ToInt32(item["QuestionOrder"]),
                                               Parameter = Convert.ToString(item["Parameter"]),
                                               Desired = Convert.ToString(item["Desired"]),
                                               Comments = Convert.ToString(item["Remarks"]),
                                               PriorityName = Convert.ToString(item["PriorityName"]),
                                               ResponsibilityName = Convert.ToString(item["ResponsibilityName"]),
                                               Score = Convert.ToInt32(item["Score"]),
                                               ScoreGiven = !string.IsNullOrWhiteSpace(Convert.ToString(item["ScoreGiven"])) ? Convert.ToInt32(item["ScoreGiven"]) : (int?)null,
                                               ScoreObtained = !string.IsNullOrWhiteSpace(Convert.ToString(item["ScoreObtained"])) ? Convert.ToInt32(item["ScoreObtained"]) : (int?)null,
                                               Status = Convert.ToString(item["StatusName"])
                                           }).ToList();
                    }
                    
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHRiskAssessmentBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }
    }
}