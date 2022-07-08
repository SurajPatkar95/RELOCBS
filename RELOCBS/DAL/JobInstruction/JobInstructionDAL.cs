using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.JobInstruction
{
    public class JobInstructionDAL
    {
        private CommonSubs _CSubs;


        public CommonSubs CSubs
        {

            get
            {
                if (this._CSubs == null)
                    this._CSubs = new CommonSubs();
                return this._CSubs;
            }
        }

        public bool Insert(RELOCBS.Entities.InstructionSheet sheet,string submit, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string ActivityXml = string.Empty;
                        string CaseDimXml = string.Empty;
                        string InsturctionsXML = string.Empty;
                        string SubInsturctionsXML = string.Empty;
                        string ModeLabelXML = string.Empty;
                        bool IsSentToWarehouse = false;

                        if (submit.ToUpper()== "SENTTOWAREHOUSE")
                        {
                            IsSentToWarehouse = true;
                        }

                        if (sheet.activities!=null && sheet.activities.Count>0)
                        {
                            ActivityXml = Convert.ToString(new XElement("Activities", from emp in sheet.activities
                                                                                      select new XElement("Activity",
                                                                             new XElement("ACTID", emp.ActivityID),
                                                                             new XElement("TypeID", emp.ActivityTypeID),
                                                                             new XElement("FromDate", Convert.ToDateTime(emp.FromDate).ToString("dd-MMM-yyyy HH:mm")),
                                                                             new XElement("ToDate", Convert.ToDateTime(emp.ToDate).ToString("dd-MMM-yyyy HH:mm")),
                                                                             new XElement("RepTime", emp.RepTime.Value.ToString()),
                                                                             new XElement("FromLoc", emp.FromLocation),
                                                                             new XElement("ToLoc", emp.ToLocation),
                                                                             new XElement("Remark", emp.Remark),
                                                                             new XElement("Days", emp.NumberOfDays),
                                                                             new XElement("Deleted", emp.InActive)

                                                                         )));
                        }

                        if (sheet.Dimensions != null && sheet.Dimensions.Count > 0)
                        {
                            CaseDimXml = Convert.ToString(new XElement("Cases", from emp in sheet.Dimensions
                                                                                select new XElement("Case",
                                                                       new XElement("ACTID", emp.CS_ID),
                                                                       new XElement("TypeID", emp.CaseTypeID),
                                                                       new XElement("Length", emp.Length),
                                                                       new XElement("Breadth", emp.Breadth),
                                                                       new XElement("Height", emp.Height),
                                                                       new XElement("UnitID", emp.UnitID),
                                                                       new XElement("NoOfPackages", emp.NoOfPackages),
                                                                       new XElement("Deleted", emp.InActive)

                                                                   )));
                        }


                        if (sheet.SelectedMultiInstructionsId != null && sheet.SelectedMultiInstructionsId.Count() > 0)
                        {
                            InsturctionsXML = Convert.ToString(new XElement("Questions", from emp in sheet.SelectedMultiInstructionsId
                                                                                         select new XElement("Question",
                                                                                new XElement("QustID", emp)
                                                                            )));

                            XDocument doc = new XDocument(new XElement("SubQuestions"));
                            foreach (Question item in sheet.questions)
                            {
                                if (item.subQuestions!=null && item.subQuestions.Count>0)
                                {
                                    doc.Root.Add(from test in item.subQuestions
                                                 select new XElement("SubQuestion",
                                        new XElement("QustID", test.QuestionID),
                                        new XElement("SubQuestID", test.SubQuestionID),
                                        new XElement("AnswerText", test.AnswerText),
                                        new XElement("AnswerDate", test.AnswerDate),
                                        new XElement("IDtoRefer", test.IDtoRefer)
                                        ));
                                }
                                
                            }

                            SubInsturctionsXML = Convert.ToString(doc);
                        }
                        
                        if (sheet.modeLables!=null && sheet.modeLables.Count>0)
                        {
                            ModeLabelXML = Convert.ToString(new XElement("Labels", from emp in sheet.modeLables
                                                                                   select new XElement("Label",
                                                                new XElement("InfoID", emp.InfoID),
                                                                new XElement("MoveTypeID", emp.ModeID),
                                                                new XElement("NoOfLables", emp.NoOfLables),
                                                                new XElement("LabelStartFrom", emp.LabelStartFrom)
                                                              )));
                        } 
                        
                        conn.AddCommand("[Warehouse].[AddEditInstructionSheet]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, sheet.InstID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, sheet.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Special_Instructions", SqlDbType.NVarChar, 3500, ParameterDirection.Input, CSubs.PSafeValue(sheet.SpecialInstructions));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.NVarChar, 3500, ParameterDirection.Input, CSubs.PSafeValue(sheet.Remarks));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchID", SqlDbType.Int, 0, ParameterDirection.Input, sheet.BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseID", SqlDbType.Int, 0, ParameterDirection.Input, sheet.WareHouseID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateComponentID", SqlDbType.Int, 0, ParameterDirection.Input, sheet.ComponentTypeID);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivitiesXML", SqlDbType.Xml, -1, ParameterDirection.Input, ActivityXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CaseDimensionXML", SqlDbType.Xml, -1, ParameterDirection.Input, CaseDimXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsturctionsXML", SqlDbType.Xml, -1, ParameterDirection.Input, InsturctionsXML);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubInsturctionsXML", SqlDbType.Xml, -1, ParameterDirection.Input, SubInsturctionsXML);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeLabelsXML", SqlDbType.Xml, -1, ParameterDirection.Input, ModeLabelXML);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAddrs1", SqlDbType.NVarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(sheet.OrgAdd1) );
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAddrs2", SqlDbType.NVarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(sheet.OrgAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0 , ParameterDirection.Input, sheet.OrgCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPincode",SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(sheet.OrgPincode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgEmail", SqlDbType.VarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(sheet.OrgEmail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPhone" ,SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(sheet.OrgPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgMobile", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(sheet.OrgMobile));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAddrs1" , SqlDbType.NVarChar,150,ParameterDirection.Input, CSubs.PSafeValue(sheet.DestAdd1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAddrs2", SqlDbType.NVarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(sheet.DestAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, sheet.DestCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPincode", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(sheet.DestPincode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestEmail", SqlDbType.NVarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(sheet.DestEmail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPhone", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(sheet.DestPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestMobile", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(sheet.DestMobile));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, sheet.WeightUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@WeightFrom", SqlDbType.Float,0, ParameterDirection.Input, sheet.WeightUnitFrom);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@WeightTo", SqlDbType.Float, 0, ParameterDirection.Input, sheet.WeightUnitTo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSentToWarehouse", SqlDbType.Bit, 0, ParameterDirection.Input,IsSentToWarehouse);
                        


                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {
                                sheet.InstID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InstID"));
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobInstructionDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public IQueryable<JobInstGrid> GetForGrid(int LoginID, int CompID,string JobNo,Int64? Moveid,DateTime? FromDate,DateTime? ToDate,string Shipper, bool? RMCBuss = false)
        {
            IQueryable<JobInstGrid> data= new List<JobInstGrid>().AsQueryable();
            try
            {
                string query = string.Format("EXEC [Warehouse].[GetInstructionSheetGrid] @SP_LoginID={0},@SP_FromDate={1},@SP_ToDate={2},@SP_JobNo={3},@SP_MoveID={4},@SP_CompID={5},@SP_IsRMCBuss={6},@SP_Shipper={7}",
                Convert.ToString(LoginID)
                , CSubs.QSafeValue(FromDate !=null ? (Convert.ToDateTime(FromDate)).ToString("dd-MMM-yyyy") :"")
                , CSubs.QSafeValue(ToDate != null ? Convert.ToDateTime(ToDate).ToString("dd-MMM-yyyy") : "")
                , CSubs.QSafeValue(JobNo)
                , CSubs.QSafeValue(Convert.ToString(Moveid))
                , CSubs.QSafeValue(Convert.ToString(CompID))
                , CSubs.QSafeValue(Convert.ToString(RMCBuss))
                ,CSubs.QSafeValue(Shipper)
                );

                DataSet dataSet = CSubs.GetDataSet(query);


                if (dataSet!=null && dataSet.Tables.Count > 0)
                {
                    var result = (from rw in dataSet.Tables[0].AsEnumerable()
                                  select new JobInstGrid()
                                  {
                                      MoveID = Convert.ToInt64(rw["MoveID"]),
                                      JobNo = Convert.ToString(rw["JobNo"]),
                                      //FromCity = Convert.ToString(rw["OrgCity"]),
                                      //ExitPort = Convert.ToString(rw["ExitPort"]),
                                      //EntryPort = Convert.ToString(rw["EntryPort"]),
                                      //ToCity = Convert.ToString(rw["DestCity"]),
                                      JobOpenDate = Convert.ToDateTime(rw["JobDate"]),
                                      Mode = Convert.ToString(rw["ModeName"]),
                                      Shipper = Convert.ToString(rw["Shipper"]),
                                      Client = Convert.ToString(rw["Client"]),
                                      Corporate = Convert.ToString(rw["Corporate"]),
                                      //Volume = Convert.ToInt64(rw["WeightFrom"]),
                                      Status = Convert.ToString(rw["JobStatus"]),
                                      ServiceLine = Convert.ToString(rw["ServiceLine"]),
                                      instructionSheetGrids = (from item in dataSet.Tables[1].Select("MoveID = " + Convert.ToString(rw["MoveID"])).AsEnumerable()
                                                               select new InstructionSheetGrid()
                                                               {
                                                                   MoveID = Convert.ToInt64(item["MoveID"]),
                                                                   InstID = Convert.ToInt64(item["InstID"]),
                                                                   SpecialInstructions = Convert.ToString(item["Special_Instruction"]),
                                                                   InstDate = Convert.ToDateTime(item["InstructionDate"]),
                                                                   ExpectedBeginDateTime = !string.IsNullOrWhiteSpace(Convert.ToString(item["ExpectedBeginDate"])) ? Convert.ToDateTime(item["ExpectedBeginDate"]) : (DateTime?)null,
                                                                   ExpectedCompletionDateTime = !string.IsNullOrWhiteSpace(Convert.ToString(item["ExpectedCompletionDate"])) ? Convert.ToDateTime(item["ExpectedCompletionDate"]) : (DateTime?)null,
                                                                   Inst_Status = Convert.ToString(item["Status"]),
                                                                   InstType = Convert.ToString(item["RateComponentName"]),
                                                                   Wt_Vol = Convert.ToString(item["Wt_Vol"]),
                                                                   //Edit = Convert.ToString(item["Edit"]),
                                                                   //Delete = Convert.ToString(item["Delete"]),
                                                                   //View = Convert.ToString(rw["View"]),
                                                                   CreatedDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["CreatedDate"])) ? Convert.ToDateTime(item["CreatedDate"]) : (DateTime?)null,
                                                                   CreatedBy = Convert.ToString(item["CreatedBy"]),
                                                                   ModifiedBy = !string.IsNullOrWhiteSpace(Convert.ToString(item["ModifiedBy"])) ? Convert.ToString(item["ModifiedBy"]) : "",
                                                                   ModifiedDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["ModifiedDate"])) ? Convert.ToDateTime(item["ModifiedDate"]) : (DateTime?)null,
                                                                   BranchName = Convert.ToString(item["BranchName"]),
                                                                   WarehouseName = Convert.ToString(item["Warehoue_Name"]),
                                                                   IsSentToWarehouse = !string.IsNullOrWhiteSpace(Convert.ToString(item["IsSentToWarehouse"])) ? Convert.ToInt16(item["IsSentToWarehouse"]) : 0,

                                                               }).ToList()


                                  }).ToList();

                    data = result.AsQueryable<JobInstGrid>();
                }
                
                //foreach (var Job in result)
                //{

                //    string MoveID = Convert.ToString(Job.MoveID);
                //    Job.instructionSheetGrids=(from rw in dataSet.Tables[1].Select("MoveID = " +CSubs.QSafeValue(MoveID)).AsEnumerable()
                //                                select new InstructionSheetGrid()
                //                                {
                //                                    MoveID = Convert.ToInt64(rw["MoveID"]),
                //                                    InstID = Convert.ToInt64(rw["InstID"]),
                //                                    SpecialInstructions = Convert.ToString(rw["Special_Instruction"]),
                //                                    InstDate= Convert.ToDateTime(rw["InstructionDate"]),
                //                                    ExpectedBeginDateTime = !string.IsNullOrWhiteSpace(Convert.ToString(rw["ExpectedBeginDate"])) ?Convert.ToDateTime(rw["ExpectedBeginDate"]) :(DateTime?)null,
                //                                    ExpectedCompletionDateTime = !string.IsNullOrWhiteSpace(Convert.ToString(rw["ExpectedCompletionDate"])) ? Convert.ToDateTime(rw["ExpectedCompletionDate"]) : (DateTime?)null,
                //                                    Inst_Status = Convert.ToString(rw["Status"]),
                //                                    InstType = Convert.ToString(rw["RateComponentName"]),
                //                                    Edit = Convert.ToString(rw["Edit"]),
                //                                    Delete = Convert.ToString(rw["Delete"]),
                //                                    View = Convert.ToString(rw["View"]),
                //                                    CreatedDate = !string.IsNullOrWhiteSpace(Convert.ToString(rw["CreatedDate"])) ? Convert.ToDateTime(rw["CreatedDate"]) : (DateTime?)null,
                //                                    CreatedBy = Convert.ToString(rw["CreatedBy"]),
                //                                    ModifiedBy = !string.IsNullOrWhiteSpace(Convert.ToString(rw["ModifiedBy"]))? Convert.ToString(rw["ModifiedBy"]) :"",
                //                                    ModifiedDate = !string.IsNullOrWhiteSpace(Convert.ToString( rw["ModifiedDate"])) ? Convert.ToDateTime(rw["ModifiedDate"]) : (DateTime?)null,
                //                                    BranchName = Convert.ToString(rw["BranchName"]),
                //                                    WarehouseName = Convert.ToString(rw["Warehoue_Name"])
                //                                }).ToList();

                //    if (Job.instructionSheetGrids.Count(p => p.Inst_Status.ToUpper() == "COMPLETED")== Job.instructionSheetGrids.Count)
                //    {
                //        Job.Status = "Completed";
                //    }
                //}


                ;

                return data;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobInstructionDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDetailById(int LoginID, Int64? JAID, Int64? MoveID)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Warehouse].[GetJobInstructionSheetForDisplay] @SP_InstID={0},@SP_MoveID={1},@SP_LoginID={2}",
                 CSubs.QSafeValue(Convert.ToString(JAID)), CSubs.QSafeValue(Convert.ToString(MoveID))
                 , CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobInstructionDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;
        }

        public DataSet GetPrintDetail(int LoginID,Int64 JAID,Int64 MoveID)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Warehouse].[GetJobInstructionSheetForPrint] @SP_InstID={0},@SP_MoveID={1},@SP_LoginID={2}",
                 CSubs.QSafeValue(Convert.ToString(JAID)), CSubs.QSafeValue(Convert.ToString(MoveID)),CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobInstructionDAL", "GetPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;

        }
    }
}