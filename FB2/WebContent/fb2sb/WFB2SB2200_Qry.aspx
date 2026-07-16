<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB2200_Qry.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2200_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $('#txt_SALARY_NAME').attr("readonly", true);
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".number").mask('9999/99/99');
            $('.empid').mask('99999');

            gridviewScroll();
            $.unblockUI();
            $('#txt_START_SDT_B,#txt_START_SDT_A').change(function () {
                if ($('#txt_START_SDT_B').val() != '' && $('#txt_START_SDT_A').val() != '') {
                    var b = new Date($('#txt_START_SDT_B').val());
                    var a = new Date($('#txt_START_SDT_A').val());
                    if (b > a) {
                        $(this).val("");
                        alert("起日不可大於迄日");
                    }
                }
            });
            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg == "1")
                                alert(JData.errMsg);
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                                $('#txt_LICENSE_ID').val(JData.LICENSE_ID);
                                $('#txt_BIRTH_DT').val(JData.BIRTH_DT);
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

            $('#txt_SALARY_ID').change(function () {
                if ($('#txt_SALARY_ID').val().length <= 4) {
                    //ajax 取得薪資
                    $.ajax({
                        url: "../commgeo/WFB2GetSalaryData.ashx",
                        data: {
                            SALARY_ID: $('#txt_SALARY_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                alert(JData.errMsg);

                                $('#txt_SALARY_ID').val("");
                                $('#txt_SALARY_NAME').val("");
                            }
                            else {
                                $('#txt_SALARY_ID').val(JData.SALARY_ID);
                                $('#txt_SALARY_NAME').val(JData.SALARY_NAME);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            });
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 6

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }
        function ClearAll() {
            $('#txt_SALARY_ID').val("");
            $('#txt_SALARY_NAME').val("");
            $('#txt_START_SDT_B').val("");
            $('#txt_START_SDT_A').val("");
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
            $('#ddl_EMP_CD').val(-1);
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;


            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function CheckApproveAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($("#hidwfb2sc_Approve_ConfirmMessage").val());
            else {
                alert($("#hidwfb2sc_action_NotChoiceMessage").val());
                return false;
            }
        }
        function CheckRejectAction() {
            if (LookUpCheckboxs() > 0) {
                return confirm($("#hidwfb2sc_Reject_ConfirmMessage").val());
            }
            else {
                alert($("#hidwfb2sc_action_NotChoiceMessage").val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2SB2200_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="13%" />
                                <col width="27%" />
                                <col width="13%" />
                                <col width="47%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_SALARY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ID" runat="server" MaxLength="4" Columns="10" ClientIDMode="Static"></asp:TextBox>
                                        <input id="Button8" type="button" value="..." onclick="OpenSearch('SalaryItem_Search.aspx', 'txt_SALARY_ID', 'txt_SALARY_NAME', '', '');" />
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" MaxLength="15" Columns="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_START_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_SDT_B" runat="server" MaxLength="15" Columns="10" ClientIDMode="Static" CssClass="date number"></asp:TextBox>
                                        ~
                                        <asp:TextBox ID="txt_START_SDT_A" runat="server" MaxLength="15" Columns="10" ClientIDMode="Static" CssClass="date number"></asp:TextBox>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_SDT_B" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_SDT_A" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_START_SDT_B"
                                            ControlToValidate="txt_START_SDT_A" ErrorMessage="<%$Resources:Resource,wfb2sb_lb_START_DT_error%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Columns="5" ClientIDMode="Static" CssClass="empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="15" Columns="5" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_EMP_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left"></th>
                                    <td align="left" class="Body_label"></td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SB2200Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2SB2200Search_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SB2200Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2SB2200Search_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>
                                            <input id="WFB2SB2200Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
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

                            <aces:Btn ID="WFB2SB2200Approve" runat="server" Text="<%$Resources:Resource,btn_approve%>" OnClick="WFB2SB2200Approve_Click" OnClientClick="return CheckApproveAction();" Visible="false" />
                            <aces:Btn ID="WFB2SB2200Reject" runat="server" Text="<%$Resources:Resource,btn_reject%>" OnClick="WFB2SB2200Reject_Click" OnClientClick="return CheckRejectAction();" Visible="false" />

                            <%--<asp:Button ID="WFB2SB2200Approve" runat="server" Text="<%$Resources:Resource,btn_approve%>" OnClick="WFB2SB2200Approve_Click" OnClientClick="return CheckApproveAction();" Visible="false" />
                            <asp:Button ID="WFB2SB2200Reject" runat="server" Text="<%$Resources:Resource,btn_reject%>" OnClick="WFB2SB2200Reject_Click" OnClientClick="return CheckRejectAction();" Visible="false" />--%>
                        </div>

                    </td>
                </tr>
            </table>
            <!--2SB2200_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SB2200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_ID"
                        Name="txt_SALARY_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_SDT_B"
                        Name="txt_START_SDT_B" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_SDT_B"
                        Name="txt_START_SDT_A" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_EMP_CD"
                        Name="ddl_EMP_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="txt_EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1950px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                                <asp:HiddenField ID="hid_SEQ_NO" runat="server" Value='<%#Bind("SEQ_NO")%>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lbl_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_EMP_CD" runat="server" Text='<%# Convert.ToString(Eval("EMP_CD_DESC1"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_SALARY_ID%>" SortExpression="SALARY_ID" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 150px;">
                                <asp:Label ID="lbl_SALARY_ID" runat="server" Text='<%# Convert.ToString(Eval("SALARY_NAME"))%>'></asp:Label>
                                <div style="display: none;">
                                    <asp:Label ID="hid_SALARY_ID" runat="server" Text='<%# Convert.ToString(Eval("SALARY_ID"))%>'></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_CHG_STATUS" runat="server" Text='<%#Bind("DESC2")%>'></asp:Label>
                                <div style="display: none;">
                                    <asp:Label ID="hid_CHG_STATUS" runat="server" Text='<%# Convert.ToString(Eval("CHG_STATUS"))%>'></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_B%>" SortExpression="CHG_AMT_B" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_CHG_AMT_B" runat="server" Text='<%# Convert.ToString(Eval("CHG_AMT_B","{0:N0}"))%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_A%>" SortExpression="CHG_AMT_A" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lbl_CHG_AMT_A" runat="server" Text='<%# Convert.ToString(Eval("CHG_AMT_A","{0:N0}"))%>'></asp:Label>
                                <asp:HiddenField ID="HID_CHG_AMT_A" runat="server" ClientIDMode="Static" Value='<%# Convert.ToString(Eval("CHG_AMT_A"))%>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_START_DT_B%>" SortExpression="START_DT_B" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_START_DT_B" runat="server" Text='<%#Bind("START_DT_B","{0:yyyy/MM/dd}")%>'></asp:Label>
                                <asp:HiddenField ID="hid_START_DT_A" runat="server" Value='<%#Bind("START_DT_A","{0:yyyy/MM/dd}")%>'></asp:HiddenField>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_END_DT_B%>" SortExpression="END_DATE_B" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_END_DATE_B" runat="server" Text='<%#Bind("END_DATE_B","{0:yyyy/MM/dd}")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_END_DT_A%>" SortExpression="END_DATE_A" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_END_DATE_A" runat="server" Text='<%#Bind("END_DATE_A","{0:yyyy/MM/dd}")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_CREATED_BY%>" SortExpression="CREATED_BY" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_CREATED_BY" runat="server" Text='<%# Convert.ToString(Eval("CREATED_DESC")).Trim() %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text='<%#Bind("DESC1")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lbl_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                                <%--<asp:TextBox ID="txt_REMARK" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("REMARK")%>' TextMode="MultiLine" Columns="45" Rows="3" ReadOnly="true" BorderWidth="0"></asp:TextBox>--%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sb_lb_APP_REMARK%>" SortExpression="APP_REMARK" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_APP_REMARK_Add" MaxLength="150" ClientIDMode="Static" runat="server" Text='<%#Bind("APP_REMARK")%>' TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                </EmptyDataTemplate>
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static">
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Reject_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Reject_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Approve_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Approve_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_action_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


