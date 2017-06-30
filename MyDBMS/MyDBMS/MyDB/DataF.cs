using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    [Serializable]
    class DataF
    {
        /// <summary>
        /// 表数据的集合
        /// </summary>
        public List<Data> datas;
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
            return null;
        }
        /// <summary>
        /// 查询数据操作（多个数据表的复杂查询）
        /// </summary>
        /// <param name="tableNames">表名</param>
        /// <param name="fieldName">投影的字段名:如果为null表示查询所有列</param>
        /// <param name="condition">查询条件,若为null表示没有条件查询全部</param>
        /// <returns>数据表DataTable</returns>
        public DataTable selectData(string[] tableNames, string[] fieldName, Condition condition)
        {
            return null;
        }
        /// <summary>
        /// 使用DataTable直接更改数据表中的数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">数据表</param>
        public void changeData(string tableName,DataTable dt)
        {

        }
        /// <summary>
        /// 向表中插入一行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="values">值列表</param>
        public void insert(string tableName,List<object> values)
        {

        }
        /// <summary>
        /// 从某个表中删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="condition">删除条件</param>
        public void delete(string tableName,Condition condition)
        {

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

        }
    }
}
