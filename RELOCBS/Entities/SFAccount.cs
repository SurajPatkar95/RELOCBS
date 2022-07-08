using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class SFAccount
    {
        public Int64 TempAgentID { get; set; }

        public string SFAccountID { get; set; }

        [Display(Name ="Group Name")]
        public int? AgentGroupNameID { get; set; }

        public string AgentFName { get; set; }

        public string AgentLName { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string AgentName { get; set; }

        [Required(ErrorMessage = "Short Name required")]
        public string AgentshortName { get; set; }

        [Required(ErrorMessage = "Address1 required")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage ="City required")]
        public int? CityId { get; set; }

        public string CityName { get; set; }

        public int? StateId { get; set; }
        public string StateName { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Company required")]
        public int? CompID { get; set; }

        [Display(Name = "Company Name")]
        public string CompName { get; set; }
        
        public string EmailID { get; set; }

        public string Fin_AccountCode { get; set; }

        public string GSTNO { get; set; }

        public string OldCbsId { get; set; }

        public string PINCode { get; set; }

        public string VATNo { get; set; }

        public string ContactPerson { get; set; }

        public string ContactPhone { get; set; }

        public bool IsMoved { get; set; }

        [Required(ErrorMessage = "AgentOrCorp required")]
        public string AgentOrCorp { get; set; }

        public string AgentOrCorpName { get; set; }

        public bool? IsOnlyIGST { get; set; }

        public bool? IsSez { get; set; }

        public bool? ShowForRmc { get; set; }

        public bool Isactive { get; set; } = true;

    }
}