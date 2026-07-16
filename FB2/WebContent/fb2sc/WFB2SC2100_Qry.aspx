<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2100_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2100_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //getYearMonth()
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function getYearMonth() {
            var datetime = new Date();
            var year = datetime.getFullYear().toString();
            var month = datetime.getMonth();
            if (month < 10)
                month = "0" + month;
            $("#txt_SALARY_YM_search").val(year + month);
        }
        function ClearAll() {
            $('#txt_SALARY_DT_S_search').val("");
            $('#txt_SALARY_DT_E_search').val("");
            $('#txt_SALARY_YM_search').val("");
            $('#ddl_SALARY_TYPE_search').val("");
            $('#txt_salary_sdt_search').val("");
            $('#txt_salary_edt_search').val("");
            $('#txt_DUTY_SDT_search').val("");
            $('#txt_DUTY_EDT_search').val("");
            return false;
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb2sc_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb2sc_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Dtl_NotChoiceMessage').val());
                return false;
            }
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
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>

                                    <%--薪資年月--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_YM_search" runat="server" MaxLength="6" CssClass="ym" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_YM_Format_Error%>"
                                            ControlToValidate="txt_SALARY_YM_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <%--計薪日期區間--%>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_SDT_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_CAL%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_salary_sdt_search" runat="server" Width="81px" CssClass="date ymd" ClientIDMode="Static" MaxLength="8"></asp:TextBox>
                                        ~  
                                        <asp:TextBox ID="txt_salary_edt_search" runat="server" Width="81px" CssClass="date ymd" ClientIDMode="Static" MaxLength="8"></asp:TextBox>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_SDT_CAL_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_salary_sdt_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_EDT_CAL_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_salary_edt_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <%--考勤日期區間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DUTY_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT_TO_EDT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DUTY_SDT_search" runat="server" Width="81px" CssClass="date ymd" ClientIDMode="Static" MaxLength="8"></asp:TextBox>
                                        ~  
                                        <asp:TextBox ID="txt_DUTY_EDT_search" runat="server" Width="81px" CssClass="date ymd" ClientIDMode="Static" MaxLength="8"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_DUTY_SDT_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DUTY_EDT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_DUTY_SDT_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--發薪日期區間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_DT_Range%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_SALARY_DT_S_search" runat="server" Width="81px" CssClass="date ymd" ClientIDMode="Static" MaxLength="8"></asp:TextBox>
                                        ~  
                                        <asp:TextBox ID="txt_SALARY_DT_E_search" runat="server" Width="81px" CssClass="date ymd" ClientIDMode="Static" MaxLength="8"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_S_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT_S_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_E_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT_E_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC2100Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC2100Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
<%--                                            <asp:Button ID="WFB2SC2100Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC2100Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2sc_btn_clear%>" OnClientClick="ClearAll();" />
                                        </div>
                                    </td>
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
                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2SC2100Add" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC2100Add_Click" />
                        <aces:Btn ID="WFB2SC2100Delete" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC2100Delete_Click" Visible="false" />
                        <aces:Btn ID="WFB2SC2100Detail" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2SC2100Detail_Click" Visible="false" />
<%--                            <asp:Button ID="WFB2SC2100Add" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC2100Add_Click" />
                            <asp:Button ID="WFB2SC2100Delete" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC2100Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2SC2100Detail" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2SC2100Detail_Click" Visible="false" />--%>
                        </div>

                    </td>
                </tr>
                <tr></tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT_S_search" DefaultValue=""
                        Name="salary_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT_E_search" DefaultValue=""
                        Name="salary_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM_search"
                        Name="salary_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_salary_sdt_search" DefaultValue=""
                        Name="salary_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_salary_edt_search" DefaultValue=""
                        Name="salary_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DUTY_SDT_search" DefaultValue=""
                        Name="duty_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DUTY_EDT_search" DefaultValue=""
                        Name="duty_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE_search"
                        Name="salary_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Style="text-align: center;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--薪資年月--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_YM2%>" SortExpression="SALARY_YM" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM")%>' CssClass="ym" Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_TYPE" runat="server" Text='<%#Bind("SALARY_TYPE_DESC")%>' Width="100%" Style="text-align: left;"></asp:Label>
                            <asp:HiddenField ID="hid_SALARY_TYPE" runat="server" Value='<%#Bind("SALARY_TYPE")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--入帳週期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_IACYC%>" SortExpression="IACYC" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IACYC" runat="server" Text='<%#Bind("IACYC")%>'  CssClass="ym" Width="100%" Style="text-align: left;"></asp:Label>                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發放項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_PAY_KIND" runat="server" Text='<%#Bind("PAY_KIND_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--計薪日期區間起--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_SDT_CAL%>" SortExpression="SALARY_SDT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_SDT" runat="server" Text='<%#Bind("SALARY_SDT","{0:yyyy/MM/dd}")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--計薪日期區間迄--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_EDT_CAL%>" SortExpression="SALARY_EDT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_EDT" runat="server" Text='<%#Bind("SALARY_EDT","{0:yyyy/MM/dd}")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考勤日期區間起--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT%>" SortExpression="DUTY_SDT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_DUTY_SDT" runat="server" Text='<%#Bind("DUTY_SDT","{0:yyyy/MM/dd}")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考勤日期區間迄--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_DUTY_EDT%>" SortExpression="DUTY_EDT" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_DUTY_EDT" runat="server" Text='<%#Bind("DUTY_EDT","{0:yyyy/MM/dd}")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--處理狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS2%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text='<%#Bind("PROCESS_STATUS_DESC")%>' Width="100%" Style="text-align: left;"></asp:Label>
                            <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" Value='<%#Bind("PROCESS_STATUS")%>' />
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


