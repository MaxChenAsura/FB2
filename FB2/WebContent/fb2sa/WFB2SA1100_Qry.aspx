<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1100_Qry.aspx.cs" Inherits="WebContent_WFB2SA1100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".year").mask('9999');
            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //凍結視窗 
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }


        //清空畫面
        function ClearAll() {
            var day = new Date();
            var year = day.getFullYear();
            $("#ddl_WS_CD").val("");
            $("#txt_DATA_YEAR").val(year);
            $("#txt_LEVEL_CD").val("");
            $("#txt_GRADE_CD").val("");
            $("#txt_GRADE_YEAR").val("");
            $("#ddl_EDUCATION_CD").val(-1);
            $("#ddl_WS_CD").val(-1);
            return false;
        }
        //檔案匯入檢驗
        function checkUpload(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                //BlockUI();
            }
            else {
                processed = false;
                return false;
            }

            BlockUI();
            processed = confirm($("#hidconfirm_Execute").val() + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        function MycheckDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return;
            BlockUI();
            processed = confirm($("#hidconfirm_Execute").val() + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        function doUnBlock() {
            $.unblockUI();
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
                                <col width="12%" />
                                <col width="21%" />
                                <col width="10%" />
                                <col width="23%" />
                                <col width="10%" />
                                <col width="24%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <!--初任薪年度 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR1%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YEAR" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sa_required_DATA_YEAR1%>"
                                            ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                            ErrorMessage="年度必須為4碼!" ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <!--敍薪學歷 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EDUCATION_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EDUCATION_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <!--資格 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_LEVEL_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                            ErrorMessage="資格只能輸入英數字" ControlToValidate="txt_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <!--級數 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GRADE_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_GRADE_CD" runat="server" MaxLength="1" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="級數只能輸入英數字" ControlToValidate="txt_GRADE_CD" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <!--職種區分 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_WS_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <!--畢業年度 -->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GRADE_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_GRADE_YEAR" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="year"></asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="畢業年度長度需為4碼"
                                            ControlToValidate="txt_GRADE_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="6">

                                        <div id="init">
                                             <aces:Btn ID="WFB2SA1100Process" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1100Process%>" ValidationGroup="GroupA" OnClick="WFB2SA1100Process_Click" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2SA1100Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Search%>" ValidationGroup="GroupA" OnClick="WFB2SA1100Search_Click" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2SA1100ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0401ExcelDown%>" OnClick="WFB2SA1100ExcelDown_Click" ValidationGroup="GroupA" OnClientClick="return checkDowning(this.value);" ClientIDMode="Static" />
                                            <%--<asp:Button ID="WFB2SA1100Process" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1100Process%>" ValidationGroup="GroupA" OnClick="WFB2SA1100Process_Click" OnClientClick="CheckValid();" />
                                            <asp:Button ID="WFB2SA1100Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Search%>" ValidationGroup="GroupA" OnClick="WFB2SA1100Search_Click" OnClientClick="CheckValid();" />
                                            <asp:Button ID="WFB2SA1100ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0401ExcelDown%>" OnClick="WFB2SA1100ExcelDown_Click" ValidationGroup="GroupA" OnClientClick="return MycheckDowning(this.value);" ClientIDMode="Static" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2df_btn_clear%>" OnClientClick="ClearAll();" CausesValidation="false" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData" SelectCountMethod="getCount" TypeName="CFB2SA1100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting" StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YEAR" DefaultValue=""
                        Name="data_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_EDUCATION_CD"
                        Name="education_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LEVEL_CD" DefaultValue=""
                        Name="level_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_GRADE_CD" DefaultValue=""
                        Name="grade_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_WS_CD"
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_GRADE_YEAR" DefaultValue=""
                        Name="grade_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1000px" OnRowCreated="gv_result_RowCreated" OnPageIndexChanging="gv_result_PageIndexChanging"
                OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>" SortExpression="LEVEL_CD" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>" SortExpression="EDUCATION_CD" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EDUCATION_CD" runat="server" Text='<%#Bind("EDUCATION_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_YEAR%>" SortExpression="GRADE_YEAR" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_YEAR" runat="server" Text='<%#Bind("GRADE_YEAR")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY1%>" SortExpression="LEVEL_PAY1" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_PAY1" runat="server" Text='<%#Bind("LEVEL_PAY1","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY2%>" SortExpression="LEVEL_PAY2" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_PAY2" runat="server" Text='<%#Bind("LEVEL_PAY2","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_LEVEL_PAY3%>" SortExpression="LEVEL_PAY3" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_PAY3" runat="server" Text='<%#Bind("LEVEL_PAY3","{0:N0}")%>'></asp:Label>
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
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidconfirm_Execute" Value="<%$Resources:Resource,confirm_Execute%>" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SA1100ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>

