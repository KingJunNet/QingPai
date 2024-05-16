using LabSystem.DAL;
using System;
using System.Collections.Generic;
using System.Data;
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
    public sealed class LogHandler
    {
        private static readonly LogHandler instance = new LogHandler();
        /// <summary>
        /// 显式的静态构造函数⽤来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static LogHandler() { }
        private LogHandler()
        {
            this.init();
        }
        public static LogHandler Instance { get { return instance; } }


        private void init()
        {

        }

        public void info(string author,string moudle, string message)
        {
            string nowTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string sql = $"insert into FormLog([Datetime],Name,[Module],Recorder) values ('{nowTime}','{author}','{moudle}','{message}')";
            SqlHelper.ExecuteNonquery(sql, CommandType.Text);
        }

        public void error(string author, string moudle, string errorMessage)
        {
            errorMessage =$"错误信息：{errorMessage}";
            string nowTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string sql = $"insert into FormLog([Datetime],Name,[Module],Recorder) values ('{nowTime}','{author}','{moudle}','{errorMessage}')";
            SqlHelper.ExecuteNonquery(sql, CommandType.Text);
        }
    }
}
