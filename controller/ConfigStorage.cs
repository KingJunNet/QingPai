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
using TaskManager.Model;

namespace TaskManager.controller
{
    public sealed class ConfigStorage
    {
        private static readonly ConfigStorage instance = new ConfigStorage();
        /// <summary>
        /// 显式的静态构造函数⽤来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static ConfigStorage() { }
        private ConfigStorage() {
            this.init();
        }
        public static ConfigStorage Instance { get { return instance; } }

        public Dictionary<ThirdAppName, string> ThirdAppRelativePathMap { get; set; }

        private void init() {
            this.ThirdAppRelativePathMap = new Dictionary<ThirdAppName, string>();
            this.ThirdAppRelativePathMap.Add(ThirdAppName.WEIGHT_CLIENT, @"称重客户端\SqliteManage.exe");
            this.ThirdAppRelativePathMap.Add(ThirdAppName.EVAPORATION_SYSTEM, @"蒸发客户端\ORVR.exe");
        }
    }
}
