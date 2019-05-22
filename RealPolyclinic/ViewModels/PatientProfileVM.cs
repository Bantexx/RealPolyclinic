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
        public ICommand EdProfile { get; set; }
        public ICommand CreateCard { get; set; }
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

        private string _CheckCard;
        public string Check_Card
        {
            get { return _CheckCard; }
            set
            {
                _CheckCard = value;
                OnPropertyChanged();
            }
        }

        private string _isVisible;
        public string isVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        private string _VisibleCard;
        public string VisibleCard
        {
            get { return _VisibleCard; }
            set
            {
                _VisibleCard = value;
                OnPropertyChanged();
            }
        }

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
            CreateCard = new RelayCommand(x =>CreateMedCard());
            delProfile = new RelayCommand(x => DelPatient(temp));
            SelectById(temp);
            if (InfoPat.Id_Card == 0)
            {
                Check_Card = "Нет";
                isVisible = "Visible";
                VisibleCard = "Hidden";               
            }
            else
            {    
                isVisible = "Hidden";
                Check_Card = "Есть";
                VisibleCard = "Visible";
                ShowMedCard = new MedCardVM(temp);
            }
        }
        
        private Patient SelectById(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
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
                        Birthday = reader.GetValue(9) == DBNull.Value ? "" : reader.GetDateTime(9).ToString()
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
        private void CreateMedCard()
        {
            Dictionary<string, object> newmedcard = new Dictionary<string, object>()
            {
                {"@Id_P",InfoPat.Id },
                {"@D_C", DateTime.Now},
                {"@Ins_Co",666}
            };
            Dictionary<string, object> mc = new Dictionary<string, object>()
            {
                {"@Id",InfoPat.Id}
            };
            var query = "INSERT INTO Med_Card (Id_Patient, Date_Created, Institution_Code) VALUES (@Id_P, @D_C, @Ins_Co)";
            var query1 = "UPDATE Patients SET Id_Card=(SELECT Id_Card FROM Med_Card WHERE Id_Patient=@Id) WHERE Id_Patient=@Id";

            var types = new Queue<SqlDbType>();
            types.Enqueue(SqlDbType.Int);
            types.Enqueue(SqlDbType.Date);
            types.Enqueue(SqlDbType.SmallInt);
            WorkWithDb wwd = new WorkWithDb();
            wwd.InsertInDb(query,newmedcard,types);
            wwd.UpdateDb(query1, mc);
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
    }
}
