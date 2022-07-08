using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.WH_Assessment;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using RELOCBS.Entities;
using System.Data;

namespace RELOCBS.BL.WH_Assessment
{
    public class WHAssessQuestionsBL
    {

        private WHAssessQuestionsDAL _assessmentDAL;

        public WHAssessQuestionsDAL assessmentDAL
        {

            get
            {
                if (this._assessmentDAL == null)
                    this._assessmentDAL = new WHAssessQuestionsDAL();
                return this._assessmentDAL;
            }
        }


        public IEnumerable<Entities.WHAssessmentQuestionGrid> GetGrid(int? CategoryId, string Parameter,  string sort, string sortdir, int skip, int pageSize, out int totalCount)
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
                IQueryable<Entities.WHAssessmentQuestionGrid> List = assessmentDAL.GetGrid(CategoryId, Parameter, RMCBuss);
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
                    return new List<Entities.WHAssessmentQuestionGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHAssessQuestionsBL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public WHAssessmentQuestions GetDetail(Int64 TransId)
        {
            WHAssessmentQuestions model = new WHAssessmentQuestions();

            try
            {
                DataTable data = assessmentDAL.GetDetail(UserSession.GetUserSession().LoginID, TransId);

                if (data != null && data.Rows.Count > 0)
                {
                    model = (from item in data.AsEnumerable()
                             select new WHAssessmentQuestions()
                             {
                                 QuestionId = Convert.ToInt32(item["QuestionId"]),
                                 QuestionOrder = Convert.ToInt16(item["QuestionOrder"]),
                                 Parameter = Convert.ToString(item["Parameter"]),
                                 Desired = Convert.ToString(item["Desired"]),
                                 CategoryId = Convert.ToInt32(item["CategoryId"]),
                                 ResponsibilityId = Convert.ToInt32(item["ResponsibilityId"]),
                                 PriorityId = Convert.ToInt32(item["PriorityId"]),
                                 Score = Convert.ToInt32(item["Score"]),
                                 IsActive = Convert.ToBoolean(item["IsActive"]),
                                 EffectiveFrom = Convert.ToDateTime(item["EffectiveFrom"]),
                                 
                             }).FirstOrDefault();

                    
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHAssessQuestionsBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHAssessQuestionsBL", "GetWarehouseArea", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return string.Empty;

        }

        public bool Insert(WHAssessmentQuestions model, out string result)
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHAssessQuestionsBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WHAssessQuestionsBL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}