using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Agent
    {

        public int AgentID { get; set; }

        public int AgentGroupID { get; set; }

        public string AgentGroupName { get; set; }

        public string AgentName { get; set; }

        public string ShortAgentName { get; set; }

        public string ContactPerson { get; set; }

        public string ContactPhone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public int CityID { get; set; }

        public string CityName { get; set; }

        public int CompID { get; set; }

        public string CompanyName { get; set; }

        public bool Isactive { get; set; }
      
    }

    public class AgentViewModel
    {

        public int AgentID { get; set; }

        [Required(ErrorMessage ="Agent Group is required")]
        public int AgentGroupID { get; set; }

        public string AgentGroupName { get; set; }

        [Display(Name ="Agent")]
        [Required(ErrorMessage = "Agent Name is required")]
        public string AgentName { get; set; }

        
        public string ShortAgentName { get; set; }

        public string ContactPerson { get; set; }

        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Adderess 1 is required")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }


        [Required(ErrorMessage = "City is required")]
        public int CityID { get; set; }

        public string CityName { get; set; }

        [Required(ErrorMessage = "Company is required")]
        public int CompID { get; set; }

        public string CompanyName { get; set; }

        public bool Isactive { get; set; }

    }
}