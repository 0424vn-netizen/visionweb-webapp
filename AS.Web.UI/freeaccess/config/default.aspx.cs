using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using AS.Common.DBManager;
using System.Collections;

public partial class freeaccess_config_default : Page
{
 
    enum DataBindAction
    {
        BindClients,
        BindHierarchyList,
        BindDrilldownList,
        BindHierarchyForDrilldownDropdown,
        BindUserModes,
        BindPositionDropdownInGrid,
        BindPositionList,
        BindHierarchyForAHFDropdown,
        BindHierarchyForMMWDropdown,
        BindAHFList,
        BindMMWList
    }
    enum PostBackAction
    {
        ClientListSelected,
        AddHierarchy,
        UpdateHierarchy,
        DeleteHierarchy,
        DrilldownHierarchySelected,
        UserModeSelected,
        ChangePositionSelected,
        SelectHierarchyForAHF,
        EditHierarchyForAHF,
        RemoveHierarchyFromAHF,
        SelectHierarchyForMMW,
        EditHierarchyForMMW,
        RemoveHierarchyFromMMW,
        PositionGroupChanged
    }

    protected void OnPostBackActions(Enum type, object sender)
    {
        FilterParameterCollection inParames = new FilterParameterCollection();
        FilterParameterCollection outParames = new FilterParameterCollection();
        switch ((PostBackAction)type)
        {
            case PostBackAction.ClientListSelected:
                {
                    #region
                    uxHierarchyList.Rebind();
                    uxDrillDownHierarchyList.Rebind();
                    uxAssHierarchyFilterList.Rebind();
                    uxMMWHierarchyList.Rebind();
                    OnDataBindControls(DataBindAction.BindUserModes);
                    uxPositionHierarchyList.Rebind();
                    #endregion
                }
                break;
            case PostBackAction.UpdateHierarchy:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;

                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, e.Item as GridEditableItem);
                    int hierarchyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyID"];

                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));

                    inParames.Add(new FilterParameter("@HierarchyName", newValues["HierarchyName"], DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyMode", newValues["HierarchyMode"], DbType.String));

                    inParames.Add(new FilterParameter("@EntityType", newValues["EntityType"], DbType.Int32));

                    inParames.Add(new FilterParameter("@HierarchyLevel", newValues["HierarchyLevel"], DbType.Int32));

                    inParames.Add(new FilterParameter("@MinLength", newValues["MinLength"], DbType.Int32));

                    inParames.Add(new FilterParameter("@MaxLength", newValues["MaxLength"], DbType.Int32));

                    inParames.Add(new FilterParameter("@ValidateExpression", newValues["ValidateExpression"], DbType.String));
                    var val = "";
                    if (newValues["ValidInput"] != null)
                    {
                        val = newValues["ValidInput"].ToString().ToLower();
                        if (val != "true" && val != "false") val = null;
                    }
                    else val = null;
                    inParames.Add(new FilterParameter("@ValidInput", val, DbType.Boolean));

                    inParames.Add(new FilterParameter("@MaxLenMsg", newValues["MaxLenMsg"], DbType.String));

                    inParames.Add(new FilterParameter("@MinLenMsg", newValues["MinLenMsg"], DbType.String));

                    inParames.Add(new FilterParameter("@InvalidMsg", newValues["InvalidMsg"], DbType.String));

                    inParames.Add(new FilterParameter("@IsMerchant", newValues["IsMerchant"], DbType.Boolean));
                    val = "false";
                    if (newValues["ShowPrefix"] != null)
                    {
                        val = newValues["ShowPrefix"].ToString().ToLower();
                        if (val != "true" && val != "false") val = "false";
                    }
                    inParames.Add(new FilterParameter("@ShowPrefix", val, DbType.Boolean));

                    inParames.Add(new FilterParameter("@HierarchyGridName", newValues["HierarchyGridName"], DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyPrefix", newValues["HierarchyPrefix"], DbType.String));
                    inParames.Add(new FilterParameter("@BEProcessor", newValues["BEProcessor"], DbType.String));
                    inParames.Add(new FilterParameter("@UserMode", newValues["UserMode"], DbType.String));
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_UpdateHierarchyInfo", inParames, out outParames);
                    uxDrillDownHierarchyList.Rebind();
                    uxAssHierarchyFilterList.Rebind();
                    uxMMWHierarchyList.Rebind();
                    OnDataBindControls(DataBindAction.BindUserModes);
                    uxPositionHierarchyList.Rebind();
                    #endregion
                }
                break;
            case PostBackAction.AddHierarchy:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;

                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues,  e.Item as GridEditableItem);


                    inParames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));

                    inParames.Add(new FilterParameter("@HierarchyName", newValues["HierarchyName"], DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyMode", newValues["HierarchyMode"], DbType.String));

                    inParames.Add(new FilterParameter("@EntityType", newValues["EntityType"], DbType.Int32));

                    inParames.Add(new FilterParameter("@HierarchyLevel", newValues["HierarchyLevel"], DbType.Int32));

                    inParames.Add(new FilterParameter("@MinLength", newValues["MinLength"], DbType.Int32));

                    inParames.Add(new FilterParameter("@MaxLength", newValues["MaxLength"], DbType.Int32));

                    inParames.Add(new FilterParameter("@ValidateExpression", newValues["ValidateExpression"], DbType.String));
                    var val = "";
                    if (newValues["ValidInput"] != null)
                    {
                        val = newValues["ValidInput"].ToString().ToLower();
                        if (val != "true" && val != "false") val = null;
                    }
                    else val = null;
                    inParames.Add(new FilterParameter("@ValidInput", val, DbType.Boolean));

                    inParames.Add(new FilterParameter("@MaxLenMsg", newValues["MaxLenMsg"], DbType.String));

                    inParames.Add(new FilterParameter("@MinLenMsg", newValues["MinLenMsg"], DbType.String));

                    inParames.Add(new FilterParameter("@InvalidMsg", newValues["InvalidMsg"], DbType.String));
                    inParames.Add(new FilterParameter("@IsMerchant", newValues["IsMerchant"], DbType.Boolean));

                    val = "false";
                    if (newValues["ShowPrefix"] != null)
                    {
                        val = newValues["ShowPrefix"].ToString().ToLower();
                        if (val != "true" && val != "false") val = "false";
                    }

                    inParames.Add(new FilterParameter("@ShowPrefix", val, DbType.Boolean));

                    inParames.Add(new FilterParameter("@HierarchyGridName", newValues["HierarchyGridName"], DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyPrefix", newValues["HierarchyPrefix"], DbType.String));

                    inParames.Add(new FilterParameter("@BEProcessor", newValues["BEProcessor"], DbType.String));

                    inParames.Add(new FilterParameter("@UserMode", newValues["UserMode"], DbType.String));
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_InsertHierarchyInfo", inParames, out outParames);
                    uxDrillDownHierarchyList.Rebind();
                    uxAssHierarchyFilterList.Rebind();
                    uxMMWHierarchyList.Rebind();
                    OnDataBindControls(DataBindAction.BindUserModes);
                    uxPositionHierarchyList.Rebind();
                    #endregion
                }
                break;

            case PostBackAction.DeleteHierarchy:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;
                    int hierarchyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyID"];
                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_DeleteHierarchyInfo", inParames, out outParames);
                    uxDrillDownHierarchyList.Rebind();
                    uxAssHierarchyFilterList.Rebind();
                    uxMMWHierarchyList.Rebind();
                    OnDataBindControls(DataBindAction.BindUserModes);
                    uxPositionHierarchyList.Rebind();
                    #endregion
                }
                break;
            case PostBackAction.PositionGroupChanged:
                {
                    #region
                    RadTextBox uxPositionGroup = (RadTextBox)sender;
                    Control container = uxPositionGroup.Parent;

                    HiddenField hd = (HiddenField)container.FindControl("uxSelectedHierarchyID");
                    int hierarchyID = int.Parse(hd.Value);
                    inParames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    inParames.Add(new FilterParameter("@UserMode", uxUserModes.SelectedValue, DbType.String));
                    inParames.Add(new FilterParameter("@PositionGroup", int.Parse(uxPositionGroup.Text), DbType.Int32));

                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_UpdatePositionHierarchyInfo", inParames, out outParames);
                    #endregion
                }
                break;
            case PostBackAction.ChangePositionSelected:
                {
                    #region
                    RadComboBox combo = (RadComboBox)sender;
                    Control container = combo.Parent;

                    HiddenField hd = (HiddenField)container.FindControl("uxSelectedHierarchyID");
                    RadTextBox uxPositionGroup = (RadTextBox)container.FindControl("uxPositionGroup");
                    int hierarchyID = int.Parse(hd.Value);
                    inParames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    inParames.Add(new FilterParameter("@UserMode", uxUserModes.SelectedValue, DbType.String));
                    if(uxPositionGroup.Text.IsNullOrEmpty())
                        inParames.Add(new FilterParameter("@PositionGroup", 0, DbType.Int32));
                    else
                        inParames.Add(new FilterParameter("@PositionGroup", int.Parse(uxPositionGroup.Text), DbType.Int32));

                    switch (combo.ID)
                    {
                        case "uxPreHierarchies":
                            inParames.Add(new FilterParameter("@PreID", int.Parse(combo.SelectedValue), DbType.Int32));
                            break;
                        case "uxNextHierarchies":
                            inParames.Add(new FilterParameter("@NextID", int.Parse(combo.SelectedValue), DbType.Int32));
                            break;

                    }
                    
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_UpdatePositionHierarchyInfo", inParames, out outParames);
                    #endregion
                }
                break;

            case PostBackAction.DrilldownHierarchySelected:
                {
                    #region
                    RadComboBox combo = (RadComboBox)sender;
                    HiddenField hd = (HiddenField)combo.Parent.FindControl("uxDrilldownHierarchyID");
                    int hierarchyID = int.Parse(hd.Value);
                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    inParames.Add(new FilterParameter("@NextHierarchyID", int.Parse(combo.SelectedValue), DbType.Int32));
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_UpdateDrilldownHierarchyInfo", inParames, out outParames);
                    #endregion
                }
                break;
            case PostBackAction.EditHierarchyForAHF:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;

                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, e.Item as GridEditableItem);
                    int hierarchyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyID"];

                    inParames.Add(new FilterParameter("@DataText", newValues["DataText"], DbType.String));

                    inParames.Add(new FilterParameter("@DataKey", newValues["DataKey"], DbType.String));

                    inParames.Add(new FilterParameter("@SourceTable", newValues["SourceTable"], DbType.String));

                    inParames.Add(new FilterParameter("@MIFEntity", newValues["MIFEntity"], DbType.String));
                    inParames.Add(new FilterParameter("@SortSeq", newValues["SortSeq"], DbType.Int32));

                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_UpdateAssignmentHierarchy", inParames, out outParames);
                    #endregion
                }
                break;
            case PostBackAction.EditHierarchyForMMW:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;

                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, e.Item as GridEditableItem);
                    int hierarchyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyID"];

                    inParames.Add(new FilterParameter("@HierarchyGridHeaderText", newValues["HierarchyGridHeaderText"], DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyHeaderTooltip", newValues["HierarchyHeaderTooltip"], DbType.String));

                    inParames.Add(new FilterParameter("@FilterTypeText", newValues["FilterTypeText"], DbType.String));

                    inParames.Add(new FilterParameter("@FilterValueTextColumn", newValues["FilterValueTextColumn"], DbType.String));

                    inParames.Add(new FilterParameter("@MWEntity", newValues["MWEntity"], DbType.String));

                    inParames.Add(new FilterParameter("@MIFEntity", newValues["MIFEntity"], DbType.String));

                    inParames.Add(new FilterParameter("@MinCharactersSearch", newValues["MinCharactersSearch"], DbType.Int32));

                    inParames.Add(new FilterParameter("@SortSeq", newValues["SortSeq"], DbType.Int32));
                    inParames.Add(new FilterParameter("@DataText", newValues["DataText"], DbType.String));
                    inParames.Add(new FilterParameter("@DataKey", newValues["DataKey"], DbType.String));
                    inParames.Add(new FilterParameter("@SourceTable", newValues["SourceTable"], DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_UpdateMMWHierarchy", inParames, out outParames);
                    
                    #endregion
                }
                break;
            case PostBackAction.RemoveHierarchyFromAHF:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;
                    int hierarchyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyID"];
                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_DeleteAssignmentHierarchy", inParames, out outParames);
                    #endregion
                }
                break;
            case PostBackAction.RemoveHierarchyFromMMW:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;
                    int hierarchyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyID"];
                    inParames.Add(new FilterParameter("@HierarchyID", hierarchyID, DbType.Int32));
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_DeleteMMWHierarchy", inParames, out outParames);
                    #endregion
                }
                break;
            case PostBackAction.SelectHierarchyForAHF:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;
                    RadComboBox uxHierarchies = (RadComboBox)e.Item.FindControl("uxHierarchies");

                    inParames.Add(new FilterParameter("@DataText", null, DbType.String));

                    inParames.Add(new FilterParameter("@DataKey", null, DbType.String));

                    inParames.Add(new FilterParameter("@SourceTable", null, DbType.String));

                    inParames.Add(new FilterParameter("@MIFEntity", null, DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyID", int.Parse(uxHierarchies.SelectedValue), DbType.Int32));


                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_InsertAssignmentHierarchy", inParames, out outParames);
                    uxAssHierarchyFilterList.Rebind();
                    #endregion
                }
                break;
            case PostBackAction.SelectHierarchyForMMW:
                {
                    #region
                    GridCommandEventArgs e = (GridCommandEventArgs)sender;
                    RadComboBox uxHierarchies = (RadComboBox)e.Item.FindControl("uxHierarchies");

                    inParames.Add(new FilterParameter("@HierarchyGridHeaderText", null, DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyGridDataField", null, DbType.String));

                    inParames.Add(new FilterParameter("@HierarchyHeaderTooltip", null, DbType.String));

                    inParames.Add(new FilterParameter("@FilterTypeText", null, DbType.String));

                    inParames.Add(new FilterParameter("@FilterValueTextColumn", null, DbType.String));

                    inParames.Add(new FilterParameter("@MWEntity", null, DbType.String));

                    inParames.Add(new FilterParameter("@MIFEntity", null, DbType.String));

                    inParames.Add(new FilterParameter("@MinCharactersSearch", null, DbType.Int32));

                    inParames.Add(new FilterParameter("@SortSeq", null, DbType.Int32));

                    inParames.Add(new FilterParameter("@HierarchyID", int.Parse(uxHierarchies.SelectedValue), DbType.Int32));


                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_config_InsertMMWHierarchy", inParames, out outParames);
                    uxMMWHierarchyList.Rebind();
                    #endregion
                }
                break;
            case PostBackAction.UserModeSelected:
                {
                    #region
                    uxPositionHierarchyList.Rebind();
                    #endregion
                }
                break;
        }
    }  
    protected void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindClients:
                {
                    uxClients.DataSource = WebServices.CsReportServices.GetReports("spa_config_GetClients", parames);
                    uxClients.DataBind();
                }
                break;
            case DataBindAction.BindHierarchyList:
                {

                    uxHierarchyList.DataSource = AllHierarchies;
                }
                break;
            case DataBindAction.BindDrilldownList:
                {
                    parames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                    uxDrillDownHierarchyList.DataSource = WebServices.CsReportServices.GetReports("spa_config_GetDrilldownHierarchyInfos", parames);
                }
                break;
            case DataBindAction.BindAHFList:
                {
                    parames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                    uxAssHierarchyFilterList.DataSource = WebServices.CsReportServices.GetReports("spa_config_GetAssignmentHierarchies", parames);
                }
                break;
            case DataBindAction.BindMMWList:
                {
                    parames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                    uxMMWHierarchyList.DataSource = WebServices.CsReportServices.GetReports("spa_config_GetMMWHierarchies", parames);
                }
                break;
            case DataBindAction.BindHierarchyForDrilldownDropdown:
                {
                    GridDataItem item = sender as GridDataItem;
                    RadComboBox uxDrilldownItems = (RadComboBox)item.FindControl("uxDrilldownItems");
                    
                    uxDrilldownItems.DataSource = AllHierarchies;
                    uxDrilldownItems.DataBind();
                    uxDrilldownItems.Items.Insert(0, new RadComboBoxItem("\u00A0", "-1"));
                    uxDrilldownItems.SelectedValue = DataBinder.Eval(item.DataItem, "NextHierarchyID").ToString();
                }
                break;
            case DataBindAction.BindUserModes:
                {
                    parames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                    uxUserModes.DataSource = WebServices.CsReportServices.GetReports("spa_config_GetUserModes", parames); 
                    uxUserModes.DataBind();
                    uxUserModes.Items.Insert(0, new RadComboBoxItem("CS/CLIENT", "CS"));
                }

                break;

            case DataBindAction.BindHierarchyForAHFDropdown:
                {
                    GridCommandItem item = sender as GridCommandItem;
                    RadComboBox uxHierarchies = (RadComboBox)item.FindControl("uxHierarchies");
                    parames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                    uxHierarchies.DataSource = WebServices.CsReportServices.GetReports("spa_config_GetUnselectedAssignmentHierarchies", parames);
                    uxHierarchies.DataBind();
                    
                    RadButton uxAddHierarchy = (RadButton)item.FindControl("uxAddHierarchy");
                    uxAddHierarchy.Enabled = uxHierarchies.Items.Count > 0;
                }

                break;//
            case DataBindAction.BindHierarchyForMMWDropdown:
                {
                    GridCommandItem item = sender as GridCommandItem;
                    RadComboBox uxHierarchies = (RadComboBox)item.FindControl("uxHierarchies");
                    parames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                    uxHierarchies.DataSource = WebServices.CsReportServices.GetReports("spa_config_GetUnselectedMMWHierarchies", parames);
                    uxHierarchies.DataBind();

                    RadButton uxAddHierarchy = (RadButton)item.FindControl("uxAddHierarchy");
                    uxAddHierarchy.Enabled = uxHierarchies.Items.Count > 0;
                }

                break;
            case DataBindAction.BindPositionList:
                {
                    uxPositionHierarchyList.DataSource = UserModeHierarchies;
                }

                break;
            case DataBindAction.BindPositionDropdownInGrid:
                {
                    GridDataItem item = sender as GridDataItem;
                    RadComboBox uxHierarchyItems ;
  
                    uxHierarchyItems = (RadComboBox)item.FindControl("uxPreHierarchies");
                    uxHierarchyItems.DataSource = UserModeHierarchies;
                    uxHierarchyItems.DataBind();
                    uxHierarchyItems.Items.Insert(0, new RadComboBoxItem("\u00A0", "-1"));
                    uxHierarchyItems.SelectedValue = DataBinder.Eval(item.DataItem, "PreviousHierarchyID").ToString();

                    uxHierarchyItems = (RadComboBox)item.FindControl("uxNextHierarchies");
                    uxHierarchyItems.DataSource = UserModeHierarchies;
                    uxHierarchyItems.DataBind();
                    uxHierarchyItems.Items.Insert(0, new RadComboBoxItem("\u00A0", "-1"));
                    uxHierarchyItems.SelectedValue = DataBinder.Eval(item.DataItem, "NextHierarchyID").ToString();
                }
                break;
                
        }
    }
    #region Never edit this
    DataTable _fullHierarcies = null;
    DataTable AllHierarchies
    {
        get
        {
            if (_fullHierarcies == null)
            {
                FilterParameterCollection parames = new FilterParameterCollection();
                parames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                parames.AddLanguageID();
                _fullHierarcies = WebServices.CsReportServices.GetReports("spa_config_GetHierarchyInfos", parames);
            }
            return _fullHierarcies;
        }

    }

    DataTable _userModeHierarcies = null;
    DataTable UserModeHierarchies
    {
        get
        {
            if (_userModeHierarcies == null)
            {
                FilterParameterCollection parames = new FilterParameterCollection();
                parames.Add(new FilterParameter("@ASClientID", int.Parse(uxClients.SelectedValue), DbType.Int32));
                parames.Add(new FilterParameter("@UserMode", uxUserModes.SelectedValue, DbType.String));                
                _userModeHierarcies = WebServices.CsReportServices.GetReports("spa_config_GetHierarchyInfosByUserMode", parames);
            }
            return _userModeHierarcies;
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.MaintainScrollPositionOnPostBack = true;
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindClients);
            OnDataBindControls(DataBindAction.BindUserModes);
        }
        
    }

    protected void RadComboBox_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (sender == uxClients)
        {
            OnPostBackActions(PostBackAction.ClientListSelected);
        }
        else if (sender == uxUserModes)
        {
            OnPostBackActions(PostBackAction.UserModeSelected);
        }
        else
        {
            switch (((RadComboBox)sender).ID)
            {
                case "uxDrilldownItems":
                    {
                        OnPostBackActions(PostBackAction.DrilldownHierarchySelected, sender);
                    }
                    break;
                case "uxPreHierarchies":
                case "uxNextHierarchies":
                    {
                        OnPostBackActions(PostBackAction.ChangePositionSelected, sender);
                    }
                    break;
            }
        }
    }
    protected void RadTextBox_TextChanged(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.PositionGroupChanged, sender);
    }
    protected void RadGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case GridItemType.Item:
            case GridItemType.AlternatingItem:
                {
                    GridDataItem item = e.Item as GridDataItem;
                    if (sender == uxDrillDownHierarchyList)
                    {
                        OnDataBindControls(DataBindAction.BindHierarchyForDrilldownDropdown, item);
                    }
                    else if (sender == uxPositionHierarchyList)
                    {
                        OnDataBindControls(DataBindAction.BindPositionDropdownInGrid, item);
                    }
                }
                break;
        }
    }

    
    
    protected void RadGrid_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (source == uxHierarchyList)
        {
            switch (e.CommandName)
            {
                case RadGrid.PerformInsertCommandName:
                    OnPostBackActions(PostBackAction.AddHierarchy, e);
                    break;
                case RadGrid.DeleteCommandName:
                    OnPostBackActions(PostBackAction.DeleteHierarchy, e);
                    break;
                case RadGrid.UpdateCommandName:
                    OnPostBackActions(PostBackAction.UpdateHierarchy, e);
                    break;
            }
        }
        else if (source == uxAssHierarchyFilterList)
        {
            switch (e.CommandName)
            {
                case "AddHierarchy":
                    OnPostBackActions(PostBackAction.SelectHierarchyForAHF, e);
                    break;
                case RadGrid.UpdateCommandName:
                    OnPostBackActions(PostBackAction.EditHierarchyForAHF, e);
                    break;
                case RadGrid.DeleteCommandName:
                    OnPostBackActions(PostBackAction.RemoveHierarchyFromAHF, e);
                    break;
            }
        }
        else if (source == uxMMWHierarchyList)
        {
            switch (e.CommandName)
            {
                case "AddHierarchy":
                    OnPostBackActions(PostBackAction.SelectHierarchyForMMW, e);
                    break;
                case RadGrid.UpdateCommandName:
                    OnPostBackActions(PostBackAction.EditHierarchyForMMW, e);
                    break;
                case RadGrid.DeleteCommandName:
                    OnPostBackActions(PostBackAction.RemoveHierarchyFromMMW, e);
                    break;
            }
        }
    } 
 
    protected void RadGrid_ItemCreated(object sender, GridItemEventArgs e)
    {

        switch (e.Item.ItemType)
        {
            case GridItemType.CommandItem:
                {
                    GridCommandItem item = e.Item as GridCommandItem;
                    if (sender == uxAssHierarchyFilterList)
                    {
                        OnDataBindControls(DataBindAction.BindHierarchyForAHFDropdown, item);
                    }
                    else if (sender == uxMMWHierarchyList)
                    {
                        OnDataBindControls(DataBindAction.BindHierarchyForMMWDropdown, item);
                    }
                }
                break;
        }
        
    }
    protected void RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        if (sender == uxHierarchyList)
        {
            OnDataBindControls(DataBindAction.BindHierarchyList);
        }
        else if (sender == uxDrillDownHierarchyList)
        {
            OnDataBindControls(DataBindAction.BindDrilldownList);
        }
        else if (sender == uxPositionHierarchyList)
        {
            OnDataBindControls(DataBindAction.BindPositionList);
        }
        else if (sender == uxAssHierarchyFilterList)
        {
            OnDataBindControls(DataBindAction.BindAHFList);
        }
        else if (sender == uxMMWHierarchyList)
        {
            OnDataBindControls(DataBindAction.BindMMWList);
        }
        
    }
    void ShowMsg(string msg)
    {
        ClientScript.RegisterStartupScript(GetType(), "alert_msg", "alert('" + msg + "');", true);
    }
    protected void OnDataBindControls(Enum type) { OnDataBindControls(type, null); }
    protected void OnPostBackActions(Enum type) { OnPostBackActions(type, null); }
    #endregion
}
