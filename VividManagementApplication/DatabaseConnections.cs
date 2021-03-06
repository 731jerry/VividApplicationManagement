using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Data;

namespace VividManagementApplication
{
    class DatabaseConnections
    {
        //连接用的字符串  
        private static String OnlineConnStr;
        private static String LocalConnStr;

        //public String OnlineConnStr
        //{
        //    get { return this.onlineConnStr; }
        //    set { this.onlineConnStr = value; }
        //}

        //public String LocalConnStr
        //{
        //    get { return this.localConnStr; }
        //    set { this.localConnStr = value; }
        //}

        private DatabaseConnections()
        {
        }

        //DbManager单实例  
        private static DatabaseConnections _instance = null;
        public static DatabaseConnections Connector
        {
            get
            {
                OnlineConnStr = @"server=121.42.154.95; user id=vivid; password=vivid; database=vivid;Charset=utf8;";
                LocalConnStr = "Data Source =" + MainWindow.LOCAL_DATABASE_LOCATION;
                if (_instance == null)
                {
                    _instance = new DatabaseConnections();
                }
                return _instance;
            }
        }

        #region 联网
        // MySQL
        /*
        public void OnlineDbOpen()
        {
            try
            {
                if (onlineSqlConnection.State == ConnectionState.Open)
                {
                    onlineSqlConnection.Close();
                }
                onlineSqlConnection.Open();
            }
            catch
            {
                //RecordLog("无法打开连接!\r\nTargetSite: " + ex.TargetSite + "\r\n" + ex.ToString());
                //System.Windows.Forms.MessageBox.Show(ex.Message, "无法打开数据库连接！");
                throw;
            }
        }

        public void OnlineDbClose()
        {
            onlineSqlConnection.Close();
        }
        */

        public void UserLogin(string acc, string psw)
        {
            using (MySqlConnection con = new MySqlConnection(OnlineConnStr))
            {
                string hash = FormBasicFeatrues.GetInstence().GetMd5Hash(MD5.Create(), psw);

                StringBuilder sbSQL = new StringBuilder(
                        @"SELECT Count(id),id,userid,password,companyNickname,workloads,company,companyowner,address,bankname,bankcard,phone,fax,QQ,email,cast(GZB_addtime as char) as GZB_addtime,GZB_degree,GZB_expiretime,GZB_isonline,notification,companyBalance,GZB_signature FROM users WHERE userid = '");
                sbSQL.Append(acc);
                sbSQL.Append(@"'");
                sbSQL.Append(@" AND password = '");
                sbSQL.Append(hash.ToLower());
                sbSQL.Append(@"'");

                string SQLforGeneral = sbSQL.ToString();

                using (MySqlCommand cmd = new MySqlCommand(SQLforGeneral, con))
                {
                    con.Open();
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        MainWindow.IS_PASSWORD_CORRECT = (int.Parse((dataReader["Count(id)"].ToString() == "") ? "0" : dataReader["Count(id)"].ToString()) == 1) ? true : false;
                        if (MainWindow.IS_PASSWORD_CORRECT)
                        {
                            MainWindow.ID = dataReader["id"].ToString();
                            MainWindow.USER_ID = dataReader["userid"].ToString();
                            MainWindow.PASSWORD_HASH = dataReader["password"].ToString();
                            MainWindow.COMPANY_NICKNAME = dataReader["companyNickname"].ToString();
                            MainWindow.WORKLOADS = dataReader["workloads"].ToString();
                            MainWindow.COMPANY_NAME = dataReader["company"].ToString();
                            MainWindow.COMPANY_OWNER = dataReader["companyowner"].ToString();
                            MainWindow.ADDRESS = dataReader["address"].ToString();
                            MainWindow.BANK_NAME = dataReader["bankname"].ToString();
                            MainWindow.BANK_CARD = dataReader["bankcard"].ToString();
                            MainWindow.PHONE = dataReader["phone"].ToString();
                            MainWindow.FAX = dataReader["fax"].ToString();
                            MainWindow.QQ = dataReader["QQ"].ToString();
                            MainWindow.EMAIL = dataReader["email"].ToString();
                            MainWindow.NOTIFICATION = dataReader["notification"].ToString();
                            MainWindow.IS_USER_ONLINE = (int.Parse(dataReader["GZB_isonline"].ToString().Equals("") ? "0" : dataReader["GZB_isonline"].ToString()) == 0) ? false : true;
                            MainWindow.DEGREE = int.Parse(dataReader["GZB_degree"].ToString());
                            MainWindow.ADDTIME = DateTime.Parse(dataReader["GZB_addtime"].ToString());
                            MainWindow.EXPIRETIME = DateTime.Parse(dataReader["GZB_expiretime"].ToString());
                            MainWindow.COMPANY_BALANCE = float.Parse(dataReader["companyBalance"].ToString());
                            MainWindow.SIGNATURE = dataReader["GZB_signature"].ToString();
                            //MainWindow.LAST_LOGON_TIME = dataReader["lastLogonTime"].ToString().Equals("") ? "首次登录" : dataReader["lastLogonTime"].ToString();
                        }
                    }
                    dataReader.Close();
                }
            }
        }

