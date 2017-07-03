using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    [Serializable]
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
            if (isFieldNameExist(field.FieldName)!=-1)
            {
                throw new TableEditException("字段已存在" + field.FieldName);
            }
            if (field.isKey && isPKExist() != -1)
            {
                throw new TableEditException("主码已存在");
            }
            fields.Add(field);
            
        }
        /// <summary>
        /// 编辑字段
        /// </summary>
        /// <param name="fieldName">原字段名</param>
        /// <param name="changedField">更改后的字段</param>
        public void editField(string fieldName,Field changedField)
        {
            int i = isFieldNameExist(fieldName);
            if (i == -1)
            {
                throw new TableEditException("字段不存在" + fieldName);
            }
            int pk = isPKExist();
            if (changedField.isKey && !(pk == -1 || pk == i))
            {
                throw new TableEditException("主码已存在");
            }
            fields[i] = changedField;
        }
        /// <summary>
        /// 设置主键
        /// </summary>
        /// <param name="fieldName">主键字段名</param>
        /// <returns>若主键存在，则设置成功</returns>
        public void setPrimaryKey(string fieldName)
        {
            int i = isFieldNameExist(fieldName);
            if (i == -1)
            {
                throw new TableEditException("字段不存在" + fieldName);
            }
            for(int j = 0; i < fields.Count; i++)
            {
                fields[j].isKey = (i == j);
            }

        }
        public DataTable getEmptyTable()
        {
            DataTable dt = new DataTable();
            for(int i = 0; i < fields.Count; i++)
            {
                DataColumn dataColumn = new DataColumn(fields[i].FieldName);
                switch (fields[i].type)
                {
                    case Field.Type.Bit:
                        dataColumn.DataType = typeof(bool);
                        break;
                    case Field.Type.Int:
                        dataColumn.DataType = typeof(int);
                        break;
                    case Field.Type.nChar:
                        dataColumn.DataType=typeof(string);
                        dataColumn.MaxLength = fields[i].length;
                        break;
                    case Field.Type.Real:
                        dataColumn.DataType = typeof(double);
                        break;

                }
                dt.Columns.Add(new DataColumn());
            }
            return dt;
        }
        public Table(string tableName)
        {
            TableName = tableName;
        }
        /// <summary>
        /// 查询是否存在同名字段
        /// </summary>
        /// <param name="name">表名</param>
        /// <returns>若存在返回表序号，不存在返回-1</returns>
        public int isFieldNameExist(string name)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                if (name == fields[i].FieldName)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 主码是否存在
        /// </summary>
        /// <returns>主码序号，不存在返回-1</returns>
        public int isPKExist()
        {
            for (int i = 0; i < fields.Count; i++) {
                if (fields[i].isKey)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
