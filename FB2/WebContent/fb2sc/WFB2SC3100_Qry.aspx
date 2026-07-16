<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC3100_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC3100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask("9999/99/99");
            $(".ym").mask("9999/99");
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
                height: "400",
                barcolor: "#7F7F7F"
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }
        function CheckYMD(source, arguments) {
            if ($("#txt_SALARY_SDT").val() != "" && $("#txt_SALARY_EDT").val() != "") {
                if ($("#txt_SALARY_EDT").val() < $("#txt_SALARY_SDT").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }
        function ClearAll() {
            $("#txt_SALARY_SDT").val("");
            $("#txt_SALARY_EDT").val("");
            $("#txt_SALARY_YM").val($("#hid_SALARY_YM").val());
            $("#ddl_SALARY_TYPE").val("-1");
            $("#ddl_PROCESS_STATUS").val("-1");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class='pos_top'>
                <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                    <colgroup>
                        <col width="10%" />
                        <col width="30%" />
                        <col width="10%" />
                        <col width="21%" />
                        <col width="10%" />
                        <col width="12%" />
                        <col width="5%" />
                        <col width="7%" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_SALARY_SDT" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                <label>~</label>
                                <asp:TextBox ID="txt_SALARY_EDT" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfd2sc_ERR_SALARY_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_SALARY_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfd2sc_ERR_SALARY_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_SALARY_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="發薪日期起不可大於迄" ClientValidationFunction="CheckYMD" ForeColor="Red"
                                    ControlToValidate="txt_SALARY_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" CssClass="ym date2"></asp:TextBox>
                                <asp:HiddenField ID="hid_SALARY_YM" runat="server" ClientIDMode="Static" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sc_ERR_SALARY_YM%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red"
                                    ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>

                            </td>
                            <td align="left" class="Body_label"></td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_PROCESS_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td align="left" class="Body_label"></td>
                        </tr>
                    </tbody>
                </table>


                <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                    <colgroup>
                        <col width="10%" />
                        <col width="32%" />
                        <col width="35%" />
                        <col width="23%" />
                    </colgroup>
                    <tbody>

                        <tr>
                            <td align="right" class="Body_label" colspan="10">
                                <%--<asp:Button ID="WFB2SC3100Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C3100Search%>" Visible="true" OnClick="WFB2SC3100Search_Click" ValidationGroup="GroupA" />--%>
                                <aces:Btn ID="WFB2SC3100Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C3100Search%>" Visible="true" OnClick="WFB2SC3100Search_Click" ValidationGroup="GroupA" />
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sc_WFB2SC3100Clear%>" onclick="ClearAll();" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" height="1" colspan="10">
                                <hr />
                            </td>
                        </tr>
                        <tr>

                            <td align="right" colspan="10">
                                <div id="init_grid" style="display: inline">
                                    <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_COMPANY_CD%>" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="ddl_COMPANY_CD" runat="server" ClientIDMode="Static" Visible="false"></asp:DropDownList>

                                    <%--<asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PLANT_CD%>" Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static" Visible="false"></asp:DropDownList>--%>
                                </div>

                                <div id="init_grid2" style="display: inline">
                                   <%-- <asp:Button ID="WFB2SC3100Print" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C3200Print%>" Visible="false" OnClientClick="BlockUI();" OnClick="WFB2SC3100Print_Click" />
                                    <asp:Button ID="WFB2SC3100Print2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C3200Print2%>" Visible="false" OnClientClick="BlockUI();" OnClick="WFB2SC3100Print2_Click" />--%>
                                    <%--wfb2sc_WFB2C3200Print 彙計表 ,wfb2sc_WFB2C3200Print2 明計表--%>
                                    <aces:Btn ID="WFB2SC3100Print" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C3200Print%>" Visible="false" OnClientClick="BlockUI();" OnClick="WFB2SC3100Print_Click" />
                                    <aces:Btn ID="WFB2SC3100Print2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C3200Print2%>" Visible="false" OnClientClick="BlockUI();" OnClick="WFB2SC3100Print2_Click" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <%--<div align="right" class='pos_top'>
    </div>
    <span>
        <br/>
    </span>--%>
            <div id="editPage1">
                <div id="gridList1">
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                        SelectCountMethod="getCount" TypeName="CFB2SC3100DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="txt_SALARY_SDT" DefaultValue=""
                                Name="salary_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="txt_SALARY_EDT" DefaultValue=""
                                Name="salary_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="txt_SALARY_YM" DefaultValue=""
                                Name="salary_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="ddl_SALARY_TYPE" DefaultValue=""
                                Name="salary_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="ddl_PROCESS_STATUS" DefaultValue=""
                                Name="process_status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                        OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                    <asp:HiddenField ID="hid_PAY_KIND" runat="server" ClientIDMode="Static" Value='<%#Bind("hid_PAY_KIND")%>' />
                                    <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" ClientIDMode="Static" Value='<%#Bind("hid_PROCESS_STATUS")%>' />
                                    <asp:HiddenField ID="hid_SALARY_TYPE" runat="server" ClientIDMode="Static" Value='<%#Bind("hid_SALARY_TYPE")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="50px" />
                            <asp:BoundField DataField="SALARY_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="hid_SALARY_TYPE" Visible="false" />
                            <%--                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>" SortExpression="SALARY_YM" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="txt_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                            <asp:BoundField DataField="SALARY_YM" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>" SortExpression="SALARY_YM" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="SALARY_DT" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="PAY_KIND_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="hid_PAY_KIND" Visible="false" />
                            <asp:BoundField DataField="PROCESS_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="hid_PROCESS_STATUS" Visible="false" />
                            <asp:BoundField DataField="PAY_ID" HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_ID%>" SortExpression="PAY_ID" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="PAY_DT" HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_DT%>" SortExpression="PAY_DT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                        <HeaderStyle CssClass="GridviewScrollHeader" />
                        <PagerStyle CssClass="GridviewScrollPager" />
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
                </div>
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SC3100Print" />
            <asp:PostBackTrigger ControlID="WFB2SC3100Print2" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

