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
using System.Windows.Controls;
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class AuthorizationVM
    {
        public string login { get; set; }
        public string pass { get; set; }
        public ICommand enter { get; set; }
        public User ActiveUser { get; set; }
        public AuthorizationVM()
        {
            enter = new RelayCommand(x => { if (CheckData(x)) ShowView(x); });
        }
        bool CheckData(object items)
        {
            object[] parameters = items as object[];
            var passwordBox = parameters[1] as PasswordBox;
            var pass = passwordBox.Password;
            if (!string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(login))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    string sqlexp = "SELECT * FROM Users WHERE login = @login and password = @pass";
                    SqlCommand cmd = new SqlCommand(sqlexp, connect);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            ActiveUser = new User()
                            {
                                id = reader.GetInt32(0),
                                login = reader.GetString(1),
                                status = reader.GetInt16(3)
                            };
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("User not found");
                            return false;

                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return false;
                    }

                }
            }
            else{
                MessageBox.Show("Enter data");
                return false;
            }
            
        }
        void ShowView(object items)
        {
            object[] parameters = items as object[];
            var currentWind = parameters[0] as Window;
            if (ActiveUser.status == 1)
            {
                MainVM mv = new MainVM();
                MainWindow mw = new MainWindow() { DataContext = mv };
                currentWind.Close();
                mw.Show();
            }else if(ActiveUser.status == 2)
            {
                DoctorWindowVM dwmv = new DoctorWindowVM(ActiveUser);
                DoctorWindow dw = new DoctorWindow() { DataContext = dwmv };
                currentWind.Close();
                dw.Show();               
            }
        }
    }
}
