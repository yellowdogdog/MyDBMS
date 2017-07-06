using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    [Serializable]
    class DataF
    {
        public const string FILE_PATH = "myDBData.dat";
        /// <summary>
        /// 表数据的集合
        /// </summary>
        public List<Data> datas=new List<Data>();
        [NonSerialized]
        protected TableF tableF;
        /// <summary>
        /// 查询数据操作（单个数据表的简单查询）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">投影的字段名:如果为null表示查询所有列</param>
        /// <param name="condition">查询条件</param>
        /// <returns>数据表DataTable</returns>
        public DataTable selectData(string tableName,string[] fieldName,Condition condition)
        {
            return selectData(new string[] { tableName },fieldName,new int[] { 0 },condition);
        }
        /// <summary>
        /// 查询数据操作（多个数据表的复杂查询）
        /// </summary>
        /// <param name="tableNames">表名</param>
        /// <param name="fieldName">投影的字段名:如果为null表示查询所有列</param>
        /// <param name="tableIndex">表序号：当fieldName为空时无效。若为省略的唯一列，传入-1</param>
        /// <param name="condition">查询条件,若为null表示没有条件查询全部</param>
        /// <returns>数据表DataTable</returns>
        public DataTable selectData(string[] tableNames, string[] fieldNames,int[] tableIndex, Condition condition)
        {
            int fromTableCount = tableNames.Length;//查询的表数
            int[] fromTable = new int[fromTableCount];//查询表的索引
            int selectCount = 0;//查询字段数
            int[] fieldIndex;//查询字段索引
            #region 列名转化为内部索引
            for (int i = 0; i < fromTableCount; i++)
            {
                fromTable[i] = tableF.isTableNameExist(tableNames[i]);
                if (fromTable[i] == -1)
                {
                    throw new DataEditException("表不存在："+tableNames[i]);
                }
            }             
            if (fieldNames == null)
            {//所有列
                for(int i = 0; i < fromTableCount; i++)
                {
                    selectCount += tableF.tables[fromTable[i]].fields.Count;
                }
                fieldIndex = new int[selectCount];
                tableIndex = new int[selectCount];
                fieldNames = new string[selectCount];
                int p = 0;
                for(int i = 0; i < fromTableCount; i++)
                {
                    for(int j = 0; j < tableF.tables[fromTable[i]].fields.Count; j++)
                    {
                        fieldIndex[p] = j;
                        tableIndex[p] = i;
                        fieldNames[p] = tableF.tables[fromTable[i]].fields[j].FieldName;
                        p++;
                    }
                }
            }else
            {//指定列
                selectCount = fieldNames.Length;
                if (tableIndex == null || tableIndex.Length != selectCount)
                {
                    throw new DataEditException("内部参数错误：表索引长度错误");
                }
                fieldIndex = new int[fieldNames.Length];
                for(int i=0;i<selectCount; i++)
                {
                    if (tableIndex[i] == -1)
                    {//查找确定表号
                        int tIndex = -1;
                        for(int j = 0; j < fromTableCount; j++)
                        {
                            int tmp = tableF.tables[fromTable[j]].isFieldNameExist(fieldNames[i]);
                            if (tmp != -1)
                            {
                                if (tIndex != -1)
                                {
                                    throw new DataEditException("列名不明确：" + fieldNames[i]);
                                }
                                else
                                {
                                    tIndex = j;
                                    fieldIndex[i] = tmp;
                                }
                            }
                        }
                        if (tIndex == -1)
                        {
                            throw new DataEditException("不存在的列名：" + fieldNames[i]);
                        }
                        tableIndex[i] = tIndex;
                    }
                    else
                    {
                        int tmp = tableF.tables[fromTable[tableIndex[i]]].isFieldNameExist(fieldNames[i]);
                        if (tmp == -1)
                        {
                            throw new DataEditException("列名不明确：" + fieldNames[i]);
                        }
                        fieldIndex[i] = tmp;
                    }
                }
            }
            #endregion
            //取出数据
            DataTable[] dts = new DataTable[fromTableCount];
            Table[] tables = new Table[fromTableCount];
            for(int i = 0; i < fromTableCount; i++)
            {
                dts[i] = datas[fromTable[i]].getData(tableF.tables[fromTable[i]]);
                tables[i] = tableF.tables[fromTable[i]];
            }
            //构造空表
            DataTable dtResult = new DataTable();
            for(int i = 0; i < selectCount; i++)
            {
                int t = 0;
                while (true)
                {
                    try
                    {
                        if (t != 0)
                        {
                            dtResult.Columns.Add(new DataColumn(fieldNames[i]+t));
                        }else
                        {
                            dtResult.Columns.Add(new DataColumn(fieldNames[i]));
                        }
                        
                        break;
                    }
                    catch (Exception)
                    {
                        t++;
                    }
                }
                
            }

            int[] rows = new int[dts.Length];//每张表的行数
            int cartesian = 1;
            for (int j = 0; j < rows.Length; j++)
            {
                rows[j] = dts[j].Rows.Count;
                cartesian *= dts[j].Rows.Count;
            }
            for (int i = 0; i < cartesian; i++)
            {//笛卡尔积主循环
                if (condition==null||condition.isConditionSetUp(tables, dts, i))
                {//条件成立
                    DataRow dr = dtResult.NewRow();
                    for(int j = 0; j < selectCount; j++)
                    {
                        dr[j] = dts[tableIndex[j]].Rows[getInTableIndex(rows, i, tableIndex[j])][fieldIndex[j]];
                    }
                    dtResult.Rows.Add(dr);                    
                }
            }
            return dtResult;
        }
        /// <summary>
        /// 使用DataTable直接更改数据表中的数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">数据表</param>
        public void changeData(string tableName,DataTable dt)
        {
            int index = tableF.isTableNameExist(tableName);
            if (index == -1)
            {
                throw new DataEditException("表不存在" + tableName);
            }
            datas[index].saveData(dt, tableF.tables[index]);
            saveFile();
        }
        /// <summary>
        /// 向表中插入一行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="values">值列表</param>
        public void insert(string tableName,List<object> values)
        {
            int index = tableF.isTableNameExist(tableName);
            if (index == -1)
            {
                throw new DataEditException("表不存在：" + tableName);
            }
            string[] names = new string[tableF.tables[index].fields.Count];

            for(int i = 0; i < names.Length; i++)
            {
                names[i] = tableF.tables[index].fields[i].FieldName;
            }
            datas[index].addData(values.ToArray(), names, tableF.tables[index]);
            saveFile();
        }
        /// <summary>
        /// 从某个表中删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="condition">删除条件</param>
        public void delete(string tableName,Condition condition)
        {
            int index = tableF.isTableNameExist(tableName);
            if (index == -1)
            {
                throw new DataEditException("表不存在：" + tableName);
            }
            DataTable dt = datas[index].getData(tableF.tables[index]);
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                if(condition==null||condition.isConditionSetUp(new Table[] { tableF.tables[index] },new DataTable[] {dt }, i))
                {
                    dt.Rows.RemoveAt(i);
                    i--;
                }
            }
            datas[index].saveData(dt, tableF.tables[index]);
            saveFile();
        }
        /// <summary>
        /// 更新某个表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldNames">需要更新的字段名：若为null表示更新全部字段</param>
        /// <param name="values">字段对应的值</param>
        /// <param name="condition">更新的条件</param>
        public void update(string tableName,string[] fieldNames,object[] values,Condition condition)
        {
            int index = tableF.isTableNameExist(tableName);
            if (index == -1)
            {
                throw new DataEditException("表不存在：" + tableName);
            }
            Table table = tableF.tables[index];
            DataTable dt = datas[index].getData(tableF.tables[index]);
            int[] fieldIndex = new int[values.Length];
            #region 字段索引匹配
            if (fieldNames == null)
            {
                if (values.Length != table.fields.Count)
                {
                    throw new DataEditException("值数量和表的列数不匹配！请指定需要更新的列");
                }
                for(int i = 0; i < values.Length; i++)
                {
                    fieldIndex[i] = i;
                }
            }else
            {
                if (fieldNames.Length != values.Length)
                {
                    throw new DataEditException("值数量和列名数量不匹配！");
                }
                for(int i = 0; i < values.Length; i++)
                {
                    int tmp = table.isFieldNameExist(fieldNames[i]);
                    if (tmp == -1)
                    {
                        throw new DataEditException("列名不存在：" + fieldNames[i]);
                    }                   
                    fieldIndex[i] = tmp;
                }
            }
            #endregion
            #region 格式检查
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null)
                {
                    continue;
                }
                switch (table.fields[fieldIndex[i]].type)
                {
                    case Field.Type.Bit:
                        if (values[i].GetType() != typeof(bool))
                        {
                            throw new DataEditException("字段的类型应该为布尔值："+ table.fields[fieldIndex[i]].FieldName);
                        }
                        break;
                    case Field.Type.Int:
                        if (values[i].GetType() != typeof(int))
                        {
                            throw new DataEditException("字段的类型应该为整型：" + table.fields[fieldIndex[i]].FieldName);
                        }
                        break;
                    case Field.Type.nChar:
                        if (values[i].GetType() != typeof(string))
                        {
                            throw new DataEditException("字段的类型应该为字符：" + table.fields[fieldIndex[i]].FieldName);
                        }
                        break;
                    case Field.Type.Real:
                        if (values[i].GetType() != typeof(double)&& values[i].GetType() != typeof(int))
                        {
                            throw new DataEditException("字段的类型应该为实数：" + table.fields[fieldIndex[i]].FieldName);
                        }
                        break;

                }
            }
            #endregion

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (condition==null||condition.isConditionSetUp(new Table[] { table }, new DataTable[] { dt }, i))
                {
                    for(int j = 0; j < values.Length; j++)
                    {
                        dt.Rows[i][fieldIndex[j]] = values[j];
                    }
                }
            }
            datas[index].saveData(dt, table);
            saveFile();
        }
        /// <summary>
        /// 获取文件中存储的DataF实例
        /// </summary>
        /// <returns>用以操作数据的DataF实例</returns>
        public static DataF getDataF()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(FILE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read);
            DataF dataF = (DataF)formatter.Deserialize(stream);
            stream.Close();
            dataF.getTable();
            return dataF;
        }
        static DataF()
        {
            if (!File.Exists(FILE_PATH))
            {
                DataF DataF = new DataF();
                DataF.saveFile();
            }
        }
        /// <summary>
        /// 新建表时调用，保持文件一致性
        /// </summary>
        public void addTableData()
        {
            Data data = new Data();
            datas.Add(data);
            saveFile();
        }
        /// <summary>
        /// 删除表时调用，保持文件一致性
        /// </summary>
        /// <param name="index">表索引</param>
        public void deleteTableData(int index)
        {
            datas.RemoveAt(index);
            saveFile();
        }
        public void getTable()
        {
            tableF = TableF.getTableF();
        }
        private void saveFile()
        {
            Stream stream = new FileStream(FILE_PATH, FileMode.Create, FileAccess.Write, FileShare.None);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Close();
        }
        private int getInTableIndex(int[] rows, int i,int tableIndex)
        {           
            int time = 1;
            for (int j = rows.Length - 1; j > tableIndex; j--)
            {
                time *= rows[j];
            }
            return (i / time) % rows[tableIndex];
        }
    }
}
