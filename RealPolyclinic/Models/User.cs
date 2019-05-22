using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealPolyclinic.Models
{
    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int status { get; set; }
    }
}
