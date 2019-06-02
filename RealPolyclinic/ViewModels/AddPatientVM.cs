using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                Birthday=""
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
                    {"@Birth",Convert.ToDateTime(SelectedPatient.Birthday)},

                };
                string queryaddPat = "INSERT INTO Patients (Firstname,Surname,Patronymic,Snils,Telephone,Address,Policy,Birthday)" +
                    " VALUES(@FN,@SN,@Pat,@Sni,@Tele,@Addr,@Poli,@Birth)";
                Queue<SqlDbType> types = new Queue<SqlDbType>();
                for (int i = 0; i < 7; i++) { types.Enqueue(SqlDbType.NVarChar); }
                types.Enqueue(SqlDbType.Date);
                WorkWithDb insertPat = new WorkWithDb();
                if (insertPat.InsertInDb(queryaddPat, paramval, types))
                {
                    CreateMedCard(SelectedPatient.Policy);
                    SelectedPatient.FirstName = String.Empty;
                    SelectedPatient.SurName = String.Empty;
                    SelectedPatient.Patronymic = String.Empty;
                    SelectedPatient.Snils = String.Empty;
                    SelectedPatient.Telephone = String.Empty;
                    SelectedPatient.Address = String.Empty;
                    SelectedPatient.Policy = String.Empty;
                    SelectedPatient.Birthday = String.Empty;
                }
            }
            else
            {
                MessageBox.Show("Исправте поля");
            }

        }
        private void CreateMedCard(string policy)
        {
            int id_pat = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Patients WHERE Policy =@pol";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@pol", policy);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    id_pat = reader.GetInt32(0);
                }
            }
            Dictionary<string, object> newmedcard = new Dictionary<string, object>()
            {
                {"@Id_P",id_pat},
                {"@D_C", DateTime.Now.Date},
                {"@Ins_Co",666}
            };
            Dictionary<string, object> mc = new Dictionary<string, object>()
            {
                {"@Id",id_pat}
            };
            var query = "INSERT INTO Med_Card (Id_Patient, Date_Created, Institution_Code) VALUES (@Id_P, @D_C, @Ins_Co)";
            var query1 = "UPDATE Patients SET Id_Card=(SELECT Id_Card FROM Med_Card WHERE Id_Patient=@Id) WHERE Id_Patient=@Id";
            var types = new Queue<SqlDbType>();
            types.Enqueue(SqlDbType.Int);
            types.Enqueue(SqlDbType.Date);
            types.Enqueue(SqlDbType.SmallInt);
            WorkWithDb wwd = new WorkWithDb();
            wwd.InsertInDb(query, newmedcard, types);
            wwd.UpdateDb(query1, mc);
        }
    }
}
