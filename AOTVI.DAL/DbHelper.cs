using System;
using System.Data;
using System.Data.SqlClient;
using AOTVI.Common;
namespace AOTVI.DAL
{
    public class DbHelper
    {
        //private string conn = "server=127.0.0.1;database=AAA;uid=groupadmin;password=0000;Connect Timeout=3";
        private string conn = ConfigHelper.GetConn("DbConn");
        public DataTable Query(string sql, SqlParameter[] ps)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(sql, con);
                    if (ps != null) da.SelectCommand.Parameters.AddRange(ps);
                    da.SelectCommand.CommandTimeout = 3;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    //LogService.Error("数据库连接异常", ex);
                    throw; 
                }
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