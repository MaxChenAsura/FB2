<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2hd/WFB2HD0100_Qry.aspx.cs" Inherits="WebContent_fb2hd_WFB2HD0100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
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
                        cache: false,
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
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 4

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }
        function ClearAll() {
            $('#txt_EMP_ID,#txt_EMP_NAME,#txt_DOC_NO,#txt_START_DT_S,#txt_START_DT_E').val("");
            $('#ddl_JUDGEMENT_TYPE,#ddl_REASON_CD').val(-1);

        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
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

        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();

            if (checkboxsSelected() > 0) {
                processed = confirm($('#hidwfb299_Del_ConfirmMessage').val());
            } else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            choietr = null;
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        //查詢前檢查
        function searchCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }
        function CheckModeifyAction() {
            if (checkboxsSelected() == 1)
                return true;
            else {
                alert($('#hidwfb299_Mod_NotChoiceMessage').val());
                return false;
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2HD0100_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                                <col width="15%" />
                                <col width="30%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" MaxLength="5" Columns="10" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Columns="10" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_JUDGEMENT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_JUDGEMENT_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_JUDGEMENT_TYPE" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_JUDGEMENT_TYPE_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DOC_NO" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_DOC_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DOC_NO" runat="server" MaxLength="20" Columns="10" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2_lb_START_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        ~
                                         <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <!--驗證日期格式-->
                                        <asp:CustomValidator ID="RegularExpressionValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hd_format_start_dt_s%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="RegularExpressionValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hd_format_start_dt_e%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <!--起日不可大於迄日 -->
                                        <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_START_DT_S"
                                            ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2hd_error_start_dt_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REASON_CD" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_REASON_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:DropDownList ID="ddl_REASON_CD" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2HD0100Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2HD0100Search_Click" OnClientClick="return searchCheck();" />
                                            <%--<asp:Button ID="WFB2HD0100Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2HD0100Search_Click" OnClientClick="return searchCheck();" />--%>
                                            <input id="WFB2HD0100Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
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
                                                    
                            <aces:Btn ID="WFB2HD0100Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2HD0100Add_Click" />
                            <aces:Btn ID="WFB2HD0100Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return doDelete();" OnClick="WFB2HD0100Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2HD0100Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HD0100Edit_Click" Visible="False" />
                            
                            <%--
                            <asp:Button ID="WFB2HD0100Add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="WFB2HD0100Add_Click" />
                            <asp:Button ID="WFB2HD0100Delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return doDelete();" OnClick="WFB2HD0100Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2HD0100Edit" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HD0100Edit_Click" Visible="False" />--%>
                        </div>

                    </td>
                </tr>
            </table>
            <!--2HD0100_QRY結束-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HD0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="txt_EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="txt_EMP_NAME" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_JUDGEMENT_TYPE"
                        Name="ddl_JUDGEMENT_TYPE" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DOC_NO"
                        Name="txt_DOC_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_DT_S"
                        Name="txt_START_DT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_DT_E"
                        Name="txt_START_DT_E" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_REASON_CD"
                        Name="ddl_REASON_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1500px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hd_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hd_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2hd_lb_DEPT_NAME%>" SortExpression="DEPT_NAME" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2hd_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PJOB_DESC" HeaderText="<%$Resources:Resource,wfb2hd_lb_PJOB_DESC%>" SortExpression="PJOB_DESC" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DOC_NO" HeaderText="<%$Resources:Resource,wfb2hd_lb_DOC_NO%>" SortExpression="DOC_NO" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="60px" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="JUDGEMENT_TYPE_str" HeaderText="<%$Resources:Resource,wfb2hd_lb_JUDGEMENT_TYPE%>" SortExpression="JUDGEMENT_TYPE" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="FIRST_CNT" HeaderText="<%$Resources:Resource,wfb2hd_lb_FIRST_CNT%>" SortExpression="FIRST_CNT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="SECOND_CNT" HeaderText="<%$Resources:Resource,wfb2hd_lb_SECOND_CNT%>" SortExpression="SECOND_CNT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="THIRD_CNT" HeaderText="<%$Resources:Resource,wfb2hd_lb_THIRD_CNT%>" SortExpression="THIRD_CNT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                    <asp:CheckBoxField DataField="IS_FIRE_b" HeaderText="<%$Resources:Resource,wfb2hd_lb_IS_FIRE%>" SortExpression="IS_FIRE" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="REASON_CD_str" HeaderText="<%$Resources:Resource,wfb2hd_lb_REASON_CD%>" SortExpression="REASON_CD" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
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
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


