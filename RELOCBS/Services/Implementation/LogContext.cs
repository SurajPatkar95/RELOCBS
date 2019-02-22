using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Services.Implementation
{
    public class LogContext
    {
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        public LogLevel LogLevel { get; set; }
        public User User { get; set; }

        public bool HashNotFullMessage { get; set; }
        public bool HashIpAddress { get; set; }
    }
}