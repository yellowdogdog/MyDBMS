using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    class Field
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        public  enum Type { Int,Real,Varchar,Bit}
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
