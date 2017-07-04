using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.MyDB
{
    [Serializable]
    class Data
    {
        protected List<byte[]> tableDatas=new List<byte[]>();
        //[NonSerialized]
        //protected Table table;
        /// <summary>
        /// 获得此表中的所存放的数据
        /// </summary>
        /// <returns>存放数据的DataTable</returns>
        public DataTable getData(Table table)
        {
            DataTable dt=table.getEmptyTable();
            int rowLength = getRowLength(table);
            int fieldNum = table.fields.Count;
            for (int i = 0; i < tableDatas.Count; i++)
            {
                int p = 0;
                DataRow dr = dt.NewRow();
                for (int j=0; j < fieldNum; j++)
                {
                    if (BitConverter.ToBoolean(tableDatas[i], rowLength + j))
                    {
                        dr[i] = null;
                    }else
                    {
                        switch (table.fields[j].type)
                        {
                            case Field.Type.Bit:
                                dr[j] = BitConverter.ToBoolean(tableDatas[i], p);
                                break;
                            case Field.Type.Int:
                                dr[j] = BitConverter.ToInt32(tableDatas[i], p);
                                break;
                            case Field.Type.nChar:
                                dr[j]=Encoding.Default.GetString(tableDatas[i], p, table.fields[j].length);
                                break;
                            case Field.Type.Real:
                                dr[j] = BitConverter.ToDouble(tableDatas[i], p);
                                break;
                        }
                    }
                    
                    p += table.fields[j].length;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public void saveData(DataTable dt,Table table)
        {
            //格式：每个数据转成byte[]后，最后跟上每个数据是否为空
            tableDatas = new List<byte[]>();
            # region    检查主码
            int j = table.isPKExist();
            if (j != -1)
            {
                for(int i = 0; i < dt.Rows.Count-1; i++)
                {
                    for(int k = i + 1; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[i][j].Equals(dt.Rows[k][j])){
                            throw new DataEditException("主码约束冲突");
                        }
                    }
                }
            }
            #endregion
            int rowLength = getRowLength(table);
            int fieldNum = table.fields.Count;
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                byte[] data = new byte[rowLength + fieldNum];
                int p = 0;
                for(j = 0; j < fieldNum; j++)
                {
                    if (dt.Rows[i][j] == null)
                    {
                        if (!table.fields[j].isNullable)
                        {
                            throw new DataEditException("字段"+table.fields[j].FieldName+"不可为空");
                        }
                        BitConverter.GetBytes(true).CopyTo(data, rowLength + j);

                    }else
                    {
                        switch (table.fields[j].type)
                        {
                            case Field.Type.Bit:
                                BitConverter.GetBytes((bool)dt.Rows[i][j]).CopyTo(data, p);
                                break;
                            case Field.Type.Int:
                                BitConverter.GetBytes((int)dt.Rows[i][j]).CopyTo(data, p);
                                break;
                            case Field.Type.nChar:
                                Encoding.Default.GetBytes((string)dt.Rows[i][j]).CopyTo(data, p);
                                break;
                            case Field.Type.Real:
                                BitConverter.GetBytes((double)dt.Rows[i][j]).CopyTo(data, p);
                                break;
                        }
                    }                   
                    p += table.fields[j].length;
                }
                tableDatas.Add(data);
            }
        }
        public void addData(object[] values,string[] names,Table table)
        {
            object[] tableValue = new object[table.fields.Count];
            #region 转换值
            if (values.Length != names.Length)
            {
                throw new DataEditException("字段名和值数量不匹配");
                
            }            
            for(int i = 0; i < names.Length; i++)
            {
                int j = table.isFieldNameExist(names[i]);
                if (j == -1)
                {
                    throw new DataEditException("列名不存在" +names[i]);
                }
                if (tableValue[j] != null)
                {
                    throw new DataEditException("重复的列名" + names[i]);
                }
                tableValue[i] = values[i];
            }
            #endregion
            #region 主码检查
            int pk = table.isPKExist();
            if (pk != -1)
            {
                if (tableValue[pk] == null)
                {
                    throw new DataEditException("主码不可为空！");
                }
                DataTable dt = getData(table);
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    if (tableValue[pk].Equals(dt.Rows[i]))
                    {
                        throw new DataEditException("不符合主码约束");
                    }
                }
            }
            #endregion
            int rowLength = getRowLength(table);
            int fieldNum = table.fields.Count;
            byte[] data = new byte[rowLength + fieldNum];
            int p = 0;
            for (int j = 0; j < fieldNum; j++)
            {
                if (tableValue[j]== null)
                {
                    if (!table.fields[j].isNullable)
                    {
                        throw new DataEditException("字段" + table.fields[j].FieldName + "不可为空");
                    }
                    BitConverter.GetBytes(true).CopyTo(data, rowLength + j);

                }else
                {
                    switch (table.fields[j].type)
                    {
                        case Field.Type.Bit:
                            BitConverter.GetBytes((bool)tableValue[j]).CopyTo(data, p);
                            break;
                        case Field.Type.Int:
                            BitConverter.GetBytes((int)tableValue[j]).CopyTo(data, p);
                            break;
                        case Field.Type.nChar:
                            Encoding.Default.GetBytes((string)tableValue[j]).CopyTo(data, p);
                            break;
                        case Field.Type.Real:
                            BitConverter.GetBytes((double)tableValue[j]).CopyTo(data, p);
                            break;
                    }
                }
               
                p += table.fields[j].length;
            }
            tableDatas.Add(data);
        }
        /// <summary>
        /// 获取单行占用字节数
        /// </summary>
        /// <param name="table">表格式</param>
        /// <returns></returns>
        private int getRowLength(Table table)
        {
            int sum = 0;
            for(int i = 0; i < table.fields.Count; i++)
            {
                sum += table.fields[i].length;
            }
            return sum;
        }
    }
}
