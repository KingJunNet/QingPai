using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FileView : UserControl
    {
        public const string RootFolder = "轻排参数表服务器";

        public string Folder => $"\\\\{Server}{RootFolder}\\{Category}\\{Id}";

        public bool Authority
        {
            set
            {
                if(DesignMode)
                    return;
                上传ToolStripMenuItem.Enabled = value;
                删除ToolStripMenuItem.Enabled = value;
            }
        }

        public readonly string Server;

        private string _category;

        [Description("文件子目录"), Category("自定义属性")]
        public string Category
        {
            get => _category;
            set
            {
                if (DesignMode)
                    return;
                _category = value;
                label1.Text = $"文件夹地址:{Folder}";
            }
        }

        private int _id;

        [Description("ID"), Category("自定义属性")]
        public int Id
        {
            get => _id;
            set
            {
                if (DesignMode)
                    return;

                _id = value;
                label1.Text = $"文件夹地址:{Folder}";
            }
        }

        public FileView()
        {
            InitializeComponent();
            if (DesignMode)
                return;

            var sql = new DataControl();
            Server = sql.ServerIP;
            if (!Server.EndsWith("\\"))
                Server += "\\";
        }
        
        private void FileView_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            label1.Text = $"文件夹地址:{Folder}";
            LoadView();
        }

        public void LoadView()
        {
            if (Id < 0)
                return;

            try
            {
                var dir = new DirectoryInfo(Folder);
                if (!dir.Exists) return;
                var files = dir.GetFiles();
                listView1.Items.Clear();
                foreach (var file in files)
                {
                    listView1.Items.Add(Path.GetFileName(file.Name));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("请联系管理员设置服务器共享文件夹属性", "提示");
                Log.e($"FileView {e}");
            }
        }

        private void 上传ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Id < 0)
            {
                MessageBox.Show("请先保存再上传文件");
                return;
            }

            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "选择文件",
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Form1.ShowWaitForm();
                var dir = new DirectoryInfo(Folder);
                if (!dir.Exists)
                    dir.Create();

                foreach (var filePath in fileDialog.FileNames)
                {
                    var name = Path.GetFileName(filePath);
                    var target = $"{Folder}\\{name}";

                    if (File.Exists(target))
                    {
                        if (MessageBox.Show($"{name}已存在是否覆盖?", "提示", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) != DialogResult.OK)
                            continue;
                    }

                    File.Copy(filePath, target, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"请联系管理员设置服务器共享文件夹属性", "提示");
                Log.e($"上传文件 {ex}");
            }
            finally
            {
                Form1.CloseWaitForm();
            }
            
            LoadView();
        }

        private void 下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Id < 0)
            {
                MessageBox.Show("请先保存再下载文件");
                return;
            }

            var items = listView1.SelectedItems;
            if (items.Count == 0)
            {
                MessageBox.Show("请选择文件", "提示");
                return;
            }

            foreach (ListViewItem item in items)
            {
                var name = item.SubItems[0].Text;
                var sourcePath = $"{Folder}\\{name}";
                var fileDialog = new SaveFileDialog
                {
                    Title = $"备份 {name}",
                    FileName = name,
                    RestoreDirectory = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                };
                if (fileDialog.ShowDialog() != DialogResult.OK) continue;

                var targetPath = fileDialog.FileName;
                File.Copy(sourcePath, targetPath, true);
            }

            LoadView();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Id < 0)
            {
                MessageBox.Show("请先保存再删除文件");
                return;
            }

            var items = listView1.SelectedItems;
            if (items.Count == 0)
            {
                MessageBox.Show("请选择文件", "提示");
                return;
            }

            if (MessageBox.Show("删除后无法恢复,是否继续", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            try
            {
                foreach (ListViewItem item in items)
                {
                    var name = item.SubItems[0].Text;
                    var sourcePath = $"{Folder}\\{name}";

                    File.Delete(sourcePath);
                }
            }
            catch (Exception e1)
            {
                Log.e($"删除ToolStripMenuItem_Click {e1}");
            }
            LoadView();
        }

        private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Folder))
                System.Diagnostics.Process.Start("Explorer.exe", Folder);
            else
                MessageBox.Show($"{Folder}文件夹不存在", "提醒");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
