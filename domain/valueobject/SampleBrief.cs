using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public String Vin { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <value>The id.</value>
        public string SampleType { get; set; }

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

        public bool equals(SampleBrief another)
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
              another.Roz.Equals(this.Roz);
        }
    }
}
