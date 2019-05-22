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
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class MakeAnAppointment:BaseViewModel
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
        public InfoAppoint record { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public string showDate { get; set; }
        public ICommand MakeAppoint { get; set; }
        public ICommand DelAppoint { get; set; }
        public string VisibleDel { get; set; }
        public string NamePat { get; set; }

        int _idpat;
        public int Id_Patient
        {
            get { return _idpat; }
            set
            {
                _idpat = value;
                OnPropertyChanged();
            }
        }

        int year;
        int month;
        int day;
        int hour;

        public MakeAnAppointment(InfoAppoint IA)
        { 
            record = IA;
            year = record.Date.Year;
            month = record.Date.Month;
            day = record.Date.Day;
            hour = Convert.ToInt32(record.Time.Substring(0, record.Time.LastIndexOf(':')));
            VisibleDel = "Hidden";
            CurrentDateTime = new DateTime(year, month, day, hour, 0, 0);
            if (record.Text == "Booked")
            {
                VisibleDel = "Visible";
                TakeInfoAboutPat();
                DelAppoint = new RelayCommand(x => DelRecord());
            }           
            showDate = CurrentDateTime.ToString("g");
            MakeAppoint = new RelayCommand(x=>MakeRecord(CurrentDateTime));          
        }
        private void MakeRecord(DateTime currentdate)
        {

            if (Id_Patient != 0&&CheckPatient() && CheckAppointMent())
            {
                string sqlquery = "INSERT INTO Appointments(Id_Doctor,Date,Time,Id_Patient) VALUES(@Id_Doc,@Date,@Time,@Id_Pat)";
                Dictionary<string, object> newmAppoint = new Dictionary<string, object>()
            {
                {"@Id_Doc",record.Doc.Id_Doctor },
                {"@Date", currentdate.Date},
                {"@Time", currentdate.TimeOfDay},
                {"@Id_Pat",Id_Patient}
            };
                var types = new Queue<SqlDbType>();
                types.Enqueue(SqlDbType.Int);
                types.Enqueue(SqlDbType.Date);
                types.Enqueue(SqlDbType.Time);
                types.Enqueue(SqlDbType.Int);
                WorkWithDb wwd = new WorkWithDb();
                wwd.InsertInDb(sqlquery, newmAppoint, types);
            }
            
        }
        private bool CheckAppointMent()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp =
                String.Format("SELECT * FROM Appointments WHERE Id_Doctor='{0}' AND" +
                " (Date='{1}' AND Time='{2}' )", record.Doc.Id_Doctor,CurrentDateTime.ToString("yyyy-MM-dd"),
                CurrentDateTime.TimeOfDay);
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return false;
                }
                return true;
            }
        }
        private void TakeInfoAboutPat()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp =
                String.Format("SELECT Id_Patient FROM Appointments WHERE Id_Doctor='{0}' AND" +
                " (Date='{1}' AND Time='{2}' )", record.Doc.Id_Doctor, CurrentDateTime.ToString("yyyy-MM-dd"),
                CurrentDateTime.TimeOfDay);
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Id_Patient = Convert.ToInt32(reader.GetValue(0));
                }
                reader.Close();
                string sqlexp1 =
                String.Format("SELECT * FROM Patients WHERE Id_Patient='{0}'", Id_Patient);
                SqlCommand cmd1 = new SqlCommand(sqlexp1, connect);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                if (reader1.HasRows)
                {
                    reader1.Read();
                    NamePat = reader1.GetString(1)+" "+reader1.GetString(2);
                }

            }
        }
        private void DelRecord()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp =
                String.Format("DELETE FROM Appointments WHERE Id_Doctor='{0}' " +
                "and Date='{1}'AND Time='{2}' and Id_Patient='{3}'", record.Doc.Id_Doctor, CurrentDateTime.ToString("yyyy-MM-dd")
                , CurrentDateTime.TimeOfDay, Id_Patient);
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record was deleted");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private bool CheckPatient()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Patients Where Id_Patient=@Pat";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@Pat", Id_Patient);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Такого пациента нет");
                    return false;
                }              
            }
        }
    }
}
