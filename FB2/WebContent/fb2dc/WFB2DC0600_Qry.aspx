<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0600_Qry.aspx.cs" Inherits="WebContent_fb2dc_WFB2DC0600_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".dateym").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');            

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

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_DEPT_NO").blur(function () {
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

            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        //清空畫面
        function ClearAll() {
            $("#txt_ABNORMAL_DT_S").val("");
            $("#txt_ABNORMAL_DT_E").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_ABNORMAL_TYPE").val("-1");
            $("#ddl_ABNORMAL_REASON_CD").val("-1");
            $("#ddl_ABNORMAL_SOURCE_CD").val("-1");
            $("#txt_IFLOW_NO").val("");
            $("#txt_IFLOW_APPROVE_DT").val("");

        }

        function CheckSearch() {

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
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hid_wfb2sk_Del_ConfirmMessage').val());
            else {
                alert($('#hid_wfb2sk_Del_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert('<%=Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage%>');
                return false;
            }
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
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="25%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="5%" />
                    <col width="5%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ABNORMAL_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="6">
                            <asp:TextBox ID="txt_ABNORMAL_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ymd" ></asp:TextBox>
                            ~
                     <asp:TextBox ID="txt_ABNORMAL_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ymd" ></asp:TextBox>
                            <asp:CustomValidator ID="cv_ABNORMAL_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_ABNORMAL_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_ABNORMAL_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cv_ABNORMAL_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_ABNORMAL_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_ABNORMAL_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_ABNORMAL_DT_S"
                                ControlToValidate="txt_ABNORMAL_DT_E" ErrorMessage="申請異常刷卡起曰不可大於迄日!" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_Reguired_ABNORMAL_DT_S%>"
                                ControlToValidate="txt_ABNORMAL_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None" ></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_Reguired_ABNORMAL_DT_E%>"
                                ControlToValidate="txt_ABNORMAL_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None" ></asp:RequiredFieldValidator>
                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ABNORMAL_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_TYPE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ABNORMAL_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ABNORMAL_REASON_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_REASON_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ABNORMAL_REASON_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ABNORMAL_SOURCE_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_SOURCE_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ABNORMAL_SOURCE_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th></th>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IFLOW_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_NO" runat="server" MaxLength="20" Width="150px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IFLOW_APPROVE_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_APPROVE_DT" runat="server" Width="81px" ClientIDMode="Static" CssClass="dateym ym"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2df_ERR_IFLOW_APPROVE_DT%>" ControlToValidate="txt_IFLOW_APPROVE_DT" ForeColor="Red"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </td>
                        <th align="left">&nbsp;</th>
                        <td align="left" class="Body_label">&nbsp;</td>
                        <th></th>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DC0600Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Search%>" OnClick="WFB2DC0600Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                                <%--<asp:Button ID="WFB2DC0600Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Search%>" OnClick="WFB2DC0600Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>

                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>
                    </tr>


                    <!-- end: Create MODULE ID -->
                    <!-- START: Create a line to separate Search field with body field -->
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                        <aces:Btn ID="WFB2DC0600Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Add%>" OnClick="WFB2DC0600Add_Click" />
                            <aces:Btn ID="WFB2DC0600BatchApply" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600BatchApply%>" OnClick="WFB2DC0600BatchApply_Click" />
                            <aces:Btn ID="WFB2DC0600Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DC0600Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2DC0600Edit" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DC0600Edit_Click" Visible="False" />

<%--                            <asp:Button ID="WFB2DC0600Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Add%>" OnClick="WFB2DC0600Add_Click" />
                            <asp:Button ID="WFB2DC0600BatchApply" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600BatchApply%>" OnClick="WFB2DC0600BatchApply_Click" />
                            <asp:Button ID="WFB2DC0600Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DC0600Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2DC0600Edit" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DC0600Edit_Click" Visible="False" />--%>
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DC0600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_ABNORMAL_DT_S"
                        Name="abnormal_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_ABNORMAL_DT_E" DefaultValue=""
                        Name="abnormal_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_ABNORMAL_TYPE"
                        Name="abnormal_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_ABNORMAL_REASON_CD" DefaultValue=""
                        Name="abnormal_reason_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_ABNORMAL_SOURCE_CD" DefaultValue=""
                        Name="abnormal_source_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_IFLOW_NO" DefaultValue=""
                        Name="iflow_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_IFLOW_APPROVE_DT" DefaultValue=""
                        Name="iflow_approve_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1260px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2dc_lb_DEP_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2dc_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2dc_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ABNORMAL_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_TYPE%>" SortExpression="ABNORMAL_TYPE" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ABNORMAL_REASON_DESC" HeaderText="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_REASON_CD%>" SortExpression="ABNORMAL_REASON_CD" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="CALENDAR_DT" HeaderText="<%$Resources:Resource,wfb2dc_lb_CALENDAR_DT%>" SortExpression="CALENDAR_DT" HeaderStyle-Width="60px" />
                    <asp:BoundField DataField="ABNORMAL_DT" HeaderText="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_DT2%>" SortExpression="ABNORMAL_DT" HeaderStyle-Width="140px" />
                    <asp:BoundField DataField="ABNORMAL_SOURCE_DESC" HeaderText="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_SOURCE_CD%>" SortExpression="ABNORMAL_SOURCE_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2dc_lb_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="IFLOW_APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2dc_lb_IFLOW_APPROVE_DT2%>" SortExpression="IFLOW_APPROVE_DT" HeaderStyle-Width="60px" />
                    <asp:BoundField DataField="IS_CONFIRM" HeaderText="<%$Resources:Resource,wfb2dc_lb_IS_CONFIRM%>" SortExpression="IS_CONFIRM" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="IS_RE_MAKE" HeaderText="<%$Resources:Resource,wfb2dc_lb_IS_RE_MAKE%>" SortExpression="IS_RE_MAKE" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left" />

                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2sk_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Del_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Del_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Del_ConfirmMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
