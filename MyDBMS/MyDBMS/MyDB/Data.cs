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
        protected List<byte[]> tableDatas;
        [NonSerialized]
        protected Table table;
        /// <summary>
        /// 获得此表中的所存放的数据
        /// </summary>
        /// <returns>存放数据的DataTable</returns>
        public DataTable getData()
        {
            return null;
        }
    }
}
