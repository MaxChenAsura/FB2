<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0300_Emp.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0300_Emp" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".year").mask('9999');
            $("#txt_PJOB_CD").attr("readonly", true);

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

        function OpenMultiSelect2(targetObj, TableName, TextColumn, ValueColumn, WhereColumn) {
            var WhereValue = "";
            var SelectValue = "";
            if ($("#ddl_NEW_WS_CD").val() != "-1") {
                WhereColumn = " END_DT >= GETDATE() and WS_CD "
                WhereValue = $("#ddl_NEW_WS_CD").val();
            }
            else {
                WhereColumn = " END_DT >= GETDATE() and 1 "
                WhereValue = "1";
            }
            if ($("#txt_PJOB_CD").val() != "") {
                SelectValue = $("#txt_PJOB_CD").val();
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
            if (LookUpCheckboxs() > 0)
                return confirm("確定要刪除?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function openQry() {
            window.location.href("WFB2DI0300_Qry.aspx");
            return false;
        }
        function CheckAddAction() {
            if ($("#ddl_TARGET_TYPE").val() != "" && $("#txt_TARGET_YEAR").val() != "")
                return true;
            else {
                alert("請先輸入年度和管理類別!");
                return false;
            }
        }
        //清空畫面
        function ClearAll() {
            $("#ddl_TARGET_TYPE").val("");
            $("#txt_TARGET_YEAR").val("");
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
                                <col width="20%" />
                                <col width="10%" />
                                <col width="40%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TARGET_YEAR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TARGET_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_TARGET_YEAR" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_YEAR%>"
                                            ControlToValidate="txt_TARGET_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorTARGET_YEAR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="年度只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_TARGET_YEAR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                    </td>
                                    <%--<th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" ClientIDMode="Static" ></asp:TextBox>
                                    </td>--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TARGET_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TARGET_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_TARGET_TYPE" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TARGET_TYPE%>"
                                            ControlToValidate="ddl_TARGET_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue=""></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DI0301Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DI0301Search_Click" />

                                            <%--<asp:Button ID="WFB2DI0300Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0300Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DI0300Search_Click" />--%>

                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_btn_clear%>" onclick="ClearAll();" />
                                             <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2di_btn_back%>" OnClick="btn_back_Click" />
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
                            <aces:Btn ID="WFB2DI0301Add" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0301Add%>" OnClientClick="return CheckAddAction();" OnClick="WFB2DI0301Add_Click" />
                            <aces:Btn ID="WFB2DI0301Delete" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0301Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DI0301Delete_Click" />

                            <%--<asp:Button ID="WFB2DI0301Add" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0301Add%>" OnClick="WFB2DI0301Add_Click" />
                            <asp:Button ID="WFB2DI0301Delete" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0301Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DI0301Delete_Click" />
                            --%>
                           
                            <aces:Btn ID="WFB2DI0301Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0301Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DI0301Save_Click" />
                            <%--<asp:Button ID="WFB2DI0301Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0301Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DI0301Save_Click" />--%>

                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2di_btn_Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_Cancel_Click" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData2"
                SelectCountMethod="getCount2" TypeName="CFB2DI0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <%-- <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />--%>
                    <asp:ControlParameter ControlID="ddl_TARGET_TYPE"
                        Name="target_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_TARGET_YEAR"
                        Name="target_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
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
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_WS_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_WS_CD%>"
                                ControlToValidate="ddl_NEW_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_CD" runat="server" Text='<%#Bind("WORK_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_WORK_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_WORK_CD%>"
                                ControlToValidate="ddl_NEW_WORK_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_PJOB_CD%>" SortExpression="PJOB_CD" HeaderStyle-Width="400px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_PJOB_CD" runat="server" Width="500px" CssClass="MandatoryField" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                <input id="btn_PJOB_CD" type="button" value="..." onclick="OpenMultiSelect2('txt_PJOB_CD', 'TB_H_M_PJOB', 'PJOB_DESC', 'PJOB_CD', 'WS_CD');" />
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_PJOB_CD%>"
                                ControlToValidate="txt_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
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
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2di_lb_WS_CD%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2di_lb_PJOB_CD%>" Width="400px"></asp:Label>
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
                                    <asp:DropDownList ID="ddl_NEW_WS_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_WS_CD%>"
                                    ControlToValidate="ddl_NEW_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                             <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:DropDownList ID="ddl_NEW_WORK_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_WORK_CD%>"
                                ControlToValidate="ddl_NEW_WORK_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_PJOB_CD" runat="server" Width="500px" CssClass="MandatoryField" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    <input id="btn_PJOB_CD" type="button" value="..." onclick="OpenMultiSelect2('txt_PJOB_CD', 'TB_H_M_PJOB', 'PJOB_DESC', 'PJOB_CD', 'WS_CD');" />
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_PJOB_CD%>"
                                    ControlToValidate="txt_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
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
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

