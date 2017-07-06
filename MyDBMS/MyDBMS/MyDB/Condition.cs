using System;
using System.Collections.Generic;
using System.Data;
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
        /// <param name="notCondition">非条件</param>
        public Condition(Condition notCondition)
        {
            leftCondition = notCondition;
            operate = Operate.Not;
        }
        /// <summary>
        /// 获取条件类型
        /// </summary>
        /// <returns>条件类型：
        /// 1：变量叶节点
        /// 2：值叶节点
        /// 3：双目条件
        /// 4：否条件
        /// -1:条件格式错误
        /// </returns>
        public int getConditionType() 
        {
            if (value == null && fieldName != null)
            {
                return 1;
            }
            if (value !=null && fieldName == null)
            {
                return 2;
            }
            if (value == null && fieldName == null && operate != Operate.Not)
            {
                return 3;
            }
            if (value == null && fieldName == null && operate == Operate.Not)
            {
                return 4;
            }
            return -1;
        }
        public bool isConditionSetUp(Table[] tables,DataTable[] dts,int i)
        {
            switch (getConditionType())
            {
                case 1://列名
                    Table table = tables[tableIndex];
                    int j = table.isFieldNameExist(fieldName);
                    if (j== -1)
                    {
                        throw new DataEditException("列名不存在：" + fieldName);
                    }
                    if (table.fields[j].type != Field.Type.Bit)
                    {
                        throw new DataEditException("条件格式错误:不可为非布尔值的列");
                    }
                    
                    break;
                case 2://值
                    if (value.GetType() != typeof(bool))
                    {
                        throw new DataEditException("条件格式错误：不可为非布尔值的值");
                    }else
                    {
                        return (bool)value;
                    }
                case 3://双目运算
                    if (operate == Operate.Equ)
                    {
                        object leftValue = leftCondition.getValue(tables,dts,i);
                        if (leftValue.GetType() == typeof(string))
                        {
                            leftValue=((string)leftValue).Replace("\0", "");
                        }                       
                        object rightValue = rightCondition.getValue(tables, dts, i);
                        if (rightValue.GetType() == typeof(string))
                        {
                            rightValue = ((string)rightValue).Replace("\0", "");
                        }
                        return leftValue.Equals(rightValue);
                    }else if (operate == Operate.Greater || operate == Operate.GreaterE || operate == Operate.Less || operate == Operate.LessE)
                    {
                        object leftValue = leftCondition.getValue(tables, dts, i);
                        object rightValue = rightCondition.getValue(tables, dts, i);
                        double l;
                        double r;
                        if ((leftValue.GetType() == typeof(int) || leftValue.GetType() == typeof(double))
                            &&(rightValue.GetType()==typeof(int)||rightValue.GetType()==typeof(double)))
                        {
                            l = (double)leftValue;
                            r = (double)rightValue;
                            switch (operate)
                            {
                                case Operate.Greater:
                                    return l > r;
                                case Operate.GreaterE:
                                    return l >= r;
                                case Operate.Less:
                                    return l < r;
                                case Operate.LessE:
                                    return l <= r;
                            }
                        }else
                        {
                            throw new DataEditException("大于小于运算符的操作数不为数值");
                        }
                    }
                    else
                    {
                        if (operate == Operate.And)
                        {
                            return leftCondition.isConditionSetUp(tables,dts,i) && rightCondition.isConditionSetUp(tables,dts,i);
                        }else
                        {
                            return leftCondition.isConditionSetUp(tables, dts, i) || rightCondition.isConditionSetUp(tables, dts, i);
                        }
                    }
                    break;
                case 4://非运算
                    return !leftCondition.isConditionSetUp(tables, dts, i);               
            }
            throw new DataEditException("未知的条件");
        }
        public object getValue(Table[] tables, DataTable[] dts, int i)
        {
            int cd = getConditionType();
            if (cd == 3 || cd == 4)
            {
                throw new DataEditException("运算符找不到值对象");
            }
            if (cd == 1)
            {
                int j = -1 ;
                if (tableIndex == -1)
                {//查找确定表号
                    int tIndex = -1;
                    for (int k = 0; k <tables.Length; k++)
                    {
                        int tmp = tables[k].isFieldNameExist(fieldName);
                        if (tmp != -1)
                        {
                            if (tIndex != -1)
                            {
                                throw new DataEditException("列名不明确：" + fieldName);
                            }
                            else
                            {
                                tIndex = k;
                                j = tmp;
                            }
                        }
                    }
                    if (tIndex == -1)
                    {
                        throw new DataEditException("不存在的列名：" + fieldName);
                    }
                    tableIndex = tIndex;
                }else
                {
                    Table table = tables[tableIndex];
                    j = table.isFieldNameExist(fieldName);
                }
              
                if (j == -1)
                {
                    throw new DataEditException("列名不存在：" + fieldName);
                }
                return dts[tableIndex].Rows[getInTableIndex(dts, i)][j];
            }
            if (cd == 2)
            {
                return value;
            }
            return null;

        }
        /// <summary>
        /// 获取笛卡尔积排列的表中第i个元组在tableIndex个表中的相对位置
        /// </summary>
        /// <param name="dts">数据表集</param>
        /// <param name="i">笛卡尔积中的元组序号</param>
        /// <returns>指定表中的相对序号</returns>
        private int getInTableIndex(DataTable[] dts,int i)
        {
            int[] rows = new int[dts.Length];
            for(int j = 0; j < rows.Length; j++)
            {
                rows[j] = dts[j].Rows.Count;
            }
            int time = 1;
            for(int j = rows.Length - 1; j > tableIndex; j--)
            {
                time *= rows[j];
            }
            return (i / time) % rows[tableIndex];

        }
    }
}
