//������Դ�� http://blog.csdn.net/jh_zzz
//���ڴ˻����Ͻ����˴����޸�

namespace ExpertLib.IO
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Text;
    using System.Runtime.InteropServices;
    using ComTypes = System.Runtime.InteropServices.ComTypes;
    using System.Drawing;
    using System.Windows.Forms;
    
    #region StorageShareMode
    /// <summary>
    /// �洢�Ĺ���ģʽ
    /// </summary>
    public enum StorageShareMode : uint
    {
        /// <summary>
        /// �����ȡ����
        /// </summary>
        ShareDenyNone = 0x40,
        /// <summary>
        /// �ܾ�����Ķ�����
        /// </summary>
        ShareDenyRead = 0x30,
        /// <summary>
        /// �ܾ������д����
        /// </summary>
        ShareDenyWrite = 0x20,
        /// <summary>
        /// ��ռ�Ĵ�ȡģʽ
        /// </summary>
        ShareExclusive = 0x10,
    }
    #endregion

    #region StorageCreateMode
    /// <summary>
    /// �洢����ģʽ
    /// </summary>
    public enum StorageCreateMode : uint
    {
        /// <summary>
        /// ����Ѿ�����һ����/�洢��������ɾ�������û���Ѵ��ڵ���/�洢���ʹ���һ���µ�
        /// </summary>
        Create = 0x1000,
        /// <summary>
        /// ����ԭ�������ݣ��������ݱ�����CONTENTS�������У��Ҹ���λ�ڵ�ǰ�洢������
        /// </summary>
        Convert = 0x00020000,
        /// <summary>
        /// ����Ѿ�������һ������洢������ʧ��
        /// </summary>
        FailIfThere =0x00000000,
        /// <summary>
        /// �����������ʶ�ĸ����ĵ��е�����洢�ĸ��洢���ͷ�ʱ�����ᱻ�Զ��ͷ�
        /// </summary>
        DeleteOnRelease =0x04000000
    }
    #endregion

    #region StorageTransactedMode
    /// <summary>
    /// �洢����ģʽ
    /// </summary>
    public enum StorageTransactedMode :uint
    {
        /// <summary>
        /// ���жԸ����ĵ����޸�������Ч
        /// </summary>
        Direct =0x00400000,
        /// <summary>
        /// ֱ���ύ�������޸Ĳű�д�뵽�����ĵ������������ݿ�����е��ύ�ͻع�
        /// </summary>
        Transacted =0x00010000
    }
    #endregion

    #region StorageReadWriteMode
    /// <summary>
    /// �洢��д��ʽ
    /// </summary>
    public enum StorageReadWriteMode :uint
    {
        /// <summary>
        /// ֻ��ģʽ
        /// </summary>
        Read = 0x0,
        /// <summary>
        /// ֻдģʽ
        /// </summary>
        Write = 0x1,
        /// <summary>
        /// ��дģʽ
        /// </summary>
        ReadWrite = 0x2
    }
    #endregion

    #region StgcMode
    /// <summary>
    /// �ύ��ʽ
    /// </summary>
    public enum StgcMode : uint
    {
        /// <summary>
        /// Ĭ�ϵ�
        /// </summary>
        Default = 0,
        /// <summary>
        /// ����
        /// </summary>
        OverWrite = 1,
        /// <summary>
        /// ��ǰ�ύ
        /// </summary>
        OnlyIfCurrent = 2,
        /// <summary>
        /// �ύ��Cache��
        /// </summary>
        DangerouslyCommitmerelyToDiskCache = 4
    }
    #endregion

    #region StorageFile
    /// <summary>
    /// �ṹ���洢�ļ���
    /// </summary>
    public static class StorageFile 
    {
      
        #region IsStorageFile
        /// <summary>
        /// ����һ���ļ��Ƿ�ṹ���ļ�
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsStorageFile(string fileName)
        {
            ArgumentValidation.CheckForEmptyString(fileName, "fileName");
            
            //����ļ�������
            if(!File.Exists(fileName))
            {
                throw new Exception(SR.ExceptionFileNotExist(fileName));
            }

            return StorageNativeMethods.IsStorageFile(fileName);
        }
        #endregion

        #region CreateTempStorageFile
        /// <summary>
        /// ����һ����ʱ�Ľṹ���洢�ĵ�
        /// </summary>
        /// <returns></returns>
        public static Storage CreateTempStorageFile()
        {
            uint mode = (uint)StorageCreateMode.Create + (uint)StorageCreateMode.DeleteOnRelease
                + (uint)StorageReadWriteMode.ReadWrite + (uint)StorageShareMode.ShareExclusive;
            try
            {
                IStorage storage = StorageNativeMethods.StgCreateDocfile(null, mode, 0);
                return new Storage(storage);
            }
            catch (COMException)
            {
                return null;
            }
        }
        #endregion

        #region CreateStorageFile
        /// <summary>
        /// ���Ǵ���һ���ṹ���ĵ�����ԭ����û�д���,�����ظ��洢���ɶ�д�����������񡢾ܾ�����д��ʽ��
        /// </summary>
        /// <param name="storageFile"></param>
        /// <returns>���洢</returns>
        public static Storage CreateStorageFile(string storageFile)
        {
            
            return CreateStorageFile(storageFile, StorageCreateMode.Create, StorageReadWriteMode.ReadWrite,
                StorageShareMode.ShareExclusive, StorageTransactedMode.Direct);
        }

        public static Storage CreateStorageFile(string storageFile,
                          StorageCreateMode createMode,
                          StorageReadWriteMode readwriteMode,
                          StorageShareMode shareMode,
                          StorageTransactedMode transactedMode)
        {
            ArgumentValidation.CheckForEmptyString(storageFile, "storageFile");

            uint mode = (uint)createMode + (uint)readwriteMode + (uint)shareMode + (uint)transactedMode;
            try
            {
                IStorage storage = StorageNativeMethods.StgCreateDocfile(storageFile, mode, 0);
                return new Storage(storage);
            }
            catch (COMException)
            {
                return null;
            }
        }
        #endregion

        #region OpenStorageFile
        /// <summary>
        /// ��һ�������ĵ��������ظ�Ŀ¼��Storage����
        /// </summary>
        /// <param name="storageFile"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static Storage OpenStorageFile(string storageFile)
        {
            return OpenStorageFile(storageFile, StorageReadWriteMode.ReadWrite,
                StorageShareMode.ShareExclusive, StorageTransactedMode.Direct);
        }

        public static Storage ReadStorageFile(string storageFile)
        {
            return OpenStorageFile(storageFile, StorageReadWriteMode.Read,
                StorageShareMode.ShareExclusive, StorageTransactedMode.Direct);
        }

        public static Storage OpenStorageFile(string storageFile,
                          StorageReadWriteMode readwriteMode,
                          StorageShareMode shareMode,
                          StorageTransactedMode transactedMode)
        {
            if(!IsStorageFile(storageFile))
            {
                return null;
            }

            uint mode = (uint)readwriteMode + (uint)shareMode + (uint)transactedMode;
            try
            {
                IStorage storage = StorageNativeMethods.StgOpenStorage(storageFile, IntPtr.Zero, (uint)mode, IntPtr.Zero, 0);
                return new Storage(storage);
            }
            catch (COMException ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        #endregion

        #region CleanCopyStorageFile
        /// <summary>
        /// ����һ���洢�ļ�����һ���洢�ļ���������Ч�Ŀռ�ȥ��
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="dectFileName"></param>
        public static void CleanCopyStorageFile(string sourceFileName, string dectFileName)
        {
            ArgumentValidation.CheckForEmptyString(sourceFileName, "sourceFileName");
            ArgumentValidation.CheckForEmptyString(dectFileName, "dectFileName");

            Storage source = OpenStorageFile(sourceFileName);
            Storage dect = CreateStorageFile(dectFileName);
            source.CopyTo(dect);
            dect.Commit();
            source.Dispose();
            dect.Dispose();
        }

        public static void CleanCopyStorageFile(this Storage source, string dectFileName)
        {
            ArgumentValidation.CheckForEmptyString(source.Name, "sourceFileName");
            ArgumentValidation.CheckForEmptyString(dectFileName, "dectFileName");

            //Storage source = OpenStorageFile(sourceFileName);
            Storage dect = CreateStorageFile(dectFileName);
            source.CopyTo(dect);
            dect.Commit();
            source.Dispose();
            dect.Dispose();
        }
        #endregion
    }
    #endregion

    #region Storage
    /// <summary>
    /// �洢��
    /// </summary>
    public class Storage : System.IDisposable
    {
        #region Instance Field
        private bool disposed;
        private IStorage storage;
        #endregion

        #region Constructor
        internal Storage(IStorage storage)
        {
            this.storage = storage;
        }

        ~Storage()
        {
            Dispose();
        }

        public void Dispose()
        {
            try
            {
                if (!this.disposed)
                {
                    Marshal.ReleaseComObject(this.storage);
                    //this.storage.Release();
                    this.storage = null;                   
                    this.disposed = true;
                }

                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region CreateStorage
        /// <summary>
        /// ����һ���洢 ��ֱ��ģʽ��
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Storage CreateStorage(string name)
        {
            return CreateStorage(name, StorageCreateMode.Create, StorageReadWriteMode.ReadWrite, StorageTransactedMode.Direct);
        }

        public Storage CreateStorage(string name, StorageCreateMode createMode,
                 StorageReadWriteMode readwriteMode,
                 StorageTransactedMode transactedMode)
        {
            StorageHelper.ValidateStorageName(name);

            IStorage subStorage = null;

            try
            {
                //�ӵ�ڣ����ǽ���ʹ�ö�ռģʽ
                uint mode = (uint)readwriteMode + (uint)StorageShareMode.ShareExclusive
                         + (uint)transactedMode + (uint)createMode;
                this.storage.CreateStorage(name,mode,0, 0, out subStorage);
                this.storage.Commit(0);

                return new Storage(subStorage);
            }
            catch (COMException ex)
            {
                if (subStorage != null)
                    Marshal.ReleaseComObject(subStorage);
            }

            return null;
        }
        #endregion

        #region OpenStorage
        /// <summary>
        /// ��һ���Ӵ洢
        /// </summary>
        /// <param name="name">�洢����</param>
        /// <returns></returns>
        public Storage OpenStorage(string name)
        {
            return OpenStorage(name, StorageReadWriteMode.ReadWrite, StorageTransactedMode.Direct);
        }

        public Storage ReadStorage(string name)
        {
            return OpenStorage(name, StorageReadWriteMode.Read, StorageTransactedMode.Direct);
        }

        public Storage OpenStorage(string name,
                 StorageReadWriteMode readwriteMode,
                 StorageTransactedMode transactedMode)
        {
            StorageHelper.ValidateStorageName(name);

            IStorage subStorage = null;

            try
            {
                //�ӵ�ڣ����ǽ���ʹ�ö�ռģʽ
                uint mode = (uint)readwriteMode + (uint)StorageShareMode.ShareExclusive
                         + (uint)transactedMode;
                this.storage.OpenStorage(name, null, mode, IntPtr.Zero, 0, out subStorage);
                this.storage.Commit(0);

                return new Storage(subStorage);
            }
            catch (COMException)
            {
                if (subStorage != null)
                    Marshal.ReleaseComObject(subStorage);
            }

            return null;
        }
        #endregion

        #region CopyTo
        /// <summary>
        /// ������洢���Ƶ���һ���洢������ʵ�ֿռ�����
        /// </summary>
        /// <param name="destinationStorage"></param>
        public void CopyTo(Storage destinationStorage)
        {
            this.storage.CopyTo(0, IntPtr.Zero, IntPtr.Zero, destinationStorage.storage);
        }
        #endregion

        #region RecurOpenStorage
        /// <summary>
        /// ��·���ķ�ʽ��λһ���洢
        /// </summary>
        /// <param name="name">������·�����ַ�����ABC\DEF\JJJ</param>
        /// <returns></returns>
        public Storage RecurOpenStorage(string name)
        {
            string pwcsName;

            int pos = name.IndexOf('\\');
            if (pos > 0)
            {
                pwcsName = name.Substring(0, pos);
                name = name.Substring(pos + 1);
            }
            else
            {
                pwcsName = name;
                name = "";
            }

            Storage subStorage = OpenStorage(pwcsName);
            if (subStorage != null && name.Length > 0)
            {
                return subStorage.RecurOpenStorage(name);
            }

            return subStorage;
        }
        #endregion

        #region CreateStream
        /// <summary>
        /// ����һ����
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StorageStream CreateStream(string name)
        {
            return CreateStream(name, StorageCreateMode.Create, StorageReadWriteMode.ReadWrite, StorageTransactedMode.Direct);
        }

        public StorageStream CreateStream(string name, StorageCreateMode createMode,
                 StorageReadWriteMode readwriteMode,
                 StorageTransactedMode transactedMode)
        {
            StorageHelper.ValidateStorageName(name);

            IStream subStream = null;

            try
            {
                //���ǽ���ʹ�ö�ռģʽ
                uint mode = (uint)readwriteMode + (uint)StorageShareMode.ShareExclusive
                         + (uint)transactedMode + (uint)createMode;
                this.storage.CreateStream(name,mode,0, 0, out subStream);
                this.storage.Commit(0);

                return new StorageStream(subStream);
            }
            catch (COMException)
            {
                if (subStream != null)
                    Marshal.ReleaseComObject(subStream);

                return null;
            }
        }
        #endregion

        #region OpenStream
        /// <summary>
        /// ��һ����
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StorageStream OpenStream(string name)
        {
            return OpenStream(name, StorageReadWriteMode.ReadWrite, StorageTransactedMode.Direct);
        }

        public StorageStream ReadStream(string name)
        {
            return OpenStream(name, StorageReadWriteMode.Read, StorageTransactedMode.Direct);
        }

        public StorageStream OpenStream(string name, 
                 StorageReadWriteMode readwriteMode,
                 StorageTransactedMode transactedMode)
        {
            StorageHelper.ValidateStorageName(name);

            IStream subStream = null;

            try
            {
                //���ǽ���ʹ�ö�ռģʽ
                uint mode = (uint)readwriteMode + (uint)StorageShareMode.ShareExclusive
                         + (uint)transactedMode;
                this.storage.OpenStream(name, IntPtr.Zero,mode,0, out subStream);
                return new StorageStream(subStream);
            }
            catch (COMException)
            {
                if (subStream != null)
                    Marshal.ReleaseComObject(subStream);

                return null;
            }
        }
        #endregion

        #region Commit
        /// <summary>
        /// �ύ,�����Ӷ���
        /// </summary>
        public void Commit()
        {
           this.storage.Commit((uint)StgcMode.Default);
        }
        #endregion

        #region Revert
        /// <summary>
        /// ȡ���Դ��������ϴ��ύ�����������޸�
        /// </summary>
        /// <remarks>
        /// ��������ģʽ����Ч��ע��˺������ú����е��Ӵ洢������������Ч��
        /// </remarks>
        public void Revert()
        {
            this.storage.Revert();
        }
        #endregion

        #region ȡ��Ԫ����Ϣ
        
        public List<StgElementInfo> GetChildElementsInfo()
        {
            IEnumSTATSTG statstg;
            ComTypes.STATSTG stat;
            uint k;
            List<StgElementInfo> list = new List<StgElementInfo>();
            storage.EnumElements(0, IntPtr.Zero, 0,out statstg);//�˴�û���ͷ�
            statstg.Reset();
            while (statstg.Next(1, out stat, out k) == HRESULT.S_OK)
            {
                list.Add(new StgElementInfo(stat));
            }
            Marshal.ReleaseComObject(statstg);//�ͷ�
            statstg = null;
            return list;
        }
        #endregion

        #region MoveElement
        /// <summary>
        /// �����洢������ָ���Ĵ洢���������ƶ���ָ��������洢��
        /// </summary>
        /// <param name="elementName">�洢������</param>
        /// <param name="destStorage">Ŀ��洢</param>
        public void MoveElement(string elementName, Storage destStorage)
        {
            ArgumentValidation.CheckForEmptyString(elementName,"elementName");
            if (IsElementExist(elementName))
            {
                try
                {
                    this.storage.MoveElementTo(elementName, destStorage.storage, elementName, StorageConst.STGMOVE_MOVE);
                }
                catch (COMException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception(SR.ExceptionElementNotExist);
            }

        }
        #endregion

        #region CopyElement
        /// <summary>
        /// �����洢������ָ���Ĵ洢���������Ƶ�ָ��������洢��
        /// </summary>
        /// <param name="elementName">�洢������</param>
        /// <param name="destStorage">Ŀ��洢</param>
        public void CopyElement(string elementName, Storage destStorage)
        {
            ArgumentValidation.CheckForEmptyString(elementName, "elementName");
            if (IsElementExist(elementName))
            {
                try
                {
                    this.storage.MoveElementTo(elementName, destStorage.storage, elementName, StorageConst.STGMOVE_COPY);
                }
                catch (COMException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception(SR.ExceptionElementNotExist);
            }

        }
        #endregion

        #region DeleteElement
        /// <summary>
        /// ɾ��ָ������Ԫ��
        /// </summary>
        /// <param name="elementName"></param>
        public void DeleteElement(string elementName)
        {
            if (!IsElementExist(elementName))
                throw new Exception(SR.ExceptionElementNotExist);

            this.storage.DestroyElement(elementName);
        }
        #endregion

        #region RenameElement
        /// <summary>
        /// ��������Ԫ��
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="newName"></param>
        public void RenameElement(string elementName, string newName)
        {
            StorageHelper.ValidateStorageName(newName);

            if (!IsElementExist(elementName))
                throw new Exception(SR.ExceptionElementNotExist);

            if (IsElementExist(newName))
                throw new Exception(SR.ExceptionElementNameExist);
            

            this.storage.RenameElement(elementName, newName);
        }
        #endregion

        #region IsElementExist
        /// <summary>
        /// ����Ƿ����ָ�����Ƶ��Ӷ���
        /// </summary>
        /// <param name="elementName">��������</param>
        /// <returns></returns>
        /// <remarks>���Դ�Сд�Ƚ�����</remarks>
        public bool IsElementExist(string elementName)
        {
            ArgumentValidation.CheckForEmptyString(elementName, "elementName");

            IEnumSTATSTG statstg;
            ComTypes.STATSTG stat;
            uint k;
            this.storage.EnumElements(0, IntPtr.Zero, 0, out statstg);
            statstg.Reset();
            while (statstg.Next(1, out stat, out k) == HRESULT.S_OK)
            {
                //���Դ�Сд�Ƚ�
                if (string.Compare(stat.pwcsName, elementName, true) == 0) return true;
            }
            return false;
        }
        #endregion

        #region CLSID
        /// <summary>
        /// ��ȡ�����ô洢��Ӧ��CLSIDֵ
        /// </summary>
        public Guid CLSID
        {
            get
            {
                ComTypes.STATSTG statstg;
                this.storage.Stat(out statstg, StorageConst.STATFLAG_NONAME);
                return statstg.clsid;
            }
            set
            {
                this.storage.SetClass(value);
            }
        }
        #endregion

        #region Name
        /// <summary>
        /// ��ȡ�洢������
        /// </summary>
        public string Name
        {
            get
            {
                ComTypes.STATSTG statstg;
                this.storage.Stat(out statstg, StorageConst.STATFLAG_DEFAULT);
                return statstg.pwcsName;

            }
        }
        #endregion

        #region StateBits
        /// <summary>
        /// ��д״̬��
        /// </summary>
        public uint StateBits
        {
            get
            {
                ComTypes.STATSTG statstg;
                this.storage.Stat(out statstg, StorageConst.STATFLAG_NONAME);
                
                return ValueConvert.ToUInt32(statstg.grfStateBits);
            }
            set
            {
                this.storage.SetStateBits(value, 0x0);
            }
        }
        #endregion

        #region StorageInfo
        /// <summary>
        /// ��ȡ�洢����
        /// </summary>
        public StgElementInfo StorageInfo
        {
            get
            {
                ComTypes.STATSTG statstg;
                this.storage.Stat(out statstg, StorageConst.STATFLAG_DEFAULT);
                return new StgElementInfo(statstg);
            }
        }
        #endregion
    }
    #endregion
}
