using System.Data.OleDb;
using FirebirdSql.Data.FirebirdClient;
using Oracle.ManagedDataAccess.Client;

namespace DataAccess.Clases
{
    public class Connectors
    {
        /// <summary>
        /// Получение пожключения к серверу FIREBIRD как пользователь SYSDBA
        /// </summary>
        /// <returns>Переменная подключения</returns>
        public static FbConnection GetFirebirdConnection()
        {
            FbConnectionStringBuilder fbCon = new FbConnectionStringBuilder();
            fbCon.Charset = "WIN1251"; //используемая кодировка
            fbCon.UserID = @"sysdba"; //логин
            fbCon.Password = @"t2vkrc0fa"; //пароль
            fbCon.Database = @"ACSServ:C:\Program Files (x86)\PERCo\PERCo-S-20\BASE\SCD17K.FDB"; //путь к файлу базы данных
            fbCon.ServerType = 0;
            return new FbConnection(fbCon.ToString());
        }
        /// <summary>
        /// Получение пожключения к серверу PERS как пользователь CREW#
        /// </summary>
        /// <returns>Переменная подключения</returns>
        //public static OleDbConnection GetOracleConnectionCrew()
        //{
        //    return new OleDbConnection("Provider=MSDAORA;Data Source=pers;Persist Security Info=True;User ID=crew; Password=brivzoup38901");
        //}

        public static OracleConnection GetOracleConnection()
        {
            string connectString = string.Format(@"Data Source=(DESCRIPTION=
    (ADDRESS=
      (PROTOCOL=TCP)
      (HOST=dbkserv64)
      (PORT=1521)
    )
    (CONNECT_DATA=
      (SID=kadr)
    )
    );User ID = crew;Password = brivzoup38901");

            return new OracleConnection(connectString);
        }
    }
}