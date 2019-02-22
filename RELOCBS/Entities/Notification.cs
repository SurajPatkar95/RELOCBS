using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Notification
    {
        [Key]
        public int? NotificationID { get; set; }
        public int? UserID { get; set; }
        public String NotificationDescription { get; set; }
        public String NotificationLink { get; set; }
        public bool isRead { get; set; }
    }

    public class NotificationCount
    {
        [Key]
        public int? UnreadNotificatonCount { get; set; }
        public int? ReadNotificatonCount { get; set; }
    }
}