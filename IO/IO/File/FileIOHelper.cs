//��������ռ�������ĳƪ�����ļ����������£����ִ����Ȩ��ԭ�������С�

namespace ExpertLib.IO
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// �ļ�����������
    /// </summary>
    /// <remarks>
    /// �������������������Ϊ�˼�һЩ���룬ʵ���Ͽ��ǵ��������������ʱ�����ܻ��������������
    /// �ض���File Directory FileInfo DirectoryInfo Path Drive DriveInfo���������ܸ���
    /// </remarks>
    public static class FileIOHelper
    {
        #region FileIsExist
        /// <summary>
        /// �ļ��Ƿ���ڻ���Ȩ����
        /// </summary>
        /// <param name="path">���·�������·��</param>
        /// <returns>�����Ŀ¼Ҳ����false</returns>
        public static bool FileIsExist(string path)
        {
            return File.Exists(path);
        }
        #endregion

        #region DirectoryIsExist
        /// <summary>
        /// Ŀ¼�Ƿ���ڻ���Ȩ����
        /// </summary>
        /// <param name="Path">���·�������·��</param>
        /// <returns></returns>
        public static bool DirectoryIsExist(string path)
        {
            return Directory.Exists(path);
        }
        #endregion

        #region FileIsReadOnly
        /// <summary>
        /// �ļ��Ƿ�ֻ��
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static bool FileIsReadOnly(string fullpath)
        {
            FileInfo file = new FileInfo(fullpath);
            if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region SetFileReadonly
        /// <summary>
        /// �����ļ��Ƿ�ֻ��
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="flag">true��ʾֻ������֮</param>
        public static void SetFileReadonly(string fullpath,bool flag)
        {
            FileInfo file = new FileInfo(fullpath);

            if (flag)
            {
                // ���ֻ������
                file.Attributes |= FileAttributes.ReadOnly;
            }
            else
            {
                // �Ƴ�ֻ������
                file.Attributes &= ~FileAttributes.ReadOnly;
            }
        }
        #endregion

        #region GetFileSize
        /// <summary>
        /// ȡ�ļ�����
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static long GetFileSize(string fullpath)
        {
            FileInfo fi = new FileInfo(fullpath);
            return fi.Length;
        }
        #endregion

        #region GetFileCreateTime
        /// <summary>
        /// ȡ�ļ�����ʱ��
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static DateTime GetFileCreateTime(string fullpath)
        {
            FileInfo fi = new FileInfo(fullpath);
            return fi.CreationTime;
        }
        #endregion

        #region GetLastWriteTime
        /// <summary>
        /// ȡ�ļ����洢ʱ��
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static DateTime GetLastWriteTime(string fullpath)
        {
            FileInfo fi = new FileInfo(fullpath);
            return fi.LastWriteTime;
        }
        #endregion

        #region IsPathRooted
        /// <summary>
        /// ָʾһ��·�������·�����Ǿ���·��
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPathRooted(string path)
        {
            return Path.IsPathRooted(path);
        }
        #endregion

        #region GetSystemDirectory
        /// <summary>
        /// ȡϵͳĿ¼
        /// </summary>
        /// <returns></returns>
        public static string GetSystemDirectory()
        {
            return System.Environment.SystemDirectory;
        }
        #endregion

        #region GetSpeicalFolder
        /// <summary>
        /// ȡϵͳ���ر�Ŀ¼
        /// </summary>
        /// <param name="folderType"></param>
        /// <returns></returns>
        public static string GetSpeicalFolder(Environment.SpecialFolder folderType)
        {
            return System.Environment.GetFolderPath(folderType);
        }
        #endregion

        #region GetTempPath
        /// <summary>
        /// ���ص�ǰϵͳ����ʱĿ¼
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }
        #endregion

        #region GetInvalidPathChars
        /// <summary>
        /// ȡ·���в�������ڵ��ַ�
        /// </summary>
        /// <returns></returns>
        public static char[] GetInvalidPathChars()
        {
            return Path.GetInvalidPathChars();
        }
        #endregion

        #region GetCurrentDirectory
        /// <summary>
        /// ȡ��ǰĿ¼
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
        #endregion

        #region SetCurrentDirectory
        /// <summary>
        /// �赱ǰĿ¼
        /// </summary>
        /// <param name="path"></param>
        public static void SetCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }
        #endregion

        #region CreateTempZeroByteFile
        /// <summary>
        /// ����һ�����ֽ���ʱ�ļ�
        /// </summary>
        /// <returns></returns>
        public static string CreateTempZeroByteFile()
        {
            return Path.GetTempFileName();
        }
        #endregion

        #region GetRandomFileName
        /// <summary>
        /// ����һ������ļ������������ļ�����
        /// </summary>
        /// <returns></returns>
        public static string GetRandomFileName()
        {
            return Path.GetRandomFileName();
        }
        #endregion

        #region CompareFilesHash
        /// <summary>
        /// �ж������ļ��Ĺ�ϣֵ�Ƿ�һ��
        /// </summary>
        /// <param name="fileName1"></param>
        /// <param name="fileName2"></param>
        /// <returns></returns>
        public static bool CompareFilesHash(string fileName1, string fileName2)
        {
            using (HashAlgorithm hashAlg = HashAlgorithm.Create())
            {
                using (FileStream fs1 = new FileStream(fileName1, FileMode.Open), fs2 = new FileStream(fileName2, FileMode.Open))
                {
                    byte[] hashBytes1 = hashAlg.ComputeHash(fs1);
                    byte[] hashBytes2 = hashAlg.ComputeHash(fs2);

                    // �ȽϹ�ϣ��
                    return (BitConverter.ToString(hashBytes1) == BitConverter.ToString(hashBytes2));
                }
            }
        }
        #endregion

        #region CalcuDirectorySize
        /// <summary>
        /// ����һ��Ŀ¼�Ĵ�С
        /// </summary>
        /// <param name="di">ָ��Ŀ¼</param>
        /// <param name="includeSubDir">�Ƿ������Ŀ¼</param>
        /// <returns></returns>
        public static long CalcuDirectorySize(DirectoryInfo di, bool includeSubDir)
        {
            long totalSize = 0;
            
            // ������У�ֱ�ӣ��������ļ�
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }

            // ���������Ŀ¼�����includeSubDir����Ϊtrue
            if (includeSubDir)
            {
                DirectoryInfo[] dirs = di.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    totalSize += CalcuDirectorySize(dir, includeSubDir);
                }
            }

            return totalSize;
        }
        #endregion

        #region CopyDirectory
        /// <summary>
        /// ����Ŀ¼��Ŀ��Ŀ¼
        /// </summary>
        /// <param name="source">ԴĿ¼</param>
        /// <param name="destination">Ŀ��Ŀ¼</param>
        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            // �������Ŀ¼��ͬ�������븴��
            if (destination.FullName.Equals(source.FullName))
            {
                return;
            }

            // ���Ŀ��Ŀ¼�����ڣ�������
            if (!destination.Exists)
            {
                destination.Create();
            }

            // ���������ļ�
            FileInfo[] files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                // ���ļ����Ƶ�Ŀ��Ŀ¼
                file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
            }

            // ������Ŀ¼
            DirectoryInfo[] dirs = source.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                string destinationDir = Path.Combine(destination.FullName, dir.Name);

                // �ݹ鴦����Ŀ¼
                CopyDirectory(dir, new DirectoryInfo(destinationDir));
            }
        }
        #endregion

        #region GetAllDrives
        /// <summary>
        /// ȡϵͳ���е��߼�������
        /// </summary>
        /// <returns></returns>
        public static DriveInfo[] GetAllDrives()
        {
            return DriveInfo.GetDrives();
        }
        #endregion

        #region IsDrivePath
        /// <summary>
        /// ���һ��Ŀ¼�ǲ��Ǹ�Ŀ¼��־
        /// </summary>
        /// <param name="path">·������</param>
        /// <returns></returns>
        public static bool IsDrivePath(string path)
        {
            if (Path.IsPathRooted(path)) //���Ϊ���·�����϶���������Ŀ¼
                return false;

            if (Path.GetPathRoot(path) != path)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        #endregion
    }
}
