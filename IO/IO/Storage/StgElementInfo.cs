using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ComTypes = System.Runtime.InteropServices.ComTypes;
namespace ExpertLib.IO
{
    #region StgElementType
    /// <summary>
    /// �ڵ�����
    /// </summary>
    public enum StgElementType  :int
    {
        /// <summary>
        /// Indicates that the storage element is a storage object.
        /// </summary>
        Storage=1, 
      
        /// <summary>
        /// Indicates that the storage element is a stream object.
        /// </summary>
        Stream=2,
        
        /// <summary>
        /// Indicates that the storage element is a byte-array object.
        /// </summary>
        LockBytes=3,

        /// <summary>
        /// Indicates that the storage element is a property storage object.
        /// </summary>
        Property=4
    }
    #endregion

    /// <summary>
    /// ��������һ��������װ System.Runtime.InteropServices.ComTypes�е�STATSTG�ṹ������ʹ��
    /// </summary>
    public class StgElementInfo
    {
        private ComTypes.STATSTG stg_;

        public StgElementInfo(ComTypes.STATSTG stg)
        {
            stg_ = stg;
        }

        /// <summary>
        /// �ڵ�����
        /// </summary>
        public StgElementType StgType
        {
            get
            {
                return (StgElementType)stg_.type;
            }
        }

        /// <summary>
        /// �ڵ��������
        /// </summary>
        public string Name
        {
            get
            {
                return stg_.pwcsName;
            }
        }

        /// <summary>
        /// ���ش洢��������ʶ��
        /// </summary>
        public Guid CLSID
        {
            get
            {
                return stg_.clsid;
            }
        }

        /// <summary>
        /// ָ�����Ĵ�С
        /// </summary>
        public long Size
        {
            get
            {
                return stg_.cbSize;
            }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return DateTimeConvert.ToDateTime(stg_.ctime);
            }
        }

        /// <summary>
        /// ������ʱ��
        /// </summary>
        public DateTime LastAccessTime
        {
            get
            {
                return DateTimeConvert.ToDateTime(stg_.atime);
            }
        }

        /// <summary>
        /// ����޸�ʱ��
        /// </summary>
        public DateTime LastModifyTime
        {
            get
            {
                return DateTimeConvert.ToDateTime(stg_.mtime);
            }
        }

        //public void Dispose()
        //{
        //    Marshal.ReleaseComObject(this.stg_);
            
        //}
    }
}
