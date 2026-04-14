using System;
using System.Configuration;
using System.Data.SqlClient;

namespace AOTVI.Common
{
    public class ConnHelper
    {
        public static (string ip, int port) GetDbEndpoint()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["DbConn"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(connStr))
                throw new Exception("DbConn 未配置");

            var builder = new SqlConnectionStringBuilder(connStr);

            string dataSource = builder.DataSource; // 例如：127.0.0.1,1433

            string ip = dataSource;
            int port = 1433; // 默认端口

            if (dataSource.Contains(","))
            {
                var arr = dataSource.Split(',');
                ip = arr[0];
                port = int.Parse(arr[1]);
            }

            return (ip, port);
        }


        /// <summary>
        /// 获取 MES 的 IP 和端口
        /// </summary>
        public static (string ip, int port) GetMesEndpoint()
        {
            string url = ConfigHelper.Get("MesUrl");

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                throw new Exception("MesUrl 格式错误");

            string ip = uri.Host;
            int port = uri.Port;

            // 标准化 localhost
            if (ip == "localhost")
                ip = "127.0.0.1";

            return (ip, port);
        }
    }
}