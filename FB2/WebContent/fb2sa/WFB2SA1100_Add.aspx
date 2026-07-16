<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1100_Add.aspx.cs" Inherits="WebContent_fb2sa_WFB2SA1100_Add" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".year").mask('9999');
            $(".formatnum1").mask('9999999');
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

        function Valid()
        {
            BlockUI();
            Page_IsValid = Page_ClientValidate('GroupA');;
            if (Page_IsValid) {
                if (confirm('是否確定生成?')) {
                    return true;
                } else {
                    $.unblockUI();
                    return false;
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--頁面table--%>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <%--查詢條件table(HTML)--%>
                        <%--TextBox,input button,DropDownList--%>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="20%" />
                                <col width="30%" />
                                <col width="20%" />
                                <col width="30%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR1%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YEAR" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sa_required_DATA_YEAR1%>"
                                            ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SA1100New" runat="server" Text="<%$Resources:Resource,wfb2sa_bt_DataMark%>" OnClientClick="return CheckValid();" OnClick="WFB2SA1100New_Click" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SA1100New" runat="server" Text="<%$Resources:Resource,wfb2sa_bt_DataMark%>" OnClientClick="return CheckValid();" OnClick="WFB2SA1100New_Click" ValidationGroup="GroupA" />--%>
                                            <asp:Button runat="server" ID="btn_back" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click"/>
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
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData1" SelectCountMethod="GetCount1" TypeName="CFB2SA1100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting" StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_DATA_YEAR" DefaultValue="" Name="data_year" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                   
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="false"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1000px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Width="40px" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 職種--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="hid_WS_CD" runat="server" Value='<%#Bind("WS_CD")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 初任資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>" SortExpression="LEVEL_CD" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 級數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label Width="50px" Style="text-align: left;" ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--敍薪學歷--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>" SortExpression="EDUCATION_CD" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EDUCATION_CD" runat="server" Text='<%#Bind("EDUCATION_CD_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="hid_EDUCATION_CD" runat="server" Value='<%#Bind("EDUCATION_CD")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--起算薪資--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_START_SALARY%>" SortExpression="START_SALARY" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:TextBox Style="text-align: right" ID="txt_EDIT_START_SALARY" runat="server" MaxLength="7" Width="70px" ClientIDMode="Static" Text='<%#Bind("START_SALARY")%>' CssClass="MandatoryField formatnum1"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--基準年--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_BASE_YEAR%>" SortExpression="BASE_YEAR" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:TextBox Style="text-align: right" ID="txt_EDIT_BASE_YEAR" runat="server" MaxLength="4" Width="60px" ClientIDMode="Static" Text='<%#Bind("BASE_YEAR")%>' CssClass="MandatoryField year"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--推算起年--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_START_YEAR%>" SortExpression="START_YEAR" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:TextBox Style="text-align: right" ID="txt_EDIT_START_YEAR" runat="server" MaxLength="4" Width="60px" Text='<%#Bind("START_YEAR")%>' CssClass="MandatoryField year"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--推算迄年--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_END_YEAR%>" SortExpression="END_YEAR" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:TextBox Style="text-align: right" ID="txt_EDIT_END_YEAR" runat="server" MaxLength="4" Width="60px" ClientIDMode="Static" Text='<%#Bind("END_YEAR")%>' CssClass="MandatoryField year"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--基本格差--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_BASE_RANGE%>" SortExpression="BASE_RANGE" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Style="text-align: right" ID="txt_EDIT_BASE_RANGE" runat="server" MaxLength="7" Width="70px" ClientIDMode="Static" Text='<%#Bind("BASE_RANGE")%>' CssClass="MandatoryField formatnum1"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--女性格差--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_FEMALE_RANGE%>" SortExpression="FEMALE_RANGE" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:TextBox Style="text-align: right" ID="txt_EDIT_FEMALE_RANGE" runat="server" MaxLength="7" Width="70px" ClientIDMode="Static" Text='<%#Bind("FEMALE_RANGE")%>' CssClass="formatnum1"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--待役格差--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_lb_ARMY_RANGE%>" SortExpression="ARMY_RANGE" HeaderStyle-Width="90px">
                        <ItemTemplate>
                            <asp:TextBox Style="text-align: right" ID="txt_EDIT_ARMY_RANGE" runat="server" MaxLength="7" Width="90px" ClientIDMode="Static" Text='<%#Bind("ARMY_RANGE")%>' CssClass="formatnum1"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sk_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_WS_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sa_LEVEL_CD1%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_GRADE_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EDUCATION_CD%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_START_SALARY%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_BASE_YEAR%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_START_YEAR%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_END_YEAR%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_BASE_RANGE%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_FEMALE_RANGE%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_ARMY_RANGE%>"></asp:Label>
                            </td>
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
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10000_Rows%>" Value="10000"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
             <asp:HiddenField ID="hid_DATA_YEAR" runat="server" ClientIDMode="Static" />
            <%--紀錄頁數(跳頁用)--%>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="add_check" runat="server" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <%--resource)--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
