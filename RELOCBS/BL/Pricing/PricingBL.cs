﻿using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.Pricing
{
    public class PricingBL
    {

        public DataTable GetProperWeightSlab(int UploadType)
        {
            DataTable dtweightslab = new DataTable();
            dtweightslab.Columns.Add("WeightSlab", typeof(string));

            if (UploadType == 1 || UploadType == 3)
            {
                dtweightslab.Rows.Add("1000-1999");
                dtweightslab.Rows.Add("2000-2999");
                dtweightslab.Rows.Add("3000-3999");
                dtweightslab.Rows.Add("4000-4999");
                dtweightslab.Rows.Add("5000-5999");
                dtweightslab.Rows.Add("6000-6999");
                dtweightslab.Rows.Add("7000-7999");
                dtweightslab.Rows.Add("8000-8999");
                dtweightslab.Rows.Add("9000-9999");
                dtweightslab.Rows.Add("10000-10999");
                dtweightslab.Rows.Add("11000-11999");
                dtweightslab.Rows.Add("12000-12999");
                dtweightslab.Rows.Add("13000-13999");
                dtweightslab.Rows.Add("14000-14999");
                dtweightslab.Rows.Add("15000-15999");
                dtweightslab.Rows.Add("16000-16999");
                dtweightslab.Rows.Add("17000-17999");
                dtweightslab.Rows.Add("18000-99999");
                dtweightslab.Rows.Add("1-22");
                dtweightslab.Rows.Add("23-43");
                dtweightslab.Rows.Add("44-65");
                dtweightslab.Rows.Add("66-106");
                dtweightslab.Rows.Add("107-154");
                dtweightslab.Rows.Add("155-212");
            }
            else if (UploadType == 2)
            {
                dtweightslab.Rows.Add("1000-1999");
                dtweightslab.Rows.Add("2000-2999");
                dtweightslab.Rows.Add("3000-5999");
                dtweightslab.Rows.Add("6000-6999");
                dtweightslab.Rows.Add("7000-12999");
                dtweightslab.Rows.Add("13000-13999");
                dtweightslab.Rows.Add("14000-14999");
                dtweightslab.Rows.Add("15000-15999");
                dtweightslab.Rows.Add("16000-99999");
                dtweightslab.Rows.Add("1-22");
                dtweightslab.Rows.Add("23-43");
                dtweightslab.Rows.Add("44-65");
                dtweightslab.Rows.Add("66-106");
                dtweightslab.Rows.Add("107-154");
                dtweightslab.Rows.Add("155-212");
            }
            else if (UploadType == 4)
            {
                dtweightslab.Rows.Add("1000-1999");
                dtweightslab.Rows.Add("2000-2999");
                dtweightslab.Rows.Add("3000-3999");
                dtweightslab.Rows.Add("4000-4999");
                dtweightslab.Rows.Add("5000-5999");
                dtweightslab.Rows.Add("6000-6999");
                dtweightslab.Rows.Add("7000-7999");
                dtweightslab.Rows.Add("8000-8999");
                dtweightslab.Rows.Add("9000-9999");
                dtweightslab.Rows.Add("10000-10999");
                dtweightslab.Rows.Add("11000-11999");
                dtweightslab.Rows.Add("12000-12999");
                dtweightslab.Rows.Add("13000-13999");
                dtweightslab.Rows.Add("14000-14999");
                dtweightslab.Rows.Add("15000-15999");
                dtweightslab.Rows.Add("16000-16999");
                dtweightslab.Rows.Add("17000-17999");
                dtweightslab.Rows.Add("18000-99999");
            }
            else if (UploadType == 7)//7 for Brookfield Air file Templatedata
            {
                dtweightslab.Rows.Add("Minimum");
                dtweightslab.Rows.Add("100k");
                dtweightslab.Rows.Add("200k");
                dtweightslab.Rows.Add("300k");
                dtweightslab.Rows.Add("400k");
                dtweightslab.Rows.Add("500k");
                dtweightslab.Rows.Add("600k");
                dtweightslab.Rows.Add("700k");
                dtweightslab.Rows.Add("800k");
                dtweightslab.Rows.Add("900k");
                dtweightslab.Rows.Add("1000k");
                dtweightslab.Rows.Add("1200k");
                dtweightslab.Rows.Add("1500k");

            }

            else if (UploadType == 50)// 50 for Brookfield Sea file Templatedata
            {

                dtweightslab.Rows.Add("Minimum");
                dtweightslab.Rows.Add("5 cbm");
                dtweightslab.Rows.Add("7 cbm");
                dtweightslab.Rows.Add("9 cbm");
                dtweightslab.Rows.Add("11 cbm");
                dtweightslab.Rows.Add("13 cbm");
                dtweightslab.Rows.Add("15 cbm");
                dtweightslab.Rows.Add("17 cbm");
                dtweightslab.Rows.Add("19 cbm");
                dtweightslab.Rows.Add("21 cbm");
                dtweightslab.Rows.Add("23 cbm");
                dtweightslab.Rows.Add("25 cbm");
                dtweightslab.Rows.Add("27 cbm");
                dtweightslab.Rows.Add("28 cbm");
                dtweightslab.Rows.Add("30 cbm");
                dtweightslab.Rows.Add("20 lump sum");
                dtweightslab.Rows.Add("32 cbm");
                dtweightslab.Rows.Add("34 cbm");
                dtweightslab.Rows.Add("36 cbm");
                dtweightslab.Rows.Add("38 cbm");
                dtweightslab.Rows.Add("40 cbm");
                dtweightslab.Rows.Add("42 cbm");
                dtweightslab.Rows.Add("44 cbm");
                dtweightslab.Rows.Add("46 cbm");
                dtweightslab.Rows.Add("48 cbm");
                dtweightslab.Rows.Add("50 cbm");
                dtweightslab.Rows.Add("52 cbm");
                dtweightslab.Rows.Add("54 cbm");
                dtweightslab.Rows.Add("56 cbm");
                dtweightslab.Rows.Add("58 cbm");
                dtweightslab.Rows.Add("60 cbm");
                dtweightslab.Rows.Add("40 lump sum");
                dtweightslab.Rows.Add("61 cbm");
                dtweightslab.Rows.Add("63 cbm");
                dtweightslab.Rows.Add("65 cbm");
                dtweightslab.Rows.Add("67 cbm");
                dtweightslab.Rows.Add("40 HC lump sum");


            }

            else if (UploadType == 8)//8 for Freight Brookfield Air file Templatedata
            {
                dtweightslab.Rows.Add("Minimum");
                dtweightslab.Rows.Add("100k");
                dtweightslab.Rows.Add("200k");
                dtweightslab.Rows.Add("300k");
                dtweightslab.Rows.Add("400k");
                dtweightslab.Rows.Add("500k");
                dtweightslab.Rows.Add("600k");
                dtweightslab.Rows.Add("700k");
                dtweightslab.Rows.Add("800k");
                dtweightslab.Rows.Add("900k");
                dtweightslab.Rows.Add("1000k");
                dtweightslab.Rows.Add("1200k");
                dtweightslab.Rows.Add("1500k");

            }
            else if (UploadType == 51)//51 for Freight Brookfield sea file Templatedata
            {

                dtweightslab.Rows.Add("Minimum");
                dtweightslab.Rows.Add("5 cbm");
                dtweightslab.Rows.Add("7 cbm");
                dtweightslab.Rows.Add("9 cbm");
                dtweightslab.Rows.Add("11 cbm");
                dtweightslab.Rows.Add("20'");
                dtweightslab.Rows.Add("40'");
                dtweightslab.Rows.Add("40' HC");

            }
            else if (UploadType == 9)//9 for  Brookfield Rate
            {
                dtweightslab.Rows.Add("Minimum");
                dtweightslab.Rows.Add("100k");
                dtweightslab.Rows.Add("200k");
                dtweightslab.Rows.Add("300k");
                dtweightslab.Rows.Add("400k");
                dtweightslab.Rows.Add("500k");
                dtweightslab.Rows.Add("600k");
                dtweightslab.Rows.Add("700k");
                dtweightslab.Rows.Add("800k");
                dtweightslab.Rows.Add("900k");
                dtweightslab.Rows.Add("1000k");
                dtweightslab.Rows.Add("1200k");
                dtweightslab.Rows.Add("1500k");

            }
            // SANKET BROOKFILED CR 6JUNE2018
            else if (UploadType == 52)//52for sEA od  Brookfield Rate
            {
                dtweightslab.Rows.Add("Minimum");
                dtweightslab.Rows.Add("5 cbm");
                dtweightslab.Rows.Add("7 cbm");
                dtweightslab.Rows.Add("9 cbm");
                dtweightslab.Rows.Add("11 cbm");
                dtweightslab.Rows.Add("13 cbm");
                dtweightslab.Rows.Add("15 cbm");
                dtweightslab.Rows.Add("17 cbm");
                dtweightslab.Rows.Add("19 cbm");
                dtweightslab.Rows.Add("21 cbm");
                dtweightslab.Rows.Add("23 cbm");
                dtweightslab.Rows.Add("25 cbm");
                dtweightslab.Rows.Add("27 cbm");
                dtweightslab.Rows.Add("28 cbm");
                dtweightslab.Rows.Add("30 cbm");
                dtweightslab.Rows.Add("20 lump sum");
                dtweightslab.Rows.Add("32 cbm");
                dtweightslab.Rows.Add("34 cbm");
                dtweightslab.Rows.Add("36 cbm");
                dtweightslab.Rows.Add("38 cbm");
                dtweightslab.Rows.Add("40 cbm");
                dtweightslab.Rows.Add("42 cbm");
                dtweightslab.Rows.Add("44 cbm");
                dtweightslab.Rows.Add("46 cbm");
                dtweightslab.Rows.Add("48 cbm");
                dtweightslab.Rows.Add("50 cbm");
                dtweightslab.Rows.Add("52 cbm");
                dtweightslab.Rows.Add("54 cbm");
                dtweightslab.Rows.Add("56 cbm");
                dtweightslab.Rows.Add("58 cbm");
                dtweightslab.Rows.Add("60 cbm");
                dtweightslab.Rows.Add("40 lump sum");
                dtweightslab.Rows.Add("61 cbm");
                dtweightslab.Rows.Add("63 cbm");
                dtweightslab.Rows.Add("65 cbm");
                dtweightslab.Rows.Add("67 cbm");
                dtweightslab.Rows.Add("40 HC lump sum");

            }
            else if (UploadType == 53)//53 for air odf  Brookfield Rate
            {
                dtweightslab.Rows.Add("Minimum");
                dtweightslab.Rows.Add("100k");
                dtweightslab.Rows.Add("200k");
                dtweightslab.Rows.Add("300k");
                dtweightslab.Rows.Add("400k");
                dtweightslab.Rows.Add("500k");
                dtweightslab.Rows.Add("600k");
                dtweightslab.Rows.Add("700k");
                dtweightslab.Rows.Add("800k");
                dtweightslab.Rows.Add("900k");
                dtweightslab.Rows.Add("1000k");
                dtweightslab.Rows.Add("1200k");
                dtweightslab.Rows.Add("1500k");

            }
            else if (UploadType == 54)//54 for sea f  Brookfield Rate
            {
                dtweightslab.Rows.Add("Minimum");
                dtweightslab.Rows.Add("5 cbm");
                dtweightslab.Rows.Add("7 cbm");
                dtweightslab.Rows.Add("9 cbm");
                dtweightslab.Rows.Add("11 cbm");
                dtweightslab.Rows.Add("20'");
                dtweightslab.Rows.Add("40'");
                dtweightslab.Rows.Add("40' HC");

            }

            // SANKET BROOKFILED CR 6JUNE2018

            return dtweightslab;
        }

        public DataTable CalculateAmtToUSD(DataTable dt, decimal ConversionRate, int UploadType)
        {
            if (dt.Rows.Count > 0)
            {
                switch (UploadType)
                {
                    case 1: // Origin Rates
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["OriginRate"] = dr["OriginRate"].ToString() != "" ? Convert.ToDecimal(dr["OriginRate"].ToString()) * ConversionRate : 0;
                            dr["OriginTHC"] = dr["OriginTHC"].ToString() != "" ? Convert.ToDecimal(dr["OriginTHC"].ToString()) * ConversionRate : 0;
                            dr["LiftVanOriginRate"] = dr["LiftVanOriginRate"].ToString() != "" ? Convert.ToDecimal(dr["LiftVanOriginRate"].ToString()) * ConversionRate : 0;
                            dr["LiftVanOriginTHC"] = dr["LiftVanOriginTHC"].ToString() != "" ? Convert.ToDecimal(dr["LiftVanOriginTHC"].ToString()) * ConversionRate : 0;
                            dr["MiscRate"] = dr["MiscRate"].ToString() != "" ? Convert.ToDecimal(dr["MiscRate"].ToString()) * ConversionRate : 0;
                        }
                        break;

                    case 2: // Freight Rates
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["FreightRate"] = dr["FreightRate"].ToString() != "" ? Convert.ToDecimal(dr["FreightRate"].ToString()) * ConversionRate : 0;
                            dr["FreightAmount"] = dr["FreightAmount"].ToString() != "" ? Convert.ToDecimal(dr["FreightAmount"].ToString()) * ConversionRate : 0;
                            dr["MiscRate"] = dr["MiscRate"].ToString() != "" ? Convert.ToDecimal(dr["MiscRate"].ToString()) * ConversionRate : 0;
                        }
                        break;

                    case 3: // Destination Rates
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["DestinationRate"] = dr["DestinationRate"].ToString() != "" ? Convert.ToDecimal(dr["DestinationRate"].ToString()) * ConversionRate : 0;
                            dr["DestinationTHC"] = dr["DestinationTHC"].ToString() != "" ? Convert.ToDecimal(dr["DestinationTHC"].ToString()) * ConversionRate : 0;
                            dr["DestinationAQIS"] = dr["DestinationAQIS"].ToString() != "" ? Convert.ToDecimal(dr["DestinationAQIS"].ToString()) * ConversionRate : 0;
                            dr["LiftVanDestinationRate"] = dr["LiftVanDestinationRate"].ToString() != "" ? Convert.ToDecimal(dr["LiftVanDestinationRate"].ToString()) * ConversionRate : 0;
                            dr["LiftVanDestinationTHC"] = dr["LiftVanDestinationTHC"].ToString() != "" ? Convert.ToDecimal(dr["LiftVanDestinationTHC"].ToString()) * ConversionRate : 0;
                            dr["LiftVanDestinationAQIS"] = dr["LiftVanDestinationAQIS"].ToString() != "" ? Convert.ToDecimal(dr["LiftVanDestinationAQIS"].ToString()) * ConversionRate : 0;
                            dr["MiscRate"] = dr["MiscRate"].ToString() != "" ? Convert.ToDecimal(dr["MiscRate"].ToString()) * ConversionRate : 0;
                        }
                        break;

                    //case 4: // Blanket Rates

                    //    var BlanketSlabs = _spService.BindDropdown("BlanketSlabIds", "", "");
                    //    int RowCntr = 1;

                    //    if (BlanketSlabs != null)
                    //    {
                    //        foreach (DataRow dr in dt.Rows)
                    //        {
                    //            dr["WeightSlabID"] = BlanketSlabs.Single(o => Convert.ToInt32(o.Text) == RowCntr).Value;
                    //            dr["Rate"] = dr["Rate"].ToString() != "" ? Convert.ToDecimal(dr["Rate"].ToString()) * ConversionRate : 0;
                    //            dr["Amount"] = dr["Amount"].ToString() != "" ? Convert.ToDecimal(dr["Amount"].ToString()) * ConversionRate : 0;
                    //            dr["MiscRate"] = dr["MiscRate"].ToString() != "" ? Convert.ToDecimal(dr["MiscRate"].ToString()) * ConversionRate : 0;
                    //            RowCntr++;
                    //        }
                    //    }
                    //    break;

                    case 5: // Accesserial Rates
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["Amount"] = Convert.ToDecimal(dr["Amount"].ToString()) * ConversionRate;
                        }
                        break;

                    //case 6: // Permanent Storage Rates
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        dr["CartageINRate"] = dr["CartageINRate"].ToString() != "" ? Convert.ToDecimal(dr["CartageINRate"].ToString()) * ConversionRate : 0;
                    //        dr["CartageINAmount"] = dr["CartageINAmount"].ToString() != "" ? Convert.ToDecimal(dr["CartageINAmount"].ToString()) * ConversionRate : 0;
                    //        dr["CartageINMisc"] = dr["CartageINMisc"].ToString() != "" ? Convert.ToDecimal(dr["CartageINMisc"].ToString()) * ConversionRate : 0;
                    //        dr["CartageOUTRate"] = dr["CartageOUTRate"].ToString() != "" ? Convert.ToDecimal(dr["CartageOUTRate"].ToString()) * ConversionRate : 0;
                    //        dr["CartageOUTAmount"] = dr["CartageOUTAmount"].ToString() != "" ? Convert.ToDecimal(dr["CartageOUTAmount"].ToString()) * ConversionRate : 0;
                    //        dr["CartageOUTMisc"] = dr["CartageOUTMisc"].ToString() != "" ? Convert.ToDecimal(dr["CartageOUTMisc"].ToString()) * ConversionRate : 0;
                    //    }
                    //    break;
                    //case 7: // BrookField Origin Rates
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        dr["Rate"] = dr["Rate"].ToString() != "" ? Convert.ToDecimal(dr["Rate"].ToString()) * ConversionRate : 0;
                    //        dr["Misc"] = dr["Misc"].ToString() != "" ? Convert.ToDecimal(dr["Misc"].ToString()) * ConversionRate : 0;
                    //        //dr["Total"] = dr["Total"].ToString() != "" ? Convert.ToDecimal(dr["Total"].ToString()) * ConversionRate : 0; // SANKET BROOKFILED CR 6JUNE2018
                    //    }
                    //    break;
                    //case 8: // BrookField freight Rates
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        dr["Rate"] = dr["Rate"].ToString() != "" ? Convert.ToDecimal(dr["Rate"].ToString()) * ConversionRate : 0;
                    //        dr["Misc"] = dr["Misc"].ToString() != "" ? Convert.ToDecimal(dr["Misc"].ToString()) * ConversionRate : 0;
                    //        //dr["Total"] = dr["Total"].ToString() != "" ? Convert.ToDecimal(dr["Total"].ToString()) * ConversionRate : 0;   // SANKET BROOKFILED CR 6JUNE2018
                    //    }
                    //    break;
                    //case 9: // BrookField  Rates
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        dr["Rate"] = dr["Rate"].ToString() != "" ? Convert.ToDecimal(dr["Rate"].ToString()) * ConversionRate : 0;

                    //    }
                    //    break;

                    //case 10: // BrookField freight Rates
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        dr["Rate"] = dr["Rate"].ToString() != "" ? Convert.ToDecimal(dr["Rate"].ToString()) * ConversionRate : 0;
                    //        // dr["Misc"] = dr["Misc"].ToString() != "" ? Convert.ToDecimal(dr["Misc"].ToString()) * ConversionRate : 0;
                    //        //dr["Total"] = dr["Total"].ToString() != "" ? Convert.ToDecimal(dr["Total"].ToString()) * ConversionRate : 0;   // SANKET BROOKFILED CR 6JUNE2018
                    //    }
                    //    break;
                    default:
                        break;
                }
            }

            return dt;
        }


        public bool  InsertOriginRate(List<SaveOriginRate> SaveOriginRate, RateUpload R, int LoginID)
        {

            try
            {

            }
            catch (Exception)
            {
                return false;
                //throw;
               
            }

            return true;
        }

        public bool InsertFreightRate(List<SaveFreightRate> SaveOriginRate, RateUpload R, int LoginID)
        {
            try
            {

            }
            catch (Exception)
            {
                return false;
                //throw;

            }

            return true;
        }

        public bool InsertDestinationRate(List<SaveDestinationRate> SaveOriginRate, RateUpload R, int LoginID)
        {
            try
            {

            }
            catch (Exception)
            {
                return false;
                //throw;

            }

            return true;

        }

        public bool InsertBlanketRate(List<SaveBlanketRate> SaveOriginRate, RateUpload R, int LoginID)
        {
            try
            {

            }
            catch (Exception)
            {
                return false;
                //throw;

            }

            return true;

        }

        public bool UpdateVendorrate(int val, int LoginID)
        {
            try
            {

            }
            catch (Exception)
            {

                return false;
                //throw;
            }

            return true;

        }


    }
}