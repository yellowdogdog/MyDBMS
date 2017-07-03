using System;

namespace MyDBMS.MyDB
{
    [Serializable]
    class TableEditException : ApplicationException {
        public TableEditException()
        {

        }
        public TableEditException(string msg):base(msg)
        {

        }
    }
    class DataEditException : ApplicationException
    {
        public DataEditException()
        {

        }
        public DataEditException(string msg) : base(msg)
        {

        }
    }
}