using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.common.utils;

namespace TaskManager.domain.valueobject
{
    public class SampleBrief
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// 样本类型
        /// </summary>
        /// <value>The id.</value>
        public string SampleType { get; set; }

        /// <summary>
        /// vin
        /// </summary>
        /// <value>The id.</value>
        public string Vin { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string CarType { get; set; }


        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string CarModel { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string Producer { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string PowerType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string EngineModel { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string EngineProducer { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string YNDirect { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string TransType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string DriverType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string FuelType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string Roz { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string Tirepressure { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string CanisterCode { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string CanisterType { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string CanisterProductor { get; set; }


        public bool equalsBack(SampleBrief another)
        {
            return another.Vin.Equals(this.Vin) &&
              another.SampleType.Equals(this.SampleType) &&
              another.CarType.Equals(this.CarType) &&
              another.CarModel.Equals(this.CarModel) &&
              another.Producer.Equals(this.Producer) &&
              another.PowerType.Equals(this.PowerType) &&
              another.EngineModel.Equals(this.EngineModel) &&
              another.EngineProducer.Equals(this.EngineProducer) &&
              another.YNDirect.Equals(this.YNDirect) &&
              another.TransType.Equals(this.TransType) &&
              another.DriverType.Equals(this.DriverType) &&
              another.FuelType.Equals(this.FuelType) &&
              another.Roz.Equals(this.Roz) &&
              another.Tirepressure.Equals(this.Tirepressure);
        }

        public void copyFrom(SampleBrief another)
        {
            this.SampleType = another.SampleType;
            this.Vin = another.Vin;
            this.CarType = another.CarType;
            this.CarModel = another.CarModel;
            this.Producer = another.Producer;
            this.PowerType = another.PowerType;
            this.EngineModel = another.EngineModel;
            this.EngineProducer = another.EngineProducer;
            this.YNDirect = another.YNDirect;
            this.TransType = another.TransType;
            this.DriverType = another.DriverType;
            this.FuelType = another.FuelType;
            this.Roz = another.Roz;
            this.Tirepressure = another.Tirepressure;

            this.CanisterCode = another.CanisterCode;
            this.CanisterType = another.CanisterType;
            this.CanisterProductor = another.CanisterProductor;
        }

        public bool equals(SampleBrief another, out List<FieldChangedState> changedStates)
        {
            changedStates = new List<FieldChangedState>();

            if (!another.SampleType.Equals(this.SampleType))
            {
                changedStates.Add(new FieldChangedState("样本类型", this.SampleType, another.SampleType));
            }
            if (!another.Vin.Equals(this.Vin))
            {
                changedStates.Add(new FieldChangedState("Vin", this.Vin, another.Vin));
            }
            if (!another.CarType.Equals(this.CarType))
            {
                changedStates.Add(new FieldChangedState("车辆类型", this.CarType, another.CarType));
            }
            if (!another.CarModel.Equals(this.CarModel))
            {
                changedStates.Add(new FieldChangedState("车辆型号", this.CarModel, another.CarModel));
            }
            if (!another.Producer.Equals(this.Producer))
            {
                changedStates.Add(new FieldChangedState("生产厂家", this.Producer, another.Producer));
            }
            if (!another.PowerType.Equals(this.PowerType))
            {
                changedStates.Add(new FieldChangedState("动力类型", this.PowerType, another.PowerType));
            }
            if (!another.EngineModel.Equals(this.EngineModel))
            {
                changedStates.Add(new FieldChangedState("发动机型号", this.EngineModel, another.EngineModel));
            }
            if (!another.EngineProducer.Equals(this.EngineProducer))
            {
                changedStates.Add(new FieldChangedState("发动机厂家", this.EngineProducer, another.EngineProducer));
            }
            if (!another.YNDirect.Equals(this.YNDirect))
            {
                changedStates.Add(new FieldChangedState("是否直喷", this.YNDirect, another.YNDirect));
            }
            if (!another.TransType.Equals(this.TransType))
            {
                changedStates.Add(new FieldChangedState("变速器形式", this.TransType, another.TransType));
            }
            if (!another.DriverType.Equals(this.DriverType))
            {
                changedStates.Add(new FieldChangedState("驱动形式", this.DriverType, another.DriverType));
            }
            if (!another.FuelType.Equals(this.FuelType))
            {
                changedStates.Add(new FieldChangedState("燃油种类", this.FuelType, another.FuelType));
            }
            if (!another.Roz.Equals(this.Roz))
            {
                changedStates.Add(new FieldChangedState("燃油标号", this.Roz, another.Roz));
            }

            if (!another.CanisterCode.Equals(this.CanisterCode))
            {
                changedStates.Add(new FieldChangedState("碳罐编号", this.CanisterCode, another.CanisterCode));
            }
            if (!another.CanisterType.Equals(this.CanisterType))
            {
                changedStates.Add(new FieldChangedState("碳罐型号", this.CanisterType, another.CanisterType));
            }
            if (!another.CanisterProductor.Equals(this.CanisterProductor))
            {
                changedStates.Add(new FieldChangedState("碳罐生产厂", this.CanisterProductor, another.CanisterProductor));
            }


            return Collections.isEmpty(changedStates);
        }

        public bool equalsSameSample(SampleBrief another)
        {
            if (!another.SampleType.Equals(this.SampleType))
            {
                return false;
            }
            if (!another.Vin.Equals(this.Vin))
            {
                return false;
            }

            return true;
        }
    }
}
