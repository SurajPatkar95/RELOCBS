using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class CurrencyConversion
    {
        public Int64 CurrConvID { get; set; }
        public string FIN_PERIOD { get; set; }
        public string Currency_Code { get; set; }
        public int From_Curr_ID { get; set; }
        public string Currency_Code_To { get; set; }
        public int To_Curr_ID { get; set; }
        public string Multiply_Divide { get; set; }
        public decimal ConversionRate { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
         

    }
}