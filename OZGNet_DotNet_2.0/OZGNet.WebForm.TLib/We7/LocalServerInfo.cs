using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace OZGNet.WebForm.ThirdPart.We7
{
    /// <summary>
    /// ���ط�������Ϣ
    /// </summary>
    [Serializable]
    public class LocalServerInfo
    {
        string cpuID;
        /// <summary>
        /// 
        /// </summary>
        public string CpuID
        {
            get { return cpuID; }
        }

        string hdID;
        /// <summary>
        /// 
        /// </summary>
        public string HdID
        {
            get { return hdID; }
        }

        string websitePath;
        /// <summary>
        /// 
        /// </summary>
        public string WebsitePath
        {
            get { return websitePath; }
            set { websitePath = value; }
        }

        DateTime date;
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public LocalServerInfo()
        {
            ManagementClass mc;
            ManagementObjectCollection moc;

            //��ȡ���ط�����CPU���к�
            mc = new ManagementClass("Win32_Processor");
            moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuID = mo.Properties["ProcessorId"].Value.ToString();
            }

            //��ȡ���ط�����Ӳ��ID
            mc = new ManagementClass("Win32_DiskDrive");
            moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                hdID = mo.Properties["Model"].Value.ToString();
            }

            //��ȡ���ط�������վ����·��
            //localInfo.WebsitePath = Server.MapPath("/");

            //��ȡ���ط���������
            date = DateTime.Today;

            moc = null;
            mc = null;
        }
    }
}
