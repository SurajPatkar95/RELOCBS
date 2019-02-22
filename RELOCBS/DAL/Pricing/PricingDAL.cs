using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Pricing
{
    public class PricingDAL
    {

        public bool InsertOriginRate(List<SaveOriginRate> SaveOriginRate, RateUpload R, int LoginID)
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

    }
}