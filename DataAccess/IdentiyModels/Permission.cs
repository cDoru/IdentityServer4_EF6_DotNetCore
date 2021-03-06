﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IdentiyModels
{
    public class Permission
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ModuleId { get; set; }
        public string Description { get; set; }
        public string PermissionCode { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("ModuleId")]
        public Module Module { get; set; }
    }
}