        // 插入数据
        public int OnlineInsertData(String table, String query, String value)
        {
            int affectedRows = -1;
            using (MySqlConnection con = new MySqlConnection(OnlineConnStr))
            {
                String SQLforGeneral = "INSERT INTO " + table + " (" + query + ") VALUES(" + value + ")";
                using (MySqlCommand cmdInsert = new MySqlCommand(SQLforGeneral, con))
                {
                    con.Open();
                    affectedRows = cmdInsert.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        // 修改数据
        public int OnlineUpdateData(string table, string[] query, string[] value, string id)
        {
            int affectedRows = -1;
            using (MySqlConnection con = new MySqlConnection(OnlineConnStr))
            {
                string innerSQL = "";

                for (int i = 0; i < query.Length; i++)
                {
                    innerSQL += query[i] + " = '" + value[i] + "',";
                }
                if (!innerSQL.Equals(""))
                {
                    innerSQL = innerSQL.Substring(0, innerSQL.Length - 1); // 去掉最后的逗号
                }
                string SQLforGeneral = "UPDATE " + table + " SET " + innerSQL + " WHERE id = '" + id + "'";
                using (MySqlCommand cmdInsert = new MySqlCommand(SQLforGeneral, con))
                {
                    con.Open();
                    affectedRows = cmdInsert.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        // 修改原始数据
        public int OnlineUpdateDataFromOriginalSQL(String sql)
        {
            int affectedRows = -1;
            using (MySqlConnection con = new MySqlConnection(OnlineConnStr))
            {
                using (MySqlCommand cmdInsert = new MySqlCommand(sql, con))
                {
                    cmdInsert.CommandTimeout = 0;
                    con.Open();
                    affectedRows = cmdInsert.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        public List<String> OnlineGetOneRowDataById(String table, List<String> query, String baseName, String id)
        {
            List<String> resultsStringList;
            using (MySqlConnection con = new MySqlConnection(OnlineConnStr))
            {
                // ORDER BY id ASC
                String innerSQL = "";

                for (int i = 0; i < query.Count; i++)
                {
                    innerSQL += query[i] + ",";
                }
                if (!innerSQL.Equals(""))
                {
                    innerSQL = innerSQL.Substring(0, innerSQL.Length - 1); // 去掉最后的逗号
                }
                String sql = "SELECT " + innerSQL + " FROM " + table + " WHERE " + baseName + "='" + id + "'";//建表语句  
                using (MySqlCommand cmdCreateTable = new MySqlCommand(sql, con))
                {
                    cmdCreateTable.CommandText = sql;
                    con.Open();
                    MySqlDataReader dataReader = cmdCreateTable.ExecuteReader();
                    //String[] resultsStringArray = new String[query.Count];
                    resultsStringList = new List<string>();

                    while (dataReader.Read())
                    {
                        for (int i = 0; i < query.Count; i++)
                        {
                            resultsStringList.Add(dataReader[query[i]].ToString());
                        }
                    }
                    dataReader.Close();
                }
            }
            return resultsStringList;
        }

        public List<List<String>> OnlineGetRowsDataById(String table, List<String> query, String baseName, String id, String condition)
        {
            List<List<String>> resultsStringList;
            using (MySqlConnection con = new MySqlConnection(OnlineConnStr))
            {
                // ORDER BY id ASC
                String innerSQL = "";

                /*
                for (int i = 0; i < query.Count; i++)
                {
                    innerSQL += query[i] + ",";
                }
                if (!innerSQL.Equals(""))
                {
                    innerSQL = innerSQL.Substring(0, innerSQL.Length - 1); // 去掉最后的逗号
                }
                 */
                innerSQL = String.Join(",", query);

                String sql = "SELECT " + innerSQL + " FROM " + table + " WHERE " + baseName + "='" + id + "' " + condition;//建表语句  
                using (MySqlCommand cmdCreateTable = new MySqlCommand(sql, con))
                {
                    cmdCreateTable.CommandTimeout = 0;
                    cmdCreateTable.CommandText = sql;
                    con.Open();
                    MySqlDataReader dataReader = cmdCreateTable.ExecuteReader();
                    //String[] resultsStringArray = new String[query.Count];
                    resultsStringList = new List<List<String>>();

                    while (dataReader.Read())
                    {
                        List<String> temp = new List<string>();
                        for (int i = 0; i < query.Count; i++)
                        {
                            temp.Add(dataReader[query[i]].ToString());
                        }
                        resultsStringList.Add(temp);
                    }
                    dataReader.Close();
                }
            }
            return resultsStringList;
        }

        public List<List<String>> OnlineGetRowsDataByCondition(String table, List<String> query, String condition)
        {
            List<List<String>> resultsStringList;
            using (MySqlConnection con = new MySqlConnection(OnlineConnStr))
            {
                // ORDER BY id ASC
                String innerSQL = String.Join(",", query);

                String sql = "SELECT " + innerSQL + " FROM " + table + condition;//建表语句  
                using (MySqlCommand cmdCreateTable = new MySqlCommand(sql, con))
                {
                    cmdCreateTable.CommandTimeout = 0;
                    cmdCreateTable.CommandText = sql;
                    con.Open();
                    MySqlDataReader dataReader = cmdCreateTable.ExecuteReader();
                    //String[] resultsStringArray = new String[query.Count];
                    resultsStringList = new List<List<String>>();

                    while (dataReader.Read())
                    {
                        List<String> temp = new List<string>();
                        for (int i = 0; i < query.Count; i++)
                        {
                            temp.Add(dataReader[query[i]].ToString());
                        }
                        resultsStringList.Add(temp);
                    }
                    dataReader.Close();
                }
            }
            return resultsStringList;
        }

        #endregion

        #region 本地

        public void LocalCreateDatabase(String databaseName)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                string sql = @" --
                                -- File generated with SQLiteStudio v3.0.3 on 周四 4月 9 14:58:30 2015
                                --
                                -- Text encoding used: GBK
                                --
                                --PRAGMA foreign_keys = off;
                                --BEGIN TRANSACTION;

                                -- Table: goods
                                CREATE TABLE IF NOT EXISTS goods (id INTEGER PRIMARY KEY AUTOINCREMENT, goodId VARCHAR UNIQUE, name VARCHAR (250), guige VARCHAR (250), unit VARCHAR (50), dengji VARCHAR (50), storageName VARCHAR (50), storageManager VARCHAR (100), storageManagerPhone VARCHAR (50), storageLocation VARCHAR (50), storageAddress VARCHAR (250), initalCount VARCHAR DEFAULT (0), purchasePrice VARCHAR (50), purchaseTotal VARCHAR (100), currentCount VARCHAR (100), currentTotal VARCHAR (150), currntsalesPrice VARCHAR (100), beizhu VARCHAR (250), addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: clients
                                CREATE TABLE IF NOT EXISTS clients (id INTEGER PRIMARY KEY AUTOINCREMENT, clientID VARCHAR UNIQUE, gzbID VARCHAR, type VARCHAR, company VARCHAR (50), companyOwner VARCHAR, address VARCHAR (250), phone VARCHAR (20), fax VARCHAR, QQ VARCHAR, taxNumber VARCHAR (100), email VARCHAR (50), bankName VARCHAR (100), bankCard VARCHAR (50), PrivateAccount VARCHAR (200), shouldPay VARCHAR DEFAULT (0), shouldReceive VARCHAR DEFAULT (0), beizhu VARCHAR (250), addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: cgdList
                                CREATE TABLE IF NOT EXISTS cgdList (id INTEGER PRIMARY KEY AUTOINCREMENT, cgdID VARCHAR UNIQUE, clientID VARCHAR, companyName VARCHAR, goodsName VARCHAR, jsonData VARCHAR (255), discardFlag INT (2), sum VARCHAR, beizhu VARCHAR (50), fpPu VARCHAR, fpZeng VARCHAR, fpCount VARCHAR, kxQq VARCHAR, kxXq VARCHAR, kxJf VARCHAR, kxSq VARCHAR, kxDay VARCHAR, addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: xsdList
                                CREATE TABLE IF NOT EXISTS xsdList (id INTEGER PRIMARY KEY AUTOINCREMENT, xsdID VARCHAR UNIQUE, clientID VARCHAR, companyName VARCHAR, goodsName VARCHAR, jsonData VARCHAR (255), discardFlag INT (2), sum VARCHAR, beizhu VARCHAR (50), fpPu VARCHAR, fpZeng VARCHAR, fpCount VARCHAR, kxQq VARCHAR, kxXq VARCHAR, kxJf VARCHAR, kxSq VARCHAR, kxDay VARCHAR, addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: htList
                                CREATE TABLE IF NOT EXISTS htList (id INTEGER PRIMARY KEY AUTOINCREMENT, htID VARCHAR UNIQUE, leixing VARCHAR, htLocation VARCHAR, htDate VARCHAR, clientID VARCHAR, companyName VARCHAR, jsonData VARCHAR (255), sum VARCHAR, option VARCHAR, discardFlag INT (2), addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: ccdList
                                CREATE TABLE IF NOT EXISTS ccdList (id INTEGER PRIMARY KEY AUTOINCREMENT, ccdID VARCHAR UNIQUE, clientID VARCHAR, cgxsID VARCHAR, companyName VARCHAR, goodsName VARCHAR, jsonData VARCHAR (255), discardFlag INT (2), sum VARCHAR, beizhu VARCHAR (50), fpPu VARCHAR, fpZeng VARCHAR, fpCount VARCHAR, addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: jcdList
                                CREATE TABLE IF NOT EXISTS jcdList (id INTEGER PRIMARY KEY AUTOINCREMENT, jcdID VARCHAR UNIQUE, clientID VARCHAR, cgxsID VARCHAR, companyName VARCHAR, goodsName VARCHAR, jsonData VARCHAR (255), discardFlag INT (2), sum VARCHAR, beizhu VARCHAR (50), fpPu VARCHAR, fpZeng VARCHAR, fpCount VARCHAR, addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: pzList
                                CREATE TABLE IF NOT EXISTS pzList (id INTEGER PRIMARY KEY AUTOINCREMENT, pzID VARCHAR UNIQUE, leixing VARCHAR, clientID VARCHAR, companyName VARCHAR, zhaiyao VARCHAR (100), jsonData VARCHAR (255), operateMoney VARCHAR, remaintingMoney VARCHAR, beizhu VARCHAR, discardFlag INT (2), addtime DATETIME, modifyTime DATETIME);

                                -- Table: remoteSign
                                CREATE TABLE IF NOT EXISTS remoteSign (id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, fromGZBID VARCHAR (100), toGZBID VARCHAR (100), companyNickName VARCHAR (255), isSigned INT, signValue TEXT (300000), refusedMessage VARCHAR (288), sendTime DATETIME, signTime DATETIME);

                                --COMMIT TRANSACTION;";//建表语句  

                using (SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn))
                {
                    conn.Open();
                    cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建数据表  
                }
            }
        }
        /*
        public void LocalDbOpen()
        {
            // conn.Open();//打开数据库，若文件不存在会自动创建  
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                //RecordLog("无法打开连接!\r\nTargetSite: " + ex.TargetSite + "\r\n" + ex.ToString());
                System.Windows.Forms.MessageBox.Show(ex.Message, "无法打开本地数据库连接！");
                return;
            }
        }

        public void LocalDbClose()
        {
            conn.Close();
        }
        */

        // 插入数据
        public void LocalInsertData(string table, string query, string value)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                using (SQLiteCommand cmdInsert = new SQLiteCommand(conn))
                {
                    cmdInsert.CommandText = "REPLACE INTO  " + table + "  (" + query + ") " +
                                               " VALUES(" + value + ")";
                    cmdInsert.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public int LocalInsertDataReturnAffectRows(string table, string query, string value)
        {
            int returnAffectRows = -1;
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                using (SQLiteCommand cmdInsert = new SQLiteCommand(conn))
                {
                    cmdInsert.CommandText = "INSERT INTO  " + table + "  (" + query + ") " +
                                               " VALUES(" + value + ")";
                    conn.Open();
                    returnAffectRows = cmdInsert.ExecuteNonQuery();
                }
            }
            return returnAffectRows;
        }

        // 删除数据
        public void LocalDeleteDataByID(string table, int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                using (SQLiteCommand cmdInsert = new SQLiteCommand(conn))
                {
                    cmdInsert.CommandText = "DELETE FROM  " + table + "  WHERE id = " + id;
                    conn.Open();
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }

        // 修改数据
        public void LocalUpdateData(string table, string[] query, string[] value, Boolean isValueString, string baseName, string id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                string innerSQL = "";

                for (int i = 0; i < query.Length; i++)
                {
                    if (isValueString)
                    {
                        innerSQL += query[i] + " = '" + value[i] + "',";
                    }
                    else
                    {
                        innerSQL += query[i] + " = " + value[i] + ",";
                    }
                }
                if (!innerSQL.Equals(""))
                {
                    innerSQL = innerSQL.Substring(0, innerSQL.Length - 1); // 去掉最后的逗号
                }

                using (SQLiteCommand cmdInsert = new SQLiteCommand(conn))
                {
                    cmdInsert.CommandText = "UPDATE " + table + " SET " + innerSQL + " WHERE " + baseName + " = '" + id + "'";
                    conn.Open();
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }

        // 插入或更新数据
        public int LocalReplaceIntoDataReturnAffectRows(string table, string[] query, string[] value, string id)
        {
            int returnAffectRows = -1;
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                string innerQuerySQL = "";
                string innerVauleSQL = "";

                //for (int i = 0; i < query.Length; i++)
                //{
                //    innerQuerySQL += query[i] + ",";
                //}
                //if (!innerQuerySQL.Equals(""))
                //{
                //    innerQuerySQL = innerQuerySQL.Substring(0, innerQuerySQL.Length - 1); // 去掉最后的逗号
                //}

                //for (int i = 0; i < value.Length; i++)
                //{
                //    innerVauleSQL += "'" + value[i] + "',";
                //}
                //if (!innerVauleSQL.Equals(""))
                //{
                //    innerVauleSQL = innerVauleSQL.Substring(0, innerVauleSQL.Length - 1); // 去掉最后的逗号
                //}
                innerQuerySQL = String.Join(",", query);
                innerVauleSQL = String.Join("','", value);

                using (SQLiteCommand cmdInsert = new SQLiteCommand(conn))
                {
                    cmdInsert.CommandText = "REPLACE INTO  " + table + "  (" + innerQuerySQL + ") " + " VALUES(" + innerVauleSQL + ")";
                    conn.Open();
                    returnAffectRows = cmdInsert.ExecuteNonQuery();
                }
            }
            return returnAffectRows;
        }

        public void LocalReplaceIntoData(string table, string[] query, string[] value, string id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                string innerQuerySQL = "";
                string innerVauleSQL = "";

                //for (int i = 0; i < query.Length; i++)
                //{
                //    innerQuerySQL += query[i] + ",";
                //}
                //if (!innerQuerySQL.Equals(""))
                //{
                //    innerQuerySQL = innerQuerySQL.Substring(0, innerQuerySQL.Length - 1); // 去掉最后的逗号
                //}

                //for (int i = 0; i < value.Length; i++)
                //{
                //    innerVauleSQL += "'" + value[i] + "',";
                //}
                //if (!innerVauleSQL.Equals(""))
                //{
                //    innerVauleSQL = innerVauleSQL.Substring(0, innerVauleSQL.Length - 1); // 去掉最后的逗号
                //}
                innerQuerySQL = String.Join(",", query);
                innerVauleSQL = String.Join("','", value);
                //SQLiteTransaction transaction = conn.BeginTransaction();
                using (SQLiteCommand cmdInsert = new SQLiteCommand(conn))
                {
                    cmdInsert.CommandText = "REPLACE INTO  " + table + "  (" + innerQuerySQL + ") " + " VALUES('" + innerVauleSQL + "')";
                    conn.Open();
                    cmdInsert.ExecuteNonQuery();
                    //transaction.Commit();
                    conn.Close();
                }
            }
        }

        // 清空表
        public void LocalClearTable(String table)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                using (SQLiteCommand cmdInsert = new SQLiteCommand(conn))
                {
                    cmdInsert.CommandText = "DELETE FROM " + table;
                    conn.Open();
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }

        public int LocalGetCountOfTable(String table, String condition)
        {
            int resultCount = -1;
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                if (!condition.Equals(""))
                {
                    condition = " WHERE " + condition;
                }

                try
                {
                    using (SQLiteCommand cmdInsert = new SQLiteCommand(conn))
                    {
                        cmdInsert.CommandText = "SELECT COUNT(*) itemCount FROM " + table;
                        conn.Open();
                        SQLiteDataReader reader = cmdInsert.ExecuteReader();
                        while (reader.Read())
                        {
                            resultCount = int.Parse(reader[0].ToString());
                        }
                        //resultCount = cmdInsert.ExecuteNonQuery();
                    }
                }
                catch { conn.Close(); }
            }
            return resultCount;
        }

        public string[] LocalGetOneRowDataById(string table, string[] query, string baseName, string id)
        {
            string[] resultsStringArray = new String[] { };
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                try
                {
                    // ORDER BY id ASC
                    string innerSQL = "";

                    for (int i = 0; i < query.Length; i++)
                    {
                        innerSQL += query[i] + ",";
                    }
                    if (!innerSQL.Equals(""))
                    {
                        innerSQL = innerSQL.Substring(0, innerSQL.Length - 1); // 去掉最后的逗号
                    }
                    string sql = "SELECT " + innerSQL + " FROM " + table + " WHERE " + baseName + "='" + id + "'";//建表语句  
                    using (SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn))
                    {
                        cmdCreateTable.CommandText = sql;
                        conn.Open();
                        System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
                        resultsStringArray = new string[query.Length];

                        while (reader.Read())
                        {
                            for (int i = 0; i < query.Length; i++)
                            {
                                resultsStringArray[i] = reader[query[i]].ToString();
                            }
                        }
                    }
                }
                catch { conn.Close(); return resultsStringArray; }
            }
            return resultsStringArray;
        }

        // 列出数据
        public List<String[]> LocalGetData(String table, String[] query, String order)
        {
            List<String[]> resultsStringList;
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                // ORDER BY id ASC
                String innerSQL = "";

                for (int i = 0; i < query.Length; i++)
                {
                    innerSQL += query[i] + ",";
                }
                if (!innerSQL.Equals(""))
                {
                    innerSQL = innerSQL.Substring(0, innerSQL.Length - 1); // 去掉最后的逗号
                }
                String sql = "SELECT " + innerSQL + " FROM " + table + " " + order;//建表语句  
                using (SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn))
                {
                    cmdCreateTable.CommandText = sql;
                    conn.Open();
                    System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
                    String[] resultsStringArray = new String[query.Length];
                    resultsStringList = new List<String[]>();

                    while (reader.Read())
                    {
                        for (int i = 0; i < query.Length; i++)
                        {
                            resultsStringArray[i] = reader[i].ToString();
                        }
                        resultsStringList.Add(resultsStringArray);
                        resultsStringArray = new String[query.Length];
                    }
                    reader.Close();
                }
            }
            return resultsStringList;
        }

        // 最原始的列出数据
        public List<String[]> LocalGetDataFromOriginalSQL(String sql, String[] query)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                using (SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn))
                {
                    cmdCreateTable.CommandText = sql;
                    conn.Open();
                    System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
                    String[] resultsStringArray = new String[query.Length];
                    List<String[]> resultsStringList = new List<String[]>();

                    while (reader.Read())
                    {
                        for (int i = 0; i < query.Length; i++)
                        {
                            resultsStringArray[i] = reader[i].ToString();
                        }
                        resultsStringList.Add(resultsStringArray);
                        resultsStringArray = new String[query.Length];
                    }
                    reader.Close();
                    return resultsStringList;
                }
            }
        }

        // 获取表的id
        public List<string> LocalGetIdsOfTable(string table, string baseName, string order)
        {
            List<string> resultsStringList;
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                string sql = "SELECT " + baseName + " FROM " + table + " " + order;//建表语句  
                using (SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn))
                {
                    cmdCreateTable.CommandText = sql;
                    conn.Open();
                    System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
                    resultsStringList = new List<string>();

                    while (reader.Read())
                    {
                        resultsStringList.Add(reader.GetString(0));
                    }
                    reader.Close();
                }
            }
            return resultsStringList;
        }

        // 检测是否重名
        public Boolean LocalCheckIfDuplicate(string table, string baseName, string id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                string sql = "SELECT " + baseName + " FROM " + table + " WHERE " + baseName + "='" + id + "'";//建表语句  
                using (SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn))
                {
                    cmdCreateTable.CommandText = sql;
                    conn.Open();
                    System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        // 自动生成ID
        public String LocalAutoincreaseID(string table, string baseName)
        {
            using (SQLiteConnection conn = new SQLiteConnection(LocalConnStr))
            {
                String maxNumber = "";
                // SELECT max(jcdID) as max FROM jcdList 
                // cast(yysid as UNSIGNED INTEGER)
                string sql = "SELECT max(cast(" + baseName + " as UNSIGNED INTEGER)) as max FROM " + table;//建表语句  
                using (SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn))
                {
                    cmdCreateTable.CommandText = sql;
                    conn.Open();
                    System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();

                    while (reader.Read())
                    {
                        maxNumber = (int.Parse(reader["max"].ToString().Equals("") ? "0" : reader["max"].ToString()) + 1).ToString();
                    }

                    return FormBasicFeatrues.GetInstence().FormatID(maxNumber, 6, "0");
                }
            }
        }
        #endregion
    }
}
