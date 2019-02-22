using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Lane : Entity
    {
        [Key]
        public int LaneId { get; set; }
        public string ShortLaneName { get; set; }
        public string LaneName { get; set; }
        public int OriginCountryID { get; set; }
        public int DestinationCountryID { get; set; }
        public string DestinationCountry { get; set; }
        public string OriginCountry { get; set; }

        public int OriginCityID { get; set; }

        public string OriginCity { get; set; }

        public int DestinationCityID { get; set; }

        public string DestinationCity { get; set; }

        public Boolean isActive { get; set; }
        public int? TotalRows { get; set; }
    }

    public class LaneViewModel : Entity
    {
        [Key]
        public int LaneId { get; set; }

        [Display(Name = "Lane Name")]
        public string LaneName { get; set; }

        [Required(ErrorMessage = "Please select origin country.")]
        public int? OriginCountryID { get; set; }

        [Display(Name = "Origin Country")]
        public string OriginCountry { get; set; }

        [Required(ErrorMessage = "Please select destination country.")]
        public int? DestinationCountryID { get; set; }

        [Display(Name = "Destination Country")]
        public string DestinationCountry { get; set; }
        
        [Required(ErrorMessage = "Please select origin city.")]
        public int? OriginCityID { get; set; }

        [Display(Name = "Origin City")]
        public string OriginCity { get; set; }

        [Required(ErrorMessage = "Please select destination city.")]
        public int? DestinationCityID { get; set; }

        [Display(Name = "Destination City")]
        public string DestinationCity { get; set; }

        public Boolean isActive { get; set; }

    }

    public class Pricing
    {
        public Lane LaneData { get; set; }
        public BufferPricingData BufferPricingData { get; set; }
    }

    public class BufferPricingData
    {

        public BufferPricingData()
        {
            bufferList = new List<Buffer>();
        }

        [Key]
        public int? LeadID { get; set; }
        public List<Buffer> bufferList;
    }

    public class Buffer
    {
        public int BufferID { get; set; }
        public string BufferName { get; set; }
        public decimal? BufferValue { get; set; }
    }

}