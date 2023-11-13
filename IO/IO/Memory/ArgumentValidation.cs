using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ExpertLib
{
    /// <summary>
    /// ������֤��
    /// </summary>
    public static class ArgumentValidation
    {
        #region CheckForEmptyString
        /// <summary>
        /// ����Ƿ���ַ�
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="variableName"></param>
        public static void CheckForEmptyString(string variable, string variableName)
        {
            CheckForNullReference(variable, variableName);
            CheckForNullReference(variableName, "variableName");
            if (variable.Length == 0)
            {
                throw new ArgumentException(SR.ExceptionEmptyString(variableName));
				
            }
        }
        #endregion

        #region CheckForNullReference
        /// <summary>
        /// �������Ƿ�NULL����
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="variableName"></param>
        public static void CheckForNullReference(object variable, string variableName)
        {
            if (variableName == null)
            {
                throw new ArgumentNullException("variableName");
            }

            if (null == variable)
            {
                throw new ArgumentNullException(variableName);
            }
        }
        #endregion

        #region CheckForZeroBytes
        /// <summary>
        /// ��֤�������ֽ������Ƿ�Ϊ�ջ��㳤
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="variableName"></param>
        public static void CheckForZeroBytes(byte[] bytes, string variableName)
        {
            CheckForNullReference(bytes, "bytes");
            CheckForNullReference(variableName, "variableName");
            if (bytes.Length == 0)
            {
                throw new ArgumentException(SR.ExceptionByteArrayValueMustBeGreaterThanZeroBytes, variableName);
            }
        }
        #endregion

        #region CheckForZeroArray
        /// <summary>
        /// �������Ƿ��㳤��������ֵ���������׳�һ���쳣
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="paraName"></param>
        public static void CheckForZeroArray<T> (T[] value,string paraName)
        {
            CheckForNullReference(value, "paraName");
            CheckForNullReference(paraName, "paraName");
            if (value.Length == 0)
            {
                throw new ArgumentException(SR.ExceptionNullOrZeroArray(paraName));
            }
        }
        #endregion

        #region CheckExpectedType
        /// <summary>
        /// ��֤һ��ֵ�Ƿ��ڴ�������
        /// </summary>
        /// <param name="variable">����ֵ</param>
        /// <param name="type">�ڴ�������</param>
        public static void CheckExpectedType(object variable, Type type)
        {
            CheckForNullReference(variable, "variable");
            CheckForNullReference(type, "type");
            if (!type.IsAssignableFrom(variable.GetType()))   //��Ҫ�Ǽ̳���Ҳ����
            {
                throw new ArgumentException(SR.ExceptionExpectedType(type.FullName));
            }
        }
        #endregion

        #region CheckEnumeration
        /// <summary>
        /// ��֤һ��ֵ�ǲ���ö�����͵Ŀ�ѡֵ
        /// </summary>
        /// <param name="enumType">ö������</param>
        /// <param name="variable">��֤�ı���</param>
        /// <param name="variableName">����������</param>
        public static void CheckEnumeration(Type enumType, object variable, string variableName)
        {
            CheckForNullReference(variable, "variable");
            CheckForNullReference(enumType, "enumType");
            CheckForNullReference(variableName, "variableName");

            if (!Enum.IsDefined(enumType, variable))
            {
                throw new ArgumentException(SR.ExceptionEnumerationNotDefined(variable.ToString(), enumType.FullName), variableName);
            }
        }
        #endregion

        #region CheckValueLimit
        /// <summary>
        /// ���һ��ֵ�Ƿ��ں�������ݷ�Χ��
        /// </summary>
        /// <typeparam name="T">ֻ֧�־���IComparable�ӿڵĶ���</typeparam>
        /// <param name="value">Ҫ��ɵ�ֵ</param>
        /// <param name="paraName">��������</param>
        /// <param name="min">��Сֵ</param>
        /// <param name="max">���ֵ</param>
        public static void CheckValueLimit<T> (T value, string paraName, T min,T max)
            where T : System.IComparable
        {
            CheckForNullReference(paraName, "paraName");
            if (value.CompareTo(min) < 0)
            {
                throw new ArgumentOutOfRangeException(paraName);
            }
            else if (value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(paraName);
            }
            else
            {
            }
        }
        #endregion 

        #region CheckValueMinLimit
        /// <summary>
        /// ���һ��ֵ�Ƿ�С����Сֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="paraName"></param>
        /// <param name="min">��С����ֵ</param>
        public static void CheckValueMinLimit<T>(T value, string paraName, T min)
            where T : System.IComparable
        {
            CheckForNullReference(paraName, "paraName");
            if (value.CompareTo(min) < 0)
            {
                throw new ArgumentOutOfRangeException(paraName);
            }
        }
        #endregion

        #region CheckValueMaxLimit
        /// <summary>
        /// ���һ��ֵ�Ƿ�������ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="paraName"></param>
        /// <param name="max">���ֵ</param>
        public static void CheckValueMaxLimit<T>(T value, string paraName, T max)
            where T : System.IComparable
        {
            CheckForNullReference(paraName, "paraName");
            if (value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(paraName);
            }
        }

        #endregion

        #region CheckRegExpression
        /// <summary>
        /// ����һ���ַ����Ƿ�Ϸ���������ʽ.
        /// </summary>
        /// <param name="expression">��Ҫ���Եı��ʽ</param>
        /// <param name="paramName">��������</param>
        public static void CheckRegExpression(string expression,string paramName)
        {
            CheckForEmptyString(expression, "expresion");
            CheckForEmptyString(expression, "paramName");
            try
            {
                Regex exp = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            catch
            {
                throw new ArgumentException(SR.ExceptionNotValidRegexExpress(expression),paramName);
            }
        }    
        #endregion

        #region CheckForValidatePathName
        /// <summary>
        /// ���·���Ƿ���Ч�ַ���
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="variableName"></param>
        //public static void CheckForValidatePathName(string variable, string variableName)
        //{
        //    CheckForNullReference(variable, variableName);
        //    CheckForNullReference(variableName, "variableName");
        //    if (Path.)
        //    {
        //        throw new ArgumentException(SR.ExceptionInvalidatePathString(variableName));

        //    }
        //}
        #endregion
    }
}