using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.application.Iservice;
using TaskManager.application.service;
using TaskManager.application.viewmodel;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.infrastructure.db;

namespace TaskManager.controller
{
    public sealed class ServerConfig
    {
        public static readonly string TASK_MANAGER_APP_EXE_NAME = "TaskMangerSetup.msi";
        public static readonly string PROJECT_CODE_NAME = "轻排程序2.0.zip";

        public static readonly string LIMS_API_HOST_LAN = "http://10.12.48.2";
        public static readonly string LIMS_API_HOST_NET = "http://rmyc6395.xicp.net:17099";
        public static readonly string EQUIPMENT_API_HOST_NET = "https://its-equ.catarc.ac.cn";

        private static readonly ServerConfig instance = new ServerConfig();
        /// <summary>
        /// 显式的静态构造函数⽤来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static ServerConfig() { }
        private ServerConfig()
        {
            this.init();
        }
        public static ServerConfig Instance { get { return instance; } }

        private string blobServer;
        private string RootFolder = "轻排参数表服务器";
        private string paramTableFolder;
        private string softFolder;
        private bool canConnectBlobServer;
        public string CannNotConnectBlobServerTips => $"无法连接至文件服务，该功能只支持内网环境！";
        private string limsApiHost;

        private void init()
        {
            var sql = new DataControl();
            //文件系统服务
            blobServer = sql.BlobServer;
            paramTableFolder = $"\\\\{blobServer}\\{RootFolder}\\参数表";
            softFolder = $"\\\\{blobServer}\\{RootFolder}\\软件更新包";

            //测试连接blob服务
            this.testConectBlobServer();

            //配置任务服务api
            this.limsApiHost = LIMS_API_HOST_LAN;
        }

        public string ParamTableFolder
        {
            get { return this.paramTableFolder; }
        }

        public string SoftFolder
        {
            get { return this.softFolder; }
        }

        public string AppExePath
        {
            get { return $"{this.softFolder}\\{TASK_MANAGER_APP_EXE_NAME}"; }
        }

        public string AppExeDirectory
        {
            get { return $"{this.softFolder}"; }
        }

        public string CodeDirectory
        {
            get { return $"\\\\{blobServer}\\{RootFolder}\\项目代码"; }
        }

        public bool CanConnectBlobServer
        {
            get { return this.canConnectBlobServer; }
        }

        public bool CanNotConnectBlobServer
        {
            get { return !this.canConnectBlobServer; }
        }

        public string LimsApiHost
        {
            get { return this.limsApiHost; }
        }

        public bool IsCanConnectBlobServer()
        {
            return this.testConnection(this.blobServer, 1000);
        }

        private void testConectBlobServer()
        {
            this.canConnectBlobServer = testConnection(this.blobServer, 1000);
        }

        public bool testConnection(string targetIpAddress, int millisecondsTimeout = 1000)
        {
            bool isConnect = false;
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(targetIpAddress, millisecondsTimeout); // 1000 毫秒超时时间
                    isConnect = (reply.Status == IPStatus.Success);
                }
            }
            catch (Exception ex)
            {
                isConnect = false;
            }

            return isConnect;
        }
    }
}
