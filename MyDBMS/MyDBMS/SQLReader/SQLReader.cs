using MyDBMS.MyDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDBMS.SQLReader
{
    class SQLreader
    {
        public static string[] third = new string[20];
        public static string[] operate = new string[10];
        public static int k = 0, l = 0;
        private static void initialize(string[] f)
        {
            for (int a1 = 0; a1 < f.Length; a1++)
                f[a1] = null;
        }
        public static DataTable readsql(string sql)
        {
            initialize(third);
            initialize(operate);
            //sql = "select a from student where sno='1234'";
            string[] all = new string[10];
            all[0] = "select";
            all[1] = "insert";
            all[2] = "delete";
            all[3] = "update";
            all[4] = "drop";
            all[5] = "create";
            
            string[]  keyword =new string[10];
            string[] first = new string[10];
            string[] second = new string[10];
            initialize(keyword);
            initialize(first);
            initialize(second);
            int i;
            for (i = 0; i < 10; i++)
            {
                keyword[i]=null;
            }
            while (i < sql.Length)
            {
                for (i = 0; sql[i] != ' ' && sql[i] != '\n'; i++)
                {
                    keyword[0] = keyword[0] + sql[i];
                    k = 1;
                }
                if (k==1 && sql[i] == ' ')
                    break;
            }            
            int m = 0,n=0;
            k = 0;
            if (keyword[0].ToLower() == all[0])//select
            {
                while (i < sql.Length)
                {
                    first[m]=null;
                    for (; i < sql.Length && sql[i] != ' ' && sql[i] != '\n'; i++)
                    {
                        first[m] = first[m] + sql[i];
                        k = 1;
                    }
                    if (first[m]!=null&&first[m].ToLower() == "from")
                        break;
                    if (k == 1)
                    {
                        m++;
                    }
                    i++;

                }

                keyword[1] = first[m];
                for (; m < 10; m++)
                {
                    first[m] = null;
                }
                k = 0;
                while (i < sql.Length)
                {
                    second[n]=null;
                    for (; i < sql.Length  && sql[i] != '\n' && sql[i] != ',' && sql[i] != ' '; i++)
                    {
                        second[n] = second[n] + sql[i];
                        k = 1;
                        if (sql[i] == '>' || sql[i] == '<' || sql[i] == '=')
                        {
                            MessageBox.Show("未输入where，错误");
                            break;
                        }
                    }
                    if (second[n]!=null&&second[n].ToLower() == "where")
                        break;
                    if (k == 1)
                    {
                        n++;
                        k = 0;
                    }
                    i++;
                }
                keyword[2] = "where";
                for (; n < 10; n++)
                {
                    second[n] = null;
                }
                k = 0;
                int s;
               //去second[]  null
                for ( s = 0; s < second.Length;s++)
                {
                    if (second[s] == null)
                        break;
                }
                string[] second0 = new string[s];
                for (int s1 = 0; s1 < s; s1++)
                {
                    second0[s1] = second[s1];
                }
                //去first[]  null
                for (s = 0; s < first.Length; s++)
                {
                    if (first[s] == null)
                        break;
                }
                string[] first0 = new string[s];
                for (int s1 = 0; s1 < s; s1++)
                {
                    first0[s1] = first[s1];
                }
                int[] index = new int[s];
                for(int s1=0;s1<s;s1++)
                {
                    index[s1] = getindex(second0,first0[s1]);
                    first0[s1] = getname(first0[s1]);
                }
                if (first0[0] == "*")
                    first0 = null;
                DataF dataF = DataF.getDataF();
                if(i==sql.Length)
                {
                    DataTable dt0 = dataF.selectData(second0, first0, index, null);
                    return dt0;
                }
                else
                {
                    DataTable dt = dataF.selectData(second0, first0, index, getcondition(second0, i, sql));//select查询
                    return dt;
                }                   
            }
            else if (keyword[0].ToLower() == all[1])//insert
            {
                int ins = 0;

                k = 0;
                while (i < sql.Length)
                {
                    second[ins]=null;
                    for (; i < sql.Length && sql[i] != '(' && sql[i] != ')' && sql[i] != ',' && sql[i] != ' ' && sql[i] != '\n'; i++)
                    {
                        second[ins] = second[ins] + sql[i];
                        k = 1;
                    }
                    if (k == 1)
                    {
                        ins++;
                        k = 0;
                    }


                    //second[1]是表名
                    if (sql[i] == '(')
                        break;
                    i++;
                }
                //写到这！！！！！
                int ins1 = 0;
                k = 0;
                while (i < sql.Length - 1)
                {
                    third[ins1]=null;
                    for (; sql[i] != '\'' && sql[i] != '(' && sql[i] != ')' && sql[i] != ',' && sql[i] != ' ' && sql[i] != '\n' && i < sql.Length - 1; i++)
                    {
                        third[ins1] = third[ins1] + sql[i];
                        k = 1;
                    }
                    if (k == 1)
                    {
                        ins1++;
                        k = 0;
                    }

                    if (sql[i] == ')')
                        break;
                    i++;
                }
                //third[]中存储values
                int s;
                for (s = 0; s < third.Length; s++)
                {
                    if (third[s] == null)
                        break;
                }
                string[] third1 = new string[s];
                for (int s1 = 0; s1 < s; s1++)
                    third1[s1] = third[s1];
                //DataF dataF = new DataF();
                DataF dataF = DataF.getDataF();
                List<object> list = new List<object>();
                for (int s1 = 0; s1 < s; s1++)
                    list.Add(isname(third1[s1]));
                //object[] b = (object[])ArrayList.Adapter((Array)third).ToArray(typeof(object));
                dataF.insert(second[1], list);
                DataTable dt = null;
                return dt;
            }
            else if (keyword[0].ToLower() == all[2])//delete
            {
                int ins = 0;

                k = 0;
                while (i < sql.Length)
                {
                    second[ins]=null;
                    for (; i < sql.Length && sql[i] != '(' && sql[i] != ')' && sql[i] != ',' && sql[i] != ' ' && sql[i] != '\n'; i++)
                    {
                        second[ins] = second[ins] + sql[i];
                        k = 1;
                    }

                    //second[1]是表名
                    if (second[ins]!=null&&second[ins].ToLower() == "where")
                        break;
                    if (k == 1)
                    {
                        ins++;
                        k = 0;
                    }
                    i++;
                }
                //获取condition
                //DataF dataf = new DataF();
                DataF dataf = DataF.getDataF();
                string[] second1 = new string[1];
                second1[0] = second[1];
                dataf.delete(second[1], getcondition(second1,i,sql));
                DataTable dt = null;
                return dt;
            }
            else if (keyword[0].ToLower() == all[3])//update
            {
                int ins = 0;
                k = 0;
                while (i < sql.Length)
                {
                    second[ins]=null;
                    for (; i < sql.Length && sql[i] != '(' && sql[i] != ')' && sql[i] != ',' && sql[i] != ' ' && sql[i] != '\n'; i++)
                    {
                        second[ins] = second[ins] + sql[i];
                        k = 1;
                    }

                    //second[0]是表名
                    if (second[ins]!=null&&second[ins].ToLower() == "set")
                        break;
                    if (k == 1)
                    {
                        ins++;
                        k = 0;
                    }
                    i++;
                }
                int upd = 0, upd1 = 0, k1 = 0;
                string[] first1 = new string[10];
                k = 0;
                while (i < sql.Length)
                {

                    k1 = 0;
                    first[upd]=null;
                    first1[upd1]=null;
                    for (; i < sql.Length && sql[i] != ',' && sql[i] != ' ' && sql[i] != '\n'; i++)
                    {
                        if (sql[i] == '=')
                            k1 = 1;
                        if (k1 == 0)
                            first[upd] = first[upd] + sql[i];
                        else
                        {
                            if (sql[i] != '=' && sql[i] != '\'')
                                first1[upd1] = first1[upd1] + sql[i];
                        }
                        k = 1;
                    }
                    if (first[upd]!=null&&first[upd].ToLower() == "where")
                    {
                        first[upd] = null;
                        first1[upd1] = null;
                        break;
                    }

                    if (k == 1)
                    {
                        upd++;
                        upd1++;
                        k = 0;
                    }
                    i++;
                }
                //获取condition

                //去null
                string[] second1 = new string[1];
                second1[0] = second[1];
                int s;
                for (s = 0; s < first.Length; s++)
                    if (first[s] == null)
                        break;
                string[] first0 = new string[s];
                for (int s1 = 0; s1 < s; s1++)
                    first0[s1] = first[s1];
                for (s = 0; s < first1.Length; s++)
                    if (first1[s] == null)
                        break;
                object[] first10 = new string[s];
                for (int s1 = 0; s1 < s; s1++)
                    first10[s1] = isname(first1[s1]);
                //DataF dataf = new DataF();
                DataF dataf = DataF.getDataF();
                dataf.update(second[0], first0, first10, getcondition(second1,i,sql));
                DataTable dt = null;
                return dt;
            }
            else if (keyword[0] == all[4])//drop
            {
                int ins = 0;
                k = 0;
                while (i < sql.Length)
                {
                    second[ins]=null;
                    for (; i < sql.Length && sql[i] != '(' && sql[i] != ')' && sql[i] != ',' && sql[i] != ' ' && sql[i] != '\n'; i++)
                    {
                        second[ins] = second[ins] + sql[i];
                        k = 1;
                    }

                    //second[1]是表名
                    /*if (second[ins].ToLower() == "set")
                        break;*/
                    if (k == 1)
                    {
                        ins++;
                        k = 0;
                    }
                    i++;
                }
                //11111111
                TableF tablef = TableF.getTableF();
                tablef.deleteTable(second[1]);
            }
            else if (keyword[0].ToLower() == all[5])//create
            {
                int ins = 0;
                k = 0;
                while (i < sql.Length)
                {
                    second[ins]=null;
                    for (; i < sql.Length && sql[i] != '(' && sql[i] != ')' && sql[i] != ',' && sql[i] != ' ' && sql[i] != '\n'; i++)
                    {
                        second[ins] = second[ins] + sql[i];
                        k = 1;
                    }
                    //second[1]是表名
                    if (sql[i] == '(')
                        break;
                    if (k == 1)
                    {
                        ins++;
                        k = 0;
                    }
                    i++;
                }
                //1111111111
                TableF tableF =TableF.getTableF();
                Table table = new Table(second[1]);
                int c = 0;
                k = 0;
                bool nul = false, key = false;
                while (i < sql.Length)
                {
                    third[c] = null;
                    for (; i < sql.Length && sql[i] != ' ' && sql[i] != '\n' && sql[i] != '(' && sql[i] != ')' && sql[i] != '[' && sql[i] != ']'; i++)
                    {
                        if (sql[i] == ',')
                            break;
                        third[c] = third[c] + sql[i];
                        k = 1;
                    }
                    if (c == 2)
                    {
                        bool num = true;
                        for (int i1 = 0; third[2] != null && i1 < third[c].Length; i1++)
                        {
                            if (third[2]!=null&&!Char.IsNumber(third[2], i1))
                            {
                                num = false;
                                break;
                            }
                        }
                        if (third[2]==null||num != true)
                        {
                            c++;
                            third[c] = third[c - 1];
                            third[c - 1] = "0";
                        }
                    }
                    if (c == 3)
                    {
                        if (third[c]!=null&&third[c].ToLower() == "null")
                            nul = true;
                        else if (third[c] != null && third[c].ToLower() == "iskey")
                            key = true;
                        else if(third[c]!=null)
                            MessageBox.Show("是否为空或是否为主键输入错误！");
                    }
                    if (c == 4)
                    {
                        if (third[c] != null && third[c].ToLower() == "iskey")
                            key = true;
                        else if (third[c] != null)
                            MessageBox.Show("是否为主键输入错误！");
                    }
                    if (sql[i] == ',' || sql[i] == ')')
                    {
                        if (third[2] == null)
                            third[2] = "0";
                        //获取枚举值en
                        Type ob = typeof(Field.Type);
                        //Field.Type ob = new Field.Type();
                        //ob = Field.Type.Int;
                        Array ar = Enum.GetValues(ob);
                        int en;
                        for (en = 0; en < ar.Length; en++)
                        {
                            if (third[1].ToLower() == ar.GetValue(en).ToString().ToLower())
                                break;
                        }
                        Field field =new Field(third[0], (Field.Type)en, Convert.ToInt16(third[2]), nul, key);
                        table.addField(field);
                    }
                    if (sql[i] == ')')
                        break;
                    if (sql[i] == ',')
                    {
                        for (int i1 = 0; i1 < third.Length; i1++)
                            third[i1] = null;
                        c = 0;
                        k = 0;
                    }
                    if (k == 1)
                    {
                        c++;
                        k = 0;
                    }

                   
                    i++;
                }
                tableF.addTable(table);
                DataTable dt = null;
                return dt;

            }
            else
            {
                MessageBox.Show("语句输入错误！");
                DataTable dt = null;
                return dt;
            }
            DataTable dt1 = null;
            return dt1;
        }

        private static Condition getcondition(string []second2,int i, string sql)
        {
            l = 0;
            k = 0;
            while (i < sql.Length)
            {

                third[l]=null;
                for (; i < sql.Length && sql[i] != ' ' && sql[i] != '\n'; i++)
                {
                    if (sql[i] == '>')
                    {
                        l++;
                        third[l] = "Greater";
                        k = 1;
                        break;
                    }
                    else if (sql[i] == '<')
                    {
                        l++;
                        third[l] = "Less";
                        k = 1;
                        break;
                    }
                    else if (sql[i] == '=')
                    {
                        l++;
                        third[l] = "Equ";
                        k = 1;
                        break;
                    }
                    /*else if (sql[i] == '\'' || sql[i] == '\'')
                    {
                        k = 0;
                    }*/
                    else
                    {
                        third[l] = third[l] + sql[i];
                        k = 1;
                        if (i < sql.Length - 1)
                        {
                            if (sql[i + 1] == '>' || sql[i + 1] == '<' || sql[i + 1] == '=')
                                k = 0;
                        }

                    }
                }
                i++;
                if (k == 1)
                {
                    l++;
                    k = 0;
                }

            }
            //l = l + 1;
            for (int t = 1; t < l; t++)
            {
                if (third[t] == "Equ")
                {
                    int t1 = t;
                    t1 = t1 - 1;
                    if (third[t1] == "Greater")
                    {
                        third[t1] = "GreaterE";//等待修改
                        for (int t2 = t + 1; t2 < l; t2++)
                        {
                            t1++;
                            third[t1] = third[t2];
                        }
                        l = l - 1;
                    }
                    if (third[t1] == "Less")
                    {
                        third[t1] = "LessE";//等待修改
                        for (int t2 = t + 1; t2 < l; t2++)
                        {
                            t1++;
                            third[t1] = third[t2];
                        }
                        l = l - 1;
                    }
                }
            }
            operate[0] = "Greater";
            operate[1] = "Less";
            operate[2] = "Equ";
            operate[3] = "GreaterE";//等待修改
            operate[4] = "LessE";//等待修改
            //DataF dataF = new DataF();
            //DataF dataF = DataF.getDataF();
            Condition[] condition = new Condition[20];
            int c = 0;
            for (int t = 0; t < l; t++)
            {
                if (third[t]!=null&&third[t].ToLower() == "not")
                {
                    condition[c] = new Condition(condition[c]);
                    c++;
                }
                for (int t1 = 0; t1 < 5; t1++)
                    if (third[t] == operate[t1])//等待添加大于等于和小于等于
                    {
                        int a = third[t-1].IndexOf('\'');
                        if (a == -1)
                        {
                            if (isname(third[t - 1]).GetType() == typeof(string))
                                condition[c] = new Condition(getname(third[t - 1]),getindex(second2,third[t-1]) );
                            else condition[c] = new Condition(isname(third[t - 1]));
                        }
                        else condition[c] = new Condition((object)third[t - 1].Substring(1, third[t - 1].Length - 2));//错误
                        //condition[c] = new Condition((Object)third[t - 1]);
                        int a1 = third[t + 1].IndexOf('\'');
                        if (a1 == -1)
                        {
                            if (isname(third[t + 1]).GetType() == typeof(string))
                                condition[c+1] = new Condition(getname(third[t + 1]),getindex(second2,third[t+1] ));
                            else condition[c+1] = new Condition(isname(third[t + 1]));
                        }
                        else condition[c+1] = new Condition((object)third[t +1 ].Substring(1, third[t + 1].Length - 2));
                       // condition[c + 1] = new Condition((Object)third[t + 1]);
                        if (t1 == 0)
                            condition[c] = new Condition(condition[c], condition[c + 1], Condition.Operate.Greater);
                        if (t1 == 1)
                            condition[c] = new Condition(condition[c], condition[c + 1], Condition.Operate.Less);
                        if (t1 == 2)
                            condition[c] = new Condition(condition[c], condition[c + 1], Condition.Operate.Equ);
                        if (t1 == 3)
                            condition[c] = new Condition(condition[c], condition[c + 1], Condition.Operate.GreaterE);
                        if (t1 == 4)
                            condition[c] = new Condition(condition[c], condition[c + 1], Condition.Operate.LessE);
                        c++;
                        break;
                    }
            }
            c = c - 1;
            int c1 = 0;
            for (int t = 0; t < l; t++)
            {
                if (third[t]!=null&&third[t].ToLower() == "and")
                {
                    condition[c1] = new Condition(condition[c1], condition[c1 + 1], Condition.Operate.And);
                    for (int c0 = 1; c0 < c; c++)
                    {
                        condition[c0] = condition[c0 + 1];
                    }
                }
                if (third[t] != null && third[t].ToLower() == "less")
                {
                    condition[c1] = new Condition(condition[c1], condition[c1 + 1], Condition.Operate.Or);
                    for (int c0 = 1; c0 < c; c++)
                    {
                        condition[c0] = condition[c0 + 1];
                    }
                }

            }
            return condition[0];


           /* string a;
            int b;
            double cc;
            if (Int32.TryParse(a,out b))
            {

            }else if (double.TryParse(a,out cc))
            {

            }*/
        }

        private static int getindex(string[] second2, string f)
        {
            int c = f.IndexOf('.');
            if (c != -1)
            {
                string b = f.Substring(0, c);
                return Array.IndexOf(second2, b);
            }
           //if(isname("").GetType()==typeof(string))
            return -1;
        }
        private static object isname(string f)
        {
            string a;
            a = f;
            int b;
            double cc;
            bool t;
            if (Int32.TryParse(a, out b))
            {
                return Int32.Parse(a);
            }
            else if (double.TryParse(a, out cc))
            {
                return double.Parse(a);
            }
            else if (bool.TryParse(a, out t))
                return bool.Parse(a);
            else return a;
            //return 0;
        }
        private static string getname(string f)
        {
            int c = f.IndexOf('.');
            if (c != -1)
            {
                string b = f.Substring(c+1, f.Length-c-1);
                return b;
            }
            return f;
        }
    }
}
