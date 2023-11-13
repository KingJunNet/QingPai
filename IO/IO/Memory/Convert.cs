//���ļ�����һЩ���õģ�CLRδ�ṩ��ת����
//Ŀǰ�汾��������ת����
//    DateTimeConvert  :   ʱ��ת����

namespace ExpertLib
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Runtime.InteropServices.ComTypes;

    #region DateTimeConvert
    /// <summary>
    /// ʱ��ת����
    /// </summary>
    /// <remarks>����FILETIME��DateTime��ת��</remarks>
    public static class DateTimeConvert
    {
        /// <summary>
        /// ��DateTimeת����FILETIME�ṹ
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.Runtime.InteropServices.ComTypes.FILETIME ToFILETIME(DateTime dt)
        {
            System.Runtime.InteropServices.ComTypes.FILETIME ft;
            long hFT1 = dt.ToFileTimeUtc();
            ft.dwLowDateTime = (int)(hFT1 & 0xFFFFFFFF);
            ft.dwHighDateTime = (int)(hFT1 >> 32);
            return ft;
        }

        /// <summary>
        /// ��FILETIMEת����DateTime
        /// </summary>
        /// <param name="ft"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(System.Runtime.InteropServices.ComTypes.FILETIME ft)
        {
            long hFT2 = (((long)ft.dwHighDateTime) << 32) + ft.dwLowDateTime;
            DateTime dt = DateTime.FromFileTimeUtc(hFT2);
            return dt;
        }
    }
    #endregion

    #region ValueConvert
    /// <summary>
    /// ��������ֵת���������ڴ��ʾ��ͬ�������ܸ���ʾ��ֵ��һ��
    /// </summary>
    /// <remarks>
    /// ��int ����uint�����ڴ�ռ�����һ����С�������ܱ�ʾ��ֵ�ǲ�һ���ģ��������ǿ���
    /// ͨ�����µĺ�������ת����������Ҫ�õ�CopyMemory֮��ĺ���
    /// </remarks>
    public static class ValueConvert
    {
        /// <summary>
        /// uint ת���ڴ��ʾ��ͬ��int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32(uint value)
        {
            byte[] bytes = System.BitConverter.GetBytes(value);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static uint ToUInt32(int value)
        {
            byte[] bytes = System.BitConverter.GetBytes(value);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static short ToInt16(ushort value)
        {
            byte[] bytes = System.BitConverter.GetBytes(value);
            return BitConverter.ToInt16(bytes, 0);
        }

        public static ushort ToUInt16(short value)
        {
            byte[] bytes = System.BitConverter.GetBytes(value);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static long ToInt64(ulong value)
        {
            byte[] bytes = System.BitConverter.GetBytes(value);
            return BitConverter.ToInt64(bytes, 0);
        }

        public static ulong ToUInt64(long value)
        {
            byte[] bytes = System.BitConverter.GetBytes(value);
            return BitConverter.ToUInt64(bytes, 0);
        }

    }
    #endregion
}
