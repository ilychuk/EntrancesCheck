using System.Data;


namespace DataAccess.Interfaces
{
    public interface iWorkWithKserv
    {
        /// <summary>
        /// Формирование DataTable На основе строки запроса
        /// </summary>
        /// <param name="str">Строка запроса</param>
        /// <returns></returns>
        DataTable GetOraDataTable(string str);

        /// <summary>
        /// Обновление базы на основе строки запроса
        /// </summary>
        /// <param name="str">Строка запроса</param>
        void UpdateOra(string str);

        /// <summary>
        /// Чтение информации из базы Oracle с помощью Рмдера
        /// </summary>
        /// <param name="str">Запрос к базе</param>
        /// <returns></returns>
        DataTable GetOraDataTableReader(string str);
    }
}