using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.UserFeedback;
using RELOCBS.Models;
using RELOCBS.Utility;

namespace RELOCBS.BL.UserFeedback
{

    public class UserFeedbackBL
    {

        private CommonSubs _cSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (this._cSubs == null)
                    this._cSubs = new CommonSubs();
                return this._cSubs;
            }
        }


        private UserFeedbackDAL _feedbackDAL;
        public UserFeedbackDAL feedbackDAL
        {
            get
            {
                if (this._feedbackDAL == null)
                    this._feedbackDAL = new UserFeedbackDAL();
                return this._feedbackDAL;
            }
        }

        public Entities.UserFeedback GetFeedbackQuestions()
        {
            Entities.UserFeedback feedback = new Entities.UserFeedback();
            Int32 LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                DataSet questionsDs = feedbackDAL.GetFeedbackQuestions(LoginID);

                if (questionsDs != null && questionsDs.Tables.Count > 0)
                {
                    if (questionsDs.Tables.Count > 0 && questionsDs.Tables[0] != null && questionsDs.Tables[0].Rows.Count > 0)
                    {

                        feedback.questions = (from item in questionsDs.Tables[0].AsEnumerable()
                                 select new Entities.feedbackQuestions()
                                 {
                                     QuestionID = Convert.ToInt32(item["QuestID"]),
                                     QuestionText = Convert.ToString(item["Question_Text"]),
                                     AnswerType = Convert.ToString(item["Answer_Type"]),
                                     SrNo = Convert.ToInt16(item["SrNo"]),
                                     options = GetQuestionsOptions(Convert.ToInt32(item["QuestID"]), questionsDs.Tables[1]),
                                     
                                 }).ToList();


                        DataRow TemplateRow = questionsDs.Tables[0].Rows[0];
                        feedback.TemplateID = Convert.ToInt32(TemplateRow["TemplateID"]);
                        feedback.Title = Convert.ToString(TemplateRow["TemplateName"]);
                        feedback.Description = Convert.ToString(TemplateRow["Description"]);

                    }

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "UserFeedbackBL", "GetFeedbackQuestions", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return feedback;

        }


        public bool SumbitUserFeedback(Entities.UserFeedback model, out string message)
        {
            message = string.Empty;
            Int32 LoginID = UserSession.GetUserSession().LoginID;
            try
            {

                GetQuestionAnswerXml(model);
                return feedbackDAL.SumbitUserFeedback(model, LoginID, out message);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "UserFeedbackBL", "SumbitUserFeedback", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            

        }


        public Entities.UserFeedbackStatus CheckUserFeedbackStatus()
        {
            Entities.UserFeedbackStatus status = new Entities.UserFeedbackStatus();
            Int32 LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                DataTable statusDt = feedbackDAL.CheckUserFeedbackStatus(LoginID);
                if (statusDt != null && statusDt.Rows.Count>0)
                {
                    status = (from rw in statusDt.AsEnumerable()
                              select new Entities.UserFeedbackStatus()
                              {
                                  TemplateID = Convert.ToInt32(rw["TemplateID"]),
                                  Status = Convert.ToString(rw["UserfeebackStatus"])
                              }).FirstOrDefault();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "UserFeedbackBL", "CheckUserFeedbackStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return status;
        }

        public List<Entities.QuestionOptions> GetQuestionsOptions(int QuestionID,DataTable Optionsdt)
        {
            List<Entities.QuestionOptions> options = new List<Entities.QuestionOptions>();

            if (Optionsdt != null && Optionsdt.Rows.Count > 0)
            {
                options =(from item in Optionsdt.AsEnumerable().Where(r => r.Field<Int32>("QuestID") == QuestionID).AsEnumerable()
                        select new Entities.QuestionOptions()
                        {
                            OptionID = Convert.ToInt32(item["AnsOptionID"]),
                            OptionDescription = Convert.ToString(item["AnswerText"]),
                            OptionImgUrl = Convert.ToString(item["ImgUrl"]),
                            SrNo = Convert.ToInt16(item["SrNo"]),
                            Value = Convert.ToString(item["AnsOptionID"]),
                            Text = Convert.ToString(item["AnswerText"])
                        }).ToList();
            }

            return options;
        }

        public void UpdateUserFeedbackSession()
        {
            CustomSessionStore session = new CustomSessionStore();
            var user = UserSession.GetUserSession();
            user.UserFeedbackStatus = CheckUserFeedbackStatus().Status;
            session.Set<UserInformationModel>("UserSession", user);
        }

        private void GetQuestionAnswerXml(Entities.UserFeedback model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, Encoding.Unicode);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.CloseOutput = true;
                XmlWriter xmltextWriter = XmlWriter.Create(stringWriter, settings);
                //StringWriter stringWriter = new StringWriter();
                // XmlTextWriter xmltextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented, };

                // Start document
                xmltextWriter.WriteStartDocument();
                xmltextWriter.WriteStartElement("ROOT");

                foreach (var question in model.questions)
                {
                    //Create a page element
                    ///Question element
                    if (question.AnswerType.Equals("LIST", StringComparison.OrdinalIgnoreCase) || question.AnswerType.Equals("RADIOGROUP", StringComparison.OrdinalIgnoreCase) || question.AnswerType.Equals("TEXT", StringComparison.OrdinalIgnoreCase) || question.AnswerType.Equals("DATE", StringComparison.OrdinalIgnoreCase))
                    {
                        xmltextWriter.WriteStartElement("Question");
                        xmltextWriter.WriteAttributeString("QustID", Convert.ToString(question.QuestionID));
                    }
                    ////Answers element
                    if (question.AnswerType.Equals("TEXT", StringComparison.OrdinalIgnoreCase) || question.AnswerType.Equals("DATE", StringComparison.OrdinalIgnoreCase))
                    {
                        xmltextWriter.WriteAttributeString("AnsText", question.AnswerText);
                        xmltextWriter.WriteEndElement();
                    }
                    else if (question.AnswerType.Equals("LIST", StringComparison.OrdinalIgnoreCase) || question.AnswerType.Equals("RADIOGROUP", StringComparison.OrdinalIgnoreCase))
                    {
                        xmltextWriter.WriteAttributeString("AnsOptionID", Convert.ToString(question.AnswerID));
                        xmltextWriter.WriteEndElement();
                    }

                    ///Question Answer element
                    if (question.AnswerType.Equals("CHECKBOX", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var option in question.options)
                        {
                            if (option.IsCheck)
                            {
                                xmltextWriter.WriteStartElement("Question");
                                xmltextWriter.WriteAttributeString("QustID", Convert.ToString(question.QuestionID));
                                xmltextWriter.WriteAttributeString("AnsOptionID", Convert.ToString(option.OptionID));
                                xmltextWriter.WriteEndElement();
                            }
                        }
                    }

                    
                }

                xmltextWriter.WriteEndElement();
                xmltextWriter.Flush();
                xmltextWriter.Close();
                //XmlDocument xm = new XmlDocument();
                //xm.LoadXml(sb.ToString());
                //model.AnswerList = xm.InnerXml.ToString();
                model.AnswerList = sb.ToString();
                //stringWriter.Flush();
            }
            catch (Exception)
            {

                throw;
            }


        }

    }

    public class StringWriterWithEncoding : StringWriter
    {
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            this.m_Encoding = encoding;
        }
        private readonly Encoding m_Encoding;
        public override Encoding Encoding
        {
            get
            {
                return this.m_Encoding;
            }
        }
    }
}