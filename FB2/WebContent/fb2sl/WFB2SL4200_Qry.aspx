<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sl/action/WFB2SL4200_Qry.aspx.cs" Inherits="WebContent_fb2sl_WFB2SL4200_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            getSALARY_DT();
            $('#txt_DATA_YM_search').mask('9999');
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_EMP_DESC').attr("readonly", true);
            gridviewScroll();
            $.unblockUI();

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
                    $('#txt_DEPT_NAME').val("");
                }
            });

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
            $('#lb_SALARY_DT').text("");
            $('#txt_DEPT_NO').val("");
            $('#txt_DEPT_NAME').val("");
            $('#txt_LICENSE_ID_search').val("");
            $('#ddl_WS_CD_search').val("");
            $('#ddl_EMP_STATUS').val("");
            $("#txt_EMP_ID").val($("#hid_defalut_EMP_ID").val());
            $("#txt_EMP_DESC").val($("#hid_defalut_EMP_NAME").val());
            getYear();
            getSALARY_DT();
            return false;
        }
        function getYear() {
            var datetime = new Date();
            var year = datetime.getFullYear();
            $('#txt_DATA_YM_search').val(year-1);
        }
        function getSALARY_DT() {
            var year = $('#txt_DATA_YM_search').val();
            if (year != "") {
                $('#lb_SALARY_DT').text(year + "/01/01  ~ " + year + "/12/31");
                $('#hid_SALARY_DT_S').val(year + "/01/01");
                $('#hid_SALARY_DT_E').val(year + "/12/31");
            }
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
                                <col width="18%" />
                                <col width="37%" />

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
                                        <asp:Label ID="lb_SALARY_DT" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:HiddenField ID="hid_SALARY_DT_S" runat="server" ClientIDMode="Static"></asp:HiddenField>
                                        <asp:HiddenField ID="hid_SALARY_DT_E" runat="server" ClientIDMode="Static"></asp:HiddenField>
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
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_EMP_ID_search" Text="<%$Resources:Resource,wfb2sl_lb_EMP_ID2%>" />:
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_EMP_ID" Width="64" MaxLength="5" ClientIDMode="Static"/>
                                        <input type="button" runat="server" id="btn_EMP_ID" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_DESC', 'N', ''); return false;" />
                                        <asp:TextBox runat="server" ID="txt_EMP_DESC" BorderWidth="0" ClientIDMode="Static" Style="background-color: white; color: black;" />
                                    </td>
                                    <%--證號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_LICENSE_ID_search" Text="<%$Resources:Resource,wfb2sl_lb_LICENCE_ID%>" />:
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
                                            <aces:Btn ID="WFB2SL4200Generate" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL310Generate%>" OnClick="WFB2SL4200Generate_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <%--<asp:Button ID="WFB2SL4200Generate" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL310Generate%>" OnClick="WFB2SL4200Generate_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2sl_WFB2SL310Clear%>" OnClientClick="return ClearAll();" />
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
                SelectCountMethod="getCount" TypeName="CFB2SL4200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YM_search"
                        Name="year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
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
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound"
                OnRowCommand="gv_result_RowCommand" >
                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sl_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                                <asp:Label ID="lb_RowNumber" runat="server" Width="40px" style="text-align:center;" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="270px" ItemStyle-Width="270px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                                <asp:Label ID="lb_DEPT_NO" style="word-wrap: break-word;" runat="server" Text='<%#Bind("DEPT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_EMP_ID2%>" SortExpression="EMP_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                                <asp:Label ID="lb_EMP_ID" runat="server" style="word-wrap: break-word;" Text='<%#Bind("EMP_ID")%>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_EMP_NAME2%>" SortExpression="EMP_NAME" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                                <asp:Label ID="lb_EMP_NAME" runat="server" style="word-wrap: break-word;" Text='<%#Bind("EMP_NAME")%>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職種--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                                <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                                <asp:Label ID="lb_PJOB_CD" runat="server" style="word-wrap: break-word;" Text='<%#Bind("PJOB")%>'></asp:Label>
                                 <asp:HiddenField ID="hid_LICENSE_ID" runat="server" Value='<%#Bind("LICENSE_ID")%>'></asp:HiddenField>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_WFB2SL410Function%>" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2SL4200Detail" runat="server" Width="80px"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sl_WFB2SL4100Detail%>" />
                          <%-- <asp:Button ID="WFB2SL4200Detail" runat="server" Width="80px"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sl_WFB2SL4100Detail%>" />--%>
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
            <!--預設的工號,姓名-->
            <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


