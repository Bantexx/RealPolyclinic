using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class SearchPatientVM : MainVM
    {
        private ObservableCollection<Patient> _ListPatients;
        public ObservableCollection<Patient> ListPatients
        {
            get { return _ListPatients; }
            set
            {
                _ListPatients = value;
                OnPropertyChanged("ListPatients");
            }
        }
        public RelayCommand SrchPat { get; set; }

        public string[] typeofParameters { get; set; }
        public string selectedParam { get; set; }
        private string patforsrch;
        public string PatForSrch
        {
            get { return patforsrch; }
            set
            {
                patforsrch = value;
                OnPropertyChanged("PatForSrch");
            }
        }

        private string _visibleGrid;
        public string visibleGrid
        {
            get { return _visibleGrid; }
            set
            {
                _visibleGrid = value;
                OnPropertyChanged();
            }
        }

        public SearchPatientVM()
        {
            selectedParam = "";
            typeofParameters = new string[] { "Firstname", "Surname", "Policy" };
            ListPatients = new ObservableCollection<Patient>();
            SrchPat = new RelayCommand(obj => FindPatient(selectedParam));
            visibleGrid = "Hidden";
        }

        private void FindPatient(string strforsrch)
        {
            if (!string.IsNullOrEmpty(strforsrch))
            {
                if (!string.IsNullOrEmpty(PatForSrch) && !string.IsNullOrWhiteSpace(PatForSrch))
                {
                    visibleGrid = "Visible";
                    ListPatients.Clear();
                    string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
                    using (SqlConnection connect = new SqlConnection(connectionString))
                    {
                        connect.Open();
                        string sqlexp = String.Format("SELECT * FROM Patients WHERE {0} = '{1}'",strforsrch, PatForSrch);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlexp, connect);
                        DataSet dt = new DataSet();
                        adapter.Fill(dt);
                        if (dt.Tables[0].Rows.Count == 0)
                        {
                            PatForSrch = "";
                            MessageBox.Show("Данные не найдены");
                        }
                        else
                        {
                            foreach (DataRow item in dt.Tables[0].Rows)
                            {
                                ListPatients.Add(new Patient
                                {
                                    Id = Convert.ToInt32(item[0]),
                                    FirstName = item[1].ToString(),
                                    SurName = item[2].ToString(),
                                    Patronymic = item[3] == DBNull.Value ? "" : item[3].ToString(),
                                    Snils=item[4].ToString(),
                                    Telephone= item[5] == DBNull.Value ? "" : item[5].ToString(),
                                    Address=item[6]==DBNull.Value?"": item[6].ToString(),
                                    Id_Card =item[7]== DBNull.Value ? 0 : Convert.ToInt32(item[7]),
                                    Policy= item[8] == DBNull.Value ? "" : item[8].ToString(),
                                    Birthday = item[9] == DBNull.Value ? "" : item[9].ToString()
                                });
                            }
                            dt = null;
                            adapter.Dispose();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Заполните поле для поиска");
                }
            }
            else
            {
                MessageBox.Show("Выберите тип поиска");
            }
        }
    }
}
