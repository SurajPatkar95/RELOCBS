using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
	public class SubCosthead
	{
		public int RateCompID { get; set; } = 0;
		public int CostHeadID { get; set; } = 0;
		public int SubCostID { get; set; } = 0;
		public int RateCurrID { get; set; } = 0;
		public string RateCurr { get; set; }
		public decimal RateValue { get; set; } = 0;
		public decimal ConvRate { get; set; } = 1;
		public string CostHeadName { get; set; }
		public decimal Value { get; set; } = 0;
	}
}