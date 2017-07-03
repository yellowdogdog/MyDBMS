using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    
    [Serializable]
    class TableF
    {
        public const string FILE_PATH = "myDB.dbf";
        /// <summary>
        /// 表列表
        /// </summary>
        public List<Table> tables=new List<Table>();
        /// <summary>
        /// 新建表
        /// </summary>
        /// <param name="table">需要新建的表</param>
        public void addTable(Table table)
        {
            if (isTableNameExist(table.TableName)!=-1)
            {
                throw new TableEditException("存在同名表"+table.TableName);
            }
            tables.Add(table);
            saveFile();
            DataF.getDataF().addTableData();
        }
        public void deleteTable(string tableName)
        {
            int i = isTableNameExist(tableName);
            if (i == -1)
            {
                throw new TableEditException("表不存在"+tableName); 
            }
            tables.RemoveAt(i);
            saveFile();
            DataF.getDataF().deleteTableData(i);
        }
        private void saveFile()
        {
            Stream stream = new FileStream(FILE_PATH, FileMode.Create, FileAccess.Write, FileShare.None);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream,this);
            stream.Close();
        }
        static TableF()
        {
            if (!File.Exists(FILE_PATH))
            {
                TableF tableF = new TableF();
                tableF.saveFile();
            }
        }
        public static TableF getTableF()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read);
            TableF tableF = (TableF)formatter.Deserialize(stream);
            stream.Close();
            return tableF;
        }

        /// <summary>
        /// 查询是否存在同名表
        /// </summary>
        /// <param name="name">表名</param>
        /// <returns>若存在返回表序号，不存在返回-1</returns>
        public int isTableNameExist(string name)
        {
            for(int i = 0; i < tables.Count; i++)
            {
                if (name == tables[i].TableName)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
