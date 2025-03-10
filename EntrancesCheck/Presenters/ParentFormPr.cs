using System.Data;
using DataAccess.Clases;
using DataAccess.Interfaces;


namespace EntrancesCheck.Presenters
{
    public class ParentFormPr
    {
        private void Start()
        {
            iWorkWithKserv _kserv = new WorkWithKserv();
            DataTable dt = _kserv.GetOraDataTable("select * from tb_sv t where t.tn=44877");

            iWorkWithFirebird _firebird = new WorkWithFirebird();
            DataTable dt2 = _firebird.GetFbData("select * from staff");
        }
    }
}