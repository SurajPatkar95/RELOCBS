using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.VendorContract;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace RELOCBS.BL.VendorContract
{
    public class VendorContractBL
    {
        private VendorContractDAL _DAL;

        public VendorContractDAL DAL
        {

            get
            {
                if (this._DAL == null)
                    this._DAL = new VendorContractDAL();
                return this._DAL;
            }
        }


        public IEnumerable<Entities.VendorContractGrid> GetGrid(string VendorName, string MasterCode, string SubCode, string sort, string sortdir, int skip, int pageSize, out int totalCount)
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
                IQueryable<Entities.VendorContractGrid> List = DAL.GetGrid(VendorName, MasterCode,SubCode, RMCBuss);
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
                    return new List<Entities.VendorContractGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorContractBL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public Entities.VendorContractModel GetDetail(Int64 ContractId)
        {
            VendorContractModel model = new VendorContractModel();

            try
            {
                DataTable data = DAL.GetDetail(UserSession.GetUserSession().LoginID, ContractId);

                if (data != null && data.Rows.Count > 0)
                {
                    model = (from item in data.AsEnumerable()
                             select new VendorContractModel()
                             {
                                 VContractId = Convert.ToInt32(item["VContractId"]),
                                 BusinessUnitId=Convert.ToInt32(item["BusinessUnitId"]),
                                 VendorCityId = Convert.ToInt16(item["VendorCityId"]),
                                 VendorName = Convert.ToString(item["VendorName"]),
                                 Account_MasterCode = Convert.ToString(item["Account_MasterCode"]),
                                 Account_SubCode = Convert.ToString(item["Account_SubCode"]),
                                 ServiceCategoryId = Convert.ToInt32(item["ServiceCategoryId"]),
                                 BranchId = Convert.ToInt32(item["BranchId"]),
                                 Finance_EmpId = Convert.ToInt32(item["Finance_EmpId"]),
                                 Contact_PersonName = Convert.ToString(item["Contact_PersonName"]),
                                 Contact_PersonEmail = Convert.ToString(item["Contact_PersonEmail"]),
                                 Contact_PersonMobile = Convert.ToString(item["Contact_PersonMobile"]),
                                 Account_Reco_LastCompleteDate = item["Account_Reco_LastCompleteDate"] != DBNull.Value ? Convert.ToDateTime(item["Account_Reco_LastCompleteDate"]) : (DateTime?)null,
                                 Certificate_LastDuesDate = item["Certificate_LastDuesDate"] != DBNull.Value ? Convert.ToDateTime(item["Certificate_LastDuesDate"]) : (DateTime?)null,
                                 Commencement_Date = Convert.ToDateTime(item["Commencement_Date"]),
                                 ExpiryDate = Convert.ToDateTime(item["ExpiryDate"]),
                                 GSTNo =Convert.ToString(item["GSTNo"]),
                                 GSTR2A_Reco_LastCompleteDate = item["GSTR2A_Reco_LastCompleteDate"] != DBNull.Value ? Convert.ToDateTime(item["GSTR2A_Reco_LastCompleteDate"]) : (DateTime?)null,
                                 IsMSME = Convert.ToString(item["IsMSME"]),
                                 Owner_Name = Convert.ToString(item["Owner_Name"]),
                                 PAN = Convert.ToString(item["PAN"]),
                                 LastModifiedBy = Convert.ToString(item["LastModifiedBy"]),
                                 LastModifiedDate = Convert.ToDateTime(item["LastModifiedDate"]),
                                 VCStatusID = item["VCStatusID"] != DBNull.Value ? Convert.ToInt32(item["VCStatusID"]) : (int?)null
                             }).FirstOrDefault();


                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorContractBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        
        public bool Insert(VendorContractModel model, out string result)
        {
            try
            {
                return DAL.Insert(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorContractBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Delete(Int64 ID, out string message)
        {
            try
            {
                return DAL.Delete(ID, UserSession.GetUserSession().LoginID, out message);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorContractBL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public VendorContractDocUpload GetDocumentGrid(Int64 ID, string FromType, String DocFromType, int DocTypeID = -1, int DocNameID = -1, string DocDescription = "")
        {
            VendorContractDocUpload jobDoc = new VendorContractDocUpload();
            jobDoc.ID = ID;
            jobDoc.DocFromType = FromType;
            jobDoc.DocTypeID = DocTypeID;
            jobDoc.DocDescription = DocDescription;
            jobDoc.DocNameID = DocNameID;
            try
            {
                DataTable DocDs = DAL.GetDocumentGrid(ID, DocFromType, DocTypeID, DocNameID, DocDescription);


                if (DocDs != null)
                {

                    jobDoc.docLists = (from item in DocDs.AsEnumerable()
                                       select new VendorContractDocuments()
                                       {
                                           FileID = Convert.ToInt32(item["FileID"]),
                                           DocType = Convert.ToString(item["DocTypeName"]),
                                           DocName = Convert.ToString(item["DocName"]),
                                           DocDescription = Convert.ToString(item["Description"]),
                                           FileName = Convert.ToString(item["DocFileName"]),
                                           UploadBy = Convert.ToString(item["UploadBy"]),
                                           UploadById = Convert.ToInt32(item["CreatedBy"]),
                                           UploadDate = Convert.ToDateTime(item["CreatedDate"]),
                                           ExpiryDate = item["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ExpiryDate"]),
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorContractBL", "GetDocumentGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool DeleteDocument(int ID, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                return DAL.DeleteDocument(ID, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "VendorContractBL", "InsertDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public VendorContractDocuments GetDownloadFile(int FileID)
        {
            VendorContractDocuments obj;
            try
            {
                return DAL.GetDownloadFile(FileID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorContractBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }


        public bool InsertDocument(VendorContractDocUpload SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return DAL.InsertDocument(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "VendorContractBL", "InsertDocument", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}