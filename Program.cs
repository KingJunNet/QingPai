using DevExpress.XtraSplashScreen;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;


namespace TaskManager
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            RunApplication();
        }

        private static void RunApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);



            if (IsRunning())
            {
                MessageBox.Show("系统正在运行");
                return;
            }
            bool IsRunning()
            {
                Process current = default(Process);
                current = System.Diagnostics.Process.GetCurrentProcess();
                Process[] processes = null;
                processes = System.Diagnostics.Process.GetProcessesByName(current.ProcessName);

                Process process = default(Process);

                foreach (Process tempLoopVar_process in processes)
                {
                    process = tempLoopVar_process;

                    if (process.Id != current.Id)
                    {
                        if (System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                        {
                            return true;

                        }

                    }
                }
                return false;

            }

            var sql = new DataControl();

            if (!sql.TestConnection(2, out _))
            {
                var frmDataBase = new FormDataBase("连接失败");
                frmDataBase.ShowDialog();
                if (frmDataBase.DialogResult != DialogResult.OK)
                    return;
            }

            var signIn = new FormSignIn();
            signIn.ShowDialog();
            if (signIn.DialogResult != DialogResult.OK)
                return;

            Log.Begin = DateTime.Now;
            Log.Last = DateTime.Now;
            Log.i("启动界面打开","登录");

            SplashScreenManager.ShowForm(typeof(SplashScreen1));


        


            var mainWindow = new Form1();

            SplashScreenManager.CloseForm();
            Log.i("启动界面关闭","登录");

            Application.Run(mainWindow);
          
            //Application.Run(new Delmia());
        }
    }
}
