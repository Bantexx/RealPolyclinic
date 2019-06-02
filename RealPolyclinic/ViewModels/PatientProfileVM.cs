using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using RealPolyclinic.Views;
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
    public class PatientProfileVM : MainVM
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
        public ICommand EdProfile { get; set; }
        public ICommand delProfile { get; set; }
        private Patient _InfoPat;
        public EditProfileWindow epw;
        public Patient InfoPat
        {
            get { return _InfoPat; }
            set
            {
                _InfoPat = value;
                OnPropertyChanged();
            }
        }

        public List<InfoAppoint> appointments { get; set; }

        private BaseViewModel _ShowMedCard;
        public BaseViewModel ShowMedCard
        {
            get { return _ShowMedCard; }
            set
            {
                _ShowMedCard = value;
                OnPropertyChanged();
            }
        }

        public PatientProfileVM(int temp)
        {
            EdProfile = new RelayCommand(x => CreateNewEditWindow(x));
            delProfile = new RelayCommand(x => DelPatient(temp));
            SelectById(temp);
            ShowMedCard = new MedCardVM(temp);
            SelectAppointments(temp);
        }
        
        private Patient SelectById(int id)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = String.Format("SELECT * FROM Patients WHERE Id_Patient ='{0}'", id);
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    InfoPat = new Patient()
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        SurName = reader.GetString(2),
                        Patronymic = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
                        Snils = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4),
                        Telephone = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5),
                        Address = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6),
                        Id_Card = reader.GetValue(7) == DBNull.Value ? 0 : reader.GetInt32(7),
                        Policy = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8),
                        Birthday = reader.GetValue(9) == DBNull.Value ? "" : reader.GetDateTime(9).ToString("dd.MM.yyyy")
                    };
                }
                reader.Close();
                return InfoPat;
            }
        }
        private void CreateNewEditWindow(object prof)
        {
            epw = new EditProfileWindow(prof);
            epw.ShowDialog();          
        }     
        private void DelPatient(int id)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (InfoPat.Id_Card != 0)
                {
                    var query1 = "DELETE FROM Med_Card WHERE Id_Patient=@Pat";
                    SqlCommand cmd1 = new SqlCommand(query1, connection);
                    cmd1.Parameters.AddWithValue("@Pat", id);
                    try
                    {
                        cmd1.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }       
                var query = "DELETE FROM Patients WHERE Id_Patient=@Pat";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Pat",id);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно удалены");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void SelectAppointments(int id)
        {
            appointments = new List<InfoAppoint>();
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Appointments WHERE Id_Patient =@Id_p";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@Id_p", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string namedoc;
                        using (SqlConnection connect1 = new SqlConnection(connectionString))
                        {
                            connect1.Open();
                            string sqlexp1 = "SELECT * FROM Doctors WHERE Id_Doctor =@Id_d";
                            SqlCommand cmd1 = new SqlCommand(sqlexp1, connect1);
                            cmd1.Parameters.AddWithValue("@Id_d", reader.GetInt32(1));
                            SqlDataReader reader1 = cmd1.ExecuteReader();
                            reader1.Read();
                            namedoc = reader1.GetString(1);
                        }
                        appointments.Add(new InfoAppoint
                        {
                            textdate=reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                            Time=reader.GetTimeSpan(3).Hours.ToString()+":00",
                            Text = namedoc
                        });
                    }
                }
            }
        }
    }
}
