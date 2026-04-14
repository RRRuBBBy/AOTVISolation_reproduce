using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AOTVI.Common
{
    public class NetHelper
    {


        public static bool IsPortOpen(string ip, int port, int timeout = 1000)
        {

            try
            {


                using (var client = new TcpClient())
                {
                    var task = client.ConnectAsync(ip, port);
                    return task.Wait(timeout) && client.Connected;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
