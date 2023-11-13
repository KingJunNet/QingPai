using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

//private void TestINI_Load(object sender, System.EventArgs e)
//  {
//   //����һ��INIFile���󣬲���Ϊ�ļ�·������������������Զ�������
//   INIFile inf=new INIFile(@"D:\����Ŀ¼\VSPP\VSPPServer\bin\Debug\COMPILED.INI");
//   //��ʾINI���õĽṹ
//   foreach(string k in inf.Segments.Keys)
//   {
//    TreeNode o=new TreeNode(k);
//    INISegment s=inf.Segments[k]; //ȡ����ǰ���ý�
//    foreach(string k1 in s.Items.Keys)
//    {
//     TreeNode o1=new TreeNode(k1+" = "+s.Items[k1].Value); //�������ý���ÿ��������
//     o.Nodes.Add(o1);
//    }
//    treeView1.Nodes.Add(o);
//   }
//   //���һ������
//   inf.Segments.Add("Test"); //���Test��
//   inf.Segments["Test"].Items.Add("IP","192.168.0.1"); //���һ��������
//   //�������һ��������
//   inf.Segments["����"].Items["Host"].Value="localhost"; // ��������������һ����Ч��
//   //��ȡҲ��ͬ���ķ���
//   MessageBox.Show(inf.Segments["����"].Items["Host"].Value);
//   //�������������������������ؿ��ַ��������������
//   MessageBox.Show(inf.Segments["����"].Items["Server"].Value);
//   //���һ�����ý����������������
//   //inf.Segments["Action"].Clear();
//  }

namespace ExpertLib.IO
{
    #region INI�ļ�������
    /// <summary>
    /// ���ý�
    /// </summary>
    public class INISegment
    {
        private string __Name;
        private INISegments __Owner;
        /// <summary>
        /// �����������
        /// </summary>
        public INIItems Items;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="o">Owner</param>
        /// <param name="vName">���ý�����</param>
        public INISegment(INISegments o, string vName)
        {
            __Owner = o;
            __Name = vName;
            Items = new INIItems(this);
            o.Owner.GetSegment(this);
        }
        /// <summary>
        /// ��ȡ���ýڵ�����
        /// </summary>
        public string Name
        {
            get { return __Name; }
        }
        /// <summary>
        /// ��ȡSegment��Owner��INISegments����
        /// </summary>
        public INISegments Owner
        {
            get { return __Owner; }
        }
        /// <summary>
        /// �������������
        /// </summary>
        public void Clear()
        {
            __Owner.Owner.WriteSegment(__Name, "\0\0");
        }
    }
    
    /// <summary>
    /// ���ýڼ���
    /// </summary>
    public class INISegments : DictionaryBase
    {
        private INIFile __Owner;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="o">Owner</param>
        public INISegments(INIFile o)
        {
            __Owner = o;
        }
        /// <summary>
        /// ��ȡ�˶����Owner��INIFile��
        /// </summary>
        public INIFile Owner
        {
            get { return __Owner; }
        }
        /// <summary>
        /// ���һ���Ѿ����ڵ����ý�
        /// </summary>
        /// <param name="o">���ýڶ���</param>
        public void Add(INISegment o)
        {
            if (!Dictionary.Contains(o.Name))
                Dictionary.Add(o.Name, o);
        }
        /// <summary>
        /// ���һ�����ܲ����ڵ����ýڣ�����һ�����ýڣ�
        /// </summary>
        /// <param name="vName">���ý�����</param>
        /// <returns>��ӵ����ý�</returns>
        public INISegment Add(string vName)
        {
            if (Dictionary.Contains(vName))
                return (INISegment)Dictionary[vName];
            INISegment o = new INISegment(this, vName);
            Dictionary.Add(vName, o);
            return o;
        }
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public ICollection Keys
        {
            get { return Dictionary.Keys; }
        }
        /// <summary>
        /// ��ȡֵ����
        /// </summary>
        public ICollection Values
        {
            get { return Dictionary.Values; }
        }
        /// <summary>
        /// ��ȡ���ý�
        /// </summary>
        public INISegment this[string vName]
        {
            get
            {
                if (!Dictionary.Contains(vName))
                    return this.Add(vName);
                else
                    return (INISegment)Dictionary[vName];
            }
        }
        /// <summary>
        /// ��ȡ�Ƿ����ĳ���ý�
        /// </summary>
        /// <param name="vName">���ý�����</param>
        /// <returns>�Ƿ�</returns>
        public bool Contains(string vName)
        {
            return Dictionary.Contains(vName);
        }
    }
    
