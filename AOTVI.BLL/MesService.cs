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
        private static readonly HttpClient client = new HttpClient();  

        private readonly string url = ConfigHelper.Get("MesUrl");
        //private readonly string url = "http://mes/api/aoi";


        public async Task<bool> UploadAsync(MesLotResult data)
        {
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
            }
        }

        //private readonly string url = "http://mes/api/aoi"; // 

        //public async Task<bool> UploadAsync(MesLotResult data)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string json = JsonConvert.SerializeObject(data);

        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await client.PostAsync(url, content);

        //        return response.IsSuccessStatusCode;
        //    }
        //}
    }
}