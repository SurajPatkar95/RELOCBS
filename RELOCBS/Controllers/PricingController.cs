using Newtonsoft.Json;
using RELOCBS.AjaxHelper;
using RELOCBS.BL;
using RELOCBS.BL.Pricing;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RELOCBS.Controllers
{
	[AuthorizeUser]
	public class PricingController : BaseController
	{

		private PricingBL _pricingBL;

		public PricingBL pricingBL
		{
			get
			{

				if (_pricingBL == null)
				{
					_pricingBL = new PricingBL();
				}

				return _pricingBL;

			}
		}

		private ComboBL _comobBL;

		public ComboBL comboBL
		{
			get
			{

				if (_comobBL == null)
				{
					_comobBL = new ComboBL();
				}

				return _comobBL;

			}
		}

		// GET: Pricing
		public ActionResult Index(int OriginId, int DestinationId, int RMCId, bool IsRoad)
		{
			session.Set<string>("PageSession", "Pricing");
			RMCPricing model = new RMCPricing();

			model.FromCityId = OriginId;
			model.ToCityId = DestinationId;
			model.RMCID = RMCId;
			model.IsRoad = IsRoad;

			model.CalculationMethod = UserSession.GetUserSession().CompanyID==2 ? 2 : 1;
			model.NormalRev = "Rev";
			GetPricingBuffer(model.RMCID, model.IsRoad, ref model);
			GetFixedRateCharges(model.RMCID, ref model);
			DataTable dtAgentCombo = new DataTable();
			DataTable dtOriginal = new DataTable();

			FillDropDown();
			model.Origin = comboBL.GetCityDropdown(SPTYPE: "Single", CityID: OriginId).FirstOrDefault().Text;
			model.Destination = comboBL.GetCityDropdown(SPTYPE: "Single", CityID: DestinationId).FirstOrDefault().Text;
			bool bufferflag = false;
			model.PricingCombinations = pricingBL.GetPricingCombination(ref dtAgentCombo, ref dtOriginal, model, bufferflag, IsRoad);
			model.AgentCombination = dtAgentCombo;
			FillDynamicCombo(dtAgentCombo);
			TempData["OriginalData"] = JsonConvert.SerializeObject(dtOriginal);
			TempData["ModelData"] = JsonConvert.SerializeObject(model);
			return View(model);
		}

		[HttpPost]
		public ActionResult Index(RMCPricing model, int OriginId, int DestinationId, int RMCId, bool IsRoad)
		{

			DataTable dtAgent = new DataTable();
			DataTable dtOriginal = new DataTable();

			model.PricingCombinations = pricingBL.GetPricingCombination(ref dtAgent, ref dtOriginal, model, true, IsRoad);
			model.AgentCombination = dtAgent;
			FillDynamicCombo(dtAgent);
			FillDropDown();
			TempData["OriginalData"] = JsonConvert.SerializeObject(dtOriginal);
			TempData["ModelData"] = JsonConvert.SerializeObject(model);
			ViewData["OriginVendorListSelected"] = model.OrgVendorID;
			ViewData["DestVendorListSelected"] = model.DestVendorID;

			model.CombinationNo = null;
			return View(model);
		}

		private void GetPricingBuffer(int RMCId, bool IsRoad, ref RMCPricing model)
		{
			DataTable ds = pricingBL.GetPricingBuffer(RMCId);
			if (ds != null && ds.Rows.Count > 0)
			{
				if (IsRoad)
				{
					if (ds.Select("Modeid=3") != null && ds.Select("Modeid=3").Count() > 0)
					{
						model.PricingSeaBuffer = ds.Select("Modeid=3").CopyToDataTable().AsEnumerable()
						.Select(dataRow => new PricingBuffer
						{
							ModeId = Convert.ToInt32(dataRow["Modeid"]),
							BufferSlab = Convert.ToString(dataRow["BufferSlab"]),
							BufferCost = 0
						}).ToList();
					}
				}
				else
				{
					if (ds.Select("Modeid=1") != null && ds.Select("Modeid=1").Count() > 0)
					{
						model.PricingSeaBuffer = ds.Select("Modeid=1").CopyToDataTable().AsEnumerable()
						.Select(dataRow => new PricingBuffer
						{
							ModeId = Convert.ToInt32(dataRow["Modeid"]),
							BufferSlab = Convert.ToString(dataRow["BufferSlab"]),
							BufferCost = 0
						}).ToList();
					}
					if (ds.Select("Modeid=2") != null && ds.Select("Modeid=2").Count() > 0)
					{
						model.PricingAirBuffer = ds.Select("Modeid=2").CopyToDataTable().AsEnumerable()
						.Select(dataRow => new PricingBuffer
						{
							ModeId = Convert.ToInt32(dataRow["Modeid"]),
							BufferSlab = Convert.ToString(dataRow["BufferSlab"]),
							BufferCost = 0
						}).ToList();
					}
				}
			}
		}

		private void GetFixedRateCharges(int RMCId, ref RMCPricing model)
		{
			DataTable ds = pricingBL.GetFixedRateCharges(RMCId);
			if (ds != null && ds.Rows.Count > 0)
			{
				model.RMCFees = ds.AsEnumerable()
					.Select(dataRow => new RmcFees
					{
						BAFlag = Convert.ToChar(dataRow["AddBeforeOrAfteSFR"]),
						CostHeadId = Convert.ToString(dataRow["AddCostForBillId"]),
						CostHeadName = Convert.ToString(dataRow["CostHeadName"]),
						Amount = dataRow["AmtToAdd"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["AmtToAdd"]),
						Percent = dataRow["PercentToAdd"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["PercentToAdd"])
					}).ToList();
			}
		}

		public ActionResult SFRCalculation(string CombinationId)
		{
			DataTable dt = new DataTable();
			TempData.Keep("OriginalData");
			TempData.Keep("ModelData");
			ViewBag.IsView = false;
			dt = ((DataTable)JsonConvert.DeserializeObject(Convert.ToString(TempData["OriginalData"]), (typeof(DataTable)))).Select("UniqId2 ='" + CombinationId + "'").CopyToDataTable();
			RMCPricing model = ((RMCPricing)JsonConvert.DeserializeObject(Convert.ToString(TempData["ModelData"]), (typeof(RMCPricing))));
			model.CombinationNo = CombinationId;
			model.SFRCalculationList = dt.AsEnumerable().Select(
				dataRow => new SFRCaculationList
				{
					UniqID = Convert.ToString(dataRow["UniqId2"]),
					AgentsDetail = Convert.ToString(dataRow["AgentsDetail"]),
					WeightFrom = Convert.ToString(dataRow["WeightFrom"]),
					CostVal = Convert.ToString(dataRow["CostVal"]),
					OrgCost = Convert.ToString(dataRow["OrgCost"]),
					FrtCost = Convert.ToString(dataRow["FrtCost"]),
					DestCost = Convert.ToString(dataRow["DestCost"]),
					DtDCost = Convert.ToString(dataRow["DtDCost"]),
					Buff = Convert.ToString(dataRow["Buff"]),
					SFRAmt = Convert.ToString(dataRow["SFRAmt"]),
					SFR = Convert.ToString(dataRow["SFR"]),
					RevSFR = Convert.ToString(dataRow["RevSFR"]),
					FSFR = Convert.ToString(dataRow["FSFR"]),
					FSFRAmt = Convert.ToString(dataRow["FSFRAmt"]),
					NetRev = Convert.ToString(dataRow["NetRev"]),
					GPVal = Convert.ToString(dataRow["GPVal"]),
					GPPercent = Convert.ToString(dataRow["GPPercent"]),
					TransitFrom = Convert.ToString(dataRow["TransitFrom"]),
					TransitTo = Convert.ToString(dataRow["TransitTo"]),
					GrpRefID = Convert.ToString(dataRow["GrpRefID"]),
					GrpRefId = Convert.ToString(dataRow["GrpRefId"]),
					OriginPort = Convert.ToString(dataRow["OriginPort"]),
					DestPort = Convert.ToString(dataRow["DestPort"]),
					OriginVendor = Convert.ToString(dataRow["OriginVendor"]),
					DestVendor = Convert.ToString(dataRow["DestVendor"]),
					TransitTime = Convert.ToString(dataRow["TransitTime"]),
					ModeID = Convert.ToString(dataRow["ModeID"]),
				}).ToList();
			TempData["ModelData"] = JsonConvert.SerializeObject(model);
			return PartialView("SFRListGrid", model);
		}

		private void FillDropDown()
		{
			ViewData["CurencyList"] = comboBL.GetCurrencyDropdown();
			ViewData["CalculationMethodList"] = comboBL.GetSFRCalculationMethodDropdown(false);
			ViewBag.CalculationMappingList = comboBL.GetSFRCalculationMethodDropdown(true);
			//ViewData["Company"] = comboBL.GetCompanyDropdown(CompanyID: UserSession.GetUserSession().CompanyID, SPTYPE: "SINGLE").FirstOrDefault().Text;
		}

		private void FillDynamicCombo(DataTable dtAgentCombo)
		{
			var AgentList = dtAgentCombo.DefaultView.ToTable(true, "OrigAgent").AsEnumerable();
			IEnumerable<SelectListItem> OriginVendorList = (IEnumerable<SelectListItem>)AgentList.Select(s => new SelectListItem()
			{
				Value = s.Field<string>("OrigAgent"),
				Text = s.Field<string>("OrigAgent")
			}).ToList();
			ViewData["OriginVendorList"] = OriginVendorList;
			AgentList = dtAgentCombo.DefaultView.ToTable(true, "DestAgent").AsEnumerable();
			//IEnumerable<SelectListItem> DestVendorList = (IEnumerable<SelectListItem>)dtAgentCombo.DefaultView.ToTable(true, "DestVendor").AsEnumerable()
			IEnumerable<SelectListItem> DestVendorList = (IEnumerable<SelectListItem>)AgentList.Select(s => new SelectListItem()
			{
				Value = s.Field<string>("DestAgent"),
				Text = s.Field<string>("DestAgent")
			}).ToList();
			ViewData["DestVendorList"] = DestVendorList;
		}

		[HttpPost]
		public ActionResult SaveAmendRates(RMCPricing model)
		{
			TempData.Keep("OriginalData");
			TempData.Keep("ModelData");
			RMCPricing model_rmc = ((RMCPricing)JsonConvert.DeserializeObject(Convert.ToString(TempData["ModelData"]), (typeof(RMCPricing))));
			//model_rmc.ModeList = new string[] { "1", "2" };
			model_rmc.SFRGridList = model.SFRGridList;

			bool success = pricingBL.SaveAmendRates(model_rmc);
			if (success)
			{
				this.AddToastMessage("RELOCBS", "Data saved successfully.", ToastType.Success);
				return RedirectToAction("Create", "Lead");
			}
			else
			{
				this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
				return RedirectToAction("Index", "Pricing", new { OriginId = model.FromCityId, DestinationId = model.ToCityId, RMCId = model.RMCID, IsRoad = model.IsRoad });
			}
		}
		[HttpGet]
		public ActionResult ImportRateUpload()
		{
			ViewData["CurrencyList"] = comboBL.GetCurrencyDropdown("ALL");
			ViewData["CurrencyId"] = 1;

			ViewData["OConversionRateToUSD"] = 1;
			ViewData["FConversionRateToUSD"] = 1;
			ViewData["DConversionRateToUSD"] = 1;
			ViewData["BConversionRateToUSD"] = 1;
			ViewData["AConversionRateToUSD"] = 1;

			//ViewData["PSConversionRateToUSD"] = 1;
			//ViewData["RConversionRateToUSD"] = 1;
			//ViewData["BOConversionRateToUSD"] = 1;
			//ViewData["BFConversionRateToUSD"] = 1;
			//ViewData["RBOConversionRateToUSD"] = 1;
			//ViewData["RBFConversionRateToUSD"] = 1;
			//ViewData["TYPE"] = _spService.BindDropdown("Mode", "", "");
			RateUpload R = new RateUpload();
			RateUploadall RU = new RateUploadall();
			RU.uploadType = 1;
			R.ShippmentMode = "S";
			R.RateUploadall = RU;
			return Request.IsAjaxRequest()
			? (ActionResult)PartialView("ImportRateUpload", R)
			: View(R);
		}

		[HttpPost]
		public ActionResult ImportRateUpload(HttpPostedFileBase file, RateUpload R)
		{
			DataTable dt1 = new DataTable();
			DataSet ds = new DataSet();

			ViewBag.RateTypeFlagValue = R.RateTypeFlagValue;
			ViewBag.ShippmentMode = R.ShippmentMode;
			ViewData["CurrencyList"] = comboBL.GetCurrencyDropdown("ALL");
			HttpPostedFileBase FileRate = null;

			if (R.RateUploadall.uploadType == 1 || R.RateUploadall.uploadType == 0)
			{
				ViewData["CurrencyId"] = R.OriginRate.CurrencyID;
				ViewData["OConversionRateToUSD"] = R.OriginRate.ConversionRateToUSD;
				FileRate = Request.Files[0];
			}
			else if (R.RateUploadall.uploadType == 2)
			{
				ViewData["CurrencyId"] = R.FreightRate.CurrencyID;
				ViewData["FConversionRateToUSD"] = R.FreightRate.ConversionRateToUSD;
				FileRate = Request.Files[1];
			}
			else if (R.RateUploadall.uploadType == 3)
			{
				ViewData["CurrencyId"] = R.DestinationRate.CurrencyID;
				ViewData["DConversionRateToUSD"] = R.DestinationRate.ConversionRateToUSD;
				FileRate = Request.Files[2];
			}
			else if (R.RateUploadall.uploadType == 4)
			{
				ViewData["CurrencyId"] = R.BlanketRate.CurrencyID;
				ViewData["BConversionRateToUSD"] = R.BlanketRate.ConversionRateToUSD;
				FileRate = Request.Files[3];
			}
			else if (R.RateUploadall.uploadType == 5)
			{
				ViewData["CurrencyId"] = R.AccesRate.CurrencyID;
				ViewData["AConversionRateToUSD"] = R.AccesRate.ConversionRateToUSD;
				FileRate = Request.Files[4];
			}

			if (FileRate.ContentLength > 0)
			{
				string fileExtension = System.IO.Path.GetExtension(FileRate.FileName);

				if (fileExtension == ".xls" || fileExtension == ".xlsx")
				{
					string fileLocation = Server.MapPath("~/uploads/") + FileRate.FileName;
					if (System.IO.File.Exists(fileLocation))
					{
						System.IO.File.SetAttributes(fileLocation, FileAttributes.Normal);
						//   System.IO.File.Delete(fileLocation);
					}
					FileRate.SaveAs(fileLocation);

					string excelConnectionString = string.Empty;
					excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
					fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
					//connection String for xls file format.
					if (fileExtension == ".xls")
					{
						excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
						fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
					}
					//connection String for xlsx file format.
					else if (fileExtension == ".xlsx")
					{
						excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
						fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
					}
					DataTable dt = new DataTable();

					//Create Connection to Excel work book and add oledb namespace
					using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
					{
						excelConnection.Open();

						dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
						if (dt == null)
						{
							return null;
						}

						String[] excelSheets = new String[dt.Rows.Count];
						int t = 0;
						//excel data saves in temp file here.
						foreach (DataRow row in dt.Rows)
						{
							excelSheets[t] = row["TABLE_NAME"].ToString();
							t++;
						}

						string query = string.Empty;

						if (R.RateUploadall.uploadType == 1 || R.RateUploadall.uploadType == 3)
						{
							query = string.Format("Select top 24 * from [{0}]", excelSheets[0]);
						}
						else if (R.RateUploadall.uploadType == 2)
						{
							query = string.Format("Select top 15 * from [{0}]", excelSheets[0]);
						}
						//else if (R.RateUploadall.uploadType == 6)
						//{
						//    query = string.Format("Select * from [{0}]", excelSheets[0]);
						//}
						else if (R.RateUploadall.uploadType == 4)
						{
							query = string.Format("Select top 18 * from [{0}]", excelSheets[0]);
						}
						else
						{
							query = string.Format("Select * from [{0}]", excelSheets[0]);
						}

						using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
						{
							dataAdapter.Fill(ds);
						}
						if (excelConnection.State == ConnectionState.Open)
						{
							excelConnection.Close();
						}
					}

				}
				if (fileExtension.ToString().ToLower().Equals(".xml"))
				{
					string fileLocation = Server.MapPath("~/uploads/") + FileRate.FileName;
					if (System.IO.File.Exists(fileLocation))
					{
						System.IO.File.Delete(fileLocation);
					}
					FileRate.SaveAs(fileLocation);
					XmlTextReader xmlreader = new XmlTextReader(fileLocation);
					ds.ReadXml(xmlreader);
					xmlreader.Close();
				}
				dt1 = ds.Tables[0] as DataTable;

				if (R.RateUploadall.uploadType == 1 || R.RateUploadall.uploadType == 0)
				{
					R.OriginRate.dtTable = pricingBL.CalculateAmtToUSD(dt1, R.OriginRate.ConversionRateToUSD, R.RateUploadall.uploadType);

					DataTable dtOriginSample = pricingBL.GetProperWeightSlab(R.RateUploadall.uploadType);

					var missedWeightSlabList = dtOriginSample.AsEnumerable().Select(r => r.Field<string>("WeightSlab"))
									 .Except(dt1.AsEnumerable().Select(r => r.Field<string>("WeightSlab")));

					if (missedWeightSlabList.Count() > 0)
					{
						R.OriginRate.dtTable = null;
						ModelState.AddModelError(string.Empty, "Please select proper template for Origin");
					}

				}
				else if (R.RateUploadall.uploadType == 2)
				{
					R.FreightRate.dtTable = pricingBL.CalculateAmtToUSD(dt1, R.FreightRate.ConversionRateToUSD, R.RateUploadall.uploadType);
					DataTable dtFreightSample = pricingBL.GetProperWeightSlab(R.RateUploadall.uploadType);

					var missedWeightSlabList = dtFreightSample.AsEnumerable().Select(r => r.Field<string>("WeightSlab"))
									 .Except(dt1.AsEnumerable().Select(r => r.Field<string>("WeightSlab")));

					if (missedWeightSlabList.Count() > 0)
					{
						R.FreightRate.dtTable = null;
						ModelState.AddModelError(string.Empty, "Please select proper template for Freight");
					}
				}
				else if (R.RateUploadall.uploadType == 3)
				{
					R.DestinationRate.dtTable = pricingBL.CalculateAmtToUSD(dt1, R.DestinationRate.ConversionRateToUSD, R.RateUploadall.uploadType);

					DataTable dtDestinationSample = pricingBL.GetProperWeightSlab(R.RateUploadall.uploadType);

					var missedWeightSlabList = dtDestinationSample.AsEnumerable().Select(r => r.Field<string>("WeightSlab"))
									 .Except(dt1.AsEnumerable().Select(r => r.Field<string>("WeightSlab")));

					if (missedWeightSlabList.Count() > 0)
					{
						R.DestinationRate.dtTable = null;
						ModelState.AddModelError(string.Empty, "Please select proper template for Destination");
					}

				}
				else if (R.RateUploadall.uploadType == 4)
				{
					if (!dt1.Columns.Contains("WeightSlabID"))
					{
						dt1.Columns.Add("WeightSlabID", typeof(int)).SetOrdinal(0);
					}
					R.BlanketRate.dtTable = pricingBL.CalculateAmtToUSD(dt1, R.BlanketRate.ConversionRateToUSD, R.RateUploadall.uploadType);

					DataTable dtBlanketSample = pricingBL.GetProperWeightSlab(R.RateUploadall.uploadType);

					var missedWeightSlabList = dtBlanketSample.AsEnumerable().Select(r => r.Field<string>("WeightSlab"))
									 .Except(dt1.AsEnumerable().Select(r => r.Field<string>("WeightSlab")));

					if (missedWeightSlabList.Count() > 0)
					{
						R.BlanketRate.dtTable = null;
						ModelState.AddModelError(string.Empty, "Please select proper template for Blanket Rate");
					}

				}


			}
			// ADD BY MINAL
			else
			{
				if (R.RateUploadall.uploadType == 5)
				{
					//R.AccesRate.dtTable = _spLeadService.GetAdditionalServicesDetails(R.RateUploadall.RMCID);
				}

			}
			// END ADD BY MINAL
			// ViewData["RateType"] = R.PermanentStorageRate.RateType;
			return Request.IsAjaxRequest() ? (ActionResult)PartialView("ImportRateUpload", R) : View(R);
		}

		//public ActionResult SaveBasicPricingData(List<BasicPricingData> BasicPricingData, int? leadid, int? RaterequestID, int? RaterequestVendorID)
		//{
		//    AjaxResponse result = new AjaxResponse();
		//    int res = 0;

		//    res = _spLeadService.InsertBasicPricingData(BasicPricingData); // to do                
		//    if (res < 0)
		//    {
		//        result.Success = false;
		//        ModelState.AddModelError(string.Empty, "Unable to save pricing data.");
		//        return Json(result);
		//    }

		//    result.Success = false;
		//    return Json(result);
		//}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult SaveOriginRate(List<SaveOriginRate> SaveOriginRate,
										   int? RMCID, int? VendorID,
										   string CityID, string SeaPortID, string AirPortID, string FromDate,
										   int? DTPSea, int? DTPAir, int? DTPSeaMax, int? DTPAirMax,
										   int AgentCurrencyId, decimal ConversionRateToUSD)
		{
			AjaxResponse result = new AjaxResponse();
			int valid = 1;
			RateUpload R = new RateUpload();
			OriginRate OR = new OriginRate();
			RateUploadall RU = new RateUploadall();
			R.OriginRate = OR;
			RU.uploadType = 1;
			R.RateUploadall = RU;
			try
			{
				if (!string.IsNullOrWhiteSpace(AirPortID)) { R.OriginRate.AirPortId = Convert.ToInt32(AirPortID); }

				if (!string.IsNullOrWhiteSpace(SeaPortID)) { R.OriginRate.SeaPortId = Convert.ToInt32(SeaPortID); }

				R.OriginRate.CityID = Convert.ToInt32(CityID);

				if (DTPSea != 0) { R.OriginRate.DTPSea = Convert.ToInt32(DTPSea); }

				if (DTPAir != 0) { R.OriginRate.DTPAir = Convert.ToInt32(DTPAir); }

				if (DTPSeaMax != 0) { R.OriginRate.DTPSeaMax = Convert.ToInt32(DTPSeaMax); }

				if (DTPAirMax != 0) { R.OriginRate.DTPAirMax = Convert.ToInt32(DTPAirMax); }

				if (AgentCurrencyId != 0) { R.OriginRate.CurrencyID = Convert.ToInt32(AgentCurrencyId); }
				else { R.OriginRate.CurrencyID = 1; }

				if (ConversionRateToUSD > 0) { R.OriginRate.ConversionRateToUSD = Convert.ToDecimal(ConversionRateToUSD); }
				else { R.OriginRate.ConversionRateToUSD = 1; }

				R.RateUploadall.RMCID = Convert.ToInt32(RMCID);
				R.RateUploadall.VendorId = Convert.ToInt32(VendorID);
				R.OriginRate.FromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			}
			catch { }

			if (valid == 1)
			{
				bool res = false;
				res = pricingBL.InsertOriginRate(SaveOriginRate, R, UserSession.GetUserSession().LoginID);
				if (!res)
				{
					result.Success = false;
					ModelState.AddModelError(string.Empty, "Unable to save pricing data.");
					return Json(result);
				}
				else
				{
					result.Success = true;
					return Json(result);
				}
			}
			else
			{
				return Request.IsAjaxRequest() ? (ActionResult)PartialView("ImportRateUpload")
			  : View();
			}
		}

		public JsonResult updateVendorrates()
		{
			AjaxResponse result = new AjaxResponse();
			bool i = pricingBL.UpdateVendorrate(1, UserSession.GetUserSession().LoginID);
			if (!i)
			{
				result.Success = false;
				ModelState.AddModelError(string.Empty, "Unable to save update agent rate data.");
				return Json(result);
			}
			else
			{
				result.Success = true;
				return Json(result);
			}

			//return Json(JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult SaveDestinationRate(List<SaveDestinationRate> SaveDestinationRate,
												int? RMCID, int? VendorID,
												string CityID, string SeaPortID, string AirPortID, string FromDate,
												int? PTDSea, int? PTDAir, int? PTDSeaMax, int? PTDAirMax,
												int AgentCurrencyId, decimal ConversionRateToUSD)
		{
			AjaxResponse result = new AjaxResponse();
			int valid = 1;
			RateUpload R = new RateUpload();
			DestinationRate DR = new DestinationRate();
			RateUploadall RU = new RateUploadall();
			R.DestinationRate = DR;
			RU.uploadType = 2;
			R.RateUploadall = RU;

			try
			{
				if (AirPortID.Trim() != "")
				{
					R.DestinationRate.AirPortId = Convert.ToInt32(AirPortID);
				}
				if (SeaPortID.Trim() != "")
				{
					R.DestinationRate.SeaPortId = Convert.ToInt32(SeaPortID);
				}
				R.DestinationRate.CityID = Convert.ToInt32(CityID);
				if (PTDSea != 0)
				{
					R.DestinationRate.PTDSea = Convert.ToInt32(PTDSea);
				}
				if (PTDAir != 0)
				{
					R.DestinationRate.PTDAir = Convert.ToInt32(PTDAir);
				}
				if (PTDSeaMax != 0)
				{
					R.DestinationRate.PTDSeaMax = Convert.ToInt32(PTDSeaMax);
				}
				if (PTDAirMax != 0)
				{
					R.DestinationRate.PTDAirMax = Convert.ToInt32(PTDAirMax);
				}

				if (AgentCurrencyId != 0) { R.DestinationRate.CurrencyID = Convert.ToInt32(AgentCurrencyId); }
				else { R.DestinationRate.CurrencyID = 1; }

				if (ConversionRateToUSD > 0) { R.DestinationRate.ConversionRateToUSD = Convert.ToDecimal(ConversionRateToUSD); }
				else { R.DestinationRate.ConversionRateToUSD = 1; }

				R.RateUploadall.RMCID = Convert.ToInt32(RMCID);
				R.RateUploadall.VendorId = Convert.ToInt32(VendorID);
				R.DestinationRate.FromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			}
			catch { }

			if (valid == 1)
			{
				bool res = false;
				res = pricingBL.InsertDestinationRate(SaveDestinationRate, R, UserSession.GetUserSession().LoginID);

				if (!res)
				{
					result.Success = false;
					ModelState.AddModelError(string.Empty, "Unable to save pricing data.");
					return Json(result);
				}
				else
				{
					result.Success = true;
					return Json(result);
				}
			}
			else
			{
				return Request.IsAjaxRequest() ? (ActionResult)PartialView("ImportRateUpload")
			  : View();
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult SaveFreightRate(List<SaveFreightRate> SaveFreightRate,
											int? RMCID, int? VendorID,
											string OrgPortIDSea, string OrgPortIDAir,
											string DestPortIDSea, string DestPortIDAir, string FromDate,
											int? PTPSea, int? PTPAir, int? PTPSeaMax, int? PTPAirMax,
											int AgentCurrencyId, decimal ConversionRateToUSD)
		{
			AjaxResponse result = new AjaxResponse();
			int valid = 1;
			RateUpload R = new RateUpload();
			FreightRate FR = new FreightRate();
			RateUploadall RU = new RateUploadall();
			R.FreightRate = FR;
			RU.uploadType = 4;
			R.RateUploadall = RU;
			try
			{
				if (OrgPortIDSea.Trim() != "")
				{
					R.FreightRate.OriginSeaPortId = Convert.ToInt32(OrgPortIDSea);
				}
				else
				{
					valid = 0;
				}
				if (OrgPortIDAir.Trim() != "")
				{
					R.FreightRate.OriginAirPortId = Convert.ToInt32(OrgPortIDAir);
				}
				else
				{
					valid = 0;
				}
				if (DestPortIDSea.Trim() != "")
				{
					R.FreightRate.DestSeaPortId = Convert.ToInt32(DestPortIDSea);
				}
				else
				{
					valid = 0;
				}
				if (DestPortIDAir.Trim() != "")
				{
					R.FreightRate.DestAirPortId = Convert.ToInt32(DestPortIDAir);
				}
				else
				{
					valid = 0;
				}
				if (PTPSea != 0)
				{
					R.FreightRate.PTPSea = Convert.ToInt32(PTPSea);
				}
				if (PTPSeaMax != 0)
				{
					R.FreightRate.PTPSeaMax = Convert.ToInt32(PTPSeaMax);
				}
				if (PTPAir != 0)
				{
					R.FreightRate.PTPAir = Convert.ToInt32(PTPAir);
				}
				if (PTPAirMax != 0)
				{
					R.FreightRate.PTPAirMax = Convert.ToInt32(PTPAirMax);
				}

				if (AgentCurrencyId != 0) { R.FreightRate.CurrencyID = Convert.ToInt32(AgentCurrencyId); }
				else { R.FreightRate.CurrencyID = 1; }

				if (ConversionRateToUSD > 0) { R.FreightRate.ConversionRateToUSD = Convert.ToDecimal(ConversionRateToUSD); }
				else { R.FreightRate.ConversionRateToUSD = 1; }

				R.RateUploadall.RMCID = Convert.ToInt32(RMCID);
				R.RateUploadall.VendorId = Convert.ToInt32(VendorID);
				R.FreightRate.FromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			}
			catch { }

			if (valid == 1)
			{
				bool res = false;
				res = pricingBL.InsertFreightRate(SaveFreightRate, R, UserSession.GetUserSession().LoginID);
				if (!res)
				{
					result.Success = false;
					ModelState.AddModelError(string.Empty, "Unable to save pricing data.");
					return Json(result);
				}
				else
				{
					result.Success = true;
					return Json(result);
				}
			}
			else
			{
				return Request.IsAjaxRequest() ? (ActionResult)PartialView("ImportRateUpload")
			  : View();
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult SaveBlanketRate(List<SaveBlanketRate> SaveBlanketRate,
											int VendorID, int RMCID, int FromCityID, int ToCityID,
											string FromDate, int DTPMin, int DTPMax,
											int CurrencyID, decimal? ConversionRateToUSD)
		{
			AjaxResponse result = new AjaxResponse();
			int valid = 1;

			RateUpload R = new RateUpload();
			BlanketRate OR = new BlanketRate();
			RateUploadall RU = new RateUploadall();
			R.BlanketRate = OR;
			RU.uploadType = 4;
			R.RateUploadall = RU;

			try
			{
				if (FromCityID != 0) { R.BlanketRate.FromCityID = Convert.ToInt32(FromCityID); }
				//else { valid = 0; }

				if (ToCityID != 0) { R.BlanketRate.ToCityID = Convert.ToInt32(ToCityID); }
				//else { valid = 0; }

				if (DTPMin != 0) { R.BlanketRate.DTDMin = Convert.ToInt32(DTPMin); }
				//else { valid = 0; }

				if (DTPMax != 0) { R.BlanketRate.DTDMax = Convert.ToInt32(DTPMax); }
				//else { valid = 0; }

				if (CurrencyID != 0) { R.BlanketRate.CurrencyID = Convert.ToInt32(CurrencyID); }
				else { R.BlanketRate.CurrencyID = 1; }

				if (ConversionRateToUSD.HasValue) { R.BlanketRate.ConversionRateToUSD = Convert.ToDecimal(ConversionRateToUSD); }
				else { R.BlanketRate.ConversionRateToUSD = 1; }

				R.RateUploadall.RMCID = Convert.ToInt32(RMCID);
				R.RateUploadall.VendorId = Convert.ToInt32(VendorID);
				R.BlanketRate.FromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			}
			catch { }

			if (valid == 1)
			{
				bool res = false;
				res = pricingBL.InsertBlanketRate(SaveBlanketRate, R, UserSession.GetUserSession().LoginID);
				if (!res)
				{
					result.Success = false;
					ModelState.AddModelError(string.Empty, "Unable to save pricing data.");
					return Json(result);
				}
				else
				{
					result.Success = true;
					return Json(result);
				}
			}
			else
			{
				return Request.IsAjaxRequest() ? (ActionResult)PartialView("ImportRateUpload") : View();
			}

		}

		public ActionResult JobSFRCalculation(int UpdatedBatchId)
		{
			ViewBag.IsView = true;
			RMCPricing model = new RMCPricing();
			DataTable dt = new DataTable();
			dt = pricingBL.GetPricingByJob(UpdatedBatchId);
			//TempData.Keep("OriginalData");
			//TempData.Keep("ModelData");
			//dt = ((DataTable)JsonConvert.DeserializeObject(Convert.ToString(TempData["OriginalData"]), (typeof(DataTable)))).Select("UniqId2 ='" + CombinationId + "'").CopyToDataTable();
			//RMCPricing model = ((RMCPricing)JsonConvert.DeserializeObject(Convert.ToString(TempData["ModelData"]), (typeof(RMCPricing))));
			//model.CombinationNo = CombinationId;
			model.SFRCalculationList = dt.AsEnumerable().Select(
				dataRow => new SFRCaculationList
				{
					//UniqID = Convert.ToString(dataRow["UniqId2"]),
					//AgentsDetail = Convert.ToString(dataRow["AgentsDetail"]),
					WeightFrom = Convert.ToString(dataRow["WeightFrom"]),
					//CostVal = Convert.ToString(dataRow["CostVal"]),
					OrgCost = Convert.ToString(dataRow["OrgCost"]),
					FrtCost = Convert.ToString(dataRow["FrtCost"]),
					DestCost = Convert.ToString(dataRow["DestCost"]),
					DtDCost = Convert.ToString(dataRow["DtDCost"]),
					Buff = Convert.ToString(dataRow["Buff"]),
					SFRAmt = Convert.ToString(dataRow["SFRAmt"]),
					SFR = Convert.ToString(dataRow["SFR"]),
					RevSFR = Convert.ToString(dataRow["RevSFR"]),
					FSFR = Convert.ToString(dataRow["FSFR"]),
					FSFRAmt = Convert.ToString(dataRow["FSFRAmt"]),
					NetRev = Convert.ToString(dataRow["NetRev"]),
					GPVal = Convert.ToString(dataRow["GPVal"]),
					GPPercent = Convert.ToString(dataRow["GPPercent"]),
					TransitFrom = Convert.ToString(dataRow["TransitFrom"]),
					TransitTo = Convert.ToString(dataRow["TransitTo"]),
					//GrpRefID = Convert.ToString(dataRow["GrpRefID"]),
					//GrpRefId = Convert.ToString(dataRow["GrpRefId"]),
					//OriginPort = Convert.ToString(dataRow["OriginPort"]),
					//DestPort = Convert.ToString(dataRow["DestPort"]),
					//OriginVendor = Convert.ToString(dataRow["OriginVendor"]),
					//DestVendor = Convert.ToString(dataRow["DestVendor"]),
					//TransitTime = Convert.ToString(dataRow["TransitTime"]),
					//ModeID = Convert.ToString(dataRow["ModeID"]),
				}).ToList();
			//TempData["ModelData"] = JsonConvert.SerializeObject(model);
			return PartialView("SFRListGrid", model);
		}
	}
}