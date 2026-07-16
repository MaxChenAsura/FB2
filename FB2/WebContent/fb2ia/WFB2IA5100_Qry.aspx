<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA5100_Qry.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA5100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            $('.empid').mask('99999');
            gridviewScroll();
            $.unblockUI();
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
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
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
        }

        function ShowRecord(obj) {
            if (obj == "ddlPerPageRow")
                $("#HID_PageRow").val($("#ddlPerPageRow").val());
            if (obj == "ddlPerPageRow2")
                $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
            //$("#HID_PageRow").val($("#ddlPerPageRow").val());
            //$("#HID_PageRow2").val($("#ddlPerPageRow2").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        function empCheck(value) {
            if (value != "")
                __doPostBack('question', 'true');
            else
                $("#txt_EMP_NAME").val("");
        }

        //清空
        function doClear() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            $("#txt_LICENSE_ID").val("");
            return false;
        }
        function CheckData(source, arguments) {
            if ($("#txt_EMP_ID").val() == "" && $("#txt_LEAVE_DT_S").val() == "" && $("#txt_LEAVE_DT_E").val() == "" && $("#txt_LICENSE_ID").val() == "") {
                arguments.IsValid = false;
            }
            else
                arguments.IsValid = true;

        }
        

        //function delayedShow() {
        //    if (Page_IsValid != null && Page_IsValid == true) {
        //        BlockUI();
        //    }
        //}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="30%" />
                                <col width="15%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid" ></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_UPDATED_DT" runat="server" Text="<%$Resources:Resource,wfb2ia_UPDATED_DT%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <uc1:UCDateTimeRange runat="server" ID="UCDateTimeRange" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10" EndDateCssClass="ym" StartDateCssClass="ym"
                                            CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="<%$Resources:Resource,wfb2ia_greaterthan_UPDATED_DT%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2ia_error_UPDATED_DT_start%>"
                                            ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2ia_error_UPDATED_DT_end%>" ClientValidationFunctionE="CheckDate" ClientValidationFunctionS="CheckDate" ValidationGroup="GroupA" />

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_LICENSE_ID%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" MaxLength="20" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                            ErrorMessage="身分證/居留證只能輸入英數字" ControlToValidate="txt_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="至少輸入一條件" ClientValidationFunction="CheckData" ForeColor="Red"
                                            ControlToValidate="txt_LICENSE_ID" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA5100Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA5100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2IA5100Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA5100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sk_clear%>" OnClientClick="return doClear();" />
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
            </table>

            <!--grid1-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2IA5100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                        Name="updated_sdt" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                        Name="updated_edt" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LICENSE_ID" DefaultValue=""
                        Name="license_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_ID%>" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_UPDATED_DT%>" SortExpression="UPDATED_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_UPDATED_DT" runat="server" Text='<%#Bind("UPDATED_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_REATION_NAME1%>" SortExpression="REATION_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_REATION_NAME" runat="server" Text='<%#Bind("REATION_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LICENSE_ID%>" SortExpression="LICENSE_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LICENSE_ID%>" SortExpression="LICENSE_ID_FIRST" Visible="false">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENSE_ID_FIRST" runat="server" Text='<%#Bind("LICENSE_ID_FIRST")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_BIRTH_DT%>" SortExpression="BIRTH_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_BIRTH_DT" runat="server" Text='<%#Bind("BIRTH_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2IA5100History" runat="server"
                                CommandName="ToHistory"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2ia_WFB2IA5100History%>" />

                            <%--<asp:Button ID="WFB2IA5100History" runat="server"
                                CommandName="ToHistory"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2ia_WFB2IA5100History%>" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_EMP_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_UPDATED_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_REATION_NAME1%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_LICENSE_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_EMP_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_BIRTH_DT%>"></asp:Label>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('ddlPerPageRow')" ClientIDMode="Static" AutoPostBack="true">
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

            <!--grid2-->
            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="GetData2"
                SelectCountMethod="GetCount2" TypeName="CFB2IA5100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting2"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected2">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HID_EMP_ID" DefaultValue=""
                        Name="EMP_ID" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="HID_LICENSE_ID_FIRST" DefaultValue=""
                        Name="LICENSE_ID_FIRST" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting2"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging2" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound2">
                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_UPDATED_DT%>" SortExpression="CREATED_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_CREATED_DT" runat="server" Text='<%#Bind("CREATED_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_ID%>" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_REATION_NAME1%>" SortExpression="REATION_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_REATION_NAME" runat="server" Text='<%#Bind("REATION_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LICENSE_ID%>" SortExpression="LICENSE_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_BIRTH_DT%>" SortExpression="BIRTH_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_BIRTH_DT" runat="server" Text='<%#Bind("BIRTH_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_UPDATED_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_EMP_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_REATION_NAME1%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_LICENSE_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_EMP_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_BIRTH_DT%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow2" runat="server" onchange="javascript:ShowRecord('ddlPerPageRow2')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount2" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_LICENSE_ID_FIRST" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2ia_condition_NottypeMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_condition_NottypeMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
