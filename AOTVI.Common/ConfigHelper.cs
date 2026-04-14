using System;
using System.Configuration;
using System.Net.Sockets;

namespace AOTVI.Common
{
    public class ConfigHelper
    {
        public static string Get(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"配置缺失: {key}");
            }

            return value;
        }

        public static string GetConn(string name)
        {
            var conn = ConfigurationManager.ConnectionStrings[name]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(conn))
                throw new Exception($"连接字符串缺失: {name}");

            return conn;
        }
    }


}