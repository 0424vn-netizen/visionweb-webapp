using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AS.Controls.ASP.Net;
using AS.Controls.Global;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using HiddenField = System.Web.UI.WebControls.HiddenField;

public class UserControls_rm_MCF_CustomListBoxItem : GlobalUserControl
{
    #region Constants

    private const string SELECT_ITALIC_ITEM = "select-italic-item";
    private const string UNSELECT_ITEM = "unSelectedItem";

    #endregion

    #region Properties

    private RepeaterItem _repeaterItem;
    public UserControls_rm_MCF_CustomListBoxItem(RepeaterItem rItem)
    {
        _repeaterItem = rItem;
    }

    public UserControls_rm_MCF_CustomListBoxItem() { }

    public object DataItem
    {
        get
        {
            if (_repeaterItem == null) return null;
            return _repeaterItem.DataItem;
        }
    }

    public bool Selected
    {
        get
        {
            return (_repeaterItem.FindControl("uxIsSelected") as CheckBox).Checked;
        }
        set
        {
            (_repeaterItem.FindControl("uxIsSelected") as CheckBox).Checked = value;
        }
    }

    private string _text;

    public string Text
    {
        get
        {
            if (_repeaterItem == null) return _text;
            return ((HtmlGenericControl)_repeaterItem.FindControl("uxLabel")).InnerText;
        }
        set
        {
            if (_repeaterItem == null) _text = value;
            ((HtmlGenericControl)_repeaterItem.FindControl("uxLabel")).InnerText = value.DoVeraCode();
        }
    }

    private string _textEx;
    public string TextEx
    {
        set
        {
            if (_repeaterItem == null) _textEx = value;
            var uxLabel = ((HtmlGenericControl) _repeaterItem.FindControl("uxLabel"));
            var textVal = string.Format("&nbsp<span>{0}</span>", value);
            uxLabel.Controls.Add(new LiteralControl(textVal));
        }
    }

    private string _value;
    public string Value
    {
        get
        {
            if (_repeaterItem == null) return _value;
            return ((HiddenField)_repeaterItem.FindControl("uxValue")).Value;
        }
        set
        {
            if (_repeaterItem == null) _value = value;
            ((HiddenField)_repeaterItem.FindControl("uxValue")).Value = value.DoVeraCode();
        }
    }

    public string CssCanNotSelected
    {
        get
        {
            return ((HtmlGenericControl)_repeaterItem.FindControl("uxLabel")).Attributes["class"];
        }
        set
        {
            ((HtmlGenericControl)_repeaterItem.FindControl("uxLabel")).Attributes.Add("class", value);
        }
    }

    private bool? _selectable;
    public bool? Selectable
    {
        set
        {
            _selectable = value;
            if (_selectable == true)
                CssCanNotSelected = SELECT_ITALIC_ITEM;
            else if (_selectable == false)
                CssCanNotSelected = UNSELECT_ITEM;
        }
        get
        {
            if (CssCanNotSelected == SELECT_ITALIC_ITEM)
                return true;
            else if (CssCanNotSelected == UNSELECT_ITEM)
                return false;
            else
                return _selectable;
        }
    }

    public string DisplayMode { get; set; }

    #endregion
}

public class UserControls_rm_MCF_CustomListBoxItemEventArgs : EventArgs
{
    public UserControls_rm_MCF_CustomListBoxItem Item { get; set; }
}

public partial class UserControls_rm_MCF_CustomListBox : GlobalUserControl
{
    #region Properties

    public bool isAutoPostBack { get; set; }

    public string DataFieldValue { get; set; }

    public string DataFieldText { get; set; }

    private List<UserControls_rm_MCF_CustomListBoxItem> _selectedItems = null;
    public List<UserControls_rm_MCF_CustomListBoxItem> SelectedItems
    {
        get
        {
            if (_selectedItems == null)
            {
                _selectedItems = new List<UserControls_rm_MCF_CustomListBoxItem>();
                foreach (RepeaterItem item in uxRepeater.Items)
                {
                    var lItem = new UserControls_rm_MCF_CustomListBoxItem(item);
                    if (lItem.Selected)
                        _selectedItems.Add(lItem);
                }
            }
            return _selectedItems;
        }
    }

    private List<UserControls_rm_MCF_CustomListBoxItem> _items = null;
    public List<UserControls_rm_MCF_CustomListBoxItem> Items
    {
        get
        {
            if (_items == null)
            {
                _items = new List<UserControls_rm_MCF_CustomListBoxItem>();
                foreach (RepeaterItem item in uxRepeater.Items)
                {
                    _items.Add(new UserControls_rm_MCF_CustomListBoxItem(item));
                }
            }
            return _items;
        }
    }

    public object DataSource
    {
        get
        {
            return uxRepeater.DataSource;
        }
        set
        {
            uxRepeater.DataSource = value;
        }
    }

    #endregion

    #region Override

    public override void DataBind()
    {
        uxRepeater.DataBind();
    }

    #endregion

    #region Events

    public event ItemDataBoundEvent ItemDataBound;
    public delegate void ItemDataBoundEvent(UserControls_rm_MCF_CustomListBox sender, UserControls_rm_MCF_CustomListBoxItemEventArgs e);

    public event ItemCommandEvent ItemCommand;
    public delegate void ItemCommandEvent(object sender, string arg, bool isCheck);

    protected void uxRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        object dataItem = e.Item.DataItem;
        HtmlGenericControl control = (HtmlGenericControl)e.Item.FindControl("uxLabel");

        CheckBox chk = (CheckBox)e.Item.FindControl("uxIsSelected");
        chk.Attributes["CommandName"] = "click";
        chk.Attributes["CommandArgument"] = DataBinder.Eval(dataItem, DataFieldValue).ToString().DoVeraCode();
        chk.AutoPostBack = isAutoPostBack;

        control.InnerText = DataBinder.Eval(dataItem, DataFieldText).ToString().DoVeraCode();
        control.Attributes["for"] = chk.ClientID;


        ((HiddenField)e.Item.FindControl("uxValue")).Value = DataBinder.Eval(dataItem, DataFieldValue).ToString().DoVeraCode();

        HtmlGenericControl li = (HtmlGenericControl)e.Item.FindControl("listItem");

        var row = e.Item.DataItem as DataRowView;
        if (row.Row.Table.Columns.Contains("IsSelected"))
        {
            var isSelected = row["IsSelected"].ToBoolean();
            if (isSelected)
            {
                CheckBox chkSelected = (CheckBox)e.Item.FindControl("uxIsSelected");
                chkSelected.Checked = isSelected;

                li.Attributes["class"] = "rlbItem item rlbSelected";
            }
        }

        if (ItemDataBound != null)
            ItemDataBound(this, new UserControls_rm_MCF_CustomListBoxItemEventArgs() { Item = new UserControls_rm_MCF_CustomListBoxItem(e.Item) });
    }

    protected void uxIsSelected_btn(object sender, EventArgs e)
    {
        var chk = sender as CheckBox;
        var commandAgrument = chk.Attributes["CommandArgument"].ToString();
        if (ItemCommand != null)
            ItemCommand(sender, commandAgrument, chk.Checked);
    }

    #endregion
}