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
        private const string onlineSqlConnectionCommand = @"server=qdm-011.hichina.com; user id=qdm0110106; password=CYYDB2014; database=qdm0110106_db";
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
            MD5 md5Hash = MD5.Create();
            string hash = FormBasicFeatrues.GetInstence().GetMd5Hash(md5Hash, psw);

            StringBuilder sbSQL = new StringBuilder(
                    @"SELECT Count(loginId), loginId, name, nickName, notification, lastLogonTime
                    FROM caiyyUser WHERE loginId = '");
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
                MainWindow.IS_LOGED_IN = (int.Parse((dataReader["Count(loginId)"].ToString() == "") ? "0" : dataReader["Count(loginId)"].ToString()) == 1) ? true : false;
                MainWindow.LOGIN_ID = dataReader["loginId"].ToString();
                MainWindow.NAME = dataReader["name"].ToString();
                MainWindow.NICK_NAME = dataReader["nickName"].ToString();
                MainWindow.NOTIFICATION = dataReader["notification"].ToString();
                MainWindow.LAST_LOGON_TIME = dataReader["lastLogonTime"].ToString().Equals("") ? "首次登录" : dataReader["lastLogonTime"].ToString();
            }

            dataReader.Close();
            OnlineDbClose();
        }
        #endregion

        #region 本地
        private SQLiteConnection localSqlConnectionCommand = new SQLiteConnection("Data Source =" + Environment.CurrentDirectory + "/data/data.db");

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
            /*
            string sql = "CREATE TABLE IF NOT EXISTS clients(id integer PRIMARY KEY NOT NULL, name varchar(20), sex varchar(2));";//建表语句  
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建数据表  
             */
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
        public void LocalUpdateData(string table, string[] query, string[] value, string id)
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
            LocalDbOpen();
            SQLiteCommand cmdInsert = new SQLiteCommand(localSqlConnectionCommand);
            cmdInsert.CommandText = "UPDATE " + table + " SET " + innerSQL + " WHERE id = '" + id + "'";
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
            string sql = "SELECT " + innerSQL + " FROM " + table + " WHERE "+baseName+"='" + id + "'";//建表语句  
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
        public List<string[]> LocalGetData(string table, string[] query, string order)
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
            string sql = "SELECT " + innerSQL + " FROM " + table + " " + order;//建表语句  
            LocalDbOpen();
            SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, localSqlConnectionCommand);
            cmdCreateTable.CommandText = sql;
            System.Data.SQLite.SQLiteDataReader reader = cmdCreateTable.ExecuteReader();
            string[] resultsStringArray = new string[query.Length];
            List<string[]> resultsStringList = new List<string[]>();

            while (reader.Read())
            {
                for (int i = 0; i < query.Length; i++)
                {
                    resultsStringArray[i] = reader[query[i]].ToString();
                }
                resultsStringList.Add(resultsStringArray);
                resultsStringArray = new string[query.Length];
            }
            reader.Close();
            LocalDbClose();
            return resultsStringList;
        }

        #endregion

    }
}
