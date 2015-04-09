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
        private static DatabaseConnections _databaseConnections = null;

        public static DatabaseConnections GetInstence()
        {
            if (_databaseConnections == null)
            {
                _databaseConnections = new DatabaseConnections();
            }
            return _databaseConnections;
        }

        #region 联网
        // MySQL
        private const string onlineSqlConnectionCommand = @"server=121.42.154.95; user id=vivid; password=vivid; database=vivid;Charset=utf8";
        private MySqlConnection onlineSqlConnection = new MySqlConnection(onlineSqlConnectionCommand);

        public void OnlineDbOpen()
        {
            try
            {
                if (onlineSqlConnection.State != ConnectionState.Open)
                {
                    onlineSqlConnection.Open();
                }
            }
            catch (Exception ex)
            {
                //RecordLog("无法打开连接!\r\nTargetSite: " + ex.TargetSite + "\r\n" + ex.ToString());
                System.Windows.Forms.MessageBox.Show(ex.Message, "无法打开数据库连接！");
                return;
            }
        }

        public void OnlineDbClose()
        {
            onlineSqlConnection.Close();
        }

        public void UserLogin(string acc, string psw)
        {
            string hash = FormBasicFeatrues.GetInstence().GetMd5Hash(MD5.Create(), psw);

            StringBuilder sbSQL = new StringBuilder(
                    @"SELECT Count(id),id,userid,password,realname,workloads,company,companyowner,address,bankname,bankcard,phone,fax,QQ,email,cast(addtime as char) as addtime,VMA_expiretime,notification,companyBalance FROM users WHERE userid = '");
            sbSQL.Append(acc);
            sbSQL.Append(@"'");
            sbSQL.Append(@" AND password = '");
            sbSQL.Append(hash.ToLower());
            sbSQL.Append(@"'");

            string SQLforGeneral = sbSQL.ToString();

            MySqlCommand cmd = new MySqlCommand(SQLforGeneral, onlineSqlConnection);
            OnlineDbOpen();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                MainWindow.IS_PASSWORD_CORRECT = (int.Parse((dataReader["Count(id)"].ToString() == "") ? "0" : dataReader["Count(id)"].ToString()) == 1) ? true : false;
                if (MainWindow.IS_PASSWORD_CORRECT)
                {
                    MainWindow.ID = dataReader["id"].ToString();
                    MainWindow.USER_ID = dataReader["userid"].ToString();
                    MainWindow.PASSWORD_HASH = dataReader["password"].ToString();
                    MainWindow.REAL_NAME = dataReader["realname"].ToString();
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
                    MainWindow.ADDTIME = dataReader["addtime"].ToString();
                    MainWindow.NOTIFICATION = dataReader["notification"].ToString();
                    MainWindow.EXPIRETIME = DateTime.Parse(dataReader["VMA_expiretime"].ToString());
                    MainWindow.COMPANY_BALANCE = int.Parse(dataReader["companyBalance"].ToString());
                    //MainWindow.LAST_LOGON_TIME = dataReader["lastLogonTime"].ToString().Equals("") ? "首次登录" : dataReader["lastLogonTime"].ToString();
                }
            }

            dataReader.Close();
            OnlineDbClose();
        }

        // 修改数据
        public void OnlineUpdateData(string table, string[] query, string[] value, string id)
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
            OnlineDbOpen();
            string SQLforGeneral = "UPDATE " + table + " SET " + innerSQL + " WHERE id = '" + id + "'";
            MySqlCommand cmdInsert = new MySqlCommand(SQLforGeneral, onlineSqlConnection);
            cmdInsert.ExecuteNonQuery();
            OnlineDbClose();
        }

        // 修改原始数据
        public void OnlineUpdateDataFromOriginalSQL(String sql)
        {
            OnlineDbOpen();
            MySqlCommand cmdInsert = new MySqlCommand(sql, onlineSqlConnection);
            cmdInsert.ExecuteNonQuery();
            OnlineDbClose();
        }

        #endregion

        #region 本地
        private SQLiteConnection localSqlConnectionCommand = new SQLiteConnection("Data Source =" +MainWindow.LOCAL_DATABASE_LOCATION);

        public void LocalCreateDatabase()
        {
            LocalDbOpen();
            string sql = @" --
                                -- File generated with SQLiteStudio v3.0.3 on 周四 4月 9 14:58:30 2015
                                --
                                -- Text encoding used: GBK
                                --
                                --PRAGMA foreign_keys = off;
                                --BEGIN TRANSACTION;

                                -- Table: goods
                                CREATE TABLE IF NOT EXISTS goods (id INTEGER PRIMARY KEY AUTOINCREMENT, goodId VARCHAR UNIQUE, name VARCHAR (250), guige VARCHAR (250), unit VARCHAR (50), dengji VARCHAR (50), storageName VARCHAR (50), storageManager VARCHAR (100), storageManagerPhone VARCHAR (50), storageLocation VARCHAR (50), storageAddress VARCHAR (250), initalCount VARCHAR DEFAULT (0), purchasePrice VARCHAR (50), purchaseTotal VARCHAR (100), currentCount VARCHAR (100), currentTotal VARCHAR (150), currntsalesPrice VARCHAR (100), beizhu VARCHAR (250), addtime VARCHAR (50), modifyDate VARCHAR (50));

                                -- Table: clients
                                CREATE TABLE IF NOT EXISTS clients (id INTEGER PRIMARY KEY AUTOINCREMENT, clientID VARCHAR UNIQUE, contact VARCHAR (50), sex VARCHAR, type VARCHAR, company VARCHAR (20), companyOwner VARCHAR, address VARCHAR (250), phone VARCHAR (250), fax VARCHAR, taxNumber VARCHAR (100), email VARCHAR (250), bankInfo VARCHAR (250), otherContacts VARCHAR (250), PrimaryAccount VARCHAR (250), beizhu VARCHAR (250), addtime VARCHAR (50), modifyDate VARCHAR (50));

                                -- Table: cgdList
                                CREATE TABLE IF NOT EXISTS cgdList (id INTEGER PRIMARY KEY AUTOINCREMENT, cgdID VARCHAR UNIQUE, clientID VARCHAR, companyName VARCHAR, goodsName VARCHAR, jsonData VARCHAR (255), discardFlag INT (2), sum VARCHAR, beizhu VARCHAR (50), fpPu VARCHAR, fpZeng VARCHAR, fpCount VARCHAR, kxQq VARCHAR, kxXq VARCHAR, kxJf VARCHAR, kxSq VARCHAR, kxDay VARCHAR, addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: xsdList
                                CREATE TABLE IF NOT EXISTS xsdList (id INTEGER PRIMARY KEY AUTOINCREMENT, xsdID VARCHAR UNIQUE, clientID VARCHAR, companyName VARCHAR, goodsName VARCHAR, jsonData VARCHAR (255), discardFlag INT (2), sum VARCHAR, beizhu VARCHAR (50), fpPu VARCHAR, fpZeng VARCHAR, fpCount VARCHAR, kxQq VARCHAR, kxXq VARCHAR, kxJf VARCHAR, kxSq VARCHAR, kxDay VARCHAR, addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: htList
                                CREATE TABLE IF NOT EXISTS htList (id INTEGER PRIMARY KEY AUTOINCREMENT, htID VARCHAR UNIQUE, leixing VARCHAR, htDate VARCHAR, clientID VARCHAR, companyName VARCHAR, jsonData VARCHAR (255), sum VARCHAR, option VARCHAR, discardFlag INT (2), addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: ccdList
                                CREATE TABLE IF NOT EXISTS ccdList (id INTEGER PRIMARY KEY AUTOINCREMENT, ccdID VARCHAR UNIQUE, clientID VARCHAR, cgxsID VARCHAR, companyName VARCHAR, goodsName VARCHAR, jsonData VARCHAR (255), discardFlag INT (2), sum VARCHAR, beizhu VARCHAR (50), fpPu VARCHAR, fpZeng VARCHAR, fpCount VARCHAR, addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: jcdList
                                CREATE TABLE IF NOT EXISTS jcdList (id INTEGER PRIMARY KEY AUTOINCREMENT, jcdID VARCHAR UNIQUE, clientID VARCHAR, cgxsID VARCHAR, companyName VARCHAR, goodsName VARCHAR, jsonData VARCHAR (255), discardFlag INT (2), sum VARCHAR, beizhu VARCHAR (50), fpPu VARCHAR, fpZeng VARCHAR, fpCount VARCHAR, addtime VARCHAR, modifyTime VARCHAR);

                                -- Table: pzList
                                CREATE TABLE IF NOT EXISTS pzList (id INTEGER PRIMARY KEY AUTOINCREMENT, pzID VARCHAR UNIQUE, leixing VARCHAR, clientID VARCHAR, companyName VARCHAR, zhaiyao VARCHAR (100), jsonData VARCHAR (255), operateMoney VARCHAR, remaintingMoney VARCHAR, beizhu VARCHAR, discardFlag INT (2), addtime DATETIME, modifyTime DATETIME);

                                -- Table: config
                                CREATE TABLE IF NOT EXISTS config (id INTEGER PRIMARY KEY AUTOINCREMENT, configKey VARCHAR, configValue VARCHAR);

                                --COMMIT TRANSACTION;";//建表语句  
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建数据表  
            LocalDbClose();
        }

        public void LocalDbOpen()
        {
            // localSqlConnectionCommand.Open();//打开数据库，若文件不存在会自动创建  
            try
            {
                if (localSqlConnectionCommand.State != ConnectionState.Open)
                {
                    localSqlConnectionCommand.Open();
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
            localSqlConnectionCommand.Close();
        }

        // 插入数据
        public void LocalInsertData(string table, string query, string value)
        {
            LocalDbOpen();
            SQLiteCommand cmdInsert = new SQLiteCommand(localSqlConnectionCommand);
            cmdInsert.CommandText = "REPLACE INTO  " + table + "  (" + query + ") " +
                                       " VALUES(" + value + ")";
            cmdInsert.ExecuteNonQuery();
            LocalDbClose();
        }

        // 删除数据
        public void LocalDeleteDataByID(string table, int id)
        {
            LocalDbOpen();
            SQLiteCommand cmdInsert = new SQLiteCommand(localSqlConnectionCommand);
            cmdInsert.CommandText = "DELETE FROM  " + table + "  WHERE id = " + id;
            cmdInsert.ExecuteNonQuery();
            LocalDbClose();
        }

        // 修改数据
        public void LocalUpdateData(string table, string[] query, string[] value, Boolean isValueString, string baseName, string id)
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
            LocalDbOpen();
            SQLiteCommand cmdInsert = new SQLiteCommand(localSqlConnectionCommand);
            cmdInsert.CommandText = "UPDATE " + table + " SET " + innerSQL + " WHERE " + baseName + " = '" + id + "'";
            cmdInsert.ExecuteNonQuery();
            LocalDbClose();
        }

        // 插入或更新数据
        public void LocalReplaceIntoData(string table, string[] query, string[] value, string id)
        {
            string innerQuerySQL = "";

            for (int i = 0; i < query.Length; i++)
            {
                innerQuerySQL += query[i] + ",";
            }
            if (!innerQuerySQL.Equals(""))
            {
                innerQuerySQL = innerQuerySQL.Substring(0, innerQuerySQL.Length - 1); // 去掉最后的逗号
            }

            string innerVauleSQL = "";
            for (int i = 0; i < value.Length; i++)
            {
                innerVauleSQL += "'" + value[i] + "',";
            }
            if (!innerVauleSQL.Equals(""))
            {
                innerVauleSQL = innerVauleSQL.Substring(0, innerVauleSQL.Length - 1); // 去掉最后的逗号
            }

            LocalDbOpen();
            SQLiteCommand cmdInsert = new SQLiteCommand(localSqlConnectionCommand);
            cmdInsert.CommandText = "REPLACE INTO  " + table + "  (" + innerQuerySQL + ") " +
                                       " VALUES(" + innerVauleSQL + ")";
            cmdInsert.ExecuteNonQuery();
            LocalDbClose();
        }


        public string[] LocalGetOneRowDataById(string table, string[] query, string baseName, string id)
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
            LocalDbOpen();
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.CommandText = sql;
            System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
            string[] resultsStringArray = new string[query.Length];

            while (reader.Read())
            {
                for (int i = 0; i < query.Length; i++)
                {
                    resultsStringArray[i] = reader[query[i]].ToString();
                }
            }
            reader.Close();
            LocalDbClose();
            return resultsStringArray;
        }

        // 列出数据
        public List<String[]> LocalGetData(String table, String[] query, String order)
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
            LocalDbOpen();
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.CommandText = sql;
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
            LocalDbClose();
            return resultsStringList;
        }

        // 最原始的列出数据
        public List<String[]> LocalGetDataFromOriginalSQL(String sql, String[] query)
        {
            LocalDbOpen();
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.CommandText = sql;
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
            LocalDbClose();
            return resultsStringList;
        }

        // 获取表的id
        public List<string> LocalGetIdsOfTable(string table, string baseName, string order)
        {
            string sql = "SELECT " + baseName + " FROM " + table + " " + order;//建表语句  
            LocalDbOpen();
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.CommandText = sql;
            System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
            List<string> resultsStringList = new List<string>();

            while (reader.Read())
            {
                resultsStringList.Add(reader.GetString(0));
            }
            reader.Close();
            LocalDbClose();
            return resultsStringList;
        }

        // 检测是否重名
        public Boolean LocalCheckIfDuplicate(string table, string baseName, string id)
        {
            string sql = "SELECT " + baseName + " FROM " + table + " WHERE " + baseName + "='" + id + "'";//建表语句  
            LocalDbOpen();
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.CommandText = sql;
            System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
            LocalDbClose();
            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 自动生成ID
        public String LocalAutoincreaseID(string table, string baseName)
        {
            String maxNumber = "";
            // SELECT max(jcdID) as max FROM jcdList 
            // cast(yysid as UNSIGNED INTEGER)
            string sql = "SELECT max(cast(" + baseName + " as UNSIGNED INTEGER)) as max FROM " + table;//建表语句  
            LocalDbOpen();
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.CommandText = sql;
            System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();

            while (reader.Read())
            {
                maxNumber = (int.Parse(reader["max"].ToString().Equals("") ? "0" : reader["max"].ToString()) + 1).ToString();
            }

            LocalDbClose();
            return FormBasicFeatrues.GetInstence().FormatID(maxNumber, 10, "0");
        }
        #endregion

    }
}
