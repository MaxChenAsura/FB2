<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0210_Qry.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0210_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ymd").mask("9999/99/99");

            $("#txt_DEPT_NO").change(function () {
                $("#txt_DEPT_NO").val($("#txt_DEPT_NO").val().toUpperCase());
            });

            //部門代號取得部門名稱的ajax
            $('#txt_DEPT_NAME').attr("readonly", true);
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptAllData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });
            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"

            });
        }


        //清空畫面
        function ClearAll() {
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_START_DT_S").val("");
            $("#txt_START_DT_E").val("");
            $("#txt_END_DT_S").val("");
            $("#txt_END_DT_E").val("");
            $('#rbl_IS_VALID').find("input[value='Y']").prop("checked", "checked");
            $("#ddl_DEPT_LEVEL").val("-1");
        }


        function CheckDeptNo() {
            var processed = true;
            if ($("#txt_DEPT_NO").val() == "") {
                alert("請輸入上層部門代號!!");
                processed = false;
            }

            return processed;
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert($('#hid_chooseOneMessage').val());
                return false;
            }
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                var rtnval = confirm($('#hid_delete_ConfirmMessage').val());
                if (rtnval) BlockUI();
                return rtnval;
            }
            else {
                alert($('#hid_notChooseMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("#gv_result [type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="28%" />
                    <col width="12%" />
                    <col width="23%" />
                    <col width="25%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <%-- 上層部門代號 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_UP_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hid_DEPT_NO" ClientIDMode="Static" runat="server" />
                            <input id="Button14" type="button" value="..." onclick="OpenSearch('DeptGrid_Search.aspx', 'txt_DEPT_NO', 'txt_DEPT_NAME', '');" />
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="140px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_LEVEL" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_DEPT_LEVEL" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            <asp:HiddenField ID="hid_DEPT_LEVEL" ClientIDMode="Static" runat="server" />
                        </td>
                        <td align="left" class="Body_label"></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_start_dt%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            ~
                            <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            <asp:HiddenField ID="hid_START_DT_S" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hid_START_DT_E" ClientIDMode="Static" runat="server" />
                            <asp:CustomValidator ID="cv_START_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cv_START_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT_S"
                                ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_EFFECT_DT_RANGE3%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <%--結束日期--%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_end_dt%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_END_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            ~
                                <asp:TextBox ID="txt_END_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_END_DT_S"
                                ControlToValidate="txt_END_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_EFFECT_END_DT_RANGE3%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <td align="left" class="Body_label">
                            <asp:RadioButtonList ID="rbl_IS_VALID" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                <asp:ListItem Text="<%$Resources:Resource,wfb2ha_lb_IS_VALID_Y%>" Value="Y" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2ha_lb_IS_VALID_N%>" Value="N"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2ha_lb_IS_VALID_ALL%>" Value="ALL"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:HiddenField ID="hid_IS_VALID" ClientIDMode="Static" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="5">
                            <div id="init">

                                <aces:Btn ID="WFB2HA0210Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Search%>" OnClick="WFB2HA0210Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                <aces:Btn ID="WFB2HA0102Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Search%>" OnClick="WFB2HA0210Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"/>
                                <aces:Btn ID="WFB2HA0202Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Search%>" OnClick="WFB2HA0210Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"/>
                                <%--<asp:Button ID="WFB2HA0210Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Search%>" OnClick="WFB2HA0210Search_Click" OnClientClick="CheckValid();" />--%>
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ha_btn_clear%>" onclick="ClearAll();" />

                                <asp:HiddenField ID="hid_cancel_ConfirmMessage" ClientIDMode="Static" runat="server" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
                                <asp:HiddenField ID="hid_delete_ConfirmMessage" ClientIDMode="Static" runat="server" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
                                <asp:HiddenField ID="hid_notChooseMessage" ClientIDMode="Static" runat="server" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
                                <asp:HiddenField ID="hid_chooseOneMessage" ClientIDMode="Static" runat="server" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%>" />
                            </div>
                        </td>
                    </tr>


                    <!-- end: Create MODULE ID -->
                    <!-- START: Create a line to separate Search field with body field -->
                    <tr>
                        <td align="center" height="1" colspan="5">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="5">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2HA0210Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Add%>" OnClick="WFB2HA0210Add_Click" OnClientClick="return CheckDeptNo();" />
                                <aces:Btn ID="WFB2HA0210Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0210Edit_Click" Visible="false" />
                                <aces:Btn ID="WFB2HA0210Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA0210Delete_Click" Visible="false" />
                                <aces:Btn ID="WFB2HA0210Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Save%>" Visible="false" OnClick="WFB2HA0210Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                                <aces:Btn ID="WFB2HA0102Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Add%>" OnClick="WFB2HA0210Add_Click" OnClientClick="return CheckDeptNo();" />
                                <aces:Btn ID="WFB2HA0102Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0210Edit_Click" Visible="false" />
                                <aces:Btn ID="WFB2HA0102Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA0210Delete_Click" Visible="false" />
                                <aces:Btn ID="WFB2HA0102Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Save%>" Visible="false" OnClick="WFB2HA0210Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                                <aces:Btn ID="WFB2HA0202Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Add%>" OnClick="WFB2HA0210Add_Click" OnClientClick="return CheckDeptNo();" />
                                <aces:Btn ID="WFB2HA0202Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0210Edit_Click" Visible="false" />
                                <aces:Btn ID="WFB2HA0202Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA0210Delete_Click" Visible="false" />
                                <aces:Btn ID="WFB2HA0202Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Save%>" Visible="false" OnClick="WFB2HA0210Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                                <%-- <asp:Button ID="WFB2HA0210Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Add%>" OnClick="WFB2HA0210Add_Click" OnClientClick="return CheckDeptNo();" />
                                <asp:Button ID="WFB2HA0210Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0210Edit_Click" Visible="false" />
                                <asp:Button ID="WFB2HA0210Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Save%>" Visible="false" OnClick="WFB2HA0210Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"/>--%>
                                <asp:Button ID="WFB2HA0210Cancel" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Cancel%>" Visible="false" OnClick="WFB2HA0210Cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val())" />
                            </div>
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0210DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_START_DT_S" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_s" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_START_DT_E" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_e" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="txt_END_DT_S"
                        Name="end_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_END_DT_E"
                        Name="end_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_IS_VALID" DefaultValue=""
                        Name="is_valid" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_DEPT_LEVEL" DefaultValue=""
                        Name="dept_level" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2ha_RowNumber%>" HeaderStyle-Width="30px" ReadOnly="true" />
                    <asp:BoundField DataField="DEPT_LEVEL_DESC" HeaderText="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL%>" SortExpression="DEPT_LEVEL" HeaderStyle-Width="70px" ReadOnly="true" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2ha_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="40px" ReadOnly="true" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2ha_DEPT_NAME%>" SortExpression="DEPT_NAME" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" ReadOnly="true" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2ha_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" ReadOnly="true" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_START_DT" runat="server" Style="display: none;" ReadOnly="true" Text='<%#Bind("START_DT")%>' ClientIDMode="Static"></asp:TextBox>
                            <asp:TextBox ID="txt_EDIT_END_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="AutoID" CssClass="MandatoryField date ymd" Text='<%#Bind("END_DT")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ValidEndDt" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_END_DT%>"
                                ControlToValidate="txt_EDIT_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_EDIT_END_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_EDIT_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT"
                                ControlToValidate="txt_EDIT_END_DT" ErrorMessage="<%$Resources:Resource,wfb2ha_Start_End_Date%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UP_DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2ha_UP_DEPT_NAME%>" SortExpression="UP_DEPT_NAME" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" ReadOnly="true" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_REMARK" runat="server" MaxLength="210" Width="300px" Text='<%#Bind("REMARK")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">

                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_parentFuncID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
