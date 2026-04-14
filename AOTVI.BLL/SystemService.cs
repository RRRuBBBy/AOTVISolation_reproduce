using AOTVI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOTVI.BLL
{
    public class SystemService
    {
        public bool CheckDb()
        {
            var (ip, port) = ConnHelper.GetDbEndpoint();

            return NetHelper.IsPortOpen(ip, port);
            //return NetHelper.IsPortOpen("127.0.0.1", 1433);
        }

        public bool CheckMes()
        {
            var (ip, port) = ConnHelper.GetMesEndpoint();

            return NetHelper.IsPortOpen(ip, port);
            //return NetHelper.IsPortOpen("127.0.0.1", 1433);
        }


    }
}
