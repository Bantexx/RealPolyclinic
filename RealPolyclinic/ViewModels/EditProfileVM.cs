using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealPolyclinic.ViewModels
{
    public class EditProfileVM : BaseViewModel
    {
        public Patient profpat { get; set; }
        public RelayCommand saveprof { get; set; }

        public EditProfileVM(object _prof)
        {
            profpat = (Patient)_prof;
            saveprof = new RelayCommand(x => UpdatePat(profpat.Id));
        }

        private void UpdatePat(int id)
        {
            if (profpat.Error == String.Empty)
            {
                string queryaddPat = "UPDATE Patients SET Firstname=@FN, Surname=@SN,Patronymic=@Pat WHERE Id_Patient=@Id";
                Dictionary<string, object> dictPat = new Dictionary<string, object>()
                {
                    {"@FN",profpat.FirstName },
                    {"@SN",profpat.SurName },
                    {"@Pat",profpat.Patronymic },
                    {"@Id",profpat.Id },
                };
                WorkWithDb wwd = new WorkWithDb();
                wwd.UpdateDb(queryaddPat, dictPat);                              
            }
            else
            {
                MessageBox.Show("Исправте поля");
            }
        }
    }
}
