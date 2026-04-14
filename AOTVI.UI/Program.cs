using AOTVI.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using AOTVI.Common;

namespace AOTVI.UI
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            //  全局UI线程异常
            Application.ThreadException += (s, e) =>
            {
                LogService.Error("UI线程异常", e.Exception);
                MessageBox.Show("系统发生异常，请联系工程师", "错误",MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            // 非UI线程异常（Task / 线程池）
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogService.Error("非UI线程异常", e.ExceptionObject as Exception);
            };

            //XmlConfig
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 初始化日志
            //log4net.Config.XmlConfigurator.Configure();

            Application.Run(new Form1());


    }
    }
}
