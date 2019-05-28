using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealPolyclinic.ViewModels
{
    public class ShowMedCard : BaseViewModel
    {
        private string thisid;
        public string ThisId
        {
            get { return thisid; }
            set
            {
                thisid = value;
                OnPropertyChanged();
            }
        }
        public Patient pat { get; set; }
        public WorkWithDb wwd;
        public ShowMedCard(int id_patient)
        {
            wwd = new WorkWithDb();
            pat = wwd.SelectById(id_patient);
            ThisId = pat.FirstName;
        }       
    }
}
