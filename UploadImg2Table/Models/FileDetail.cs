using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadImg2Table.Models
{
    public class FileDetail
    {
        public int FileDetailId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int SupportId { get; set; }

        public virtual Support Support { get; set; }

    }
}