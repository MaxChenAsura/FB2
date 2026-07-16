<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2si/WFB2SI0100_Qry.aspx.cs" Inherits="WebContent_fb2si_WFB2SI0100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.year').mask('9999');
            $(".ymd").mask('9999/99/99');

            gridviewScroll();
            $.unblockUI();

        }
        function adddefault() {
            var year = new Date().getFullYear() - 1;
            var sdt = new Date().getFullYear() - 1 + "/04/01";
            var edt = new Date().getFullYear() + "/03/31";
            $("#HID_DEFAULT_BONUS_YEAR").val(year);
            $("#HID_DEFAULT_BONUS_SDT").val(sdt);
            $("#HID_DEFAULT_BONUS_EDT").val(edt);

        }
        function addcheck() {

            $("#HID_NEW_BONUS_YEAR").val($("#txt_NEW_BONUS_YEAR").val());
            $("#HID_NEW_BONUS_SDT").val($("#txt_NEW_BONUS_SDT").val());
            $("#HID_NEW_BONUS_EDT").val($("#txt_NEW_BONUS_EDT").val());
            $("#HID_NEW_BONUS_DT").val($("#txt_NEW_BONUS_DT").val());
            //alert($("#HID_NEW_DATA_YM").val());

        }


        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hid_wfb2si_Del_NotChoiceMessage  ').val());
            else {
                alert($('#hid_wfb2si_Del_ConfirmMessage').val());
                return false;
            }
        }
        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_wfb2si_Mod_NotChoiceMessage').val());
                return false;
            }
        }
        //檢核對象生成
        function CheckExecute() {
            var processed = false;

            if (LookUpCheckboxs() == 1) {
                if ($('#HID_CHECK_COUNT').val() == "0") {
                    if (confirm($('#hid_wfb2si_Execute_confirmMessage').val()))
                        processed = true;
                    else
                        processed = false;
                }
                //確定要進行對象生成！[!!!注意：己維護資料會被覆蓋]
                if (confirm($('#hid_wfb2si_Execute_checkMessage').val()))
                    processed = true;
                else
                    processed = false;
            } else {
                alert($('#hid_wfb2si_Execute_NotChoiceMessage').val());
            }
            BlockUI();
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }
        //檢核提出核可
        function CheckRelease() {
            var processed = false;
            if (LookUpCheckboxs() == 1) {
                choietr = choietr - 2;
                //對象生成
                if ($("[id=hid_target_gen_dt]")[choietr].value == "") {
                    alert($('#hid_wfb2si_Announce_NotExecuteMessage').val());
                    processed = false;
                }
                    //核可作業
                else if ($("[id=hid_approve_status]")[choietr].value == "Y") {
                    alert($('#hid_wfb2si_Announce_ReleaseMessage').val());
                    processed = false;
                }
                    //紅利計算生成日
                else if ($("[id=hid_gen_dt]")[choietr].value == "") {
                    alert($('#hid_wfb2si_Gen_dt_NotReleaseMessage').val());
                    processed = false;
                }
                else {
                    if (confirm($('#hid_wfb2si_Release_ConfirmMessage').val()))
                        processed = true;
                    else
                        processed = false;
                    choietr = null;
                }
            }
            else {
                alert($('#hid_wfb2si_Release_NotChoiceMessage').val());
                choietr = null;
                processed = false;
            }
            BlockUI();
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }
        //檢核薪資轉出
        function CheckAnnounce() {
            var processed = false;
            if (LookUpCheckboxs() == 1) {
                choietr = choietr - 2;
                //對象生成
                if ($("[id=hid_target_gen_dt]")[choietr].value == "") {
                    alert($('#hid_wfb2si_Announce_NotExecuteMessage').val());
                    processed = false;
                }
                    //核可作業
                else if ($("[id=hid_approve_status]")[choietr].value != "Y") {
                    alert($('#hid_wfb2si_Announce_NotReleaseMessage').val());
                    processed = false;
                }
                else {
                    if (confirm($('#hid_wfb2si_Announce_Message').val()))
                        processed = true;
                    else
                        processed = false;
                    choietr = null;
                }
            }
            else {
                alert($('#hid_wfb2si_Announce_NotChoiceMessage').val());
                choietr = null;
                processed = false;
            }
            BlockUI();
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }
        var choietr = null;
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            choietr = null;
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    if (choietr == null)
                        choietr = i;
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        //全選
        function mySelectAllCheckboxes(spanChk) {
            if (spanChk.checked == false) {
                $(':checkbox[id$=cb_check]').prop('checked', false);
                return;
            }
            $(":checkbox[id$=cb_check]").each(function (index, ele) {
                if ($(this).prop("disabled") == false) {
                    //console.log("tt");
                    $(this).prop('checked', true);
                }

            });
        }
        function CheckYM(source, arguments) {
            if ($("#txt_Year_DT_S").val() != "" && $("#txt_Year_DT_E").val() != "") {
                if ($("#txt_Year_DT_E").val() < $("#txt_Year_DT_S").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }
        //清空
        function doClear() {
            $("#txt_Year_DT_S").val("");
            $("#txt_Year_DT_E").val("");
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="5%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="20%" />

                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_YEAR" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_YEAR%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_Year_DT_S" runat="server" MaxLength="4" Width="64px" CssClass="year" ClientIDMode="Static" Text=""></asp:TextBox>~
                                            <asp:TextBox ID="txt_Year_DT_E" runat="server" MaxLength="4" Width="64px" CssClass="year" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_YEAR_start%>" ControlToValidate="txt_Year_DT_S" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_YEAR_end%>" ControlToValidate="txt_Year_DT_E" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_greaterthan_BONUS_YEAR%>" ClientValidationFunction="CheckYM" ForeColor="Red"
                                            ControlToValidate="txt_Year_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_SDT" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_SDT%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <uc1:UCDateTimeRange runat="server" ID="UCDateTimeRange" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10" EndDateCssClass="ymd" StartDateCssClass="ymd"
                                            CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="<%$Resources:Resource,wfb2si_greaterthan_BONUS_SDT%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2si_error_BONUS_SDT_start%>"
                                            ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2si_error_BONUS_SDT_end%>" ClientValidationFunctionE="CheckDate" ClientValidationFunctionS="CheckDate" ValidationGroup="GroupA" />
                                    </td>
                                </tr>

                                <%-- <tr>
                                    <td colspan="4">
                                        <hr />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">

                                            <aces:Btn ID="WFB2SI0100Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SI0100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2si_clear%>" OnClientClick="return doClear();" />

                                            <%--
                                            <asp:Button ID="WFB2SI0100Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SI0100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2si_clear%>" OnClientClick="return doClear();" />
                                            --%>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">

                        <div id="init_grid">

                            <aces:Btn ID="WFB2SI0100Add" runat="server" Text="<%$Resources:Resource,wfb2si_add%>" OnClick="WFB2SI0100Add_Click" Visible="true" OnClientClick="adddefault(); BlockUI(); " />
                            <aces:Btn ID="WFB2SI0100Delete" runat="server" Text="<%$Resources:Resource,wfb2si_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SI0100Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2SI0100Edit" runat="server" Text="<%$Resources:Resource,wfb2si_edit%>" OnClick="WFB2SI0100Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <aces:Btn ID="WFB2SI0100Execute" runat="server" Text="<%$Resources:Resource,wfb2si_execute%>" OnClick="WFB2SI0100Execute_Click" OnClientClick="return CheckExecute();" Visible="false" />
                            <aces:Btn ID="WFB2SI0100Release" runat="server" Text="<%$Resources:Resource,wfb2si_release%>" OnClick="WFB2SI0100Release_Click" OnClientClick="return CheckRelease();" Visible="false" />
                            <aces:Btn ID="WFB2SI0100Announce" runat="server" Text="<%$Resources:Resource,wfb2si_announce%>" OnClick="WFB2SI0100Announce_Click" OnClientClick="return CheckAnnounce();" Visible="false" />
                            <aces:Btn ID="WFB2SI0100OK" runat="server" Text="<%$Resources:Resource,wfb2si_ok%>" Visible="false" OnClick="WFB2SI0100OK_Click" OnClientClick="addcheck(); CheckValid();" ValidationGroup="GroupB" />

                            <%--
                            <asp:Button ID="WFB2SI0100Add" runat="server" Text="<%$Resources:Resource,wfb2si_add%>" OnClick="WFB2SI0100Add_Click" Visible="true" OnClientClick="adddefault(); BlockUI(); " />
                            <asp:Button ID="WFB2SI0100Delete" runat="server" Text="<%$Resources:Resource,wfb2si_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SI0100Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2SI0100Edit" runat="server" Text="<%$Resources:Resource,wfb2si_edit%>" OnClick="WFB2SI0100Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <asp:Button ID="WFB2SI0100Execute" runat="server" Text="<%$Resources:Resource,wfb2si_execute%>" OnClick="WFB2SI0100Execute_Click" OnClientClick="return CheckExecute();" Visible="false" />
                            <asp:Button ID="WFB2SI0100Release" runat="server" Text="<%$Resources:Resource,wfb2si_release%>" OnClick="WFB2SI0100Release_Click" OnClientClick="return CheckRelease();" Visible="false" />
                            <asp:Button ID="WFB2SI0100Announce" runat="server" Text="<%$Resources:Resource,wfb2si_announce%>" OnClick="WFB2SI0100Announce_Click" OnClientClick="return CheckAnnounce();" Visible="false" />
                            <asp:Button ID="WFB2SI0100OK" runat="server" Text="<%$Resources:Resource,wfb2si_ok%>" Visible="false" OnClick="WFB2SI0100OK_Click" OnClientClick="addcheck(); CheckValid();" ValidationGroup="GroupB" />
                            --%>
                            <asp:Button ID="btn_cancel" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wfb2si_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click"  />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SI0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_Year_DT_S" DefaultValue=""
                        Name="bonus_year_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_Year_DT_E" DefaultValue=""
                        Name="bonus_year_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                        Name="bonus_sdt" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                        Name="bonus_edt" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="mySelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>

                        <ItemTemplate>
                            <div style="text-align: center; width: 20px">
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                            </div>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width:35px">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 35px">
                                <asp:Label ID="lb_RowNumber" runat="server" Width="35px" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Width="35px" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_YEAR%>" SortExpression="BONUS_YEAR" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: center;" ID="lb_BONUS_YEAR" runat="server" Text='<%#Bind("BONUS_YEAR")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="40px" Style="text-align: center;" ID="lb_BONUS_YEAR" runat="server" Text='<%#Bind("BONUS_YEAR")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 40px">
                                <asp:TextBox Width="40px" ID="txt_NEW_BONUS_YEAR" runat="server" MaxLength="4" Text='<%#this.HID_DEFAULT_BONUS_YEAR.Value%>' CssClass="MandatoryField year" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_YEAR%>"
                                    ControlToValidate="txt_NEW_BONUS_YEAR" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_YEAR%>" ControlToValidate="txt_NEW_BONUS_YEAR" ForeColor="Red" ValidationGroup="GroupB"
                                    ValidationExpression="(19|20)\d\d" Display="None"></asp:RegularExpressionValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_SDT_start%>" SortExpression="BONUS_SDT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_BONUS_SDT" runat="server" Text='<%#Bind("BONUS_SDT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_BONUS_SDT" runat="server" Text='<%#Bind("BONUS_SDT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100px">
                                <asp:TextBox Width="100px" ID="txt_NEW_BONUS_SDT" runat="server" MaxLength="10" Text='<%#this.HID_DEFAULT_BONUS_SDT.Value%>' ClientIDMode="Static" CssClass="MandatoryField date ymd"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_SDT_start%>"
                                    ControlToValidate="txt_NEW_BONUS_SDT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_SDT_start1%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_BONUS_SDT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_SDT_end%>" SortExpression="BONUS_EDT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_BONUS_EDT" runat="server" Text='<%#Bind("BONUS_EDT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_BONUS_EDT" runat="server" Text='<%#Bind("BONUS_EDT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100px">
                                <asp:TextBox Width="100px" ID="txt_NEW_BONUS_EDT" runat="server" MaxLength="10" Text='<%#this.HID_DEFAULT_BONUS_EDT.Value%>' ClientIDMode="Static" CssClass="MandatoryField date ymd"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_SDT_end%>"
                                    ControlToValidate="txt_NEW_BONUS_EDT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_BONUS_EDT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_BONUS_SDT"
                                    ControlToValidate="txt_NEW_BONUS_EDT" ErrorMessage="<%$Resources:Resource,wfb2si_greaterthan_BONUS_SDT%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupB"></asp:CompareValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_DT%>" SortExpression="BONUS_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_BONUS_DT" runat="server" Text='<%#Bind("BONUS_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                            <asp:HiddenField ID="hid_gen_dt" runat="server" ClientIDMode="Static" Value='<%#Bind("GEN_DT")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100px">
                                <asp:TextBox ID="txt_EDIT_BONUS_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" Text='<%#Bind("BONUS_DT", "{0:yyyy/MM/dd}")%>' CssClass="MandatoryField date ymd"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_DT%>"
                                    ControlToValidate="txt_EDIT_BONUS_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest3" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_EDIT_BONUS_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100px">
                                <asp:TextBox Width="100px" ID="txt_NEW_BONUS_DT" runat="server" MaxLength="10" ClientIDMode="Static" CssClass="MandatoryField date ymd"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_DT%>"
                                    ControlToValidate="txt_NEW_BONUS_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest4" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_BONUS_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_TARGET_GEN_DT%>" SortExpression="TARGET_GEN_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_TARGET_GEN_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("TARGET_GEN_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                            <asp:HiddenField ID="hid_target_gen_dt" runat="server" ClientIDMode="Static" Value='<%#Bind("TARGET_GEN_DT")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_TARGET_GEN_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("TARGET_GEN_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_RELEASE_DT%>" SortExpression="RELEASE_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_APPROVE_DT%>" SortExpression="APPROVE_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                            <asp:HiddenField ID="hid_approve_status" runat="server" ClientIDMode="Static" Value='<%#Bind("APPROVE_STATUS")%>' />

                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_SALARY_TRANS_DT%>" SortExpression="SALARY_TRANS_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_SALARY_TRANS_DT" runat="server" Text='<%#Bind("SALARY_TRANS_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100px" Style="text-align: center;" ID="lb_SALARY_TRANS_DT" runat="server" Text='<%#Bind("SALARY_TRANS_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_FREEZE_FLAG%>" SortExpression="FREEZE_FLAG" Visible="false" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_FREEZE_FLAG" runat="server" Text='<%#Bind("FREEZE_FLAG")%>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_FREEZE_FLAG" runat="server" Text='<%#Bind("FREEZE_FLAG")%>' Visible="false"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_ROUND%>" SortExpression="BONUS_ROUND" Visible="false" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_BONUS_ROUND" runat="server" Text='<%#Bind("BONUS_ROUND")%>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_BONUS_ROUND" runat="server" Text='<%#Bind("BONUS_ROUND")%>' Visible="false"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="明細功能" HeaderStyle-Width="100px">
                        <ItemTemplate>

                            <aces:Btn ID="WFB2SI0100Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2si_Dtl%>" />
                            <%--
                            <asp:Button ID="WFB2SI0100Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2si_Dtl%>" />
                            --%>
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>

                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                            </td>
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2si_YEAR%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_SDT_start%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_SDT_end%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2si_TARGET_GEN_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2si_RELEASE_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2si_SALARY_TRANS_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="明細功能"></asp:Label>
                            </td>

                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lb_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_BONUS_YEAR" runat="server" MaxLength="4" Width="35px" CssClass="MandatoryField year" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_YEAR%>"
                                    ControlToValidate="txt_NEW_BONUS_YEAR" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_YEAR%>" ControlToValidate="txt_NEW_BONUS_YEAR" ForeColor="Red" ValidationGroup="GroupB"
                                    ValidationExpression="(19|20)\d\d" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_BONUS_SDT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date ymd"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_SDT_start%>"
                                    ControlToValidate="txt_NEW_BONUS_SDT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest5" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_SDT_start1%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_BONUS_SDT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_BONUS_EDT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date ymd"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_BONUS_SDT"
                                    ControlToValidate="txt_NEW_BONUS_EDT" ErrorMessage="<%$Resources:Resource,wfb2si_greaterthan_BONUS_SDT%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupB"></asp:CompareValidator>
                                <asp:CustomValidator ID="qrytest6" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_BONUS_EDT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_BONUS_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date ymd"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_BONUS_DT%>"
                                    ControlToValidate="txt_NEW_BONUS_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest7" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_BONUS_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>

                        </tr>
                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DEFAULT_BONUS_YEAR" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DEFAULT_BONUS_SDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DEFAULT_BONUS_EDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_BONUS_YEAR" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_BONUS_SDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_BONUS_EDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_BONUS_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_CHECK_COUNT" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2si_Announce_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Announce_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Announce_NotExecuteMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Announce_NotExecuteMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Announce_NotReleaseMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Announce_NotReleaseMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Announce_Message" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Announce_Message%>" />
            <asp:HiddenField ID="hid_wfb2si_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Del_ConfirmMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Del_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Del_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Mod_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Announce_ReleaseMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Announce_ReleaseMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Gen_dt_NotReleaseMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Gen_dt_NotReleaseMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Release_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Release_ConfirmMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Release_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Release_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Execute_checkMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Execute_checkMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Execute_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Execute_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2si_Execute_confirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2si_Execute_confirmMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

