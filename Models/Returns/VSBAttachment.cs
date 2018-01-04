using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSBaseAngular.Models {
    public class VSBAttachment {
        public string SiNumber { get; set; }
        public string Username { get; set; }
        public string Filename { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }
}