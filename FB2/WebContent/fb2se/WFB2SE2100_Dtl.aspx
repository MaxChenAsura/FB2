<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE2100_Dtl.aspx.cs" Inherits="WebContent_fb2se_WFB2SE2100_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            //$(".date").datepicker({ dateFormat: 'yy/mm/dd' }); (小日曆開窗)
            $(".ym").mask('9999/99');
            $(".ymd").mask('9999/99/99');
            $(".money").formatNumber({ format: "#,###", locale: "tw" });
            $('.empid').mask('99999');
            $("#lb_EMP_NAME").attr("readonly", true);
            
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
        //清空
        function doClear() {
            $("#txt_EMP_ID").val("");
            $("#lb_EMP_NAME").val("");
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
                                        <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2_EFFECT_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EFFECT_YM" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true" CssClass="ym"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_STATUS_NAME" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_APPROVE_STATUS_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MEM_CREATE_BY" runat="server" Text="<%$Resources:Resource,wfb2se_lb_MEM_CREATE_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MEM_CREATE_BY" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MEM_CREATE_DT" runat="server" Text="<%$Resources:Resource,wfb2se_lb_MEM_CREATE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MEM_CREATE_DT" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <td></td>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="Div1">
                                            <asp:Button ID="btn_backpage" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_backpage%>" OnClick="btn_backpage_Click" OnClientClick="BlockUI();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                        <tr>
                            <td colspan="5">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                                    <colgroup>
                                        <col width="20%" />
                                        <col width="30%" />
                                        <col width="20%" />
                                        <col width="30%" />
                                    </colgroup>
                                    <tbody>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_ID2" runat="server" Text="<%$Resources:Resource,wfb2sa_EMP_ID%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'lb_EMP_NAME', 'N', '');" />
                                                <asp:TextBox ID="lb_EMP_NAME" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <td></td>
                                            <th></th>
                                            <td align="right" class="Body_label">
                                                <div id="init">
                                                    <aces:Btn ID="WFB2SE2101Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SE2101Search_Click"  />

                                                    <%--<asp:Button ID="WFB2SE2101Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SE2101Search_Click"  />--%>
                                                    <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Clear%>" OnClientClick="return doClear();" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <hr />
                                            </td>
                                        </tr>

                                    </tbody>

                                </table>
                            </td>
                        </tr>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetDtlData" SelectCountMethod="GetDtlCount" TypeName="CFB2SE2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting" StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue="" Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EFFECT_YM" DefaultValue="" Name="effect_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1000px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="55px">
                        <ItemTemplate>
                            <asp:Label width="55px" ID="lb_EMP_ID" Style="text-align: left;" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" ID="lb_EMP_NAME" Style="text-align: left;" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2df_lb_JOIN_DT%>" SortExpression="JOIN_DT" HeaderStyle-Width="100px"  >
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_JOIN_DT" runat="server" Text='<%#Bind("JOIN_DT","{0:yyyy/MM/dd}")%>'> </asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_PJOB_CD%>" SortExpression="PJOB_CD_NAME" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Label Width="200px" ID="lb_PJOB_CD_NAME" Style="text-align: left;" runat="server" Text='<%#Bind("PJOB_CD_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_LEVEL_PAY_OLD%>" SortExpression="LEVEL_PAY_OLD" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" ID="lb_LEVEL_PAY_OLD" Style="text-align: right;" runat="server" Text='<%#Bind("LEVEL_PAY_OLD","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_LEVEL_PAY_NEW%>" SortExpression="LEVEL_PAY_NEW" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" ID="lb_LEVEL_PAY_NEW" Style="text-align: right;" runat="server" Text='<%#Bind("LEVEL_PAY_NEW","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_ABILITY_PAY_OLD%>" SortExpression="ABILITY_PAY_OLD" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" ID="ABILITY_PAY_OLD" Style="text-align: right;" runat="server" Text='<%#Bind("ABILITY_PAY_OLD","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_ABILITY_PAY_NEW%>" SortExpression="ABILITY_PAY_NEW" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" ID="ABILITY_PAY_NEW" Style="text-align: right;" runat="server" Text='<%#Bind("ABILITY_PAY_NEW","{0:N0}")%>'></asp:Label>
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

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>


    </asp:UpdatePanel>
</asp:Content>
