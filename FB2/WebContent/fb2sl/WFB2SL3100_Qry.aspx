<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sl/action/WFB2SL3100_Qry.aspx.cs" Inherits="WebContent_fb2sl_WFB2SL3100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            getYear();
            iniForm();
        });

        function iniForm() {
            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_qry_DEPT_NAME').val("");
                }
            });
            getSALARY_DT();
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_EMP_DESC').attr("readonly", true);
            $('#txt_DATA_YM_search').mask("9999");
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
        function CheckYear(source, arguments) {
            var re = /^[0-9]{4}$/;
            if (re.test($("#txt_DATA_YM_search").val()) && $("#txt_DATA_YM_search").val() > 1911) {
                arguments.IsValid = true;
            }
            else {
                arguments.IsValid = false;
            }
        }
        function ClearAll() {
            $('#lb_SALARY_DT_S').text("");
            $('#lb_SALARY_DT_E').text("");
            $('#txt_DEPT_NO').val("");
            $('#txt_DEPT_NAME').val("");
            $('#txt_LICENSE_ID_search').val("");
            $('#ddl_WS_CD_search').val("");
            $('#ddl_EMP_STATUS').val("");
            $('#txt_DATA_YM_search').val("");
            $("#txt_EMP_ID").val($("#hid_defalut_EMP_ID").val());
            $("#txt_EMP_DESC").val($("#hid_defalut_EMP_NAME").val());
            // =rbl_BORROW_TYPE.ClientID
            //$('#<% %>').find("input[value=1]").prop("checked", "checked");
            getYear();
            getSALARY_DT();
            return false;
        }
        function getYear() {
            var datetime = new Date();
            var year = datetime.getFullYear();
            $('#txt_DATA_YM_search').val(year - 1);
        }
        function getSALARY_DT() {
            var year = $('#txt_DATA_YM_search').val();
            if (year != "") {
                $('#lb_SALARY_DT_S').text(year + "/01/01  ~");
                $('#lb_SALARY_DT_E').text(year + "/12/31");
            }
        }
        function OpenPERSON_ID() {
            var is_super = $("#hid_is_super").val();
            OpenEmpSearch('txt_EMP_ID', 'txt_EMP_DESC', is_super, '');

            //var ddl = $("#rbl_BORROW_TYPE input:radio:checked").val();
            //if (ddl == '1') {
            //    var is_super = $("#hid_is_super").val();
            //    OpenEmpSearch('txt_EMP_ID', 'txt_EMP_DESC', is_super, '');
            //} else {
            //    OpenSearch('Vendor_d_Search.aspx', 'txt_EMP_ID', 'txt_EMP_DESC', '', '');
            //}
        }
        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            } else {
                $("#txt_EMP_DESC").val("");
            }
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
                                <col width="19%" />
                                <col width="36%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YM_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YM_search" runat="server" MaxLength="4" Width="50px" onblur="getSALARY_DT();" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_Required_DATA_YM%>"
                                            ControlToValidate="txt_DATA_YM_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_YM_isError%>"
                                            ClientValidationFunction="CheckYear" ForeColor="Red"
                                            ControlToValidate="txt_DATA_YM_search" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                    <%--發薪期間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_SALARY_DT_S" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:Label ID="lb_SALARY_DT_E" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--部門--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="65px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" MaxLength="60" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--職種--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WS_CD_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_WS_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_WS_CD_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--工號/廠商代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_EMP_ID_search" Text="<%$Resources:Resource,wfb2sl_lb_EMP_ID%>" />:
                                    </th>
                                    <td>
                                        <%--<asp:RadioButtonList ID="rbl_BORROW_TYPE" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_PERSON%>" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_COMPANY%>" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"
                                            onblur="javascript:getEmpName(this.id)"></asp:TextBox>
                                        <input id="bt_PERSON_ID" type="button" value="..." onclick="OpenPERSON_ID();" />
                                        <asp:TextBox ID="txt_EMP_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--證號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_LICENSE_ID_search" Text="<%$Resources:Resource,wfb2sl_lb_LICENSE_ID%>" />:
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_LICENSE_ID_search" Width="80px" MaxLength="10" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                <tr>
                                    <%--狀態--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_EMP_STATUS_search" Text="<%$Resources:Resource,wfb2sl_lb_EMP_STATUS%>" />:
                                    </th>
                                    <td colspan="3">
                                        <asp:DropDownList runat="server" ID="ddl_EMP_STATUS" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SL310Generate" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL310Generate%>" Visible="true" OnClick="WFB2SL310Generate_Click" ValidationGroup="GroupA" />
<%--                                            <asp:Button ID="WFB2SL310Generate" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL310Generate%>" Visible="true" OnClick="WFB2SL310Generate_Click" ValidationGroup="GroupA" />--%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sl_WFB2SL310Clear%>" onclick="return ClearAll();" />
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
                    <td>
                        <%-- PDF產出及寄送電子郵件 --%>
<%--                        <aces:Btn ID="WFB2SL310Go" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL310Go%>" Visible="false" OnClick="WFB2SL310Go_Click" OnClientClick="BlockUI();" />--%>
                        <%-- 下載 --%>
<%--                        <aces:Btn ID="WFB2SL310Down" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL310Down%>" Visible="false"  OnClick="WFB2SL310Down_Click" />--%>
                        <asp:Button ID="WFB2SL310Go" runat="server" Text="<%$Resources:Resource,wfb2sk_WFB2SK120SendEmail%>" Visible="false" OnClick="WFB2SL310Go_Click" OnClientClick="BlockUI();" />
                        <asp:Button ID="WFB2SL310Down" runat="server" Text="<%$Resources:Resource,wfb2sk_WFB2SK120Down%>" Visible="false" OnClick="WFB2SL310Down_Click" />
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SL3100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YM_search"
                        Name="data_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_WS_CD_search"
                        Name="ws_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_LICENSE_ID_search"
                        Name="license_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_EMP_STATUS"
                        Name="emp_status" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1000px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sl_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Width="40px" Style="text-align: center;" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="240px" ItemStyle-Width="240px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" Style="word-wrap: break-word;" runat="server" Text='<%#Bind("DEPT_NO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--所得格式--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_TAX_FORMAT%>" SortExpression="TAX_FORMAT" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_TAX_FORMAT" Style="word-wrap: break-word;" runat="server" Text='<%#Bind("TAX_FORMAT_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號 / 廠商代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Style="word-wrap: break-word;" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名 / 廠商名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Style="word-wrap: break-word;" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職種--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--級數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_CD" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" runat="server" Style="word-wrap: break-word;" Text='<%#Bind("PJOB_CD")%>'></asp:Label>
                            <asp:HiddenField ID="hid_LICENSE_ID" runat="server" Value='<%#Bind("LICENSE_ID")%>'></asp:HiddenField>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LICENSE_ID" SortExpression="LICENSE_ID" Visible="false" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SALARY_EMAIL" SortExpression="SALARY_EMAIL" Visible="false" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
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
            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
            <!--預設的工號,姓名-->
            <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SL310Down" />          
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


