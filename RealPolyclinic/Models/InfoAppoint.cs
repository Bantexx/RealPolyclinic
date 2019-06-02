using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealPolyclinic.Models
{
    public class InfoAppoint
    {
        public Doctor Doc { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public DateTime Date { get; set; }
        public int Id_Pat { get; set; }
        public int Id { get; set; }
        public string textdate {get;set;}
    }
}
