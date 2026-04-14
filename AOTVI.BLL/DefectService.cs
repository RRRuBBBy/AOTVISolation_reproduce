using AOTVI.DAL;
using AOTVI.Models;
using AOTVI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AOTVI.BLL
{
    public class DefectService
    {
        private DefectRepository repo = new DefectRepository();


        /// <summary>
        /// 根据 Lot 加载缺陷数据（数据库 → 内存对象）
        /// </summary>
        public List<Defect> LoadDefects(string lot)
        {
            try
            {
                DataTable dt = repo.GetByLot(lot);

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
                throw new Exception("加载缺陷数据失败",ex); // 给UI看的
            }
        }

        /// <summary>
        /// 保存判定结果
        /// </summary>
        public void SaveResult(string lot, List<Defect> defects)
        {
            try
            {
                if (defects == null || defects.Count == 0)
                    return;

                repo.UpdateResult(lot, defects);
            }
            catch (Exception ex)
            {
                LogService.Error($"保存结果失败 Lot:{lot}", ex);
                throw new Exception("保存数据失败");
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

            if (lot.Length < 10)
            {
                msg = "长度不足";
                return false;
            }

 
            if ((lot.Substring(0, 1) != "8"
                & lot.Substring(0, 1) != "7"
                & lot.Substring(0, 1) != "S")
                //  || bool a=int.TryParse(txtLot.Text.Substring(8, 1),out a) == false
                || !int.TryParse(lot.Substring(8, 1), out _)
                || !char.IsLetter(lot.Substring(9, 1)[0]))
            {
                msg = "格式错误";
                LogService.Info($"Scan Lot 失败: {lot}");
                return false ;
            }


            return true;
        }
    }
}