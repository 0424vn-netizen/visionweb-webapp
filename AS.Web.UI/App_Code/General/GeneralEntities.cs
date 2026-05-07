using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

using Telerik.Web.UI;

using AS.Controls;
using AS.Security.WS.Entities;
using AS.Common.DBManager;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Summary description for GeneralEntities
/// </summary>
[Serializable]
public class PageStateCollection : CollectionBase, AS.Controls.IPageStateCollection
{

    public AS.Controls.IPageState this[int index]
    {
        get { return ((AS.Controls.IPageState)List[index]); }
        set { List[index] = value; }
    }
    AS.Controls.IPageState getObjectByName(string name)
    {
        for (int i = 0; i < List.Count; i++)
        {
            if (((AS.Controls.IPageState)List[i]).Name == name) return (AS.Controls.IPageState)List[i];
        }
        return null;
    }
    public AS.Controls.IPageState this[string name]
    {
        get { return getObjectByName(name); }
        set
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (((AS.Controls.IPageState)List[i]).Name == name)
                {
                    List[i] = value;
                    return;
                }
            }
            List.Add(value);
        }
    }

    public int Add(AS.Controls.IPageState value)
    {
        return (List.Add(value));
    }

    public int IndexOf(AS.Controls.IPageState value)
    {
        return (List.IndexOf(value));
    }

    public void Insert(int index, AS.Controls.IPageState value)
    {
        List.Insert(index, value);
    }

    public void Remove(AS.Controls.IPageState value)
    {
        List.Remove(value);
    }

    public bool Contains(AS.Controls.IPageState value)
    {
        // If value is not of type SECMenuItem, this will return false.
        return (List.Contains(value));
    }



}
public class PageState : AS.Controls.IPageState
{
    private string _name = "first";
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }


    private object _datasource;

    public object RefDatasource
    {
        get { return _datasource; }
        set { _datasource = value; }
    }

    private string _refPage;

    public string RefPage
    {
        get { return _refPage; }
        set { _refPage = value; }
    }

    private int _RowPerPage;

    public int RowPerPage
    {
        get { return _RowPerPage; }
        set { _RowPerPage = value; }
    }

    private int _PageIndex;

    public int PageIndex
    {
        get { return _PageIndex; }
        set { _PageIndex = value; }
    }


}

[Serializable]
public class ClientInfo
{
    private string _ClientName = "";
    private string _Address1 = "";
    private string _Address2 = "";
    private string _Zip = "";
    private string _Phone = "";
    private string _Fax = "";
    private string _ClientEmail = "";
    private string _ContactEmail = "";
    private string _NoReplyEmail = "";
    private string _FinancialIntitutions = "";
    private string _DirectMerchants = "";

    public string ClientName
    {
        get { return _ClientName; }
        set { _ClientName = value; }
    }
    public string Address1
    {
        get { return _Address1; }
        set { _Address1 = value; }
    }
    public string Address2
    {
        get { return _Address2; }
        set { _Address2 = value; }
    }
    public string Zip
    {
        get { return _Zip; }
        set { _Zip = value; }
    }
    public string Phone
    {
        get { return _Phone; }
        set { _Phone = value; }
    }
    public string Fax
    {
        get { return _Fax; }
        set { _Fax = value; }
    }
    public string ClientEmail
    {
        get { return _ClientEmail; }
        set { _ClientEmail = value; }
    }
    public string ContactEmail
    {
        get { return _ContactEmail; }
        set { _ContactEmail = value; }
    }
    public string NoReplyEmail
    {
        get { return _NoReplyEmail; }
        set { _NoReplyEmail = value; }
    }
    public string FinancialIntitutions
    {
        get { return _FinancialIntitutions; }
        set { _FinancialIntitutions = value; }
    }
    public string DirectMerchants
    {
        get { return _DirectMerchants; }
        set { _DirectMerchants = value; }
    }
}
