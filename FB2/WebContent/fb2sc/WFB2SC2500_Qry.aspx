<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2500_Qry.aspx.cs" Inherits="WebContent_WFB2SC_WFB2SC2500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".ym").mask('9999/99');
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
            $('#txt_SALARY_DT_S_search').val("");
            $('#txt_SALARY_DT_E_search').val("");
            $('#ddl_SALARY_TYPE_search').val("");
            $("#ddl_APPROVE_STATUS_search").val("");
            $("#txt_PAY_ID").val("");
            return false;
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function CheckApproveAction() {
            if (LookUpCheckboxs() == 1) {
                var datetime = new Date();
                if ($('#txt_REMIT_DT').val() == "") {
                    alert('確定關帳請輸入實際匯款日期!!');
                    return false;

                }
                else {
                    //datecompare = new Date($('#txt_REMIT_DT').val());
                    //if (datecompare < datetime) {
                    //    alert('實際匯款日期不可小於系統日!!');
                    //    return false;
                    //}
                    //else
                    BlockUI();
                    return true;
                }
            }
            else {
                alert('請選取一筆資料!');
                return false;
            }
        }
        function CheckRejectAction() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                alert('請選取一筆資料!');
                return false;
            }
        }

        function changeDT() {
            var count = CheckboxIndex();
            if (count > 1) {
                alert('請選取一筆資料!');
                return false;
            }
            else if (count == 1) {
                for (var i = 0; i < CheckBoxs.length; i++) {
                    if (choietr[i] != null) {
                        var ck = choietr[i] - 2;
                        var date = $("[id=lb_SALARY_DT]")[ck].innerText;
                        $("#txt_REMIT_DT").val(date);
                    }
                }
            }
        }
        //檢查勾了幾個checkbox
        var choietr = [];
        var CheckBoxs = null;
        function CheckboxIndex() {
            CheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < CheckBoxs.length; i++) {
                choietr[i] = null;
                if (CheckBoxs[i].checked && CheckBoxs[i].id.indexOf("cb_all") == -1) {
                    if (choietr[i] == null)
                        choietr[i] = i;
                    HaveCheck++;
                }
            }
            return HaveCheck;
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
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--發薪日期區間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_DT_Range%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT_S_search" runat="server" Width="81px" CssClass="date ymd" ClientIDMode="Static" MaxLength="8"></asp:TextBox>
                                        ~  
                                        <asp:TextBox ID="txt_SALARY_DT_E_search" runat="server" Width="81px" CssClass="date ymd" ClientIDMode="Static" MaxLength="8"></asp:TextBox>
                                        <asp:CustomValidator ID="RegularExpressionValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_S_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT_S_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="RegularExpressionValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_E_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT_E_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_SALARY_DT_S_search"
                                            ControlToValidate="txt_SALARY_DT_E_search" ErrorMessage="<%$Resources:Resource,wfb2_ERR_SALART_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA">
                                        </asp:CompareValidator>
                                    </td>
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--是否關帳--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_STATUS_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_APPROVE_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_APPROVE_STATUS_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--關帳代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_ID" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_ID" runat="server" MaxLength="11" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th align="right">
                                        <aces:Btn ID="WFB2SC2500Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2500Search%>" OnClick="WFB2SC2500Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" CausesValidation="true" />
                                        <%--<asp:Button ID="WFB2SC2500Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2500Search%>" OnClick="WFB2SC2500Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" CausesValidation="true" />--%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2500Clear%>" OnClientClick="return ClearAll();" CausesValidation="false" />
                                    </th>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <td>
                    <table width="1020px" border="0">
                        <colgroup>
                            <col width="64%" />
                            <col width="11%" />
                            <col width="25%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <div id="init_grid">
                                    <td></td>
                                    <th align="left">
                                        <asp:Label ID="lb_wfb2sc_WFB2SC2500_REMIT_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_REMIT_DT%>" Visible="false"></asp:Label>
                                    </th>
                                    <td align="right" class="Body_label">
                                        <asp:TextBox ID="txt_REMIT_DT" runat="server" MaxLength="10" Width="81px" CssClass="date MandatoryField" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                         <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_PAY_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_REMIT_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                        <aces:Btn ID="WFB2SC2500Execute" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C2500Execute%>" OnClick="WFB2SC2500Execute_Click" Visible="false" ValidationGroup="GroupB" OnClientClick="return CheckApproveAction();" />
                                        <aces:Btn ID="WFB2SC2500Execute2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C2500Execute2%>" OnClick="WFB2SC2500Execute2_Click" Visible="false" ValidationGroup="GroupB" OnClientClick="return CheckRejectAction();" />
                                        <%--<asp:Button ID="WFB2C2500Execute" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C2500Execute%>" OnClick="WFB2C2500Execute_Click" Visible="false" ValidationGroup="GroupB" OnClientClick="return CheckApproveAction();" />
                                        <asp:Button ID="WFB2C2500Execute2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C2500Execute2%>" OnClick="WFB2C2500Execute2_Click" Visible="false" ValidationGroup="GroupB" OnClientClick="return CheckRejectAction();" />--%>
                                    </td>
                                </div>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC2500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT_S_search" DefaultValue=""
                        Name="qry_salary_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT_E_search" DefaultValue=""
                        Name="qry_salary_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE_search"
                        Name="qry_salary_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_APPROVE_STATUS_search"
                        Name="qry_process_status" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_PAY_ID"
                        Name="qry_pay_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1120px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" onclick="javascript:changeDT();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_TYPE_NAME" runat="server" Text='<%#Bind("SALARY_TYPE_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="hid_SALARY_TYPE" runat="server" ClientIDMode="Static" Value='<%#Bind("SALARY_TYPE")%>' />
                            <asp:HiddenField ID="hid_PAY_KIND" runat="server" ClientIDMode="Static" Value='<%#Bind("PAY_KIND")%>' />
                            <asp:HiddenField ID="hid_SALARY_SDT" runat="server" ClientIDMode="Static" Value='<%#Bind("SALARY_SDT")%>'  />
                            <asp:HiddenField ID="hid_SALARY_EDT" runat="server" ClientIDMode="Static" Value='<%#Bind("SALARY_EDT")%>'  />
                            <asp:HiddenField ID="hid_DUTY_SDT" runat="server" ClientIDMode="Static" Value='<%#Bind("DUTY_SDT")%>'  />
                            <asp:HiddenField ID="hid_DUTY_EDT" runat="server" ClientIDMode="Static" Value='<%#Bind("DUTY_EDT")%>'  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發放項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND" HeaderStyle-Width="120px" ItemStyle-Width="120px" >
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_PAY_KIND" runat="server" Text='<%#Bind("PAY_KIND_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資年月--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_YM2%>" SortExpression="SALARY_YM" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_YM" runat="server" CssClass="ym" Text='<%#Bind("SALARY_YM")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("SALARY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--計薪筆數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_DATA_CNT%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_DATA_CNT" runat="server" Text='<%#Bind("DATA_CNT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--確認筆數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_CFN_CNT%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_CFN_CNT" runat="server" Text='<%#Bind("CFN_PAY","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--未確認筆數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_NOT_CFN_CNT%>" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_NOT_CFN_CNT" runat="server" Text='<%#Bind("UNCFN_PAY","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--註銷筆數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_DEL_CNT%>" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_DEL_CNT" runat="server" Text='<%#Bind("DEL_CNT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PROCESS_STATUS_NAME" runat="server" Text='<%#Bind("PROCESS_STATUS_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" ClientIDMode="Static" Value='<%#Bind("PROCESS_STATUS")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--關帳日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_PAY_DT%>" SortExpression="PAY_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PAY_DT" runat="server" Text='<%#Bind("PAY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--關帳代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_PAY_ID%>" SortExpression="PAY_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PAY_ID" runat="server" Text='<%#Bind("PAY_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--實際匯款日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_REMIT_DT%>" SortExpression="REMIT_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_REMIT_DT" runat="server" Text='<%#Bind("REMIT_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
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

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_USER_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
