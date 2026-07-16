<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2200_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2200_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            getCurrent_DATA_YM();
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');            
            //工號取得姓名的ajax
            $("#txt_EMP_ID_search").blur(function () {
                if ($("#txt_EMP_ID_search").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME_search').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME_search').val(JData.EMP_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME_search').val("");
                }
            });
            gridviewScroll();
            $.unblockUI();
        }
        function getCurrent_DATA_YM() {
            var datatime = new Date();
            var year = datatime.getFullYear().toString();
            var month = (datatime.getMonth()).toString();
            if (month.length == 1)
                month = "0" + month;
            if ($("#txt_SALARY_YM_search").val() == "") {
                $("#txt_SALARY_YM_search").val(year + "/" + month);
            }
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
            if ($("#hid_FN_S_SALARY_YM").val() >= 6) {
                var data = $("#hid_FN_S_SALARY_YM").val().replace("/", "");
                data = data.substring(0, 4) + "/" + data.substring(4, 6);
                $('#txt_SALARY_YM_search').val(data);
            } else {
                $('#txt_SALARY_YM_search').val("");
            }
            $('#txt_SALARY_DT_search').val("");
            $('#txt_EMP_ID_search').val("");
            $('#txt_EMP_NAME_search').val("");
            getCurrent_DATA_YM();
            return false;
        }
        function CheckDetail1Action() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Detail1_Choice_Not_Equal_1_Message').val());
                return false;
            }
        }

        function CheckDetail2Action() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Detail2_Choice_Not_Equal_1_Message').val());
                return false;
            }
        }

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
        function CheckORDER_SEQ(source, arguments) {
            var re = /^\+?[0-9][0-9]*$/;
            if (!re.test($("#txt_ORDER_SEQ_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function proc_data() {
            var txt_SALARY_YM_search = $("#txt_SALARY_YM_search").val();
            $("#tmp_SALARY_YM_search").val("");
            if (txt_SALARY_YM_search != "")
                $("#tmp_SALARY_YM_search").val(txt_SALARY_YM_search.substring(0, 4) + '/' + txt_SALARY_YM_search.substring(4, 6) + '/01');
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
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <td height="10px"></td>
                                </tr>
                                <tr>
                                    <%--薪資年月--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_YM_search" runat="server" MaxLength="6" size="10" ClientIDMode="Static" CssClass="date2 ym MandatoryField"></asp:TextBox>
                                        <asp:TextBox ID="tmp_SALARY_YM_search" runat="server" size="10" Style="display: none;" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_YM2_isNull%> "
                                            ControlToValidate="txt_SALARY_YM_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_YM_Format_Error%>"
                                            ControlToValidate="txt_SALARY_YM_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT_search" runat="server" MaxLength="10" size="10" CssClass="date ymd" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CompareValidator ID="cv_SALARY_DT_search" runat="server"
                                            ControlToValidate="txt_SALARY_DT_search" ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_DT_Format_Error%>" Type="Date" Operator="DataTypeCheck"
                                            Display="None" ForeColor="Red" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" size="10" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_search', 'txt_EMP_NAME_search', 'N', '');" />                                        
                                    </td>
                                    <%--員工姓名--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME_search" runat="server" MaxLength="30" size="15" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC2200Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2SC2200Search_Click" OnClientClick="proc_data();CheckValid();" ValidationGroup="GroupA" />

                                            <%--<asp:Button ID="WFB2SC2200Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2SC2200Search_Click" OnClientClick="proc_data();CheckValid();" ValidationGroup="GroupA" />--%>
                                            
                                            <asp:Button ID="WFB2SC2200Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="return ClearAll();" />
                                            <asp:HiddenField ID="hid_SALARY_YM_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_SALARY_DT_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_EMP_ID_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_EMP_NAME_search" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_FN_S_SALARY_YM" ClientIDMode="Static" runat="server" />
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
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                        <aces:Btn ID="WFB2SC2200Detail1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2200Detail1%>" OnClientClick="return CheckDetail1Action();" OnClick="WFB2SC2200Detail1_Click" Visible="false" />
                            <aces:Btn ID="WFB2SC2200Detail2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2200Detail2%>" OnClientClick="return CheckDetail2Action();" OnClick="WFB2SC2200Detail2_Click" Visible="false" />

                            <%--<asp:Button ID="WFB2SC2200Detail1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2200Detail1%>" OnClientClick="return CheckDetail1Action();" OnClick="WFB2SC2200Detail1_Click" Visible="false" />
                            <asp:Button ID="WFB2SC2200Detail2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2200Detail2%>" OnClientClick="return CheckDetail2Action();" OnClick="WFB2SC2200Detail2_Click" Visible="false" />--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;"></td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC2200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_SALARY_YM_search"
                        Name="salary_ym" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_SALARY_DT_search"
                        Name="salary_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_EMP_ID_search"
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_EMP_NAME_search"
                        Name="emp_name" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="NO" ItemStyle-Width="30px" />
                    <asp:BoundField DataField="SALARY_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2sc_hd_SALARY_TYPE_DESC%>" ItemStyle-Width="105px" SortExpression="SALARY_TYPE_DESC" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SALARY_YM" HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>" ItemStyle-Width="100px" ItemStyle-CssClass="ym" SortExpression="SALARY_YM" />
                    <asp:BoundField DataField="SALARY_DT" HeaderText="<%$Resources:Resource,wfb2sc_hd_SALARY_DT%>" ItemStyle-Width="105px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="SALARY_DT" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_ID" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_NAME%>" ItemStyle-Width="100px" SortExpression="EMP_NAME" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SALARY_SDT" HeaderText="<%$Resources:Resource,wfb2sc_hd_SALARY_SDT%>" ItemStyle-Width="105px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="SALARY_SDT" />
                    <asp:BoundField DataField="SALARY_EDT" HeaderText="<%$Resources:Resource,wfb2sc_hd_SALARY_EDT%>" ItemStyle-Width="105px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="SALARY_EDT" />
                    <asp:BoundField DataField="DUTY_SDT" HeaderText="<%$Resources:Resource,wfb2sc_hd_DUTY_SDT%>" ItemStyle-Width="105px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="DUTY_SDT" />
                    <asp:BoundField DataField="DUTY_EDT" HeaderText="<%$Resources:Resource,wfb2sc_hd_DUTY_EDT%>" ItemStyle-Width="105px" DataFormatString="{0:yyyy/MM/dd}" SortExpression="DUTY_EDT" />
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail1_Choice_Not_Equal_1_Message" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Detail2_Choice_Not_Equal_1_Message" />
        </ContentTemplate>

    </asp:UpdatePanel>
    <asp:ValidationSummary ID="vs1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>


