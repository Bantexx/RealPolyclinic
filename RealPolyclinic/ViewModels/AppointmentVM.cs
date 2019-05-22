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
using System.Windows.Controls;
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class AppointmentVM : BaseViewModel
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;

        public string[] typeofDoctors { get; set; }
        public string selectedType { get; set; }
        public ICommand tryit { get; set; }
        public ICommand doRecord { get; set; }

        string _showelem;
        public string ShowElem
        {
            get { return _showelem; }
            set
            {
                _showelem = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<Doctor> docs;
        public ObservableCollection<Doctor> Docs
        {
            get { return docs; }
            set
            {
                docs = value;
                OnPropertyChanged();
            }
        }

        public string[] times { get; set; }

        ObservableCollection<List<InfoAppoint>> _ScheduleTable;
        public ObservableCollection<List<InfoAppoint>> ScheduleTable
        {
            get { return _ScheduleTable; }
            set
            {
                _ScheduleTable = value;
                OnPropertyChanged();
            }
        }

        DateTime _selecteddate;
        public DateTime SelectedDate
        {
            get { return _selecteddate; }
            set
            {
                if (value != _selecteddate)
                {
                    TakeDoctors(value);
                    TakeSchedule(value);
                    ShowDataText = value.Date.ToString("dd/MM/yy")+"-"+value.Date.DayOfWeek;
                    _selecteddate = value;
                    OnPropertyChanged();
                }               
            }
        }

        string _ShowDataText;
        public string ShowDataText
        {
            get { return _ShowDataText; }
            set
            {
                _ShowDataText = value;
                OnPropertyChanged();
            }
        }


        public AppointmentVM()
        {
            SelectedDate = DateTime.Today;
            ShowElem = "Hidden";
            typeofDoctors = TakeTypesofDoctors();
            times = TimeTable();
            tryit = new RelayCommand(x=> { TakeDoctors(SelectedDate); TakeSchedule(SelectedDate);});
            doRecord = new RelayCommand(x =>MakeAppoint(x));
        }

        private void TakeDoctors(DateTime date)
        {
            string Selected_type = selectedType;
            int NeedDayofWeek = Convert.ToInt32(date.DayOfWeek);
            Docs = new ObservableCollection<Doctor>();
            if (Selected_type != " " && Selected_type != null)
            {
                ShowElem = "Visible";
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    string sqlexp =
                    String.Format("SELECT * FROM Doctors WHERE Specialization = (SELECT Id FROM Specializations WHERE Title ='{0}') " +
                    "AND Id_Doctor IN (SELECT Id_Doctor FROM Schedule WHERE DayWeek LIKE '%{1}%')", Selected_type, NeedDayofWeek);
                    SqlCommand cmd = new SqlCommand(sqlexp, connect);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Docs.Add(new Doctor
                            {
                                Id_Doctor = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Specialization= reader.GetInt32(2),
                                Experience = reader.GetValue(3) == DBNull.Value ? 0 : reader.GetInt32(3),
                                CabinetNumber = reader.GetValue(4) == DBNull.Value ? 0 : reader.GetInt32(4),
                                Login = reader.GetValue(5)==DBNull.Value?"" : reader.GetString(5)
                            });
                        }
                    }                  
                }               
            }
        }
        private void TakeSchedule(DateTime selectedDate)
        {
            ScheduleTable = new ObservableCollection<List<InfoAppoint>>();
            List<InfoAppoint> templistInfo;
            string textinBtn="";
            string datetime;
            DateTime dt;
            TimeSpan ts;
            if (AvailbaleRecord(selectedDate))
            {
                if (selectedType != "" && selectedType != null && Docs.Count > 0)
                {
                    for (int i = 0; i < times.Length; i++)
                    {
                        templistInfo = new List<InfoAppoint>();
                        datetime = times[i];
                        for (int j = 0; j < Docs.Count; j++)
                        {
                            bool isWork = CheckTimeWork(datetime, Docs[j].Id_Doctor);
                            using (SqlConnection connect = new SqlConnection(connectionString))
                            {
                                connect.Open();
                                string sqlexp =
                                    String.Format("SELECT * FROM Appointments WHERE Id_Doctor='{0}' " +
                                    "and (Date = '{1}' and Time='{2}')",
                                    Docs[j].Id_Doctor, selectedDate.Date.ToString("yyyy-MM-dd"), datetime);
                                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                                SqlDataReader reader = cmd.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    dt = reader.GetDateTime(2);
                                    ts = reader.GetTimeSpan(3);
                                    if (!isWork)
                                    {
                                        textinBtn = "Not Work";
                                    }
                                    else if (dt.Date < DateTime.Now.Date)
                                    {
                                        textinBtn = "Blocked";
                                    }
                                    else if (dt.Date <= DateTime.Now.Date && Convert.ToInt32(datetime.Substring(0, datetime.LastIndexOf(':'))) < DateTime.Now.Hour)
                                    {
                                        textinBtn = "Time is up";
                                    }
                                    else if (ts.Hours == Convert.ToInt32(datetime.Substring(0, datetime.LastIndexOf(':'))))
                                    {
                                        textinBtn = "Booked";
                                    }
                                    else
                                    {
                                        textinBtn = "Free";
                                    }
                                }
                                else
                                {
                                    if (!isWork)
                                    {
                                        textinBtn = "Not Work";
                                    }
                                    else if (selectedDate.Date < DateTime.Now.Date)
                                    {
                                        textinBtn = "Blocked";
                                    }
                                    else if (selectedDate.Date <= DateTime.Now.Date && Convert.ToInt32(datetime.Substring(0, datetime.LastIndexOf(':'))) < DateTime.Now.Hour)
                                    {
                                        textinBtn = "Time is up";
                                    }
                                    else
                                    {
                                        textinBtn = "Free";
                                    }
                                }
                                templistInfo.Add(new InfoAppoint
                                {
                                    Doc = Docs[j],
                                    Text = textinBtn,
                                    Time = datetime,
                                    Date = selectedDate.Date
                                });
                            }
                        }
                        ScheduleTable.Add(templistInfo);
                    }
                }
            }
            else
            {
                MessageBox.Show("Запись на эту дату еще не доступна");
            }
        }
        private string[] TakeTypesofDoctors()
        {
            List<string> list_types = new List<string>();
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT Title FROM Specializations";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list_types.Add(reader.GetValue(0).ToString());
                    }
                }

            }
            return list_types.ToArray();
        }
        private string[] TimeTable()
        {
            List<string> lst = new List<string>();
            for(int i = 8; i <= 17; i++)
            {
                lst.Add(i.ToString() + ":00");
            }
            return lst.ToArray();
        }
        private bool CheckTimeWork(string currentTime,int id_doc)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp =
                String.Format("SELECT * FROM Schedule WHERE Id_Doctor='{0}'", id_doc);
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int startHour = Convert.ToInt32(reader.GetValue(3));
                    int endHour = Convert.ToInt32(reader.GetValue(4));
                    int convertime = Convert.ToInt32(currentTime.Substring(0, currentTime.LastIndexOf(':')));
                    if (convertime >= startHour && convertime <= endHour)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
        }
        private void MakeAppoint(object record)
        {
            InfoAppoint myRecord = (InfoAppoint)record;
            if(myRecord.Text=="Free" || myRecord.Text=="Booked")
            {
                DoRecord dr = new DoRecord(myRecord);
                dr.ShowDialog();
            }
            else
            {
                MessageBox.Show("Record anavailable");
            }
           
        }
        private bool AvailbaleRecord(DateTime date)
        {
            var nowdate = DateTime.Now.Date;
            if (nowdate.AddDays(30)>=date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
