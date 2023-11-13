
namespace ExpertLib.IO
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ExpertLib;

    
    internal class StorageHelper
    {
        private static string[] UnallowedChar = new string[] { "!", ":", "/", "\\" };

        #region ValidateStorageName
        /// <summary>
        /// ��֤Storage��Stream�������Ƿ���ȷ
        /// </summary>
        /// <param name="name"></param>
        /// <remarks>
        /// �����ĵ��������洢����������
        ///��1�����Ʋ��ܳ���31�ַ��ĳ��ȡ�
        ///��2�������в��ܰ�����������/��\����Щ�ַ���
        ///��3������ʹ���κ�Ord(char)С��32���ַ���Ϊ���ַ�
        /// (4) . ��..���Ʊ�����
        /// </remarks>
        public static void ValidateStorageName(string name)
        {
            ArgumentValidation.CheckForEmptyString(name, "name");
            if (name.Length > 31)
                throw new ArgumentException(SR.ExceptionInvalidStorageName, "name");
            if(name[0]<0x32)
                throw new ArgumentException(SR.ExceptionInvalidStorageName, "name");
            if( (name == "." )|| (name =="..")) //����ʹ�ñ�������
                throw new ArgumentException(SR.ExceptionInvalidStorageName, "name");

            foreach (string s in UnallowedChar)
            {
                if(name.Contains(s))
                    throw new ArgumentException(SR.ExceptionInvalidStorageName, "name");
            }
        }
        #endregion

    }
}
