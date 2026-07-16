<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0200_Qry.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0200_Qry" Culture="auto" UICulture="auto" %>

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

            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_NEW_DEPT_NO").attr("readonly", true);

            gridviewScroll();
            $.unblockUI();

            //Grid部門代號取得部門名稱的ajax
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

        //清空畫面
        function ClearAll() {
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_WORK_CD").val("-1");
            $("#txt_START_DT").val("");
            $("#txt_END_DT2").val("");
            $('#rbl_USE_STATUS').find("input[value='Y']").prop("checked", "checked");
        }

        function getDEPT_NO(type) {

            if (type == "search") {
                //var dept_no = $("#txt_DEPT_NO").val();
                //OpenSearch('DeptGrid_Search.aspx', 'txt_DEPT_NO', 'txt_DEPT_NAME', 'DEPT_NO=' + dept_no);
                OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', "N");
            } else {
                //var new_dept_no = $("#txt_NEW_DEPT_NO").val();
                //OpenSearch('DeptGrid_Search.aspx', 'txt_NEW_DEPT_NO', 'txt_NEW_DEPT_NAME', 'DEPT_NO=' + new_dept_no);
                OpenDeptSearch('txt_NEW_DEPT_NO', 'txt_NEW_DEPT_NAME', "N");
            }
        }

        function OpenMultiSelect2(targetObj, TableName, TextColumn, ValueColumn, WhereColumn) {
            var WhereValue = "";
            var SelectValue = "";
            if ($("#txt_NEW_DEPT_NO").val() != "") {
                SelectValue = $("#txt_NEW_DEPT_NO").val();
            }
            OpenMultiSelectNew(targetObj, TableName, TextColumn, ValueColumn, WhereColumn, WhereValue, SelectValue, '');//跑公用程式開窗

            //var returnValue = window.showModalDialog("../comm/Multi_Select.aspx?TableName=" + TableName + "&TextColumn=" + TextColumn + "&ValueColumn=" + ValueColumn +
            //                    "&WhereColumn=" + WhereColumn + "&WhereValue=" + WhereValue + "&SelectValue=" + SelectValue + "&parentFuncId=" + parentFuncID,
            //                    self, 'dialogWidth=520px;dialogHeight=400px;scroll=no');
            //if (returnValue == undefined) {
            //    returnValue = window.returnValue;
            //}
            //if (!(typeof returnValue === 'undefined')) {

            //    $("#" + targetObj).val(returnValue);
            //}
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
            if (LookUpCheckboxs() > 0) {
                if (confirm("確定要刪除?")) {
                    BlockUI();
                    return true;
                } else {
                    return false;
                }
            }
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function getDEPT_NAME(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getDEPT_NAME').click();
                return false;
            } else {
                $("#txt_DEPT_NAME").val("");
            }
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupB")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed) {
                $.unblockUI();
                return;
            }

            return processed;
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
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="btn_DEPT_NO" type="button" value="..." onclick="getDEPT_NO('search');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_WORK_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_WORK_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_USE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_USE_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:RadioButtonList ID="rbl_USE_STATUS" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2di_lb_IS_VALID_Y%>" Value="Y" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2di_lb_IS_VALID_N%>" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2di_lb_IS_VALID_ALL%>" Value="ALL"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DI0200Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Search%>" OnClientClick="BlockUI();" OnClick="WFB2DI0200Search_Click" />

                                            <%--<asp:Button ID="WFB2DI0200Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Search%>" OnClientClick="BlockUI();" OnClick="WFB2DI0200Search_Click" />--%>

                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_btn_clear%>" onclick="ClearAll();" />
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
                            <aces:Btn ID="WFB2DI0200Add" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Add%>" OnClick="WFB2DI0200Add_Click" />
                            <aces:Btn ID="WFB2DI0200Delete" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DI0200Delete_Click" />
                            <aces:Btn ID="WFB2DI0200Edit" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Edit%>" Visible="false" OnClick="WFB2DI0200Edit_Click" />
                            <aces:Btn ID="WFB2DI0200Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Save%>" Visible="false"  OnClick="WFB2DI0200Save_Click"  OnClientClick="return saveCheck()" />

