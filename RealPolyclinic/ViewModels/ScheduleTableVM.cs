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
    public class ScheduleTableVM : BaseViewModel
    {
        public Doctor Doc { get; set; }
        public ObservableCollection<InfoAppoint> mycoll { get; set; }
        public ICommand addVisit { get; set; }
        private BaseViewModel _ViewPatient;
        public BaseViewModel ViewPatient
        {
            get { return _ViewPatient; }
            set
            {
                _ViewPatient = value;
                OnPropertyChanged();
            }
        }
        private InfoAppoint _selectPat;
        public InfoAppoint selectedPatient
        {
            get { return _selectPat; }
            set
            {
                InfoAppoint IA = value as InfoAppoint;
                ViewPatient = new ShowMedCard(IA.Id_Pat);
                _selectPat = value;
                OnPropertyChanged();
            }
        }
        public ScheduleTableVM(Doctor doc)
        {
            mycoll = new ObservableCollection<InfoAppoint>();
            SelectAppoint(doc);
            addVisit = new RelayCommand(x => markVisit(x));
        }
        private void SelectAppoint(Doctor Doctor)
        {
            DateTime dt = DateTime.Now.Date;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Appointments WHERE Id_Doctor = @id_doc and Date=@nowdate";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@id_doc", Doctor.Id_Doctor);
                cmd.Parameters.AddWithValue("@nowdate", dt);
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string FullName;
                            using (SqlConnection connect1 = new SqlConnection(connectionString))
                            {
                                connect1.Open();
                                string sqlexp1 = "SELECT * FROM Patients WHERE Id_Patient=@Id_p";
                                SqlCommand cmd1 = new SqlCommand(sqlexp1, connect1);
                                cmd1.Parameters.AddWithValue("@Id_p", reader.GetInt32(4));
                                SqlDataReader reader1 = cmd1.ExecuteReader();
                                reader1.Read();
                                string checkPatrnomyic = reader1.GetString(3) == "" ? "" : reader1.GetString(3);
                                if (checkPatrnomyic == "")
                                {
                                    FullName = reader1.GetString(2) + " " + reader1.GetString(1);
                                }
                                else
                                {
                                    FullName = reader1.GetString(2) + "." + reader1.GetString(1).Substring(0, 1)+ "."+
                                        checkPatrnomyic.Substring(0,1);
                                }
                               
                            }
                              
                            mycoll.Add(new InfoAppoint {
                                Time=reader.GetTimeSpan(3).Hours.ToString()+":00",
                                Text=FullName,
                                Id_Pat=reader.GetInt32(4),
                                Id=reader.GetInt32(0),
                                Date=reader.GetDateTime(2),
                                Doc=new Doctor()
                                {
                                    Id_Doctor=reader.GetInt32(1)
                                }
                            });
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пациентов сегодня нет");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private void markVisit(object items)
        {
            object[] parameters = items as object[];
            Doctor dc = parameters[2] as Doctor;
            string time = parameters[3].ToString();
            int mynumb;
            if (time.Length == 4)
            {
                mynumb = Convert.ToInt32(time.Substring(0, 1));
            }
            else
            {
                mynumb = Convert.ToInt32(time.Substring(0, 2));

            }

            if (mynumb <= DateTime.Now.Hour)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    string sqlexp = "SELECT * FROM Visits WHERE Id_Patient = @Id_p and Date=@Date and Id_Doc=@Id_d ";
                    SqlCommand cmd = new SqlCommand(sqlexp, connect);
                    cmd.Parameters.AddWithValue("@Id_p", parameters[0]);
                    cmd.Parameters.AddWithValue("@Date", parameters[1]);
                    cmd.Parameters.AddWithValue("@Id_d", dc.Id_Doctor);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Patient was marked");
                    }
                    else
                    {
                        WorkWithDb wwd = new WorkWithDb();
                        string query = "INSERT INTO Visits(Date,Health_complaints,Id_Patient,Id_Doc) VALUES(@Date,@H_C,@Id_p,@Id_d)";
                        Dictionary<string, object> mydic = new Dictionary<string, object>()
                    {
                        {"@Date",parameters[1]},
                        {"@H_C","" },
                        {"Id_p",parameters[0]},
                        {"@Id_d",dc.Id_Doctor }
                    };
                        Queue<SqlDbType> que = new Queue<SqlDbType>();
                        que.Enqueue(SqlDbType.Date);
                        que.Enqueue(SqlDbType.NVarChar);
                        que.Enqueue(SqlDbType.Int);
                        que.Enqueue(SqlDbType.Int);
                        wwd.InsertInDb(query, mydic, que);
                    }
                }
            }
            else
            {
                MessageBox.Show("The time has not come yet");
            }
           
        }
    }
}
