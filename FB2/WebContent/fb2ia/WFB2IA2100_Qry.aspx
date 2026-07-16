<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA2100_Qry.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA2100_Qry" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".ymd").mask('9999/99/99');
            $(".empid").mask('99999');
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
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_wfb2ia_Dtl_NotChoiceMessage').val());
                return false;
            }
        }

        //清空
        function doClear() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_LICENSE_ID").val("");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="30%" />
                                <col width="5%" />
                                <col width="10%" />
                                <col width="40%" />



                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass=" empid"></asp:TextBox> <%--MandatoryField--%>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_error_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="身分證號碼"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static" ></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>

                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA2100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA2100Search_Click" ValidationGroup="GroupA" OnClientClick="BlockUI();" />
                                            <aces:Btn ID="WFB2IA2100Detail_PDF" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2100Detail_PDF%>" OnClientClick="BlockUI();" OnClick="WFB2IA2100Detail_PDF_Click" />
                                            <%--<asp:Button ID="WFB2IA2100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA2100Search_Click" ValidationGroup="GroupA" OnClientClick="BlockUI();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="return doClear();" />
                                            <%--<asp:Button ID="WFB2IA2100Detail_PDF" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2100Detail_PDF%>" OnClientClick="BlockUI();" OnClick="WFB2IA2100Detail_PDF_Click" />--%>
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
                            <aces:Btn ID="WFB2IA2100Detail" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2100Detail%>" OnClick="WFB2IA2100Detail_Click" Visible="false" OnClientClick="return CheckDtlAction(); BlockUI();" />
                            <%--<asp:Button ID="WFB2IA2100Detail" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2100Detail%>" OnClick="WFB2IA2100Detail_Click" Visible="false" OnClientClick="return CheckDtlAction(); BlockUI();" />--%>
                        </div>

                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2IA2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LICENSE_ID" DefaultValue=""
                        Name="license_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <%--<HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>--%>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_BIRTH_DT%>" SortExpression="BIRTH_DT" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_BIRTH_DT" runat="server" Text='<%#Bind("BIRTH_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_LICENCE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_SUB_DESC%>" SortExpression="SUB_DESC" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SUB_DESC" runat="server" Text='<%#Bind("SUB_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_ENTRY_DT%>" SortExpression="INS_ENTRY_DT" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_INS_ENTRY_DT" runat="server" Text='<%#Bind("INS_ENTRY_DT","{0:yyyy/MM/dd}")%>' CssClass="ymd"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_INS_QUIT_DT%>" SortExpression="INS_QUIT_DT" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_INS_QUIT_DT" runat="server" Text='<%#Bind("INS_QUIT_DT","{0:yyyy/MM/dd}")%>' CssClass="ymd"></asp:Label>
                            <asp:HiddenField ID="hid_COMPANY_SNAME" runat="server" ClientIDMode="Static" Value='<%#Bind("COMPANY_SNAME")%>' />
                            <asp:HiddenField ID="hid_DIV_DEPT_FULL_NAME" runat="server" ClientIDMode="Static" Value='<%#Bind("DIV_DEPT_FULL_NAME")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                            </td>
                            <td>
                                <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2si_RowNum%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2ia_EMP_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_SUB_DESC%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_ENTRY_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_INS_QUIT_DT%>"></asp:Label>
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
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <!--resource-->
            <asp:HiddenField ID="hid_wfb2ia_Dtl_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_Dtl_NotChoiceMessage%>" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2IA2100Detail_PDF" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

