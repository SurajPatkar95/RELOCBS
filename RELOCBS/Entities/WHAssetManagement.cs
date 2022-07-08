using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace RELOCBS.Entities
{
    public class WHAssetMaster
    {
        public Int64? ID { get; set; }
        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string EmailBcc { get; set; }
        public string ActiveTab { get; set; }
        public bool? IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string WHInAssetMastListHidden { get; set; }
        public string WHInAssetDetailsListHidden { get; set; }
        public string WHOutAssetMastListHidden { get; set; }
        public string WHOutAssetDetailsListHidden { get; set; }
        public string AssetLiftVanMapHidden { get; set; }
        public WHInAssetMaster WHInAssetMaster { get; set; } = new WHInAssetMaster();
        public List<WHInAssetMaster> WHInAssetMasterList { get; set; } = new List<WHInAssetMaster>();
        public WHOutAssetMaster WHOutAssetMaster { get; set; } = new WHOutAssetMaster();
        public List<WHOutAssetMaster> WHOutAssetMasterList { get; set; } = new List<WHOutAssetMaster>();
        public WHLocationMap WHLocationMap { get; set; } = new WHLocationMap();
        public WHJobDocUpload WHJobDocUpload { get; set; } = new WHJobDocUpload();
        public List<WHJobDocUpload> WHJobDocUploadList { get; set; } = new List<WHJobDocUpload>();
        public DataSet BingoSheetDataSet { get; set; }
    }

    public class WHInAssetMaster
    {
        public Int64? InMastID { get; set; }
        public Int64? MoveID { get; set; }
        public string JobID { get; set; }
        public string RefJobID { get; set; }
        public bool? IsDirectDel { get; set; }
        public int? WareHouseID { get; set; }
        public string WareHouse { get; set; }
        public string ShipperName { get; set; }
        public DateTime? InDateTime { get; set; }
        public DateTime? JobOpenedDate { get; set; }
        public int? NoOfPackaage { get; set; }
        public int? VolUnitID { get; set; }
        public string VolUnit { get; set; }
        public decimal? TotalVol { get; set; }
        public string GateChallanNumber { get; set; }
        public DateTime? GateChalanDate { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public WHInAssetDetails WHInAssetDetails { get; set; } = new WHInAssetDetails();
        public List<WHInAssetDetails> WHInAssetDetailsList { get; set; } = new List<WHInAssetDetails>();
    }

    public class WHInAssetDetails
    {
        public Int64? InMastID { get; set; }
        public string JobID { get; set; }
        public Int64? AssetDetID { get; set; }
        public string GateChallanNumber { get; set; }
        public string GateChalanDate { get; set; }
        public string BarcodeID { get; set; }
        public Int64? BarcodeSeqNo { get; set; }
        public string AssetRefID { get; set; }
        public string AssetDescription { get; set; }
        public decimal? AssetDimL { get; set; }
        public decimal? AssetDimB { get; set; }
        public decimal? AssetDimH { get; set; }
        public decimal? AssetVol { get; set; }
        public int? DimentionUnitID { get; set; }
        public string DimentionUnit { get; set; }
        public int? VolumeUnitID { get; set; }
        public string VolumeUnit { get; set; }
        public string AssetRemarks { get; set; }
        public bool? IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class WHOutAssetMaster
    {
        public Int64? OutMasterID { get; set; }
        public Int64? MoveID { get; set; }
        public DateTime? OutDateTime { get; set; }
        public int? OutNoPackage { get; set; }
        public int? OutVolumeUnitID { get; set; }
        public string OutVolumeUnit { get; set; }
        public decimal? OutVolume { get; set; }
        public string GateOutChalanNo { get; set; }
        public DateTime? GateOutChalanDate { get; set; }
        public string AddressType { get; set; }
        public string OutLocName { get; set; }
        public string OutLocAdd1 { get; set; }
        public string OutLocAdd2 { get; set; }
        public int? OutLocCityID { get; set; }
        public string OutLocCity { get; set; }
        public string OutLocContactPerson { get; set; }
        public string OutLocPinCode { get; set; }
        public string OutLocPhone { get; set; }
        public string DeliveryProofNo { get; set; }
        public bool IsProofUploaded { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modifieddate { get; set; }
        public WHOutAssetDetails WHOutAssetDetails { get; set; } = new WHOutAssetDetails();
        public List<WHOutAssetDetails> WHOutAssetDetailsList { get; set; } = new List<WHOutAssetDetails>();
    }

    public class WHOutAssetDetails
    {
        public Int64? OutMasterID { get; set; }
        public Int64? OutDetailID { get; set; }
        public Int64? AssetDetID { get; set; }
        public string GateOutChalanNo { get; set; }
        public string GateOutChalanDate { get; set; }
        public string BarcodeID { get; set; }
        public Int64? BarcodeSeqNo { get; set; }
        public string AssetRefID { get; set; }
        public string AssetDescription { get; set; }
        public string AssetRemarks { get; set; }
        public bool? IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class WHInOutAssetMaster
    {
        public Int64? InMastID { get; set; }
        public Int64? MoveID { get; set; }
        public string JobID { get; set; }
        public string RefJobID { get; set; }
        public string WareHouse { get; set; }
        public DateTime? InDateTime { get; set; }
        public string BarcodeSeqNo { get; set; }
        public string InAssetRefID { get; set; }
        public string InAssetDescription { get; set; }
        public string InAssetRemarks { get; set; }
        public DateTime? OutDateTime { get; set; }
        public string OutLocAdd1 { get; set; }
        public string OutLocAdd2 { get; set; }
        public string OutLocCity { get; set; }
        public string OutLocContactPerson { get; set; }
        public string OutAssetRemarks { get; set; }
    }

    public class WHLocationMap
    {
        public Int64? InMastID { get; set; }
        public int? LocationID { get; set; }
        public Int64? LiftVanID { get; set; }
        public List<LiftVanDetails> LiftVanDetailsList { get; set; } = new List<LiftVanDetails>();
        public List<LiftVanDetails> DefLiftVanDetailsList { get; set; } = new List<LiftVanDetails>();
        public List<WHInAssetDetails> OtherLiftVanDetailsList { get; set; } = new List<WHInAssetDetails>();
        public int[] AssetList { get; set; }
        public int[] DefAssetList { get; set; }
    }

    public class LiftVanDetails
    {
        public Int64? MapID { get; set; }
        public Int64? LiftVanID { get; set; }
        public Int64? AssetDetID { get; set; }
        public string AssetDesc { get; set; }
        public DateTime? MappedOn { get; set; }
        public DateTime? RemovedOn { get; set; }
        public int? MappedBy { get; set; }
        public int? RemovedBy { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
    }

    public class WHJobDocUpload
    {
        public Int64 ID { get; set; }
        public int? FileID { get; set; }
        public Int64? MoveID { get; set; }
        public string DocFromType { get; set; }
        public int? DocTypeID { get; set; }
        public string DocTypeText { get; set; }
        public int? DocNameID { get; set; }
        public string DocNameText { get; set; }
        public string DocDescription { get; set; }
        public string FileName { get; set; }
        public string UploadBy { get; set; }
        public int? UploadById { get; set; }
        public DateTime? UploadDate { get; set; }
        //public int[] DocNameIDList { get; set; }
        //public int[] DocNameIDListSelected { get; set; }
        //public string DocNameIDListHidden { get; set; }
        //public bool IsShowToAssignee { get; set; }
        //public string Remarks { get; set; }
        public HttpPostedFileBase[] File { get; set; }
        //public HttpPostedFileBase[] ExtFile { get; set; }
        //public List<JobDocument> DocList { get; set; } = new List<JobDocument>();
    }
}