<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0200_Qry.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');

            //工號取得姓名的ajax
            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_EMP_ID").blur(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        cache: false,
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

            gridviewScroll();
            $.unblockUI();
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 6

            });

        }

        //清空畫面
        function ClearAll() {
            $("#ddl_HR_CHG_CD").val("-1");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_ICT_TYPE").val("-1");
            $("#ddl_TRANSFER_NATION_CD").val("-1");
            $("#ddl_TRANSFER_COMPANY_CD").empty();
            $("#ddl_TRANSFER_COMPANY_CD").val("-1");
            $("#txt_START_DT_S").val("");
            $("#txt_START_DT_E").val("");
            $('#rbl_IS_VALID').find("input[value='ALL']").prop("checked", "checked");
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function CheckYMD(source, arguments) {
            if ($("#txt_START_DT_S").val() != "" && $("#txt_START_DT_E").val() != "") {
                if ($("#txt_START_DT_E").val() < $("#txt_START_DT_S").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }
        function CheckSearch() {

            if ($("#ddl_HR_CHG_CD").val() == "-1") {
                if ($("#ddl_TRANSFER_COMPANY_CD").val() != "-1" && $("#ddl_TRANSFER_COMPANY_CD").val() != null) {
                    alert("未輸入外調類別，不可以輸入受入公司別");
                    return false;
                }
            }

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0"
                class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="30%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="42px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="50px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_HR_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_HR_CHG_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_HR_CHG_CD" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_HR_CHG_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ICT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_ICT_TYPE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ICT_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TRANSFER_NATION_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_NATION_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_TRANSFER_NATION_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TRANSFER_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_COMPANY_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_TRANSFER_COMPANY_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT_SE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_START_DT_SE2%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            ~
                    <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="cv_START_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_START_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cv_START_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_ERR_START_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_HB020_START_DT%>" ClientValidationFunction="CheckYMD" ForeColor="Red"
                                ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <%--<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT_S"
                                ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2hb_HB020_START_DT%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>--%>
                        </td>
                        <td align="left" class="Body_label" colspan="4">
                            <asp:RadioButtonList ID="rbl_IS_VALID" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                <asp:ListItem Text="<%$Resources:Resource,wfb2hb_IS_VALID_1%>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2hb_IS_VALID_2%>" Value="2"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2hb_IS_VALID_ALL%>" Value="ALL" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <th></th>
                    </tr>

                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2HB0200Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0200Search%>" OnClick="WFB2HB0200Search_Click" OnClientClick="return CheckSearch();" />
                                <%--<asp:Button ID="WFB2HB0200Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0200Search%>" OnClick="WFB2HB0200Search_Click" OnClientClick="return CheckSearch();" />--%>
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr />
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HB0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_HR_CHG_CD" DefaultValue=""
                        Name="hr_chg_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_ICT_TYPE" DefaultValue=""
                        Name="ict_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_TRANSFER_NATION_CD" DefaultValue=""
                        Name="transfer_nation_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_TRANSFER_COMPANY_CD" DefaultValue="" ConvertEmptyStringToNull="False"
                        Name="transfer_company_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT_S" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_s" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT_E" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_e" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="rbl_IS_VALID" DefaultValue=""
                        Name="is_valid" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1520px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="HR_CHG_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_HR_CHG_CD%>" SortExpression="TRANSFER_REASON" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ICT_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_ICT_TYPE%>" SortExpression="ICT_TYPE" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ORI_DEPT_NAME_20" HeaderText="<%$Resources:Resource,wfb2hb_lb_ORI_DEPT_NAME_20D%>" SortExpression="ORI_DEPT_NO" HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ORI_LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_ORI_LEVEL_CD%>" SortExpression="ORI_LEVEL_CD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NAME_20" HeaderText="<%$Resources:Resource,wfb2hb_lb_DEPT_NAME_20%>" SortExpression="DEPT_NAME_20" HeaderStyle-Width="170px" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="TRANSFER_NATION" HeaderText="<%$Resources:Resource,wfb2hb_lb_TRANSFER_NATION%>" SortExpression="TRANSFER_NATION_CD" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="TRANSFER_COMPANY" HeaderText="<%$Resources:Resource,wfb2hb_lb_TRANSFER_COMPANY%>" SortExpression="TRANSFER_COMPANY_CD" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_TRANSFER_DEPT%>" SortExpression="TRANSFER_DEPT" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="150px" Style="text-align: left; word-wrap: break-word;" ID="TRANSFER_DEPT" runat="server" Text='<%#Bind("TRANSFER_DEPT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="TRANSFER_DEPT" HeaderText="<%$Resources:Resource,wfb2hb_lb_TRANSFER_DEPT%>" SortExpression="TRANSFER_DEPT" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" ItemStyle-CssClass="word" />--%>
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="PLAN_END_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_PLAN_END_DT%>" SortExpression="PLAN_END_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" HtmlEncode="false" />
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" HtmlEncode="false" />
                    <asp:BoundField DataField="HR_CHG_NO" HeaderText="<%$Resources:Resource,wfb2hb_lb_HR_CHG_NO%>" SortExpression="HR_CHG_NO" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
