using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Support;
using System;

namespace RELOCBS.BL.Support
{
    public class SupportBL
    {

        private SupportDAL _SupportDAL;
        public SupportDAL SupportDAL
        {
            get
            {
                if (_SupportDAL == null)
                    _SupportDAL = new SupportDAL();
                return _SupportDAL;
            }
        }

        public bool RemoveInvApproval(Entities.Support SupportObj, int LoginID, out string result)
        {
            try
            {
                return SupportDAL.RemoveInvApproval(SupportObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "SupportBL", "RemoveInvApproval", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool ChangeCityInJob(Entities.Support SupportObj, int LoginID, out string result)
        {
            try
            {
                return SupportDAL.ChangeCityInJob(SupportObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "SupportBL", "ChangeCityInJob", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool ChangeRefInInv(Entities.Support SupportObj, int LoginID, out string result)
        {
            try
            {
                return SupportDAL.ChangeRefInInv(SupportObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "SupportBL", "ChangeRefInInv", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool ChangeRevBr(Entities.Support SupportObj, int LoginID, out string result)
        {
            try
            {
                return SupportDAL.ChangeRevBr(SupportObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "SupportBL", "ChangeRevBr", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}