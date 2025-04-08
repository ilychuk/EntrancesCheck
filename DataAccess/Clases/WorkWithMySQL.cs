using System;
using System.Data;
using DataAccess.Interfaces;
using MySql.Data.MySqlClient;

namespace DataAccess.Clases
{
    public class WorkWithMySql : IWorkWithMySql
    {
        private readonly MySqlConnection _mySqlConnection = Connectors.GetMySqlConnection();

        public DataTable GetDataTable(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException();
            }
            DataTable dt = new DataTable();

            MySqlCommand command = _mySqlConnection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            var mySqlAdapter = new MySqlDataAdapter(command);
            try
            {
                _mySqlConnection.Open();
                mySqlAdapter.Fill(dt);
            }
            catch (Exception)
            {
               // Console.WriteLine();
               //throw;
            }


           
            finally
            {
                _mySqlConnection.Close();
            }
            return dt;
           
        }
    }
}