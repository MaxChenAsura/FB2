<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0700_Qry.aspx.cs" Inherits="WebContent_fb2dc_WFB2DC0700_Qry" Culture="auto" UICulture="auto" %>
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

            $("#txt_PERSON_DC_NAME").attr("readonly", true);
            $("#txt_PERSON_NAME").attr("readonly", true);
            $("#txt_CLOCK_DESC2").attr("readonly", true);
            gridviewScroll();
            $.unblockUI();
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

        //清空畫面
        function ClearAll() {
            $("#txt_CLOCK_DT_S").val("");
            $("#txt_CLOCK_DT_E").val("");
            $("#ddl_CARD_CHECK_STATUS").val("-1");
            $("#txt_PERSON_DC").val("");
            $("#txt_PERSON_DC_NAME").val("");
            $("#txt_PERSON_ID").val("");
            $("#txt_PERSON_NAME").val("");
            $("#ddl_CARD_TYPE").val("-1");
            $("#txt_CLOCK_NO").val("");
            $("#txt_CLOCK_DESC2").val("");
            $('#rbl_PERSON_TYPE').find("input[value='1']").prop("checked", "checked");
            setDefaultData();
        }

        function OpenPERSON_DC() {
            var ddl = $("#rbl_PERSON_TYPE input:radio:checked").val();
            if (ddl == '1') {
                var json = OpenDeptSearch('txt_PERSON_DC', 'txt_PERSON_DC_NAME', "Y");
                //var json = OpenSearch('DeptGrid_Search.aspx', 'txt_PERSON_DC', 'txt_PERSON_DC_NAME', '');
                //if (json != undefined) {
                //    $("#txt_PERSON_DC").val(json.CD);
                //    $("#txt_PERSON_DC_NAME").val(json.DESC);
                //}
            } else {
                var json = OpenSearch('Vendor_Search.aspx', 'txt_PERSON_DC', 'txt_PERSON_DC_NAME', '');
                //if (json != undefined) {
                //    $("#txt_PERSON_DC").val(json.CD);
                //    $("#txt_PERSON_DC_NAME").val(json.DESC);
                //}
            }
        }

        function OpenPERSON_ID() {
            var is_super = $("#hid_is_super").val();
            var ddl = $("#rbl_PERSON_TYPE input:radio:checked").val();
            if (ddl == '1') {
                OpenEmpSearch('txt_PERSON_ID', 'txt_PERSON_NAME', is_super);
                //var json = OpenEmpSearch('txt_PERSON_ID', 'txt_PERSON_NAME', is_super);
                //if (json != undefined) {
                //    $("#txt_PERSON_ID").val(json.EMP_ID);
                //    $("#txt_PERSON_NAME").val(json.EMP_NAME);
                //}
            } else {
                OpenSearch('Vendor_d_Search.aspx', 'txt_PERSON_ID', 'txt_PERSON_NAME', '');
                //var json = OpenSearch('Vendor_d_Search.aspx', 'txt_PERSON_ID', 'txt_PERSON_NAME', '');
                //if (json != undefined) {
                //    $("#txt_PERSON_ID").val(json.CD);
                //    $("#txt_PERSON_NAME").val(json.DESC);
                //}
            }
        }




        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            } else {
                $("#txt_PERSON_NAME").val("");
            }
        }

        function getPERSON_DC_NAME(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getPERSON_DC_NAME').click();
                return false;
            } else {
                $("#txt_PERSON_DC_NAME").val("");
            }
        }

        function getCLOCK_DESC(CLOCK_id) {
            var txt = document.getElementById(CLOCK_id);
            if (txt.value != "") {
                document.getElementById('hid_getCLOCK_DESC').click();
                return false;
            } else {
                $("#txt_CLOCK_DESC2").val("");
            }
        }
        //給予查詢條件預設值
        function setDefaultData() {
            $("#txt_CLOCK_DT_S").val($("#hid_defalut_DT_S").val());
            $("#txt_CLOCK_DT_E").val($("#hid_defalut_DT_E").val());
            $("#txt_PERSON_DC").val($("#hid_defalut_dept_no").val());
            $("#txt_PERSON_DC_NAME").val($("#hid_defalut_dept_name").val());
            $("#txt_PERSON_ID").val($("#hid_defalut_emp_id").val());
            $("#txt_PERSON_NAME").val($("#hid_defalut_emp_name").val());

        }


        function checkDowning() {
            if (Page_ClientValidate("GroupA")) {

            }
            else {
                return false;
            }

            BlockUI();
            return true;
        }

        //查詢前檢核
        function CheckQryCondition() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                processed = true;
            } else {
                processed = false;
            }
            var errMsg = "";
            if (processed) {
                var startDT = new Date($("#txt_CLOCK_DT_S").val());
                var endDT = new Date($("#txt_CLOCK_DT_E").val());
                var diffDay = (endDT - startDT) / 86400000;
                if (diffDay > 365) {
                    errMsg += "起迄日不可大於1年 \r\n"
                    //alert("起迄日不可大於1年");
                    processed = false;
                }
                if ($.trim($("#txt_PERSON_DC").val()) == "" && $.trim($("#txt_PERSON_ID").val()) == "") {
                    errMsg += "「部門/廠商別」與「工號/廠商人員編號」二擇一必輸入 \r\n"
                    processed = false;
                }

                if (errMsg != "") {
                    alert(errMsg);
                    processed = false;
                    return false;
                }
            }
            BlockUI();
            if (!processed)
                $.unblockUI();
            return processed;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="25%" />
                                <col width="15%" />
                                <col width="25%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CLOCK_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_DT%>"></asp:Label>:
                                    </th>
                                    <%-- 查詢期間 --%>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CLOCK_DT_S" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="100px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_CLOCK_DT_E" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" Width="100px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DT_S%>"
                                            ControlToValidate="txt_CLOCK_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DT_E%>"
                                            ControlToValidate="txt_CLOCK_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorCLOCK_DT_S" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DATE_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_CLOCK_DT_S" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorCLOCK_DT_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DATE_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_CLOCK_DT_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_CLOCK_DT_S"
                                            ControlToValidate="txt_CLOCK_DT_E" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CLOCK_DATE_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CARD_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_CHECK_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CARD_CHECK_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--部門代號/廠商別 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PERSON_DC" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_DC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:RadioButtonList ID="rbl_PERSON_TYPE" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                            <%-- 
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_PERSON%>" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_COMPANY%>" Value="2"></asp:ListItem>
                                            --%>
                                        </asp:RadioButtonList>
                                        <asp:TextBox ID="txt_PERSON_DC" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" onblur="javascript:getPERSON_DC_NAME(this.id)"></asp:TextBox>
                                        <input id="bt_PPERSON_DC" type="button" value="..." onclick="OpenPERSON_DC();" />
                                        <asp:TextBox ID="txt_PERSON_DC_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PERSON_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>"></asp:Label>:
                                    </th>
                                    <%-- 工號/廠商 --%>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PERSON_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" onblur="javascript:getEmpName(this.id)"></asp:TextBox>
                                        <input id="bt_PERSON_ID" type="button" value="..." onclick="OpenPERSON_ID();" />
                                        <asp:TextBox ID="txt_PERSON_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CARD_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CARD_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CLOCK_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CLOCK_NO" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" onblur="javascript:getCLOCK_DESC(this.id)"></asp:TextBox>
                                        <input id="btn_CLOCK_NO" type="button" value="..." onclick="OpenSearch('Clock_Search.aspx', 'txt_CLOCK_NO', 'txt_CLOCK_DESC2', '');" />
                                        <asp:TextBox ID="txt_CLOCK_DESC2" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DC0700Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0700Search%>" ValidationGroup="GroupA" OnClientClick="return CheckQryCondition();" OnClick="WFB2DC0700Search_Click" />
                                            
                                            <%--<asp:Button ID="WFB2DC0700Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DC0700Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />
                                            
                                        <aces:Btn ID="WFB2DC0700CardImport" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0700CardImport%>" OnClick="WFB2DC0700CardImport_Click" OnClientClick="BlockUI();" />
                                            <aces:Btn ID="WFB2DC0700Export" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0700Export%>" OnClick="WFB2DC0700Export_Click" OnClientClick="return CheckQryCondition();" />
                                            
