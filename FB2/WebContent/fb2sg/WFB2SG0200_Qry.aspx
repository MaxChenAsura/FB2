<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sg/WFB2SG0200_Qry.aspx.cs" Inherits="WebContent_WFB2SG0200_Qry" Culture="auto" UICulture="auto" %>
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
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");


            reComma("ALLOWANCE_VALUE", 0);
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
            if (checkboxsSelected() > 0) {
                return confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
                return false;
            }
        }

        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            choietr = null;
            var ItemCheckBoxs = $("[type=checkbox]");
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

        //對象生成執行前檢查(無凍結視窗才能使用)
        function executeCheck() {

            var processed = true;
            if (checkboxsSelected() == 1) {
                choietr = choietr - 2;
                var cfType = $("[id=hid_FESTIVAL_TYPE]")[choietr].value;
                //if ( cfType== "3"||cfType== "4"||cfType== "5") {
                //    alert("此節金類別無法進行對象生成");
                //    return false;
                //} 


                if ($("[id=hid_TARGET_GEN_DT]")[choietr].value == "") {
                    if (confirm('確定要進行對象生成?')) {
                        BlockUI();
                    }
                    else {
                        processed = false;
                    }
                } else {
                    if (confirm('確定要進行對象生成?  [ !!!注意：己維護資料會被覆蓋]')) {
                        BlockUI();
                    }
                    else {
                        processed = false;
                    }
                }
            }

            if (!processed)
                $.unblockUI();

            return processed;
        }
        function uploadCheck() {

            var processed = true;
            if (checkboxsSelected() == 1) {
                choietr = choietr - 2;
                var cfType = $("[id=hid_FESTIVAL_TYPE]")[choietr].value;
                if (cfType == "1" || cfType == "2") {
                    alert("此節金類別無法進行對象上傳");
                    return false;
                }

            }

            if (!processed)
                $.unblockUI();

            return processed;
        }


        //清空畫面
        function ClearAll() {
            $("#ddl_FESTIVAL_TYPE").val("-1");
            $("#ddl_EMP_CD").val("-1");
            $("#txt_FESTIVAL_PAY_DT").val("");
            $("#txt_FESTIVAL_DT").val("");

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
                    <col width="20%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--節金類別--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_FESTIVAL_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--員工區分--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_prid_cd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--節日日期--%>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_dt%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_FESTIVAL_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <!--驗證日期格式-->
                            <asp:CustomValidator ID="qry_FESTIVAL_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_FESTIVAL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--節金發放日期--%>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_pay_dt%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_FESTIVAL_PAY_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <!--驗證日期格式-->
                            <asp:CustomValidator ID="qry_FESTIVAL_PAY_DT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_pay_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_FESTIVAL_PAY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>


                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <aces:Btn ID="WFB2SG0200Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SG0200Search_Click" OnClientClick="return CheckSearch();" />
                            <%--
                                <asp:Button ID="WFB2SG0200Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SG0200Search_Click" OnClientClick="return CheckSearch();" />
                                --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sg_btn_clear%>" onclick="ClearAll();" />
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
                                <aces:Btn ID="WFB2SG0200Add" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_add%>" Visible="true" OnClick="WFB2SG0200Add_Click" OnClientClick="return saveCheck();" />
                                <aces:Btn ID="WFB2SG0200Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" Visible="false" OnClick="WFB2SG0200Delete_Click" OnClientClick="return doDelete();" />
                                <aces:Btn ID="WFB2SG0200Edit" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_edit%>" Visible="false" OnClick="WFB2SG0200Edit_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SG0200Execute" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_execute%>" Visible="false" OnClick="WFB2SG0200Execute_Click" OnClientClick="return executeCheck(); " />
                                <aces:Btn ID="WFB2SG0200Upload" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_upload%>" Visible="false" OnClick="WFB2SG0200Upload_Click" OnClientClick="return uploadCheck(); " />
                                <aces:Btn ID="WFB2SG0200OK" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_ok%>" Visible="false" OnClick="WFB2SG0200OK_Click" OnClientClick="return saveCheck();" />

                                <%--<asp:Button ID="WFB2SG0200Add" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_add%>" Visible="true" OnClick="WFB2SG0200Add_Click" OnClientClick="return saveCheck();" />
                                <asp:Button ID="WFB2SG0200Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" Visible="false" OnClick="WFB2SG0200Delete_Click" OnClientClick="return doDelete();" />
                                <asp:Button ID="WFB2SG0200Edit" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_edit%>" Visible="false" OnClick="WFB2SG0200Edit_Click" OnClientClick="BlockUI();" />
                                <asp:Button ID="WFB2SG0200Execute" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_execute%>" Visible="false" OnClick="WFB2SG0200Execute_Click" OnClientClick="return executeCheck(); " />
                                <asp:Button ID="WFB2SG0200Upload" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_upload%>" Visible="false" OnClick="WFB2SG0200Upload_Click" OnClientClick="return uploadCheck(); " />
                                <asp:Button ID="WFB2SG0200OK" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_ok%>" Visible="false" OnClick="WFB2SG0200OK_Click" OnClientClick="return saveCheck();" />--%>
                                <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SG0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_qry_FESTIVAL_TYPE"
                        Name="festival_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_EMP_CD"
                        Name="emp_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_FESTIVAL_DT"
                        Name="festival_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_FESTIVAL_PAY_DT"
                        Name="festivalPayDT" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <%-- 
					//txt
					 <asp:ControlParameter ControlID="txt_APPLY_DT_S"
                        Name="appleDT_S" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
					//下拉式                   
					<asp:ControlParameter ControlID="ddl_ENV_ALLOWANCE_TYPE"
                        Name="env_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
					//radio
                    <asp:ControlParameter ControlID="rbl_USE_STATUS" DefaultValue=""
                        Name="is_valid" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    --%>
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_rownumber%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_type%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_TYPE" runat="server" Text='<%#Bind("FESTIVAL_TYPE_DESC")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_FESTIVAL_TYPE" runat="server" Value='<%#Bind("FESTIVAL_TYPE")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_TYPE" runat="server" Text='<%#Bind("FESTIVAL_TYPE_DESC")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_FESTIVAL_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--員工區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_prid_cd%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_CD" runat="server" Text='<%#Bind("EMP_CD_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EMP_CD" runat="server" Text='<%#Bind("EMP_CD_DESC")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_EMP_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節日日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_DT" runat="server" Text='<%#Bind("FESTIVAL_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_DT" runat="server" Text='<%#Bind("FESTIVAL_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_FESTIVAL_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <asp:RequiredFieldValidator ID="FESTIVAL_DT1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_dt%>"
                                    ControlToValidate="txt_NEW_FESTIVAL_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="FESTIVAL_DT2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_FESTIVAL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>


                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金發放日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_pay_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_PAY_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_PAY_DT" runat="server" Text='<%#Bind("FESTIVAL_PAY_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_FESTIVAL_PAY_DT" runat="server" Text='<%#Bind("FESTIVAL_PAY_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_FESTIVAL_PAY_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                <!--驗證不可空白 -->
                                <asp:RequiredFieldValidator ID="FESTIVAL_PAY_DT1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_pay_dt%>"
                                    ControlToValidate="txt_NEW_FESTIVAL_PAY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <!--驗證日期格式-->
                                <asp:CustomValidator ID="FESTIVAL_PAY_DT2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_pay_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_FESTIVAL_PAY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>


                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--節金說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_desc%>" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" SortExpression="FESTIVAL_DESC">
                        <ItemTemplate>
                            <asp:Label ID="FESTIVAL_DESC" runat="server" Text='<%#Bind("FESTIVAL_DESC")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_FESTIVAL_DESC" runat="server" Text='<%#Bind("FESTIVAL_DESC")%>' Width="160px" MaxLength="60"></asp:TextBox>
                            </ItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_FESTIVAL_DESC" runat="server" Width="160px" MaxLength="60"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--對象生成日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_target_gen_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="TARGET_GEN_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_TARGET_GEN_DT" runat="server" Text='<%#Bind("TARGET_GEN_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                            <asp:HiddenField ID="hid_TARGET_GEN_DT" runat="server" Value='<%#Bind("TARGET_GEN_DT", "{0:yyyy/MM/dd}")%> ' ClientIDMode="Static" />
                            <asp:HiddenField ID="hid_FREEZE_FLAG" runat="server" Value='<%#Bind("FREEZE_FLAG")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_TARGET_GEN_DT" runat="server" Text='<%#Bind("TARGET_GEN_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_function%>" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2SG0200Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sg_btn_detail%>" />

                            <%--<asp:Button ID="WFB2SG0200Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sg_btn_detail%>" />--%>
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--當DB無資料時，就會使用此table --%>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_rownumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_prid_cd%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_pay_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_desc%>" Width="160px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_target_gen_dt%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_function%>" Width="40px"></asp:Label>
                            </td>

                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_FESTIVAL_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_EMP_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_FESTIVAL_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                    <!--驗證不可空白 -->
                                    <asp:RequiredFieldValidator ID="FESTIVAL_DT3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_dt%>"
                                        ControlToValidate="txt_NEW_FESTIVAL_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <!--驗證日期格式-->
                                    <asp:CustomValidator ID="FESTIVAL_DT4" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_NEW_FESTIVAL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                </div>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_NEW_FESTIVAL_PAY_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                    <!--驗證不可空白 -->
                                    <asp:RequiredFieldValidator ID="FESTIVAL_PAY_DT3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_festival_pay_dt%>"
                                        ControlToValidate="txt_NEW_FESTIVAL_PAY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <!--驗證日期格式-->
                                    <asp:CustomValidator ID="FESTIVAL_PAY_DT4" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sg_format_festival_pay_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_NEW_FESTIVAL_PAY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>


                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_FESTIVAL_DESC" runat="server" Width="160px" MaxLength="60"></asp:TextBox></td>
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
            <asp:HiddenField ID="hid_qry_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_EMP_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_FESTIVAL_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_FESTIVAL_PAY_DT" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
