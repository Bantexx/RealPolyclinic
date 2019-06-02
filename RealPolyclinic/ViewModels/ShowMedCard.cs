using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using RealPolyclinic.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class ShowMedCard : BaseViewModel
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;

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

        public ICommand addDiag { get; set; }
        public ICommand addMark { get; set; }

        private ObservableCollection<Diagnosis> diagnoses;
        public ObservableCollection<Diagnosis> Diagnoses
        {
            get { return diagnoses; }
            set
            {
                diagnoses = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Vaccination> vaccinations;
        public ObservableCollection<Vaccination> Vaccinations
        {
            get { return vaccinations; }
            set
            {
                vaccinations = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Survey> surveys;
        public ObservableCollection<Survey> Surveys
        {
            get { return surveys; }
            set
            {
                surveys = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Analysis> analysis;
        public ObservableCollection<Analysis> Analysis
        {
            get { return analysis; }
            set
            {
                analysis = value;
                OnPropertyChanged();
            }
        }

        string chkvis;
        public string checkVis
        {
            get { return chkvis; }
            set
            {
                chkvis = value;
                OnPropertyChanged();
            }
        }
        public string checkNew { get; set; }

        public ShowMedCard(int id_patient,int id_doc,DateTime dt)
        {
            checkVis = "True";
            checkNew = "True";
            selectallfrommedcard(id_patient);
            addDiag = new RelayCommand(x => AddSomeNew(id_patient,id_doc));
            addMark = new RelayCommand(x=>AddMark(id_patient, id_doc));
            if (CheckDate(dt)||!CheckVisits(id_patient, id_doc))
            {
                checkVis = "False";
            }
            if (CheckDate(dt))
            {
                checkNew = "False";
            }
        }  
        
        private void selectallfrommedcard(int id)
        {
            Diagnoses = new ObservableCollection<Diagnosis>();
            Vaccinations = new ObservableCollection<Vaccination>();
            Surveys = new ObservableCollection<Survey>();
            Analysis = new ObservableCollection<Analysis>();

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Diagnoses WHERE Id_Patient =@Id_p";
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
                            cmd1.Parameters.AddWithValue("@Id_d", reader.GetInt32(3));
                            SqlDataReader reader1= cmd1.ExecuteReader();
                            reader1.Read();
                            namedoc = reader1.GetString(1);
                        }

                        Diagnoses.Add(new Diagnosis
                        {
                            Id_Diagnosis = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Id_Patient = reader.GetInt32(2),
                            Id_Doctor = reader.GetInt32(3),
                            Date = reader.GetDateTime(4).ToString("dd.MM.yyyy"),
                            Description=reader.GetString(5),
                            NameDoc = namedoc
                        });
                    }                  
                }
            }
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Vaccinations WHERE Id_Patient =@Id_p";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@Id_p", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Vaccinations.Add(new Vaccination
                        {
                            Type = reader.GetString(1),
                            Description = reader.GetString(2),
                            Date = reader.GetDateTime(3).ToString("dd/MM/yyyy"),
                            Id_Patient = reader.GetInt32(4)
                        });
                    }
                }
            }
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Surveys WHERE Id_Patient =@Id_p";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@Id_p", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Surveys.Add(new Survey
                        {
                            Title = reader.GetString(2),
                            Description = reader.GetString(1),
                            Date = reader.GetDateTime(3).ToString("dd/MM/yyyy"),
                            Id_Patient = reader.GetInt32(4)
                        });
                    }
                }
            }
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Analysis WHERE Id_Patient =@Id_p";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@Id_p", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Analysis.Add(new Analysis
                        {
                            Type = reader.GetString(2),
                            Description = reader.GetString(4),
                            Date = reader.GetDateTime(3).ToString("dd/MM/yyyy"),
                            Id_Patient = reader.GetInt32(1)
                        });
                    }
                }
            }
        }
        private void AddSomeNew(int id_pat,int id_doc)
        {
            AddtoCard atc = new AddtoCard() {DataContext=new AddNewToCardVM(id_pat,id_doc)};
            atc.ShowDialog();
           
        }
        private void AddMark(int id_p,int id_d)
        {
            if (CheckVisits(id_p, id_d))
            {
                MarkVisit mv = new MarkVisit() { DataContext = new MarkVisitVM(id_p, id_d) };
                mv.ShowDialog();
            }
        }
        private bool CheckVisits(int id_pat,int id_doc)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Visits WHERE Id_Patient =@Id_p And Id_Doc=@Id_d and Date=@Date";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@Id_p",id_pat);
                cmd.Parameters.AddWithValue("@Id_d", id_doc);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        private bool CheckDate(DateTime date)
        {
            if (date.Date < DateTime.Now.Date || date.Date > DateTime.Now.Date)
            {
                return true;
            }
            return false;
        }
    }
}
