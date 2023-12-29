using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.viewmodel;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.service;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;

namespace TaskManager.application.Iservice
{
    /// <summary>
    /// 设备命令服务
    /// </summary>
    public class EquipmentCommandService : IEquipmentCommandService
    {
        /// <summary>
        /// 项目设备关系实体仓库
        /// </summary>
        private IItemEquipmentRepository itemEquipmentRepository;

        /// <summary>
        /// 设备仓库
        /// </summary>
        private IEquipmentRepository equipmentRepository;


        public EquipmentCommandService()
        {
            this.itemEquipmentRepository = new ItemEquipmentRepository();
            this.equipmentRepository = new EquipmentRepository();
        }

        /// <summary>
        /// 创建项目设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别信息</param>
        /// <param name="equipmentCodes">设备编号集合</param>
        /// <returns>void</returns>
        public void createItemEquipments(string itemName, string group, List<string> equipmentCodes)
        {
            this.updateItemEquipments(itemName, group, equipmentCodes);
        }

        /// <summary>
        /// 更新项目设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别信息</param>
        /// <param name="equipmentCodes">设备编号集合</param>
        /// <returns>void</returns>
        public void updateItemEquipments(string itemName, string group, List<string> equipmentCodes)
        {
            this.itemEquipmentRepository.removeEquipmentsOfItem(itemName, group);
            DateTime nowTime = DateTime.Now;
            equipmentCodes.ForEach(item =>
            {
                ItemEquipmentEntity entity = new ItemEquipmentEntity(itemName,group,item,UseHolder.Instance.CurrentUser.Name,nowTime);
                itemEquipmentRepository.save(entity);
            });
        }
    }
}
