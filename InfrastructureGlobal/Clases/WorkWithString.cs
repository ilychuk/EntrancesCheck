using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InfrastructureGlobal.Clases
{
    public static class WorkWithString
    {
        /// <summary>
        /// Преобразует список элементов в строку
        /// </summary>
        /// <param name="spisList">массив элементов</param>
        /// <param name="separator">символ, в который необходимо заключить каждый элемент списка</param>
        /// <returns></returns>
        public static string ConvertStringListToString(IEnumerable<string> spisList, string separator = "")
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (string stringValue in spisList)
            {
                if (i > 0) builder.Append(",");
                builder.Append(separator);
                builder.Append(stringValue);
                builder.Append(separator);
                i++;
            }
            return builder.ToString();
        }

        public static List<string> DatatableToStringList(DataTable inputDataTable)
        {
            List<string> resultList = new List<string>();
            foreach (DataRow row in inputDataTable.Rows)
            {
                resultList.Add(row["rez"].ToString());
            }
            return resultList;
        }

        public static string DatatableToString(DataTable dt)
        {
            string rez = dt.Rows[0][0].ToString();
            return rez;
        }

        public static string GetDateString(this DateTime date, string format, string separator = ".")
        {
            DateTime outDate;

            string convertDate = date.ToString();

            if (DateTime.TryParse(convertDate, out outDate))
            {
                var formatDate = outDate.ToString(format);

                return string.IsNullOrEmpty(separator) || separator == "."
                    ? formatDate
                    : formatDate.Replace(".", separator);
            }

            return string.Empty;
        }
    }
}