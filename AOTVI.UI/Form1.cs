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
using static System.Windows.Forms.AxHost;
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
        private string currentUser = "OP001";//后续添加注册系统

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
                e.Handled = true;          
                e.SuppressKeyPress = true; 
                await  ScanLotAsync(txtLot.Text.Trim());
            }
        }

        private void SetUiEnabled(bool enabled)
        {
            btnConfirm.Enabled = enabled;
            pictureBox1.Enabled = enabled;
        }


        private DateTime lastScanTime = DateTime.MinValue;
        private bool isLoading = false;
        private async Task ScanLotAsync(string lot)
        {
            if (!string.IsNullOrEmpty(currentLot))
            {
                lblMsg.Text = $"请先提交当前Lot: {currentLot}";
                lblMsg.ForeColor = Color.Red;

                LogService.Info($"未提交Lot禁止切换: 当前Lot={currentLot}, 扫描Lot={lot}");

                return;
            }

            if ((DateTime.Now - lastScanTime).TotalMilliseconds < 500)
                return;

            lastScanTime = DateTime.Now;

            if (isLoading) return;  

            isLoading = true;
            SetUiEnabled(false); // 开始加载
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
                //await defectService.LoadDefectsAsync(lot);
                //defects = await Task.Run(() =>  defectService.LoadDefects(lot));
                defects = await defectService.LoadDefectsAsync(lot);

                // 测试用图片，后续变更为model对象
                aoiImage?.Dispose();
                aoiImage = new Bitmap(800, 600);
                Random rand = new Random();
                int gray = rand.Next(0, 255);  // 0 ~ 255
                Color grayColor = Color.FromArgb(gray, gray, gray);
                using (Graphics g = Graphics.FromImage(aoiImage))
                {
                    g.Clear(grayColor);
                }

                pictureBox1.Image = aoiImage;

                lblMsg.Text = $"已加载 {defects.Count} 个缺陷";

                LogService.Info($"Scan Lot: {lot}已加载{defects.Count} 个缺陷");

                pictureBox1.Invalidate();
                txtLot.Clear();
            }
            catch (Exception ex)
            {
                txtLot.Clear();
                currentLot = "";


                lblMsg.Text = "系统异常,请联系工程师";
                LogService.Error("ScanLot异常", ex);
            }
            finally
            {
                
                isLoading = false;
                SetUiEnabled(true);  // 加载完成
                txtLot.Focus();

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

        private void Form1_Load(object sender, EventArgs e)
        {
            LogService.Info("启动");

            if (ConfigHelper.IsDemoMode())
            {
                lblNotice.Text = "模拟中：请输入A开头的任意10位字符并回车\r\n        点选提示框后点击确认";
            }
            else
            {
                lblNotice.Text = "非模拟状态，请注意数据库连接";
            }
            
            timer1.Interval = 5000; // 1秒
            timer1.Start();
        }


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


        }



        #region 提交流程
        private bool isSubmitting = false;
        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            if (isSubmitting) return;

            await ConfirmAsync();
        }
        private async Task ConfirmAsync()
        {
            if (!ValidateBeforeSubmit())
                return;

            isSubmitting = true;
            btnConfirm.Enabled = false;
            lblMsg.Text = "上传中，请稍候";
            txtLot.Enabled = false;

            try
            {
                
                var result = await ExecuteSubmitAsync();
                
                HandleSubmitResult(result);
                LogService.Info($"提交成功:{currentLot }");
                lblMsg.Text = "请扫码";
            }
            catch (Exception ex)
            {

                lblMsg.Text = "提交异常，请联系工程师";
                lblMsg.ForeColor = Color.Red;   
                LogService.Error("提交异常", ex);
            }
            finally
            {
                ResetState();
            }
        }
        private bool ValidateBeforeSubmit()
        {
            if (string.IsNullOrEmpty(currentLot))
            {
                lblMsg.Text = "请先扫码";
                return false;
            }

            if (defects == null || defects.Count == 0)
            {
                lblMsg.Text = "无数据";
                return false;
            }

            if (defects.All(d => !d.IsNG))
            {
                var r = MessageBox.Show(
                    "当前全部OK，是否确认？",
                    "提示",
                    MessageBoxButtons.YesNo);

                if (r != DialogResult.Yes)
                    return false;
            }

            return true;
        }
        private async Task<(bool success, string result)> ExecuteSubmitAsync()
        {
            // 1 保存数据库
            await Task.Run(() => defectService.SaveResult(currentLot, defects));

            // 2 获取结果
            string result = defectService.GetLotResult(defects);

            LogService.Info($"Lot:{currentLot} Result:{result}");

            // 3 组MES数据
            MesLotResult mes = new MesLotResult
            {
                LotNumber = currentLot,
                Result = result,
                User = currentUser,
                Defects = defects
            };

            // 4 上传MES
            bool ok = await mesService.UploadAsync(mes);

            return (ok, result);
        }
        private void HandleSubmitResult((bool success, string result) r)
        {
            if (r.success)
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
        private void ResetState()
        {
            isSubmitting = false;
            btnConfirm.Enabled = true;
            txtLot.Enabled = true; 
            txtLot.Text = "";
            txtLot.Focus();
            currentLot = "";
            defects.Clear();

            aoiImage?.Dispose();
            aoiImage = null;

            pictureBox1.Image = null;
            pictureBox1.Invalidate();
        }
        #endregion

    }
}