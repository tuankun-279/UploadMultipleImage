﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UploadImg2Table.Models
{
    public class Support
    {
        public int SupportId { get; set; }
        [Required(ErrorMessage ="Please Enter Your Name")]
        [Display(Name ="Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Summary")]
        [Display(Name = "Summary")]
        [MaxLength(500)]
        public string Summary { get; set; }
        public virtual ICollection<FileDetail> FileDetail { get; set; }
    }
}