using System;
using System.Data;
using DataAccess.Interfaces;
using FirebirdSql.Data.FirebirdClient;

namespace DataAccess.Clases
{
    public class WorkWithFirebird :iWorkWithFirebird
    {
        private readonly FbConnection _fbConnection = Connectors.GetFirebirdConnection();


        public DataTable GetFbData(string sqlQuery)
        {
            if (string.IsNullOrEmpty(sqlQuery))
            {
                throw new ArgumentNullException();
            }
            var dt = new DataTable();
            FbCommand command = _fbConnection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sqlQuery;
            var fbdataadapter = new FbDataAdapter(command);
            try
            {
                _fbConnection.Open();
                fbdataadapter.Fill(dt);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString(), @"Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return null;
            }
            finally
            {
                _fbConnection.Close();
            }
            return dt;
        }
    }
}