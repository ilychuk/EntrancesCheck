using System.Data;

namespace DataAccess.Interfaces
{
    public interface IWorkWithMySql
    {
        /// <summary>
        /// Получить данные по запросу
        /// </summary>
        /// <param name="query">Запрос к базе</param>
        /// <returns></returns>
        DataTable GetDataTable(string query);
    }
}