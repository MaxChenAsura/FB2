<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA2200_Qry.aspx.cs" Inherits="WebContent_WFB2SA_WFB2SA2200_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            
            iniForm();
        });

        function iniForm() {
            $(".empid").mask('99999')
            gridviewScroll();
            $.unblockUI();

            $('#txt_EMP_NAME').attr("readonly", true);
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
                freezesize: 1

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_EMP_CD").val("-1");
            $("#ddl_COMPANY_CD").val("-1");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
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
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
                                <col width="30%" />
                                <col width="12%" />
                                <col width="46%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_COMPANY_CD%>"></asp:Label>:</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_COMPANY_CD" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">

                                        <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static"  MaxLength="5" CssClass="empid"> </asp:TextBox>
                                         <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="113px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th align="right">
                                        <aces:Btn ID="WFB2SA2200Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Search%>" OnClick="WFB2SA2200Search_Click" OnClientClick="BlockUI();" CausesValidation="true" />
                                        <%--<asp:Button ID="WFB2SA2200Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Search%>" OnClick="WFB2SA2200Search_Click" OnClientClick="BlockUI();" CausesValidation="true" />--%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Clear%>" OnClientClick="ClearAll();" CausesValidation="false" />
                                    </th>
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
                            <aces:Btn ID="WFB2SA2200Approve" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2200Approve%>" OnClick="WFB2SA2200Approve_Click" Visible="false" OnClientClick="return CheckApproveAction();" />
                            <aces:Btn ID="WFB2SA2200Reject" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2200Reject%>" OnClick="WFB2SA2200Reject_Click" Visible="false"  OnClientClick="return CheckRejectAction();" />
                            <%--<asp:Button ID="WFB2SA2200Approve" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2200Approve%>" OnClick="WFB2SA2200Approve_Click" Visible="false" OnClientClick="return CheckApproveAction();" />
                            <asp:Button ID="WFB2SA2200Reject" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2200Reject%>" OnClick="WFB2SA2200Reject_Click" Visible="false"  OnClientClick="return CheckRejectAction();" />--%>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SA2200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_COMPANY_CD" DefaultValue=""
                        Name="company_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_EMP_CD" DefaultValue=""
                        Name="emp_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_USER_ID" DefaultValue=""
                        Name="user_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="2030px" OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sa_RowNumber%>" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="COMPANY_SNAME" HeaderText="<%$Resources:Resource,wfb2sa_lb_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2sa_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2sa_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_CD_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="JOIN_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_JOIN_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="JOIN_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_EMP_STATUS%>" SortExpression="EMP_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SALARY_NAME" HeaderText="<%$Resources:Resource,wfb2sa_lb_SALARY_ID%>" SortExpression="SALARY_ID" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="CHG_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="CHG_AMT_B" HeaderText="<%$Resources:Resource,wfb2sa_grd_CHG_AMT_B%>" SortExpression="CHG_AMT_B" DataFormatString="{0:N0}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="CHG_AMT_A" HeaderText="<%$Resources:Resource,wfb2sa_grd_AMOUNT%>" SortExpression="CHG_AMT_A" DataFormatString="{0:N0}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="EFFECT_SDT_B" HeaderText="<%$Resources:Resource,wfb2sa_lb_EFFECT_SDT_B%>" SortExpression="EFFECT_SDT_B" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EFFECT_EDT_B" HeaderText="<%$Resources:Resource,wfb2sa_lb_EFFECT_EDT_B%>" SortExpression="EFFECT_EDT_B" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EFFECT_SDT_A" HeaderText="<%$Resources:Resource,wfb2sa_lb_START_DT%>" SortExpression="EFFECT_SDT_A" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EFFECT_EDT_A" HeaderText="<%$Resources:Resource,wfb2sa_lb_END_DT%>" SortExpression="EFFECT_EDT_A" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PROCESS_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="REMARK" HeaderText="<%$Resources:Resource,wfb2sa_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderStyle-Width="200px" HeaderText="<%$Resources:Resource,wfb2sa_lb_APP_REMARK%>">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txt_APP_REMARK" MaxLength="100" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            <asp:HiddenField ID="hid_SEQ_NO" runat="server" Value='<%#Bind("SEQ_NO")%>' />
                            <asp:HiddenField ID="hid_SEQ_NO_B" runat="server" Value='<%#Bind("SEQ_NO_B")%>' />
                            <asp:HiddenField ID="hid_SALARY_ID" runat="server" Value='<%#Bind("SALARY_ID")%>' />
                            <asp:HiddenField ID="hid_CHG_STATUS" runat="server" Value='<%#Bind("CHG_STATUS")%>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_USER_ID" runat="server" ClientIDMode="Static" />
             <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Reject_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Reject_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Approve_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Approve_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_action_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
