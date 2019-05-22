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
                Telephone="",
                Address="",
                Policy="",
                Birthday=null
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
                    {"@Tele",SelectedPatient.Telephone },
                    {"@Addr",SelectedPatient.Address },
                    {"@Poli",SelectedPatient.Policy },
                    {"@Birth",Convert.ToDateTime(SelectedPatient.Birthday) },

                };
                string queryaddPat = "INSERT INTO Patients (Firstname,Surname,Patronymic,Snils,Telephone,Address,Policy,Birthday)" +
                    " VALUES(@FN,@SN,@Pat,@Sni,@Tele,@Addr,@Poli,@Birth)";
                Queue<SqlDbType> types = new Queue<SqlDbType>();
                for (int i = 0; i < 7; i++) { types.Enqueue(SqlDbType.NVarChar); }
                types.Enqueue(SqlDbType.Date);
                WorkWithDb insertPat = new WorkWithDb();
                if (insertPat.InsertInDb(queryaddPat, paramval, types))
                {
                    SelectedPatient.FirstName = String.Empty;
                    SelectedPatient.SurName = String.Empty;
                    SelectedPatient.Patronymic = String.Empty;
                    SelectedPatient.Snils = String.Empty;
                    SelectedPatient.Telephone = String.Empty;
                    SelectedPatient.Address = String.Empty;
                    SelectedPatient.Policy = String.Empty;
                    SelectedPatient.Birthday = null;
                }
            }
            else
            {
                MessageBox.Show("Исправте поля");
            }

        }
    }
}
