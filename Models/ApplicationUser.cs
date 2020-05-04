using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapiRealPage.Models {

    public class ApplicationUser : IdentityUser {
        [Column(TypeName = "varchar(150)") ]
        public string FullName { get; set; }
        public bool Admin { get; set; }
    }
}