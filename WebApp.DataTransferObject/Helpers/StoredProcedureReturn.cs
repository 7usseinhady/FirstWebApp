using System.Data;

namespace WebApp.DataTransferObjects.Helpers
{
    public class StoredProcedureReturn
    {
        public StoredProcedureReturn()
        {
            dataTable = new DataTable();
            lParameters = new List<IDataParameter>();
        }
        public DataTable dataTable { get; set; }
        public List<IDataParameter> lParameters { get; set; }
    }
}
