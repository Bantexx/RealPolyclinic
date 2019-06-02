using RealPolyclinic.HelpMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RealPolyclinic.ViewModels
{
    public class MarkVisitVM : BaseViewModel
    {
        public ICommand doMark { get; set; }
        public string thistext { get; set; }
        public MarkVisitVM(int id_pat,int id_doc)
        {
            doMark = new RelayCommand(x => AddMark(id_pat, id_doc));
        }
        private void AddMark(int id_patient,int id_doctor)
        {
            if (thistext != ""&&CheckVisits(id_patient,id_doctor))
            {
                WorkWithDb wwd = new WorkWithDb();
                string query = "INSERT INTO Visits(Date,Health_complaints,Id_Patient,Id_Doc)" +
                    "VALUES(@Date,@H_c,@Id_p,@Id_d)";
                Dictionary<string, object> dict = new Dictionary<string, object>()
                {
                    {"@Date",DateTime.Now.Date },
                    {"@H_c",thistext },
                    {"@Id_p",id_patient },
                    {"@Id_d",id_doctor}
                };
                Queue<SqlDbType> types = new Queue<SqlDbType>();
                types.Enqueue(SqlDbType.Date);
                types.Enqueue(SqlDbType.NVarChar);
                types.Enqueue(SqlDbType.Int);
                types.Enqueue(SqlDbType.Int);
                wwd.InsertInDb(query, dict, types);
            }
        }
        private bool CheckVisits(int id_pat, int id_doc)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Visits WHERE Id_Patient =@Id_p And Id_Doc=@Id_d and Date=@Date";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@Id_p", id_pat);
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
    }
}
