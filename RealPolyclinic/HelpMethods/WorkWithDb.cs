using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using RealPolyclinic.Models;

namespace RealPolyclinic.HelpMethods
{
    public class WorkWithDb
    {
        string connectionString;
        public WorkWithDb()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectToDb"].ConnectionString;
        }
        public bool InsertInDb(string query, Dictionary<string, object> ParamValue, Queue<SqlDbType> types)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                foreach (KeyValuePair<string, object> keyvalue in ParamValue)
                {
                    SqlParameter param = new SqlParameter()
                    {
                        ParameterName = keyvalue.Key,
                        Value = keyvalue.Value,
                        SqlDbType = types.Dequeue()
                    };
                    cmd.Parameters.Add(param);
                }
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно добавлены");
                    return true;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;

                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public bool UpdateDb(string query, Dictionary<string, object> ParamValue)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                foreach (KeyValuePair<string, object> keyvalue in ParamValue)
                {
                    cmd.Parameters.AddWithValue(keyvalue.Key, keyvalue.Value);
                }
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Данные успешно Обновлены");
                    return true;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public Patient SelectById(int id)
        {
            Patient InfoPat;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string sqlexp = "SELECT * FROM Patients WHERE Id_Patient =@Id_p";
                SqlCommand cmd = new SqlCommand(sqlexp, connect);
                cmd.Parameters.AddWithValue("@Id_p", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    InfoPat = new Patient()
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        SurName = reader.GetString(2),
                        Patronymic = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
                        Snils = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4),
                        Telephone = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5),
                        Address = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6),
                        Id_Card = reader.GetValue(7) == DBNull.Value ? 0 : reader.GetInt32(7),
                        Policy = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8),
                        Birthday = reader.GetValue(9) == DBNull.Value ? "" : reader.GetDateTime(9).ToString()
                    };
                    reader.Close();
                    return InfoPat;
                }
                else
                {
                    return null;
                }
            }
        }
           
    }
}
