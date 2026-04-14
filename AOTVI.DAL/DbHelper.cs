using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AOTVI.Common;
namespace AOTVI.DAL
{
    public class DbHelper
    {
        //private string conn = "server=127.0.0.1;database=AAA;uid=groupadmin;password=0000;Connect Timeout=3";
        private string conn = ConfigHelper.GetConn("DbConn");
        public async Task <DataTable>  QueryAsync(string sql, SqlParameter[] ps)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (ps != null)
                        cmd.Parameters.AddRange(ps);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }

                //try
                //{
                //    SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //    if (ps != null) da.SelectCommand.Parameters.AddRange(ps);
                //    da.SelectCommand.CommandTimeout = 3;
                //    DataTable dt = new DataTable();
                //    da.Fill(dt);
                //    return dt;
                //}
                //catch (Exception ex)
                //{
                //    throw; 
                //}
            }
        }

        public int Execute(string sql, SqlParameter[] ps)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 3;
                if (ps != null) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}