using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.application.viewmodel;
using TaskManager.domain.repository;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;

namespace TaskManager.application.Iservice
{
    /// <summary>
    /// 设备查询服务
    /// </summary>
    public class EquipmentQueryService : IEquipmentQueryService
    {
        /// <summary>
        /// 项目设备关系实体仓库
        /// </summary>
        private IItemEquipmentRepository itemEquipmentRepository;

        /// <summary>
        /// 设备仓库
        /// </summary>
        private IEquipmentRepository equipmentRepository;


        public EquipmentQueryService()
        {
            this.itemEquipmentRepository = new ItemEquipmentRepository();
            this.equipmentRepository = new EquipmentRepository();
        }

        /// <summary>
        /// 获取使用中的设备
        /// </summary>
        /// <param name="group">组别</param>
        /// <returns>设备简要信息</returns>
        public List<EquipmentBreiefViewModel> usingEquipments(string group)
        {
            List<EquipmentBreiefViewModel> results = new List<EquipmentBreiefViewModel>();

            List<EquipmentBreief> equipments = this.equipmentRepository.select("");
            if (equipments == null)
            {
                return results;
            }
            //模型转换
            results = equipments.Select(item => new EquipmentBreiefViewModel().fromEntity(item).setViewName(group)).ToList();

            //排序
            results.Sort((arg0, arg1) =>
            {
                //先按照状态排序
                EquipmentStateChn arg0State = EquipmentStateChn.报废;
                EquipmentStateChn arg1State = EquipmentStateChn.报废;
                Enum.TryParse(arg0.State, out arg0State);
                Enum.TryParse(arg1.State, out arg1State);

                int value = ((int)arg0State).CompareTo((int)arg1State);
                if (value != 0) {
                    return value;
                }

                //按照是否为本组排序
                int arg0Order = arg0.Group.Equals(group) ? 0 : 1;
                int arg1Order = arg1.Group.Equals(group) ? 0 : 1;
                return arg0Order.CompareTo(arg1Order);
            });

            return results;
        }

        /// <summary>
        /// 查询指定项目的设备信息
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="group">组别</param>
        /// <param name="locationNumber">定位编号</param> 
        /// <returns>设备简要信息集合</returns>
        public List<EquipmentLite> equipmentsOfItem(string itemName, string group, string locationNumber)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return new List<EquipmentLite>();
            }
            return this.itemEquipmentRepository.equipmentsOfItem(itemName,group,locationNumber);
        }

        /// <summary>
        /// 查询指定实验任务的设备信息
        /// </summary>
        /// <param name="testTaskId">实验任务Id</param>
        /// <returns>设备简要信息集合</returns>
        public List<EquipmentLite> equipmentsOfTestTask(int testTaskId) {
            if (testTaskId<=0)
            {
                return new List<EquipmentLite>();
            }
            return this.itemEquipmentRepository.equipmentsOfTestTask(testTaskId);
        }
    }
}
