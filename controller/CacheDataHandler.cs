using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class CacheDataHandler
    {
        private static readonly CacheDataHandler instance = new CacheDataHandler();
        /// <summary>
        /// 显式的静态构造函数⽤来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static CacheDataHandler() { }
        private CacheDataHandler() {
            this.sampleQueryService = new SampleQueryService();
            this.equipmentQueryService = new EquipmentQueryService();
        }
        public static CacheDataHandler Instance { get { return instance; } }

        private ISampleQueryService sampleQueryService;

        private IEquipmentQueryService equipmentQueryService;

        private List<string> vins;

        private bool isLoadVinData = false;

        private List<EquipmentBreiefViewModel> equipmentBreiefViewModels;

        private bool isLoadEquipmentData = false;

        public List<string> getVins()
        {
            if (isLoadVinData)
            {
                return this.vins;
            }

            return reloadVins();
        }

        public List<string> reloadVins()
        {
            this.vins = this.sampleQueryService.allSampleVins();
            this.isLoadVinData = true;
            return this.vins;
        }

        public void loadVins()
        {
            this.vins = this.sampleQueryService.allSampleVins();
            this.isLoadVinData = true;
        }

        public List<EquipmentBreiefViewModel> getCurUserEquipments()
        {
            if (isLoadEquipmentData)
            {
                return this.equipmentBreiefViewModels;
            }

            return this.reloadCurUserEquipments();
        }

        public List<EquipmentBreiefViewModel> reloadCurUserEquipments() {
            this.equipmentBreiefViewModels = this.equipmentQueryService.usingEquipments(FormSignIn.CurrentUser.Department);
            this.isLoadEquipmentData = true;
            return this.equipmentBreiefViewModels;
        }

        public void asyncLoadVins() {
            Thread exportWordThread = new Thread(loadVins);
            exportWordThread.IsBackground = true;
            exportWordThread.Start();
        }        
    }
}
