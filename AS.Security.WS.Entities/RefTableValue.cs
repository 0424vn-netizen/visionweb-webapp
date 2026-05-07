using System;
using System.Data;
using System.Collections;

namespace AS.Security.WS.Entities
{
    public class RefTableValue
    {
        public RefTableValue() { }

        public int ASClient { get; set; }
        public string RefTblName { get; set; }
        public string RefTblKey { get; set; }
        public string RefTblKey1 { get; set; }
        public string RefTblLang { get; set; }
        public string ActvStatus { get; set; }
        public string[] RefTblCols { get; set; } = new string[9];
        public DateTime EffStartDTS { get; set; } = DateTime.MinValue;
        public DateTime EffEndDTS { get; set; } = DateTime.MinValue;
    }

    
    public class RefTableValueCollection : CollectionBase
    {
        public RefTableValue this[int index]
        {
            get { return ((RefTableValue)List[index]); }
            set { List[index] = value; }
        }

        public int Add(RefTableValue value)
        {
            return (List.Add(value));
        }

        public int IndexOf(RefTableValue value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, RefTableValue value)
        {
            List.Insert(index, value);
        }

        public void Remove(RefTableValue value)
        {
            List.Remove(value);
        }

        public bool Contains(RefTableValue value)
        {
            // If value is not of type UserIDs, this will return false.
            return (List.Contains(value));
        }
    }
}
