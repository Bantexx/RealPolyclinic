using RealPolyclinic.HelpMethods;
using RealPolyclinic.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class DoctorWindowVM : BaseViewModel
    {
        public Doctor ActiveDoc { get; set; }
        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public ICommand showSched { get; set; }

        public DoctorWindowVM(User user)
        {
            showSched = new RelayCommand(x =>CurrentView= new ScheduleTableVM(ActiveDoc));
            SelectDoctor(user);
        }
        private void SelectDoctor(User actdoc)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Doctors WHERE Login = @login";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@login", actdoc.login);
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        ActiveDoc = new Doctor()
                        {
                            Id_Doctor = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Specialization = reader.GetInt32(2),
                            Experience = reader.GetInt32(3),
                            CabinetNumber = reader.GetInt16(4),
                            Login = reader.GetString(5)
                        };
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

    }
}
