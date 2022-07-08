using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.FundTranfer;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.BL.FundTransfer
{
	public class FundTransferBL
	{
		private FundTransferDAL _FundtranferDAL;

		public FundTransferDAL fundtranferDAL
		{

			get
			{
				if (this._FundtranferDAL == null)
					this._FundtranferDAL = new FundTransferDAL();
				return this._FundtranferDAL;
			}
		}

		public IEnumerable<Entities.InvoiceGrid> GetFDGridDetails(FundTranfer fa, string sort, string sortdir, int pageSize, out DataTable dt)
		{
			

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
				
				IQueryable<Entities.InvoiceGrid> FDList = fundtranferDAL.GetFDGridDetails(fa , out dt);
				if (FDList != null)
				{
					
					//FDList = FDList.OrderBy(sort + " " + sortdir);
					if (pageSize > 0)
					{
						//FDList = FDList.Skip(skip).Take(pageSize);
					}
					return FDList.ToList();
				}
				else
				{
					return new List<Entities.InvoiceGrid>();
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

		public bool InsertTransferFA(Entities.FundTranfer model, out string result)
		{
			try
			{
				//var ModeXML = model.ModeList != null ? new XElement("Modes", model.ModeList.Select(x => new XElement("ModeIDs", new XElement("ModeID", x)))) : new XElement("Modes");

				var billitem = new XElement("SelectedLists",
				from emp in model.InvGrid
				select new XElement("SelectedList",
							   new XElement("CBSRefID", emp.CBSRefID),
							   new XElement("InvOrCredit", emp.InvOrCredit),
							   new XElement("AccountCode", emp.AccountCode)
						   ));

				//result = "";
				return fundtranferDAL.InsertTransferFA(billitem, UserSession.GetUserSession().LoginID, out result);
				//return true;
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}

        public DataTable GetTransferToFADubai(Entities.FundTranfer model, out string message)
        {
            DataTable Searchdt = new DataTable();
            message = string.Empty;

            try
            {
                Searchdt = fundtranferDAL.GetTransferToFADubai(model, out message);

                if (Searchdt != null)
                {
                    model.InvGrid = (from rw in Searchdt.AsEnumerable()
                                     select new Entities.InvoiceGrid()
                                     {
                                         Layout = Convert.ToString(rw["Layout"]),
                                         BillNo = Convert.ToString(rw["Transaction Reference"]),
                                         BillDate = rw["Transaction Date"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rw["Transaction Date"]),
                                         Account = Convert.ToString(rw["Transaction Reference"]),
                                         FAClientCode = rw["COA"] == DBNull.Value ? null : Convert.ToString(rw["COA"]),
                                         AccountCode = rw["Account Code"] == DBNull.Value ? null : Convert.ToString(rw["Account Code"]),
                                         FACode = rw["SBU_BL_PRD"] == DBNull.Value ? null : Convert.ToString(rw["SBU_BL_PRD"]),

                                         Description = rw["Description"] == DBNull.Value ? null : Convert.ToString(rw["Description"]),
                                         JobNo = rw["JOB"] == DBNull.Value ? null : Convert.ToString(rw["JOB"]),
                                         Amount = rw["Transaction Amount"] == DBNull.Value ? null : Convert.ToString(rw["Transaction Amount"]),
                                         Currency = rw["Currency Code"] == DBNull.Value ? null : Convert.ToString(rw["Currency Code"]),
                                         Credit_Debit_Marker = rw["Debit/Credit marker"] == DBNull.Value ? null : Convert.ToString(rw["Debit/Credit marker"]),
                                         
                                         CBSRefID = rw["CBSRefID"] == DBNull.Value ? null : Convert.ToString(rw["CBSRefID"]),
                                         InvOrCredit = rw["InvOrCredit"] == DBNull.Value ? null : Convert.ToString(rw["InvOrCredit"]),
                                         BillTo = rw["BillTo"] == DBNull.Value ? null : Convert.ToString(rw["BillTo"]),
                                         
                                     }).ToList();
                }

                return Searchdt;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "FundTransferBL", "GetTransferToFADubai", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public DataTable GetDubaiTAExport(Entities.FundTranfer model, DataTable ExportDt, out string message)
        {
            bool res = false;
            try
            {
                if (ExportDt == null || ExportDt.Rows.Count <= 0)
                {
                    message = "No records to download";
                    return ExportDt;
                }

                if (string.IsNullOrWhiteSpace(model.SearchFor))
                {
                    message = "Status is required.";
                    return ExportDt;
                }
                ExportDt.CaseSensitive = false;
                if (model.SearchFor.ToUpper().Equals("FINALIZED"))
                {
                    var billitem = new XElement("SelectedLists",
                                        from emp in model.InvGrid.Where(x => x.IsExport == true && !string.IsNullOrWhiteSpace(x.CBSRefID))
                                        select new XElement("SelectedList",
                                                       new XElement("CBSRefID", emp.CBSRefID),
                                                       new XElement("InvOrCredit", emp.InvOrCredit),
                                                       new XElement("AccountCode", emp.AccountCode)//,
                                                       //new XElement("FAClientCode", emp.FAClientCode)
                                                   ));

                    res = fundtranferDAL.InsertTransferFA(billitem, UserSession.GetUserSession().LoginID, out message);
                }
                else
                {
                    res = true;
                    message = "Data Exported Successfully.";
                }
                List<string> items = model.InvGrid.Where(x => x.IsExport==true).Select(x => x.BillNo).ToList();
                
                var rows = ExportDt.AsEnumerable().Where(row => items.Contains(row.Field<string>("TRANSACTION REFERENCE")));

                if (rows.Any())
                {
                    ExportDt = new DataTable();
                    ExportDt = rows.CopyToDataTable();

                    if (ExportDt.Columns.Contains("InvOrCredit"))
                        ExportDt.Columns.Remove("InvOrCredit");

                    if (ExportDt.Columns.Contains("CBSRefID"))
                        ExportDt.Columns.Remove("CBSRefID");

                    if (ExportDt.Columns.Contains("BillTo"))
                        ExportDt.Columns.Remove("BillTo");

                    if (ExportDt.Columns.Contains("SrNo"))
                        ExportDt.Columns.Remove("SrNo");

                }

                return ExportDt;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "FundTransferBL", "GetDubaiTAExport", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }


    }
}