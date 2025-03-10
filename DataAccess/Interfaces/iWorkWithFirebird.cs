using System.Data;

namespace DataAccess.Interfaces
{
    public interface iWorkWithFirebird
    {
        DataTable GetFbData(string sqlQuery);
    }
}