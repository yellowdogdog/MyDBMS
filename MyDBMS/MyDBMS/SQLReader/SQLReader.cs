using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBMS.SQLReader
{
    class SQLReader
    {
        public static DataTable readsql(string sql)
        {
            string[] all = new string[10];
            all[0] = "select";
            all[1] = "insert";
            all[2] = "delete";
            all[3] = "uqdate";
            all[4] = "drop";
            all[5] = "create";
            char[] first = new char[10];
            int i,j, k;
            j = 0;
            for (i = 0; sql[i] != '\0'; i++)
            {
                if (sql[i] != ' ')
                {
                    first[j] = sql[i];
                }
                if (j > 0 && sql[i] == ' ')
                    break;
            }
            k = i;
        }      

    }
}
