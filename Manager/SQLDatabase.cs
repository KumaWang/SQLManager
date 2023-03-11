using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace Manager
{
    /// <summary>
    /// 异步数据库服务器
    /// </summary>
    public class SQLDatabase : IDisposable
    {
        private string          m_databaseName;
        private string          m_databaseAccount;
        private string          m_databasePassword;
        private string          m_databaseHost;
        private int             m_databasePort;
        private MySqlConnection m_sqlConnection;
        private Process         m_mysql_process;

        //private Process m_mysql_process;
        
        /// <summary>
        /// 获取异步数据库当前状态
        /// </summary>
        public ConnectionState  State 
        {
            get;
            private set;
        }

        public SQLDatabase(
            string databaseName, 
            string databaseAccount, 
            string databasePassword, 
            string databaseHost = "localhost", 
            int databasePort=3306, 
            string mysqlExeFileName = null)                                                                                                                 
        {
            // 启动mysql-nt
            if (!string.IsNullOrEmpty(mysqlExeFileName) && Process.GetProcessesByName("mysqld.exe").Length == 0)
            {
                //var mysqlExeFileName = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)), "lib", "mysql", "bin", "mysqld-nt.exe");
                m_mysql_process = new Process();
                m_mysql_process.StartInfo = new ProcessStartInfo()
                {
                    FileName = mysqlExeFileName,
                    WorkingDirectory = Path.GetDirectoryName(mysqlExeFileName),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                };

                m_mysql_process.Start();

                int num = 0;
                while (!PortInUse(databasePort))
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                    num++;

                    if (num > 100)
                        break;
                }
         
                if(!PortInUse(databasePort)) throw new NotImplementedException("mysql端口未打开");
            }

            State = ConnectionState.Closed;

            m_databaseName = databaseName;
            m_databaseAccount = databaseAccount;
            m_databasePassword = databasePassword;
            m_databaseHost = databaseHost;
            m_databasePort = databasePort;

            m_sqlConnection = new MySqlConnection(
                string.Format("host={0};port={1};user={2};pwd={3}",
                                    m_databaseHost,
                                    m_databasePort,
                                    //m_databaseName, 
                                    m_databaseAccount,
                                    m_databasePassword));

            m_sqlConnection.StateChange += (s, e) =>
            {
                State = e.CurrentState;
            };

            m_sqlConnection.Open();

            while (State != ConnectionState.Open)
            {
                Thread.Sleep(100);
                Application.DoEvents();
            }

            // 首先检查数据库是否存在
            CheckDatabase();
        }

        private bool PortInUse(int port)                                                                                                                    
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        /// <summary>
        /// 检查数据库
        /// </summary>
        public void CheckDatabase()                                                                                                                         
        {
            const string CMD_TEMPLATE = @"CREATE DATABASE IF NOT EXISTS `{0}`;";
            var cmd = string.Format(CMD_TEMPLATE, m_databaseName);
            MySqlHelper.ExecuteNonQuery(m_sqlConnection, cmd);

            m_sqlConnection.ChangeDatabase(m_databaseName);
        }

        /// <summary>
        /// 检查数据表是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsTableExists(string tableName)                                                                                                         
        {
            const string CMD_TEMPLATE = @"SELECT EXISTS(SELECT `TABLE_NAME` FROM `INFORMATION_SCHEMA`.`TABLES` WHERE (`TABLE_NAME` = '{0}') AND (`TABLE_SCHEMA` = '{1}')) as `is-exists`";

            var cmd = string.Format(CMD_TEMPLATE, tableName, m_databaseName);
            var reader = MySqlHelper.ExecuteReader(m_sqlConnection, cmd);
            if (reader.Read())
            {
                var value = reader.GetBoolean(0);
                reader.Close();
                return value;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 检查数据列是否存在
        /// </summary>
        public bool IsColumnExists(string tableName, string columnName)                                                                                     
        {
            const string CMD_TEMPLATE = @"SELECT EXISTS(SELECT `TABLE_NAME` FROM `INFORMATION_SCHEMA`.`TABLES` WHERE (`COLUMN_NAME` = '{0}') AND (`TABLE_NAME` = '{1}') AND (`TABLE_SCHEMA` = '{2}')) as `is-exists`";
            var cmd = string.Format(CMD_TEMPLATE, columnName, tableName, m_databaseName);
            var reader = MySqlHelper.ExecuteReader(m_sqlConnection, cmd);
            if (reader.Read())
            {
                var value = reader.GetBoolean(0);
                reader.Close();
                return value;
            }
            else 
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 获取所有列名
        /// </summary>
        public Dictionary<string, (string Type, string Comment)> GetColumnInfos(string tableName)                                                                                  
        {
            const string CMD_TEMPLATE = @"SELECT `COLUMN_NAME`,`DATA_TYPE`,`column_comment` FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE (`TABLE_NAME` = '{0}') AND (`TABLE_SCHEMA` = '{1}')";

            Dictionary<string, (string,string)> infos = new Dictionary<string, (string, string)>();
            var cmd = string.Format(CMD_TEMPLATE, tableName, m_databaseName);
            var reader = MySqlHelper.ExecuteReader(m_sqlConnection, cmd);

            while (reader.Read()) 
            {
                infos[reader.GetString(0)] = (reader.GetString(1), reader.GetString(2));
            }

            reader.Close();

            return infos;
        }

        /// <summary>
        /// 获取表数据读取器
        /// </summary>
        public IDataReader GetTableReader(string tableName) 
        {
            const string CMD_TEMPLATE = @"SELECT * FROM `{0}`";
            var cmd = string.Format(CMD_TEMPLATE, tableName);
            return MySqlHelper.ExecuteReader(m_sqlConnection, cmd);
        }

        /// <summary>
        /// 获取table结构
        /// </summary>
        public SQLTable GetTable(string tableName) 
        {
            var fields = GetColumnInfos(tableName).Select(x => new SQLField(x.Key, x.Value.Comment, SQLFieldType.Parse(x.Value.Type))).ToArray();

            return new SQLTable(this, tableName, fields);
        }

        /// <summary>
        /// 获取所有表名
        /// </summary>
        /// <returns></returns>
        public IList<string> GetTableNames() 
        {
            const string CMD_TEMPLATE = @"SELECT table_name FROM information_schema.tables WHERE table_schema = '{0}'";

            List<string> infos = new List<string>();
            var cmd = string.Format(CMD_TEMPLATE, m_databaseName);
            var reader = MySqlHelper.ExecuteReader(m_sqlConnection, cmd);

            while (reader.Read())
            {
                infos.Add(reader.GetString(0));
            }

            reader.Close();
            return infos;
        }

        /// <summary>
        /// 获取所有数据表结构
        /// </summary>
        /// <returns></returns>
        public IList<SQLTable> GetTables() 
        {
            return GetTableNames().Select(x => GetTable(x)).ToArray();
        }

        /// <summary>
        /// 创建表
        /// </summary>
        public void CheckTable(string tableName, Dictionary<string, SQLFieldType> newList)                                                                  
        {
            var isTableExists = IsTableExists(tableName);
            // 如果表不存在则创建表
            if (!isTableExists)
            {
                const string CMD_TEMPLATE = @"CREATE TABLE IF NOT EXISTS `{0}`({1}) ENGINE = MyISAM;";
                // 检查table是否存在,不存在则创建
                var cmd = string.Format(CMD_TEMPLATE, tableName, string.Join(",", newList.Select(X => string.Format("`{0}` {1}", X.Key, X.Value.TypeName + (X.Value.PrimaryKey ? " PRIMARY KEY" : String.Empty) + (X.Value.AutoIncrement ? " NOT NULL AUTO_INCREMENT" : String.Empty))).ToArray())); // tableStructSw.ToString().TrimEnd(','));
                MySqlHelper.ExecuteNonQuery(m_sqlConnection, cmd);
            }
            else if(false)
            {
                var oldList = GetColumnInfos(tableName);
                // 新列表中不存在的旧列
                var removedColumns = oldList.Where(X => !newList.ContainsKey(X.Key)).ToArray();

                // 新列表中存在的旧列却属性不同
                var propertyDiffection = newList.Where(X => oldList.ContainsKey(X.Key) && oldList[X.Key].Type.ToLower() != X.Value.TypeName.ToLower()).ToArray();

                // 旧列表中不存在的新列
                var newColumns = newList.Where(X => !oldList.ContainsKey(X.Key)).ToArray();

                // 首先移除不存在的列
                foreach (var column in removedColumns)
                    RemoveColumn(tableName, column.Key);

                // 在添加新列
                foreach (var column in newColumns)
                    AddColumn(tableName, column.Key, column.Value);

                // 在修改列属性
                foreach (var column in propertyDiffection)
                    ModifyColumn(tableName, column.Key, column.Value);
            }
        }

        /// <summary>
        /// 移除一列数据
        /// </summary>
        public void RemoveColumn(string tableName, string columnName)                                                                                       
        {
            const string CMD_TEMPLATE = @"ALTER TABLE `{0}` DROP COLUMN `{1}`;";
            var cmd = string.Format(CMD_TEMPLATE, tableName, columnName);
            MySqlHelper.ExecuteNonQuery(m_sqlConnection, cmd);
        }

        /// <summary>
        /// 添加一列数据
        /// </summary>
        public void AddColumn(string tableName, string columnName, SQLFieldType columnDataType)                                                             
        {
            const string CMD_TEMPLATE = @"ALTER TABLE `{0}` ADD COLUMN `{1}` {2}{3}{4};";
            var cmd = string.Format(CMD_TEMPLATE, tableName, columnName, columnDataType.TypeName, columnDataType.PrimaryKey ? " PRIMARY KEY" : String.Empty, columnDataType.AutoIncrement ? " NOT NULL AUTO_INCREMENT" : String.Empty);
            MySqlHelper.ExecuteNonQuery(m_sqlConnection, cmd);
        }

        /// <summary>
        /// 修改一些数据属性
        /// </summary>
        public void ModifyColumn(string tableName, string columnName, SQLFieldType newDataType)                                                             
        {
            const string CMD_TEMPLATE = @"ALTER TABLE `{0}` MODIFY COLUMN `{1}` {2}{3}{4};";
            var cmd = string.Format(CMD_TEMPLATE, tableName, columnName, newDataType.TypeName, newDataType.PrimaryKey ? " PRIMARY KEY" : String.Empty, newDataType.AutoIncrement ? " NOT NULL AUTO_INCREMENT" : String.Empty);
            MySqlHelper.ExecuteNonQuery(m_sqlConnection, cmd);
        }

        /// <summary>
        /// 执行一条sql命令
        /// </summary>
        public void ExecuteCommand(string commandText)                                                                                                      
        {
            MySqlHelper.ExecuteScalar(m_sqlConnection, commandText);
        }

        public MySqlDataReader RunCommand(string commandText)                                                                                               
        {
            return MySqlHelper.ExecuteReader(m_sqlConnection, commandText);
        }

        public void Dispose()                                                                                                                               
        {
            m_mysql_process.Kill();
        }

    }
}
