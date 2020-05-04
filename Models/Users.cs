using System;
using System.Collections.Generic;

namespace webapiRealPage.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Pswd { get; set; }
        public bool? Admin { get; set; }
    }
}
