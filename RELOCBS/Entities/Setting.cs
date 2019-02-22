using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Setting
    {
        public Setting() { }

        public Setting(string name, string value, string DisplayName, int userId = 0)
        {
            this.Name = name;
            this.Value = value;
            this.DisplayName = DisplayName;
            this.UserId = userId;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the displayname
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the user for which this setting is valid. 0 is set when the setting is for all users
        /// </summary>
        public int UserId { get; set; }
    }

    public class DashboardFilter
    {
        [Key]
        public string ForPanel { get; set; }
        public string Period { get; set; }
        public Nullable<DateTime> From { get; set; }
        public Nullable<DateTime> To { get; set; }
        public int? LoggedinUserID { get; set; }
        public int ShipmentMode { get; set; }
        public string Profit_Loss { get; set; }
        public string MoveStages { get; set; }
        public string RevenueLoad { get; set; }
        public string UnitForRevenueLoad { get; set; }
    }

    public class DashboardPanelDetails : DashboardFilter
    {
        public int RMCID { get; set; }
    }
}