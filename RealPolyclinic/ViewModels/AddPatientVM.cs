using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class AddPatientVM : MainVM
    {
        public RelayCommand addNew { get; set; }
        public Patient SelectedPatient { get; set; }
        public AddPatientVM()
        {
            SelectedPatient = new Patient()
            {
                FirstName = "",
                SurName = "",
                Patronymic = "",
                Snils="",
                Telephone=""
            };
            addNew = new RelayCommand(obj => addnewpatient());
        }
        private void addnewpatient()
        {
            if (SelectedPatient.Error == String.Empty)
            {
                Dictionary<string, object> paramval = new Dictionary<string, object>()
                {
                    {"@FN",SelectedPatient.FirstName },
                    {"@SN",SelectedPatient.SurName },
                    {"@Pat",SelectedPatient.Patronymic },
                    {"@Sni",SelectedPatient.Snils },
                    {"@Tele",SelectedPatient.Telephone }
                };
                string queryaddPat = "INSERT INTO Patients (Firstname,Surname,Patronymic,Snils,Telephone) VALUES(@FN,@SN,@Pat,@Sni,@Tele)";
                Queue<SqlDbType> types = new Queue<SqlDbType>();
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.NVarChar);
                WorkWithDb insertPat = new WorkWithDb();
                if (insertPat.InsertInDb(queryaddPat, paramval, types))
                {
                    SelectedPatient.FirstName = String.Empty;
                    SelectedPatient.SurName = String.Empty;
                    SelectedPatient.Patronymic = String.Empty;
                    SelectedPatient.Snils = String.Empty;
                    SelectedPatient.Telephone = String.Empty;
                }
            }
            else
            {
                MessageBox.Show("Исправте поля");
            }

        }
    }
}
