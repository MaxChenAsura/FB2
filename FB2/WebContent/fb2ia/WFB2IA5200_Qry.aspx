<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA5200_Qry.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA5200_Qry" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.year').mask('9999');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
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
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
            $("#HID_PageRow3").val($("#ddlPerPageRow3").val());
        }
        //凍結視窗
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    headerrowcount: 1,
                    freezesize: 2
                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }

        }

        //清空
        function doClear() {
            var now = new Date();
            var Current = now.getFullYear();
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_SALARY_YEAR").val(Current);
            $("#txt_SALARY_YM").val("");
            $("#txt_EMP_ID").val($("#hid_defalut_EMP_ID").val());
            $("#txt_EMP_NAME").val($("#hid_defalut_EMP_NAME").val());
            return false;
        }
        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            }
        }
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
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass=" MandatoryField empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YEAR" runat="server" Text="<%$Resources:Resource,wfb2ia_SALARY_YEAR%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_SALARY_YEAR" runat="server" MaxLength="4" Width="35px" CssClass=" MandatoryField year" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_SALARY_YEAR%>" ControlToValidate="txt_SALARY_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_SALARY_YEAR%>"
                                            ControlToValidate="txt_SALARY_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2ia_SALARY_YM1%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="6" Width="64px" CssClass="ym date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_SALARY_YM1%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
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
                                            <aces:Btn ID="WFB2IA5200Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA5200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2SC4104InsSearch" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA5200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2IA5200Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2IA5200Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
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
                SelectCountMethod="GetCount" TypeName="CFB2IA5200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_YEAR" DefaultValue=""
                        Name="salary_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM" DefaultValue=""
                        Name="salary_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:Label ID="gv1" runat="server" Text="<%$Resources:Resource,wfb2ia520_gv1%>" Visible="false"></asp:Label>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_CTYPE%>" SortExpression="CTYPE">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_CTYPE" runat="server" Text='<%#Bind("CTYPE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_CTYPE%>" SortExpression="CTYPE">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_CTYPE" runat="server" Text='<%#Bind("CTYPE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_YM%>" SortExpression="SALARY_YM">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM","{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_TYPE_NAME%>" SortExpression="INS_TYPE_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_INS_TYPE_NAME" runat="server" Text='<%#Bind("INS_TYPE_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_AMT%>" SortExpression="INS_AMT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_TOTAL%>" SortExpression="INS_TOTAL">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_INS_TOTAL" runat="server" Text='<%#Bind("INS_TOTAL","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SLEF_RATE%>" SortExpression="SLEF_RATE">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_SLEF_RATE" runat="server" Text='<%#Bind("SLEF_RATE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SALARY_DT%>" SortExpression="SALARY_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_CMEMO%>" SortExpression="CMEMO">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_CMEMO" runat="server" Text='<%#Bind("CMEMO")%>'></asp:Label>
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

            <!--grid2-->
            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="GetData2"
                SelectCountMethod="GetCount2" TypeName="CFB2IA5200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting2"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected2">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_YEAR" DefaultValue=""
                        Name="salary_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM" DefaultValue=""
                        Name="salary_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br/><br/>
            <asp:Label ID="gv2" runat="server" Text="<%$Resources:Resource,wfb2ia520_gv2%>" Visible="false"></asp:Label>
            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting2"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging2" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound2">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_CTYPE%>" SortExpression="CTYPE">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_CTYPE" runat="server" Text='<%#Bind("CTYPE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_YM%>" SortExpression="SALARY_YM">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM","{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_REATION_NAME%>" SortExpression="REATION_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_REATION_NAME" runat="server" Text='<%#Bind("REATION_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_AMT%>" SortExpression="INS_AMT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_INS_AMT" runat="server" Text='<%#Bind("INS_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_TOTAL%>" SortExpression="INS_TOTAL">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_INS_TOTAL" runat="server" Text='<%#Bind("INS_TOTAL","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SALARY_DT%>" SortExpression="SALARY_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_CMEMO%>" SortExpression="CMEMO">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_CMEMO" runat="server" Text='<%#Bind("CMEMO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow2" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount2" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!--grid3-->
            <asp:ObjectDataSource ID="ods3" runat="server" SelectMethod="GetData3"
                SelectCountMethod="GetCount3" TypeName="CFB2IA5200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting3"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected3">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_YEAR" DefaultValue=""
                        Name="salary_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM" DefaultValue=""
                        Name="salary_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br/>
            <asp:Label ID="gv3" runat="server" Text="<%$Resources:Resource,wfb2ia520_gv3%>" Visible="false"></asp:Label>
            <asp:GridView ID="gv_result3" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting3"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging3" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound3">
                
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_YM%>" SortExpression="SALARY_YM">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM","{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_TARGET_TYPE%>" SortExpression="TRAGE_TYPE_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_TARGET_TYPE" runat="server" Text='<%#Bind("TRAGE_TYPE_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_GINS_KIND%>" SortExpression="GINS_KIND">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_GINS_KIND" runat="server" Text='<%#Bind("GINS_KIND")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_GFEES_SELF%>" SortExpression="GFEES_SELF">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_GFEES_SELF" runat="server" Text='<%#Bind("GFEES_SELF","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_SALARY_DT%>" SortExpression="SALARY_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage3" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow3" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount3" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow3" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!--預設的工號,姓名-->
            <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
             <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
