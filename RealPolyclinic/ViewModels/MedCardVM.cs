using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealPolyclinic.ViewModels
{
    public class MedCardVM : BaseViewModel
    {
        MedCard medicalCard;
        public MedCard MedicalCard
        {
            get { return medicalCard; }
            set
            {
                medicalCard = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<string> _vaccinations;
        public ObservableCollection<string> vaccinations
        {
            get { return _vaccinations; }
            set
            {
                _vaccinations = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<string> _analysis;
        public ObservableCollection<string> analysis
        {
            get { return _analysis; }
            set
            {
                _analysis = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<string> _diagnoses;
        public ObservableCollection<string> diagnoses
        {
            get { return _diagnoses; }
            set
            {
                _diagnoses = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<string> _surveys;
        public ObservableCollection<string> surveys
        {
            get { return _surveys; }
            set
            {
                _surveys = value;
                OnPropertyChanged();
            }
        }

        public int Id_Pat { get; set; }
        public MedCardVM(int idPatient)
        {
            vaccinations = new ObservableCollection<string>();
            analysis = new ObservableCollection<string>();
            diagnoses = new ObservableCollection<string>();
            surveys = new ObservableCollection<string>();

            Id_Pat = idPatient;
            SelectById(idPatient);
            Filling(MedicalCard);
        }

        private MedCard SelectById(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = String.Format("SELECT * FROM Med_Card WHERE Id_Patient ='{0}'", id);
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    MedicalCard = new MedCard()
                    {
                        Id_card = Convert.ToInt32(reader.GetValue(0)),
                        Id_Patient = Convert.ToInt32(reader.GetValue(1)),
                        Date_Created = Convert.ToDateTime(reader.GetValue(2)),
                        Institution_Code = Convert.ToInt32(reader.GetValue(3)),
                        Diagnoses = reader.GetValue(4) == DBNull.Value ? "" : reader.GetValue(4).ToString(),
                        Vaccinations = reader.GetValue(5) == DBNull.Value ? "" : reader.GetValue(5).ToString(),
                        Surveys = reader.GetValue(6) == DBNull.Value ? "" : reader.GetValue(6).ToString(),
                        Analysis = reader.GetValue(7) == DBNull.Value ? "" : reader.GetValue(7).ToString(),

                    };
                }
                reader.Close();
                return MedicalCard;
            }
        }
        private void Filling(MedCard mc)
        {
            string[] masswithIdVacc = mc.Vaccinations.Split(',');

            string[] masswithIdAnalys = mc.Analysis.Split(',');

            string[] masswithIdDiagnoses = mc.Diagnoses.Split(',');

            string[] masswithIdSurveys = mc.Surveys.Split(',');

            SelectWithArgs(masswithIdVacc, "Vaccinations", "Id_Vaccination", vaccinations, 2);
            SelectWithArgs(masswithIdAnalys, "Analysis", "Id_Analys", analysis, 2);
            SelectWithArgs(masswithIdDiagnoses, "Diagnoses", "Id_Diagnosis", diagnoses, 1);
            SelectWithArgs(masswithIdSurveys, "Surveys", "Id_Survey", surveys, 1);
        }
        private void SelectWithArgs(string[] masswithId,string table,string nameofRow,ObservableCollection<string> desiredCollection,int numberofrow)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
            if (masswithId.Length>0)
            {
                foreach (string id_as_str in masswithId)
                {
                    int desired_id;
                    if (int.TryParse(id_as_str, out desired_id))
                    {
                        using (SqlConnection connect = new SqlConnection(connectionString))
                        {
                            connect.Open();
                            string sqlexp = String.Format("SELECT * FROM {0} WHERE {1} ='{2}'",table, nameofRow, desired_id);
                            SqlCommand cmd = new SqlCommand(sqlexp, connect);
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Read();
                                desiredCollection.Add(reader.GetValue(numberofrow).ToString());
                            }
                            reader.Close();
                        }
                    }
                }
            }
        }
    }
}