<%--                                            <asp:Button ID="WFB2DC0700CardImport" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0700CardImport%>" OnClick="WFB2DC0700CardImport_Click" />
                                            <asp:Button ID="WFB2DC0700Export" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0700Export%>" OnClick="WFB2DC0700Export_Click" OnClientClick="return checkDowning();" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData" 
                SelectCountMethod="getCount" TypeName="CFB2DC0700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_CLOCK_DT_S"
                        Name="clock_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_CLOCK_DT_E"
                        Name="clock_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_CARD_CHECK_STATUS" DefaultValue=""
                        Name="card_check_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="rbl_PERSON_TYPE" DefaultValue=""
                        Name="person_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_PERSON_DC"
                        Name="person_dc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_PERSON_ID"
                        Name="person_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_CARD_TYPE" DefaultValue=""
                        Name="card_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_CLOCK_NO"
                        Name="clock_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_is_super"
                        Name="is_super" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_is_dept"
                        Name="is_dept" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_departments"
                        Name="departments" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound"  Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--卡鐘編號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_NO%>" SortExpression="CLOCK_NO" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_NO" runat="server" Text='<%#Bind("CLOCK_NO")%>' Width="200px" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--卡號 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_NO%>" SortExpression="CARD_NO" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_NO" runat="server" Text='<%#Bind("CARD_NO")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--刷卡時間 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_DT2%>" SortExpression="CLOCK_DT" HeaderStyle-Width="160px" ItemStyle-Width="160px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_DT" runat="server" Text='<%#Bind("CLOCK_DT", "{0:yyyy/MM/dd HH:mm:ss}")%>' width="160px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PERSON_DC%>"  HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_DC" runat="server" Text='<%#Bind("PERSON_DC")%>' width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>" SortExpression="PERSON_ID" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_ID" runat="server" Text='<%#Bind("PERSON_ID")%>' Width="70px" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_PERSON_NAME2%>" SortExpression="PERSON_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSON_NAME" runat="server" Text='<%#Bind("PERSON_NAME")%>' Width="80px" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_RETURN_STATUS%>" SortExpression="CLOCK_RETURN_STATUS" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLOCK_RETURN_STATUS" runat="server" Text='<%#Bind("CLOCK_RETURN_STATUS")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_CHECK_STATUS%>" SortExpression="CARD_CHECK_STATUS" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_CHECK_STATUS" runat="server" Text='<%#Bind("CARD_CHECK_STATUS")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />

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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_dept" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_departments" runat="server" ClientIDMode="Static" />

             <asp:HiddenField ID="hid_defalut_emp_id" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hid_defalut_emp_name" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hid_defalut_dept_no" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hid_defalut_dept_name" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hid_defalut_DT_S" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="hid_defalut_DT_E" runat="server" ClientIDMode="Static" />


            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
            <asp:Button ID="hid_getPERSON_DC_NAME" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getPERSON_DC_NAME_Click" />
            <asp:Button ID="hid_getCLOCK_DESC" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getCLOCK_DESC_Click" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DC0700Export" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>

