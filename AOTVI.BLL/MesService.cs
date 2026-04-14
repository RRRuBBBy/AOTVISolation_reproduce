using AOTVI.Common;
using AOTVI.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AOTVI.BLL
{
    public class MesService
    {
        private static readonly HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) };  

        private readonly string url = ConfigHelper.Get("MesUrl");
        //private readonly string url = "http://mes/api/aoi";


        public async Task<bool> UploadAsync(MesLotResult data)
        {
            if (ConfigHelper.IsDemoMode())
            {
                LogService.Info("MES通信模拟中");// 模拟模式
                return true;
            }

            try
            {
                string json = JsonConvert.SerializeObject(data);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {

                LogService.Error("MES上传异常", ex);
                throw new Exception("MES接口异常");
                //LogService.Error("MES上传异常", ex);
                //throw new Exception("MES接口异常");
            }
        }

    }
}