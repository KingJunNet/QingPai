using System;

namespace ExpertLib.IO
{
    /// <summary>
    /// ɨ���������
    /// </summary>
    public interface IScanFilter
    {
        /// <summary>
        /// �Ƿ����ƥ��
        /// </summary>
        /// <param name="name">��Ҫƥ�������</param>
        /// <returns>���ƥ��Ϊtrue��֮Ϊfalse</returns>
        bool IsMatch(string name);
    }
}
