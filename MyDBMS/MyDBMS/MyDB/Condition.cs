using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    [Serializable]
    class Condition
    {
        public enum Operate { Equ,Greater,Less,GreaterE,LessE,And,Or,Not}
        /// <summary>
        /// 条件中的字段名
        /// </summary>
        public string fieldName { get; set; }
        /// <summary>
        /// 条件中的值
        /// </summary>
        public object value { get; set; }
        /// <summary>
        /// 条件中的字段名对应的表的序号（如在select中有from table1,table2 则table1的序号为0，table2序号为1）
        /// </summary>
        public int tableIndex { get; set;}
        /// <summary>
        /// 运算符，枚举类型
        /// </summary>
        public Operate operate;
        /// <summary>
        /// 若为双目运算，则为左条件，若为not运算，则为not的子条件
        /// </summary>
        Condition leftCondition;
        /// <summary>
        /// 双目运算的右条件
        /// </summary>
        Condition rightCondition;
        /// <summary>
        /// 新建一个表示变量的叶子条件
        /// </summary>
        /// <param name="fieldName">变量的字段名</param>
        /// <param name="tableIndex">表的序号</param>
        public Condition(string fieldName,int tableIndex=0)
        {
            this.fieldName = fieldName;
            this.tableIndex = tableIndex;
        }
        /// <summary>
        /// 新建一个表示值的叶子条件
        /// </summary>
        /// <param name="value"></param>
        public Condition(object value)
        {
            this.value = value;
        }
        /// <summary>
        /// 新建双目运算条件
        /// </summary>
        /// <param name="leftCondition">左子条件</param>
        /// <param name="rightCondition">右子条件</param>
        /// <param name="operate">操作</param>
        public Condition(Condition leftCondition,Condition rightCondition,Operate operate)
        {
            this.leftCondition = leftCondition;
            this.rightCondition = rightCondition;
            this.operate = operate;
        }
        /// <summary>
        /// 新建非运算条件
        /// </summary>
        /// <param name="notCondition"></param>
        public Condition(Condition notCondition)
        {
            leftCondition = notCondition;
            operate = Operate.Not;
        }
    }
}
