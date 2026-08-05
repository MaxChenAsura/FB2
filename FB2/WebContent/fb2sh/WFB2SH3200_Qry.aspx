<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sh/WFB2SH3200_Qry.aspx.cs" Inherits="WebContent_WFB2SH3200_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {
            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".year").mask('9999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            //GridView必須
            //gridviewScroll();
            $.unblockUI();
        }


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }

        //凍結視窗用
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 0

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

        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
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
                    if (choietr == null) {
                        choietr = i;
                    }
                }
            }
            return HaveCheck;
        }

        //查詢前檢核
        function CheckSearch() {
            //其它需要檢核的
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
            }
            if (!processed)
                $.unblockUI();

            return processed;

        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
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

        

        //提出核可
        function CheckRelease() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() == 1) {
                processed = confirm("確定要提出核可？");
            }
            else {
                alert("請選取一筆資料!");
                choietr = null;
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        

        //回傳目前Checkbox被勾選的數量
        //function checkboxsSelected() {
        //    choietr = null;
        //    var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
        //    var HaveCheck = 0;
        //    for (var i = 0; i < ItemCheckBoxs.length; i++) {
        //        if (ItemCheckBoxs[i].checked) {
        //            HaveCheck++;
        //        }
        //    }
        //    return HaveCheck;
        //}

        //清空畫面
        function ClearAll() {
            $("#txt_AWARD_YEAR_S").val("");
            $("#txt_AWARD_YEAR_E").val("");
            $("#txt_AWARD_DT_S").val("");
            $("#txt_AWARD_DT_E").val("");
        }

        //function SelectNonDisabledCheckboxes(spanChk) {
        //    if (spanChk.checked == false) {
        //        $(':checkbox[id$=cb_check]').prop('checked', false);
        //        return;
        //    }
        //    $(":checkbox[id$=cb_check]").each(function (index, ele) {
        //        if ($(this).prop("disabled") == false) {
        //            console.log("tt");
        //            $(this).prop('checked', true);
        //        }

        //    });
        //}


    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--年度--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_year%>"></asp:Label>:</th>
                        <td align="left" class="Body_label"colspan="4">
                            <asp:TextBox ID="txt_AWARD_YEAR_S" runat="server" Width="100px" ClientIDMode="Static" CssClass="year"> </asp:TextBox>
                            ~
                            <asp:TextBox ID="txt_AWARD_YEAR_E" runat="server" Width="100px" ClientIDMode="Static" CssClass="year"> </asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_AWARD_YEAR_S"
                                ControlToValidate="txt_AWARD_YEAR_E" ErrorMessage="<%$Resources:Resource,wfb2sh_error_award_year_s_e%>" Type="Integer" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            <!--驗證長度需為4 -->
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator_year_s" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_year_s%>" ControlToValidate="txt_AWARD_YEAR_S" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <!--驗證長度需為4 -->
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator_year_e" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_year_e%>" ControlToValidate="txt_AWARD_YEAR_E" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </td>
                       
                    </tr>
                   
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            
                            <aces:Btn ID="WFB2SH3200Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2SH3200Search_Click" OnClientClick="CheckSearch();" />
                               <%--
                            <asp:Button ID="WFB2SH3200Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2SH3200Search_Click" OnClientClick="CheckSearch();" />
                            --%> 
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="6">
                            <div id="init_grid">
                                 
                                <aces:Btn ID="WFB2SH3200Add" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_add%>" Visible="true" OnClick="WFB2SH3200Add_Click" OnClientClick="return CheckSearch();" />
                                <aces:Btn ID="WFB2SH3200Delete" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_delete%>" Visible="false" OnClick="WFB2SH3200Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2SH3200Edit" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_edit%>" Visible="false" OnClick="WFB2SH3200Edit_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SH3200Release" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_release%>" Visible="false" OnClick="WFB2SH3200Release_Click" OnClientClick="return CheckRelease();" />
                                 <aces:Btn ID="WFB2SH3200OK" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_ok%>" Visible="false" OnClick="WFB2SH3200OK_Click" OnClientClick="return saveCheck()" />
                                  <%-- 
                                <asp:Button ID="WFB2SH3200Add" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_add%>" Visible="true" OnClick="WFB2SH3200Add_Click" OnClientClick="return CheckSearch();" />
                                <asp:Button ID="WFB2SH3200Delete" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_delete%>" Visible="false" OnClick="WFB2SH3200Delete_Click" OnClientClick="return doDelete();" />
                                <asp:Button ID="WFB2SH3200Edit" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_edit%>" Visible="false" OnClick="WFB2SH3200Edit_Click" OnClientClick="BlockUI();" />
                                <asp:Button ID="WFB2SH3200Execute" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_target_gen%>" Visible="false" OnClick="WFB2SH3200Execute_Click" OnClientClick="return executeCheck(); " />
                                <asp:Button ID="WFB2SH3200Release" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_release%>" Visible="false" OnClick="WFB2SH3200Release_Click" OnClientClick="return CheckRelease();" />
                                <asp:Button ID="WFB2SH3200Announce" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_salary_trans%>" Visible="false" OnClick="WFB2SH3200Announce_Click" OnClientClick="return CheckAnnounce();" />
                                <asp:Button ID="WFB2SH3200OK" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_ok%>" Visible="false" OnClick="WFB2SH3200OK_Click" OnClientClick="return saveCheck()" />
                                --%>

                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SH3200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_qry_AWARD_YEAR_S"
                        Name="award_year_s" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_AWARD_YEAR_E"
                        Name="award_year_e" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                   
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--年度--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_award_year%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="AWARD_YEAR">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_YEAR" runat="server" Text='<%#Bind("AWARD_YEAR")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_AWARD_YEAR" runat="server" Text='<%#Bind("AWARD_YEAR")%>' Width="40px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AWARD_YEAR" runat="server" ClientIDMode="Static" Width="40px" CssClass="MandatoryField  year"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NEW_AWARD_YEAR" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_year%>"
                                ControlToValidate="txt_NEW_AWARD_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證長度需為4 -->
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator__NEW_AWARD_YEAR" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_year%>" ControlToValidate="txt_NEW_AWARD_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                    <%--發放期間起--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_award_stime%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="AWARD_STIME">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_STIME" runat="server" Text='<%#Bind("AWARD_STIME","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_AWARD_STIME" runat="server" Text='<%#Bind("AWARD_STIME","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AWARD_STIME" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField  date"></asp:TextBox>
                            <!--驗證不可空白 -->
                            <asp:RequiredFieldValidator ID="NEW_AWARD_STIM1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_stime%>"
                                ControlToValidate="txt_NEW_AWARD_STIME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證日期格式-->
                           <asp:CustomValidator ID="NEW_AWARD_STIM2" runat="server" ValidateEmptyText="true"
		                    ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                    ControlToValidate="txt_NEW_AWARD_STIME" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>	
                            <!--起日不可大於迄日 -->	
								<asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_NEW_AWARD_STIME"
                                ControlToValidate="txt_NEW_AWARD_ETIME" ErrorMessage="<%$Resources:Resource,wfb2sh_error_award_stime_e%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>

                            </EditItemTemplate>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--發放期間迄--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_award_etime%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="AWARD_ETIME">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_ETIME" runat="server" Text='<%#Bind("AWARD_ETIME","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_AWARD_ETIME" runat="server" Text='<%#Bind("AWARD_ETIME","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            </ItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AWARD_ETIME" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField  date"></asp:TextBox>
                            <!--驗證不可空白 -->
                            <asp:RequiredFieldValidator ID="NEW_AWARD_ETIME1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_etime%>"
                                ControlToValidate="txt_NEW_AWARD_ETIME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證日期格式-->
                            <asp:CustomValidator ID="NEW_AWARD_ETIME2" runat="server" ValidateEmptyText="true"
		                    ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_etime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                    ControlToValidate="txt_NEW_AWARD_ETIME" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--年獎發放日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_award_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="AWARD_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_DT" runat="server" Text='<%#Bind("AWARD_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_AWARD_DT" runat="server" Text='<%#Bind("AWARD_DT","{0:yyyy/MM/dd}")%>' ClientIDMode="Static" Width="100px" CssClass="MandatoryField  date"></asp:TextBox>
                            <!--驗證不可空白 -->
                            <asp:RequiredFieldValidator ID="EDIT_AWARD_DT1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_dt%>"
                                ControlToValidate="txt_EDIT_AWARD_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證日期格式-->
                            <asp:CustomValidator ID="EDIT_AWARD_DT2" runat="server" ValidateEmptyText="true"
		                    ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                    ControlToValidate="txt_EDIT_AWARD_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AWARD_DT" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField  date"></asp:TextBox>
                            <!--驗證不可空白 -->
                            <asp:RequiredFieldValidator ID="NEW_AWARD_D1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_dt%>"
                                ControlToValidate="txt_NEW_AWARD_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證日期格式-->
                             <asp:CustomValidator ID="NEW_AWARD_D2" runat="server" ValidateEmptyText="true"
		                    ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                    ControlToValidate="txt_NEW_AWARD_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--年獎計算日--%>
                    <asp:TemplateField HeaderText="年獎計算日" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="TARGET_GEN_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_GEN_DT" runat="server" Text='<%#Bind("TARGET_GEN_DT","{0:yyyy/MM/dd}")%>' Width="100px" ClientIDMode="Static"></asp:Label>
                            <asp:HiddenField ID="hid_TARGET_GEN_DT" runat="server" Value='<%#Bind("TARGET_GEN_DT","{0:yyyy/MM/dd}")%>' ClientIDMode="Static" />
                            <!-- 凍結註記 -->
                            <asp:HiddenField ID="hid_FREEZE_FLAG" runat="server" Value='<%#Bind("FREEZE_FLAG")%> ' ClientIDMode="Static" />
                          
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_GEN_DT" runat="server" Text='<%#Bind("TARGET_GEN_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--提出核可日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_release_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="RELEASE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <!-- 核可狀態 -->
                            <asp:HiddenField ID="hid_APPROVE_STATUS" runat="server" Value='<%#Bind("APPROVE_STATUS")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--核可日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_approve_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                   
                    <%--功能--%>
                    <asp:TemplateField HeaderText="功能" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            
                            <aces:Btn ID="WFB2SH3200Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="查詢明細" />
                            <%--
                            <asp:Button ID="WFB2SH3200Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="查詢明細" />
                            --%> 
                                
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <%--當查詢條件,無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Labelrow" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_year%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_stime%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_etime%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="年獎計算日" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_release_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_approve_dt%>" Width="100px"></asp:Label>
                            </td>
                           
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_function%>" Width="100px"></asp:Label>
                            </td>
                            </td>

                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AWARD_YEAR" runat="server" ClientIDMode="Static" Width="40px" CssClass="MandatoryField  year"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NEW_AWARD_YEAR" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_year%>"
                                    ControlToValidate="txt_NEW_AWARD_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AWARD_STIME" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField  date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <asp:RequiredFieldValidator ID="NEW_AWARD_STIM1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_stime%>"
                                    ControlToValidate="txt_NEW_AWARD_STIME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="NEW_AWARD_STIM2" runat="server" ValidateEmptyText="true"
		                        ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                        ControlToValidate="txt_NEW_AWARD_STIME" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>


                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AWARD_ETIME" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField  date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <asp:RequiredFieldValidator ID="NEW_AWARD_ETIME1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_etime%>"
                                    ControlToValidate="txt_NEW_AWARD_ETIME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="NEW_AWARD_ETIME2" runat="server" ValidateEmptyText="true"
		                        ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_etime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                        ControlToValidate="txt_NEW_AWARD_ETIME" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AWARD_DT" runat="server" ClientIDMode="Static" Width="100px" CssClass="MandatoryField  date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <asp:RequiredFieldValidator ID="NEW_AWARD_D1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sh_required_award_dt%>"
                                    ControlToValidate="txt_NEW_AWARD_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="NEW_AWARD_D2" runat="server" ValidateEmptyText="true"
		                        ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
		                        ControlToValidate="txt_NEW_AWARD_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                    </table>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />

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
            <!-- 查詢條件 -->
             <asp:HiddenField ID="hid_qry_AWARD_YEAR_S" runat="server" ClientIDMode="Static" /> 
            <asp:HiddenField ID="hid_qry_AWARD_YEAR_E" runat="server" ClientIDMode="Static" /> 

          
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
