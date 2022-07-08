using Newtonsoft.Json;
using RELOCBS.Common;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.GMMS;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.BL.GMMS
{
    public class CostUploadBL
    {

        private CostUploadDAL _costUploadDAL;

        public CostUploadDAL costUploadDAL
        {

            get
            {
                if (this._costUploadDAL == null)
                    this._costUploadDAL = new CostUploadDAL();
                return this._costUploadDAL;
            }
        }

        public bool UploadRate(GMMSRateUpload R, out string result)
        {
            try
            {
                return costUploadDAL.UploadRate(R, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostUploadBL", "UploadRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool UploadPricing(GMMSRateUpload R, out string result)
        {
            try
            {
                return costUploadDAL.UploadPricing(R, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostUploadBL", "UploadRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


        public bool CityRateAdd(AccessorialServicesViewModel data,out string result)
        {
            try
            {
                return costUploadDAL.CityRateAdd( data, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostUploadBL", "UploadRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            
        }

        public bool CityRevenueAdd(AccessServiceAgentList data, out string result)
        {

            return costUploadDAL.CityRevenueAdd(data, UserSession.GetUserSession().LoginID, out result);

        }

        public bool CityCostUpload(CityCostUpload data, out string result)
        {
            try
            {
                return costUploadDAL.CityCostUpload(data, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostUploadBL", "CityCostUpload", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            
        }



        public DataTable StripEmptyRows(DataTable dt)
        {
			//List<int> rowIndexesToBeDeleted = new List<int>();
			//int indexCount = 0;
			//foreach (var row in dt.Rows)
			//{
			//    var r = (DataRow)row;
			//    int emptyCount = 0;
			//    int itemArrayCount = r.ItemArray.Length;
			//    foreach (var i in r.ItemArray) if (string.IsNullOrWhiteSpace(i.ToString())) emptyCount++;

			//    if (emptyCount == itemArrayCount) rowIndexesToBeDeleted.Add(indexCount);

			//    indexCount++;
			//}

			//int count = 0;
			//foreach (var i in rowIndexesToBeDeleted)
			//{
			//    dt.Rows.RemoveAt(i - count);
			//    count++;
			//}
			//		dt = dt.Rows.Cast<DataRow>().
			//Where(row => !row.ItemArray.All(field => field is System.DBNull ||
			//	  string.Compare((field as string).Trim(), string.Empty) ==
			//												  0)).CopyToDataTable();


			for (int i = dt.Rows.Count; i >= 1; i--)
			{
				DataRow currentRow = dt.Rows[i - 1];
				foreach (var colValue in currentRow.ItemArray)
				{
					if (!string.IsNullOrEmpty(colValue.ToString()))
						break;

					// If we get here, all the columns are empty
					dt.Rows[i - 1].Delete();
				}
			}



			dt.AcceptChanges();



			return dt;
        }

        public GMMSRateUpload GetDataForUpload()
        {
            GMMSRateUpload RUobj = new GMMSRateUpload();
            try
            {
                DataTable dt = costUploadDAL.GetDataForUpload(UserSession.GetUserSession().LoginID);

                RUobj.ServiceModeList = (from rw in dt.AsEnumerable()
                                         select new TransportModeList()
                                         {
                                             ModeID = Convert.ToInt32(rw["ID"]),
                                             ModeName = Convert.ToString(rw["Name"]),
                                             //FromCityID = Convert.ToInt32(rw["FromCity"]),
                                             //ToCityID = Convert.ToInt32(rw["ToCity"]),
                                             //EntryPortID = Convert.ToInt32(rw["EntryPort"]),
                                             //ExitPortID = Convert.ToInt32(rw["Exitport"]),
                                             //TransitTimeFrom = Convert.ToInt32(rw["TotEstimate"]),
                                             //TransitTimeTo = Convert.ToInt32(rw["WeightFrom"]),

                                         }).ToList();
                RUobj.THCdtTable = GetTHCWeightSlab();

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "GetDataForUpload", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return RUobj;

        }
     
        public string Validate(GMMSRateUpload R)
        {
            string result = string.Empty;
            try
            {
                

                if (!string.IsNullOrEmpty(R.CostRateList) && !CommonService.ValidateJSON(R.CostRateList))
                {
                    result = result + "-please upload valid file.";
                }
                else
                {

                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(R.CostRateList, (typeof(DataTable)));
                    foreach (DataColumn dc in dt.Columns) // trim column names
                    {
                        dc.ColumnName = dc.ColumnName.Trim();
                    }
                    dt.AcceptChanges();

                    if (dt.Rows.Count > 0)
                    {

                        //if (!CommonService.IsAllColumnExist(dt, new List<String>() {"Mode","WeightSlabFrom","WeightSlabTo"}))
                        //{
                        //    result = result + "-Required first 3 Columns WeightSlabFrom, WeightSlabTo, Mode.";
                        //}
                        //else
                        if (!string.IsNullOrWhiteSpace(R.ModeSelect))
                        {
                            var A = (from names in dt.AsEnumerable() select new { Mode = Convert.ToString(names["Mode"]).ToUpper() }).Where(user => user.Mode.ToUpper() == R.ModeSelect.ToUpper());

                            if (A.Count()<=0)
                            {
                                result = result + string.Format("-Selected {0} mode not available in import table.", R.ModeSelect.ToUpper());
                            }
                        }
                        
                        {

                            var A = (from names in dt.AsEnumerable()
                                     join town in R.ServiceModeList on Convert.ToString(names["Mode"]).ToUpper() equals town.ModeName.ToUpper()
                                     select new
                                     {
                                         Mode = R.RateComponentID == 1 ? town.ModeName : "",
                                         StartPoint = R.RateComponentID == 1 ? town.FromCityID : R.RateComponentID == 2 ? town.ExitPortID : R.RateComponentID == 3 ? town.EntryPortID : R.RateComponentID == 4 ? town.FromCityID : null,
                                         EndPoint = R.RateComponentID == 1 ? town.ExitPortID : R.RateComponentID == 2 ? town.EntryPortID : R.RateComponentID == 3 ? town.ToCityID : R.RateComponentID == 4 ? town.ToCityID : null,
                                         FromTime = town.TransitTimeFrom,
                                         ToTime = town.TransitTimeTo
                                     }).Where(user => user.StartPoint != null && user.StartPoint > 0 && user.EndPoint != null && user.EndPoint > 0).ToList();

                            if (A == null || A.Count() <= 0)
                            {
                                result = result +
                                string.Format("-{0} and {1} is required.",
                                (R.RateComponentID == 1 ? "From City" : R.RateComponentID == 2 ? "Exit Port" : R.RateComponentID == 3 ? "Exit Port" : R.RateComponentID == 4 ? "From City" : ""),
                                (R.RateComponentID == 1 ? "Exit Port" : R.RateComponentID == 2 ? "Entry Port" : R.RateComponentID == 3 ? "To City" : R.RateComponentID == 4 ? "To City" : "")
                                );
                            }
                            else if (A != null && A.Count() > 0)
                            {
                                if (A.Where(a => a.StartPoint > 0 && a.EndPoint > 0 && (a.FromTime == null || a.ToTime == null || a.FromTime <= 0 || a.ToTime <= 0)).Count() > 0)
                                {
                                    result = result + "-Transit FromTime and ToTime is required.";
                                }
                                else if (A.Where(a => a.FromTime != null && a.ToTime != null && a.FromTime > a.ToTime).Count() > 0)
                                {
                                    result = result + "-Transit FromTime must be less than ToTime.";

                                }

                            }

                            if (!string.IsNullOrWhiteSpace(result))
                            {
                                R.dtTable = dt;
                            }
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                result = "Invalid records to upload";
            }
            
            return result;
        }

        public DataTable CalculateAmtToConvertsion(DataTable dt1,double ConversionRate,int RateComponentID,int RMCId)
        {
            DataTable dt = dt1;
            try
            {

                dt = CalculateRateAmountForRMC(dt, RMCId);

                for (int i = 3; i < dt1.Columns.Count; i++)
                {
                    dt1.Rows.Cast<DataRow>().ToList().ForEach(x => x[dt1.Columns[i].ColumnName] = (!string.IsNullOrWhiteSpace(Convert.ToString(x[dt1.Columns[i].ColumnName])) ? Math.Round((double)x[dt1.Columns[i].ColumnName] * ConversionRate, 2) : 0));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid records to upload");
            }

            return dt;
        }

        public int GetBaseCurrByRMC(int RmcId)
        {

            Models.UserInformationModel user = UserSession.GetUserSession();
            try
            {
                bool IRMC = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");
                return new CommanBL().GetBaseCurrByRMC(IRMC, RMCID: RmcId, CompID: user.CompanyID,LoginID: user.LoginID);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostUploadBL", "GetBaseCurrByRMC", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public DataTable CalculateRateAmountForRMC(DataTable dt1,int RMCId)
        {
            DataTable dt = dt1;
            try
            {
                /////1.Check if the RMC for which the Rate/Amount to calculate
                if (!costUploadDAL.GetRMCForRateAmountCalculation(RMCId, UserSession.GetUserSession().LoginID))
                {
                    return dt;
                }
                string AmountColumnName = "amount";
                string RateColumnName = "rate";
                ////2.Check if the Amount and Rate Column exists
                if (!dt1.Columns.Contains(RateColumnName) || !dt1.Columns.Contains(AmountColumnName))
                {
                    return dt;
                }

                ////Calculate the Amount =Rate * Lowest weight or Rate = Amount/ Lowest weight. value with 4 decimal places
                
                foreach (DataRow row in dt.Rows)
                {

                    /// calculate Rate = Amount / Lowest weight
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(row[AmountColumnName])) &&  float.Parse(Convert.ToString(row[AmountColumnName])) > 0.0 && (string.IsNullOrWhiteSpace(Convert.ToString(row[RateColumnName])) || float.Parse(Convert.ToString(row[RateColumnName])) <= 0.0))
                    {
                        row[RateColumnName] = Math.Round(float.Parse(Convert.ToString(row[AmountColumnName])) / float.Parse(Convert.ToString(row[0])),4);
                    }
                    else if(!string.IsNullOrWhiteSpace(Convert.ToString(row[RateColumnName])) && float.Parse(Convert.ToString(row[RateColumnName])) > 0.0 && (string.IsNullOrWhiteSpace(Convert.ToString(row[AmountColumnName])) || float.Parse(Convert.ToString(row[AmountColumnName])) <= 0.0))
                    {
                        row[AmountColumnName] = Math.Round(float.Parse(Convert.ToString(row[RateColumnName])) * float.Parse(Convert.ToString(row[0])), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid data in Rate & Amount Column for Calcualtion");
            }
            return dt;

        }


        public DataTable RemoveAmountColumnForRMC(DataTable dt1, int RMCId)
        {
            DataTable dt = dt1;
            try
            {
                /////1.Check if the RMC for which the Rate/Amount to calculate
                if (!costUploadDAL.GetRMCForRateAmountCalculation(RMCId, UserSession.GetUserSession().LoginID))
                {
                    return dt;
                }

                string AmountColumnName = " amount ";
                string RateColumnName = " rate ";
                ////2.Check if the Amount Column exists
                if (!dt1.Columns.Contains(AmountColumnName))//!dt1.Columns.Contains(RateColumnName) &&
                {
                    return dt;
                }

                ///3.remove the Amount column from table  
                if (dt1.Columns.Contains(AmountColumnName))
                {
                    dt.Columns.Remove(AmountColumnName);
                    return dt;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Invalid Rate & Amount Column to remove");
            }

            return dt;
        }

        public DataTable GetTHCWeightSlab()
        {
            DataTable THCSlabDt = new DataTable();
            Models.UserInformationModel user = UserSession.GetUserSession();
            try
            {
                DataTable slabDt = costUploadDAL.GetTHCWeightSlab(user.LoginID);

                THCSlabDt.Columns.Add("ContinentID", typeof(Int32));
                THCSlabDt.Columns.Add("Continent", typeof(string));

                foreach (DataRow row in slabDt.Rows)
                {
                    THCSlabDt.Columns.Add(row["TransportModeName"].ToString() +"_"+ row["WeightSlab"].ToString(), typeof(string));
                }
                ///Add Empty Row
               ///THCSlabDt.Rows.Add();
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(user.LoginID), "CostUploadBL", "GetTHCWeightSlab", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return THCSlabDt;

        }

        public DataTable GetTHCSlabFromJson(string JsonStr)
        {
            DataTable dt= new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(JsonStr) && CommonService.ValidateJSON(JsonStr))
                {
                    dt = (DataTable)JsonConvert.DeserializeObject(JsonStr, (typeof(DataTable)));
                    foreach (DataColumn dc in dt.Columns) // trim column names
                    {
                        dc.ColumnName = dc.ColumnName.Trim();
                    }
                    dt.AcceptChanges();
                }

                if(dt==null || dt.Columns.Count==0)
                {
                    dt = GetTHCWeightSlab();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostUploadBL", "GetTHCSlabFromJson", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return dt;
        }

        public string GetTHCSlabXml(string JsonStr)
        {
            string SlabXml=string.Empty;   
            try
            {
                DataTable JsonDt = GetTHCSlabFromJson(JsonStr);

                if (JsonDt!=null && JsonDt.Rows.Count>0)
                {
                    DataTable dt = new DataTable("THCValue");
                    dt.Columns.Add("ContinentID", typeof(Int32));
                    dt.Columns.Add("Slab", typeof(Int32));
                    dt.Columns.Add("Amount", typeof(double));
                    dt.Columns.Add("ModeID", typeof(Int32));

                    foreach (DataRow row in JsonDt.Rows)
                    {
                        int ContinentID = Convert.ToInt32(row["ContinentID"]);

                        for (int i = 2; i < JsonDt.Columns.Count; i++)
                        {
                            string ColumnName = JsonDt.Columns[i].ColumnName;
                            string[] Cols = ColumnName.Split('_');
                            double Amount = Convert.ToDouble(row[ColumnName]);
                            if (Cols!=null && Cols.Length>1)
                            {
                                int ModeID = Convert.ToInt32(new ComboBL().GetModeDropdown().FirstOrDefault(x => x.Text.ToLower().Equals(Cols[0].ToLower())).Value);
                                int Slab = Convert.ToInt32(Cols[1]);
                                dt.Rows.Add(new Object[]{ContinentID,Slab,Amount,ModeID});
                            }
                        }

                    }

                    if (dt.Rows.Count>0)
                    {
                        //MemoryStream str = new MemoryStream();
                        //dt.WriteXml(str, true);
                        //str.Seek(0, SeekOrigin.Begin);
                        //StreamReader sr = new StreamReader(str);
                        //string xmlstring = "";
                        //xmlstring = sr.ReadToEnd();
                        //return (xmlstring);

                        var xEle = new XElement("THCValues",
                        from emp in dt.AsEnumerable()
                        select new XElement("THCValue",
                                     new XElement("ContinentID", Convert.ToString(emp["ContinentID"])),
                                     new XElement("Slab", Convert.ToString(emp["Slab"])),
                                     new XElement("Amount", Convert.ToString(emp["Amount"])),
                                     new XElement("ModeID", Convert.ToString(emp["ModeID"]))
                                   ));

                        SlabXml= xEle.ToString();
                    }
                }

                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostUploadBL", "GetTHCSlabForSave", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return SlabXml;

        }
    }

}