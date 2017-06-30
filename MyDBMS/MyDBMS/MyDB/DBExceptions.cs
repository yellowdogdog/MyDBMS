using System;

namespace MyDBMS.MyDB
{
    [Serializable]
    class TableEditException : ApplicationException {
        public TableEditException()
        {

        }
        public TableEditException(String msg):base(msg)
        {

        }
    }
}