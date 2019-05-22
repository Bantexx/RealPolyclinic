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
    public class ScheduleTableVM : BaseViewModel
    {
        public Doctor Doc { get; set; }
        ObservableCollection<int> mycoll { get; set; }
        public ScheduleTableVM(Doctor doc)
        {
            mycoll = new ObservableCollection<int>();
            SelectAppoint(doc);
        }
        private void SelectAppoint(Doctor Doctor)
        {           
            DateTime dt = DateTime.Now.Date;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Appointments WHERE Id_Doctor = @id_doc";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@id_doc", Doctor.Id_Doctor);
                cmd.Parameters.AddWithValue("@nowdate", dt);
                cmd.Parameters.AddWithValue("@tomorrowdate",dt.AddDays(1));
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var check = reader.GetInt32(3);
                            mycoll.Add(reader.GetInt32(3));
                        }
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