    /// <summary>
    /// ������
    /// </summary>
    public class INIItem
    {
        private string __Name;
        private string __Value;
        private INIItems __Owner;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="o">Owner</param>
        /// <param name="vName">����</param>
        /// <param name="vValue">ֵ</param>
        public INIItem(INIItems o, string vName, string vValue)
        {
            __Owner = o;
            __Name = vName;
            __Value = vValue;
            if (!o.Contains(vName))
                o.Owner.Owner.Owner.SetString(o.Owner.Name, vName, vValue);
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        public string Name
        {
            get { return __Name; }
        }
        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        public string Value
        {
            get { return __Value; }
            set
            {
                __Value = value;
                __Owner.Owner.Owner.Owner.SetString(__Owner.Owner.Name, __Name, value);
            }
        }
        /// <summary>
        /// ��ȡOwner
        /// </summary>
        public INIItems Owner
        {
            get { return __Owner; }
        }
    }
    
    /// <summary>
    /// �������
    /// </summary>
    public class INIItems : DictionaryBase
    {
        private INISegment __Owner;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="o">Owner</param>
        public INIItems(INISegment o)
        {
            __Owner = o;
        }
        /// <summary>
        /// ��ȡOwner
        /// </summary>
        public INISegment Owner
        {
            get { return __Owner; }
        }
        /// <summary>
        /// ���һ���Ѿ����ڵ�������
        /// </summary>
        /// <param name="o">������</param>
        public void Add(INIItem o)
        {
            if (!Dictionary.Contains(o.Name))
                Dictionary.Add(o.Name, o);
        }
        /// <summary>
        /// ��ȡ�Ƿ����ָ�����Ƶ�������
        /// </summary>
        /// <param name="vName">����������</param>
        /// <returns>�Ƿ�</returns>
        public bool Contains(string vName)
        {
            return Dictionary.Contains(vName);
        }
        /// <summary>
        /// ��ȡ���е���������
        /// </summary>
        public ICollection Keys
        {
            get { return Dictionary.Keys; }
        }
        /// <summary>
        /// ��ȡ���е�ֵ����
        /// </summary>
        public ICollection Values
        {
            get { return Dictionary.Values; }
        }
        /// <summary>
        /// ���һ�����ܲ����ڵ����������һ�������
        /// </summary>
        /// <param name="vName">��������</param>
        /// <param name="vValue">������ֵ</param>
        /// <returns>������������INIItem����</returns>
        public INIItem Add(string vName, string vValue)
        {
            if (Dictionary.Contains(vName))
                return (INIItem)Dictionary[vName];
            else
            {
                INIItem o = new INIItem(this, vName, vValue);
                this.Add(o);
                return o;
            }
        }
        /// <summary>
        /// ��ȡָ��������������
        /// </summary>
        public INIItem this[string vName]
        {
            get
            {
                if (Dictionary.Contains(vName))
                    return (INIItem)Dictionary[vName];
                else
                    return this.Add(vName, "");
            }
        }
    }
    
