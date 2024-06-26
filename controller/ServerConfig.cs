﻿using System;
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
        private static readonly ServerConfig instance = new ServerConfig();
        /// <summary>
        /// 显式的静态构造函数⽤来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static ServerConfig() { }
        private ServerConfig() {
            this.init();
        }
        public static ServerConfig Instance { get { return instance; } }

        private string blobServer;
        private string RootFolder = "轻排参数表服务器";
        private string paramTableFolder;
        private bool canConnectBlobServer;
        public string CannNotConnectBlobServerTips => $"无法连接至文件服务，该功能只支持内网环境！";

        private void init() {
            var sql = new DataControl();
            //文件系统服务
            blobServer = sql.BlobServer;
            paramTableFolder = $"\\\\{blobServer}\\{RootFolder}\\参数表";

            //测试连接blob服务
            this.testConectBlobServer();
        }

        public string ParamTableFolder {
            get { return this.paramTableFolder; }
        }

        public bool CanConnectBlobServer
        {
            get { return this.canConnectBlobServer; }
        }

        public bool CanNotConnectBlobServer
        {
            get { return !this.canConnectBlobServer; }
        }

        private void testConectBlobServer() {
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
            catch (Exception ex) {
                isConnect = false;
            }

            return isConnect;
        }     
    }
}
