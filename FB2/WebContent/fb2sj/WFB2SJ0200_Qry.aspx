<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0200_Qry.aspx.cs" Inherits="WebContent_WFB2SJ0200_Qry" Culture="auto" UICulture="auto" %>
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
            $//(".decimal").css("text-align", "right");


            //GridView必須
            gridviewScroll();
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
                }
            }
            return HaveCheck;
        }

        //查詢前檢核
        function CheckSearch() {
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

        //對象生成前檢查
        function executeCheck() {
            var processed = false;
            BlockUI();
            if (checkboxsSelected() == 1) {
                if (confirm('確定要進行對象生成?  [ !!!注意：已維護資料會被覆蓋]')) {
                    processed = true;
                }
                else {
                    processed = false;
                }
                if (!processed) {
                    $.unblockUI();
                    return processed;
                }
            } else {
                alert("請選取一筆資料!");
            }

            if (!processed) {
                $.unblockUI();
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
        //考核發佈
        function CheckAnnounce() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() == 1) {
                processed = confirm("確定要進行考核發佈？ [ !!!注意：考核發佈後資料無法進行修改]");
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


        //清空畫面
        function ClearAll() {
            $("#txt_ASSESS_YEAR_S").val("");
            $("#txt_ASSESS_YEAR_E").val("");
            $("#ddl_ASSESS_TYPE").val("-1");
        }



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
                    <col width="30%" />
                    <col width="10%" />
                    <col width="30%" />
                    <col width="15%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--考核年度--%>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_year%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_ASSESS_YEAR_S" runat="server" Width="60px" ClientIDMode="Static" CssClass="year"></asp:TextBox>
                            ~ 
                           <asp:TextBox ID="txt_ASSESS_YEAR_E" runat="server" Width="60px" ClientIDMode="Static" CssClass="year"></asp:TextBox>
                             <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_ASSESS_YEAR_S"
                                            ControlToValidate="txt_ASSESS_YEAR_E" ErrorMessage="<%$Resources:Resource,wfb2sj_error_assess_year_s_e%>" Type="Integer" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            <!--驗證長度需為4 -->	
								<asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_s%>" ControlToValidate="txt_ASSESS_YEAR_S" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_e%>" ControlToValidate="txt_ASSESS_YEAR_E" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </td>
                         <th align="left" class="Body_TableHeader">
                            <%--考核類別--%>
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static"> </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <aces:Btn ID="WFB2SJ0200Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SJ0200Search_Click"   OnClientClick="CheckSearch();"/>
                            <%--<asp:Button ID="WFB2SJ0200Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SJ0200Search_Click"   OnClientClick="CheckSearch();"/>--%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sj_btn_clear%>" onclick="ClearAll();" />
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
                                <aces:Btn ID="WFB2SJ0200Add" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_add%>" Visible="true" OnClick="WFB2SJ0200Add_Click" OnClientClick="return CheckSearch();" />
                                <aces:Btn ID="WFB2SJ0200Delete" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_delete%>" Visible="false" OnClick="WFB2SJ0200Delete_Click" OnClientClick="return doDelete();" />

                                 <aces:Btn ID="WFB2SJ0200Execute" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_target_gen%>" Visible="false" OnClick="WFB2SJ0200Execute_Click" OnClientClick="return executeCheck(); " />
                                 <aces:Btn ID="WFB2SJ0200Release" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_release%>" Visible="false" OnClick="WFB2SJ0200Release_Click" OnClientClick="return CheckRelease();" />
                                 <aces:Btn ID="WFB2SJ0200Announce" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_announce%>" Visible="false" OnClick="WFB2SJ0200Announce_Click" OnClientClick="return CheckAnnounce();" />


                                <aces:Btn ID="WFB2SJ0200OK" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_ok%>" Visible="false"  OnClick="WFB2SJ0200OK_Click" OnClientClick="return saveCheck()"/>
                                 <%--<asp:Button ID="WFB2SJ0200Add" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_add%>" Visible="true" OnClick="WFB2SJ0200Add_Click" OnClientClick="return CheckSearch();" />
                                <asp:Button ID="WFB2SJ0200Delete" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_delete%>" Visible="false" OnClick="WFB2SJ0200Delete_Click" OnClientClick="return doDelete();" />

                                 <asp:Button ID="WFB2SJ0200Execute" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_target_gen%>" Visible="false" OnClick="WFB2SJ0200Execute_Click" OnClientClick="return executeCheck(); " />
                                 <asp:Button ID="WFB2SJ0200Release" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_release%>" Visible="false" OnClick="WFB2SJ0200Release_Click" OnClientClick="return CheckRelease();" />
                                 <asp:Button ID="WFB2SJ0200Announce" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_announce%>" Visible="false" OnClick="WFB2SJ0200Announce_Click" OnClientClick="return CheckAnnounce();" />


                                <asp:Button ID="WFB2SJ0200OK" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_ok%>" Visible="false"  OnClick="WFB2SJ0200OK_Click" OnClientClick="return saveCheck()"/>--%>
                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData" 
                SelectCountMethod="getCount" TypeName="CFB2SJ0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_ASSESS_YEAR_S"
                        Name="assess_year_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_ASSESS_YEAR_E"
                        Name="assess_year_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_ASSESS_TYPE"
                        Name="assess_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                             <!-- 凍結註記 -->
                            <asp:HiddenField ID="hid_FREEZE_FLAG" runat="server" Value='<%#Bind("FREEZE_FLAG")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--考核年度--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_assess_year%>" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="ASSESS_YEAR">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text='<%#Bind("ASSESS_YEAR")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                             <asp:TextBox ID="txt_NEW_ASSESS_YEAR" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField  year"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="NEW_ASSESS_YEAR" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sj_required_assess_year%>"
                                    ControlToValidate="txt_NEW_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="NEW_ASSESS_YEAR2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year%>" ControlToValidate="txt_NEW_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--考核類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_assess_type%>" HeaderStyle-Width="100px"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="ASSESS_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text='<%#Bind("ASSESS_TYPE_DESC")%>' Width="100px"></asp:Label>
                              <!-- 考核類別 -->
                            <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" Value='<%#Bind("ASSESS_TYPE")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"  ></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--MAIL 檢查日--%>
                    <asp:TemplateField HeaderText="MAIL 檢查日" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" SortExpression="MAIL_CHKDT">
                        <ItemTemplate>
                            <asp:Label ID="lb_MAIL_CHKDT" runat="server" Text='<%#Bind("MAIL_CHKDT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <asp:Button ID="btn_SendMail"  Text="發送" runat="server" ClientIDMode="Static"  OnClick="btn_SendMail_Click" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' />
                            <asp:HiddenField ID="hid_MAIL_DEP20_SEND_FLAG" runat="server" Value='<%#Bind("MAIL_DEP20_SEND_FLAG")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_MAIL_CHKDT" runat="server" MaxLength="8" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="MAIL_CHKDT" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_JOIN_DT%>"
                                        ControlToValidate="txt_MAIL_CHKDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="MAIL 檢查日期輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_MAIL_CHKDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--提出期限--%>
                    <asp:TemplateField HeaderText="提出期限" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label ID="lb_DEADLINE" runat="server" Text='<%#Bind("DEADLINE", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_DEADLINE" runat="server" MaxLength="8" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="req_DEADLINE" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_JOIN_DT%>"
                                        ControlToValidate="txt_DEADLINE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cusV_DEADLINE" runat="server" ValidateEmptyText="true"
                                ErrorMessage="提出期限輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_DEADLINE" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--對象生成日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_target_gen_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="TARGET_GEN_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_GEN_DT" runat="server" Text='<%#Bind("TARGET_GEN_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                              <asp:HiddenField ID="hid_TARGET_GEN_DT" runat="server" Value='<%#Bind("TARGET_GEN_DT","{0:yyyy/MM/dd}")%>' ClientIDMode="Static" />
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--提出核可日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_release_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="RELEASE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <!-- 核可狀態 -->
                            <asp:HiddenField ID="hid_APPROVE_STATUS" runat="server" Value='<%#Bind("APPROVE_STATUS")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--核可日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_approve_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="APPROVE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--考核發佈日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_assess_release_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="ASSESS_RELEASE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_RELEASE_DT" runat="server" Text='<%#Bind("ASSESS_RELEASE_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_function%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2SJ0200Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sj_btn_detailandmaintain%>" />
                            <%--
                            <asp:Button ID="WFB2SJ0200Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sj_btn_detailandmaintain%>" />
                                --%>
                        </ItemTemplate>
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
                                 <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_year%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                  <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_type%>" Width="100px"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                            <td>
                                 <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_target_gen_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_release_dt%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_approve_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_release_dt%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                 <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_function%>" Width="100px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>  </td>
                            <td>  </td>
                             <td>  
                                 <asp:TextBox ID="txt_NEW_ASSESS_YEAR" runat="server" ClientIDMode="Static" Width="60px" CssClass="MandatoryField  year"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="NEW_ASSESS_YEAR" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sj_required_assess_year%>"
                                    ControlToValidate="txt_NEW_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="NEW_ASSESS_YEAR2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year%>" ControlToValidate="txt_NEW_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                             </td>
                             <td>  
                                  <asp:DropDownList ID="ddl_NEW_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"   ></asp:DropDownList>
                             </td>
                            <td>  
                            </td>
                            <td>  
                            </td>
                             <td>  </td>
                            <td>  </td>
                             <td>  </td>
                            <td>  </td>
                             <td>  </td>

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

			<!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
