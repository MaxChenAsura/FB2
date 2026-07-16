<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sl/action/WFB2SL1100_Qry.aspx.cs" Inherits="WebContent_fb2sl_WFB2SL1100_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            gridviewScroll();
            $.unblockUI();
            $('#txt_COMPANY_NAME_search').attr("readonly", true);
            $('#txt_DATA_YM_search').mask('9999');

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
                                $('#txt_EMP_DESC').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_DESC').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_DESC').val("");
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
        function ClearAll() {
            $('#txt_COMPANY_CD_search').val("");
            $('#txt_COMPANY_NAME_search').val("");
            $('#txt_DATA_YM_search').val("");
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_DESC').val("");
            $('#ddl_TAX_FORMAT').val("");
            $('#ddl_DATA_FORMAT').val("");
            getYear();
        }
        function getYear() {
            var datetime = new Date();
            var year = datetime.getFullYear();
            $('#txt_DATA_YM_search').val(year - 1);
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
                                <col width="30%" />
                                <col width="15%" />
                                <col width="40%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--公司別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_COMPANY_CD_search%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_CD_search" runat="server" MaxLength="1" Width="50px" class="MandatoryField"
                                            OnTextChanged="txt_COMPANY_CD_search_TextChanged" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_COMPANY_SEARCH" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD_search', 'txt_COMPANY_NAME_search', '', '');" />
                                        <asp:TextBox ID="txt_COMPANY_NAME_search" runat="server" BorderWidth="0" MaxLength="60" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_COMPANY_CD_search_isNull%>"
                                            ControlToValidate="txt_COMPANY_CD_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YM_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YM_search" runat="server" MaxLength="4" Width="50px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_YM_isNull%>"
                                            ControlToValidate="txt_DATA_YM_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_YM_isError%>"
                                            ControlToValidate="txt_DATA_YM_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_EMP_ID_search" Text="<%$Resources:Resource,wfb2sl_lb_EMP_ID2%>" />
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_EMP_ID" Width="64" MaxLength="5" ClientIDMode="Static" />
                                        <input type="button" runat="server" id="btn_EMP_ID" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_DESC', 'N', ''); return false;" />

                                    </td>
                                    <%--姓名--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_EMP_DESC" Text="<%$Resources:Resource,wfb2sl_lb_EMP_NAME2%>" />
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_EMP_DESC" ClientIDMode="Static" Width="80" MaxLength="30" />
                                    </td>
                                </tr>
                                <tr>
                                    <%--所得格式--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_TAX_FORMAT_search" Text="<%$Resources:Resource,wfb2sl_lb_TAX_FORMAT%>" />
                                    </th>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddl_TAX_FORMAT" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--資料格式--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_DATA_FORMAT_search" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT%>" />
                                    </th>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddl_DATA_FORMAT" ClientIDMode="Static">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="A" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_A%>"></asp:ListItem>
                                            <asp:ListItem Value="D" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_D%>"></asp:ListItem>
                                            <asp:ListItem Value="V" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_V%>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SL1100Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2SL1100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <%--<asp:Button ID="WFB2SL1100Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2SL1100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL310Clear%>" OnClientClick="ClearAll();" />
                                           <aces:Btn id="WFB2SL1100Process" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL1100Process%>" OnClientClick="location.href = 'WFB2SL1100_ImportExcel.aspx';" />
                                              <%--<asp:Button ID="WFB2SL1100Process" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL1100Process%>" OnClientClick="location.href = 'WFB2SL1100_ImportExcel.aspx';" />--%>
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
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SL1100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_COMPANY_CD_search"
                        Name="company_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DATA_YM_search"
                        Name="data_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="txt_EMP_DESC"
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_TAX_FORMAT"
                        Name="tax_format" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_DATA_FORMAT"
                        Name="data_format" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1000px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sl_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Width="40px" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資料格式--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT%>" SortExpression="DATA_FORMAT" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_DATA_FORMAT" Style="word-wrap: break-word;" runat="server" Text='<%#Bind("DATA_FORMAT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--公司別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_COMPANY_CD_search%>" SortExpression="COMPANY_SNAME" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_COMPANY_SNAME" runat="server" Style="word-wrap: break-word;" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號 / 廠商代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Style="word-wrap: break-word;" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--統一編(證)號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_LICENSE_ID2%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_EMP_NAME2%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Style="word-wrap: break-word;" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--所得格式--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_TAX_FORMAT%>" SortExpression="TAX_FORMAT" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_TAX_FORMAT" runat="server" Text='<%#Bind("TAX_FORMAT_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--稅額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_TAX2%>" SortExpression="TAX" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TAX" runat="server" Text='<%#Bind("TAX","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--郵遞區號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_CONTACT_ZIP_CD%>" SortExpression="CONTACT_ZIP_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CONTACT_ZIP_CD" runat="server" Style="word-wrap: break-word;" Text='<%#Bind("CONTACT_ZIP_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--地址--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_CONTACT_ADDR%>" SortExpression="CONTACT_ADDR" HeaderStyle-Width="190px" ItemStyle-Width="190px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CONTACT_ADDR" runat="server" Style="word-wrap: break-word;" Text='<%#Bind("CONTACT_ADDR")%>'></asp:Label>
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

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


