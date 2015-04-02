using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace VividManagementApplication
{
    class FormBasicFeatrues
    {
        private static FormBasicFeatrues _formBasicFeatrues = null;

        public static FormBasicFeatrues GetInstence()
        {
            if (_formBasicFeatrues == null)
            {
                _formBasicFeatrues = new FormBasicFeatrues();
            }
            return _formBasicFeatrues;
        }

        public string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #region 打印

        public int PrintPageHeight = 1169;//打印的默认高度
        public int PrintPageWidth = 827;//打印的默认宽度

        public string Page_Size(int n)
        {
            string pageN = "";//纸张的名称
            switch (n)
            {
                case 1: { pageN = "A5"; PrintPageWidth = 583; PrintPageHeight = 827; break; }
                case 2: { pageN = "A6"; PrintPageWidth = 413; PrintPageHeight = 583; break; }
                case 3: { pageN = "B5(ISO)"; PrintPageWidth = 693; PrintPageHeight = 984; break; }
                case 4: { pageN = "B5(JIS)"; PrintPageWidth = 717; PrintPageHeight = 1012; break; }
                case 5: { pageN = "Double Post Card"; PrintPageWidth = 583; PrintPageHeight = 787; break; }
                case 6: { pageN = "Envelope #10"; PrintPageWidth = 412; PrintPageHeight = 950; break; }
                case 7: { pageN = "Envelope B5"; PrintPageWidth = 693; PrintPageHeight = 984; break; }
                case 8: { pageN = "Envelope C5"; PrintPageWidth = 638; PrintPageHeight = 902; break; }
                case 9: { pageN = "Envelope DL"; PrintPageWidth = 433; PrintPageHeight = 866; break; }
                case 10: { pageN = "Envelope Monarch"; PrintPageWidth = 387; PrintPageHeight = 750; break; }
                case 11: { pageN = "ExeCutive"; PrintPageWidth = 725; PrintPageHeight = 1015; break; }
                case 12: { pageN = "Legal"; PrintPageWidth = 850; PrintPageHeight = 1400; break; }
                case 13: { pageN = "Letter"; PrintPageWidth = 850; PrintPageHeight = 1100; break; }
                case 14: { pageN = "Post Card"; PrintPageWidth = 394; PrintPageHeight = 583; break; }
                case 15: { pageN = "16K"; PrintPageWidth = 775; PrintPageHeight = 1075; break; }
                case 16: { pageN = "8.5x13"; PrintPageWidth = 850; PrintPageHeight = 1300; break; }
            }
            return pageN;//返回纸张的名
        }

        public void PrintPageSet(Panel pa, int x, int y, DataGridView dgv, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;   //先建立画布 
            //g.DrawImage(this.BackgroundImage, 50, 50);
            foreach (Control item in pa.Controls)
            {
                if (item is Label)
                {
                    Control tx = (item as Control);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x, tx.Top + y);
                }
                if (item is TextBox)
                {
                    TextBox tx = (item as TextBox);
                    g.DrawString(tx.Text, tx.Font, new SolidBrush(tx.ForeColor), tx.Left + x, tx.Top + y + 5);
                    if (tx.BorderStyle == BorderStyle.FixedSingle)
                    {
                        g.DrawRectangle(new Pen(Color.Black), tx.Left + x, tx.Top + y, tx.Width, tx.Height);
                    }
                }
                /*
                if (item is DataGridView)
                {
                    DataGridView tx = (item as DataGridView);
                    for (int i = 0; i < tx.ColumnCount; i++)
                    {
                        for (int j = 0; j < tx.RowCount; j++)
                        {
                            g.DrawString(tx.Rows[j].Cells[i].Value.ToString(), tx.Font, new SolidBrush(item.ForeColor), tx.Left + x, tx.Top + y);
                        }
                    }
                }
                */

                if (dgv != null)
                {
                    int iX = 35;
                    int iY = 140;
                    //PrintDataGridView.Print(dgv, true, e, ref iX, ref iY);
                }
            }

        }
        #endregion

        #region 上传
        public void UpLoadFile(string fileNamePath, string fileName, string uriString)
        {
            // System.Environment.CurrentDirectory + @"\data\data.db"
            // ftp://qyw28051:cyy2014@qyw28051.my3w.com/data.db
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.UploadFile(uriString, fileNamePath);
        }

        #endregion

        // 格式化String 如 0 -> 00000
        public string FormatID(string originalString, int bit, string replaceString)
        {
            string result = "";
            int gap = 0;
            string temp = "";
            if (originalString.Length <= bit)
            {
                gap = bit - originalString.Length;
                for (int i = 0; i < gap; i++)
                {
                    temp += replaceString;
                }
                result = temp + originalString;
            }
            return result;
        }

        // 记录错误
        public void RecordLog(Exception ex, string preLog)
        {
            //string log = preLog + "\r\n###异常消息Message: " + ex.Message + "\r\n###引发当前异常的方法TargetSite: " + ex.TargetSite + "\r\n###当前异常的实例InnerException: " + ex.InnerException + "\r\n###导致错误的应用程序或者对象名称Source: " + ex.Source + "\r\n###当前异常FullMessage: " + ex.ToString() + "\r\n===================================================\r\n";
            string log = preLog + "\r\n###异常消息Message: " + ex.Message + "\r\n###当前异常StackTrace: " + ex.StackTrace + "\r\n===================================================\r\n";
            string fileName = System.Environment.CurrentDirectory + @"\config\log.txt";
            string fileString = "";
            if (System.IO.File.Exists(fileName))
            {
                using (System.IO.FileStream fszz = System.IO.File.OpenRead(fileName))
                {
                    byte[] bytes = new byte[fszz.Length];
                    fszz.Read(bytes, 0, bytes.Length);

                    fileString = Encoding.UTF8.GetString(bytes);
                }
            }
            using (System.IO.FileStream fs = System.IO.File.Create(fileName))
            {
                StringBuilder sb = new StringBuilder();
                byte[] info = new UTF8Encoding().GetBytes(DateTime.Now + "-" + log + "\r\n" + fileString);
                fs.Write(info, 0, info.Length);
            }
        }

        public string RemoveEmptyItemFromString(string originalString, char split)
        {
            string resultString = "";
            string[] stringArray = originalString.Split(split);
            List<string> stringList = stringArray.ToList<string>();

            List<int> index = new List<int>();
            for (int i = 0; i < stringList.Count; i++)
            {
                if (stringList[i].Equals("") || stringList[i].Trim().Equals(""))
                {
                    index.Add(i);
                }
            }

            #region list 从大到小排列
            index.Sort(delegate(int a, int b)
                         {
                             return a.CompareTo(b);
                         });
            index.Sort((a, b) => b.CompareTo(a));
            #endregion

            foreach (int ind in index)
            {
                stringList.RemoveAt(ind);
            }

            foreach (string temp in stringList)
            {
                resultString += temp.Trim() + split.ToString();
            }
            if (resultString.Equals(""))
            {
                resultString = "";
            }
            else
            {
                resultString = resultString.Substring(0, resultString.Length - 1);
            }
            return resultString;
        }

        // 金额的大小写转换
        public string MoneyToUpper(string strAmount)
        {
            string functionReturnValue = null;
            bool IsNegative = false; // 是否是负数
            if (strAmount.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                strAmount = strAmount.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            strAmount = Math.Round(double.Parse(strAmount), 2).ToString();
            if (strAmount.IndexOf(".") > 0)
            {
                if (strAmount.IndexOf(".") == strAmount.Length - 2)
                {
                    strAmount = strAmount + "0";
                }
            }
            else
            {
                strAmount = strAmount + ".00";
            }
            strLower = strAmount;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            functionReturnValue = strUpper;

            if (IsNegative == true)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }

        // dgv补全
        public DataGridView FullFileDataGridView(int TotalRows, DataGridView dgv)
        {
            if (dgv.Rows.Count < TotalRows)
            {
                int diff = TotalRows - dgv.Rows.Count;
                for (int i = 0; i < diff; i++)
                {
                    dgv.Rows.Add();
                }
            }
            return dgv;
        }

        // 缘模原样迁移从属panel
        public void moveParentPanel(Panel fromP, Panel toP)
        {
            foreach (Control item in fromP.Controls)
            {
                //int oriX = item.Location.X;
                //int oriY = item.Location.Y;
                item.Parent = toP;
                //item.Location = new Point(oriX+fromP.Location.X,oriY+fromP.Location.Y);
            }
        }

        // 设置数值
        public void SetControlsVaule(string PreName, Panel panelPlatform, string[] finalValues)
        {
            foreach (Control item in panelPlatform.Controls)
            {
                if (item.Name.Length >= PreName.Length)
                {
                    if (item.Name.Substring(0, PreName.Length).Equals(PreName))
                    {
                        for (int i = 0; i < finalValues.Length; i++)
                        {
                            /*
                            string testS = item.Name.Substring(PreName.Length, item.Name.Length - PreName.Length);
                            int test = int.Parse(testS);
                            if (test == (i + 1))
                             */
                            if (int.Parse(item.Name.Substring(PreName.Length, item.Name.Length - PreName.Length)) == (i + 1))
                            {
                                if (item is ComboBox)
                                {
                                    int intParsed;
                                    Boolean isInt = int.TryParse(finalValues[i], out intParsed);
                                    if (isInt && ((item as ComboBox).Items.Count > intParsed))
                                    {
                                        // 是数字  
                                        (item as ComboBox).SelectedIndex = intParsed;
                                    }
                                    else
                                    {
                                        // 不是数字
                                        item.Text = finalValues[i];
                                    }
                                }
                                else
                                {
                                    item.Text = finalValues[i];
                                }
                            }
                        }
                    }
                }
            }
        }

        // 用List设置数值
        public void SetControlsVauleByControlList(List<Control> ctList, List<String> valuesList)
        {
            if (ctList.Count == valuesList.Count)
            {
                for (int i = 0; i < ctList.Count; i++)
                {
                    ctList[i].Text = valuesList[i].ToString();
                }
            }
        }

        // 修改
        public string[] GetControlsVaule(string PreName, Panel panelPlatform, int indexCount)
        {
            string[] getVaules = new string[indexCount];

            foreach (Control item in panelPlatform.Controls)
            {
                if (item.Name.Length >= PreName.Length)
                {
                    if (item.Name.Substring(0, PreName.Length).Equals(PreName))
                    {
                        int index = int.Parse(item.Name.Substring(PreName.Length, item.Name.Length - PreName.Length));

                        if (item is ComboBox)
                        {
                            getVaules[index - 1] = (item as ComboBox).SelectedIndex.ToString();
                        }
                        else
                        {
                            getVaules[index - 1] = item.Text;
                        }
                    }
                }
            }
            return getVaules;
        }

        // 播放声音
        public void SoundPlay(string filename)
        {
            System.Media.SoundPlayer media = new System.Media.SoundPlayer(filename);
            media.Play();
        }

        /// <summary>
        /// 检测是否有必填
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public Boolean isPassValidateControls(List<Control> cons)
        {
            Boolean isPass = false;
            foreach (Control ct in cons)
            {
                if (ct.Text.Equals(""))
                {
                    return false;
                }
                else
                {
                    isPass = true;
                }
            }
            return isPass;
        }

        /// <summary>
        /// 重新触发comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public void reTriggleCombox(ComboBox cb)
        {
            int index = cb.SelectedIndex;
            cb.SelectedIndex = -1;
            cb.SelectedIndex = index;
        }

        /// <summary>
        /// 在字中加入特定字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public String addCharIntoString(String addingChar, String oriString)
        {
            String newString = "";
            List<String> newStringList = new List<string>();
            if (!oriString.Equals("") || oriString != null)
            {
                for (int i = 0; i < oriString.Length; i++)
                {
                    newStringList.Add(oriString.Substring(i, 1));
                    if (i < oriString.Length - 1)
                    {
                        newStringList.Add(addingChar);
                    }
                }
                foreach (String item in newStringList)
                {
                    newString += item;
                }
            }
            return newString;
        }
    }
}
