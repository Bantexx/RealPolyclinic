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
            ListPatients = new ObservableCollection<Patient>();
            SrchPat = new RelayCommand(obj => FindPatient());
            visibleGrid = "Hidden";
        }

        private void FindPatient()
        {
            if (!string.IsNullOrEmpty(PatForSrch)&&!string.IsNullOrWhiteSpace(PatForSrch))
            {
                visibleGrid = "Visible";
                ListPatients.Clear();
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    string sqlexp = String.Format("SELECT * FROM Patients WHERE Firstname ='{0}'", PatForSrch);
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
                                Patronymic = item[3].ToString()
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
    }
}
