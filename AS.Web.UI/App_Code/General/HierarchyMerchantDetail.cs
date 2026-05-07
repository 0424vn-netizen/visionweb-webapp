using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HierarchyMerchantDetail
/// </summary>
/// 
[Serializable]
public class HierarchyDetail
{
    private string _HierarchyID;
    public string HierarchyID
    {
        get { return _HierarchyID; }
        set { _HierarchyID = value; }
    }

    private string _HierarchyMode;
    public string HierarchyMode
    {
        get { return _HierarchyMode; }
        set { _HierarchyMode = value; }
    }

    private string _HierarchyName;
    public string HierarchyName
    {
        get { return _HierarchyName; }
        set { _HierarchyName = value; }
    }

    private bool _IsMerchant;
    public bool IsMerchant
    {
        get { return _IsMerchant; }
        set { _IsMerchant = value; }
    }

    private string _Prefix;
    public string Prefix
    {
        get { return _Prefix; }
        set { _Prefix = value; }
    }
    private string _UserMode;
    public string UserMode
    {
        get { return _UserMode; }
        set { _UserMode = value; }
    }

    private bool _ShowPrefix;
    public bool ShowPrefix
    {
        get { return _ShowPrefix; }
        set { _ShowPrefix = value; }
    }

    private string _ChildOf;
    public string ChildOf
    {
        get { return _ChildOf; }
        set { _ChildOf = value; }
    }


}