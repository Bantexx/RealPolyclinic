using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;

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

    }
}
