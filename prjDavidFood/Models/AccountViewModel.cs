using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using prjDavidFood.Models;


namespace prjDavidFood.Models
{
    public class AccountViewModel
    {
        public List<tMember> Member { get; set; }

        public List<tAdmin> Admin { get; set; }
    }
}