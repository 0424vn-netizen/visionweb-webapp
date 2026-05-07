using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExtractReportEntities
/// </summary>
[Serializable]
public class MgmtReportEntities
{
    private string _SearchValue;
    public string SearchValue
    {
        get { return _SearchValue; }
        set { _SearchValue = value; }
    }
    
    private int _MerchantType;
    public int MerchantType
    {
        get { return _MerchantType; }
        set { _MerchantType = value; }
    }

    private int _CategoryCode;
    public int CategoryCode
    {
        get { return _CategoryCode; }
        set { _CategoryCode = value; }
    }

    private int _SubCategoryCode;
    public int SubCategoryCode
    {
        get { return _SubCategoryCode; }
        set { _SubCategoryCode = value; }
    }

    private string _SPAName;
    public string SPAName
    {
        get { return _SPAName; }
        set { _SPAName = value; }
    }

    private bool _HasDateRangeFilter;
    public bool HasDateRangeFilter
    {
      get { return _HasDateRangeFilter; }
      set { _HasDateRangeFilter = value; }
    }

    private bool _HasMerchantFilter;
    public bool HasMerchantFilter
    {
        get { return _HasMerchantFilter; }
        set { _HasMerchantFilter = value; }
    }

    public MgmtReportEntities()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}