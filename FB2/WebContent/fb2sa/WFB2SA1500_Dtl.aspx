<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1500_Dtl.aspx.cs" Inherits="WebContent_fb2sa_WFB2SA1500_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ymd").mask('9999/99/99');
            //$(".money").formatNumber({ format: "#,###", locale: "tw" });
            //$('#txt_EMP_NAME1').attr("readonly", true);
            gridviewScroll();
            $.unblockUI();
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
                }
                //else {
                //    $('#txt_EMP_NAME').val("");
                //}
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
        //function empCheck(value) {
        //    if (value != "")
        //        __doPostBack('question', 'true');
        //    else
        //        $("#txt_EMP_NAME1").val("");
        //}


        //清空
        function doClear() {
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            $("#txt_EMP_ID").val("");
            //$("#txt_EMP_NAME1").val("");
            $("#txt_EMP_NAME").val("");
            return false;
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
                                <col width="30%" />
                                <col width="10%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR1%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YEAR" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_PROCESS_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PROCESS_STATUS" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_STARTandEND_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_JOIN_SDT" Width="81px" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>~ 
                                        <asp:TextBox ID="txt_JOIN_EDT" Width="81px" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MEM_CREATE_BY" runat="server" Text="<%$Resources:Resource,wfb2sa_MEM_CREATE_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MEM_CREATE_BY" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MEM_CREATE_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_MEM_CREATE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MEM_CREATE_DT" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="Div1">
                                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sa_cancel%>" OnClick="btn_cancel_Click" OnClientClick="BlockUI();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                        <tr>
                            <td colspan="5">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                                    <colgroup>
                                        <col width="17%" />
                                        <col width="33%" />
                                        <col width="16%" />
                                        <col width="19%" />
                                        <col width="25%" />

                                    </colgroup>
                                    <tbody>

                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_JOIN_DT%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <uc1:UCDateTimeRange runat="server" ID="UCDateTimeRange" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10" EndDateCssClass="ymd" StartDateCssClass="ymd"
                                                    CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="<%$Resources:Resource,wfb2sa_greaterthan_JOIN_DT%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2sa_error_JOIN_SDT%>"
                                                    ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2sa_error_JOIN_EDT%>" ClientValidationFunctionE="CheckDate" ClientValidationFunctionS="CheckDate" ValidationGroup="GroupA" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_EMP_ID%>"></asp:Label>:
                                            </th>
                                            <td>
                                                <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid"></asp:TextBox>
                                                <%--onblur="empCheck(this.value);"--%>
                                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                                <%--<asp:TextBox ID="txt_EMP_NAME1" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>--%>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2si_EMP_NAME%>"></asp:Label>
                                            </th>
                                            <td>
                                                <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <td align="right" class="Body_label">
                                                <div id="init">
                                                    <%--<asp:Btn ID="WFB2SA1500Search2" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SA1500Search2_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>

                                                    <asp:Button ID="WFB2SA1500Search2" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SA1500Search2_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                                    <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1500Clear%>" OnClientClick="return doClear();" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <hr />
                                            </td>
                                        </tr>

                                    </tbody>

                                </table>
                            </td>
                        </tr>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>


            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetDtlData2"
                SelectCountMethod="GetDtlCount" TypeName="CFB2SA1500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                        Name="join_sdt" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                        Name="join_edt" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DATA_YEAR" DefaultValue=""
                        Name="data_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />

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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" Width="60px" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" Width="80px" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_CD" Width="80px" runat="server" Text='<%#Bind("DESC1")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_LEVEL_CD%>" SortExpression="LEVEL_CD" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" Width="40px" runat="server" Text='<%#Bind("LEVEL_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_GRADE_CD%>" SortExpression="GRADE_CD" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRADE_CD" Width="40px" runat="server" Text='<%#Bind("GRADE_CD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_PJOB_CD%>" SortExpression="DESC2" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" Width="120px" runat="server" Text='<%#Bind("DESC2")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_STATUS%>" SortExpression="DESC3" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="80px" ID="lb_EMP_STATUS" runat="server" Text='<%#Bind("DESC3")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_JOIN_DT1%>" SortExpression="JOIN_DT" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="90px" ID="lb_JOIN_DT" runat="server" Text='<%#Bind("JOIN_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_BE_EMP_DT%>" SortExpression="BE_EMP_DT" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_BE_EMP_DT" Width="90px" runat="server" Text='<%#Bind("BE_EMP_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_ABILITY_PAY_B%>" SortExpression="ABILITY_PAY_B" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_ABILITY_PAY_B" Width="80px" runat="server" Text='<%#Bind("ABILITY_PAY_B","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_ABILITY_PAY_A%>" SortExpression="ABILITY_PAY_A" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_ABILITY_PAY_A" Width="80px" runat="server" Text='<%#Bind("ABILITY_PAY_A","{0:N0}")%>'></asp:Label>
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

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <%--<asp:HiddenField ID="hid_wfb2si_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Del_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_PayStatus_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Upd_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Upd_NotChoiceMessage" runat="server" ClientIDMode="Static" />--%>
        </ContentTemplate>


    </asp:UpdatePanel>
</asp:Content>
