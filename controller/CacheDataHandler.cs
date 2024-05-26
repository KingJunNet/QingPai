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
using TaskManager.domain.valueobject;
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

        private List<string> carVins;

        private bool isLoadCarVinData = false;

        private List<string> canisterVins;

        private bool isLoadCanisterVinData = false;

        private List<EquipmentBreiefViewModel> equipmentBreiefViewModels;

        private bool isLoadEquipmentData = false;

        public List<string> getCarVins()
        {
            if (isLoadCarVinData)
            {
                return this.carVins;
            }

            return reloadCarVins();
        }

        public List<string> reloadCarVins()
        {
            this.carVins = this.sampleQueryService.allCarSampleVins();
            this.isLoadCarVinData = true;
            return this.carVins;
        }

        public void loadCarVins()
        {
            this.carVins = this.sampleQueryService.allCarSampleVins();
            this.isLoadCarVinData = true;
        }

        public List<string> getCanisterVins()
        {
            if (isLoadCanisterVinData)
            {
                return this.canisterVins;
            }

            return reloadCanisterVins();
        }

        public List<string> reloadCanisterVins()
        {
            this.canisterVins = this.sampleQueryService.allCanisterSampleVins();
            this.isLoadCanisterVinData = true;
            return this.canisterVins;
        }

        public void loadCanisterVins()
        {
            this.canisterVins = this.sampleQueryService.allCanisterSampleVins();
            this.isLoadCanisterVinData = true;
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
            Thread exportWordThread = new Thread(loadCarVins);
            exportWordThread.IsBackground = true;
            exportWordThread.Start();
        }

        public void addVin(string sampleType,string vin) {
            if (sampleType.Equals(SampleTypeChn.整车.ToString()))
            {
                if (!this.carVins.Contains(vin))
                {
                    this.carVins.Add(vin);
                }
            }
            else if (sampleType.Equals(SampleTypeChn.碳罐.ToString()))
            {
                if (!this.canisterVins.Contains(vin))
                {
                    this.canisterVins.Add(vin);
                }
            }
        }

        public void removeVin(string sampleType, string vin)
        {
            if (sampleType.Equals(SampleTypeChn.整车.ToString()))
            {
                if (this.carVins.Contains(vin))
                {
                    this.carVins.Remove(vin);
                }
            }
            else if (sampleType.Equals(SampleTypeChn.碳罐.ToString()))
            {
                if (this.canisterVins.Contains(vin))
                {
                    this.canisterVins.Remove(vin);
                }
            }
        }

        public void replaceVin(string sampleType, string oriVin,string newVin)
        {
            this.removeVin(sampleType,oriVin);
            this.addVin(sampleType,newVin);
        }
    }
}
