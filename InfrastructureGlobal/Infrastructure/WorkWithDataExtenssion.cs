using System.Collections.Generic;
using System.Data;

namespace InfrastructureGlobal.Infrastructure
{
    public static class WorkWithDataExtenssion
    {
        public static bool IsNotEmptyOrNullList<T>(this IList<T> list) where T : class
        {
            return list != null && list.Count > 0;
        }

        public static bool IsNotEmptyOrNullDataTable(this DataTable dataTable) 
        {
            return dataTable != null && dataTable.Rows.Count > 0;
        }

        /// <summary>
        /// Проверяет содержимое строковой переменной
        /// </summary>
        /// <param name="str">Строка для проверки</param>
        /// <returns>Логический ответ</returns>
        public static bool IsNotEmptyOrNullString(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}
