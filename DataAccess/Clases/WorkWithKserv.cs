using System;
using System.Data;
using DataAccess.Interfaces;
using Oracle.ManagedDataAccess.Client;


namespace DataAccess.Clases
{
    public class WorkWithKserv : iWorkWithKserv
    {
        //private static readonly OleDbConnection Connection = Connectors.GetOracleConnectionCrew();
        private static readonly OracleConnection Connection = Connectors.GetOracleConnection();
        public DataTable GetOraDataTable(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            DataTable dt = new DataTable();
            OracleCommand command = Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = str;
            var oracleDataAdapter = new OracleDataAdapter();

            try
            {
                Connection.Open();
                oracleDataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                //WorkWithFile.WriteToFile(e.Message);
                // MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }

        public void UpdateOra(string str)
        {
            OracleCommand command = Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = str;
            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                // MessageBox.Show(e.Message);
            }
            finally
            {
                Connection.Close();

            }
        }

        public DataTable GetOraDataTableReader(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            DataTable dt = new DataTable();
            OracleCommand command = Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = str;
            command.InitialLONGFetchSize = -1;
           
            //  var oracleDataAdapter = new OracleDataAdapter();


            try
            {
                Connection.Open();
                OracleDataReader rdr = command.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception ex)
            {
                //WorkWithFile.WriteToFile(e.Message);
                // MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
    }
}