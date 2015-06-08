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
using System.IO.Compression;

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

        public String GetMd5HashFromString(String input)
        {
            MD5 md5Hash = MD5.Create();
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

        #region TimeStamp和DateTime转换

        public System.DateTime ConvertTimeStampToDateTime(double timestamp)
        {
            //create a new datetime value based on the unix epoch
            System.DateTime converted = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            //add the timestamp to the value
            System.DateTime newdatetime = converted.AddSeconds(timestamp);
            //return the value in string format
            return newdatetime.ToLocalTime();
        }

        public double ConvertDateTimeToTimestamp(System.DateTime value)
        {
            //create timespan by subtracting the value provided from
            //the unix epoch
            System.TimeSpan span = (value - new System.DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            //return the total seconds (which is a unix timestamp)
            return (double)span.TotalSeconds;
        }
        #endregion

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
                    //int iX = 35;
                    //int iY = 140;
                    //PrintDataGridView.Print(dgv, true, e, ref iX, ref iY);
                }
            }

        }
        #endregion

        #region 上传
        public void UpLoadFileOld(string fileNamePath, string fileName, string uriString)
        {
            // System.Environment.CurrentDirectory + @"\data\data.db"
            // ftp://qyw28051:cyy2014@qyw28051.my3w.com/data.db
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.UploadFile(uriString, "STOR", fileNamePath);
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

        // 根据特定字长设置字体大小 一行
        public Font setStringFontByLongestSize(float longestWidthSize, Font ft, String oriStr, Graphics g)
        {
            SizeF fontSize = g.MeasureString(oriStr, ft);
            for (; (fontSize.Width >= longestWidthSize) && (fontSize.Width > 0); )
            {
                ft = new Font(ft.Name, ft.SizeInPoints - 0.1f);
                fontSize = g.MeasureString(oriStr, ft);
            }
            return ft;
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
                        //for (int i = 0; i < finalValues.Length; i++)
                        //{
                        /*
                        string testS = item.Name.Substring(PreName.Length, item.Name.Length - PreName.Length);
                        int test = int.Parse(testS);
                        if (test == (i + 1))
                         */
                        int i = int.Parse(item.Name.Substring(PreName.Length)) - 1;
                        //if (int.Parse(item.Name.Substring(PreName.Length, item.Name.Length - PreName.Length)) == (i + 1))
                        //{
                        if (i < finalValues.Length)
                        {
                            if (item is ComboBox)
                            {
                                int intParsed;
                                if (finalValues[i].Equals(""))
                                {
                                    item.Text = finalValues[i];
                                }
                                else
                                {
                                    Boolean isInt = int.TryParse(finalValues[i], out intParsed);
                                    if ((isInt && ((item as ComboBox).Items.Count > intParsed) && !item.Name.Equals("tbDz1") && !item.Name.Equals("tbPz1")))
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
                            }
                            else
                            {
                                item.Text = finalValues[i];
                            }
                        }
                        //}
                        //}
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
                    if (ctList[i] is ComboBox)
                    {
                        (ctList[i] as ComboBox).Items.Add(valuesList[i].ToString());
                    }
                    else
                    {
                        ctList[i].Text = valuesList[i].ToString();
                    }
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
        public void SoundPlay(Stream stream)
        {
            System.Media.SoundPlayer media = new System.Media.SoundPlayer(stream);
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

        /// <summary>
        /// 获取远程文件中的字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public String getOnlineFile(String fileUrl)
        {
            WebClient wcClient = new WebClient();
            long fileLength = 0;
            String updateFileUrl = fileUrl;

            WebRequest webReq = WebRequest.Create(updateFileUrl);
            WebResponse webRes = null;
            Stream srm = null;
            StreamReader srmReader = null;
            String ss = "";
            try
            {
                webRes = webReq.GetResponse();
                fileLength = webRes.ContentLength;

                srm = webRes.GetResponseStream();
                srmReader = new StreamReader(srm);

                byte[] bufferbyte = new byte[fileLength];
                int allByte = (int)bufferbyte.Length;
                int startByte = 0;
                while (fileLength > 0)
                {
                    Application.DoEvents();
                    int downByte = srm.Read(bufferbyte, startByte, allByte);
                    if (downByte == 0) { break; };
                    startByte += downByte;
                    allByte -= downByte;

                    float part = (float)startByte / 1024;
                    float total = (float)bufferbyte.Length / 1024;
                    int percent = Convert.ToInt32((part / total) * 100);

                }
                ss = Encoding.Default.GetString(bufferbyte).Trim();

                srm.Close();
                srmReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取信息异常: " + ex.Message);
                //throw;
                return "";
            }
            return ss;
        }

        /// <summary>
        /// 比较版本是否需要更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public bool compareVersion(string currentVersion, string newVersion, int versionBit)
        {
            string[] currentVersionArray = currentVersion.Split('.');
            string[] newVersionArray = newVersion.Split('.');

            for (int i = 0; i < currentVersionArray.Length; i++)
            {
                if (currentVersionArray[i].Length < versionBit)
                {
                    currentVersionArray[i] = "0" + currentVersionArray[i];
                }
                if (newVersionArray[i].Length < versionBit)
                {
                    newVersionArray[i] = "0" + newVersionArray[i];
                }
            }
            string currentVersionString = currentVersionArray[0] + currentVersionArray[1] + currentVersionArray[2];
            string newVersionString = newVersionArray[0] + newVersionArray[1] + newVersionArray[2];
            if (int.Parse(currentVersionString) < int.Parse(newVersionString))
            {
                return true;
            }
            return false;
        }

        // 判断是否是数字
        public bool IsNumeric(String str)
        {
            String pattern = @"^[-]?\d+[.]?\d*$";
            return Regex.IsMatch(str, pattern);
        }

        // 判断是否是整数
        public bool IsIntegerByRegex(String str)
        {
            String pattern = @"^\d*$";
            return Regex.IsMatch(str, pattern);
        }

        #region 上传文件
        private FtpWebRequest GetRequest(string URI, string username, string password)
        {
            //根据服务器信息FtpWebRequest创建类的对象
            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(URI);
            //提供身份验证信息
            result.Credentials = new System.Net.NetworkCredential(username, password);
            //设置请求完成之后是否保持到FTP服务器的控制连接，默认值为true
            result.KeepAlive = false;
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileinfo">需要上传的文件</param>
        /// <param name="targetDir">目标路径</param>
        /// <param name="hostname">ftp地址</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public void UploadFile(FileInfo fileinfo, string targetDir, string newName, string hostname, string username, string password)
        {
            //1. check target
            string target;
            if (targetDir.Trim() == "")
            {
                return;
            }
            target = newName;  //使用临时文件名


            string URI = "FTP://" + hostname + "/" + targetDir + "/" + target;
            ///WebClient webcl = new WebClient();
            System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);

            //设置FTP命令 设置所要执行的FTP命令，
            //ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;//假设此处为显示指定路径下的文件列表
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            //指定文件传输的数据类型
            ftp.UseBinary = true;
            ftp.UsePassive = true;

            //告诉ftp文件大小
            ftp.ContentLength = fileinfo.Length;
            //缓冲大小设置为2KB
            const int BufferSize = 2048;
            byte[] content = new byte[BufferSize - 1 + 1];
            int dataRead;

            //打开一个文件流 (System.IO.FileStream) 去读上传的文件
            using (FileStream fs = fileinfo.OpenRead())
            {
                try
                {
                    //把上传的文件写入流
                    using (Stream rs = ftp.GetRequestStream())
                    {
                        do
                        {
                            //每次读文件流的2KB
                            dataRead = fs.Read(content, 0, BufferSize);
                            rs.Write(content, 0, dataRead);
                        } while (!(dataRead < BufferSize));
                        rs.Close();
                    }

                }
                catch { }
                finally
                {
                    fs.Close();
                }

            }

            ftp = null;
            //设置FTP命令
            ftp = GetRequest(URI, username, password);
            ftp.Method = System.Net.WebRequestMethods.Ftp.Rename; //改名
            ftp.RenameTo = newName;
            try
            {
                ftp.GetResponse();
            }
            catch (Exception ex)
            {
                ftp = GetRequest(URI, username, password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile; //删除
                ftp.GetResponse();
                throw ex;
            }
            finally
            {
                //fileinfo.Delete();
            }

            // 可以记录一个日志  "上传" + fileinfo.FullName + "上传到" + "FTP://" + hostname + "/" + targetDir + "/" + fileinfo.Name + "成功." );
            ftp = null;

            #region
            /*****
             *FtpWebResponse
             * ****/
            //FtpWebResponse ftpWebResponse = (FtpWebResponse)ftp.GetResponse();
            #endregion
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localDir">下载至本地路径</param>
        /// <param name="FtpDir">ftp目标文件路径</param>
        /// <param name="FtpFile">从ftp要下载的文件名</param>
        /// <param name="hostname">ftp地址即IP</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public void DownloadFile(string localDir, string FtpDir, string FtpFile, string hostname, string username, string password)
        {
            string URI = "FTP://" + hostname + "/" + FtpDir + "/" + FtpFile;
            string tmpname = Guid.NewGuid().ToString();
            string localfile = localDir + @"\" + tmpname;

            System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
            ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
            ftp.UseBinary = true;
            ftp.UsePassive = false;

            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    //loop to read & write to file
                    using (FileStream fs = new FileStream(localfile, FileMode.CreateNew))
                    {
                        try
                        {
                            byte[] buffer = new byte[2048];
                            int read = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                            } while (!(read == 0));
                            responseStream.Close();
                            fs.Flush();
                            fs.Close();
                        }
                        catch (Exception)
                        {
                            //catch error and delete file only partially downloaded
                            fs.Close();
                            //delete target file as it's incomplete
                            File.Delete(localfile);
                            throw;
                        }
                    }

                    responseStream.Close();
                }

                response.Close();
            }



            try
            {
                File.Delete(localDir + @"\" + FtpFile);
                File.Move(localfile, localDir + @"\" + FtpFile);


                ftp = null;
                ftp = GetRequest(URI, username, password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
                ftp.GetResponse();

            }
            catch (Exception ex)
            {
                File.Delete(localfile);
                throw ex;
            }

            // 记录日志 "从" + URI.ToString() + "下载到" + localDir + @"\" + FtpFile + "成功." );
            ftp = null;
        }

        /// <summary>
        /// 搜索远程文件
        /// </summary>
        /// <param name="targetDir"></param>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="SearchPattern"></param>
        /// <returns></returns>
        public List<string> ListDirectory(string targetDir, string hostname, string username, string password, string SearchPattern)
        {
            List<string> result = new List<string>();
            try
            {
                string URI = "FTP://" + hostname + "/" + targetDir + "/" + SearchPattern;

                System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
                ftp.UsePassive = true;
                ftp.UseBinary = true;


                string str = GetStringResponse(ftp);
                str = str.Replace("\r\n", "\r").TrimEnd('\r');
                str = str.Replace("\n", "\r");
                if (str != string.Empty)
                    result.AddRange(str.Split('\r'));

                return result;
            }
            catch { }
            return null;
        }

        private string GetStringResponse(FtpWebRequest ftp)
        {
            //Get the result, streaming to a string
            string result = "";
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                long size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream, System.Text.Encoding.Default))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }

                    datastream.Close();
                }

                response.Close();
            }

            return result;
        }

        /// 在ftp服务器上创建目录
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        /// <param name="ftpHostIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public void MakeDir(string dirName, string ftpHostIP, string username, string password)
        {
            try
            {
                string uri = "ftp://" + ftpHostIP + "/" + dirName;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region bitmap String互转

        //图片 转为 base64编码的文本
        public String ImgToBase64String(Bitmap btmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                btmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
            catch (Exception ex)
            {
                return ("转换失败\nException:" + ex.Message);
            }
        }

        //base64编码的文本 转为    图片
        public Bitmap Base64StringToImage(String inputStr)
        {
            Bitmap btmp = null;
            try
            {
                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                btmp = new Bitmap(ms);
                ms.Close();
                return btmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("转换失败\nException：" + ex.Message);
                return btmp;
            }
        }
        #endregion

        #region 压缩和解压缩String
        public string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
        #endregion

        // 检测远程文件是否存在
        public bool UriExists(string url)
        {
            try
            {
                new System.Net.WebClient().OpenRead(url);
                return true;
            }
            catch (System.Net.WebException)
            {
                return false;
            }
        }

        public bool ifUpdateDatabasecheckLastModifiedTime(bool isDownload)
        {
            HttpWebRequest gameFile = (HttpWebRequest)WebRequest.Create(MainWindow.ONLINE_DATABASE_LOCATION_DIR + MainWindow.ONLINE_DATABASE_FILE_PREFIX);
            gameFile.Timeout = 5000;
            HttpWebResponse gameFileResponse;
            try
            {
                gameFileResponse = (HttpWebResponse)gameFile.GetResponse();
            }
            catch
            {
                if (isDownload)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            DateTime localFileModifiedTime = File.GetLastWriteTime(MainWindow.LOCAL_DATABASE_LOCATION);
            DateTime onlineFileModifiedTime = gameFileResponse.LastModified;
            gameFile.Abort();
            gameFileResponse.Close();
            if (localFileModifiedTime >= onlineFileModifiedTime)
            {
                if (isDownload)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                //Download new Update
                if (isDownload)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // 获取本地文件大小
        public long getLocalFileSize(String filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Length;
        }

    }
}
