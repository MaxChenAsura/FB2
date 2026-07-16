<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC3200_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC3200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date1").datepicker({ dateFormat: 'yy/mm' })
            $(".date").mask("9999/99/99");
            $(".date1").mask("9999/99");
            $(".PROFESSION_ALLOWANCE").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            $(".MANAGEMENT_ALLOWANCE").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });

            $(".number2").mask("999");
            gridviewScroll();
            $.unblockUI();
        }//月曆
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //function getSALARY_YM() {
        //    var datetime = new Date();
        //    var year = datetime.getFullYear().toString();
        //    var month = datetime.a
        //    if (month < 10)
        //        month = "0" + month;
        //    $("#txt_SALARY_YM").val(year + month);
        //    $(".date1").mask("9999/99");
        //}
        //清空畫面
        function ClearAll() {
            $("#txt_SALARY_YM").val($("#hid_YM").val);
            $("#txt_SALARY_SDT").val("");
            $("#txt_SALARY_EDT").val("");
            $("#ddl_PROCESS_STATUS").val("");
           // getSALARY_YM()
            return false;
        }
        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
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
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="50%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="6" Width="68px" ClientIDMode="Static" CssClass="date1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_YM_Format_Error%>"
                                            ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupB"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_SALARY_SDT"
                                        ControlToValidate="txt_SALARY_EDT" ErrorMessage="<%$Resources:Resource,wfb2sc_ERR_CALENDAR_DT%>" Type="Date" Operator="GreaterThanEqual"
                                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_SDT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_ERR_SALARY_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        ~
                                        <asp:TextBox ID="txt_SALARY_EDT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_ERR_SALARY_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                    </td>
                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>"></asp:Label></th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PROCESS_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td align="left" class="Body_label"></td>
                                    <td align="left" class="Body_label"></td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="4">
                                        <aces:Btn ID="WFB2SC3200Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC3200Search%>" OnClick="WFB2SC3200Search_Click" ValidationGroup="GroupA" />
                                        <%--<asp:Button ID="WFB2SC3200Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC3200Search%>" OnClick="WFB2SC3200Search_Click" ValidationGroup="GroupA" />--%>
                                        <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sc_btn_clear%>" onclick="return ClearAll();" />
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
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <tr>
                                <td align="right" colspan="10">
                                    <aces:Btn ID="WFB2SC3200Print" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C3200Print3%>" OnClick="WFB2SC3200Print_Click" OnClientClick="return checkDowning(this.value);"  Visible="false" />
                                    <%--<asp:Button ID="WFB2SC3200Print" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2C3200Print3%>" OnClick="WFB2SC3200Print_Click" OnClientClick="return checkDowning(this.value);" Visible="false" />--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC3200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM"
                        Name="SALARY_YM" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_SDT"
                        Name="SALARY_SDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_EDT"
                        Name="SALARY_EDT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_PROCESS_STATUS" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="PROCESS_STATUS" PropertyName="Text" Type="String" />
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
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="SALARY_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="SALARY_YM" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>" SortExpression="SALARY_YM" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="SALARY_DT" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="SALARY_SDT" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_SDT%>" SortExpression="SALARY_SDT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="SALARY_EDT" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_EDT%>" SortExpression="SALARY_EDT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="DUTY_SDT" HeaderText="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT%>" SortExpression="DUTY_SDT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="DUTY_EDT" HeaderText="<%$Resources:Resource,wfb2sc_lb_DUTY_EDT%>" SortExpression="DUTY_EDT" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="PROCESS_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="PROCESS_STATUS" Visible="false" />
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
            <!-- footer -->
            <%--<table width="980" border="0" cellspacing="0" cellpadding="0">
        <tr class="PageFooter">
            <td align="center" height="15" bgcolor="#FF0000">&copy  2007-TOYOTA MOTOR ASIA PACIFIC - ENGINEERING & MANUFACTURING CO., LTD. All Rights Reserved 
	</td>
        </tr>
    </table>--%>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_SALARY_TYPE" runat="server" ClientIDMode="Static" />
            <%--//WK期間工人數--%>
            <asp:HiddenField ID="hid_WKduthwker" runat="server" ClientIDMode="Static" />
            <%--//正社員人數--%>
            <asp:HiddenField ID="hid_WKmember" runat="server" ClientIDMode="Static" />
            <%--//薪資發放人數--%>
            <asp:HiddenField ID="hid_WKpaytotal" runat="server" ClientIDMode="Static" />
            <%--//建教生人數--%>
            <asp:HiddenField ID="hid_WKmemofstu" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="hid_YM" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SC3200Print" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
