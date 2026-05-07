using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace AS.Security.WS.Entities
{
    public class AppConfig
    {
	#region Property
    public String AppCode { get; set; }
	
    public String KeyName { get; set; }

    public String KeyValue { get; set; }

    public Int32 SortSeq { get; set; }
    #endregion

    #region Constructors

    public AppConfig() { }

    public AppConfig(
        string appCode,
        string keyName,
        string keyValue,
        int sortSeq)
    {
        this.AppCode = appCode;
        this.KeyName = keyName;
        this.KeyValue = keyValue;
        this.SortSeq = sortSeq;
    }

    #endregion
    }
    public class AppConfigCollection : CollectionBase
    {

        public AppConfig this[int index]
        {
            get { return ((AppConfig)List[index]); }
            set { List[index] = value; }
        }

        public int Add(AppConfig value)
        {
            return (List.Add(value));
        }

        public int IndexOf(AppConfig value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, AppConfig value)
        {
            List.Insert(index, value);
        }

        public void Remove(AppConfig value)
        {
            List.Remove(value);
        }

        public bool Contains(AppConfig value)
        {
            // If value is not of type AppConfig, this will return false.
            return (List.Contains(value));
        }

    }
}