using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManager.application.viewmodel;
using TaskManager.common.utils;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.domain.valueobject;
using TaskManager.infrastructure.db;

namespace TaskManager.controller
{
    public sealed class UIHelp
    {
        private static readonly UIHelp instance = new UIHelp();
        /// <summary>
        /// 显式的静态构造函数⽤来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static UIHelp() { }
        private UIHelp()
        {
            AbortEquipmentUasageRecordExportWork = false;
        }
        public static UIHelp Instance { get { return instance; } }

        public bool AbortEquipmentUasageRecordExportWork { get; set; }

        public void notifyEquipmentListViewChanged(ListView listViewUsingEquipment,
                                                    Button btn,
                                                    List<EquipmentLite> itemEquipments,
                                                    string testGroup)
        {
            if (Collections.isEmpty(itemEquipments))
            {
                return;
            }
            this.setListViewUsingEquipmentRowHeight(listViewUsingEquipment);


            //填充数据
            ListViewItem[] lvs = new ListViewItem[itemEquipments.Count];
            for (int index = 0; index < itemEquipments.Count; index++)
            {
                EquipmentLite curEquipment = itemEquipments[index];
                lvs[index] = new ListViewItem(new string[] { (index + 1).ToString(), curEquipment.Code, curEquipment.Name, curEquipment.Type, curEquipment.Group, "" });
                if (!curEquipment.Group.Equals(testGroup))
                {
                    this.setOtherGroupEquipmentStyle(lvs[index]);
                }
            }

            //更新ui
            listViewUsingEquipment.Items.Clear();
            listViewUsingEquipment.Items.AddRange(lvs);

            //设置删除事件
            this.setEquipmentListViewItemRemoveButton(listViewUsingEquipment, btn);
        }

        public void addEquipment(ListView listViewUsingEquipment,
                                 Button btn,
                                 List<EquipmentLite> itemEquipments,
                                 Dictionary<string, EquipmentLite> itemEquipmentMap,
                                 Dictionary<string, EquipmentBreiefViewModel> equipmentMap,
                                 string newEquipmentValue, string testGroup)
        {
            if (!equipmentMap.ContainsKey(newEquipmentValue))
            {
                return;
            }
            string newEquipmentCode = equipmentMap[newEquipmentValue].Code;
            if (itemEquipmentMap.ContainsKey(newEquipmentCode))
            {
                return;
            }
            EquipmentBreiefViewModel newEquipment = equipmentMap[newEquipmentValue];
            EquipmentLite addedEquipmentLite = newEquipment.toEquipmentLite();
            itemEquipments.Add(addedEquipmentLite);
            itemEquipmentMap.Add(addedEquipmentLite.Code, addedEquipmentLite);

            //填充数据
            ListViewItem lvItem = new ListViewItem(new string[] { itemEquipments.Count.ToString(), addedEquipmentLite.Code, addedEquipmentLite.Name, addedEquipmentLite.Type, addedEquipmentLite.Group, "" });
            if (!addedEquipmentLite.Group.Equals(testGroup))
            {
                this.setOtherGroupEquipmentStyle(lvItem);
            }

            //更新ui
            listViewUsingEquipment.Items.Add(lvItem);
            if (listViewUsingEquipment.Items.Count == 1)
            {
                //设置删除事件
                this.setEquipmentListViewItemRemoveButton(listViewUsingEquipment, btn);
            }
        }

        public void removeEquipment(ListView listViewUsingEquipment,
            Button btn,
            List<EquipmentLite> itemEquipments,
            Dictionary<string, EquipmentLite> itemEquipmentMap,
            string removedEquipmentCode)
        {
            //数据同步
            int removedIndex = itemEquipments.FindIndex(item => item.Code.Equals(removedEquipmentCode));
            itemEquipments.RemoveAt(removedIndex);
            itemEquipmentMap.Remove(removedEquipmentCode);


            //更新ui
            listViewUsingEquipment.Items.RemoveAt(removedIndex);

            //更新设备序号
            updateEquipmentItemOrder(listViewUsingEquipment);
            btn.Visible = false;
        }

        private void updateEquipmentItemOrder(ListView listViewUsingEquipment)
        {
            if (listViewUsingEquipment.Items.Count <= 0)
            {
                return;
            }
            for (int index = 0; index < listViewUsingEquipment.Items.Count; index++)
            {
                ListViewItem curItem = listViewUsingEquipment.Items[index];
                curItem.SubItems[0].Text = (index + 1).ToString();
            }
        }

        private void setOtherGroupEquipmentStyle(ListViewItem viewItem)
        {
            //viewItem.BackColor = this.otherGroupEquipmentBackColor;
            viewItem.ForeColor = ConstHolder.CONST_OTHER_GROUP_EQUIPMENT_FORE_COLOR;
        }

        private void setEquipmentListViewItemRemoveButton(ListView listViewUsingEquipment, Button btn)
        {
            //删除按钮         
            btn.Size = new Size(listViewUsingEquipment.Items[0].SubItems[5].Bounds.Width,
            listViewUsingEquipment.Items[0].SubItems[5].Bounds.Height);
        }

        private void setListViewUsingEquipmentRowHeight(ListView listViewUsingEquipment)
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            listViewUsingEquipment.SmallImageList = imgList;
        }

        private void testApi()
        {
            try
            {

                string url = "https://its-equ.catarc.ac.cn/api/sys/getTokenByUserNo?userNo=07358";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Headers.Add("Origin", "https://its-equ.catarc.ac.cn");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string responseData = streamReader.ReadToEnd();
                    Console.WriteLine(responseData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void testAsyncApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //string url = "https://its-equ.catarc.ac.cn/api/sys/getTokenByUserNo?userNo=07358";
                    //string url = "https://tool.lu/comment/b/y/0/comments?type=0&page=1";
                    string url = "http://rmyc6395.xicp.net:17099/lims/entservice/taskinfotemp/taskinfotemp!getTaskinfoList.action";
                    HttpResponseMessage httpResponse = await client.GetAsync(url);
                    var code = httpResponse.StatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
