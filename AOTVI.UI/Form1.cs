using AOTVI.BLL;
using AOTVI.Models;
using log4net.Util;
using AOTVI.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using System.Threading.Tasks;
//gittest

namespace AOTVI.UI
{
    public partial class Form1 : Form
    {
        private SystemService systemService = new SystemService();
        private DefectService defectService = new DefectService();
        private MesService mesService = new MesService();

        private List<Defect> defects = new List<Defect>();

        private string currentLot = "";
        private string currentUser = "OP001";

        private Image aoiImage;

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseClick += pictureBox1_MouseClick;

            txtLot.KeyDown += txtLot_KeyDown;
            btnConfirm.Click += btnConfirm_Click;

            // 订阅日志事件
            LogService.OnLog += AddLog;

        }

        private void AddLog(string msg)
        {
            if (lstLog.InvokeRequired)
            {
                lstLog.Invoke(new Action(() => AddLog(msg)));
                return;
            }

            //lstLog.Items.Insert(0, msg);
            lstLog.Items.Add(msg);

            // 限制最大条数
            if (lstLog.Items.Count > 200)
            {
                lstLog.Items.RemoveAt(0);
            }
            lstLog.TopIndex = lstLog.Items.Count - 1;
        }


        /// <summary>
        /// 扫码触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void txtLot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await  ScanLotAsync(txtLot.Text.Trim());
            }
        }

        private bool isLoading = false;
        private async Task ScanLotAsync(string lot)
        
        {
            if (isLoading) return;  

            isLoading = true;
            try
            {
                string msg;
                if (!defectService.ValidateLot(lot, out msg))
                {

                    lblMsg.Text = ($"{msg}:{lot}");
                    lblMsg.ForeColor = Color.Red;

                    LogService.Info($"Lot校验失败: {lot} - {msg}");

                    txtLot.Clear();
                    return;
                }



                currentLot = lot;
                LogService.Info($"Scan Lot: {lot}");
                defects = await Task.Run(() =>  defectService.LoadDefects(lot));

                // 测试用图片
                aoiImage = new Bitmap(800, 600);
                using (Graphics g = Graphics.FromImage(aoiImage))
                {
                    g.Clear(Color.Black);
                }

                pictureBox1.Image = aoiImage;

                lblMsg.Text = $"已加载 {defects.Count} 个缺陷";

                LogService.Info($"Scan Lot: {lot}");

                pictureBox1.Invalidate();
                txtLot.Clear();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "系统异常";
                LogService.Error("ScanLot异常", ex);
            }
            finally
            {
                isLoading = false;
            }
        }


        /// <summary>
        /// 绘制缺陷框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (aoiImage == null) return;

            Graphics g = e.Graphics;

            float scaleX = pictureBox1.Width / (float)aoiImage.Width;
            float scaleY = pictureBox1.Height / (float)aoiImage.Height;

            foreach (var d in defects)
            {
                Rectangle rect = new Rectangle(
                    (int)(d.X * scaleX),
                    (int)(d.Y * scaleY),
                    (int)(d.Width * scaleX),
                    (int)(d.Height * scaleY)
                );

                Color color = d.IsNG ? Color.Red : Color.Lime;

                using (Pen pen = new Pen(color, 2))
                {
                    g.DrawRectangle(pen, rect);
                }
            }
        }


        /// <summary>
        /// 点击切换 NG / OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (aoiImage == null) return;

            float scaleX = pictureBox1.Width / (float)aoiImage.Width;
            float scaleY = pictureBox1.Height / (float)aoiImage.Height;

            foreach (var d in defects)
            {
                Rectangle rect = new Rectangle(
                    (int)(d.X * scaleX),
                    (int)(d.Y * scaleY),
                    (int)(d.Width * scaleX),
                    (int)(d.Height * scaleY)
                );

                if (rect.Contains(e.Location))
                {
                    bool old = d.IsNG;
                    d.IsNG = !d.IsNG;

                    LogService.Info($"Lot:{currentLot} Defect[{d.Id}] {old}→{d.IsNG}");

                    break;
                }
            }

            pictureBox1.Invalidate();
        }

        /// <summary>
        /// 确认提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (defects == null || defects.Count == 0)
                {
                    lblMsg.Text = "无数据";
                    return;
                }

                // 1 保存数据库
                defectService.SaveResult(currentLot, defects);

                // 2 判断整板结果
                string result = defectService.GetLotResult(defects);

                // 3 日志
                LogService.Info($"Lot:{currentLot} Result:{result}");

                // 4 MES数据
                MesLotResult mes = new MesLotResult
                {
                    LotNumber = currentLot,
                    Result = result,
                    User = currentUser,
                    Defects = defects
                };

                // 5 上传MES
                bool ok = await mesService.UploadAsync(mes);

                if (ok)
                {
                    lblMsg.Text = "提交成功";
                    LogService.Info($"MES OK: {currentLot}");
                }
                else
                {
                    lblMsg.Text = "MES失败";
                    LogService.Error($"MES FAIL: {currentLot}", null);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "提交异常";
                LogService.Error("提交异常", ex);
            }
            // 清空
            defects.Clear();
            pictureBox1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogService.Info("启动");

            timer1.Interval = 1000; // 1秒
            timer1.Start();
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {

        }

        private void txtLot_TextChanged(object sender, EventArgs e)
        {

        }

        //private bool isChecking = false;
        private async void timer1_Tick(object sender, EventArgs e)
        {

            try
            {
                bool db = await Task.Run(() => systemService.CheckDb());

                lblDb.BackColor = db ? Color.Lime : Color.Red;

                bool ms = await Task.Run(() => systemService.CheckMes());

                lblmes.BackColor = ms ? Color.Lime : Color.Red;
            }
            catch (Exception ex)
            {
                LogService.Error("状态检测异常", ex);
            }

            //if (isChecking) return;
            //isChecking = true;

            //try
            //{
            //    bool db = await Task.Run(() => NetHelper.IsPortOpen("127.0.0.1", 1433));
            //    lblDb.BackColor = db ? Color.Lime : Color.Red;
            //}
            //catch (Exception ex)
            //{
            //    LogService.Error("检测异常", ex);
            //}
            //finally
            //{
            //    isChecking = false;
            //}




            //bool db = NetHelper.IsPortOpen("127.0.0.1", 1433);

            //lblDb.BackColor = db ? Color.Lime : Color.Red;

            //bool mes = NetHelper.IsPortOpen("127.120.0.1", 1433);

            //lblmes.BackColor = mes ? Color.Lime : Color.Red;
        }
    }
}