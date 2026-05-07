<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimeSettingDetail.ascx.cs" Inherits="UserControls_TimeSetting" %>
<div class="row">
    <div class="col-xs-8 content-timeseting">
        <asp:Panel ID="pnlSetting" runat="server">
            <span class="text-description">
                <as:Literal ID="Literal8" runat="server" meta:resourcekey="TimerSettingDescription"></as:Literal>
            </span>
            <div class="box">
                <p>
                    <b>
                        <as:Literal ID="Literal1" runat="server" meta:resourcekey="WeekDaySettingsTitle"></as:Literal>
                    </b>
                </p>
                <div class="form-group mb-3x">
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="ml-4x">
                                <as:ValidatorLabel ID="Literal3" ApplyFor="uxNormalDayWorkingHours" runat="server" meta:resourcekey="HoursPerDay"></as:ValidatorLabel>
                            </div>
                        </div>
                        <div class="col-xs-4 control uxSLRNumberValue">
                            <as:RadNumericTextBox Width="100%" ID="uxNormalDayWorkingHours" ClientEvents-OnValueChanged="onChangeWeekEnding" ShowSpinButtons="true" InputType="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" runat="server" />
                        </div>
                        <div class="col-xs-5">
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage1" ApplyFor="uxNormalDayWorkingHours" />
                        </div>
                    </div>

                </div>

                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="ml-4x">
                                <as:ValidatorLabel ID="Literal5" ApplyFor="uxNormalDayStartTime" runat="server" meta:resourcekey="StartingAt"></as:ValidatorLabel>
                            </div>
                        </div>
                        <div class="col-xs-4 control">
                            <tek:RadTimePicker ID="uxNormalDayStartTime" runat="server" ClientEvents-OnDateSelected="onChangeWeekEnding" CssClass="timepicker-border" Width="100%" meta:resourcekey="SelectTimeResource" />
                        </div>
                        <div class="col-xs-5">
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage2" ApplyFor="uxNormalDayStartTime" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="ml-4x">
                                <as:ValidatorLabel ID="ValidatorLabel1" ApplyFor="uxWeekEndingAt" runat="server" meta:resourcekey="EndingAt"></as:ValidatorLabel>
                            </div>
                        </div>
                        <div class="col-xs-4 control">
                            <tek:RadTimePicker ID="uxWeekEndingAt" runat="server" Enabled="false" CssClass="timepicker-border timepicker-endingat" Width="100%" meta:resourcekey="SelectTimeResource" />
                        </div>
                    </div>
                </div>
                <p>
                    <b>
                        <as:Literal ID="Literal2" runat="server" meta:resourcekey="WeekendSettingsTitle"></as:Literal></b>
                </p>
                <div class="form-group mb-3x">
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="ml-4x">
                                <as:ValidatorLabel ID="Literal4" runat="server" ApplyFor="uxWeekendDayWorkingHours" meta:resourcekey="HoursPerDay"></as:ValidatorLabel>
                            </div>
                        </div>
                        <div class="col-xs-4 control uxSLRNumberValue">
                            <as:RadNumericTextBox Width="100%" ID="uxWeekendDayWorkingHours" ClientEvents-OnValueChanged="onChangeWeekendEnding" ShowSpinButtons="true" InputType="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" runat="server" />

                        </div>
                        <div class="col-xs-5">
                            <as:ValidatorMessage ID="ValidatorMessage3" runat="server" ApplyFor="uxWeekendDayWorkingHours" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="ml-4x">
                                <as:ValidatorLabel ID="Literal6" runat="server" ApplyFor="uxWeekendDayStartTime" meta:resourcekey="StartingAt"></as:ValidatorLabel>
                            </div>
                        </div>
                        <div class="col-xs-4 control">
                            <tek:RadTimePicker ID="uxWeekendDayStartTime" runat="server" ClientEvents-OnDateSelected="onChangeWeekendEnding" CssClass="timepicker-border" Width="100%" meta:resourcekey="SelectTimeResource" />

                        </div>
                        <div class="col-xs-5">
                            <as:ValidatorMessage ID="ValidatorMessage4" runat="server" ApplyFor="uxWeekendDayStartTime" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="ml-4x">
                                <as:ValidatorLabel ID="ValidatorLabel2" ApplyFor="uxWeekendEndingAt" runat="server" meta:resourcekey="EndingAt"></as:ValidatorLabel>
                            </div>
                        </div>
                        <div class="col-xs-4 control">
                            <tek:RadTimePicker ID="uxWeekendEndingAt" runat="server" Enabled="false" CssClass="timepicker-border timepicker-endingat" Width="100%" meta:resourcekey="SelectTimeResource" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-3">
                            <b>
                                <as:ValidatorLabel ID="Literal7" CssClass="font-bold" runat="server" ApplyFor="uxTimeZone" meta:resourcekey="TimeZone"></as:ValidatorLabel>
                            </b>
                        </div>
                        <div class="col-xs-4 control">
                            <as:RadComboBox ID="uxTimeZone" runat="server" Width="100%" />

                        </div>
                        <div class="col-xs-5">
                            <as:ValidatorMessage ID="ValidatorMessage5" runat="server" ApplyFor="uxTimeZone" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="checkbox">
                        <as:CheckBox runat="server" ID="uxIsExcludeFederalHolidays" meta:resourcekey="ExcludeFederalHolidays" />
                    </div>
                </div>
                <as:Validator ID="uxCtrValidatorSetting" runat="server" ValidationFunction="validateInputs">
                    <Items>
                        <as:BasicValidationItem ControlToValidateID="uxNormalDayWorkingHours" Rule="Required" ResMessage="Resources.MessageManager.ValidationMessages_Required" />
                        <as:CustomValidationItem ControlToValidateID="uxNormalDayWorkingHours" ClientValidationFunction="ValidateWorkinghours_NormalNagative"  ResMessage="Resources.MessageManager.ValidationMessages_Required" />
                        <as:BasicValidationItem ControlToValidateID="uxNormalDayWorkingHours" Rule="LessOrEqualThan" MaxValue="23" ResMessage="Resources.MessageManager.ValidationMessages_DayWorkingHours1" />
                        <as:BasicValidationItem ControlToValidateID="uxNormalDayWorkingHours" Rule="GreaterOrEqualThan" MinValue="1" ResMessage="Resources.MessageManager.ValidationMessages_DayWorkingHours1" />
                        <as:BasicValidationItem ControlToValidateID="uxWeekendDayWorkingHours" Rule="Required" ResMessage="Resources.MessageManager.ValidationMessages_Required" />
                        <as:BasicValidationItem ControlToValidateID="uxWeekendDayWorkingHours" Rule="LessOrEqualThan" MaxValue="23" ResMessage="Resources.MessageManager.ValidationMessages_WeekendDay" />
                        <as:BasicValidationItem ControlToValidateID="uxNormalDayStartTime" Rule="Required" ResMessage="Resources.MessageManager.ValidationMessages_Required" />
                        <as:CustomValidationItem ControlToValidateID="uxNormalDayStartTime" ClientValidationFunction="ValidateWorkinghours_Normal" ResMessage="Resources.MessageManager.ValidationMessages_DayWorkingHours2" />
                        <as:BasicValidationItem ControlToValidateID="uxWeekendDayStartTime" Rule="Required" ResMessage="Resources.MessageManager.ValidationMessages_Required" />
                        <as:CustomValidationItem ControlToValidateID="uxWeekendDayStartTime" ClientValidationFunction="ValidateWorkinghours_Weekend" ResMessage="Resources.MessageManager.ValidationMessages_DayWorkingHours2" />
						<as:CustomValidationItem ControlToValidateID="uxWeekendDayWorkingHours" ClientValidationFunction="ValidateWorkinghours_WeekendNagative"  ResMessage="Resources.MessageManager.ValidationMessages_Required" />
                        
                    </Items>
                </as:Validator>

            </div>
        </asp:Panel>

        <div class="row">
            <div class="col-xs-12 action-container text-right">
                <asp:Button ID="uxSubmit" runat="server" Text="Submit" CssClass="btn btn-default" OnClientClick="return validateInputs();" OnClick="uxSubmit_Click" meta:resourcekey="Save" />
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    var uxNormalDayWorkingHours_ClientID = '<%= uxNormalDayWorkingHours.ClientID%>';
    var uxNormalDayStartTime_ClientID = '<% = uxNormalDayStartTime.ClientID %>';
    var uxWeekEndingAt_ClientID = '<% = uxWeekEndingAt.ClientID %>';

    var uxWeekendDayWorkingHours_ClientID = '<%= uxWeekendDayWorkingHours.ClientID%>';
    var uxWeekendDayWorkingStartTime_ClientID = '<% = uxWeekendDayStartTime.ClientID %>';
    var uxWeekendEndingAt_ClientID = '<% = uxWeekendEndingAt.ClientID %>';

    function ValidateWorkinghours_Normal() {
        return ValidateWorkinghours(uxNormalDayWorkingHours_ClientID, uxNormalDayStartTime_ClientID);
    }

    function ValidateWorkinghours_NormalRequired() {
        var timeInput = $find(uxNormalDayStartTime_ClientID);
        if (timeInput.get_selectedDate() == null) {
            $("#" + uxNormalDayStartTime_ClientID + "_dateInput").val('');
            return false;
        }
        return true;
    }

    function ValidateWorkinghours_WeekendNagative() {
        var workingHours = $('#'+uxWeekendDayWorkingHours_ClientID);
        if (workingHours.val() < 0) {
            workingHours.val('');
            return false;
        }
        return true;
    }

    function ValidateWorkinghours_NormalNagative() {
        var workingHours = $('#' + uxNormalDayWorkingHours_ClientID);
        if (workingHours.val() < 0) {
            workingHours.val('');
            return false;
        }
        return true;
    }

    function ValidateWorkinghours_WeekendRequired() {
        var timeInput = $find(uxWeekendDayWorkingStartTime_ClientID);   
        if (timeInput.get_selectedDate() == null) {
            $("#" + uxWeekendDayWorkingStartTime_ClientID + "_dateInput").val('');
            return false;
        }
        return true;
    }

    function ValidateWorkinghours_Weekend() {
        return ValidateWorkinghours(uxWeekendDayWorkingHours_ClientID, uxWeekendDayWorkingStartTime_ClientID);
    }

    function ValidateWorkinghours(hourControlID, timeControlID) {
        var hourInput = $find(hourControlID);
        var timeInput = $find(timeControlID);
       
        var result = new Date().setHours(timeInput.get_selectedDate().getHours()
            + parseInt(hourInput.get_value()),
            timeInput.get_selectedDate().getMinutes(),
            timeInput.get_selectedDate().getMilliseconds());

        if (result > new Date().setHours(23, 59, 59)) {
            return false;
        }
        return true;
    }
     
    function onChangeWeekEnding() {
        setEndingAt(uxNormalDayWorkingHours_ClientID, uxNormalDayStartTime_ClientID, uxWeekEndingAt_ClientID);
    }

    function onChangeWeekendEnding() {
        setEndingAt(uxWeekendDayWorkingHours_ClientID, uxWeekendDayWorkingStartTime_ClientID, uxWeekendEndingAt_ClientID);
    }

    function setEndingAt(hourElement, timeElement, setValueElement) {
        var hourInput = $find(hourElement);
        var timeInput = $find(timeElement);
        
        var result = new Date().setHours(timeInput.get_selectedDate().getHours()
            + parseInt(hourInput.get_value()),
            timeInput.get_selectedDate().getMinutes(),
            timeInput.get_selectedDate().getMilliseconds());

        $find(setValueElement).set_selectedDate(new Date(result));
    }

</script>
