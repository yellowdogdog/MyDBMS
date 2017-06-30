using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    [Serializable]
    class Field
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        public  enum Type { Int,Real,nChar,Bit}
        /// <summary>
        /// 字段种类
        /// </summary>
        public Type type;
        /// <summary>
        /// 字段长度
        /// </summary>
        public int length { get; set; }
        /// <summary>
        /// 是否主码
        /// </summary>
        public bool isKey { get; set; }
        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool isNullable { get; set; }
        /// <summary>
        /// 新建字段
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="type">字段类型</param>
        /// <param name="length">字段长度，不需要长度(Int,Real,Bit)的默认为0</param>
        /// <param name="isNullable">字段是否为空，默认为不可为空</param>
        /// <param name="isKey">字段是否为主键</param>
        public Field(string FieldName,Type type,int length=0,bool isNullable=false,bool isKey=false)
        {
            this.FieldName = FieldName;
            this.type = type;
            this.length = length;
            this.isNullable = isNullable;
            this.isKey = isKey;
        }

    }
}
