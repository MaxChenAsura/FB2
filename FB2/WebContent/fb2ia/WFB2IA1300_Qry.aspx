<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA1300_Qry.aspx.cs" Inherits="WebContent_WFB2IA1300_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99')
            $(".empid").mask('99999')
            gridviewScroll();
            $.unblockUI();

            //寫在這，按查詢才不會消失
            $('#txt_COMPANY_SNAME').attr("readonly", true);
            //公司代號取得公司名稱的ajax
            $("#txt_COMPANY_CD").change(function () {
                if ($("#txt_COMPANY_CD").val().length == 1) {
                    $.ajax({
                        url: "../comm/WFB2CompanyData.ashx",
                        data: {
                            COMPANY_CD: $('#txt_COMPANY_CD').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_COMPANY_SNAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_COMPANY_SNAME').val(JData.COMPANY_SNAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_COMPANY_SNAME').val("");
                }
            });

            $('#txt_EMP_NAME').attr("readonly", true);
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });

            $('#txt_SUB_DESC').attr("readonly", true);
            //工號取得姓名的ajax
            $("#txt_NATION_CD").change(function () {
                if ($("#txt_NATION_CD").val().length == 3) {
                    $.ajax({
                        url: "../commgeo/WFB2GetCommData.ashx",
                        data: {
                            SUB_CD: $('#txt_NATION_CD').val()
                            , SYS_CD: 'HB'
                            , MAIN_CD: 'NATION_CD'
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                alert(JData.errMsg);
                                $('#txt_SUB_DESC').val("");
                            }
                            else {
                                $('#txt_SUB_DESC').val(JData.SUB_DESC);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_SUB_DESC').val("");
                }
            });
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                headerrowcount: 2
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#txt_COMPANY_CD").val("");
            $("#txt_COMPANY_SNAME").val("");
            $("#txt_SALARY_SYM").val("");
            $("#txt_SALARY_EYM").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_NATION_CD").val("");
            $("#txt_SUB_DESC").val("");
            $("#txt_EFFECT_DT").val("");
            $("#txt_LICENSE_ID").val("");
            $('#<%=rb_is_EFFECTED.ClientID %>').find("input[value=1]").prop("checked", "");
            $('#<%=rb_is_EFFECTED.ClientID %>').find("input[value=0]").prop("checked", true);
            //$("#rb_is_EFFECTED").attr('checked', false);

            return false;
        }

        //刪除,檢查是否有勾選
        function doDelete() {
            BlockUI();
            var cnt = checkboxsSelected();
            //if (cnt == 0 || cnt > 1) {   //只能選取一筆,或必項勾選
            if (cnt == 0 ) {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            } else {

                var grid = document.getElementById("<%=gv_result.ClientID %>");
                var ItemCheckBoxs = $("[type=checkbox]");
                var cellchg_status;

                if (grid.rows.length > 0) {
                    for (i = 1; i <= grid.rows.length - 1; i++) {
                        if (ItemCheckBoxs[i + 2].checked) {
                            if (confirm($('#hidwfb299_Del_ConfirmMessage').val()))
                                return true;
                            else {
                                $.unblockUI();
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
        }

        //修改,檢查是否有勾選
        function doUpdate() {
            if (Page_ClientValidate("GroupB")) {
                BlockUI();
                return true;
                /*var cnt = checkboxsSelected();
                
                if (cnt == 0 ) {
                    alert($('#hidwfb299_Mod_NotChoiceMessage').val());
                    $.unblockUI();
                    return false;
                
                } else {
                    return true;
                }*/
            }
            else {
                return false;
            }
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                    if (choietr == null)
                        choietr = i;
                }
            }
            return HaveCheck;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
        <tr height="100%" valign="top">
            <td>
                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                    <colgroup>
                        <col width="10%" />
                        <col width="30%" />
                        <col width="10%" />
                        <col width="60%" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_COMPANY_CD%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="1" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_Company_CD%>"
                                    ControlToValidate="txt_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <input id="bt_COMPANY_CD_SEARCH" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD', 'txt_COMPANY_SNAME', '', '');" />
                                <asp:TextBox ID="txt_COMPANY_SNAME" MaxLength="60" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_SALARY_YM2%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_SALARY_SYM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date2"></asp:TextBox>
                                ~ 
                                    <asp:TextBox ID="txt_SALARY_EYM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_SALARY_SYM%>"
                                    ControlToValidate="txt_SALARY_SYM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_SALARY_EYM%>"
                                    ControlToValidate="txt_SALARY_EYM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_error_DATA_YM_SDT%>" ControlToValidate="txt_SALARY_SYM" ForeColor="Red"
                                    ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_error_DATA_YM_EDT%>" ControlToValidate="txt_SALARY_EYM" ForeColor="Red"
                                    ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_SALARY_SYM"
                                    ControlToValidate="txt_SALARY_EYM" ErrorMessage="<%$Resources:Resource,wfb2sk_greaterthan_DATA_YM%>" Type="String" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ib_EMP_ID%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5" CssClass="empid"> </asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sl_EMP_ID_importError%>" ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d]+$" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <%-- <th align="left" class="Body_TableHeader">國籍別:</th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_NATION_CD" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                    <input id="bt_NATION_SEARCH" type="button" value="..." onclick="OpenSearch('CommCode_Search.aspx', 'txt_NATION_CD', 'txt_SUB_DESC', 'SYS_CD=HB&MAIN_CD=NATION_CD');" />
                                    <asp:TextBox ID="txt_SUB_DESC" MaxLength="30" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                        ErrorMessage="國籍別只能輸入英數字" ControlToValidate="txt_NATION_CD" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                </td>--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EFFECT_DT" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_EXECUTIVE_DATE%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EFFECT_DT" runat="server" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_EFFECT_DATE%>" ControlToValidate="txt_EFFECT_DT" ForeColor="Red"
                                    ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_LICENSE_ID" MaxLength="20" runat="server" ClientIDMode="Static"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID_isError%>" ControlToValidate="txt_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_is_EFFECTED" runat="server" Text="生效否"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_is_EFFECTED" runat="server" ClientIDMode="Static" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0" Selected="True">否</asp:ListItem>                        
                            </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th></th>
                            <th></th>
                            <th></th>
                            <td align="right" class="Body_label">
                                <div id="init">
                                     
                                    <aces:Btn ID="WFB2IA1300Detail" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300Detail%>" OnClick="WFB2IA1300Detail_Click" />
                                    <aces:Btn ID="WFB2IA1300Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Search%>" OnClientClick="CheckValid();" ValidationGroup="GroupA" OnClick="WFB2IA1300Search_Click" />
                                    
                                   <%-- <asp:Button ID="WFB2IA1300Detail" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300Detail%>" OnClick="WFB2IA1300Detail_Click" />
                                    <asp:Button ID="WFB2IA1300Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Search%>" OnClientClick="CheckValid();" ValidationGroup="GroupA" OnClick="WFB2IA1300Search_Click" />--%>
                                    <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2df_btn_clear%>" OnClientClick="return ClearAll();" CausesValidation="false" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    <asp:Table ID="tbEdit" runat="server" Width="1020" border="0" CellSpacing="0" CellPadding="0" bgcolor="#FFFFFF" Height="100%" Visible="false">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" CssClass="Body_label">
                <asp:Label ID="lb_DEF_EFFECT_DT" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_DEF_EFFECT_DT%>"></asp:Label>:
                <asp:TextBox ID="txt_DEF_EFFECT_DT" runat="server" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_lb_DEF_EFFECT_DT_isNull%>"
                    ControlToValidate="txt_DEF_EFFECT_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                    ErrorMessage="<%$Resources:Resource,wfb2ia_lb_DEF_EFFECT_DT_isError%>" ControlToValidate="txt_DEF_EFFECT_DT" ForeColor="Red"
                    ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupB">
                </asp:RegularExpressionValidator>
                <aces:Btn ID="WFB2IA1300Update" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300Update%>" OnClick="WFB2IA1300Update_Click" ValidationGroup="GroupB" CausesValidation="true" OnClientClick="return doUpdate();" />
                <%--<asp:Button ID="WFB2IA1300Update" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300Update%>" OnClick="WFB2IA1300Update_Click" ValidationGroup="GroupB" CausesValidation="true" OnClientClick="return doUpdate();" />--%>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right" CssClass="Body_label">
                <aces:Btn ID="WFB2IA1300Del" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990101Delete%>" OnClick="WFB2IA1300Del_Click" CausesValidation="false" OnClientClick="return doDelete();" />
                <%--<asp:Button ID="WFB2IA1300Del" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990101Delete%>" OnClick="WFB2IA1300Del_Click" CausesValidation="false" OnClientClick="return doDelete();" />--%>
                <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
        SelectCountMethod="getCount" TypeName="CFB2IA1300DAO" EnablePaging="True"
        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
        StartRowIndexParameterName="startRowIndex"
        OnSelected="ods1_Selected">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:ControlParameter ControlID="txt_COMPANY_CD"
                Name="company_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="txt_SALARY_SYM"
                Name="salary_sym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="txt_SALARY_EYM"
                Name="salary_eym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="txt_EMP_ID"
                Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <%-- <asp:ControlParameter ControlID="txt_NATION_CD"
                    Name="nation_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />--%>
            <asp:ControlParameter ControlID="txt_EFFECT_DT"
                Name="effect_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="txt_LICENSE_ID"
                Name="license_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
            <asp:ControlParameter ControlID="rb_is_EFFECTED" DefaultValue="" ConvertEmptyStringToNull="false"
                Name="is_effected" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
        OnRowDataBound="gv_result_RowDataBound" Width="1020px"
        OnRowCreated="gv_result_RowCreated"
        OnPageIndexChanging="gv_result_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderStyle-Width="20px">
                <HeaderTemplate>
                    <asp:CheckBox ID='cb_all' runat='server' onclick='javascript:SelectAllCheckboxes(this);' ClientIDMode='Static' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_RowNumber%>" HeaderStyle-Width="40px">
                <ItemTemplate>
                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                </EditItemTemplate>
            </asp:TemplateField>
            <%--聘用單位--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hc_lb_COMPANY_CD%>" SortExpression="COMPANY_SNAME" HeaderStyle-Width="60px">
                <ItemTemplate>
                    <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="60px"></asp:Label>
                    <asp:HiddenField ID="hid_COMPANY_CD" runat="server" Value='<%#Bind("COMPANY_CD")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <%--工號--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px">
                <ItemTemplate>
                    <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--姓名--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px">
                <ItemTemplate>
                    <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="60px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--國籍名稱--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_NATION_NAME%>" SortExpression="NATION_NAME" HeaderStyle-Width="80px">
                <ItemTemplate>
                    <asp:Label ID="lb_NATION_NAME" runat="server" Text='<%#Bind("NATION_NAME")%>' Width="80px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--員工區分--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_SUB_DESC%>" SortExpression="EMP_CD_NAME" HeaderStyle-Width="80px">
                <ItemTemplate>
                    <asp:Label ID="lb_EMP_CD_NAME" runat="server" Text='<%#Bind("EMP_CD_NAME")%>' Width="80px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--身分證/居留證--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px">
                <ItemTemplate>
                    <asp:Label ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>' Width="100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--平均薪資--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_AVG_SALARY%>" SortExpression="AVG_SALARY" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lb_AVG_SALARY" runat="server" Text='<%#Bind("AVG_SALARY","{0:N0}")%>' Width="80px"></asp:Label>
                    <asp:HiddenField ID="hid_SALARY_SYM" runat="server" Value='<%#Bind("SALARY_SYM")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <%--原投保金額--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_B_OLD_INSAMT%>" SortExpression="A_OLD_INSAMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lb_A_OLD_INSAMT" runat="server" Text='<%#Bind("A_OLD_INSAMT","{0:N0}")%>' Width="100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--新投保金額--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_B_NEW_INSAMT%>" SortExpression="A_NEW_INSAMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lb_A_NEW_INSAMT" runat="server" Text='<%#Bind("A_NEW_INSAMT","{0:N0}")%>' Width="100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--原提繳工資--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_C_OLD_INSAMT%>" SortExpression="C_OLD_INSAMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lb_C_OLD_INSAMT" runat="server" Text='<%#Bind("C_OLD_INSAMT","{0:N0}")%>' Width="100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--新提繳工資--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_C_NEW_INSAMT%>" SortExpression="C_NEW_INSAMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lb_C_NEW_INSAMT" runat="server" Text='<%#Bind("C_NEW_INSAMT","{0:N0}")%>' Width="100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--原投保金額--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_B_OLD_INSAMT%>" SortExpression="B_OLD_INSAMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lb_B_OLD_INSAMT" runat="server" Text='<%#Bind("B_OLD_INSAMT","{0:N0}")%>' Width="100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--新投保金額--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_B_NEW_INSAMT%>" SortExpression="B_NEW_INSAMT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lb_B_NEW_INSAMT" runat="server" Text='<%#Bind("B_NEW_INSAMT","{0:N0}")%>' Width="100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--生效日期--%>
            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_START_DT%>" SortExpression="EFFECT_DT" HeaderStyle-Width="100px">
                <ItemTemplate>
                    <asp:Label ID="lb_EFFECT_DT" runat="server" Text='<%#Bind("EFFECT_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="GridviewScrollPager" />
        <FooterStyle CssClass="GridviewScrollPager" />
    </asp:GridView>
    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
</asp:Content>