<%--                            <asp:Button ID="WFB2DI0200Add" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Add%>" OnClick="WFB2DI0200Add_Click" />
                            <asp:Button ID="WFB2DI0200Delete" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DI0200Delete_Click" />
                            <asp:Button ID="WFB2DI0200Edit" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Edit%>" Visible="false" OnClick="WFB2DI0200Edit_Click" />
                            <asp:Button ID="WFB2DI0200Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0200Save%>" Visible="false" ValidationGroup="GroupB" OnClick="WFB2DI0200Save_Click" />
                            --%>
                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2di_btn_Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_Cancel_Click" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DI0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_WORK_CD" DefaultValue=""
                        Name="work_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="rbl_USE_STATUS" DefaultValue=""
                        Name="is_valid" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
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
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_DEPT_NO" runat="server" Width="180px" ClientIDMode="Static" CssClass="MandatoryField" BorderWidth="0"></asp:TextBox>
                                <input id="btn_NEW_DEPT_NO" type="button" value="..." onclick="OpenMultiSelect2('txt_NEW_DEPT_NO', 'TB_H_R_DEPT_DATA', 'DEPT_NAME', 'DEPT_NO', '');" />
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_DEPT_NO%>"
                                ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_CD" runat="server" Text='<%#Bind("WORK_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_WORK_CD" runat="server" Text='<%#Bind("WORK_CD")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_WORK_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_WORK_CD%>"
                                ControlToValidate="ddl_NEW_WORK_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_START_DT" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField date"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_START_DT%>"
                                ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidatorSTART_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_START_DATE2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_END_DT" runat="server" Text='<%#Bind("END_DT", "{0:yyyy/MM/dd}")%>' ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorEND_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_END_DATE2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_END_DT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorEND_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_END_DATE2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_START_DT"
                                ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_START_DT_RANGE2%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupB"></asp:CompareValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_REMARK" runat="server" Text='<%#Bind("REMARK")%>' ClientIDMode="Static" Width="100%" MaxLength="210"></asp:TextBox>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="210"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2di_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" Width="200px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2di_lb_WORK_CD%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2di_lb_START_DT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2di_lb_END_DT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REMARK%>" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_DEPT_NO" runat="server" Width="180px" ClientIDMode="Static" CssClass="MandatoryField" BorderWidth="0"></asp:TextBox>
                                    <input id="btn_NEW_DEPT_NO" type="button" value="..." onclick="OpenMultiSelect2('txt_NEW_DEPT_NO', 'TB_H_R_DEPT_DATA', 'DEPT_NAME', 'DEPT_NO', '');" />
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_DEPT_NO%>"
                                    ControlToValidate="txt_NEW_DEPT_NO" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_WORK_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_WORK_CD%>"
                                    ControlToValidate="ddl_NEW_WORK_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_NEW_START_DT" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField date"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_START_DT%>"
                                    ControlToValidate="txt_NEW_START_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidatorSTART_DT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_START_DATE2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_START_DT" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_NEW_END_DT" runat="server" ClientIDMode="Static" Width="100px" CssClass="date"></asp:TextBox>
                                </div>
                                <asp:CustomValidator ID="CustomValidatorEND_DT" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_END_DATE2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_END_DT" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_NEW_START_DT"
                                    ControlToValidate="txt_NEW_END_DT" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_START_DT_RANGE2%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupB"></asp:CompareValidator>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_REMARK" runat="server" ClientIDMode="Static" Width="100%" MaxLength="210"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>

                </EmptyDataTemplate>
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <asp:Button ID="hid_getDEPT_NAME" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getDEPT_NAME_Click" />

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

