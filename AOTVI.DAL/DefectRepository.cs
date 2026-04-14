using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using AOTVI.Models;
using System.Threading.Tasks;

namespace AOTVI.DAL
{
    public class DefectRepository
    {
        DbHelper db = new DbHelper();

        public async Task<DataTable> GetByLotAsync(string lot)
        {
            return  await   db.QueryAsync(
                "SELECT * FROM Log_AVI_Defect WHERE LotNumber=@lot",
                new[] { new SqlParameter("@lot", lot) });
        }

        public void UpdateResult(string lot, List<Defect> list)
        {
            foreach (var d in list)
            {
                db.Execute(
                    "UPDATE Log_AVI_Defect SET Result=@r WHERE LotNumber=@lot AND CenterX=@x AND CenterY=@y",
                    new[]
                    {
                    new SqlParameter("@r", d.IsNG?"NG":"OK"),
                    new SqlParameter("@lot", lot),
                    new SqlParameter("@x", d.X),
                    new SqlParameter("@y", d.Y)
                    });
            }
        }
    }
}