    /// <summary>
    /// INI�ļ�������
    /// </summary>
    public class INIFile
    {
        #region ����DLL����
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileIntA(string segName, string keyName, int iDefault, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileStringA(string segName, string keyName, string sDefault, StringBuilder retValue, int nSize, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileSectionA(string segName, byte[] sData, int nSize, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int WritePrivateProfileSectionA(string segName, byte[] sData, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int WritePrivateProfileStringA(string segName, string keyName, string sValue, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileSectionNamesA(byte[] vData, int iLen, string fileName);
        #endregion

        private string __Path;
        /// <summary>
        /// ���е����ý�
        /// </summary>
        public INISegments Segments;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="vPath">INI�ļ�·��</param>
        public INIFile(string vPath)
        {
            __Path = vPath;
            Segments = new INISegments(this);
            byte[] bufsegs = new byte[32767];
            int rel = GetPrivateProfileSectionNamesA(bufsegs, 32767, __Path);
            int iCnt, iPos;
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (bufsegs[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(bufsegs, iPos, iCnt).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            Segments.Add(tmp);
                    }
                }
            }
        }
        /// <summary>
        /// ��ȡINI�ļ�·��
        /// </summary>
        public string Path
        {
            get { return __Path; }
        }
        /// <summary>
        /// ��ȡһ�������͵�����ֵ
        /// </summary>
        /// <param name="segName">���ý���</param>
        /// <param name="keyName">��������</param>
        /// <param name="iDefault">Ĭ��ֵ</param>
        /// <returns>����ֵ</returns>
        public int GetInt(string segName, string keyName, int iDefault)
        {
            return GetPrivateProfileIntA(segName, keyName, iDefault, __Path);
        }

        /// <summary>
        /// ��ȡһ���ַ���������ֵ
        /// </summary>
        /// <param name="segName">���ý���</param>
        /// <param name="keyName">��������</param>
        /// <param name="sDefault">Ĭ��ֵ</param>
        /// <returns>����ֵ</returns>
        public string GetString(string segName, string keyName, string sDefault)
        {
            StringBuilder red = new StringBuilder(1024);
            GetPrivateProfileStringA(segName, keyName, "", red, 1024, __Path);
            return red.ToString();
        }

        /// <summary>
        /// д��������
        /// </summary>
        /// <param name="segName">���ý���</param>
        /// <param name="keyName">��������</param>
        /// <param name="vValue">����ֵ</param>
        public void SetString(string segName, string keyName, string vValue)
        {
            WritePrivateProfileStringA(segName, keyName, vValue, __Path);
        }
        /// <summary>
        /// д��һ�����ý�
        /// </summary>
        /// <param name="segName">���ý���</param>
        /// <param name="vData">����</param>
        /// <remarks>
        /// ����Ϊ�����������ɵ��ַ�����ÿ��������֮���� "\0" �ָ�
        /// �ַ�������� "\0\0" ����
        /// </remarks>
        /// <example>
        /// WriteSegment(segName,"\0\0"); �����������һ�����ý��µ�����������
        /// </example>
        public void WriteSegment(string segName, string vData)
        {
            WritePrivateProfileSectionA(segName, System.Text.ASCIIEncoding.Default.GetBytes(vData), __Path);
        }
        /// <summary>
        /// ��ȡһ�����ý����������������
        /// </summary>
        /// <param name="o">Ҫ��ȡ�����ý�</param>
        public void GetSegment(INISegment o)
        {
            byte[] vData = new byte[32767];
            int rLen = GetPrivateProfileSectionA(o.Name, vData, 32767, __Path);
            o.Items.Clear();
            if (rLen < 1) return;
            string tmp = "";
            int iPos, iCnt;
            iPos = 0;
            for (iCnt = 0; iCnt < rLen; iCnt++)
            {
                if (vData[iCnt] == 0x00)
                {
                    tmp = System.Text.ASCIIEncoding.Default.GetString(vData, iPos, iCnt - iPos).Trim();
                    if (tmp != "")
                    {
                        string[] t = tmp.Split('=');
                        if (t.Length <= 1)
                            o.Items.Add(t[0], "");
                        else
                            o.Items.Add(t[0], t[1]);
                    }
                    iPos = iCnt + 1;
                }
            }
        }
    }
    #endregion
}
