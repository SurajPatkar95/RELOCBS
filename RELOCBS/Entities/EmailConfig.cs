using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class EmailConfig
    {
        public string EmailFrom { get; set; }   
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Host { get; set; }
        public string EmailFromPassowrd { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }

        public List<EmailSendAttachment> attachments { get; set; } = new List<EmailSendAttachment>();

    }

    public class EmailSendAttachment
    {

        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] UploadFile { get; set; }

    }
}