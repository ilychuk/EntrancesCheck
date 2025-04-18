using FirebirdSql.Data.FirebirdClient;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;

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
            FbConnectionStringBuilder fbCon = new FbConnectionStringBuilder
            {
                Charset = "WIN1251",
                UserID = @"sysdba",
                Password = @"t2vkrc0fa",
                Database = @"ACSServ:C:\Program Files (x86)\PERCo\PERCo-S-20\BASE\SCD17K.FDB",
                ServerType = 0
            };
            //используемая кодировка
            //логин
            //пароль
            //путь к файлу базы данных
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
            string connectString = string.Format(
                "User Id={0};Password={1};Data Source=  " +
                "(DESCRIPTION = " +
                "(ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.0.94)(PORT = 1521)) " +
                "(CONNECT_DATA = (SID = HR2)))", "S.ILUCHIK", "111");

            return new OracleConnection(connectString);
        }


    //    public static OracleConnection GetOracleConnection()
    //    {
    //        string connectString = @"Data Source=(DESCRIPTION=
    //(ADDRESS=
    //  (PROTOCOL=TCP)
    //  (HOST=dbkserv64)
    //  (PORT=1521)
    //)
    //(CONNECT_DATA=
    //  (SID=kadr)
    //)
    //);User ID = crew;Password = brivzoup38901";

    //        return new OracleConnection(connectString);
    //    }




        public static MySqlConnection GetMySqlConnection()
        {
            return new MySqlConnection("server=10.111.0.124;user id=user_p;database=perco_4;port=49001;password=1111");
        }
    }
}