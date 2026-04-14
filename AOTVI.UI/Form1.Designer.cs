namespace AOTVI.UI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtLot = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblDb = new System.Windows.Forms.Label();
            this.lblmes = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLot
            // 
            this.txtLot.Location = new System.Drawing.Point(902, 84);
            this.txtLot.Name = "txtLot";
            this.txtLot.Size = new System.Drawing.Size(579, 28);
            this.txtLot.TabIndex = 0;
            this.txtLot.TextChanged += new System.EventHandler(this.txtLot_TextChanged);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(902, 141);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(579, 69);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.UseVisualStyleBackColor = true;
            // 
            // lblMsg
            // 
            this.lblMsg.Location = new System.Drawing.Point(899, 28);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(216, 41);
            this.lblMsg.TabIndex = 2;
            this.lblMsg.Text = "请扫码";
            this.lblMsg.Click += new System.EventHandler(this.lblMsg_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.pictureBox1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(882, 688);
            this.panelMain.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(882, 688);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.ItemHeight = 18;
            this.lstLog.Location = new System.Drawing.Point(902, 272);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(579, 364);
            this.lstLog.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblDb
            // 
            this.lblDb.AutoSize = true;
            this.lblDb.Location = new System.Drawing.Point(1419, 28);
            this.lblDb.Name = "lblDb";
            this.lblDb.Size = new System.Drawing.Size(62, 18);
            this.lblDb.TabIndex = 5;
            this.lblDb.Text = "数据库";
            // 
            // lblmes
            // 
            this.lblmes.AutoSize = true;
            this.lblmes.Location = new System.Drawing.Point(1321, 28);
            this.lblmes.Name = "lblmes";
            this.lblmes.Size = new System.Drawing.Size(35, 18);
            this.lblmes.TabIndex = 6;
            this.lblmes.Text = "MES";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1517, 688);
            this.Controls.Add(this.lblmes);
            this.Controls.Add(this.lblDb);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtLot);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLot;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.Label lblmes;
    }
}

