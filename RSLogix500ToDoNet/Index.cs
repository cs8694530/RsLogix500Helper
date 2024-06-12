using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RSLogix500ToDoNet
{
    public partial class Index : Form
    {
        public Index()
        {
            InitializeComponent();
        }

        RsLogix500Analyze ob;

        private void SeachBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            Dialog.Title = "請選擇RsLogix500檔案";
            Dialog.Filter = "RsLogix500專案檔案(*.SLC)|*.SLC";
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                Path_Label.Text = Dialog.FileName;

                ob = new RsLogix500Analyze(Dialog.FileName);
                ListBoxGetList(ob.LADCount);
                Filter_ListBox.SelectedIndex = 0;
                UpdateTextBox();
            }
        }

        private void TextBoxGetText(List<RsLogix500Analyze.LogixData> datas)
        {
            textBox1.Text = string.Empty;

            List<RsLogix500Analyze.LogixData> tempdatas = datas.OrderBy(x => x.Ladder).ThenBy(x => x.Rung).ToList();

            foreach (RsLogix500Analyze.LogixData data in tempdatas)
            {
                textBox1.Text += $"LAD{data.Ladder} Rung{data.Rung} : {data.Data}\r\n\r\n";
            }
        }
        private void ListBoxGetList(int LadCount)
        {
            Filter_ListBox.Items.Clear();
            Filter_ListBox.Items.Add($"全部");

            for (int i = 2; i < LadCount + 2; i++)
            {
                Filter_ListBox.Items.Add($"LAD {i}");
            }
        }

        private void Filter_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextBox();
        }

        private void UpdateTextBox()
        {
            if (Filter_ListBox.SelectedItem.ToString() == "全部")
            {
                if (ShowConvertOrRaw)
                {
                    TextBoxGetText(ob.ConvertedData);
                    TotalCount_Label.Text = $"TotalCount : {ob.ConvertedData.Count}";
                }
                else
                {
                    TextBoxGetText(ob.RawData);
                    TotalCount_Label.Text = $"TotalCount : {ob.RawData.Count}";
                }
            }
            else
            {
                if (ShowConvertOrRaw)
                {
                    string Filter_LAD = Filter_ListBox.SelectedItem.ToString();
                    Filter_LAD = Regex.Match(Filter_LAD, @"[LAD]\s{1}(.*)").Groups[1].Value;
                    List<RsLogix500Analyze.LogixData> logixDatas = ob.ConvertedData.Where(x => x.Ladder == Convert.ToInt32(Filter_LAD)).ToList();
                    TextBoxGetText(logixDatas);
                    TotalCount_Label.Text = $"TotalCount : {logixDatas.Count}";
                }
                else
                {
                    string Filter_LAD = Filter_ListBox.SelectedItem.ToString();
                    Filter_LAD = Regex.Match(Filter_LAD, @"[LAD]\s{1}(.*)").Groups[1].Value;
                    List<RsLogix500Analyze.LogixData> logixDatas = ob.RawData.Where(x => x.Ladder == Convert.ToInt32(Filter_LAD)).ToList();
                    TextBoxGetText(logixDatas);
                    TotalCount_Label.Text = $"TotalCount : {logixDatas.Count}";
                }

            }
        }

        // True => Convert , False => Raw
        bool ShowConvertOrRaw = true;

        private void RawData_BTN_Click(object sender, EventArgs e)
        {
            ShowConvertOrRaw = false;
            UpdateTextBox();
        }

        private void ConvertData_BTN_Click(object sender, EventArgs e)
        {
            ShowConvertOrRaw = true;
            UpdateTextBox();
        }

        Remark remark;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(remark != null)remark.Dispose();
            remark = new Remark();
            remark.Show();
        }
    }
}
