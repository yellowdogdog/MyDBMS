using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    class Table
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName;
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<Field> fields;
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段</param>
        public void addField(Field field)
        {

        }
        /// <summary>
        /// 编辑字段
        /// </summary>
        /// <param name="fieldName">原字段名</param>
        /// <param name="changedField">更改后的字段</param>
        public void editField(string fieldName,Field changedField)
        {

        }
        /// <summary>
        /// 设置主键
        /// </summary>
        /// <param name="fieldName">主键字段名</param>
        /// <returns>若主键存在，则设置成功</returns>
        public bool setPrimaryKey(string fieldName)
        {
            return false;
        }
    }
}
