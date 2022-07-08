using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class VendorContractModel
    {/// <summary>
    /// Master Details for Vendor contract
    /// </summary>
        public int? VContractId { get; set; } = -1;
        [Required(ErrorMessage = "Account MasterCode required")]
        public string Account_MasterCode { get; set; }
        [Required(ErrorMessage = "Account SubCode required")]
        public string Account_SubCode { get; set; }
        [Required(ErrorMessage = "Branch required")]
        public int BranchId { get; set; }
        [Required(ErrorMessage = "BusinessUnit required")]
        public int BusinessUnitId { get; set; }
        [Required(ErrorMessage = "Vendor Name required")]
        public string VendorName { get; set; }
        [Required(ErrorMessage = "Vendor City required")]
        public int VendorCityId { get; set; }
        [Required(ErrorMessage = "Responsible Person required")]
        public int Finance_EmpId { get; set; }
        public string PAN { get; set; }
        public string GSTNo { get; set; }
        public int ServiceCategoryId { get; set; }
        public string IsMSME { get; set; }
        public string Owner_Name { get; set; }
        public string Contact_PersonName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Contact_PersonEmail { get; set; }
        [RegularExpression("^[ 0-9/#+-]*$", ErrorMessage ="Invalid Characters for Contact No.")]
        public string Contact_PersonMobile { get; set; }

        public int? VCStatusID { get; set; }

        /// <summary>
        /// Transactional Dates for Vendor contract
        /// </summary>
        [Required(ErrorMessage = "Commencement Date required")]
        public DateTime? Commencement_Date { get; set; }
        [Required(ErrorMessage = "Expiry Date required")]
        public DateTime? ExpiryDate { get; set; }
        public DateTime? Account_Reco_LastCompleteDate { get; set; }
        public DateTime? Certificate_LastDuesDate { get; set; }
        public DateTime? GSTR2A_Reco_LastCompleteDate { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class VendorContractGrid
    {
        public int VContractId { get; set; }
        public string Account_MasterCode { get; set; }
        public string Account_SubCode { get; set; }
        public string BranchName { get; set; }
        public string BusinessUnitName { get; set; }
        public string VendorName { get; set; }
        public string VendorCity { get; set; }
        public string Finance_EmpName { get; set; }
        public string PAN { get; set; }
        public string GSTNo { get; set; }
        public string ServiceCategoryName { get; set; }
        public string IsMSME { get; set; }
        public string Owner_Name { get; set; }
        public string Contact_PersonName { get; set; }
        public string Contact_PersonEmail { get; set; }
        public string Contact_PersonMobile { get; set; }

        public DateTime Commencement_Date { get; set; }
        public DateTime ExpiryDate { get; set; }
        
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }


    public class VendorContractDocUpload
    {

        [Required(ErrorMessage = "ID is required")]
        public Int64 ID { get; set; }

        public Int64? JobDocTypeId { get; set; }

        [Required(ErrorMessage = "From Type is required")]
        public string DocFromType { get; set; }

        [Required(ErrorMessage = "Document Type is required")]
        public Int32 DocTypeID { get; set; }

        [Required(ErrorMessage = "Document Name is required")]
        public Int32 DocNameID { get; set; }

        public DateTime? ExpiryDate { get; set; }
        
        public string DocDescription { get; set; }

        public string Remarks { get; set; }

        public string FileName { get; set; }

        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase[] file { get; set; }

        public HttpPostedFileBase[] ExtFile { get; set; }

        public List<VendorContractDocuments> docLists { get; set; } = new List<VendorContractDocuments>();

        public String DocTypeText { get; set; }
        public String DocNameText { get; set; }
    }

    public class VendorContractDocuments
    {
        public Int32 FileID { get; set; }

        public Int32 DocTypeID { get; set; }

        public string DocType { get; set; }

        public Int32 DocNameID { get; set; }

        public string DocName { get; set; }

        public String DocDescription { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public String FileName { get; set; }

        public String FilePath { get; set; }

        public String UploadBy { get; set; }

        public int UploadById { get; set; }

        public HttpPostedFileBase[] file { get; set; }

        public DateTime UploadDate { get; set; }
    }
}