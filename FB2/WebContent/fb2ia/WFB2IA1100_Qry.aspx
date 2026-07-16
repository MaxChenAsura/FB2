<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA1100_Qry.aspx.cs" Inherits="WebContent_WFB2IA1100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number2").mask('99.9');
            $(".numberr").css("text-align", "right");

            $("#txt_HR_CHG_DESC").attr("readonly", true);
            $("#txt_COMPANY_SNAME").attr("readonly", true);
            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_SUB_DESC").attr("readonly", true);

            var txt = $("#ddl_OP_STATUS").val();
            if (txt == undefined || txt != "E") {
                gridviewScroll();
            }
            else {
                gridviewScroll2();
            }

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
                barcolor: "#7F7F7F",
                headerrowcount: 2,
                freezesize: 4
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function gridviewScroll2() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                freezesize: 4
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function IsDelete() {
            var answer = confirm("確定要刪除?");
            if (answer)
                return true;
            else {
                document.getElementById('HID_cancel').click();
                return false;
            }
        }

        function getEmp() {
            var is_super = $("#hid_is_super").val();
            OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'Y','');
        }

        //清空畫面
        function ClearAll() {
            $("#txt_HR_CHG_CD").val("");
            $("#txt_HR_CHG_DESC").val("");
            $("#txt_COMPANY_CD_OLD").val("");
            $("#txt_COMPANY_SNAME").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_NATION_CD").val("");
            $("#txt_SUB_DESC").val("");
            $("#txt_OP_DT_S").val("");
            $("#txt_OP_DT_E").val("");
            $("#txt_CHG_DT_S").val("");
            $("#txt_CHG_DT_E").val("");
            $("#ddl_OPERATION_KIND").val("I");
            $("#ddl_OP_STATUS").val("N");
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm("確定要刪除?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function CheckProcessAction() {
            if (LookUpCheckboxs() > 0) {
                BlockUI();
                return true;
            }
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function checkDowning() {            
            if (Page_ClientValidate("GroupA")) {
                
            }
            else {
                return false;
            }
            var msg = "";
            if ($("#txt_OP_DT_S").val() == "" || $("#txt_OP_DT_E").val() == "") {
                msg += "處理日期起迄不允許空白!\n";
            }
            if ($("#txt_COMPANY_CD_OLD").val() == "") {
                msg += "聘用單位不允許空白!\n";
            }
            if (msg != "") {
                alert(msg);
                return false;
            }
            BlockUI();
            return true;
        }

        function doUnBlock() {
            $.unblockUI();
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="13%" />
                                <col width="27%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OPERATION_KIND" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_OPERATION_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_OPERATION_KIND" runat="server" BackColor="#FFD7D7" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OP_STATUS" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_OP_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_OP_STATUS" runat="server" BackColor="#FFD7D7" ClientIDMode="Static" OnSelectedIndexChanged="ddl_OP_STATUS_SelectedIndexChanged" ></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HR_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HR_CHG_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HR_CHG_CD" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" OnTextChanged="txt_HR_CHG_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="bt_HR_CHG_CD" type="button" value="..." onclick="OpenSearch('HrChangeCode_Search.aspx', 'txt_HR_CHG_CD', 'txt_HR_CHG_DESC', '', '');" />
                                        <asp:TextBox ID="txt_HR_CHG_DESC" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD_OLD" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_CD_OLD" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" OnTextChanged="txt_COMPANY_CD_OLD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="bt_COMPANY_CD_OLD_SEARCH" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD_OLD', 'txt_COMPANY_SNAME', '', '');" />
                                        <asp:TextBox ID="txt_COMPANY_SNAME" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="getEmp();" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_NATION_CD" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_NATION_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_NATION_CD" runat="server" MaxLength="3" Width="64px" ClientIDMode="Static" OnTextChanged="txt_NATION_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="bt_NATION_SEARCH" type="button" value="..." onclick="OpenSearch('CommCode_Search.aspx', 'txt_NATION_CD', 'txt_SUB_DESC', 'SYS_CD=HB&MAIN_CD=NATION_CD', '');" />
                                        <asp:TextBox ID="txt_SUB_DESC" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OP_DT_SE" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_OP_DT_SE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OP_DT_S" runat="server" ClientIDMode="Static" CssClass="date" Width="81px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_OP_DT_E" runat="server" ClientIDMode="Static" CssClass="date" Width="81px"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorOP_DT_S" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_OP_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_OP_DT_S" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorOP_DT_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_OP_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_OP_DT_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_OP_DT_S"
                                            ControlToValidate="txt_OP_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_OP_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CHG_DT_SE" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_DT_SE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CHG_DT_S" runat="server" ClientIDMode="Static" CssClass="date" Width="81px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_CHG_DT_E" runat="server" ClientIDMode="Static" CssClass="date" Width="81px"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorCHG_DT_S" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_CHG_DT_S" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorCHG_DT_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_CHG_DT_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_CHG_DT_S"
                                            ControlToValidate="txt_CHG_DT_E" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_CHG_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA1100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1100Search%>" OnClick="WFB2IA1100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2IA1100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1100Search%>" OnClick="WFB2IA1100Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ia_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                        <div id="init_grid">
                        <aces:Btn ID="WFB2IA1100Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1100Excel%>" OnClick="WFB2IA1100Excel_Click" Visible="false" OnClientClick="return checkDowning();"/>
                            <aces:Btn ID="WFB2IA1101Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1101Excel%>" OnClick="WFB2IA1101Excel_Click" Visible="false" OnClientClick="return checkDowning();" />
                            <aces:Btn ID="WFB2IA1100Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1100Process%>" OnClick="WFB2IA1100Process_Click" Visible="false" OnClientClick="return CheckProcessAction();" />
                            <aces:Btn ID="WFB2IA1100Del" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1100Del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA1100Del_Click" Visible="false" />

                           <%-- <asp:Button ID="WFB2IA1100Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1100Excel%>" OnClick="WFB2IA1100Excel_Click" Visible="false" OnClientClick="return checkDowning();"/>
                            <asp:Button ID="WFB2IA1101Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1101Excel%>" OnClick="WFB2IA1101Excel_Click" Visible="false" OnClientClick="return checkDowning();" />
                            <asp:Button ID="WFB2IA1100Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1100Process%>" OnClick="WFB2IA1100Process_Click" Visible="false" OnClientClick="return CheckProcessAction();" />
                            <asp:Button ID="WFB2IA1100Del" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1100Del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2IA1100Del_Click" Visible="false" />--%>
                        </div>
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IA1100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_OPERATION_KIND" DefaultValue=""
                        Name="operation_kind" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_HR_CHG_CD"
                        Name="hr_chg_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_COMPANY_CD_OLD"
                        Name="company_cd_old" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_NATION_CD"
                        Name="nation_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_OP_STATUS" DefaultValue=""
                        Name="op_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_OP_DT_S" DefaultValue=""
                        Name="op_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_OP_DT_E" DefaultValue=""
                        Name="op_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_CHG_DT_S" DefaultValue=""
                        Name="chg_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_CHG_DT_E" DefaultValue=""
                        Name="chg_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_EMP_NAME%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_PJOB_DESC%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_HR_CHG_CD%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_HR_CHG_DESC" runat="server" Text='<%#Bind("HR_CHG_DESC")%>' Width="80px"></asp:Label>
                            <asp:HiddenField ID="HID_HR_CHG_CD" runat="server" Value='<%#Bind("HR_CHG_CD")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_DT%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_DT" runat="server" Text='<%#Bind("CHG_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_LICENCE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_BIRTH_DT%>" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_BIRTH_DT" runat="server" Text='<%#Bind("BIRTH_DT")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="60px"></asp:Label>
                            <asp:HiddenField ID="HID_COMPANY_CD_NEW" runat="server" Value='<%#Bind("COMPANY_CD_NEW")%>' />
                            <asp:HiddenField ID="HID_COMPANY_CD_OLD" runat="server" Value='<%#Bind("COMPANY_CD_OLD")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_NATION_NAME%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUB_DESC" runat="server" Text='<%#Bind("SUB_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BASIC_SALARY%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_BASIC_SALARY" runat="server" Text='<%#Bind("BASIC_SALARY","{0:n0}")%>' ToolTip='<%#Bind("BASIC_SALARY")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_IS_YN%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_LABOR_IS_YN" runat="server" ClientIDMode="Static" Width="60px" Checked="true" />
                            <asp:HiddenField ID="HID_LABOR_IS_YN" runat="server" Value='<%#Bind("LABOR_IS_YN")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_DT_IN%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_LABOR_CHG_DT" runat="server" Text='<%#Bind("LABOR_CHG_DT", "{0:yyyy/MM/dd}")%>' ClientIDMode="AutoID"
                                Width="100px" CssClass="date"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LABOR_INS_AMT" runat="server" Text='<%#Bind("LABOR_INS_AMT","{0:n0}")%>' ToolTip='<%#Bind("LABOR_INS_AMT")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_IS_YN%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_HEALTH_IS_YN" runat="server" ClientIDMode="Static" Width="60px" Checked="true" />
                            <asp:HiddenField ID="HID_HEALTH_IS_YN" runat="server" Value='<%#Bind("HEALTH_IS_YN")%>' />
                            <asp:HiddenField ID="HID_PJOB_CD" runat="server" Value='<%#Bind("PJOB_CD")%>' />
                            <asp:HiddenField ID="HID_IsTWN" runat="server" Value='<%#Bind("IsTWN")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_DT_IN%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_HEALTH_CHG_DT" runat="server" Text='<%#Bind("HEALTH_CHG_DT", "{0:yyyy/MM/dd}")%>' ClientIDMode="AutoID"
                                Width="100px" CssClass="date"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_HEALTH_INS_AMT" runat="server" Text='<%#Bind("HEALTH_INS_AMT","{0:n0}")%>' ToolTip='<%#Bind("HEALTH_INS_AMT")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_IS_YN%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_PENSION_IS_YN" runat="server" ClientIDMode="Static" Width="60px" Checked="true" />
                            <asp:HiddenField ID="HID_PENSION_IS_YN" runat="server" Value='<%#Bind("PENSION_IS_YN")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_DT_IN%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_PENSION_CHG_DT" runat="server" Text='<%#Bind("PENSION_CHG_DT", "{0:yyyy/MM/dd}")%>' ClientIDMode="AutoID"
                                Width="100px" CssClass="date"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_AMT%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_PENSION_INS_AMT" runat="server" Text='<%#Bind("PENSION_INS_AMT","{0:n0}")%>' ToolTip='<%#Bind("PENSION_INS_AMT")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_SELF_RATIO%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_PENSION_SELF_RATIO" runat="server" Text='<%#Bind("PENSION_SELF_RATIO")%>' Width="120px" CssClass="number2 numberr"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_IS_YN%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_GINS_IS_YN" runat="server" ClientIDMode="Static" Width="60px" Checked="true" />
                            <asp:HiddenField ID="HID_GINS_IS_YN" runat="server" Value='<%#Bind("GINS_IS_YN")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_CHG_DT_IN%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_GROUP_CHG_DT" runat="server" Text='<%#Bind("GROUP_CHG_DT", "{0:yyyy/MM/dd}")%>' ClientIDMode="AutoID"
                                Width="100px" CssClass="date"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_OP_DT%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OP_DT" runat="server" Text='<%#Bind("OP_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_OP_MSG%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OP_MSG" runat="server" Text='<%#Bind("OP_MSG")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_NAME%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_PJOB_DESC%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HR_CHG_CD%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CHG_DT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_BIRTH_DT%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_COMPANY_CD%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_NATION_NAME%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_BASIC_SALARY%>" Width="80px"></asp:Label>
                            </td>
                        </tr>
                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2IA1100Excel" />
            <asp:PostBackTrigger ControlID="WFB2IA1101Excel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

