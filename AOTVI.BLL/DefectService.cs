using AOTVI.DAL;
using AOTVI.Models;
using AOTVI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AOTVI.BLL
{
    public class DefectService
    {
        private DefectRepository repo = new DefectRepository();


        /// <summary>
        /// 模拟数据
        /// </summary>
        /// <returns></returns>
        private List<Defect> MockDefects()
        {
            var list = new List<Defect>();
            var rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                list.Add(new Defect
                {
                    Id = i,
                    X = rand.Next(50, 700),
                    Y = rand.Next(50, 500),
                    Width = rand.Next(30, 80),
                    Height = rand.Next(30, 80),
                    IsNG = false
                });
            }

            return list;
        }

        /// <summary>
        /// 根据 Lot 加载缺陷数据（数据库 → 内存对象）
        /// </summary>
        public async Task <List<Defect>> LoadDefectsAsync(string lot)
        {
            

            if (ConfigHelper.IsDemoMode())
            {
                LogService.Info($"数据库通信模拟中:{lot}");//模拟模式
                return MockDefects();
            }

            try
            {
                 DataTable dt = await  repo.GetByLotAsync(lot);

                if (dt == null || dt.Rows.Count == 0)
                    return new List<Defect>();

                List<Defect> list = new List<Defect>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];

                    Defect d = new Defect
                    {
                        Id = i,
                        X = Convert.ToSingle(r["CenterX"]),
                        Y = Convert.ToSingle(r["CenterY"]),
                        Width = Convert.ToSingle(r["Width"]),
                        Height = Convert.ToSingle(r["Height"]),
                        IsNG = false // 默认OK
                    };

                    list.Add(d);
                }

                return list;
            }
            catch (Exception ex)
            {
                    LogService.Error($"加载缺陷失败 Lot:{lot}", ex);
                    throw new Exception("加载缺陷数据失败", ex);
            }
        }

        /// <summary>
        /// 保存判定结果
        /// </summary>
        public void SaveResult(string lot, List<Defect> defects)
        {
            if (ConfigHelper.IsDemoMode())
            {
                LogService.Info($"数据库记录模拟中 Lot:{lot}");// 模拟模式
                return;
            }

            try
            {
                if (defects == null || defects.Count == 0)
                    return;

                repo.UpdateResult(lot, defects);
            }
            catch (Exception ex)
            {

                    LogService.Error($"DB保存失败 Lot:{lot}", ex);
                    throw new Exception("DB保存数据失败", ex);

            }
        }

        /// <summary>
        /// 判断整板是否NG
        /// </summary>
        public string GetLotResult(List<Defect> defects)
        {
            if (defects.Any(d => d.IsNG))
                return "NG";
            return "OK";
        }

        public  bool ValidateLot(string lot, out string msg)
        {
            msg = "";

            if (string.IsNullOrWhiteSpace(lot))
            {
                msg = "Lot为空";
                return false;
            }

            if (lot.Length != 10)
            {
                msg = "长度错误";
                return false;
            }

 
            if ((lot.Substring(0, 1) != "A"
                //&& lot.Substring(0, 1) != "7"
                //&& lot.Substring(0, 1) != "S")
                ////  || bool a=int.TryParse(txtLot.Text.Substring(8, 1),out a) == false
                //|| !int.TryParse(lot.Substring(8, 1), out _)
                //|| !char.IsLetter(lot.Substring(9, 1)[0]
                ))
            {
                msg = "格式错误";
                LogService.Info($"Scan Lot 失败: {lot}");
                return false ;
            }



            return true;
        }
    }
}