<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0200_Qry.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".ymd").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ymd").mask("9999/99/99");
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_ACC_DEPT_NAME').attr("readonly", true);

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").blur(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptAllData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
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
            //parentFuncID
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                freezesize: 6

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }


        //清空畫面
        function ClearAll() {
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_DEPT_LEVEL").val("-1");
            $("#ddl_ORG_TYPE").val("-1");
            //$("#ddl_ACC_SALARY_CD").val("-1");
            $("#ddl_ACC_CD").val("-1");
            $("#txt_ACC_DEPT_NO").val("");
            $("#txt_ACC_DEPT_NAME").val("");
            $("#txt_START_DT_S").val("");
            $("#txt_START_DT_E").val("");
            $("#txt_END_DT_S").val("");
            $("#txt_END_DT_E").val("");
            $('#rbl_IS_VALID').find("input[value='Y']").prop("checked", "checked");
            $('#txt_DEPT_NAME_search').val("");
        }
        //開啟部門基本資料維護
        function openHA0210(id) {
            window.open("WFB2HA0210_Qry.aspx?dept_no=" + id + "&parentFuncId=" + parentFuncID, "HA0210");
        }

        //function CheckSearch() {

        //    if ($("#txt_START_DT_S").val() == "" && $("#txt_START_DT_E").val() == "") {
        //        if ($("#rbl_IS_VALID input:radio:checked").val() != "ALL") {
        //            alert("未輸入生效期間，不可以點選「有效」、「無效」");
        //            return false;
        //        }
        //    }

        //    if (Page_ClientValidate("GroupA")) {
        //        BlockUI();
        //    }
        //}

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert($('#hid_chooseOneMessage').val());
                return false;
            }
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                var rtnval = confirm($('#hid_delete_ConfirmMessage').val());
                if (rtnval) BlockUI();
                return rtnval;
            }
            else {
                alert($('#hid_notChooseMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("#gv_result [type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function getACC_DEPT_Name(ACC_DEPT_id) {
            var txt = document.getElementById(ACC_DEPT_id);
            if (txt.value.length != "") {
                document.getElementById('hid_getACC_DEPT_Name').click();
                return false;
            } else {
                $("#txt_ACC_DEPT_NAME").val("");
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                                <col width="11%" />
                                <col width="14%" />
                                <col width="10%" />
                                <col width="15%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:HiddenField ID="hid_DEPT_NO" ClientIDMode="Static" runat="server" />
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="140px" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_LEVEL" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL%>"></asp:Label>:
                                    </td>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:DropDownList ID="ddl_DEPT_LEVEL" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:HiddenField ID="hid_DEPT_LEVEL" ClientIDMode="Static" runat="server" />
                                    </td>
                                    <td align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ORG_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ORG_TYPE%>"></asp:Label>:
                                    </td>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_ORG_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:HiddenField ID="hid_ORG_TYPE" ClientIDMode="Static" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <%--<th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACC_SALARY_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_SALARY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_ACC_SALARY_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>--%>
                                    <td align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACC_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_CD%>"></asp:Label>:
                                    </td>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:DropDownList ID="ddl_ACC_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:HiddenField ID="hid_ACC_CD" ClientIDMode="Static" runat="server" />
                                    </td>
                                    <td align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACC_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_DEPT_NO%>"></asp:Label>:
                                    </td>
                                    <td align="left" class="Body_label" colspan="4">
                                        <asp:TextBox ID="txt_ACC_DEPT_NO" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" onblur="javascript:getACC_DEPT_Name(this.id)"></asp:TextBox>
                                        <asp:HiddenField ID="hid_ACC_DEPT_NO" ClientIDMode="Static" runat="server" />
                                        <input id="bt_ACC_DEPT_NO" type="button" value="..." onclick="OpenSearch('DeptAcc_Search.aspx', 'txt_ACC_DEPT_NO', 'txt_ACC_DEPT_NAME', '');" />
                                        <asp:TextBox ID="txt_ACC_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--生效日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_start_dt%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                                        ~
                                <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                                        <asp:HiddenField ID="hid_START_DT_S" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="hid_START_DT_E" ClientIDMode="Static" runat="server" />
                                        <asp:CustomValidator ID="cv_START_DT_S" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cv_START_DT_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT_S"
                                            ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_EFFECT_DT_RANGE3%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                    <%--結束日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_end_dt%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:TextBox ID="txt_END_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                                        ~
                                <asp:TextBox ID="txt_END_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_END_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_END_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_END_DT_S"
                                            ControlToValidate="txt_END_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_EFFECT_END_DT_RANGE3%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:RadioButtonList ID="rbl_IS_VALID" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2ha_lb_IS_VALID_Y%>" Value="Y" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2ha_lb_IS_VALID_N%>" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2ha_lb_IS_VALID_ALL%>" Value="ALL"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:HiddenField ID="hid_IS_VALID" ClientIDMode="Static" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="2">
                                        <asp:TextBox ID="txt_DEPT_NAME_search" runat="server" MaxLength="60" Width="120px" ClientIDMode="Static"></asp:TextBox>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="2">
                                        <div id="init">
                                            <aces:Btn ID="WFB2HA0200Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Search%>" OnClick="WFB2HA0200Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <aces:Btn ID="WFB2HA0101Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Search%>" OnClick="WFB2HA0200Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <%--
                                                <asp:Button ID="WFB2HA0200Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Search%>" OnClick="WFB2HA0200Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"/>
                                                <asp:Button ID="WFB2HA0200Upload" runat="server" Text="<%$Resources:Resource,wfb2ha_btn_upload%>" OnClick="WFB2HA0200Upload_Click" />
                                                --%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ha_btn_clear%>" onclick="ClearAll();" />

                                            <aces:Btn ID="WFB2HA0200Upload" runat="server" Text="<%$Resources:Resource,wfb2ha_btn_upload%>" OnClick="WFB2HA0200Upload_Click" />


                                            <asp:HiddenField ID="hid_cancel_ConfirmMessage" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_delete_ConfirmMessage" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_notChooseMessage" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hid_chooseOneMessage" ClientIDMode="Static" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" height="1" colspan="10">
                        <hr>
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2HA0200Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Add%>"  OnClick="WFB2HA0200Add_Click" />
                            <aces:Btn ID="WFB2HA0200Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA0200Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2HA0200Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0200Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2HA0200Detail" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Dtl%>" OnClientClick="return CheckModeifyAction();" Visible="false" OnClick="WFB2HA0200Dtl_Click" />

                            <aces:Btn ID="WFB2HA0101Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Add%>" OnClick="WFB2HA0200Add_Click" />
                            <aces:Btn ID="WFB2HA0101Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA0200Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2HA0101Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0200Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2HA0101Detail" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Dtl%>" OnClientClick="return CheckModeifyAction();" Visible="false" OnClick="WFB2HA0200Dtl_Click" />
                            <%--<asp:Button ID="WFB2HA0200Add" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Add%>" OnClick="WFB2HA0200Add_Click" />
                            <asp:Button ID="WFB2HA0200Delete" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2HA0200Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2HA0200Edit" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2HA0200Edit_Click" Visible="false" />
                            <asp:Button ID="WFB2HA0200Detail" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Dtl%>" OnClientClick="return CheckModeifyAction();" Visible="false" OnClick="WFB2HA0200Dtl_Click" />--%>
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_DEPT_LEVEL" DefaultValue=""
                        Name="dept_level" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_ORG_TYPE" DefaultValue=""
                        Name="org_type" PropertyName="Value" Type="String" />
                    <%--<asp:ControlParameter ControlID="ddl_ACC_SALARY_CD" DefaultValue=""
                        Name="acc_salary_cd" PropertyName="SelectedValue" Type="String" />--%>
                    <asp:ControlParameter ControlID="hid_ACC_CD" DefaultValue=""
                        Name="acc_cd" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_ACC_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="acc_dept_no" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_START_DT_S" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_s" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_START_DT_E" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_e" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="txt_END_DT_S"
                        Name="end_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_END_DT_E"
                        Name="end_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_IS_VALID" DefaultValue=""
                        Name="is_valid" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="txt_DEPT_NAME_search" DefaultValue=""
                        Name="dept_name" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1280px"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2ha_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="DEPT_LEVEL_DESC" HeaderText="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL_DESC%>" SortExpression="DEPT_LEVEL" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2ha_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_DEPT_NAME%>" SortExpression="DEPT_NAME" HeaderStyle-Width="180px" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="180px" Style="text-align: left; word-wrap: break-word;" ID="DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DEFAULT_PLANT" HeaderText="<%$Resources:Resource,wfb2ha_DEFAULT_PLANT%>" SortExpression="DEFAULT_PLANT" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2ha_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2ha_START_DT%>" SortExpression="START_DT" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2ha_END_DT%>" SortExpression="END_DT" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="ORG_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2ha_ORG_TYPE_DESC%>" SortExpression="ORG_TYPE" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ACC_DESC" HeaderText="<%$Resources:Resource,wfb2ha_ACC_DESC%>" SortExpression="ACC_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ACC_DEPT_DESC" HeaderText="<%$Resources:Resource,wfb2ha_ACC_DEPT_DESC%>" SortExpression="ACC_DEP_NO" HeaderStyle-Width="180px" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="REMARK" HeaderText="<%$Resources:Resource,wfb2ha_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_function%>" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <input id="btn_SubDetail" type="button" value="<%$Resources:Resource,wfb2ha_btn_SubDetail%>" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>

            <asp:HiddenField ID="HID_parentFuncID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="hid_getACC_DEPT_Name" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getACC_DEPT_Name_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
