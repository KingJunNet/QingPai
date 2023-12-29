using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.infrastructure.db;

namespace TaskManager.controller
{
    public sealed class UIHelp
    {
        private static readonly UIHelp instance = new UIHelp();
        /// <summary>
        /// 显式的静态构造函数⽤来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static UIHelp() { }
        private UIHelp() {
            AbortEquipmentUasageRecordExportWork = false;
        }
        public static UIHelp Instance { get { return instance; } }

        public bool AbortEquipmentUasageRecordExportWork { get; set; }



    }
}
