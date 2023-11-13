using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExpertLib.IO
{
    #region EventArgs
    #region ScanEventArgs
    /// <summary>
	/// ɨ���¼�����.
	/// </summary>
	public class ScanEventArgs : EventArgs
	{
        #region Instance Fields
        string name_;
        bool continueRunning_ = true;
        #endregion

		/// <summary>
		/// ����һ��ɨ�����
		/// </summary>
		/// <param name="name"></param>
		public ScanEventArgs(string name)
		{
			this.name_ = name;
            this.continueRunning_ = true;
		}
		
		
		/// <summary>
		/// ����ɨ����ļ�.
		/// </summary>
		public string FileName
		{
			get { return name_; }
            internal set
            {
                this.name_ = value;
            }
		}
		
		
		/// <summary>
		/// ȷ���Ƿ����ɨ��,Ĭ��Ϊtrue.
		/// </summary>
		public bool ContinueRunning
		{
			get { return continueRunning_; }
			set { continueRunning_ = value; }
		}
    }
    #endregion

    #region ScanFailureEventArgs
    /// <summary>
	/// ɨ��ʧ�ܲ���.
	/// </summary>
	public class ScanFailureEventArgs
	{
		/// <summary>
		/// Initialise a new instance of <see cref="ScanFailureEventArgs"></see>
		/// </summary>
		/// <param name="name">The name to apply.</param>
		/// <param name="e">The exception to use.</param>
		public ScanFailureEventArgs(string name, Exception e)
		{
			name_ = name;
			exception_ = e;
		}
		
		
		/// <summary>
		/// ����������ļ���.
		/// </summary>
		public string FileName
		{
			get { return name_; }
		}
		
		/// <summary>
		/// �����ԭ��.
		/// </summary>
		public Exception Exception
		{
			get { return exception_; }
		}
		
		#region Instance Fields
		string name_;
		Exception exception_;
		#endregion
    }
    #endregion

    #endregion

    #region Delegates
    /// <summary>
	/// Delegate invoked when a directory is processed.
	/// </summary>
	public delegate void ScanDirectoryDelegate(object sender, ScanEventArgs e);
	
	/// <summary>
	/// Delegate invoked when a file is processed.
	/// </summary>
	public delegate void ScanFileDelegate(object sender, ScanEventArgs e);
	
	/// <summary>
	/// Delegate invoked when a directory failure is detected.
	/// </summary>
	public delegate void ScanFailureDelegate(object sender, ScanFailureEventArgs e);
	#endregion

	/// <summary>
	/// ͨ���ļ���Ŀ¼ɨ����
	/// </summary>
    public sealed class FileSystemScanner
    {
        #region Instance Fields
        IScanFilter fileFilter_;
        IScanFilter directoryFilter_;
        long matchFiles;
        long matchDirectorys;
        ScanEventArgs scanArgs;
        bool alive; //�Ƿ����
        #endregion

        #region Constructors
        /// <summary>
        /// ����һ�����κι���������ɨ
        /// </summary>
        public FileSystemScanner()
        {
            this.fileFilter_ = null;
            this.directoryFilter_ = null;
            this.matchFiles = 0;
            this.matchDirectorys = 0;
        }

        /// <summary>
        /// ����ɨ����
        /// </summary>
        /// <param name="filter">�ļ����ƹ��������ʽ</param>
        public FileSystemScanner(string filter)
        {
            if ((filter == null) || (filter.Trim().Length == 0))
            {
                fileFilter_ = null;
            }
            else
            {
                fileFilter_ = new FileFilter(filter);
            }
            this.directoryFilter_ = null;
            this.matchFiles = 0;
            this.matchDirectorys = 0;
        }

        /// <summary>
        /// Initialise a new instance of <see cref="FileSystemScanner"></see>
        /// </summary>
        /// <param name="fileFilter">The <see cref="NameFilter">file filter</see> to apply.</param>
        /// <param name="directoryFilter">The <see cref="NameFilter">directory filter</see> to apply.</param>
        public FileSystemScanner(string fileFilter, string directoryFilter)
        {
            if ((fileFilter == null) || (fileFilter.Trim().Length == 0))
            {
                fileFilter_ = null;
            }
            else
            {
                fileFilter_ = new FileFilter(fileFilter);
            }

            if ((directoryFilter == null) || (directoryFilter.Trim().Length == 0))
            {
                directoryFilter_ = null;
            }
            else
            {
                directoryFilter_ = new FileFilter(directoryFilter);
        
            }
            this.matchFiles = 0;
            this.matchDirectorys = 0;
        }

        /// <summary>
        /// Initialise a new instance of <see cref="FileSystemScanner"></see>
        /// </summary>
        /// <param name="fileFilter">The file <see cref="IScanFilter"></see>filter to apply.</param>
        public FileSystemScanner(IScanFilter fileFilter)
        {
            fileFilter_ = fileFilter;
            directoryFilter_ = null;
            this.matchFiles = 0;
            this.matchDirectorys = 0;
        }

        /// <summary>
        /// Initialise a new instance of <see cref="FileSystemScanner"></see>
        /// </summary>
        /// <param name="fileFilter">The file <see cref="IScanFilter"></see>filter to apply.</param>
        /// <param name="directoryFilter">The directory <see cref="IScanFilter"></see>filter to apply.</param>
        public FileSystemScanner(IScanFilter fileFilter, IScanFilter directoryFilter)
        {
            fileFilter_ = fileFilter;
            directoryFilter_ = directoryFilter;
            this.matchFiles = 0;
            this.matchDirectorys = 0;
        }
        #endregion

        #region Event
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public event ScanDirectoryDelegate ProcessDirectoryEvent;

        /// <summary>
        /// �����ļ�.
        /// </summary>
        public event ScanFileDelegate ProcessFileEvent;

        /// <summary>
        /// û�ж�ӦĿ¼ʱ����.
        /// </summary>
        public event ScanFailureDelegate DirectoryFailureEvent;

        #endregion

        #region Scan
        /// <summary>
        /// ����һ��������
        /// </summary>
        /// <param name="driveinfo"></param>
        /// <remarks>����һ������²�Ҫ���еݹ��ѯ</remarks>
        /// <exception cref="Exception"></exception>
        public void Scan(DriveInfo driveinfo,bool recurse)
        {
            if (!driveinfo.IsReady)
            {
                throw new Exception(SR.ExceptionDriveIsnReady(driveinfo.Name));
            }
            string root = driveinfo.Name;
            this.alive = true;
            scanArgs = new ScanEventArgs(root);
            ScanDir(root, recurse);
        }
        #endregion

        #region Scan
        /// <summary>
        /// ɨ��һ��Ŀ¼�µ��ļ���Ŀ¼.
        /// </summary>
        /// <param name="directory">Ҫɨ���Ŀ¼</param>
        /// <param name="recurse">�Ƿ�ݹ�ɨ����Ŀ¼</param>
        /// <remarks>������Ŀ¼����
        /// </remarks>
        /// <exception cref="ArgumentException"></exception>
        public void Scan(string directory, bool recurse)
        {
            this.alive = true;
            scanArgs = new ScanEventArgs(directory);
            ScanDir(directory, recurse);
        }
        #endregion
      
        #region MatchFiles
        /// <summary>
        /// ��ȡƥ����ļ���
        /// </summary>
        public long MatchFiles
        {
            get
            {
                return this.matchFiles;
            }
        }
        #endregion

        #region MatchDirectorys
        /// <summary>
        /// ��ȡƥ���Ŀ¼��
        /// </summary>
        public long MatchDirectorys
        {
            get
            {
                return this.matchDirectorys;
            }
        }
        #endregion

        #region FileIsMatch
        /// <summary>
        /// ����ļ��Ƿ�ƥ��
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool FileIsMatch(string filename)
        {
            if (fileFilter_ == null)
                return true;
            else
            {
                return fileFilter_.IsMatch(filename);
            }
        }
         #endregion

        #region DirectoryIsMatch
        /// <summary>
        /// ���Ŀ¼�Ƿ�ƥ��
        /// </summary>
        /// <param name="directoryname"></param>
        /// <returns></returns>
        public bool DirectoryIsMatch(string directoryname)
        {
            if (directoryFilter_ == null)
                return true;
            else
                return directoryFilter_.IsMatch(directoryname);
        }
        #endregion

        #region Private function
        
        #region OnScanFailure
        /// <summary>
        /// ����DirectoryFailure�¼�.
        /// </summary>
        private void OnScanFailure(ScanFailureEventArgs e)
        {
            this.alive = false;

            if (DirectoryFailureEvent != null)
            {
                DirectoryFailureEvent(this, e);
            }
            else
            {
                  //�����������ֱ���˳�,���׳��쳣
                throw e.Exception;
            }
        }
        #endregion

        #region OnProcessFile
        /// <summary>
        /// ���� ProcessFile �¼�.
        /// </summary>
        private void OnProcessFile(ScanEventArgs e)
        {
            if (ProcessFileEvent != null)
            {
                ProcessFileEvent(this, e);
                if (e.ContinueRunning == false)
                {
                    this.alive = false;
                }
            }
        }
        #endregion

        #region OnProcessDirectory
        /// <summary>
        /// ���� ProcessDirectory �¼�.
        /// </summary>
        private void OnProcessDirectory(ScanEventArgs e)
        {
            if (ProcessDirectoryEvent != null)
            {
                ProcessDirectoryEvent(this, e);
                if (e.ContinueRunning == false)
                {
                    this.alive = false;
                }
            }
        }
        #endregion

        #region ScanDir
        /// <summary>
        /// ɨ��Ŀ¼���ĺ���
        /// </summary>
        /// <param name="sourceDirectory">ɨ��ԴĿ¼����</param>
        /// <param name="recurse">�Ƿ�ݹ�</param>
        void ScanDir(string sourceDirectory, bool recurse)
        {
            if (this.alive == false) return;

            string[] filenames;
            string[] directorynames;
            try
            {
                filenames = System.IO.Directory.GetFiles(sourceDirectory);
                directorynames = System.IO.Directory.GetDirectories(sourceDirectory);
            }
            catch(Exception e)
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(sourceDirectory, e);
                OnScanFailure(args);
                return;
            }

            //����Ŀ¼�µ��ļ�
            foreach (string fileName in filenames)
            {
                if (FileIsMatch(fileName))
                {
                    this.matchFiles ++;  //�����ļ���
                    scanArgs.FileName = fileName;
                    OnProcessFile(scanArgs);
                    if (this.alive == false) return;
                }
            }
       

            #region ������Ŀ¼
            foreach (string directory in directorynames)
            {
                if (DirectoryIsMatch(directory))
                {
                    this.matchDirectorys++; //����ƥ���Ŀ¼��
                    this.scanArgs.FileName = directory;
                    OnProcessDirectory(scanArgs);
                    if (this.alive == false) return;

                    if (recurse) //������Ŀ¼
                    {
                        ScanDir(directory, true);
                    }
                }
            }
            
            #endregion
        }
        #endregion

       
        #endregion
    }
}
